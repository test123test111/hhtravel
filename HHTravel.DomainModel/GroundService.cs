using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HHTravel.CRM.Booking_Online.DomainModel
{
    public class GroundService : IPriceDate
    {
        public GroundService()
        {
            this.PriceDateList = new List<PriceDate>();
        }

        [DataMember]
        public int GroundServiceId { get; set; }

        [DataMember]
        public string ServiceName { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public List<PriceDate> PriceDateList { get; set; }

        public PriceDate GetPriceDate(DateTime date)
        {
            // 取有最低价（成人价）的
            PriceDate dDate = (from dd in this.PriceDateList
                               where dd.Date == date
                               orderby dd.Price
                               select dd).FirstOrDefault();
            return dDate;
        }
    }
}