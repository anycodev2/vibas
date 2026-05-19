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
            if (project == null) throw new ArgumentNullException(nameof(project));
        }

        public override VibProject Open(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
            }

            try
            {
                var content = System.IO.File.ReadAllText(filePath);
                var project = Serializer.Deserialize(content);

                return project ?? throw new System.IO.InvalidDataException("Deserialized project object is null.");
            }
            catch (System.IO.FileNotFoundException ex)
            {
                throw new System.IO.FileNotFoundException($"The project file was not found at: {filePath}", ex);
            }
            catch (System.UnauthorizedAccessException ex)
            {
                throw new System.IO.IOException($"Access denied. You do not have permission to read: {filePath}", ex);
            }
            catch (System.IO.IOException ex)
            {
                throw new System.IO.IOException($"Disk or locking error occurred while opening: {filePath}", ex);
            }
        }

        public override void Save(VibProject project)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));

            try
            {
                var content = Serializer.Serialize(project);
                System.IO.File.WriteAllText(project.FilePath, content);
            }
            catch (System.UnauthorizedAccessException ex)
            {
                throw new System.IO.IOException("Cannot save: You do not have permission to edit this file.", ex);
            }
            catch (System.Exception ex)
            {
                throw new System.IO.IOException("A critical error occurred while saving the file.", ex);
            }
        }

        public void AddDocument(VibProject project, VibDocument document)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));
            if (document == null) throw new ArgumentNullException(nameof(document));

            if (project.Documents == null)
            {
                throw new InvalidOperationException("Project documents collection is not initialized.");
            }

            if (project.Documents.Any(d => d.Identifier == document.Identifier))
            {
                throw new InvalidOperationException("This document already exists in the project.");
            }

            project.Documents.Add(document);
        }

        public void RemoveDocument(VibProject project, VibDocument document)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));
            if (project.Documents == null) return;

            project.Documents.Remove(document);
        }

        public VibDocument GetDocument(VibProject project, Guid documentId)
        {
            if (project == null) throw new ArgumentNullException(nameof(project));

            if (project.Documents == null)
            {
                throw new InvalidOperationException("Project documents collection is not initialized.");
            }

            var document = project.Documents.FirstOrDefault(d => d.Identifier == documentId);
            return document ?? throw new KeyNotFoundException($"Document with ID {documentId} was not found.");
        }
    }
}