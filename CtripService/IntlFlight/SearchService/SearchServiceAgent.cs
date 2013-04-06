using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using CtripServices.DataContract;
using CtripServices.IntlFlight.SearchService.DataContract;

namespace CtripServices.IntlFlight.SearchService
{
    public class SearchServiceAgent
    {
        private XmlSerializer s_requestSerializer = new XmlSerializer(typeof(CtripServicesRequest));
        private XmlSerializer s_responseSerializer = new XmlSerializer(typeof(CtripServicesResponse));

        [HHTravel.CRM.Booking_Online.Infrastructure.CountingHandler("FlightIntl.FlightSearch.SearchFlights")]
        public SearchResponse Search(SearchRequest searchRequest)
        {
            SearchResponse searchResponse;
            CtripServicesRequest req = new CtripServicesRequest
            {
                Header = new CtripServicesRequestHeader
                {
                    RequestType = "FlightIntl.FlightSearch.SearchFlights",
                    UserID = "340101",   // todo: 配置
                },
                RequestBody = searchRequest,
            };

            string requestXml;

            using (StringWriter sw = new Utf8StringWriter())
            {
                s_requestSerializer.Serialize(sw, req);
                requestXml = sw.ToString();
            }

            string responseXml = "";
            using (CtripServicesAgent agent = new CtripServicesAgent("http://fltws.sh.ctriptravel.com/FltIntlFlightSearchWebService/FlightSearch.asmx"))   // todo: 配置
            {
                HHTravel.CRM.Booking_Online.Infrastructure.Aspect.Counting("FlightIntl.FlightSearch.SearchFlights", () =>
                {
                    responseXml = agent.Request(requestXml);
                });
            }

            CtripServicesResponse resp;
            using (StringReader sw = new StringReader(responseXml))
            {
                resp = (CtripServicesResponse)s_responseSerializer.Deserialize(sw);
            }

            if (resp.Header.ResultCode != ResultCode.Success)
            {
                var error = string.Format("ResultCode: {0}, ResultMsg: {1}", resp.Header.ResultCode, resp.Header.ResultMsg);
                throw new Exception(error);
            }

            searchResponse = (SearchResponse)resp.ResponseBody;

            return searchResponse;
        }
    }

    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding { get { return Encoding.UTF8; } }
    }
}