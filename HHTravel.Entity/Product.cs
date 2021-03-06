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
    public partial class Product
    {
        public Product()
        {
            this.Product_NoDeparture = new HashSet<Product_NoDeparture>();
            this.Product_Price = new HashSet<Product_Price>();
        }
    
        public int ProductId { get; set; }
        public string ProductNo { get; set; }
        public string ProductName { get; set; }
        public Nullable<short> ProductType { get; set; }
        public Nullable<short> TripType { get; set; }
        public Nullable<short> ResourceType { get; set; }
        public string ProductDesc { get; set; }
        public string ProductFeature { get; set; }
        public Nullable<short> Language { get; set; }
        public string Destination1 { get; set; }
        public string Destination2 { get; set; }
        public string StartCity { get; set; }
        public string DestCity { get; set; }
        public string DestProvince { get; set; }
        public string DestCountry { get; set; }
        public Nullable<short> TripDays { get; set; }
        public Nullable<short> MinPersonNum { get; set; }
        public Nullable<short> MaxPersonNum { get; set; }
        public Nullable<short> AdvanceDays { get; set; }
        public Nullable<System.DateTime> EffectDate { get; set; }
        public Nullable<System.DateTime> ExpireDate { get; set; }
        public string SetOffDays { get; set; }
        public string ServiceDays { get; set; }
        public string SaleDesc { get; set; }
        public Nullable<int> Weight { get; set; }
        public Nullable<bool> IsUp { get; set; }
        public Nullable<bool> IsValid { get; set; }
        public string Link { get; set; }
        public string Address { get; set; }
        public Nullable<double> Longitude { get; set; }
        public Nullable<double> Latitude { get; set; }
        public Nullable<short> Company { get; set; }
        public string LastModifyMan { get; set; }
        public Nullable<System.DateTime> LastModifyTime { get; set; }
        public string ProductNote { get; set; }
        public Nullable<short> MinLodgingDays { get; set; }
        public string Manager { get; set; }
        public Nullable<bool> IsBackCash { get; set; }
        public Nullable<bool> Recommend { get; set; }
        public Nullable<bool> AllowChild { get; set; }
        public Nullable<short> TripClass { get; set; }
        public Nullable<short> AdvanceTripDays { get; set; }
        public Nullable<short> MaxTripDays { get; set; }
        public Nullable<short> ExtendTripDays { get; set; }
    
        public virtual ICollection<Product_NoDeparture> Product_NoDeparture { get; set; }
        public virtual ICollection<Product_Price> Product_Price { get; set; }
    }
    
}
