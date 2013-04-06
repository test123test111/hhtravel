using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CtripServices.DataContract
{
    [XmlRoot("Response")]
    public class CtripServicesResponse
    {
        [XmlElement("Header")]
        public CtripServicesResponseHeader Header { get; set; }

        [XmlElementAttribute("FltIntlSearchFlightsResponse", typeof(CtripServices.IntlFlight.SearchService.DataContract.SearchResponse), Form = XmlSchemaForm.Unqualified)]
        public object ResponseBody;
    }
}