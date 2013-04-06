using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using HHTravel.Infrastructure.Crosscutting;

namespace HHTravel.DomainModel
{
    /// <summary>
    /// 机票
    /// </summary>
    [DataContract]
    public class Ticket : IComparable<Ticket>, IPriceDate
    {
        public Ticket()
        {
            this.PriceDateList = new List<PriceDate>();
        }

        /// <summary>
        /// 加购说明
        /// </summary>
        [DataMember]
        public string AdditionalPurchasesNote { get; set; }

        /// <summary>
        /// 舱等名称
        /// 例如：头等舱、公务舱
        /// </summary>
        [DataMember]
        public string FlightSeatName { get; set; }

        /// <summary>
        /// 航班
        /// </summary>
        [DataMember]
        public List<Flight> FlightList { get; set; }

        /// <summary>
        ///
        /// </summary>
        [DataMember]
        public List<PriceDate> PriceDateList { get; set; }

        /// <summary>
        /// ProductId
        /// </summary>
        [DataMember]
        public int TicketId { get; set; }

        /// <summary>
        /// Implement the generic CompareTo method with the Temperature
        /// class as the Type parameter.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Ticket other)
        {
            // If other is not a valid object reference, this instance is greater.
            if (other == null || !other.PriceDateList.Any()) return 1;
            if (!this.PriceDateList.Any()) return -1;

            return this.PriceDateList.Min().CompareTo(other.PriceDateList.Min());
        }

        /// <summary>
        /// 获取某天的价格
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public decimal? GetPrice(DateTime date, TravelType travelType)
        {
            decimal? price;
            var q = (from dd in this.PriceDateList
                     where dd.Date == date
                     select dd).SingleOrDefault();

            if (q != null)
                price = q.Price;
            else
            {
                switch (travelType)
                {
                    case TravelType.TravelType3:
                    case TravelType.TravelType2:
                        throw new ArgumentOutOfRangeException("date", date, "指定的日期该机票不可用");
                    default:
                        price = null;
                        break;
                }
            }

            return price;
        }

        public PriceDate GetPriceDate(DateTime date)
        {
            PriceDate dDate = (from dd in this.PriceDateList
                               where dd.Date == date
                               select dd).SingleOrDefault();

            return dDate;
        }
    }
}