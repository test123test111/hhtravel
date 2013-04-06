using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CtripServices.DataContract
{
    public class CtripServicesResponseHeader
    {
        [XmlAttribute]
        public ResultCode ResultCode { get; set; }

        [XmlAttribute]
        public string ResultMsg { get; set; }
    }
}