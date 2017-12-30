using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Doe.Ls.EntityBase;
using Doe.Ls.ProjectTemplate.Core.BL;
using Doe.Ls.ProjectTemplate.Core.BL.Models;
using Doe.Ls.ProjectTemplate.Core.BL.Models.Extensions;
using Doe.Ls.ProjectTemplate.Core.Settings;
using Doe.Ls.ProjectTemplate.Core.Test.Mockups;
using Doe.Ls.ProjectTemplate.Data;
using Doe.Ls.TrimBase;
using HP.HPTRIM.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doe.Ls.ProjectTemplate.Core.Test
    {
    public class TestBase
        {

        private static string _unitTestTokenLight = "@UNIT_TEST-";
        protected static string UnitTestToken = _unitTestTokenLight + Environment.MachineName;

        public TestBase()
        {
              CheckDevDatabase();
        }

       

        #region Protected      

        protected int GetRandom(int max)
            {
            var rnd = new Random(DateTime.Now.Millisecond);

            return rnd.Next(1, max);
            }

        protected string GenerateString(string startWith = "", int max = -1, bool email = false)
            {
            if(!email)
                {
                startWith += "@" + UnitTestToken;
                }
            else
                {
                startWith += "$" + UnitTestToken;
                }
            var genStr = Guid.NewGuid().ToString();
            if(max == -1) return startWith + genStr;

            var newString = (startWith + genStr);

            return newString.Length <= max  ? newString : newString .Substring(0, max -1);
            }
        
        protected string GenerateStringForEntity(string entityName, string startWith = "", int max = -1, bool email = false)
            {
            if(!email)
                {
                startWith += "@" + _unitTestTokenLight + entityName;
                }
            else
                {
                startWith += "$" + _unitTestTokenLight + entityName;
                }
            var genStr = Guid.NewGuid().ToString();
            if(max == -1) return startWith + genStr;

            if(max > genStr.Length) return startWith + genStr;

            if(max < 3) max = 3;

            return startWith + genStr.Substring(0, max - 3);
            }


        protected string GeneratePhoneNumber(string start, int max)
            {
            var sb = new StringBuilder();
            sb.Append(start);
            for(var i = 0; i < max; i++)
                {
                sb.Append(GetRandom(9).ToString());
                }
            return sb.ToString();
            }

        protected void Print(string format, params object[] args)
            {
            Console.WriteLine(format, args);
            }


        protected string GetDataFilePath(string filename)
            {
            var relativePath = Path.Combine(@"..\App_Data\", filename);
            var fi = new FileInfo(relativePath);
            if(!fi.Exists) throw new FileNotFoundException();
            return fi.FullName;
            }
        /// <summary>
        /// TODO need to be re implemented
        /// </summary>
        /// <returns></returns>
        protected void PrintUserExtension(UserInfoExtension u)
            {

            }

        /// <summary>
        /// TODO need to be re implemented
        /// </summary>
        /// <returns></returns>
        protected string GetSampleSSuPowerUserId()
            {
            var repository = GetServiceRepository();
            var result = repository.SysUserRepository().List().FirstOrDefault();

            return result.UserId;

            }

        /*    protected void SetCurrentUserAtController(AppControllerBase ctr, Enums.UserRole role,
                Enums.Association association = Enums.Association.NA)
            {
                if (ctr.CurrentUser.ActiveRoleAssociations != null)
                {
                    ctr.CurrentUser.ActiveRoleAssociations.Add(new RoleAssociation
                    {
                        RoleId = role.ToInteger(),
                        AssociationId = association.ToInteger(),
                        IsActive = true
                    });
                }

                else
                {
                    ctr.CurrentUser.ActiveRoleAssociations = new List<RoleAssociation>
                    {
                        new RoleAssociation
                        {
                            RoleId = role.ToInteger(),
                            AssociationId = association.ToInteger(),
                            IsActive = true
                        }
                    };
                }
            }

            protected string GetCurrentUserRole(UserInfoExtension currentUser)
            {
                var currentUserRole = Enums.UserRoleValues.Guest;
                if (currentUser.IsSystemAdministrator) currentUserRole = Enums.UserRoleValues.SystemAdministrator;
                else if (currentUser.IsSportUnitAdministrator) currentUserRole = Enums.UserRoleValues.SsuAdministrator;
                else if (currentUser.IsSportUnitPowerUser) currentUserRole = Enums.UserRoleValues.SsuPowerUser;
                else if (currentUser.IsAssociationAdministrator)
                    currentUserRole = Enums.UserRoleValues.AssociationAdministrator;
                else if (currentUser.IsAssociationPowerUser)
                    currentUserRole = Enums.UserRoleValues.AssociationPowerUser;

                return currentUserRole;
            }*/
        protected ServiceRepository GetServiceRepository()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var serviceRep = new ServiceRepository(factory);

            return serviceRep;
            }

        protected ServiceRepository GetServiceRepository(UserInfoExtension user)
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var serviceRep = new ServiceRepository(factory);
            serviceRep.SessionService().AddToSession(Cnt.CurrentUserKey, user);
            return serviceRep;
            }

        protected ServiceRepository GlobalServiceRepository()
            {
            var factory = new MockRepositoryFactory();

            factory.RegisterAllDependencies();
            var globalServiceRepository = new ServiceRepository(factory);
            return globalServiceRepository;
            }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="obj">Either IEnumerable or string</param>
        /// <param name="separators"> print before and after</param>
        protected void PrintObjectOrArray(string label, object obj, bool separators = false)
            {
            if(separators)
                {
                Console.WriteLine($"starting {label} -------------------------------------------------------------");
                }
            var enumerable = obj as IEnumerable;

            if(enumerable != null && !(obj is string))
                {
                Console.WriteLine($"{label}:");
                foreach(var val in enumerable)
                    {
                    Console.WriteLine(val);
                    }
                }
            else
                {
                Console.WriteLine($"{label}:{obj}");
                }

            if(separators)
                {
                Console.WriteLine($"end test for  {label} -------------------------------------------------------------");
                }
            }

        protected string GetRandomTags()
            {
            var tagList =
                "#sports #sport #active #fit #fitness #football #soccer #basketball #exercise #ball #gametime #workout #game #games #training #motorcycle #motorcycles #bike TagsForLikes ride #rideout #motorcross #biker #bikergang #helmet #cycle #bikelife #streetbike #cc #instabike #motorsport #motorsports #car #race #road #track #sport #sports #extreme #engine #wheels #rims #trackday #racing #racebike #grindout #shredded #fitgirl #fitboy #nutrition #workout #zumba #aerobics #running #cardio #fitlife #aesthetics #aesthetic #gym #lift #motorsport #motorsports #car #race #road #track #sport #sports #extreme #engine #wheels #rims #trackday #fast #racecar #broncos #denver #superbowl2015 #2015superbowl #superbowl49 #gobroncos #TagsForLikes #unitedinorange #broncosnation #denverbroncos #instagood #bronco #godenver #teammanning #letsgobroncos #seahawks #sea #footballgame #footballseason #kickoff #gohawks #TagsForLikes #seahawksnation #seattle #seattleseahawks #instagood #hawks #goseahawks #goseattle #TFLers"
                    .Split('#');
            var rnd = new RNGCryptoServiceProvider();
            var shuffledtags = tagList.OrderBy(x => GetNextInt32(rnd)).ToArray().Take(3);
            var sb = new StringBuilder();
            foreach(var tag in shuffledtags)
                {
                sb.Append($"{tag.Trim()},");
                }
            return sb.ToString().TrimEnd(',');


            }
        #endregion


        protected void ReportInconclusive(string message = null)
            {
            var msg = message ?? "There is not enough data to perform this task";
            Assert.Inconclusive(msg);
            }


        static int GetNextInt32(RNGCryptoServiceProvider rnd)
            {
            var randomInt = new byte[4];
            rnd.GetBytes(randomInt);
            return Convert.ToInt32(randomInt[0]);
            }

        protected static string GetNewGuid()
            {
            var g = new Guid();
            return g.ToString();
            }

        protected SysUser GetSampleSysUser()
        {
            return new SysUser()
            {UserId = GenerateString("user-id-",64),
            Email = GenerateString(max:24, email:true),
            FirstName = GenerateString("First-name",32),
            LastName = GenerateString("Last name",32),
            Active = true,
            

                };
        }
        protected Grade GetGrade(Enums.GradeType type)
        {
             var code = DateTime.Now.Millisecond+ "X";
            var grade = new Grade()
            {
                GradeCode = code,
                GradeTitle = UnitTestToken + "Title",
                GradeType = type.ToString(),
                StatusId = 10
            };
            return grade;
        }
        protected CapabilityGroup GetCapabilityGroup(string name="test")
        {
            var group = new CapabilityGroup()
            {
                GroupName = UnitTestToken+ name,
                GroupDescription = UnitTestToken + "Test Capability group descr",
                LastModifiedBy = "Unit Test",
                CreatedBy = "Unit test"
            };
            return group;
        }
        protected CapabilityLevel GetCapabilityLevel(string name="test")
        {
            var level = new CapabilityLevel()
            {
                LevelName = UnitTestToken +name,
                DisplayOrder = 10,
                LevelOrder = 3,
                LastModifiedBy = "Unit Test",
                CreatedBy = "Unit test"

            };
            return level;
        }
        protected CapabilityName GetCapabilityName(string name = "test")
        {
            var cname = new CapabilityName()
            {
                Name = UnitTestToken +name,
                CapabilityDescription = UnitTestToken + "Test Capability name description",
                LastModifiedBy = "Unit Test",
                CreatedBy = "Unit test"
            };
            return cname;
        }
        protected CapabilityBehaviourIndicator GetCapabilityBehaviourIndicator(string ctx = "test")
        {
            var cgrp = GetCapabilityGroup("for ind");
            GlobalServiceRepository().CapabilityGroupRepository().Insert(cgrp);
            var cname = GetCapabilityName("for Cap ind");
            cname.CapabilityGroupId = cgrp.CapabilityGroupId;
            GlobalServiceRepository().CapabilityNameRepository().Insert(cname);
            var clevel = GetCapabilityLevel("for Cap ind");
            GlobalServiceRepository().CapabilityLevelRepository().Insert(clevel);
          var caind = new CapabilityBehaviourIndicator()
           {
               CapabilityNameId = cname.CapabilityNameId,
               CapabilityLevelId = clevel.CapabilityLevelId,
               IndicatorContext = UnitTestToken+ctx,
               CreatedBy = "Test",
               LastModifiedBy = "Test"
           };
            return caind;
        }
        protected Focus GetFocus(string name= "Name")
        {
            var focus = new Focus()
            {
                FocusName = UnitTestToken+name,
                OrderList= 10
            };
            return focus;
        }

        protected SelectionCriteria GetCriteria(string criteria="criteria")
        {
            var scriteria= new SelectionCriteria()
            {
                Criteria = UnitTestToken+criteria,
                LastModifiedBy ="System",
                LastModifiedDate = DateTime.Now,
                
            };
            return scriteria;
        }
        protected LookupFocusGradeCriteria GetLookupFocusGradeCriteria(int focusId, string gradeCode, int criteriaId, string lastUpdatedBy=null)
        {
            var focusGradeCriteria = new LookupFocusGradeCriteria()
            {
                FocusId = focusId,
                GradeCode = gradeCode,
                IsMandatory = true,
                SelectionCriteriaId = criteriaId,
                LastUpdatedBy = UnitTestToken+lastUpdatedBy,
                LastUpdatedDate = DateTime.Now
            };
            return focusGradeCriteria;
        }
        protected PositionDescription TestCreatePositionDescription(ServiceRepository srvRepository, int rnd)
        {
            var grade = GetGrade(Enums.GradeType.NSBTS);
            srvRepository.GradeRepository().Insert(grade);
            var rolepositionDesc = new RolePositionDescription
            {
                DocNumber = GetDocumentNumber(rnd),
                Title = UnitTestToken + "Test Insert",
                GradeCode = grade.GradeCode,
                DateOfApproval = null,
                StatusId = (int)Enums.StatusValue.Draft

            };
            var positionDescription = new PositionDescription
            {
                BriefRoleStatement = UnitTestToken + "Test role statement",
                StatementOfDuties = UnitTestToken + "Test Statement of duties"
            };
            srvRepository.PositionDescriptionRepository().CreatePositionDescription(positionDescription, rolepositionDesc);
            return positionDescription;
        }
        protected PositionDescription TestCreatePositionDescriptionForTrim(ServiceRepository srvRepository, string docNumber,string title)
        {
            var grade = srvRepository.GradeRepository().ListForPositiondesc().FirstOrDefault();
            
            var rolePositionDesc = new RolePositionDescription {
                DocNumber = docNumber,
                Title = title,
                GradeCode = grade.GradeCode,
                DateOfApproval = null,
                StatusId = (int)Enums.StatusValue.Draft

                };
            var positionDescription = new PositionDescription {
                BriefRoleStatement = "Test role statement for Trim",
                StatementOfDuties = "Test Statement of duties for Trim"
                };
            srvRepository.PositionDescriptionRepository().CreatePositionDescription(positionDescription, rolePositionDesc);
            return positionDescription;
            }
        protected RoleDescription TestCreateRoleDescriptionForTrim(ServiceRepository srvRepository, string docNumber, string title)
        {
            var gradeCode = Enums.Grade.ClerkGrade56;

            var rolePositionDesc = new RolePositionDescription {
                DocNumber = docNumber,
                Title = title,
                GradeCode = gradeCode,
                DateOfApproval = null,
                StatusId = (int)Enums.StatusValue.Draft

                };
            var roleDescription = new RoleDescription() {
              ANZSCOCode = "ANZSCOCode for TRIM",
              PCATCode = "PCATCode for TRIM",
              RolePrimaryPurpose = "Role primary purpose",
              DecisionMaking = @"<p><ul><li>DecisionMaking for Trim</li><li>2</li><li>3</li><li>4</li><li>5</li><li>6</li><li>7</li><li>8</li></ul></p>",
              KeyAccountabilities = @"<p><ul><li>KeyAccountabilities for Trim</li><li>2</li><li>3</li><li>4</li><li>5</li><li>6</li><li>7</li><li>8</li></ul></p>",
                KeyChallenges = @"<p><ul><li>KeyChallenges for Trim</li><li>2</li><li>3</li></ul></p>",
                BudgetExpenditure = Enums.Cnt.Nil,
                EssentialRequirements = @"<p><ul><li>EssentialRequirements for Trim</li><li>2</li><li>3</li></ul></p>",
                };
            var repo = srvRepository.RoleDescriptionRepository();
            repo.CreateRoleDescription(roleDescription, rolePositionDesc);

            roleDescription = repo.GetRoleDescriptionById(roleDescription.RoleDescriptionId);

            var keyRelationShips = srvRepository.KeyRelationshipRepository().List();
            var scopeDictionary = new Dictionary<int, string>();
            foreach (var kr in keyRelationShips)
            {
                if (kr.ScopeId.HasValue)
                {
                    if(scopeDictionary.ContainsKey(kr.ScopeId.Value))continue;
                    roleDescription.KeyRelationships.Add(kr);
                    scopeDictionary.Add(kr.ScopeId.Value,kr.RelationshipScope.ScopeTitle);
                }
            }
            var cbiList = srvRepository.CapabilityBehaviourIndicatorRepository().List();
            foreach(var cbi in cbiList.Take(10)) {
               
                   
                    roleDescription.RoleCapabilities.Add(new RoleCapability
                    {
                        CapabilityLevelId = cbi.CapabilityLevelId,
                        CapabilityNameId = cbi.CapabilityNameId,
                        Highlighted = true,
                        RoleDescriptionId = roleDescription.RoleDescriptionId,
                        LastUpdated = DateTime.Now,
                        DateCreated = DateTime.Now,
                        CreatedBy = Enums.Cnt.System,
                        LastModifiedBy = Enums.Cnt.System,
                        });
                   
                   
                }
            repo.Update(roleDescription);

            return roleDescription;
        }
        protected string GetDocumentNumber(int rnd = 3000)
        {
          var yr =  DateTime.Now.Year%10;
            return $"DOC{yr}/{rnd}";
        }


        protected void PrintRecord(Record record, bool printHeaderAndFooter=true)
            {
            if (printHeaderAndFooter)
            {
                Console.WriteLine($"------- Record {record.Number} ------------\n\n");
                Console.WriteLine($"Title\t\t {record.Title}");
                Console.WriteLine($"Number\t\t {record.Number}");

                Console.WriteLine($"Uri\t\t {record.Uri}");
                if(record.CurrentVersion != null) Console.WriteLine($"CurrentVersion\t\t {record.CurrentVersion.StringValue}");
                if(record.AllVersions != null) Console.WriteLine($"AllVersions\t\t {record.AllVersions.Value}");

                if(record.LatestVersion != null) Console.WriteLine($"LatestVersion\t\t {record.LatestVersion.StringValue}");
                if(record.RevisionNumber != null) Console.WriteLine($"RevisionNumber\t\t {record.RevisionNumber.Value}");
                Console.WriteLine($"DateCreated\t\t {record.DateCreated.DateTime}");

                if(record.Author!=null) Console.WriteLine($"Author\t\t {record.Author.FormattedName}");
                if(record.Creator != null) Console.WriteLine($"Author\t\t {record.Creator.FormattedName}");
                
                

                Console.WriteLine($"DateLastUpdated\t\t {record.DateLastUpdated.DateTime}");
                if(record.LastUpdatedBy != null) Console.WriteLine($"LastUpdatedBy\t\t {record.LastUpdatedBy.FormattedName}");
                Console.WriteLine($"DocumentStatus\t\t {record.DocumentStatus}");

                if(record.HomeLocation != null) Console.WriteLine($"HomeLocation\t\t {record.HomeLocation.FormattedName}");
                Console.WriteLine($"MimeType\t\t {record.MimeType}");
                Console.WriteLine($"FilePath\t\t {record.FilePath}");

                Console.WriteLine($"HomeLocationStatus\t\t {record.HomeLocationStatus}");
                


                Console.WriteLine($"-------\t\t------------\n\n");
                }
            }


        //Cleanup. Write all the delete command here

        protected static void CheckDevDatabase()
            {

            var connectionString =
                Settings.ProjectTemplateSettings.ConnectionStrings.SampleProjectTemplateEntities;

            if(connectionString.ToUpper().Contains("PW0000SQL"))
                {
                throw new SecurityException("Could not perform unit tests in production database");
                }
            if(connectionString.ToUpper().Contains("QW0000SQL"))
                {
                throw new SecurityException("Could not perform unit tests in PreProduction database");
                }

            if(TrimSettings.TrimApi.ToLower().Contains("https://trimw.det.nsw.edu.au")) {
                throw new SecurityException("Could not perform unit tests in production TRIM HP CM");
                }

            }
        protected Position GetPositionWithRpDesc(ServiceRepository globalServiceRepository, int i = 0)
        {
            var repository = globalServiceRepository.PositionRepository();
            var grade = GetGrade(Enums.GradeType.PSNE);
            //check grade
            if (!globalServiceRepository.GradeRepository().Exists(grade.GradeCode))
            {
                globalServiceRepository.GradeRepository().Insert(grade);
            }
            var rolePos = new RolePositionDescription()
            {
                Title = UnitTestToken + "Title",
                DocNumber = GetDocumentNumber(2000),
                GradeCode = grade.GradeCode,
                IsPositionDescription = false
            };
            //var rolePosDesc =
            var position = repository.CreatePositionAndRolePositionDescription(rolePos);
            return position;
        }

        public static void CleanUnitTestData()
        {
           var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var rep = new ServiceRepository(factory);
            rep.CleanUnitTestData(UnitTestToken);
            
        }

        protected void PrintDumpObject(object model,string title=null)
        {
            var objType = model.GetType();
            var propList = objType.GetProperties();
            Console.WriteLine($"{{   {title}");
            foreach (var prop in propList)
            {
                Console.WriteLine($"{prop.Name} = {prop.GetValue(model)}");
                }
            Console.WriteLine("}");

            }
        }
    }
