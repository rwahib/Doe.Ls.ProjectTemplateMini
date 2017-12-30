﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 15.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Doe.Ls.ProjectTemplate.Data.T4Templates.Scripts
{
    using Doe.Ls.EntityBase.Helper;
    using Doe.Ls.EntityBase.MVCExtensions;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\Scripts\DataTableServiceTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "15.0.0.0")]
    public partial class DataTableServiceTemplate : MVCTemplate.BaseTemplate
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write(" \r\n");
            this.Write(" \r\n");
            this.Write("\r\n");
            
            #line 12 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\Scripts\DataTableServiceTemplate.tt"
    
    var helper=new EdmMetadataHelper(EntityContext);

            
            #line default
            #line hidden
            this.Write(@"
/// <reference path=""_references.js"" />

define(['jquery', 'cnt', 'helper', 'interactive', 'api', 'fValidator'], function ($, cnt, helper, interactive, api) {

    return {
        initialise: function(dataTable){            

            dataTable.each(function () {
                var ajaxTable = $(this);
                var serviceType = ajaxTable.attr('data-service');
                
                var ajaxUrl = ajaxTable.attr('data-url');
                
                if (!helper.hasValue(ajaxUrl)) {
                    ajaxUrl = '';
                }

                applyDataTableSettings(serviceType, function (dataTableSettings) {
                    var generalSettings = getGeneralDataTableSettings(ajaxTable, ajaxUrl);
                    var newSettings = $.extend(generalSettings, dataTableSettings);
                    interactive.applyNewDataTableSettings(newSettings, ajaxTable);
                });

            });
        }
    }

    function applyDataTableSettings(serviceType, callBack){
        switch(serviceType){
");
            
            #line 45 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\Scripts\DataTableServiceTemplate.tt"
    foreach(var entityType in helper.GetEntityList())
            
            #line default
            #line hidden
            
            #line 46 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\Scripts\DataTableServiceTemplate.tt"
    { 
            
            #line default
            #line hidden
            
            #line 47 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\Scripts\DataTableServiceTemplate.tt"
        if(entityType.Name.ToLower().Contains("sysdia"))continue; 
            
            #line default
            #line hidden
            this.Write("\r\n            case \'");
            
            #line 49 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\Scripts\DataTableServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.GetLocalVariableName(entityType)));
            
            #line default
            #line hidden
            this.Write("Service\':\r\n                require([\'");
            
            #line 50 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\Scripts\DataTableServiceTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(T4Helper.GetLocalVariableName(entityType)));
            
            #line default
            #line hidden
            this.Write("Service\'], function(service){\r\n                    if(helper.hasValue(callBack)){" +
                    "\r\n                        callBack(service.dtSettings);\r\n                    }\r\n" +
                    "                });\r\n                break;\r\n");
            
            #line 56 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\Scripts\DataTableServiceTemplate.tt"
    }
            
            #line default
            #line hidden
            this.Write(@"            default: 
                    break;
        }
    }

    function getGeneralDataTableSettings(dataTable, ajaxUrl){
        return {
            'language': {
                'lengthMenu': ' _MENU_ records per page',
                'loadingRecords': 'Please wait - loading...'
            },
            'bProcessing': true,
            'bServerSide': true,
            'bPaginate': true,
            'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
            'nPaginationType': 'fullNumbers',
            'sAjaxSource': ajaxUrl,
            'initComplete': function () {
                //only exec below for IE as search functionality not working in IE9
                if (navigator.userAgent.indexOf(""MSIE"") > 0) {
                    var datatableApi = this.api();
                    $('.dataTables_filter input').unbind('keyup').bind('keyup', function (e) {
                        datatableApi.search($(this).val()).draw();
                    });
                }
                $(dataTable).show();
            }
        };
    }
});");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 1 "C:\Projects\Doe.Ls.ProjectTemplateMini\Doe.Ls.ProjectTemplate.Data\T4Templates\Scripts\DataTableServiceTemplate.tt"

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