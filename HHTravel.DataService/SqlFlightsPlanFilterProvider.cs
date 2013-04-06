using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.FlightsPlanService;
using HHTravel.DataAccess;
using HHTravel.DataAccess.DbContexts;

namespace HHTravel.DataService
{
    public class SqlFlightsPlanFilterProvider : IFlightsPlanFilterProvider
    {
        public IList<FlightsPlanFilter> GetFilters(int productId)
        {
            IList<FlightsPlanFilter> filters;
            using (var cxt = DbContextFactory.Create<ProductDbEntities>())
            {
                var query = from segment in cxt.Product_Section
                            where segment.ProductId == productId
                            && segment.SectionType == "系统机票"
                            orderby segment.SeqNo
                            select new
                            {
                                segment.IsDirectFlight,
                                segment.AirfareEarliestTime,
                                segment.AirfareLatestTime,
                                segment.ExcludeAirline,
                                segment.IncludeAirline,
                                segment.ExcludeFlight,
                                segment.IncludeFlight,
                            };
                var list = query.ToList();
                filters = list.Select((segData, i) => new FlightsPlanFilter
                {
                    FlightSegmentNo = (i + 1),
                    IsDirect = segData.IsDirectFlight ?? FlightsPlanFilter.Default.IsDirect,
                    EarliestTime = segData.AirfareEarliestTime ?? FlightsPlanFilter.Default.EarliestTime,
                    LatestTime = segData.AirfareLatestTime ?? FlightsPlanFilter.Default.LatestTime,
                    IncludeAirlineCodes = segData.IncludeAirline.Split(',').ToList(),
                    ExcludeAirlineCodes = segData.ExcludeAirline.Split(',').ToList(),
                    IncludeFlightNos = segData.IncludeFlight.Split(',').ToList(),
                    ExcludeFlightNos = segData.ExcludeFlight.Split(',').ToList(),
                }).ToList();
                return filters;
            }
        }
    }
}
