﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace Doe.Ls.ProjectTemplate.Core.PartnerAppWs {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2053.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="PartnerAppSoapBinding", Namespace="http://impl.pa.det.nsw.edu.au")]
    public partial class PartnerAppService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback decryptOperationCompleted;
        
        private System.Threading.SendOrPostCallback encryptOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public PartnerAppService() {
            this.Url = global::Doe.Ls.ProjectTemplate.Core.Properties.Settings.Default.Doe_Ls_SchoolSportsUnit_Core_PartnerAppWs_PartnerAppService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event decryptCompletedEventHandler decryptCompleted;
        
        /// <remarks/>
        public event encryptCompletedEventHandler encryptCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://impl.pa.det.nsw.edu.au", ResponseNamespace="http://impl.pa.det.nsw.edu.au")]
        [return: System.Xml.Serialization.SoapElementAttribute("decryptReturn")]
        public string decrypt(string encryptedData) {
            object[] results = this.Invoke("decrypt", new object[] {
                        encryptedData});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void decryptAsync(string encryptedData) {
            this.decryptAsync(encryptedData, null);
        }
        
        /// <remarks/>
        public void decryptAsync(string encryptedData, object userState) {
            if ((this.decryptOperationCompleted == null)) {
                this.decryptOperationCompleted = new System.Threading.SendOrPostCallback(this.OndecryptOperationCompleted);
            }
            this.InvokeAsync("decrypt", new object[] {
                        encryptedData}, this.decryptOperationCompleted, userState);
        }
        
        private void OndecryptOperationCompleted(object arg) {
            if ((this.decryptCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.decryptCompleted(this, new decryptCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://impl.pa.det.nsw.edu.au", ResponseNamespace="http://impl.pa.det.nsw.edu.au")]
        [return: System.Xml.Serialization.SoapElementAttribute("encryptReturn")]
        public string encrypt(string clearData) {
            object[] results = this.Invoke("encrypt", new object[] {
                        clearData});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void encryptAsync(string clearData) {
            this.encryptAsync(clearData, null);
        }
        
        /// <remarks/>
        public void encryptAsync(string clearData, object userState) {
            if ((this.encryptOperationCompleted == null)) {
                this.encryptOperationCompleted = new System.Threading.SendOrPostCallback(this.OnencryptOperationCompleted);
            }
            this.InvokeAsync("encrypt", new object[] {
                        clearData}, this.encryptOperationCompleted, userState);
        }
        
        private void OnencryptOperationCompleted(object arg) {
            if ((this.encryptCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.encryptCompleted(this, new encryptCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2053.0")]
    public delegate void decryptCompletedEventHandler(object sender, decryptCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2053.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class decryptCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal decryptCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2053.0")]
    public delegate void encryptCompletedEventHandler(object sender, encryptCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2053.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class encryptCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal encryptCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591