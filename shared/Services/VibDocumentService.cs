using shared.Documents;
using shared.Serialization;
using shared.Blocks.Base;

namespace shared.Services
{
    public class VibDocumentService : VibFileService<VibDocument>
    {
        public VibDocumentService(IVibSerializer<VibDocument> serializer) : base(serializer) { }
        
        public override void Close(VibDocument document)
            => throw new NotImplementedException();

        public override VibDocument Open(string filePath)
            => throw new NotImplementedException();

        public override void Save(VibDocument document)
            => throw new NotImplementedException();

        public void AddBlock(VibDocument document, VibBlock block)
        {
            document.Blocks.Add(block);
        }

        public void RemoveBlock(VibDocument document, VibBlock block)
        {
            document.Blocks.Remove(block);
        }

        public void AddConnection(VibDocument document, VibConnection connection)
            => throw new NotImplementedException();

        public void RemoveConnection(VibDocument document, VibConnection connection)
            => throw new NotImplementedException();

        public VibBlock GetBlock(VibDocument document, Guid blockId)
            => throw new NotImplementedException();

    }
}
