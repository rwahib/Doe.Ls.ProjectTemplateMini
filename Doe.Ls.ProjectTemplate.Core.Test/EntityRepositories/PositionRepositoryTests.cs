

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using Doe.Ls.ProjectTemplate.Data;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.BL.Models.JQueryDataTableParam;
using Doe.Ls.ProjectTemplate.Core.BL.UI.RolePositionDescTasks;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Doe.Ls.ProjectTemplate.Core.Test.EntityRepositories
{
    [TestClass]
    public class PositionRepositoryTests : TestBase
    {


        [ClassInitialize]
        public static void Initialise(TestContext testContext)
        {
        }

        [TestMethod]
        [Ignore]
        public void ListVsReadOnlyList()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var rep = srv.PositionRepository();
            var timer1 = DateTime.Now;

            var all = rep.List().ToArray();
            var counter = 0;
            foreach (var position in all)
            {
                ++counter;
            }
            Console.WriteLine($"{counter} positions take up to {DateTime.Now-timer1}");

            var timer2 = DateTime.Now;

             all = rep.List().AsNoTracking().ToArray();
            counter = 0;
            foreach(var position in all)
                {
                ++counter;
                }
            Console.WriteLine($"{counter} positions take up to {DateTime.Now - timer2}");

            }



        [TestMethod]
        public void SerializePosition()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var rep = srv.PositionRepository();
            var positions =
                rep.ListForPdRoleDesc().Where(p => p.RolePositionDescription.IsPositionDescription).Take(3).ToList();
            foreach (var position in positions)
            {
                var result = rep.XmlSerialize(position);
                Console.Write(result);
               // File.WriteAllText($@"c:\temp\{position.PositionId}.xml",result.ToString());
            }



        }


        [TestMethod]
      /*  [ExpectedException(typeof(InvalidOperationException),
                "To create new Position Role/Position Description should be created first")]*/
        public void Create()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var gSrv = new ServiceRepository(factory);
            var rep = gSrv.PositionRepository();
           
            var position = GetPosition();

            position.RolePositionDescriptionId = gSrv.RolePositionDescriptionRepository().List().FirstOrDefault().RolePositionDescId;

            rep.CreateOrUpdatePosition(position);

            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }
        protected Position GetPosition(int lvlId = 10, int rptpos = 0)
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var rep = new ServiceRepository(factory);
            return new Position()
            {
                Description = "Test position desc " + UnitTestToken,
                PositionTitle = UnitTestToken + "TestPosition",
                PositionNumber = "20012",
                PositionLevelId = lvlId,
                ReportToPositionId = rep.PositionRepository().List().OrderBy(p=>p.PositionId).Skip(20).First().PositionId,
                LocationId = -1,
                StatusId = 10,
                //UnitChiefPositionId = rep.PositionRepository().List().Skip(5).FirstOrDefault().PositionId,
                LastModifiedBy = UnitTestToken + "test" + UnitTestToken,
                CreatedBy = UnitTestToken,
                //  CreatedDate = DateTime.Now,
                UnitId = rep.UnitRepository().List().FirstOrDefault().UnitId

            };
        }

     

        [TestMethod]
        public void Edit()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionRepository();
            var position = GetPositionWithRpDesc(globalServiceRepository);
            Assert.IsFalse(position==null,"Create position and Role/Position Description failed");
            var updPosition = new Position()
            {
                PositionNumber = "12344",
                PositionId = position.PositionId,
                Description = UnitTestToken+"updated",
                ReportToPositionId = position.ReportToPositionId,
                UnitId = position.UnitId,
                PositionLevelId = position.PositionLevelId,
                LocationId = position.LocationId,
                PositionTitle = position.PositionTitle,
                 //UnitChiefPositionId = int.Parse(rep.GetTeamMembersOrderByMaxRate(position.UnitId).FirstOrDefault().Value),

            };
            
            rep.CreateOrUpdatePosition(updPosition);
            var getUpd = rep.List().FirstOrDefault(l => l.PositionId == updPosition.PositionId);
            Assert.IsFalse(getUpd==null || !getUpd.Description.Contains(UnitTestToken+ "updated"),"Update failed");

        }
        [Ignore]
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException),
            "Sorry, Position title can't be changed")]
        public void ValidateAndUpdatePosition()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionRepository();
            var position = GetPositionWithRpDesc(globalServiceRepository);
            Assert.IsFalse(position == null, "Create position and Role/Position Description failed");
            var updPosition = new Position()
            {
                PositionNumber = "12344",
                PositionId = position.PositionId,
                Description = position.Description,
                ReportToPositionId = position.ReportToPositionId,
                UnitId = position.UnitId,
                //UnitChiefPositionId = position.ReportToPositionId,
                PositionLevelId = position.PositionLevelId,
                LocationId = position.LocationId,
                PositionTitle = UnitTestToken
           
        };
            
            rep.CreateOrUpdatePosition(updPosition);

        }
        [TestMethod]
        public void GetTeamMembersOrderByMaxRate()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionRepository();
            if (globalServiceRepository.UnitRepository().List().Count() < 10) Assert.Inconclusive("Insufficient data to run this test");

            var unit = globalServiceRepository.UnitRepository().List().Skip(10).FirstOrDefault();
            var no = rep.List().Count(p => p.UnitId == unit.UnitId && p.StatusId != (int) Enums.StatusValue.Inactive && p.StatusId != (int)Enums.StatusValue.Deleted && p.StatusId != (int)Enums.StatusValue.Draft);
            var list = rep.GetTeamMembersOrderByMaxRate(unit.UnitId);
            foreach (var pos in list)
            {
                Console.WriteLine("Position Id: " + pos.Value + "  Title: " + pos.Text);
            }
            Assert.IsFalse(no!= list.Count()," Mismatch in team members count");
           

        }

        [TestMethod]
        public void Delete()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Detail()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }



        [TestMethod]
        public void Search()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void CreatePositionAndRolePositiondescription()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var position = GetPositionWithRpDesc(globalServiceRepository, 33);

            Assert.IsFalse(position?.RolePositionDescriptionId == null || position.RolePositionDescriptionId == 0, "Sorry the create Position and role position desc failed");

        }

        [TestMethod]
        public void BasicFilterDisplayedPositions()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionRepository();
            var arg = new JQueryDatatableParamPositionExtension();
             var positions = rep.List().Where(l => l.PositionId != -1);
            var filteredList = rep.FilterDisplayedPositions(arg, positions);
           
        }
        [TestMethod]
        public void AdvFilterByDirFilterDisplayedPositions()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionRepository();
            var arg = new JQueryDatatableParamPositionExtension();
            var dir = globalServiceRepository.DirectorateRepository().List().Skip(4).FirstOrDefault();
            arg.DirectorateId = dir.DirectorateId;
          
            var positions = rep.List().Where(l => l.PositionId != -1);
            var filteredList = rep.FilterDisplayedPositions(arg, positions);
            var exppositions = rep.List().Where(l => l.PositionId != -1 && l.Unit.BusinessUnit.DirectorateId == arg.DirectorateId);
            Assert.IsFalse(exppositions.Count() != filteredList.Count(), "Advanced Filter failed");

        }

        [TestMethod]
        public void AdvFilterByTeamFilterDisplayedPositions()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionRepository();
            var arg = new JQueryDatatableParamPositionExtension();
            var dir = globalServiceRepository.DirectorateRepository().List().Skip(4).FirstOrDefault();
            arg.DirectorateId = dir.DirectorateId;
            var bunit =
                globalServiceRepository
                    .BusinessUnitRepository()
                    .List()
                    .FirstOrDefault(l => l.DirectorateId == dir.DirectorateId);
            arg.BusinessUnitId = bunit.BUnitId;

            var team =
               globalServiceRepository
                   .UnitRepository()
                   .List()
                   .FirstOrDefault(l => l.BUnitId == bunit.BUnitId);
            arg.UnitId = team.UnitId;
            var positions = rep.List().Where(l => l.PositionId != -1);
            var filteredList = rep.FilterDisplayedPositions(arg, positions);
            var exppositions = rep.List().Where(l => l.PositionId != -1 && l.Unit.UnitId == arg.UnitId );
            Assert.IsFalse(exppositions.Count() != filteredList.Count(), "Advanced Filter by Team failed");

        }

        [TestMethod]
        public void AdvFilterByBUnitFilterDisplayedPositions()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionRepository();
            var arg = new JQueryDatatableParamPositionExtension();
            var dir = globalServiceRepository.DirectorateRepository().List().Skip(4).FirstOrDefault();
            arg.DirectorateId = dir.DirectorateId;
            var bunit =
                globalServiceRepository
                    .BusinessUnitRepository()
                    .List()
                    .FirstOrDefault(l => l.DirectorateId == dir.DirectorateId);
            arg.BusinessUnitId = bunit.BUnitId;

            var positions = rep.List().Where(l => l.PositionId != -1);
            var filteredList = rep.FilterDisplayedPositions(arg, positions);
            var exppositions = rep.List().Where(l => l.PositionId != -1 && l.Unit.BusinessUnit.DirectorateId == arg.DirectorateId && l.Unit.BusinessUnit.BUnitId == arg.BusinessUnitId);
            Assert.IsFalse(exppositions.Count() != filteredList.Count(), "Advanced Filter by Business unit failed");

        }
        [TestMethod]
        public void AdvFilterByStatusFilterDisplayedPositions()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionRepository();
            
            var arg = new JQueryDatatableParamPositionExtension();
            arg.StatusCode =new [] { ( (int)Enums.StatusValue.Draft).ToString()};
            var positions = rep.List().Where(l => l.PositionId != -1);
            var filteredList = rep.FilterDisplayedPositions(arg, positions);
            var exppositions = rep.List().Where(l => l.PositionId != -1 && l.StatusId ==(int)Enums.StatusValue.Draft);
            Assert.IsFalse(exppositions.Count() != filteredList.Count(), "Advanced Filter by Status failed");
         

        }
        [TestMethod]
        public void AdvFilterByPositionStatusFilterDisplayedPositions()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionRepository();
            
         
            var arg = new JQueryDatatableParamPositionExtension();
            arg.PosStatusCode =new[] {
                globalServiceRepository.PositionStatusValueRepository().List().Skip(1).FirstOrDefault().PosStatusCode};
            var positions = rep.List().Where(l => l.PositionId != -1);
            var filteredList = rep.FilterDisplayedPositions(arg, positions);
            var exppositions = rep.List().Where(l => l.PositionId != -1 && arg.PosStatusCode.Contains(l.PositionInformation.PositionStatusCode));
            Assert.IsFalse(exppositions.Count() != filteredList.Count(), "Advanced Filter by Position Status code failed");


        }
        [TestMethod]
        public void SearchByFilterDisplayedPositions()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionRepository();
            var arg = new JQueryDatatableParamPositionExtension();
           
            var positions = rep.List().Where(l => l.PositionId != -1);
            arg.sSearch = positions.Skip(100).FirstOrDefault().PositionNumber;
            var filteredList = rep.FilterDisplayedPositions(arg, positions);
          //  var exppositions = rep.List().Where(l => l.PositionId != -1 && l.PositionNumber == arg.PositionNumber);

           Assert.IsFalse(!filteredList.Any(),"Search by positon number filter failed");

        }
        [TestMethod]
        public void EditPosition()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
           
            var globalServiceRepository = new ServiceRepository(factory);
            var repository = globalServiceRepository.PositionRepository();
            var position = GetPositionWithRpDesc(globalServiceRepository,55);
            var insertedObj = repository.List().FirstOrDefault(p => p.PositionId == position.PositionId);
#pragma warning disable CS0472 // The result of the expression is always 'false' since a value of type 'int' is never equal to 'null' of type 'int?'
            Assert.IsFalse(position == null || position.RolePositionDescriptionId == null || position.RolePositionDescriptionId == 0, "Sorry the create Position and role position desc failed");
#pragma warning restore CS0472 // The result of the expression is always 'false' since a value of type 'int' is never equal to 'null' of type 'int?'
            var newPosition = new Position()
            {
                PositionId = insertedObj.PositionId,
             PositionNumber   = "TestEdit",
            
             Description = UnitTestToken+"Edit",
             ReportToPositionId = -1,
             UnitId = globalServiceRepository.UnitRepository().List().FirstOrDefault().UnitId,
             LocationId = -1,
             PositionLevelId = -1,
             //UnitChiefPositionId = -1
            };
            var updatedObj = repository.CreateOrUpdatePosition(newPosition);
            Assert.IsFalse(updatedObj == null || !updatedObj.Description.Contains(UnitTestToken+"Edit"), "Sorry Position update failed");

        }
      


        /*Test methods for Chart*/

        public void GenerateListFromPath()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var repository = globalServiceRepository.PositionRepository();
            //   repository.GenerateListFromPath(repository.List().ToList(),repository.List()())

        }
        [TestMethod]
        public void GetPositions()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var srv = new ServiceRepository(factory);
            var repository = srv.PositionRepository();
            var unitRep = srv.UnitRepository();
            var directorateId = 0;

            var units = unitRep.List();
            if (!units.Any()) Assert.Inconclusive("Insufficient data to run this test");
            var unit = units.ToList().ElementAt(units.Count() - 2);
            var unitId = unit.UnitId;
            var liveStatus = new List<int>() {(int) Enums.StatusValue.Imported, (int) Enums.StatusValue.Approved};
            var positionlistmatch = repository.List().Where(p => liveStatus.Contains(p.StatusId)& p.UnitId == unitId);
            var pathnumbers = new HashSet<string>();
            foreach (var ps in positionlistmatch)
            {
                char[] separatingChars = { '/' };
                var arr = ps.PositionPath.Split(separatingChars, StringSplitOptions.RemoveEmptyEntries);
                foreach (var a in arr)
                {
                    var posInPath = repository.GetPositionById(a.ToInteger());
                    if (posInPath != null && posInPath.IsLive())
                    {
                        pathnumbers.Add(a);
                    }
                }
            }
            var params1 = new  PositionChartFilterParams()
            {
                UnitId = unitId,
                DirectorateId = directorateId,
               BusinessUnitId = 0
            };
            var positions = repository.LoadPositionListForChart(params1);
            Assert.IsFalse(pathnumbers.Count() != positions.Count(),
                $"count should be {positionlistmatch.Count()}, but is {positions.Count()}");
            
        }

        [TestMethod]
        public void LoadPositionsForByunitChart()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var repository = globalServiceRepository.PositionRepository();
            var unitRep = globalServiceRepository.UnitRepository();
            var directorateId = 0;
           

            var units = unitRep.List();
            if (!units.Any()) Assert.Inconclusive("Insufficient data to run this test");
            var unit = units.ToList().ElementAt(units.Count() - 2);
            var unitId = unit.UnitId;
            var liveStatus = new List<int>() { (int)Enums.StatusValue.Imported, (int)Enums.StatusValue.Approved };
            var positionlistmatch = repository.List().Where(p => liveStatus.Contains(p.StatusId) & p.UnitId == unitId);

            var pathnumbers = new HashSet<string>();
            foreach (var ps in positionlistmatch)
            {
                char[] separatingChars = { '/' };
                var arr = ps.PositionPath.Split(separatingChars, StringSplitOptions.RemoveEmptyEntries);
                foreach (var a in arr)
                {
                    var posInPath = repository.GetPositionById(a.ToInteger());
                    if(posInPath != null && posInPath.IsLive())
                        {
                        pathnumbers.Add(a);
                        }

                    }
            }
            var params1 = new PositionChartFilterParams()
            {
                UnitId = unitId,
                DirectorateId = directorateId,
                BusinessUnitId = 0
            };
            var positions = repository.LoadPositionListForChart(params1);
            if (positions.Any())
            {
                var positionsChartModel = repository.LoadChartJson(positions);
                Assert.IsFalse(pathnumbers.Count() != positionsChartModel.Count(),
                    $"count should be {positionlistmatch.Count()}, but is {positions.Count()}");
            }
            else
            {
                Console.WriteLine("No such positions found");
            }

        }
        [TestMethod]
        public void LoadPositionsByDirectorateForChart()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var repository = globalServiceRepository.PositionRepository();
            var dirRep = globalServiceRepository.DirectorateRepository();
            var unitId = 0;

            var dirs = dirRep.List();
            if (!dirs.Any()) Assert.Inconclusive("Insufficient data to run this test");

            foreach (var dir in dirs.ToList().Take(2).Skip(2))
            {
                var dirId = dir.DirectorateId;
                var positionListMatch = repository.ListForChart().Where(l => l.Unit.FunctionalArea.DirectorateId == dirId);
                var pathnumbers = new HashSet<string>();
                foreach(var ps in positionListMatch)
                    {
                    char[] separatingChars = { '/' };
                    var arr = ps.PositionPath.Split(separatingChars, StringSplitOptions.RemoveEmptyEntries);
                    foreach(var a in arr)
                        {
                        var posInPath = repository.GetPositionById(a.ToInteger());
                        if(posInPath != null && posInPath.IsLive())
                            {
                            pathnumbers.Add(a);
                            }
                        
                        }
                    }
                var params1 = new PositionChartFilterParams()
                    {
                    UnitId = unitId,
                    DirectorateId = dirId,
                    BusinessUnitId = 0
                    };
                var positions = repository.LoadPositionListForChart(params1);
                if(!positions.Any()) Assert.Inconclusive("Insufficient data to run this test");
                var positionsChartModel = repository.LoadChartJson(positions);
                Assert.IsTrue(pathnumbers.Count == positionsChartModel.Count || positionsChartModel.Count == pathnumbers.Count + 1, "pathnumbers.Count== positionsChartModel.Count || positionsChartModel.Count == pathnumbers.Count+1");

                }




            }

        [TestMethod]
        public void LoadPositionsByBusinessUnitForChart()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var repository = globalServiceRepository.PositionRepository();
            var buRep = globalServiceRepository.BusinessUnitRepository();

            var bunits = buRep.List();
            if (!bunits.Any()) Assert.Inconclusive("Insufficient data to run this test");
            foreach (var businessUnit in bunits.Skip(2).Take(2))
            {


                var bunitId = businessUnit.BUnitId;
                var positionlistmatch = repository.ListForChart().Where(l => l.Unit.BUnitId == bunitId);
                var pathnumbers = new HashSet<int>();
                foreach (var ps in positionlistmatch)
                {
                foreach (var a in ps.GetPositionsFromPath())
                    {
                        var posInPath = repository.GetPositionById(a);
                        if(posInPath != null && posInPath.IsLive())
                            {
                            if(!pathnumbers.Contains(a)) pathnumbers.Add(a);
                            }
                        }
                }
                var params1 = new PositionChartFilterParams()
                {
                    UnitId = 0,
                    DirectorateId = 0,
                    BusinessUnitId = bunitId
                };
                var positions = repository.LoadPositionListForChart(params1);
                if (!positions.Any()) Assert.Inconclusive("Insufficient data to run this test");
                var positionsChartModel = repository.LoadChartJson(positions);
                Assert.IsTrue(
                    pathnumbers.Count == positionsChartModel.Count || positionsChartModel.Count == pathnumbers.Count + 1,
                    "pathnumbers.Count== positionsChartModel.Count || positionsChartModel.Count == pathnumbers.Count+1");
            }

        }

        [TestMethod]
        public void GetChartObjForGeneratePdf()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var repository = globalServiceRepository.PositionRepository();
            Position parent = null;
            var positions = repository.ListForChart().Where(p => p.PositionId!=-1 &&p.StatusId == (int)Enums.StatusValue.Imported || p.StatusId == (int)Enums.StatusValue.Approved);
            foreach(var pos in positions.Take(10))
                {
                if(pos.PositionTitle.Contains("TBA") || pos.ParentPosition.PositionTitle.Contains("TBA")) continue;
                parent = pos.ParentPosition;
                break;
                }
            if(parent!=null)
                {
                var result=repository.GetChartObjForGeneratePdf(parent.PositionId);
                Console.WriteLine("Chart displays");
                foreach (var tmp in result)
                    {
                        Console.WriteLine($"{tmp} {tmp.PositionPath} for parent >>>>> {tmp.ParentPosition}");
                    }

                var allLiveChilds = repository.List().Where(p => p.ReportToPositionId== parent.PositionId && (p.StatusId == (int)Enums.StatusValue.Imported || p.StatusId == (int)Enums.StatusValue.Approved)).ToList();
                Console.WriteLine("all childs displays");
                foreach(var tmp in allLiveChilds)
                    {
                    Console.WriteLine($"{tmp} {tmp.PositionPath} for parent >>>>> {tmp.ParentPosition}");
                    }

                Assert.IsTrue(result.Count>=allLiveChilds.Count,"result.Count==allLiveChilds.Count");


                }
            

            }

        [TestMethod]
        public void TestAllPositionsByBusinessUnitForChart()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var repository = globalServiceRepository.PositionRepository();
            var buRep = globalServiceRepository.BusinessUnitRepository();
            var unitId = 0;
            var directorateId = 0;

            var bunits = buRep.List();
            if (!bunits.Any()) Assert.Inconclusive("Insufficient data to run this test");
           
            foreach (var bu in bunits.Take(5).ToArray())
            {
                var positionlistmatch = repository.CachedPositionListForChart().Where(l => l.Unit.BUnitId == bu.BUnitId);
                var pathnumbers = new HashSet<int>();
                foreach (var ps in positionlistmatch)
                {                    
                    
                    foreach (var a in ps.GetPositionsFromPath())
                    {
                      if(!pathnumbers.Contains(a)) pathnumbers.Add(a);
                    }
                }
                var params1 = new PositionChartFilterParams()
                {
                    UnitId = unitId,
                    DirectorateId = directorateId,
                    BusinessUnitId = bu.BUnitId
                };
              
                var positions = repository.LoadPositionListForChart(params1);
                

                foreach (var pathnumber in pathnumbers.Take(10))
                {
                    var status = repository.GetPositionById(pathnumber).StatusValue.GetEnum();
                    if (status == Enums.StatusValue.Imported || status == Enums.StatusValue.Approved)
                    {
                        Assert.IsTrue(positions.Any(p => p.PositionId == pathnumber),
                            $"positions.Any(p => p.PositionId == pathnumber) this pos-id {pathnumber} is not found");
                    }
                }
            }
          
        }


        [TestMethod]
        
        public void TestAllPositionsByDivisionForChart()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();



            var globalServiceRepository = new ServiceRepository(factory);
            var repository = globalServiceRepository.PositionRepository();
            var divisionRep = globalServiceRepository.ExecutiveRepository();
            var unitId = 0;
            var directorateId = 0;

            var divisions = divisionRep.List();
            if (!divisions.Any()) Assert.Inconclusive("Insufficient data to run this test");

            foreach (var dv in divisions.Take(5))
            {
                var positionlistmatch = repository.CachedPositionListForChart().Where(l => l.Unit.BusinessUnit.Directorate.ExecutiveCod == dv.ExecutiveCod)
                    
                    ;
                var pathnumbers = new HashSet<int>();
                foreach (var ps in positionlistmatch)
                {

                    foreach (var a in ps.GetPositionsFromPath())
                    {


                        if (!pathnumbers.Contains(a)) pathnumbers.Add(a);
                    }
                }
                var params1 = new PositionChartFilterParams()
                {
                    UnitId = unitId,
                    DirectorateId = directorateId,
                    DivisionCode = dv.ExecutiveCod
                };

                var positions = repository.LoadPositionListForChart(params1);


                foreach (var pathnumber in pathnumbers)
                {
                    var pos=repository.GetPositionById(pathnumber);
                    if(pos==null)continue;
                    if (pos.StatusValue.GetEnum() != Enums.StatusValue.Approved &&
                        pos.StatusValue.GetEnum() != Enums.StatusValue.Imported) continue;

                    Assert.IsTrue(positions.Any(p => p.PositionId == pathnumber), $"positions.Any(p => p.PositionId == pathnumber) this pos-id {pathnumber} is not found");
                }
            }

        }
        [Ignore]
        [TestMethod]
        public void FindTopPositionToPrint()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var repository = globalServiceRepository.PositionRepository();
            var unitRep = globalServiceRepository.UnitRepository();


            var units = unitRep.List();
            if (!units.Any()) Assert.Inconclusive("Insufficient data to run this test");

            foreach (var unit in units.Take(10))
            {
                
          
        
            var unitId = unit.UnitId;

            var positionlist = repository.List().Where(l => l.UnitId == unitId);
            if (!positionlist.Any()) continue;
                
            var dic = new Dictionary<int, int>();
            foreach (var ps in positionlist)
            {
                char[] separatingChars = { '/' };
                var arr = ps.PositionPath.Split(separatingChars, StringSplitOptions.RemoveEmptyEntries);
                dic.Add(ps.PositionId, arr.Count());

            }
            var tpfromdic = dic.FirstOrDefault(x => x.Value == dic.Values.Min()).Key;
                if (dic.Count(d => d.Value == dic[tpfromdic]) > 1)
                    continue;

            var topPositionId = repository.GetTopPositionToPrint(unitId);
                Console.WriteLine(unit);
                Console.WriteLine(tpfromdic);
                Console.WriteLine(topPositionId);

           Assert.IsTrue(tpfromdic == topPositionId, "tpfromdic == topPositionId");
             }
            }


        [TestMethod]
        public void TestLinkedPositions()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionRepository();

            var rolePositionDescId = 533;

            var count = rep.GetAllLinkedPositionsById(rolePositionDescId).Count();

            var count2 = rep.GetAllLinkedPositionCount(rolePositionDescId);

            Assert.AreEqual(count, count2);
        }


        [TestMethod]
        public void TestPositionTasks()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.PositionRepository();

            var positionId = 6189;

            var position = rep.List().SingleOrDefault(p => p.PositionId == positionId);

            if (position != null)
            {
                var positionTasks = PosRdPdFactory.Create(position).BuildTasks();

                if (positionTasks.Any())
                {
                    foreach (var item in positionTasks)
                    {
                        Console.WriteLine(item.Description);

                    }
                }
            }

        }


        [TestMethod]
        public void TestPositionTasks2()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var g = new ServiceRepository(factory);
            var rep = g.PositionRepository();

            var ids = rep.GetPositionsByStatus(Enums.StatusValue.Draft).Take(10).Select(p => p.PositionId);

            foreach (var id in ids)
            {
                var position = rep.GetPositionById(id);
                var positionTasks = PosRdPdFactory.Create(position).BuildTasks();

                if (positionTasks.Any())
                {
                    var text = positionTasks.Select(p => $"{p.Name}-{p.Description}").ToValue().ToLower();

                    if (position.CostCentreDetail != null)
                    {

                        if (string.IsNullOrEmpty(position.CostCentreDetail?.CostCentre) ||
                            string.IsNullOrEmpty(position.CostCentreDetail.Fund) ||
                            string.IsNullOrEmpty(position.CostCentreDetail.PayrollWBS))
                        {
                            Assert.IsTrue(text.Contains("cost"), "text.Contains('cost')");
                            Console.WriteLine($"{position} Contains cost task");

                        }
                        else
                        {
                            Assert.IsFalse(text.Contains("cost"), "text.Contains('cost')");
                            Console.WriteLine($"{position} Not contains cost task");
                        }


                    }
                }


            }
        }


        [TestMethod]
        public void GetChildTreeTest()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var g = new ServiceRepository(factory);
            var rep = g.PositionRepository();
            var start = DateTime.Now;

            var parent = rep.GetPositionById(-1);
                //rep
                //    .List().Where(pos => pos.StatusId == (int) Enums.StatusValue.Approved ||pos.StatusId == (int) Enums.StatusValue.Imported)
                //            .FirstOrDefault(pos => SqlFunctions.DataLength(pos.PositionPath) >= 10 );

            if (parent != null)
               
                {
                var result = rep.GetChildTree(parent.PositionId);

                var interval = DateTime.Now-start;
                Console.WriteLine($"It took {interval}");
                foreach (var id in result)
                {
                    var pos = rep.GetPositionById(id);

                    Console.WriteLine(pos);
                }

            }

            }


        [TestMethod]
        public void testDirectReports()
        {
            //Direct reports in format of 'Title - grade - (count)'
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var g = new ServiceRepository(factory);
            var rep = g.PositionRepository();

            //DirectReports is the subodinates whom report to you
            //Only show those status in Approved, Imported
            var lastPosition = rep.BaseList().OrderByDescending(p => p.PositionId).FirstOrDefault();

            var parentPositionId = lastPosition.PositionId;

            var subOrdinates = rep.BaseList().Where(s => s.ReportToPositionId == parentPositionId
                   && (s.StatusId == (int)Enums.StatusValue.Approved || s.StatusId == (int)Enums.StatusValue.Imported));
            //Get direct reports (subordinates) positions
            var sb = new StringBuilder();

            if (subOrdinates.Any())
            {

                //Get title grouped and get count
                var titleGrouped = subOrdinates
                    .GroupBy(n => new {n.PositionTitle, n.RolePositionDescription.GradeCode})
                    .Select(group =>
                        new
                        {
                            PositionTitle = group.Key.PositionTitle,
                            GradeCode = group.Key.GradeCode,
                            Count = group.Count()
                        });

                sb.Append("<ul>");
                foreach (var p in titleGrouped)
                {
                    if (p.Count > 1)
                    {
                        sb.Append("<li>" + p.PositionTitle + " " + p.GradeCode + " (" + p.Count + ")</li>");
                    }
                    else
                    {
                        sb.Append("<li>" + p.PositionTitle + " " + p.GradeCode + "</li>");
                    }
                }
                sb.Append("</ul>");

                Console.WriteLine(sb.ToString());
            }
            else
            {
                Console.WriteLine("No record was found");
            }
            
        }

        [TestCleanup]
        public void CleanUp()
        {
            TestBase.CleanUnitTestData();
        }


    }

}

