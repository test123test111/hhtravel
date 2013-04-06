using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CtripServices.IntlFlight.DataContract;
using CtripServices.IntlFlight.SearchService.DataContract;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace HHTravel.FlightsPlanService
{
    public class SearchRequestBuilder
    {
        private FlightsPlanSearchCondition _searchCondition;
        private FlightsPlanFilter _filterRule;

        public SearchRequestBuilder(FlightsPlanSearchCondition searchCondition, FlightsPlanFilter rule)
        {
            this._searchCondition = searchCondition;
            this._filterRule = rule ?? FlightsPlanFilter.Default;
        }

        public List<SearchRequest> Build()
        {
            if (_searchCondition.AdultCount + _searchCondition.ChildCount == 0)
            {
                throw new ArgumentException("乘客人数不能为0");
            }

            List<SearchRequest> list = new List<SearchRequest>();
            SearchRequest request = null;

            Func<SearchRequest> create = () =>
            {
                string lastSegRouttingId = null;
                var currentSeg = _searchCondition.FlightsSegments[(int)_searchCondition.CurrentSegmentNo - 1];
                if (_searchCondition.CurrentSegmentNo > 1)
                {
                    lastSegRouttingId = _searchCondition.FlightsSegments[(int)_searchCondition.CurrentSegmentNo - 2].SelectedRouteId;
                }

                var routings = (from seg in _searchCondition.FlightsSegments

                                // 将前序航段的航班组合构造为Routings
                                where seg.Flights.Count > 0
                                from f in seg.Flights
                                select new Routing
                                {
                                    AAirport = f.ArrivalAirport.AirportCode,
                                    ACode = f.ArrivalCity.CityCode,
                                    Airline = f.Airline.AirlineCode,
                                    DAirport = f.DepartAirport.AirportCode,
                                    DCode = f.DepartCity.CityCode,
                                    FlightNo = f.FlightNo,

                                    //No =
                                    OperatorNo = f.ActualFlightNo,

                                    //SeatClass = f.FlightClass.ClassCode,
                                    SegmentInfoNo = (int)seg.SegmentNo,  // NOTE!
                                }).ToList();

                return new SearchRequest
                {
                    TripType = GetTripType(_searchCondition.FlightsSegments),
                    SegmentInfos = (from seg in _searchCondition.FlightsSegments
                                    select new SegmentInfo
                                    {
                                        DCode = seg.DepartCity.CityCode,
                                        ACode = seg.ArrivalCity.CityCode,
                                        DDate = seg.DepartDate.ToString("yyyy-MM-dd"),
                                        TimePeriod = string.Format("{0:hhmm}-{1:hhmm}", _filterRule.EarliestTime, _filterRule.LatestTime)
                                    }).ToList(),
                    Eligibility = Eligibility.NOR,
                    //Airline = ,
                    SeatGrade = (SeatGrade)_searchCondition.FlightSeat,
                    SalesType = SalesType.Online, // TODO: 是否需要对方新增hhTravel
                    IsDirect = _filterRule.IsDirect,
                    Routings = routings,
                    ShoppingInfoID = lastSegRouttingId,

                    //AgentID = _searchCondition.AgentId.ToString(),
                };
            };

            if (_searchCondition.AdultCount > 0)
            {
                request = create();
                request.PassengerType = PassengerType.ADT;
                request.PassengerCount = (int)_searchCondition.AdultCount;
                list.Add(request);
            }

            if (_searchCondition.ChildCount > 0)
            {
                request = create();
                request.PassengerType = PassengerType.CHD;
                request.PassengerCount = (int)_searchCondition.ChildCount;
                list.Add(request);
            }

            return list;
        }

        private static TripType GetTripType(List<FlightsSegment> segs)
        {
            TripType tripType;
            switch (segs.Count)
            {
                case 0:
                    throw new ArgumentOutOfRangeException("未指定任何航段信息");
                case 1:
                    tripType = TripType.OW;
                    break;

                case 2:
                    var segGo = segs.First();
                    var segReturn = segs.Last();
                    tripType = (segGo.DepartCity == segReturn.ArrivalCity && segGo.ArrivalCity == segReturn.DepartCity) ? TripType.RT : TripType.MT;
                    break;

                default:
                    tripType = TripType.MT;
                    break;
            }

            return tripType;
        }
    }
}