﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 15.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Doe.Ls.ProjectTemplate.Data.T4Templates.BL
{
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using Doe.Ls.EntityBase.Helper;
    using Doe.Ls.EntityBase.MVCExtensions;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "15.0.0.0")]
    public partial class SingleRepositoryTemplate : MVCTemplate.BaseTemplate
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write(" \r\n");
            this.Write("\r\n");
            
            #line 13 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
 T4Helper.SetAssemblyClassNameFormatFromContext(EntityContext); 
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 15 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
  
    var helper=new EdmMetadataHelper(EntityContext);   

            
            #line default
            #line hidden
            this.Write(@"using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Doe.Ls.EntityBase.Exceptions;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;
using ");
            
            #line 28 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(AppNamespace));
            
            #line default
            #line hidden
            this.Write(".Data;\r\n\r\nnamespace ");
            
            #line 30 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(AppNamespace));
            
            #line default
            #line hidden
            this.Write(".Core.BL.EntityRepositories \r\n{\r\n    public partial class ");
            
            #line 32 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.CleanClassName(EntityType.Name)));
            
            #line default
            #line hidden
            this.Write("Repository : BaseRepository<");
            
            #line 32 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(EntityType.Name));
            
            #line default
            #line hidden
            this.Write("> \r\n    {\r\n        public ");
            
            #line 34 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.CleanClassName(EntityType.Name)));
            
            #line default
            #line hidden
            this.Write("Repository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService " +
                    "sessionService) : base(unitOfWork, loggerService, sessionService)\r\n        {\r\n  " +
                    "      }\r\n\r\n        public override IQueryable<");
            
            #line 38 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(EntityType.Name));
            
            #line default
            #line hidden
            this.Write("> List()\r\n        {                       \r\n            return base.List()\r\n");
            
            #line 41 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
    foreach(var property in this.EntityType.NavigationProperties)                      
            
            #line default
            #line hidden
            
            #line 42 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
    {
            
            #line default
            #line hidden
            this.Write("                    .Include(ent=>ent.");
            
            #line 43 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Name));
            
            #line default
            #line hidden
            this.Write(") \r\n");
            
            #line 44 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
    }
            
            #line default
            #line hidden
            this.Write("                    .OrderBy(ent=>ent.");
            
            #line 45 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.GetKeys(this.EntityType).FirstOrDefault()));
            
            #line default
            #line hidden
            this.Write(");\r\n        }\r\n\r\n        public override void Insert(");
            
            #line 48 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(EntityType.Name));
            
            #line default
            #line hidden
            this.Write(" entity) \r\n        {");
            
            #line 49 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetComputedCodeForInsert()));
            
            #line default
            #line hidden
            this.Write(@"
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(");
            
            #line 59 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(EntityType.Name));
            
            #line default
            #line hidden
            this.Write(" entity, bool refresh = true) \r\n        {");
            
            #line 60 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetComputedCodeForUpdate()));
            
            #line default
            #line hidden
            this.Write("\r\n            \r\n            if (ValidateEntity(entity).Count > 0) \r\n            {" +
                    "\r\n                throw new EntityValidationException { Errors = ValidateEntity(" +
                    "entity) };\r\n            }\r\n\r\n            base.Update(entity, refresh);\r\n        " +
                    "}\r\n\r\n        ");
            
            #line 70 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
    if(T4Helper.HasSingleKeyNumber(this.EntityType)){  
            
            #line default
            #line hidden
            this.Write("\r\n       \r\n        ");
            
            #line 73 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
    }  
            
            #line default
            #line hidden
            this.Write("\r\n\r\n        public IQueryable<");
            
            #line 76 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(EntityType.Name));
            
            #line default
            #line hidden
            this.Write("> Filter");
            
            #line 76 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(EntityType.Name));
            
            #line default
            #line hidden
            this.Write("s(IQueryable<");
            
            #line 76 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(EntityType.Name));
            
            #line default
            #line hidden
            this.Write("> ");
            
            #line 76 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.GetLocalVariableName(EntityType)));
            
            #line default
            #line hidden
            this.Write("s, SearchArg searchArg)\r\n        {\r\n            var searchWord = searchArg.Search" +
                    ".ToLower();\r\n            var filtered");
            
            #line 79 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(EntityType.Name));
            
            #line default
            #line hidden
            this.Write(" = ");
            
            #line 79 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.GetLocalVariableName(EntityType)));
            
            #line default
            #line hidden
            this.Write("s.Where(ent => \r\n");
            
            #line 80 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
    var useOr = false;
            
            #line default
            #line hidden
            
            #line 81 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
    foreach(var pm in T4Helper.ListPropertiesWithFkInfo(EntityType))
            
            #line default
            #line hidden
            
            #line 82 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
    {
            
            #line default
            #line hidden
            
            #line 83 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
       
 if(pm.FK && !ToBeIgnored(pm.PareEntityType.Properties.First()))        
            
            #line default
            #line hidden
            
            #line 85 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
        {
            
            #line default
            #line hidden
            
            #line 86 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            if(useOr) {
            
            #line default
            #line hidden
            this.Write("                    || ");
            
            #line 86 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            }else{
            
            #line default
            #line hidden
            this.Write("                    ");
            
            #line 86 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            }
            
            #line default
            #line hidden
            this.Write("(!string.IsNullOrEmpty(ent.");
            
            #line 87 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(pm.PareEntityType.Name));
            
            #line default
            #line hidden
            this.Write(".");
            
            #line 87 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.PropertyOperations.GetDispayPropertyName(pm.PareEntityType)));
            
            #line default
            #line hidden
            this.Write(") && ent.");
            
            #line 87 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(pm.PareEntityType.Name));
            
            #line default
            #line hidden
            this.Write(".");
            
            #line 87 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.PropertyOperations.GetDispayPropertyName(pm.PareEntityType)));
            
            #line default
            #line hidden
            this.Write(".ToLower().Contains(searchWord))\r\n");
            
            #line 88 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            useOr=true;
            
            #line default
            #line hidden
            
            #line 89 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            continue;
            
            #line default
            #line hidden
            
            #line 90 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
        }
            
            #line default
            #line hidden
            
            #line 91 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
        

if(T4Helper.PropertyOperations.IsBinary(pm.Property) || T4Helper.PropertyOperations.IsTimeSpan(pm.Property) || T4Helper.PropertyOperations.IsNullableTimeSpan(pm.Property) ){
	continue;
}

if(!ToBeIgnored(pm.Property))
            
            #line default
            #line hidden
            
            #line 98 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
        {
            
            #line default
            #line hidden
            
            #line 99 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            if(T4Helper.PropertyOperations.IsInteger(pm.Property))
            
            #line default
            #line hidden
            
            #line 100 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            {
            
            #line default
            #line hidden
            
            #line 101 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
                if(useOr) {
            
            #line default
            #line hidden
            this.Write("                    || ");
            
            #line 101 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
                }else{
            
            #line default
            #line hidden
            this.Write("                    ");
            
            #line 101 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
                }
            
            #line default
            #line hidden
            this.Write("ent.");
            
            #line 102 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(pm.Property.Name));
            
            #line default
            #line hidden
            this.Write(".ToString().Contains(searchWord)\r\n");
            
            #line 103 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
                useOr=true;
            
            #line default
            #line hidden
            
            #line 104 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
                continue;
            
            #line default
            #line hidden
            
            #line 105 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            }
            
            #line default
            #line hidden
            
            #line 106 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            if(T4Helper.PropertyOperations.IsNumber(pm.Property))
            
            #line default
            #line hidden
            
            #line 107 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            {
            
            #line default
            #line hidden
            
            #line 108 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
                if(useOr) {
            
            #line default
            #line hidden
            this.Write("                    || ");
            
            #line 108 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
                }else{
            
            #line default
            #line hidden
            this.Write("                    ");
            
            #line 108 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
                }
            
            #line default
            #line hidden
            this.Write("ent.");
            
            #line 109 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(pm.Property.Name));
            
            #line default
            #line hidden
            this.Write(".ToString().Contains(searchWord)\r\n");
            
            #line 110 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
                useOr=true;
            
            #line default
            #line hidden
            
            #line 111 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
                continue;
            
            #line default
            #line hidden
            
            #line 112 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            }
            
            #line default
            #line hidden
            
            #line 113 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            if(useOr) {
            
            #line default
            #line hidden
            this.Write("                    || ");
            
            #line 113 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            }else{
            
            #line default
            #line hidden
            this.Write("                    ");
            
            #line 113 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            }
            
            #line default
            #line hidden
            this.Write("(!string.IsNullOrEmpty(ent.");
            
            #line 114 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(pm.Property.Name));
            
            #line default
            #line hidden
            this.Write(") && ent.");
            
            #line 114 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(pm.Property.Name));
            
            #line default
            #line hidden
            this.Write(".ToLower().Contains(searchWord))\r\n");
            
            #line 115 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            useOr=true;
            
            #line default
            #line hidden
            
            #line 116 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            continue;
            
            #line default
            #line hidden
            
            #line 117 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
        }
            
            #line default
            #line hidden
            
            #line 118 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
    }
            
            #line default
            #line hidden
            this.Write(");\r\n\r\n            return filtered");
            
            #line 120 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(EntityType.Name));
            
            #line default
            #line hidden
            this.Write(".OrderBy(e => e.");
            
            #line 120 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.GetKeys(this.EntityType).FirstOrDefault()));
            
            #line default
            #line hidden
            this.Write(");\r\n        }\r\n    }\r\n}\r\n\r\n\r\n\r\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 127 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
    
    string GetComputedCodeForInsert(){

        return T4Helper.GetComputedCodeForInsert(this.EntityType,this.EntityContext);
    }


        
        #line default
        #line hidden
        
        #line 136 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"
    
    string GetComputedCodeForUpdate(){

        return T4Helper.GetComputedCodeForUpdate(this.EntityType,this.EntityContext);
    }


        
        #line default
        #line hidden
        
        #line 144 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"

    bool ToBeIgnored(EdmProperty property){
        if (T4Helper.PropertyOperations.IsCheckbox(property))return true;
        if(T4Helper.PropertyOperations.IsDate(property)) return true;
        if(T4Helper.PropertyOperations.IsDateTime(property)) return true;
        if(property.Name.ToLower().Contains("lastmodified")) return true;
        if(property.Name.ToLower().Contains("statusid")) return true;

        return false;
    }

        
        #line default
        #line hidden
        
        #line 1 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\BL\SingleRepositoryTemplate.tt"

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
