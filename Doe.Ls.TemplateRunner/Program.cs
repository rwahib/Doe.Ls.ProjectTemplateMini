using System;
using System.IO;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.ProjectTemplate.Data;
using Doe.Ls.ProjectTemplate.Data.T4Templates;

namespace Doe.Ls.TemplateRunner
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var pr = new Program();

            pr.PrintMessage("... Starting ...");
            pr.GenerateSettings();
            pr.GenerateModelsMetadata();
            pr.GenerateLightModelsMetadata();

            pr.GenerateRepositories();
            pr.GenerateServiceRepository();
            pr.GenerateRepositoryFactory();

            pr.GenerateUnitTest();
            pr.GenerateControllers();
            pr.GenerateMvcViews();
            pr.GenerateScripts();

            pr.PrintMessage("... All Tasks Complete ...");
            pr.PrintMessage("... Press any key to halt ...");

            //Console.ReadKey();
        }

        private void GenerateLightModelsMetadata()
        {
            var options = new TemplateOptions();
            InitialiseSettings(ref options);
            var tm = new TemplateManage();

            PrintMessage("GenerateModelsMetadata...");

            tm.GenerateLightModelsMetadata(options);
            PrintMessage("GenerateModelsMetadata complete...");
        }

        private void GenerateSettings()
        {
            PrintMessage("GenerateSettings...");

            var options = new TemplateOptions();
            InitialiseSettings(ref options);

            var tm = new TemplateManage();
            tm.GenerateSettings(options);
            PrintMessage("GenerateSettings complete...");
        }

        private void GenerateModelsMetadata()
        {
            var options = new TemplateOptions();
            InitialiseSettings(ref options);
            var tm = new TemplateManage();

            PrintMessage("GenerateModelsMetadata...");

            tm.GenerateModelsMetadata(options);
            PrintMessage("GenerateModelsMetadata complete...");
        }

        private void GenerateRepositories()
        {
            var options = new TemplateOptions();
            InitialiseSettings(ref options);
            var tm = new TemplateManage();
            PrintMessage("GenerateRepositories...");
            tm.GenerateRepositories(options);
            PrintMessage("GenerateRepositories complete...");
        }

        private void GenerateServiceRepository()
        {
            var options = new TemplateOptions();
            InitialiseSettings(ref options);
            var tm = new TemplateManage();
            PrintMessage("GenerateServiceRepository...");
            tm.GenerateServiceRepository(options);
            PrintMessage("GenerateServiceRepository completed...");

        }

        private void GenerateRepositoryFactory()
        {
            var options = new TemplateOptions();
            InitialiseSettings(ref options);
            var tm = new TemplateManage();
            PrintMessage("GenerateRepositoryFactory...");
            tm.GenerateRepositoryFactory(options);
            PrintMessage("GenerateRepositoryFactory completed...");
        }


        private void GenerateUnitTest()
        {
            var options = new TemplateOptions();
            InitialiseSettings(ref options);
            var tm = new TemplateManage();
            PrintMessage("GenerateUnitTest...");
            tm.GenerateUnitTest(options);
            PrintMessage("GenerateUnitTest completed...");
        }

        private void GenerateControllers()
        {
            var options = new TemplateOptions();

            InitialiseSettings(ref options);

            var tm = new TemplateManage();

            PrintMessage("GenerateControllers...");

            tm.GenerateControllers(options);

            PrintMessage("GenerateControllers completed...");
        }

        private void GenerateMvcViews()
        {
            var options = new TemplateOptions();

            InitialiseSettings(ref options);

            var tm = new TemplateManage();

            PrintMessage("GenerateMvcViews...");

            tm.GenerateMvcViews(options);

            PrintMessage("GenerateMvcViews completed...");
        }

        private void GenerateScripts()
        {
            var options = new TemplateOptions();
            InitialiseSettings(ref options);

            var tm = new TemplateManage();
            PrintMessage("GenerateScripts...");
            tm.GenerateScripts(options);

            PrintMessage("GenerateScripts completed...");
        }


        private void CreateDirectoryIfNotExists(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }

        private void InitialiseSettings(ref TemplateOptions options)
        {

            options.CodeGeneratedFolder = @"..\..\Doe.Ls.ProjectTemplate.Core\_CodeGenerated\";
            CreateDirectoryIfNotExists(options.CodeGeneratedFolder);

            options.RepositoriesFolder = Path.Combine(options.CodeGeneratedFolder, @"BL\EntityRepositories\");
            CreateDirectoryIfNotExists(options.RepositoriesFolder);

            options.SettingsFolder = Path.Combine(options.CodeGeneratedFolder, @"Settings\");
            CreateDirectoryIfNotExists(options.SettingsFolder);

            options.EntitiesFolder = Path.Combine(options.CodeGeneratedFolder, @"Data\Entities\");
            CreateDirectoryIfNotExists(options.EntitiesFolder);

            options.BlFolder = Path.Combine(options.CodeGeneratedFolder, @"BL\");
            CreateDirectoryIfNotExists(options.BlFolder);

            options.LightFolder = Path.Combine(options.BlFolder, "Models/", "Light/");
            CreateDirectoryIfNotExists(options.LightFolder);

            options.MvcFolder = Path.Combine(options.CodeGeneratedFolder, @"MVC\");
            CreateDirectoryIfNotExists(options.MvcFolder);


            options.ControllersFolder = Path.Combine(options.MvcFolder, @"Controllers\");
            CreateDirectoryIfNotExists(options.ControllersFolder);

            options.ViewsFolder = Path.Combine(options.MvcFolder, @"Views\");
            CreateDirectoryIfNotExists(options.ViewsFolder);

            options.UnitTestFolder = Path.Combine(options.CodeGeneratedFolder, @"UnitTests\");
            CreateDirectoryIfNotExists(options.UnitTestFolder);
            CreateDirectoryIfNotExists(Path.Combine(options.UnitTestFolder, @"EntityRepositories\"));

            options.WebConfigFile = @"..\App.config";
            options.AppNamespace = @"Doe.Ls.ProjectTemplate";
            options.SettingsFile = Path.Combine(options.SettingsFolder, @"PosEstablishmentSettings.cs");

            options.ScriptsFolder = Path.Combine(options.CodeGeneratedFolder, @"Scripts\");
            CreateDirectoryIfNotExists(options.ScriptsFolder);

            options.EntityContext = new SampleProjectTemplateEntities();
            T4Helper.SetAssemblyClassNameFormatFromContext(options.EntityContext);
        }

        private void PrintMessage(string msg)
        {
            Console.WriteLine("\n\r{0}", msg.Wordify());
        }
    }
}
