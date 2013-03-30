using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace HHTravel.CRM.Booking_Online.Model
{
    [DataContract]
    public class Subscription
    {
        private bool _isValid = true;

        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public bool IsValid
        {
            get { return _isValid; }
            set { _isValid = value; }
        }
    }
}
