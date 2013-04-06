using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CtripServices.DataContract
{
    [XmlRoot("Request")]
    public class CtripServicesRequest
    {
        [XmlElement("Header")]
        public CtripServicesRequestHeader Header { get; set; }

        [XmlElementAttribute("FltIntlSearchFlightsRequest", typeof(CtripServices.IntlFlight.SearchService.DataContract.SearchRequest), Form = XmlSchemaForm.Unqualified)]
        public object RequestBody;
    }
}