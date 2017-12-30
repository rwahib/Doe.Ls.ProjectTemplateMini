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
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewEditTemplate_Modal.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "15.0.0.0")]
    public partial class SingleViewEditTemplate_Modal : BaseTemplate
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write(" \r\n");
            
            #line 11 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewEditTemplate_Modal.tt"
 T4Helper.SetAssemblyClassNameFormatFromContext(EntityContext); 
            
            #line default
            #line hidden
            
            #line 12 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewEditTemplate_Modal.tt"
    var helper=new EdmMetadataHelper(EntityContext);
            
            #line default
            #line hidden
            this.Write("\r\n@model ");
            
            #line 14 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewEditTemplate_Modal.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(EntityType.Name));
            
            #line default
            #line hidden
            this.Write("\r\n@{\r\n    var errors = ViewBagWrapper.ErrorBag.GetErrors(ViewData);\r\n");
            
            #line 17 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewEditTemplate_Modal.tt"
    foreach (EntityType lookupEntity in T4Helper.GetLookupEntities(EntityType))
            
            #line default
            #line hidden
            
            #line 18 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewEditTemplate_Modal.tt"
    {
            
            #line default
            #line hidden
            this.Write("    var ");
            
            #line 19 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewEditTemplate_Modal.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.GetLocalVariableName(lookupEntity)));
            
            #line default
            #line hidden
            this.Write("Items = ViewBagWrapper.ListBag.GetList(\"");
            
            #line 19 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewEditTemplate_Modal.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.CleanClassName(lookupEntity.Name)));
            
            #line default
            #line hidden
            this.Write("Items\",ViewData);\r\n");
            
            #line 20 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewEditTemplate_Modal.tt"
    } 
            
            #line default
            #line hidden
            this.Write(" \r\n    ViewBagWrapper.InfoBag.SetTitle(\"Edit ");
            
            #line 21 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewEditTemplate_Modal.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.Wordify(T4Helper.CleanClassName(EntityType.Name))));
            
            #line default
            #line hidden
            this.Write(@""",ViewData);
    var user = ViewBagWrapper.UserInfoExtensionBag.GetCurrentUser(ViewData);
    var task = ViewBagWrapper.TaskBag.GetCurrentTask(ViewData);
    Layout = null;
}

<div class=""modal-header"">
    <h5 class=""modal-title"" id=""tempModalLabel"">@ViewBagWrapper.InfoBag.GetTitle(ViewData)</h5>
    <button type=""button"" class=""close"" data-dismiss=""modal"" aria-label=""Close"">
        <span aria-hidden=""true"">&#215;</span>
    </button>
</div>

<div class=""error-popup"">
    <ul class=""list-group"">
        @foreach (var error in errors)
        {
            <li class=""list-group-item "">
                <label class=""validation-error"">@string.Format(""{0} has error:"", error.PropertyName.Wordify())</label>
                <label class=""validation-error"">@string.Format(""{0}"", error.ErrorMessage)</label>            
            </li>
        }
    </ul>
</div>

<div class=""success-popup"">
    <ul class=""list-group"">
        <li class=""list-group-item "">
            <label class=""text-success"" id=""lbl-success""></label>
        </li>
    </ul>
</div>

<div id=""formTab"">
    <form class="""" role=""form"" action=""@Url.Action(""Edit"")"" @*");
            
            #line 55 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewEditTemplate_Modal.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.GetFormAction(EntityType,FormType.Edit)));
            
            #line default
            #line hidden
            this.Write("*@   method=\"POST\" id=");
            
            #line 55 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewEditTemplate_Modal.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.GetFormId(EntityType,FormType.Edit)));
            
            #line default
            #line hidden
            this.Write(@" data-fv-feedbackicons-validating=""glyphicon glyphicon-refresh"">
    
        <div class=""modal-body"">
            @Html.Partial(""_partial/_update"")         
        </div>
    
        <div class=""modal-footer"">
            <button type=""submit"" class=""btn btn-primary"">Save</button>
            <button type=""button"" class=""btn btn-secondary"" data-dismiss=""modal"">Cancel</button>
        </div>
    </form>
</div>");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 1 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\MVCTemplate\SingleViewEditTemplate_Modal.tt"

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
