namespace shared.Projects
{
    public class SbProject
    {
        public string Name { get; }

        public SbProject(string name)
        {
            Name = name;
        }

        public bool HasValidName()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }
    }
}
