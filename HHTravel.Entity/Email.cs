//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace HHTravel.Entity
{
    public partial class Email
    {
        public int EmailId { get; set; }
        public string EmailType { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string Cc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool SendState { get; set; }
        public short SendTimes { get; set; }
        public Nullable<System.DateTime> SendTime { get; set; }
        public string FailInfo { get; set; }
        public Nullable<int> OrderID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
    
}
