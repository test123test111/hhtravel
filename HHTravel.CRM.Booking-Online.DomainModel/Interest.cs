using System.Runtime.Serialization;

namespace HHTravel.CRM.Booking_Online.DomainModel
{
    /// <summary>
    /// 旅行主题
    /// </summary>
    [DataContract]
    public class Interest : IAggregateRoot
    {
        [DataMember]
        public int InterestId { get; set; }

        /// <summary>
        /// 主题名称
        /// 例如：地球标记、高端火车、高端TravelType2
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}