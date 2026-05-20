using shared.Documents;
using shared.Serialization;
using shared.Blocks.Base;

namespace shared.Services
{
    public class VibDocumentService : VibFileService<VibDocument>
    {
        public VibDocumentService(IVibSerializer<VibDocument> serializer) : base(serializer) { }

        protected VibDocumentService() : base(null!) { }
        public override void Close(VibDocument document)
        {

        }

        public override VibDocument Open(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            var content = File.ReadAllText(filePath);
            var document = Serializer.Deserialize(content);
            document.FilePath = filePath;
            return document;
        }

    public override void Save(VibDocument document)
        {
            if (document == null) 
                throw new ArgumentNullException(nameof(document));

            if (string.IsNullOrWhiteSpace(document.FilePath))
                throw new ArgumentException("FilePath cannot be null or empty.", nameof(document.FilePath));

            try
            {
                var content = Serializer.Serialize(document);
                File.WriteAllText(document.FilePath, content);
            }
            catch (Exception ex) when (ex is System.Text.Json.JsonException || ex.GetType().Name.Contains("Serialization"))
            {
                throw new IOException("An unexpected error occurred during save.", ex);
            }
            catch (IOException ex)
            {
                throw new IOException($"An unexpected error during saving the document to '{document.FilePath}'.", ex);
            }
        }

        public void AddBlock(VibDocument document, VibBlock block)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            if (block == null) throw new ArgumentNullException(nameof(block));

            if (document.Blocks == null)
            {
                throw new InvalidOperationException("The document was not properly initialized.");
            }

            if (document.Blocks.Any(b => b.Identifier == block.Identifier))
            {
                throw new InvalidOperationException("Block already exists in the document.");
            }

            document.Blocks.Add(block);
        }

        public void RemoveBlock(VibDocument document, VibBlock block)
        {
            document.Blocks.Remove(block);
        }

        public void AddConnection(VibDocument document, VibConnection connection)
        {
            document.Connections.Add(connection);
        }

        public void RemoveConnection(VibDocument document, VibConnection connection)
        {
            document.Connections.Remove(connection);
        }

        public VibBlock GetBlock(VibDocument document, Guid blockId)
        {
            var block = document.Blocks.FirstOrDefault(b => b.Identifier == blockId);
            if (block == null)
                throw new KeyNotFoundException();
            return block;
        }

    }
}
