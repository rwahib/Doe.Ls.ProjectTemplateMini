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
    
    #line 1 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewDetailsTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "15.0.0.0")]
    public partial class SingleViewDetailsTemplate : BaseTemplate
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write(" \r\n");
            this.Write("\r\n");
            this.Write("\r\n");
            this.Write("\r\n");
            
            #line 13 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewDetailsTemplate.tt"
 T4Helper.SetAssemblyClassNameFormatFromContext(EntityContext); 
            
            #line default
            #line hidden
            this.Write("\r\n");
            
            #line 15 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewDetailsTemplate.tt"
    

var helper=new EdmMetadataHelper(EntityContext);
    

            
            #line default
            #line hidden
            this.Write(" \r\n\r\n@model ");
            
            #line 21 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewDetailsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(EntityType.Name));
            
            #line default
            #line hidden
            this.Write("\r\n@{\r\n    ViewBagWrapper.InfoBag.SetTitle(\"Detail ");
            
            #line 23 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewDetailsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.Wordify(T4Helper.CleanClassName(EntityType.Name))));
            
            #line default
            #line hidden
            this.Write(@""",ViewData);
    var user = ViewBagWrapper.UserInfoExtensionBag.GetCurrentUser(ViewData);
    var task = ViewBagWrapper.TaskBag.GetCurrentTask(ViewData);
}

<div class=""page-header"">
    <h2>@ViewBagWrapper.InfoBag.GetTitle(ViewData)</h2>
</div>

<div id=""formTab"" class=""card.card-body"">
    <form class="""" role=""form"" method=""POST"" id=");
            
            #line 33 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewDetailsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.GetFormId(EntityType,FormType.Details)));
            
            #line default
            #line hidden
            this.Write(">\r\n        @Html.AntiForgeryToken()\r\n        <p>\r\n            <a href=\"@Url.Actio" +
                    "n(\"Edit\", new { ");
            
            #line 36 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewDetailsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetKeyName()));
            
            #line default
            #line hidden
            this.Write("  = Model.");
            
            #line 36 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewDetailsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.GetSingleKeyProperty(EntityType).Name));
            
            #line default
            #line hidden
            this.Write(" })\" class=\"btn btn-primary\">Edit</a>\r\n            <a href=\"@Url.Action(\"Index\")\"" +
                    " class=\"btn btn-primary\">");
            
            #line 37 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewDetailsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.CleanClassName(EntityType.Name)));
            
            #line default
            #line hidden
            this.Write(" List <span class=\"glyphicon glyphicon-th-list\"></span></a>\r\n        </p>\r\n    \r\n" +
                    "        @Html.Partial(\"_partial/_details\")\r\n        <p>\r\n            <a href=\"@U" +
                    "rl.Action(\"Edit\", new { ");
            
            #line 42 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewDetailsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(GetKeyName()));
            
            #line default
            #line hidden
            this.Write("  = Model.");
            
            #line 42 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewDetailsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.GetSingleKeyProperty(EntityType).Name));
            
            #line default
            #line hidden
            this.Write(" })\" class=\"btn btn-primary\">Edit</a>\r\n            <a href=\"@Url.Action(\"Index\")\"" +
                    " class=\"btn btn-primary\">");
            
            #line 43 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewDetailsTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.CleanClassName(EntityType.Name)));
            
            #line default
            #line hidden
            this.Write(" List <span class=\"glyphicon glyphicon-th-list\"></span></a>\r\n        </p>\r\n    </" +
                    "form>\r\n</div>\r\n\r\n");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 48 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewDetailsTemplate.tt"
 string GetKeyName()
{
    return T4Helper.PropertyOperations.GetKeyLocalVariableName(EntityType);

} 
        
        #line default
        #line hidden
        
        #line 1 "C:\Users\det\Source\Repos\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewDetailsTemplate.tt"

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
