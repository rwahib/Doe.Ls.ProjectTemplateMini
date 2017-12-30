using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.Test.Mockups
{
    class UnitOfWorkMockup: UnitOfWork
    {
        public UnitOfWorkMockup()
        {
            Db = new SampleProjectTemplateEntities();
            var connectionString = Db.Database.Connection.ConnectionString;
            if (connectionString.Contains("{db-file-name}"))
            {

                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                var dbFilename = Path.Combine(Path.GetDirectoryName(path) ?? throw new InvalidOperationException(),
                    "App_Data\\unit-test.mdf");


                connectionString = connectionString.Replace("{db-file-name}", dbFilename);
                Db.Database.Connection.ConnectionString = connectionString;
            }

        }

    }
}
