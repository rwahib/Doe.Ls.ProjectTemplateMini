

using System;
using System.Collections.Generic;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.ProjectTemplate.Data;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using System.Linq.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.ProjectTemplate.Core.Test.EntityRepositories
    {
    [TestClass]
    public class Sorting : TestBase
        {



        [ClassInitialize]
        public static void Initialise(TestContext testContext)
            {

            }

        [TestMethod]
        public void List()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var srv = new ServiceRepository(factory);
            var rep = srv.DirectorateRepository();
            var list = rep.List();

            var arg = JQueryDataTableParamModel.CreateArgument(0, 10, null, "Executive.ExecutiveTitle");

            IEnumerable<Directorate> resultDirectorates = null;
            
                var orderBy = arg.SortColumnDesc ? arg.SortColumnName + " desc" : arg.SortColumnName;
                resultDirectorates = list.OrderBy(orderBy);

            foreach (var d in resultDirectorates)
            {
                Console.WriteLine($"{d.DirectorateId}-{d.DirectorateName}-{d.Executive.ExecutiveTitle}-{d.StatusValue.StatusName}");
            }



            }

        [TestCleanup]
        public void CleanUp()
            {
            CleanUnitTestData();
            }


        }

    }

