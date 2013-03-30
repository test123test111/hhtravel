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
    public partial class Orders
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<System.TimeSpan> OrderTime { get; set; }
        public Nullable<short> OrderState { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<System.DateTime> DepartureTime { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
        public Nullable<System.DateTime> StayReturnDate { get; set; }
        public Nullable<short> TravelDays { get; set; }
        public Nullable<short> AdultNum { get; set; }
        public Nullable<short> ChildNum { get; set; }
        public Nullable<short> PersonNum { get; set; }
        public Nullable<int> Amount { get; set; }
        public Nullable<bool> IsStayHotel { get; set; }
        public Nullable<bool> AirTicketOneself { get; set; }
        public string SpecialMemento { get; set; }
        public string SpecialHope { get; set; }
        public string RequestFrom { get; set; }
        public Nullable<short> Company { get; set; }
        public string LastModifyMan { get; set; }
        public Nullable<System.DateTime> LastModifyTime { get; set; }
        public Nullable<short> TripType { get; set; }
        public string DepartureCity { get; set; }
    }
    
}