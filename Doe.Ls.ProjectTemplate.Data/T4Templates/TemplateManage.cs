using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.MVCExtensions;
using Doe.Ls.ProjectTemplate.Data.T4Templates.BL;
using Doe.Ls.ProjectTemplate.Data.T4Templates.MVCTemplate;
using Doe.Ls.ProjectTemplate.Data.T4Templates.Scripts;
using Doe.Ls.ProjectTemplate.Data.T4Templates.UnitTest;

namespace Doe.Ls.ProjectTemplate.Data.T4Templates
{
    public class TemplateManage
    {

        public void GenerateModelsMetadata(TemplateOptions options)
        {
            var helper = new EdmMetadataHelper(options.EntityContext);

            foreach (var entityType in helper.GetEntityList())
            {
                if (entityType.Name.ToLower().Contains("sysdia"))
                    continue;
                var session = GetDefaultSettings(options);
                session["EntityType"] = entityType;
                var tmpl = new SingleModelMetadataTemplate
                {
                    Session = session
                };

                tmpl.Initialize();

                var result = tmpl.TransformText();
                var fileName = Path.Combine(options.EntitiesFolder, entityType.Name + ".cs");

                File.WriteAllText(fileName, result);
            }

        }
        public void GenerateLightModelsMetadata(TemplateOptions options)
        {

            var helper = new EdmMetadataHelper(options.EntityContext);

            foreach (var entityType in helper.GetEntityList())
            {
                if (entityType.Name.ToLower().Contains("sysdia"))
                    continue;
                var session = GetDefaultSettings(options);
                session["EntityType"] = entityType;
                var tmpl = new SingleLightModelMetadataTemplate
                {
                    Session = session
                };

                tmpl.Initialize();

                var result = tmpl.TransformText();

                var fileName = Path.Combine(options.LightFolder, entityType.Name.CleanEntityName() + "Light.cs");

                File.WriteAllText(fileName, result);
            }
        }

        public void GenerateSettings(TemplateOptions options)
        {
            var session = GetDefaultSettings(options);

            var template = new SettingsTemplate
            {
                Session = session
            };

            template.Initialize();

            var result = template.TransformText();

            File.WriteAllText(options.SettingsFile, result);

        }

        public void GenerateServiceRepository(TemplateOptions options)
        {
            var session = GetDefaultSettings(options);

            var template = new ServiceRepositoryTemplate
            {
                Session = session
            };

            template.Initialize();
            var result = template.TransformText();
            File.WriteAllText(Path.Combine(options.BlFolder, "ServiceRepository.cs"), result);

        }

        public void GenerateRepositoryFactory(TemplateOptions options)
        {
            var session = GetDefaultSettings(options);

            var template = new RepositoryFactoryTemplate
            {
                Session = session
            };



            template.Initialize();
            var result = template.TransformText();
            File.WriteAllText(Path.Combine(options.BlFolder, "RepositoryFactory.cs"), result);

        }

        public void GenerateUnitTest(TemplateOptions options)
        {

            var helper = new EdmMetadataHelper(options.EntityContext);
            foreach (var entityType in helper.GetEntityList())
            {
                var session = GetDefaultSettings(options);
                session["EntityType"] = entityType;
                if (entityType.Name.ToLower().Contains("sysdia")) continue;
                var tmpl = new SingleUnitTestTemplate()
                {
                    Session = session
                };
                tmpl.Initialize();

                var result = tmpl.TransformText();
                var fileName = Path.Combine(options.UnitTestFolder, @"EntityRepositories\", T4Helper.CleanClassName(entityType.Name) + "RepositoryTests.cs");

                File.WriteAllText(fileName, result);

            }
        }

        public void GenerateRepositories(TemplateOptions options)
        {
            var helper = new EdmMetadataHelper(options.EntityContext);

            foreach (var entityType in helper.GetEntityList())
            {
                if (entityType.Name.ToLower().Contains("sysdia"))
                    continue;
                var session = GetDefaultSettings(options);
                session["EntityType"] = entityType;
                var tmpl = new SingleRepositoryTemplate()
                {
                    Session = session
                };

                tmpl.Initialize();

                var result = tmpl.TransformText();

                var fileName = Path.Combine(options.RepositoriesFolder, T4Helper.CleanClassName(entityType.Name) + "Repository.cs");

                File.WriteAllText(fileName, result);
            }
        }

        public void GenerateControllers(TemplateOptions options)
        {

            var helper = new EdmMetadataHelper(options.EntityContext);

            foreach (var entityType in helper.GetEntityList())
            {
                if (entityType.Name.ToLower().Contains("sysdia")) continue;
                var session = GetDefaultSettings(options);
                session["EntityType"] = entityType;

                var tmpl = new SingleControllerTemplate()
                {
                    Session = session
                };
                tmpl.Initialize();

                var result = tmpl.TransformText();
                var fileName = Path.Combine(options.ControllersFolder, T4Helper.CleanClassName(entityType.Name) + "Controller.cs");

                File.WriteAllText(fileName, result);
            }
        }

        public void GenerateMvcViews(TemplateOptions options)
        {


            var helper = new EdmMetadataHelper(options.EntityContext);

            foreach (var entityType in helper.GetEntityList())
            {
                if (entityType.Name.ToLower().Contains("sysdia")) continue;
//#if DEBUG
//                if (!entityType.Name.Equals("Executive", StringComparison.InvariantCultureIgnoreCase)
                                    
//                                    //KeyRelationship
//                                    ) continue;
//#endif


                string indexFolder = Path.Combine(options.ViewsFolder, T4Helper.CleanClassName(entityType.Name));
                if (!Directory.Exists(indexFolder))
                {
                    Directory.CreateDirectory(indexFolder);
                }

                var session = GetDefaultSettings(options);
                session["EntityType"] = entityType;

                // Creating Index View
                var templateIndex = new SingleViewIndexTemplate()
                {
                    Session = session
                };
                templateIndex.Initialize();

                string result = templateIndex.TransformText();

                var fileName = Path.Combine(indexFolder, "Index.cshtml");
                File.WriteAllText(fileName, result);

                // Creating Details
                var templateDetails = new SingleViewDetailsTemplate
                {
                    Session = session
                };
                templateDetails.Initialize();

                result = templateDetails.TransformText();
                fileName = Path.Combine(indexFolder, "Details.cshtml");
                File.WriteAllText(fileName, result);

                // Creating Details Modal
                var templateDetailsModal = new SingleViewDetailsTemplate_Modal
                {
                    Session = session
                };
                templateDetailsModal.Initialize();

                result = templateDetailsModal.TransformText();
                fileName = Path.Combine(indexFolder, "Details-modal.cshtml");
                File.WriteAllText(fileName, result);


                // Creating Delete
                var templateDelete = new SingleViewDeleteTemplate
                {
                    Session = session
                };
                templateDelete.Initialize();

                result = templateDelete.TransformText();

                fileName = Path.Combine(indexFolder, "Delete.cshtml");
                File.WriteAllText(fileName, result);

                // Creating Delete Modal
                var templateDeleteModal = new SingleViewDeleteTemplate_Modal
                {
                    Session = session
                };
                templateDeleteModal.Initialize();

                result = templateDeleteModal.TransformText();

                fileName = Path.Combine(indexFolder, "Delete-modal.cshtml");
                File.WriteAllText(fileName, result);

                // Creating Edit
                var templateEdit = new SingleViewEditTemplate
                {
                    Session = session
                };
                templateEdit.Initialize();

                result = templateEdit.TransformText();

                fileName = Path.Combine(indexFolder, "Edit.cshtml");
                File.WriteAllText(fileName, result);


                // Creating Edit Modal
                var templateEditModal = new SingleViewEditTemplate_Modal
                {
                    Session = session
                };
                templateEditModal.Initialize();

                result = templateEditModal.TransformText();

                fileName = Path.Combine(indexFolder, "Edit-modal.cshtml");
                File.WriteAllText(fileName, result);

                // Creating Create
                var templateCreate = new SingleViewCreateTemplate
                {
                    Session = session
                };
                templateCreate.Initialize();

                result = templateCreate.TransformText();
                fileName = Path.Combine(indexFolder, "Create.cshtml");
                File.WriteAllText(fileName, result);


                // Creating Create Modal
                var templateCreateModal = new SingleViewCreateTemplate_Modal
                {
                    Session = session
                };
                templateCreateModal.Initialize();

                result = templateCreateModal.TransformText();

                fileName = Path.Combine(indexFolder, "Create-modal.cshtml");
                File.WriteAllText(fileName, result);

                //------------------------------------------------ Partial Views -----------------------------------

                // Creating Partial list
                indexFolder = Path.Combine(options.ViewsFolder, T4Helper.CleanClassName(entityType.Name), "_partial");
                if (!Directory.Exists(indexFolder))
                {
                    Directory.CreateDirectory(indexFolder);
                }
                var templatePartialList = new SingleView_partial_listTemplate
                {
                    Session = session
                };
                templatePartialList.Initialize();

                result = templatePartialList.TransformText();

                fileName = Path.Combine(indexFolder, "_list.cshtml");
                File.WriteAllText(fileName, result);

                // Creating Partial details
                var templatePartialDetails = new SingleView_partial_detailsTemplate
                {
                    Session = session
                };
                templatePartialDetails.Initialize();

                result = templatePartialDetails.TransformText();

                fileName = Path.Combine(indexFolder, "_details.cshtml");
                File.WriteAllText(fileName, result);


                // Creating Partial edit
                var templatePartialEdit = new SingleView_partial_update_Template
                {
                    Session = session
                };
                templatePartialEdit.Initialize();

                result = templatePartialEdit.TransformText();

                fileName = Path.Combine(indexFolder, "_update.cshtml");
                File.WriteAllText(fileName, result);
            }

        }

        public void GenerateScripts(TemplateOptions options)
        {
            var helper = new EdmMetadataHelper(options.EntityContext);

            foreach (var entityType in helper.GetEntityList())
            {
                if (entityType.Name.ToLower().Contains("sysdia")) continue;
                var session = GetDefaultSettings(options);
                session["EntityType"] = entityType;

                var tmpl = new SingleServiceTemplate
                {
                    Session = session
                };
                tmpl.Initialize();

                var result = tmpl.TransformText();
                var fileName = Path.Combine(options.ScriptsFolder, T4Helper.GetLocalVariableName(entityType) + "Service.js");

                File.WriteAllText(fileName, result);
            }

            var dtSession = GetDefaultSettings(options);
            var dtTemplate = new DataTableServiceTemplate
            {
                Session = dtSession
            };
            dtTemplate.Initialize();

            var dtResult = dtTemplate.TransformText();
            var dtFileName = Path.Combine(options.ScriptsFolder, "dataTableService.js");

            File.WriteAllText(dtFileName, dtResult);

            var mainServiceSession = GetDefaultSettings(options);
            var mainServiceTemplate = new MainServiceTemplate
            {
                Session = mainServiceSession
            };
            mainServiceTemplate.Initialize();

            var mainServiceResult = mainServiceTemplate.TransformText();
            var mainServiceFileName = Path.Combine(options.ScriptsFolder, "mainService.js");

            File.WriteAllText(mainServiceFileName, mainServiceResult);

        }

        public System.Data.Entity.Core.Metadata.Edm.EntityType[] GetSystemLookupItems(DbContext ctx) {
            var helper = new EdmMetadataHelper(ctx);
            var enitylist = helper.GetEntityList(DataSpace.SSpace);
            var list = new List<System.Data.Entity.Core.Metadata.Edm.EntityType>();
            foreach (var entityType in enitylist) {
                
                var metadataClass = T4Helper.GetMetadataClassFor(entityType);
                if (metadataClass == null) continue;
                object attribute = metadataClass.GetCustomAttributes(typeof(LookupEntity), false).FirstOrDefault();
                
                    
                if (attribute != null) {
                    list.Add(entityType);
                }
            }
            return list.ToArray();
        }

        private Dictionary<string, object> GetDefaultSettings(TemplateOptions options)
        {
            var sessionSettings = new Dictionary<string, object>
            {
                {"Overwrite", true},
                {"AppNamespace", options.AppNamespace},
                {"EntityContext", options.EntityContext},
                {"ConfigFile", options.WebConfigFile},

            };

            return sessionSettings;
        }
    }
}