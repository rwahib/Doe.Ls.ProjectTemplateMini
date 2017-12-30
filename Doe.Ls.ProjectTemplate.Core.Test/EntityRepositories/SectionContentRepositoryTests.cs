using System;
using System.Linq;
using Doe.Ls.SchoolSportsUnit.Data;
using Doe.Ls.SchoolSportsUnit.Core.BL;
using Doe.Ls.SchoolSportsUnit.Core.BL.Models;
using Doe.Ls.SchoolSportsUnit.Core.Settings;
using Doe.Ls.SchoolSportsUnit.Core.Test.Mockups;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Console;

namespace Doe.Ls.SchoolSportsUnit.Core.Test.EntityRepositories
{
    [TestClass]
    public class SectionContentRepositoryTests : TestBase
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

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.SectionContentRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");

            foreach (var model in rep.List().Take(10))
            {
                Console.WriteLine("{0}", model.ToString());
            }
        }
        [TestMethod]
        public void CreateSection()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.SectionContentRepository();
            var count = rep.List().Count();
            var section = new SectionContent
            {
                Tag = UnitTestToken,
                StatusId = Enums.GlobalStatusValue.Draft.ToInteger(),
                ContentTitle = GenerateString("Content title"),
                SectionContentHtml = "<b>Hello</b>",
                AssociationId = Enums.Association.All.ToInteger()

            };
            rep.Insert(section);

            Assert.IsTrue(rep.List().Count() == count + 1, "rep.List().Count()==count+1");
            Assert.IsTrue(section.SectionKey > 0, "section.SectionKey>0");
            Assert.IsTrue(section.Version == 1, "section.Version== 1");
            Assert.IsTrue(section.SectionContentTypeId > 0, "section.SectionContentTypeId>0");
            Assert.IsTrue(section.StatusId > 0, "section.StatusId>0");
            Assert.IsTrue(section.CreatedDate != DateTime.MinValue, "section.CreatedDate!=DateTime.MinValue");
            Assert.IsTrue(section.CreatedBy != null, "section.CreatedBy!=null");

            rep.Delete(section);
            Assert.IsTrue(rep.List().Count() == count, "rep.List().Count() == count");

        }

        [TestMethod]
        public void MarkUsedFlag()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.SectionContentRepository();

            var section = new SectionContent
            {
                Tag = UnitTestToken,
                StatusId = Enums.GlobalStatusValue.Published.ToInteger(),
                ContentTitle = GenerateString("Content title"),
                SectionContentHtml = "<b>Hello</b>",
                AssociationId = Enums.Association.All.ToInteger()

            };
            rep.Insert(section);

            Assert.IsTrue(section.Used == false, "section.Used==false");
            rep.MarkSectionInUse(section.Tag);
            section = rep.GetPublishedSection(section.Tag);

            Assert.IsTrue(section.Used, "section.Used==true");


            rep.MarkOffSectionInUse(section.Tag);
            section = rep.GetPublishedSection(section.Tag);

            Assert.IsTrue(!section.Used, "!section.Used");

            rep.Delete(section);

        }
        [TestMethod]
        public void UrlResolver()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var tag = Guid.NewGuid().ToString().Substring(10);
            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.SectionContentRepository();
            var assetRep = globalServiceRepository.AssetRepository();
            var asset = assetRep.PublishedList().FirstOrDefault();

            var section = new SectionContent
            {
                Tag = tag,
                StatusId = Enums.GlobalStatusValue.Draft.ToInteger(),
                ContentTitle = GenerateString("Content title"),
                SectionContentHtml =asset.GetAbsoluteLink(assetRep) ,
                AssociationId = Enums.Association.All.ToInteger()

            };

            Assert.IsTrue(section.SectionContentHtml.ToLower().Contains(SchoolSportsUnitSettings.Site.AppUrl.ToLower()),"section.SectionContentHtml.Contains(SchoolSportsUnitSettings.Site.AppUrl)");
            rep.Insert(section);
            Assert.IsTrue(section.SectionContentHtml.ToLower().Contains("~/"),
                "section.SectionContentHtml.ToLower().Contains('~/')");

            var id1 = section.SectionKey;

            var content = rep.ConvertContentToAbsoluteUrl(section.SectionContentHtml);

            Assert.IsTrue(content.ToLower().Contains(SchoolSportsUnitSettings.Site.AppUrl.ToLower()),
                "content.ToLower().Contains(SchoolSportsUnitSettings.Site.AppUrl.ToLower())");

            rep.Delete(rep.GetSectionByKey(id1));
        }
        [TestMethod]
        public void CreateTag()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();
            var tag = Guid.NewGuid().ToString().Substring(10);
            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.SectionContentRepository();
            var count = rep.List().Count();
            var section = new SectionContent
            {
                Tag = tag,
                StatusId = Enums.GlobalStatusValue.Draft.ToInteger(),
                ContentTitle = GenerateString("Content title"),
                SectionContentHtml = "<b>Hello</b>",
                AssociationId = Enums.Association.All.ToInteger()

            };

            rep.Insert(section);
            var id1 = section.SectionKey;
            Assert.IsTrue(rep.List().Count() == count + 1, "rep.List().Count()==count+1");
            Assert.IsTrue(section.SectionKey > 0, "section.SectionKey>0");
            Assert.IsTrue(section.Version == 1, "section.Version== 1");
            Assert.IsTrue(section.SectionContentTypeId > 0, "section.SectionContentTypeId>0");
            Assert.IsTrue(section.StatusId > 0, "section.StatusId>0");
            WriteLine($"Tag was {tag}");

            section = new SectionContent
            {
                Tag = tag,
                StatusId = Enums.GlobalStatusValue.Draft.ToInteger(),
                ContentTitle = GenerateString("Content title"),
                SectionContentHtml = "<b>Hello</b>",
                AssociationId = Enums.Association.All.ToInteger()

            };

            rep.Insert(section);
            var id2 = section.SectionKey;
            WriteLine($"Saved Tag in database is {section.Tag}");
            Assert.IsTrue(section.Tag != tag, "section.Tag!=tag");
            Assert.IsTrue(section.Tag.Contains(tag.ToUpper()), "section.Tag.Contains(tag)");


            rep.Delete(rep.GetSectionByKey(id1));
            rep.Delete(rep.GetSectionByKey(id2));

            Assert.IsTrue(rep.List().Count() == count, "rep.List().Count() == count");

        }
        [TestMethod]
        public void Edit()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.SectionContentRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }
        [TestMethod]
        public void Delete()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.SectionContentRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Detail()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.SectionContentRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void Search()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.SectionContentRepository();
            if (!rep.List().Any()) Assert.Inconclusive("Insufficient data to run this test");
        }

        [TestMethod]
        public void CreateDraft()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var gSrv = new ServiceRepository(factory);
            var rep = gSrv.SectionContentRepository();
           
            var section = new SectionContent
            {
                Tag = UnitTestToken+GenerateString("_",4),
                StatusId = Enums.GlobalStatusValue.Published.ToInteger(),
                ContentTitle = GenerateString("Content title"),
                SectionContentHtml = "<b>Hello</b>",
                AssociationId = Enums.Association.All.ToInteger()

            };
            rep.Insert(section);

            Assert.IsTrue(section.SectionKey > 0, "section.SectionKey>0");
            Assert.IsTrue(section.Version == 1, "section.Version== 1");
            Assert.IsTrue(section.SectionContentTypeId > 0, "section.SectionContentTypeId>0");
            Assert.IsTrue(section.StatusId > 0, "section.StatusId>0");
            Assert.IsTrue(section.CreatedDate != DateTime.MinValue, "section.CreatedDate!=DateTime.MinValue");
            Assert.IsTrue(section.CreatedBy != null, "section.CreatedBy!=null");

            var draftSection=rep.SaveDraft(section,"Draft new Title");

            Assert.IsTrue(draftSection.SectionKey > 0, "section.SectionKey>0");
            Assert.IsTrue(draftSection.Version == section.Version+1, "draftSection.Version == section.Version+1");

            Assert.IsTrue(draftSection.SectionContentTypeId == section.SectionContentTypeId, "draftSection.SectionContentTypeId == section.SectionContentTypeId");

            Assert.IsTrue(draftSection.SectionContentHtml == section.SectionContentHtml, "draftSection.SectionContentHtml == section.SectionContentHtml");
            Assert.IsTrue(draftSection.AssociationId == section.AssociationId, "draftSection.AssociationId == section.AssociationId");


            Assert.IsTrue(draftSection.StatusId ==Enums.GlobalStatusValue.Draft.ToInteger(), "draftSection.StatusId ==Enums.GlobalStatusValue.Draft.ToInteger()");

            Assert.IsTrue(draftSection.CreatedDate != DateTime.MinValue, "section.CreatedDate != DateTime.MinValue");
            Assert.IsTrue(draftSection.CreatedBy != null, "section.CreatedBy!=null");

            Assert.IsTrue(draftSection.ContentTitle.Equals("Draft new Title",StringComparison.CurrentCultureIgnoreCase),"draftSection.ContentTitle.Equals('Draft new Title',StringComparison.CurrentCultureIgnoreCase)");



            rep.Delete(section);
            rep.Delete(draftSection);





        }


        [TestMethod]
        //[Ignore]
        public void TestSectionStatus1()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var gSrv = new ServiceRepository(factory);
            var rep = gSrv.SectionContentRepository();

            var publishedVersion = GetSampleSection();
            var draft = rep.SaveDraft(publishedVersion, $"new version for {UnitTestToken} at {DateTime.Now}");
            
            // convert draft to publish
            rep.UpdateWithPublising(draft);

            var oldPublished = rep.GetSectionByKey(publishedVersion.SectionKey);

            Assert.IsTrue(oldPublished.StatusId==Enums.GlobalStatusValue.Draft.ToInteger(),"newDraft.StatusId==Enums.GlobalStatusValue.Draft.ToInteger()");
            Assert.IsTrue(draft.StatusId == Enums.GlobalStatusValue.Published.ToInteger(),"draft.StatusId == Enums.GlobalStatusValue.Published.ToInteger()");

            // Revert
            
            rep.UpdateWithPublising(oldPublished);
            draft = rep.GetSectionByKey(draft.SectionKey);
            Assert.IsTrue(publishedVersion.StatusId == Enums.GlobalStatusValue.Published.ToInteger(), "publishedVersion.StatusId == Enums.GlobalStatusValue.Published.ToInteger()");
            Assert.IsTrue(draft.StatusId == Enums.GlobalStatusValue.Draft.ToInteger(), "draft.StatusId == Enums.GlobalStatusValue.Draft.ToInteger()");



            }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestSectionStatus2()
            {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var gSrv = new ServiceRepository(factory);
            var rep = gSrv.SectionContentRepository();

            var list = rep.GetPublishedList().OrderByDescending(m => m.AssociationId).Take(4);
            var result = list.FirstOrDefault();

            result.StatusId = Enums.GlobalStatusValue.Draft.ToInteger();
            rep.Update(result);
            

            }




        [ClassCleanup]
        public static void CleanUp()
        {
            TestBase.CleanUnitTestData();
        }


    }

}

