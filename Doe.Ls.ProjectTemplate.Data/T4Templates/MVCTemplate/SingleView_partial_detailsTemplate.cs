﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 15.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Doe.Ls.ProjectTemplate.Data.T4Templates.MVCTemplate
{
    using Doe.Ls.EntityBase.Helper;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "15.0.0.0")]
    public partial class SingleView_partial_detailsTemplate : BaseTemplate
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write(" \r\n");
            
            #line 9 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
 T4Helper.SetAssemblyClassNameFormatFromContext(EntityContext); 
            
            #line default
            #line hidden
            
            #line 11 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
    var helper=new EdmMetadataHelper(EntityContext);


            
            #line default
            #line hidden
            this.Write("@model ");
            
            #line 14 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(EntityType.Name));
            
            #line default
            #line hidden
            this.Write(" \r\n@{\r\n    ViewBagWrapper.InfoBag.SetTitle(\"");
            
            #line 16 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.Wordify(T4Helper.CleanClassName(EntityType.Name))));
            
            #line default
            #line hidden
            this.Write(" Details\",ViewData);\r\n    var user = ViewBagWrapper.UserInfoExtensionBag.GetCurre" +
                    "ntUser(ViewData);\r\n    var task = ViewBagWrapper.TaskBag.GetCurrentTask(ViewData" +
                    ");\t\r\n}\r\n");
            
            #line 20 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
 foreach (var pm in T4Helper.ListPropertiesWithFkInfo(EntityType))  {   
if (pm == null) continue;
ClearIndent();

GenerateKeyTemplate(pm);

if(GenerateSimpletextTemplate(pm)){continue;} 
if(GenerateRichTextTemplate(pm)){continue;}
 }// foreach list of properties

            
            #line default
            #line hidden
            this.Write("  \r\n\r\n\r\n    \r\n\r\n\r\n\r\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 36 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
 
    //GenerateKeyTemplate
    private void GenerateKeyTemplate(PropMeta pm){    

        if(T4Helper.PropertyOperations.IsKey(pm.Property,EntityType)) {
 
        
        #line default
        #line hidden
        
        #line 41 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write("            @Html.HiddenFor(model => model.");

        
        #line default
        #line hidden
        
        #line 42 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(pm.Property.Name));

        
        #line default
        #line hidden
        
        #line 42 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write(")\r\n");

        
        #line default
        #line hidden
        
        #line 43 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
         
        }
        return;
    }

        
        #line default
        #line hidden
        
        #line 52 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
 
    //GenerateSimpletextTemplate
private bool GenerateSimpletextTemplate(PropMeta pm)
    {  var propName=pm.FK?pm.PareEntityType.Name:pm.Property.Name;  
if(!T4Helper.PropertyOperations.IsRichText(pm.Property, this.EntityType)) {
 
        
        #line default
        #line hidden
        
        #line 57 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write("<div class=\"form-group\">\r\n    <span class=\"col-lg-4 col-form-label\">@Html.Display" +
        "NameFor(model => model.");

        
        #line default
        #line hidden
        
        #line 59 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propName));

        
        #line default
        #line hidden
        
        #line 59 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write(")</span>\r\n        <div class=\"col-lg-8\">\r\n        <p class=\"form-control-plaintex" +
        "t\">");

        
        #line default
        #line hidden
        
        #line 61 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
 if(pm.FK){ 
        
        #line default
        #line hidden
        
        #line 61 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write("@Html.DisplayFor(model => model.");

        
        #line default
        #line hidden
        
        #line 61 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propName));

        
        #line default
        #line hidden
        
        #line 61 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write(".");

        
        #line default
        #line hidden
        
        #line 61 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.MetadataOperations.GetDispayPropertyName(pm.PareEntityType)));

        
        #line default
        #line hidden
        
        #line 61 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write(")\r\n                        ");

        
        #line default
        #line hidden
        
        #line 62 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
    }else{ 
        
        #line default
        #line hidden
        
        #line 62 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write("@Html.DisplayFor(model => model.");

        
        #line default
        #line hidden
        
        #line 62 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propName));

        
        #line default
        #line hidden
        
        #line 62 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write(")\r\n                        ");

        
        #line default
        #line hidden
        
        #line 63 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
    } 
        
        #line default
        #line hidden
        
        #line 63 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write("</p>\r\n       </div>\r\n</div>\r\n");

        
        #line default
        #line hidden
        
        #line 66 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
         
            return true;

}

        
        #line default
        #line hidden
        
        #line 71 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
     
    return false;
}  
// GenerateSimpletextTemplate   

        
        #line default
        #line hidden
        
        #line 79 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
 
    //GenerateRichTemplate
private bool GenerateRichTextTemplate(PropMeta pm)
    {  var propName=pm.FK?pm.PareEntityType.Name:pm.Property.Name;  
if(T4Helper.PropertyOperations.IsRichText(pm.Property, this.EntityType)) {
 
        
        #line default
        #line hidden
        
        #line 84 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write("<div class=\"form-group\">\r\n    <span class=\"col-lg-4 col-form-label\">@Html.Display" +
        "NameFor(model => model.");

        
        #line default
        #line hidden
        
        #line 86 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propName));

        
        #line default
        #line hidden
        
        #line 86 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write(")</span>\r\n        <div class=\"col-lg-8\">\r\n        <p class=\"form-control-plaintex" +
        "t\">");

        
        #line default
        #line hidden
        
        #line 88 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
 if(pm.FK){ 
        
        #line default
        #line hidden
        
        #line 88 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write("@Html.Raw(Model.");

        
        #line default
        #line hidden
        
        #line 88 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propName));

        
        #line default
        #line hidden
        
        #line 88 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write(".");

        
        #line default
        #line hidden
        
        #line 88 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.MetadataOperations.GetDispayPropertyName(pm.PareEntityType)));

        
        #line default
        #line hidden
        
        #line 88 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write(")\r\n                        ");

        
        #line default
        #line hidden
        
        #line 89 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
    }else{ 
        
        #line default
        #line hidden
        
        #line 89 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write("@Html.Raw(Model.");

        
        #line default
        #line hidden
        
        #line 89 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(propName));

        
        #line default
        #line hidden
        
        #line 89 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write(")\r\n                        ");

        
        #line default
        #line hidden
        
        #line 90 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
    } 
        
        #line default
        #line hidden
        
        #line 90 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
this.Write("</p>\r\n       </div>\r\n</div>\r\n");

        
        #line default
        #line hidden
        
        #line 93 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
         
            return true;

}

        
        #line default
        #line hidden
        
        #line 98 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"
     
    return false;
}  
// GenerateRichTemplate   

        
        #line default
        #line hidden
        
        #line 1 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleView_partial_detailsTemplate.tt"

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


/// <summary>
/// Initialize the template
/// </summary>
public override void Initialize()
{
    base.Initialize();
    if ((this.Errors.HasErrors == false))
    {
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


    }
}


        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
}
