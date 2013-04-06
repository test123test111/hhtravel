using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace HHTravel.CRM.Booking_Online.DomainModel
{
    [DataContract]
    public class Passenger
    {
        /// <summary>
        /// 姓
        /// </summary>
        [DataMember]
        public string FirstName { get; set; }

        /// <summary>
        /// 名（英文）
        /// </summary>
        [DataMember]
        public string FirstNameEn { get; set; }

        /// <summary>
        /// 名
        /// </summary>
        [DataMember]
        public string LastName { get; set; }

        /// <summary>
        /// 姓（英文）
        /// </summary>
        [DataMember]
        public string LastNameEn { get; set; }
    }
}