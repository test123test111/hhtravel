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

namespace HHTravel.CRM.Booking_Online.Entity
{
    public partial class Product_Attach_Product
    {
        public int ItemId { get; set; }
        public int ProductId { get; set; }
        public int ItemProductId { get; set; }
        public Nullable<int> ItemProductSpecId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public Nullable<short> OptionType { get; set; }
        public string LastModifyMan { get; set; }
        public Nullable<System.DateTime> LastModifyTime { get; set; }
        public Nullable<short> SeqNo { get; set; }
    }
    
}