using shared.Documents;
using shared.Projects;
using shared.Serialization;

namespace shared.Services
{
    public class VibProjectService : VibFileService<VibProject>
    {
        private readonly VibDocumentService _documentService;
        public VibProjectService(IVibSerializer serializer, VibDocumentService documentService) : base(serializer)
        {
            _documentService = documentService;
        }

        public override void Close(VibProject project)
            => throw new NotImplementedException();

        public override VibProject Open(string filePath)
            => throw new NotImplementedException();

        public override void Save(VibProject project)
            => throw new NotImplementedException();

        public void AddDocument(VibProject project, VibDocument document)
            => throw new NotImplementedException();

        public void RemoveDocument(VibProject project, VibDocument document)
            => throw new NotImplementedException();

        public VibDocument GetDocument(VibProject project, Guid documentId)
            => throw new NotImplementedException();
    }
}
