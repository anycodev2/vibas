using shared.Documents;
using shared.Projects;
using shared.Serialization;

namespace shared.Services
{
    public class VibProjectService : VibFileService<VibProject>
    {
        private readonly VibDocumentService _documentService;
        public VibProjectService(IVibSerializer<VibProject> serializer, VibDocumentService documentService) : base(serializer)
        {
            _documentService = documentService;
        }

        public override void Close(VibProject project)
        {

        }

        public override VibProject Open(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                throw new System.IO.FileNotFoundException();

            var content = System.IO.File.ReadAllText(filePath);
            var project = Serializer.Deserialize(content);
            project.FilePath = filePath;

            return project;
        }

        public override void Save(VibProject project)
        {
            if (project == null) 
                throw new ArgumentException(nameof(project));

            var content = Serializer.Serialize(project);
            System.IO.File.WriteAllText(project.FilePath, content);
        }

        public void AddDocument(VibProject project, VibDocument document)
        {
            if (project == null) 
                throw new ArgumentNullException(nameof(project));
            if (document == null)
                throw new ArgumentNullException(nameof(document));
            project.Documents.Add(document);
        }

        public void RemoveDocument(VibProject project, VibDocument document)
        {
            project.Documents.Remove(document);
        }

        public VibDocument GetDocument(VibProject project, Guid documentId)
        {
            if (project == null || project.Documents == null) 
                throw new KeyNotFoundException();

            var document = project.Documents.FirstOrDefault(d => d.Identifier == documentId);

            if (document == null)
                throw new KeyNotFoundException();

            return document;
        }
    }
}
