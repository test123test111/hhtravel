using System.Runtime.Serialization;

namespace HHTravel.DomainModel
{
    [DataContract]
    public class DepartureMonth : IAggregateRoot
    {
        [DataMember]
        public int Month { get; set; }

        [DataMember]
        public int MonthId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Year { get; set; }
    }
}