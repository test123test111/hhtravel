using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace CtripServices
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]

    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "CtripWSSoap", Namespace = "http://tempuri.org/")]
    public partial class CtripServicesAgent : System.Web.Services.Protocols.SoapHttpClientProtocol
    {
        private System.Threading.SendOrPostCallback RequestOperationCompleted;

        private System.Threading.SendOrPostCallback CommRequestOperationCompleted;

        /// <remarks/>
        public CtripServicesAgent(string url)
        {
            this.Url = url;
        }

        /// <remarks/>
        public event RequestCompletedEventHandler RequestCompleted;

        /// <remarks/>
        public event CommRequestCompletedEventHandler CommRequestCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/Request", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string Request(string requestXML)
        {
            object[] results = this.Invoke("Request", new object[] {
                    requestXML});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginRequest(string requestXML, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("Request", new object[] {
                    requestXML}, callback, asyncState);
        }

        /// <remarks/>
        public string EndRequest(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void RequestAsync(string requestXML)
        {
            this.RequestAsync(requestXML, null);
        }

        /// <remarks/>
        public void RequestAsync(string requestXML, object userState)
        {
            if ((this.RequestOperationCompleted == null))
            {
                this.RequestOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRequestOperationCompleted);
            }
            this.InvokeAsync("Request", new object[] {
                    requestXML}, this.RequestOperationCompleted, userState);
        }

        private void OnRequestOperationCompleted(object arg)
        {
            if ((this.RequestCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RequestCompleted(this, new RequestCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/CommRequest", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string CommRequest(string requestXML)
        {
            object[] results = this.Invoke("CommRequest", new object[] {
                    requestXML});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginCommRequest(string requestXML, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("CommRequest", new object[] {
                    requestXML}, callback, asyncState);
        }

        /// <remarks/>
        public string EndCommRequest(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void CommRequestAsync(string requestXML)
        {
            this.CommRequestAsync(requestXML, null);
        }

        /// <remarks/>
        public void CommRequestAsync(string requestXML, object userState)
        {
            if ((this.CommRequestOperationCompleted == null))
            {
                this.CommRequestOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCommRequestOperationCompleted);
            }
            this.InvokeAsync("CommRequest", new object[] {
                    requestXML}, this.CommRequestOperationCompleted, userState);
        }

        private void OnCommRequestOperationCompleted(object arg)
        {
            if ((this.CommRequestCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CommRequestCompleted(this, new CommRequestCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void RequestCompletedEventHandler(object sender, RequestCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RequestCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        private object[] results;

        internal RequestCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void CommRequestCompletedEventHandler(object sender, CommRequestCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CommRequestCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        private object[] results;

        internal CommRequestCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}