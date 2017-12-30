using System.Data.Entity;

namespace Doe.Ls.ProjectTemplate.Data.T4Templates
{
    public class TemplateOptions
    {
        public string AppNamespace { get; set; }
        public string BlFolder { get; set; }
        public string LightFolder { get; set; }
        public string CodeGeneratedFolder { get; set; }
        public string ControllersFolder { get; set; }
        public string EntitiesFolder { get; set; }
        public DbContext EntityContext { get; set; }
        public string MvcFolder { get; set; }
        public string OutputFolder { get; set; }
        public bool Overwrite { get; set; }
        public string RepositoriesFolder { get; set; }
        public string ScriptsFolder { get; set; }
        public string SettingsFile { get; set; }
        public string SettingsFolder { get; set; }
        public string UnitTestFolder { get; set; }
        public string ViewsFolder { get; set; }
        public string WebConfigFile { get; set; }
    }
}