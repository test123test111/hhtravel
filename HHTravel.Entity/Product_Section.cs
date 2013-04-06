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
    public partial class Product_Section
    {
        public int SectionId { get; set; }
        public int ProductId { get; set; }
        public short SeqNo { get; set; }
        public string SectionName { get; set; }
        public string SectionDesc { get; set; }
        public string SectionType { get; set; }
        public string LastModifyMan { get; set; }
        public Nullable<System.DateTime> LastModifyTime { get; set; }
        public Nullable<short> MinLodgingDays { get; set; }
        public Nullable<short> MinStayDays { get; set; }
        public Nullable<short> MaxStayDays { get; set; }
        public string StartCity { get; set; }
        public string DestCity { get; set; }
        public string StartCityCode { get; set; }
        public string DestCityCode { get; set; }
        public Nullable<bool> UseCtripFlight { get; set; }
        public Nullable<bool> IsDirectFlight { get; set; }
        public Nullable<System.TimeSpan> AirfareEarliestTime { get; set; }
        public Nullable<System.TimeSpan> AirfareLatestTime { get; set; }
        public string IncludeAirline { get; set; }
        public string ExcludeAirline { get; set; }
        public string IncludeFlight { get; set; }
        public string ExcludeFlight { get; set; }
        public Nullable<short> DepartureAdjust { get; set; }
    }
    
}