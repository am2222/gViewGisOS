﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:2.0.50727.6111
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Dieser Quellcode wurde automatisch generiert von wsdl, Version=2.0.50727.3038.
// 
namespace gView.MapServer.Tasker.InstanceConnector
{
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    // CODEGEN: Das optionale WSDL-Erweiterungselement 'method' von Namespace 'http://www.w3.org/2000/wsdl/suds' wurde nicht behandelt.
    // CODEGEN: Das optionale WSDL-Erweiterungselement 'method' von Namespace 'http://www.w3.org/2000/wsdl/suds' wurde nicht behandelt.
    // CODEGEN: Das optionale WSDL-Erweiterungselement 'method' von Namespace 'http://www.w3.org/2000/wsdl/suds' wurde nicht behandelt.
    // CODEGEN: Das optionale WSDL-Erweiterungselement 'method' von Namespace 'http://www.w3.org/2000/wsdl/suds' wurde nicht behandelt.
    // CODEGEN: Das optionale WSDL-Erweiterungselement 'method' von Namespace 'http://www.w3.org/2000/wsdl/suds' wurde nicht behandelt.
    // CODEGEN: Das optionale WSDL-Erweiterungselement 'method' von Namespace 'http://www.w3.org/2000/wsdl/suds' wurde nicht behandelt.
    // CODEGEN: Das optionale WSDL-Erweiterungselement 'method' von Namespace 'http://www.w3.org/2000/wsdl/suds' wurde nicht behandelt.
    // CODEGEN: Das optionale WSDL-Erweiterungselement 'method' von Namespace 'http://www.w3.org/2000/wsdl/suds' wurde nicht behandelt.
    // CODEGEN: Das optionale WSDL-Erweiterungselement 'class' von Namespace 'http://www.w3.org/2000/wsdl/suds' wurde nicht behandelt.
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="MapServerInstanceTypeBinding", Namespace="http://schemas.microsoft.com/clr/nsassem/gView.MapServer.Instance/gView.MapServer" +
        ".Instance%2C%20Version%3D4.0.0.0%2C%20Culture%3Dneutral%2C%20PublicKeyToken%3Dc4" +
        "39ceeb8b2eb808")]
    public partial class MapServerInstanceTypeService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ServiceRequestOperationCompleted;
        
        private System.Threading.SendOrPostCallback ServiceRequest2OperationCompleted;
        
        private System.Threading.SendOrPostCallback AddMapOperationCompleted;
        
        private System.Threading.SendOrPostCallback RemoveMapOperationCompleted;
        
        private System.Threading.SendOrPostCallback PingOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetMetadataOperationCompleted;
        
        private System.Threading.SendOrPostCallback SetMetadataOperationCompleted;
        
        private System.Threading.SendOrPostCallback ReloadMapOperationCompleted;
        
        /// <remarks/>
        public MapServerInstanceTypeService(string url) {
            if (url.ToLower().EndsWith("/mapserver"))
                this.Url = url;
            else
                this.Url = "http://" + url + "/MapServer";
        }
        
        /// <remarks/>
        public event ServiceRequestCompletedEventHandler ServiceRequestCompleted;
        
        /// <remarks/>
        public event ServiceRequest2CompletedEventHandler ServiceRequest2Completed;
        
        /// <remarks/>
        public event AddMapCompletedEventHandler AddMapCompleted;
        
        /// <remarks/>
        public event RemoveMapCompletedEventHandler RemoveMapCompleted;
        
        /// <remarks/>
        public event PingCompletedEventHandler PingCompleted;
        
        /// <remarks/>
        public event GetMetadataCompletedEventHandler GetMetadataCompleted;
        
        /// <remarks/>
        public event SetMetadataCompletedEventHandler SetMetadataCompleted;
        
        /// <remarks/>
        public event ReloadMapCompletedEventHandler ReloadMapCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://schemas.microsoft.com/clr/nsassem/gView.MapServer.Instance.MapServerInstan" +
            "ceType/gView.MapServer.Instance#ServiceRequest", RequestNamespace="http://schemas.microsoft.com/clr/nsassem/gView.MapServer.Instance.MapServerInstan" +
            "ceType/gView.MapServer.Instance", ResponseNamespace="http://schemas.microsoft.com/clr/nsassem/gView.MapServer.Instance.MapServerInstan" +
            "ceType/gView.MapServer.Instance")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public string ServiceRequest(string service, string request, string InterpreterGUID, string usr, string pwd) {
            object[] results = this.Invoke("ServiceRequest", new object[] {
                        service,
                        request,
                        InterpreterGUID,
                        usr,
                        pwd});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginServiceRequest(string service, string request, string InterpreterGUID, string usr, string pwd, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("ServiceRequest", new object[] {
                        service,
                        request,
                        InterpreterGUID,
                        usr,
                        pwd}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndServiceRequest(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ServiceRequestAsync(string service, string request, string InterpreterGUID, string usr, string pwd) {
            this.ServiceRequestAsync(service, request, InterpreterGUID, usr, pwd, null);
        }
        
        /// <remarks/>
        public void ServiceRequestAsync(string service, string request, string InterpreterGUID, string usr, string pwd, object userState) {
            if ((this.ServiceRequestOperationCompleted == null)) {
                this.ServiceRequestOperationCompleted = new System.Threading.SendOrPostCallback(this.OnServiceRequestOperationCompleted);
            }
            this.InvokeAsync("ServiceRequest", new object[] {
                        service,
                        request,
                        InterpreterGUID,
                        usr,
                        pwd}, this.ServiceRequestOperationCompleted, userState);
        }
        
        private void OnServiceRequestOperationCompleted(object arg) {
            if ((this.ServiceRequestCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ServiceRequestCompleted(this, new ServiceRequestCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://schemas.microsoft.com/clr/nsassem/gView.MapServer.Instance.MapServerInstan" +
            "ceType/gView.MapServer.Instance#ServiceRequest2", RequestNamespace="http://schemas.microsoft.com/clr/nsassem/gView.MapServer.Instance.MapServerInstan" +
            "ceType/gView.MapServer.Instance", ResponseNamespace="http://schemas.microsoft.com/clr/nsassem/gView.MapServer.Instance.MapServerInstan" +
            "ceType/gView.MapServer.Instance")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public string ServiceRequest2(string OnlineResource, string service, string request, string InterpreterGUID, string usr, string pwd) {
            object[] results = this.Invoke("ServiceRequest2", new object[] {
                        OnlineResource,
                        service,
                        request,
                        InterpreterGUID,
                        usr,
                        pwd});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginServiceRequest2(string OnlineResource, string service, string request, string InterpreterGUID, string usr, string pwd, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("ServiceRequest2", new object[] {
                        OnlineResource,
                        service,
                        request,
                        InterpreterGUID,
                        usr,
                        pwd}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndServiceRequest2(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ServiceRequest2Async(string OnlineResource, string service, string request, string InterpreterGUID, string usr, string pwd) {
            this.ServiceRequest2Async(OnlineResource, service, request, InterpreterGUID, usr, pwd, null);
        }
        
        /// <remarks/>
        public void ServiceRequest2Async(string OnlineResource, string service, string request, string InterpreterGUID, string usr, string pwd, object userState) {
            if ((this.ServiceRequest2OperationCompleted == null)) {
                this.ServiceRequest2OperationCompleted = new System.Threading.SendOrPostCallback(this.OnServiceRequest2OperationCompleted);
            }
            this.InvokeAsync("ServiceRequest2", new object[] {
                        OnlineResource,
                        service,
                        request,
                        InterpreterGUID,
                        usr,
                        pwd}, this.ServiceRequest2OperationCompleted, userState);
        }
        
        private void OnServiceRequest2OperationCompleted(object arg) {
            if ((this.ServiceRequest2Completed != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ServiceRequest2Completed(this, new ServiceRequest2CompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://schemas.microsoft.com/clr/nsassem/gView.MapServer.Instance.MapServerInstan" +
            "ceType/gView.MapServer.Instance#AddMap", RequestNamespace="http://schemas.microsoft.com/clr/nsassem/gView.MapServer.Instance.MapServerInstan" +
            "ceType/gView.MapServer.Instance", ResponseNamespace="http://schemas.microsoft.com/clr/nsassem/gView.MapServer.Instance.MapServerInstan" +
            "ceType/gView.MapServer.Instance")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public bool AddMap(string mapName, string MapXML, string usr, string pwd) {
            object[] results = this.Invoke("AddMap", new object[] {
                        mapName,
                        MapXML,
                        usr,
                        pwd});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginAddMap(string mapName, string MapXML, string usr, string pwd, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("AddMap", new object[] {
                        mapName,
                        MapXML,
                        usr,
                        pwd}, callback, asyncState);
        }
        
        /// <remarks/>
        public bool EndAddMap(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void AddMapAsync(string mapName, string MapXML, string usr, string pwd) {
            this.AddMapAsync(mapName, MapXML, usr, pwd, null);
        }
        
        /// <remarks/>
        public void AddMapAsync(string mapName, string MapXML, string usr, string pwd, object userState) {
            if ((this.AddMapOperationCompleted == null)) {
                this.AddMapOperationCompleted = new System.Threading.SendOrPostCallback(this.OnAddMapOperationCompleted);
            }
            this.InvokeAsync("AddMap", new object[] {
                        mapName,
                        MapXML,
                        usr,
                        pwd}, this.AddMapOperationCompleted, userState);
        }
        
        private void OnAddMapOperationCompleted(object arg) {
            if ((this.AddMapCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.AddMapCompleted(this, new AddMapCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://schemas.microsoft.com/clr/nsassem/gView.MapServer.Instance.MapServerInstan" +
            "ceType/gView.MapServer.Instance#RemoveMap", RequestNamespace="http://schemas.microsoft.com/clr/nsassem/gView.MapServer.Instance.MapServerInstan" +
            "ceType/gView.MapServer.Instance", ResponseNamespace="http://schemas.microsoft.com/clr/nsassem/gView.MapServer.Instance.MapServerInstan" +
            "ceType/gView.MapServer.Instance")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public bool RemoveMap(string mapName, string usr, string pwd) {
            object[] results = this.Invoke("RemoveMap", new object[] {
                        mapName,
                        usr,
                        pwd});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginRemoveMap(string mapName, string usr, string pwd, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("RemoveMap", new object[] {
                        mapName,
                        usr,
                        pwd}, callback, asyncState);
        }
        
        /// <remarks/>
        public bool EndRemoveMap(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void RemoveMapAsync(string mapName, string usr, string pwd) {
            this.RemoveMapAsync(mapName, usr, pwd, null);
        }
        
        /// <remarks/>
        public void RemoveMapAsync(string mapName, string usr, string pwd, object userState) {
            if ((this.RemoveMapOperationCompleted == null)) {
                this.RemoveMapOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRemoveMapOperationCompleted);
            }
            this.InvokeAsync("RemoveMap", new object[] {
                        mapName,
                        usr,
                        pwd}, this.RemoveMapOperationCompleted, userState);
        }
        
        private void OnRemoveMapOperationCompleted(object arg) {
            if ((this.RemoveMapCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RemoveMapCompleted(this, new RemoveMapCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://schemas.microsoft.com/clr/nsassem/gView.MapServer.Instance.MapServerInstan" +
            "ceType/gView.MapServer.Instance#Ping", RequestNamespace="http://schemas.microsoft.com/clr/nsassem/gView.MapServer.Instance.MapServerInstan" +
            "ceType/gView.MapServer.Instance", ResponseNamespace="http://schemas.microsoft.com/clr/nsassem/gView.MapServer.Instance.MapServerInstan" +
            "ceType/gView.MapServer.Instance")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public string Ping() {
            object[] results = this.Invoke("Ping", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginPing(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("Ping", new object[0], callback, asyncState);
        }
        
        /// <remarks/>
        public string EndPing(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void PingAsync() {
            this.PingAsync(null);
        }
        
        /// <remarks/>
        public void PingAsync(object userState) {
            if ((this.PingOperationCompleted == null)) {
                this.PingOperationCompleted = new System.Threading.SendOrPostCallback(this.OnPingOperationCompleted);
            }
            this.InvokeAsync("Ping", new object[0], this.PingOperationCompleted, userState);
        }
        
        private void OnPingOperationCompleted(object arg) {
            if ((this.PingCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.PingCompleted(this, new PingCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://schemas.microsoft.com/clr/nsassem/gView.MapServer.Instance.MapServerInstan" +
            "ceType/gView.MapServer.Instance#GetMetadata", RequestNamespace="http://schemas.microsoft.com/clr/nsassem/gView.MapServer.Instance.MapServerInstan" +
            "ceType/gView.MapServer.Instance", ResponseNamespace="http://schemas.microsoft.com/clr/nsassem/gView.MapServer.Instance.MapServerInstan" +
            "ceType/gView.MapServer.Instance")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public string GetMetadata(string mapName, string usr, string pwd) {
            object[] results = this.Invoke("GetMetadata", new object[] {
                        mapName,
                        usr,
                        pwd});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetMetadata(string mapName, string usr, string pwd, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetMetadata", new object[] {
                        mapName,
                        usr,
                        pwd}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndGetMetadata(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetMetadataAsync(string mapName, string usr, string pwd) {
            this.GetMetadataAsync(mapName, usr, pwd, null);
        }
        
        /// <remarks/>
        public void GetMetadataAsync(string mapName, string usr, string pwd, object userState) {
            if ((this.GetMetadataOperationCompleted == null)) {
                this.GetMetadataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetMetadataOperationCompleted);
            }
            this.InvokeAsync("GetMetadata", new object[] {
                        mapName,
                        usr,
                        pwd}, this.GetMetadataOperationCompleted, userState);
        }
        
        private void OnGetMetadataOperationCompleted(object arg) {
            if ((this.GetMetadataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetMetadataCompleted(this, new GetMetadataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://schemas.microsoft.com/clr/nsassem/gView.MapServer.Instance.MapServerInstan" +
            "ceType/gView.MapServer.Instance#SetMetadata", RequestNamespace="http://schemas.microsoft.com/clr/nsassem/gView.MapServer.Instance.MapServerInstan" +
            "ceType/gView.MapServer.Instance", ResponseNamespace="http://schemas.microsoft.com/clr/nsassem/gView.MapServer.Instance.MapServerInstan" +
            "ceType/gView.MapServer.Instance")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public bool SetMetadata(string mapName, string metadata, string usr, string pwd) {
            object[] results = this.Invoke("SetMetadata", new object[] {
                        mapName,
                        metadata,
                        usr,
                        pwd});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginSetMetadata(string mapName, string metadata, string usr, string pwd, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("SetMetadata", new object[] {
                        mapName,
                        metadata,
                        usr,
                        pwd}, callback, asyncState);
        }
        
        /// <remarks/>
        public bool EndSetMetadata(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void SetMetadataAsync(string mapName, string metadata, string usr, string pwd) {
            this.SetMetadataAsync(mapName, metadata, usr, pwd, null);
        }
        
        /// <remarks/>
        public void SetMetadataAsync(string mapName, string metadata, string usr, string pwd, object userState) {
            if ((this.SetMetadataOperationCompleted == null)) {
                this.SetMetadataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSetMetadataOperationCompleted);
            }
            this.InvokeAsync("SetMetadata", new object[] {
                        mapName,
                        metadata,
                        usr,
                        pwd}, this.SetMetadataOperationCompleted, userState);
        }
        
        private void OnSetMetadataOperationCompleted(object arg) {
            if ((this.SetMetadataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SetMetadataCompleted(this, new SetMetadataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://schemas.microsoft.com/clr/nsassem/gView.MapServer.Instance.MapServerInstan" +
            "ceType/gView.MapServer.Instance#ReloadMap", RequestNamespace="http://schemas.microsoft.com/clr/nsassem/gView.MapServer.Instance.MapServerInstan" +
            "ceType/gView.MapServer.Instance", ResponseNamespace="http://schemas.microsoft.com/clr/nsassem/gView.MapServer.Instance.MapServerInstan" +
            "ceType/gView.MapServer.Instance")]
        [return: System.Xml.Serialization.SoapElementAttribute("return")]
        public bool ReloadMap(string mapName, string usr, string pwd) {
            object[] results = this.Invoke("ReloadMap", new object[] {
                        mapName,
                        usr,
                        pwd});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginReloadMap(string mapName, string usr, string pwd, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("ReloadMap", new object[] {
                        mapName,
                        usr,
                        pwd}, callback, asyncState);
        }
        
        /// <remarks/>
        public bool EndReloadMap(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void ReloadMapAsync(string mapName, string usr, string pwd) {
            this.ReloadMapAsync(mapName, usr, pwd, null);
        }
        
        /// <remarks/>
        public void ReloadMapAsync(string mapName, string usr, string pwd, object userState) {
            if ((this.ReloadMapOperationCompleted == null)) {
                this.ReloadMapOperationCompleted = new System.Threading.SendOrPostCallback(this.OnReloadMapOperationCompleted);
            }
            this.InvokeAsync("ReloadMap", new object[] {
                        mapName,
                        usr,
                        pwd}, this.ReloadMapOperationCompleted, userState);
        }
        
        private void OnReloadMapOperationCompleted(object arg) {
            if ((this.ReloadMapCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ReloadMapCompleted(this, new ReloadMapCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    public delegate void ServiceRequestCompletedEventHandler(object sender, ServiceRequestCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ServiceRequestCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ServiceRequestCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    public delegate void ServiceRequest2CompletedEventHandler(object sender, ServiceRequest2CompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ServiceRequest2CompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ServiceRequest2CompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    public delegate void AddMapCompletedEventHandler(object sender, AddMapCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class AddMapCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal AddMapCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    public delegate void RemoveMapCompletedEventHandler(object sender, RemoveMapCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RemoveMapCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RemoveMapCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    public delegate void PingCompletedEventHandler(object sender, PingCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PingCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal PingCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    public delegate void GetMetadataCompletedEventHandler(object sender, GetMetadataCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetMetadataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetMetadataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    public delegate void SetMetadataCompletedEventHandler(object sender, SetMetadataCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SetMetadataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SetMetadataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    public delegate void ReloadMapCompletedEventHandler(object sender, ReloadMapCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ReloadMapCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ReloadMapCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
}
