using System.Runtime.Serialization;

namespace HHTravel.DomainModel
{
    [DataContract]
    public class Subscription : IAggregateRoot
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