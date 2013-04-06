using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CtripServices.DataContract
{
    public class CtripServicesRequestHeader
    {
        [XmlAttribute]
        public string UserID { get; set; }

        [XmlAttribute]
        public string RequestType { get; set; }

        [XmlAttribute]
        public bool AsyncRequest { get; set; }
    }
}