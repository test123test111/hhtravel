using System;
using System.Runtime.Serialization;

namespace HHTravel.Infrastructure.Crosscutting
{
    [DataContract]
    public class SearchCondition
    {
        public SearchCondition()
        {
        }

        public SearchCondition(string departureCity,
            int? destinationGroup, int? destinationRegion,
            int? travelType, int? interest,
            DateTime? beginDate, DateTime? endDate,
            int? daysSection, string productName)
        {
            this.DepartureCity = Infrastructure.Crosscutting.DepartureCity.Parse(departureCity);
            this.DestinationGroup = destinationGroup;
            this.DestinationRegion = destinationRegion;
            this.TravelType = travelType;
            this.Interest = interest;
            this.BeginDate = beginDate;
            this.EndDate = endDate;

            int? minDays;
            int? maxDays;
            DaysSection? dsec = (daysSection.HasValue) ? (DaysSection)daysSection : (DaysSection?)null;
            dsec.ParseRange(out minDays, out maxDays);
            this.MinDays = minDays;
            this.MaxDays = maxDays;

            this.ProductName = productName;
        }

        [DataMember]
        public DateTime? BeginDate { get; set; }

        [DataMember]
        public DepartureCity? DepartureCity { get; set; }

        [DataMember]
        public int? DestinationGroup { get; set; }

        [DataMember]
        public int? DestinationRegion { get; set; }

        [DataMember]
        public DateTime? EndDate { get; set; }

        [DataMember]
        public int? Interest { get; set; }

        [DataMember]
        public int? MaxDays { get; set; }

        [DataMember]
        public int? MinDays { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public int? TravelType { get; set; }
    }
}