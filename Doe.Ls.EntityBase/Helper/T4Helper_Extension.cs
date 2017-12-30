using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Configuration;
using System.Xml.Linq;
using Doe.Ls.EntityBase.Helper;
using Doe.Ls.EntityBase.MVCExtensions;
using EnvDTE;
using Microsoft.VisualStudio.TextTemplating;

namespace Doe.Ls.EntityBase.Helper
{
    public static partial class T4Helper
    {

        public static class Extension
        {
            public static DataTable GetDataTable(string tableName, string connectionString)
            {
                var query = "select * from " + tableName;
                var dt = new DataTable();

                var conn = new SqlConnection(connectionString);

                var cmd = new SqlCommand(query, conn);
                conn.Open();


                var da = new SqlDataAdapter(cmd);

                da.Fill(dt);
                conn.Close();
                da.Dispose();

                return dt;
            }

            public static DataTable GetSqlDataTable(string sql, string connectionString)
            {
                var query = sql;
                var dt = new DataTable();

                var conn = new SqlConnection(connectionString);

                var cmd = new SqlCommand(query, conn);
                conn.Open();


                var da = new SqlDataAdapter(cmd);

                da.Fill(dt);
                conn.Close();
                da.Dispose();

                return dt;
            }

            public static string GetConnectionStringFromAppConfig(string configPath)
            {
                var fileInfo = new FileInfo(configPath);
                if (!fileInfo.Exists)
                {
                    throw new FileNotFoundException(fileInfo.FullName);

                }
                var doc = XDocument.Load(fileInfo.FullName);
                var efConnection =
                    doc.Descendants("connectionStrings")
                        .SingleOrDefault()
                        .Element("add")
                        .Attribute("connectionString")
                        .Value;

                var result = efConnection.Split('"');

                return result[1].Trim();
            }

            public static string GetSolutionFolderPath(ITextTemplatingEngineHost host)
            {
                var serviceProvider = host as IServiceProvider;
                var dte = serviceProvider.GetService(typeof (EnvDTE.DTE)) as EnvDTE.DTE;
                return Path.GetDirectoryName(dte.Solution.FullName);

            }

            public static string GetCurrentProjectFileName(ITextTemplatingEngineHost host)
            {
                var serviceProvider = host as IServiceProvider;
                var dte = serviceProvider.GetService(typeof (EnvDTE.DTE)) as EnvDTE.DTE;

                EnvDTE.Project project = (dte.ActiveSolutionProjects as IList)[0] as EnvDTE.Project;

                return project.FullName;

            }

            public static Dictionary<string, EnvDTE.Property> GetCurrentProjectProperties(ITextTemplatingEngineHost host)
            {
                var serviceProvider = host as IServiceProvider;
                var dte = serviceProvider.GetService(typeof (EnvDTE.DTE)) as EnvDTE.DTE;

                EnvDTE.Project project = (dte.ActiveSolutionProjects as IList)[0] as EnvDTE.Project;

                var properties = new Dictionary<string, EnvDTE.Property>();
                foreach (EnvDTE.Property prop in project.Properties)
                {
                    properties.Add(prop.Name, prop);

                }

                return properties;

            }

            public static AssemblyFileInfo GetCurrentProjectAssembly(ITextTemplatingEngineHost host)
            {
               //AssemblyDescriptionAttribute
                // Assembly.LoadFile("").
                var outputFileNameProp = GetCurrentProjectProperties(host)["OutputFileName"];
                var projectFolder = Path.GetDirectoryName(GetCurrentProjectFileName(host));
                var fullBinaryLocation = Path.Combine(projectFolder, "bin", outputFileNameProp.Value.ToString());
                var fvi = FileVersionInfo.GetVersionInfo(fullBinaryLocation);
                byte[] assemblyBytes = File.ReadAllBytes(fullBinaryLocation);
                var assembly = Assembly.Load(assemblyBytes);

                return new AssemblyFileInfo
                {
                    TheAssembly =assembly,
                    FileLocation = fullBinaryLocation,
                    FileVersionInfo = fvi,
                    AssemblyDescription = fvi.Comments
                };

            }
            

        }
    }

    public class AssemblyFileInfo
    {
        public Assembly TheAssembly { get; set; }
        public string FileLocation { get; set; }
        public FileVersionInfo FileVersionInfo { get; set; }
        public string AssemblyDescription { get; set; }
    }
}