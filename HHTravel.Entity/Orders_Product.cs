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
    public partial class Orders_Product
    {
        public int ItemId { get; set; }
        public int OrderId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Nullable<int> ProductSpecId { get; set; }
        public Nullable<short> ProductState { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
        public Nullable<short> Times { get; set; }
        public Nullable<int> Amount { get; set; }
        public string LastModifyMan { get; set; }
        public Nullable<System.DateTime> LastModifyTime { get; set; }
        public string ProductName { get; set; }
        public string ProductSpecName { get; set; }
        public string SectionType { get; set; }
        public Nullable<System.DateTime> DepartDate { get; set; }
        public string Unit { get; set; }
        public Nullable<short> Quantity { get; set; }
        public Nullable<int> Price { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsStay { get; set; }
        public Nullable<System.DateTime> DepartureTime { get; set; }
        public Nullable<short> AdultNum { get; set; }
        public Nullable<short> ChildNum { get; set; }
        public Nullable<int> PriceAdult { get; set; }
        public Nullable<int> PriceChild { get; set; }
    }
    
}
