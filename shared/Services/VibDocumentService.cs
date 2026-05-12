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
            var content = Serializer.Serialize(document);
            File.WriteAllText(document.FilePath, content);
        }

        public void AddBlock(VibDocument document, VibBlock block)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            if (block == null) throw new ArgumentNullException(nameof(block));

            if (document.Blocks == null) document.Blocks = new List<VibBlock>();

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
