﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 15.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Doe.Ls.ProjectTemplate.Data.T4Templates._archived
{
    using Doe.Ls.EntityBase.Helper;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "15.0.0.0")]
    public partial class temp_1_single_view_partial_details : MVCTemplate.BaseTemplate
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write(" \r\n");
            
            #line 9 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
 T4Helper.SetAssemblyClassNameFormatFromContext(EntityContext); 
            
            #line default
            #line hidden
            
            #line 11 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
    var helper=new EdmMetadataHelper(EntityContext);


            
            #line default
            #line hidden
            this.Write("@model ");
            
            #line 14 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(EntityType.Name));
            
            #line default
            #line hidden
            this.Write(" \r\n@{\r\n    ViewBagWrapper.InfoBag.SetTitle(\"");
            
            #line 16 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.Wordify(T4Helper.CleanClassName(EntityType.Name))));
            
            #line default
            #line hidden
            this.Write(" Details\",ViewData);\r\n    var user = ViewBagWrapper.UserInfoExtensionBag.GetCurre" +
                    "ntUser(ViewData);\r\n    var task = ViewBagWrapper.TaskBag.GetCurrentTask(ViewData" +
                    ");\t\r\n}\r\n");
            
            #line 20 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
    foreach (var property in T4Helper.ListPropertiesWithFkInfo(EntityType)) 
            
            #line default
            #line hidden
            
            #line 21 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
    {
    if (property == null) continue;
 
            
            #line default
            #line hidden
            
            #line 24 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
        if(T4Helper.PropertyOperations.IsKey(property.Property,EntityType))
            
            #line default
            #line hidden
            
            #line 25 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
        {
            
            #line default
            #line hidden
            this.Write(" \r\n@Html.HiddenFor(model => model.");
            
            #line 26 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Property.Name));
            
            #line default
            #line hidden
            this.Write(")\r\n");
            
            #line 27 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
        }
            
            #line default
            #line hidden
            this.Write("<div class=\"form-group\">\r\n    ");
            
            #line 29 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
if (property.FK){ 
            
            #line default
            #line hidden
            this.Write("\t    <span class=\"col-lg-4 col-form-label\">@Html.DisplayNameFor(model => model.");
            
            #line 30 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.PareEntityType.Name));
            
            #line default
            #line hidden
            this.Write(")</span>\r\n");
            
            #line 31 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
            }else{ 
            
            #line default
            #line hidden
            this.Write("    <span class=\"col-lg-4 col-form-label\">@Html.DisplayNameFor(model => model.");
            
            #line 32 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(property.Property.Name));
            
            #line default
            #line hidden
            this.Write(")</span>\r\n    ");
            
            #line 33 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
} 
            
            #line default
            #line hidden
            this.Write("   \r\n    ");
            
            #line 34 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
    GenerateDisplayValueForProperty(property); 
            
            #line default
            #line hidden
            this.Write("</div>\r\n");
            
            #line 36 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
 } 
            
            #line default
            #line hidden
            return this.GenerationEnvironment.ToString();
        }
        
        #line 37 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"

    void GenerateDisplayValueForProperty(PropMeta property)
    {
        if (!property.FK && !T4Helper.PropertyOperations.IsRichText(property.Property, this.EntityType))
        {
        
        
        #line default
        #line hidden
        
        #line 42 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
this.Write("    <div class=\"col-lg-8\"><p class=\"form-control-plaintext\">@Html.DisplayFor(mode" +
        "l => model.");

        
        #line default
        #line hidden
        
        #line 43 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(property.Property.Name));

        
        #line default
        #line hidden
        
        #line 43 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
this.Write(")</p></div>\r\n        ");

        
        #line default
        #line hidden
        
        #line 44 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
 
        }

        if (!property.FK && T4Helper.PropertyOperations.IsRichText(property.Property, this.EntityType))
        {
        
        
        #line default
        #line hidden
        
        #line 49 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
this.Write("    <div class=\"col-lg-8\"><p class=\"form-control-plaintext\">@Html.Raw(Model.");

        
        #line default
        #line hidden
        
        #line 50 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(property.Property.Name));

        
        #line default
        #line hidden
        
        #line 50 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
this.Write(")</p></div>\r\n        ");

        
        #line default
        #line hidden
        
        #line 51 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
 
        }


                if (property.FK && !T4Helper.PropertyOperations.IsRichText(property.FkProperty, this.EntityType))
        {
        
        
        #line default
        #line hidden
        
        #line 57 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
this.Write("    <div class=\"col-lg-8\"><p class=\"form-control-plaintext\">@Html.DisplayFor(mode" +
        "l => model.");

        
        #line default
        #line hidden
        
        #line 58 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(property.PareEntityType.Name));

        
        #line default
        #line hidden
        
        #line 58 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
this.Write(".");

        
        #line default
        #line hidden
        
        #line 58 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.MetadataOperations.GetDispayPropertyName(property.PareEntityType)));

        
        #line default
        #line hidden
        
        #line 58 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
this.Write(")</p>\r\n    </div>\r\n        ");

        
        #line default
        #line hidden
        
        #line 60 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
 
        }


         if (property.FK && T4Helper.PropertyOperations.IsRichText(property.FkProperty, this.EntityType))
        {
        
        
        #line default
        #line hidden
        
        #line 66 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
this.Write("    <div class=\"col-lg-8\"><p class=\"form-control-plaintext\">@Html.Raw(Model.");

        
        #line default
        #line hidden
        
        #line 67 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(property.PareEntityType.Name));

        
        #line default
        #line hidden
        
        #line 67 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
this.Write(".");

        
        #line default
        #line hidden
        
        #line 67 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.MetadataOperations.GetDispayPropertyName(property.PareEntityType)));

        
        #line default
        #line hidden
        
        #line 67 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
this.Write(")</p></div>\r\n        ");

        
        #line default
        #line hidden
        
        #line 68 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"
 
        }

    }
    
        
        #line default
        #line hidden
        
        #line 1 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\_archived\temp_1_single_view_partial_details.tt"

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
