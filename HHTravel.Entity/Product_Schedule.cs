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
    public partial class Product_Schedule
    {
        public int ScheduleId { get; set; }
        public int ProductId { get; set; }
        public short DayNo { get; set; }
        public string Title { get; set; }
        public string LastModifyMan { get; set; }
        public Nullable<System.DateTime> LastModifyTime { get; set; }
        public Nullable<int> SectionId { get; set; }
    }
    
}
