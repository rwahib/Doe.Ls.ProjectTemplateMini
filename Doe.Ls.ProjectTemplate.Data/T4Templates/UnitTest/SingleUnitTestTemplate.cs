﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 15.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Doe.Ls.ProjectTemplate.Data.T4Templates.UnitTest
{
    using Doe.Ls.EntityBase.Helper;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\UnitTest\SingleUnitTestTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "15.0.0.0")]
    public partial class SingleUnitTestTemplate : MVCTemplate.BaseTemplate
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write(" \r\n");
            this.Write("\r\n");
            
            #line 11 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\UnitTest\SingleUnitTestTemplate.tt"
 T4Helper.SetAssemblyClassNameFormatFromContext(EntityContext); 
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 13 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\UnitTest\SingleUnitTestTemplate.tt"
    

var helper=new EdmMetadataHelper(EntityContext);    

            
            #line default
            #line hidden
            this.Write("using System;\r\nusing System.Linq; \r\nusing ");
            
            #line 19 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\UnitTest\SingleUnitTestTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(AppNamespace));
            
            #line default
            #line hidden
            this.Write(".Data;\r\nusing ");
            
            #line 20 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\UnitTest\SingleUnitTestTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(AppNamespace));
            
            #line default
            #line hidden
            this.Write(".Core.BL;\r\nusing ");
            
            #line 21 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\UnitTest\SingleUnitTestTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(AppNamespace));
            
            #line default
            #line hidden
            this.Write(".Core.Test.Mockups;\r\n\r\nusing Microsoft.VisualStudio.TestTools.UnitTesting;\r\n\r\n\r\nn" +
                    "amespace ");
            
            #line 26 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\UnitTest\SingleUnitTestTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(AppNamespace));
            
            #line default
            #line hidden
            this.Write(".Core.Test.EntityRepositories \r\n{\r\n[TestClass]\r\n    public class ");
            
            #line 29 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\UnitTest\SingleUnitTestTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.CleanClassName(EntityType.Name)));
            
            #line default
            #line hidden
            this.Write(@"RepositoryTests : TestBase {

	
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
            var rep = globalServiceRepository.");
            
            #line 44 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\UnitTest\SingleUnitTestTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.CleanClassName(EntityType.Name)));
            
            #line default
            #line hidden
            this.Write(@"Repository();
            if (!rep.List().Any())Assert.Inconclusive(""Insufficient data to run this test"");

            foreach (var model in rep.List().Take(10)) {
                Console.WriteLine(""{0}"", model.ToString());
            }
        }
        [TestMethod]
        public void Create()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.");
            
            #line 58 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\UnitTest\SingleUnitTestTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.CleanClassName(EntityType.Name)));
            
            #line default
            #line hidden
            this.Write(@"Repository();
            if (!rep.List().Any())Assert.Inconclusive(""Insufficient data to run this test"");
        }
        [TestMethod]
        public void Edit()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.");
            
            #line 68 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\UnitTest\SingleUnitTestTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.CleanClassName(EntityType.Name)));
            
            #line default
            #line hidden
            this.Write(@"Repository();
            if (!rep.List().Any())Assert.Inconclusive(""Insufficient data to run this test"");
        }
        [TestMethod]
        public void Delete()
        {
             var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.");
            
            #line 78 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\UnitTest\SingleUnitTestTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.CleanClassName(EntityType.Name)));
            
            #line default
            #line hidden
            this.Write(@"Repository();
            if (!rep.List().Any())Assert.Inconclusive(""Insufficient data to run this test"");
        }

        [TestMethod]
        public void Detail()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.");
            
            #line 89 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\UnitTest\SingleUnitTestTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.CleanClassName(EntityType.Name)));
            
            #line default
            #line hidden
            this.Write(@"Repository();
            if (!rep.List().Any())Assert.Inconclusive(""Insufficient data to run this test"");
        }

        [TestMethod]
        public void Search()
        {
            var factory = new MockRepositoryFactory();
            factory.RegisterAllDependencies();

            var globalServiceRepository = new ServiceRepository(factory);
            var rep = globalServiceRepository.");
            
            #line 100 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\UnitTest\SingleUnitTestTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.CleanClassName(EntityType.Name)));
            
            #line default
            #line hidden
            this.Write(@"Repository();
            if (!rep.List().Any())Assert.Inconclusive(""Insufficient data to run this test"");
        }

      [TestCleanup]
        public void CleanUp()
        {
            TestBase.CleanUnitTestData();
        }

       
    }   

   }

");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 1 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\UnitTest\SingleUnitTestTemplate.tt"

private global::System.Data.Entity.Core.Metadata.Edm.EntityType _EntityTypeField;

/// <summary>
/// Access the EntityType parameter of the template.
/// </summary>
private global::System.Data.Entity.Core.Metadata.Edm.EntityType EntityType
{
    get
    {
        return this._EntityTypeField;
    }
}

private string _AppNamespaceField;

/// <summary>
/// Access the AppNamespace parameter of the template.
/// </summary>
private string AppNamespace
{
    get
    {
        return this._AppNamespaceField;
    }
}

private global::System.Data.Entity.DbContext _EntityContextField;

/// <summary>
/// Access the EntityContext parameter of the template.
/// </summary>
private global::System.Data.Entity.DbContext EntityContext
{
    get
    {
        return this._EntityContextField;
    }
}


/// <summary>
/// Initialize the template
/// </summary>
public override void Initialize()
{
    base.Initialize();
    if ((this.Errors.HasErrors == false))
    {
bool EntityTypeValueAcquired = false;
if (this.Session.ContainsKey("EntityType"))
{
    this._EntityTypeField = ((global::System.Data.Entity.Core.Metadata.Edm.EntityType)(this.Session["EntityType"]));
    EntityTypeValueAcquired = true;
}
if ((EntityTypeValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("EntityType");
    if ((data != null))
    {
        this._EntityTypeField = ((global::System.Data.Entity.Core.Metadata.Edm.EntityType)(data));
    }
}
bool AppNamespaceValueAcquired = false;
if (this.Session.ContainsKey("AppNamespace"))
{
    this._AppNamespaceField = ((string)(this.Session["AppNamespace"]));
    AppNamespaceValueAcquired = true;
}
if ((AppNamespaceValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("AppNamespace");
    if ((data != null))
    {
        this._AppNamespaceField = ((string)(data));
    }
}
bool EntityContextValueAcquired = false;
if (this.Session.ContainsKey("EntityContext"))
{
    this._EntityContextField = ((global::System.Data.Entity.DbContext)(this.Session["EntityContext"]));
    EntityContextValueAcquired = true;
}
if ((EntityContextValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("EntityContext");
    if ((data != null))
    {
        this._EntityContextField = ((global::System.Data.Entity.DbContext)(data));
    }
}


    }
}


        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
}
