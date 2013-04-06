using System;
using System.Collections.Generic;
using System.Linq;
using CtripServices.IntlFlight.DataContract;
using CtripServices.IntlFlight.SearchService;
using CtripServices.IntlFlight.SearchService.DataContract;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace HHTravel.FlightsPlanService
{
    /// <summary>
    /// 要求：尽量减少对国际机票接口的类型系统的依赖
    /// </summary>
    public class FlightsPlanServiceImpl : IFlightsPlanService
    {
        private SearchServiceAgent _seachService = new SearchServiceAgent();
        private IFlightsPlanFilterProvider _filterProvider = null;

        public FlightsPlanServiceImpl(IFlightsPlanFilterProvider filterProvider)
        {
            _filterProvider = filterProvider;
        }

        protected virtual FlightsPlanFilter GetFlightsFilterRule(int productId, int flightSegmentNo)
        {
            var rule = FlightsPlanFilter.Default;
            if (_filterProvider != null)
            {
                var rules = _filterProvider.GetFilters(productId);
                rule = rules.FirstOrDefault(r => r.FlightSegmentNo == flightSegmentNo) ?? FlightsPlanFilter.Default;
            }
            return rule;
        }

        public FlightsSegment Search(FlightsPlanSearchCondition sc)
        {
            FlightsSegment seg;

            var filterRule = GetFlightsFilterRule(sc.ProductId, sc.CurrentSegmentNo);
            var requests = new SearchRequestBuilder(sc, filterRule).Build();

            int currentSegmentIndex = (int)sc.CurrentSegmentNo - 1;

            if (requests.Count == 1)
            {
                var req = requests[0];
                var response = _seachService.Search(req);
                var query = Map(sc, (req.PassengerType == PassengerType.CHD), req.PassengerCount, response);
                seg = query;
            }
            else if (requests.Count == 2)
            {
                var req = requests[0];
                var response = _seachService.Search(req);
                seg = Map(sc, (req.PassengerType == PassengerType.CHD), req.PassengerCount, response);
                var query = seg.FlightsPlans;

                req = requests[1];
                response = _seachService.Search(req);
                seg = Map(sc, (req.PassengerType == PassengerType.CHD), req.PassengerCount, response);
                var query2 = seg.FlightsPlans;

                #region 合并计算价格和税费

                //// 按RouteId取交集
                //var idIntersection = query.Select(p1 => p1.RouteId).Intersect(query2.Select(p2 => p2.RouteId));

                // 找出成人/儿童的搜索结果中，相同的FlightsPlan（参考FlightsPlan.Equals 的实现）
                var couples = from p1 in query

                              // 多组匹配的情况下，只取价格最低的
                              let p2 = query2.FirstOrDefault(p2 => p1.Equals(p2))
                              where p2 != null
                              select new { p1, p2 };

                List<FlightsPlan> list = new List<FlightsPlan>();
                foreach (var couple in couples)
                {
                    var p1 = couple.p1;
                    var p2 = couple.p2;

                    p1.TotalPrice += p2.TotalPrice;
                    p1.TotalFuelSurcharges += p2.TotalFuelSurcharges;
                    p1.TotalTax += p2.TotalTax;
                    p1.AdultCount += p2.AdultCount;
                    p1.AdultFuelSurcharges += p2.AdultFuelSurcharges;
                    p1.AdultPrice += p2.AdultPrice;
                    p1.AdultTax += p2.AdultTax;
                    p1.ChildCount += p2.ChildCount;
                    p1.ChildFuelSurcharges += p2.ChildFuelSurcharges;
                    p1.ChildTax += p2.ChildTax;
                    p1.ChildPrice += p2.ChildPrice;

                    list.Add(p1);
                }

                seg.FlightsPlans = list;

                #endregion 合并计算价格和税费
            }
            else
            {
                throw new NotSupportedException();
            }

            seg.FlightsPlans = FilterFlightsPlans(seg.FlightsPlans, filterRule);

            return seg;
        }

        private List<FlightsPlan> FilterFlightsPlans(List<FlightsPlan> list, FlightsPlanFilter filterRule)
        {
            var query = from plan in list
                        let firstFlight = plan.Flights.First()
                        where filterRule.CheckDirect(plan.Flights.Count)
                        && filterRule.CheckAriline(plan.Airline.AirlineCode)
                        && firstFlight.DepartTime >= filterRule.EarliestTime
                        && firstFlight.DepartTime <= filterRule.LatestTime
                        && filterRule.CheckFlightNo(firstFlight.FlightNo)
                        orderby firstFlight.DepartTime
                        select plan;
            return query.ToList();
        }

        private static FlightsSegment Map(FlightsPlanSearchCondition sc, bool isChild, int personCount, SearchResponse response)
        {
            var flightsSegment = sc.FlightsSegments.Single(fs => fs.SegmentNo == sc.CurrentSegmentNo);
            var plans = from r in response.ShoppingResults
                        from poli in r.PolicyInfos

                        // 一个政策有多组价格，这里只取第一组
                        let firstPriceInfo = poli.PriceInfos[0]
                        select new FlightsPlan
                        {
                            FareId = poli.FareID,
                            TotalFuelSurcharges = firstPriceInfo.FuelCharge * personCount,
                            TotalPrice = firstPriceInfo.Price * personCount,
                            TotalTax = firstPriceInfo.Tax * personCount,
                            AdultCount = isChild ? 0 : personCount,
                            AdultFuelSurcharges = isChild ? 0 : firstPriceInfo.FuelCharge,
                            AdultTax = isChild ? 0 : firstPriceInfo.Tax,
                            AdultPrice = isChild ? 0 : firstPriceInfo.Price,
                            ChildCount = isChild ? personCount : 0,
                            ChildFuelSurcharges = isChild ? firstPriceInfo.FuelCharge : 0,
                            ChildTax = isChild ? firstPriceInfo.Tax : 0,
                            ChildPrice = isChild ? firstPriceInfo.Price : 0,
                            Airline = Airline.Parse(poli.OwnerAirline),
                            RouteId = flightsSegment.SelectedRouteId ?? poli.ShoppingInfoID,
                            Flights = (from fi in r.FlightInfos
                                       where fi.SegmentInfoNo == sc.CurrentSegmentNo
                                       from f in fi.Flights

                                       // FlightBaseInfo与Flight一一对应
                                       let flightBaseInfo = poli.FlightBaseInfos[f.No - 1]
                                       select new HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting.Flight
                                       {
                                           ActualAirline = new Airline
                                           {
                                               AirlineCode = f.Carrier     // todo: 航空公司中文名
                                           },
                                           ActualFlightNo = f.CarrierFlightNo,
                                           Aircraft = new Aircraft
                                           {
                                               Type = f.CraftType,
                                           },
                                           Airline = new Airline
                                           {
                                               AirlineCode = f.AirlineCode,
                                               AirlineName = f.AirLineName,
                                           }, // f.Airline
                                           ArrivalAirport = new Airport
                                           {
                                               AirportCode = f.APort,
                                               AirportName = f.APortName,
                                           }, // f.APortNameEng
                                           ArrivalCity = new City
                                           {
                                               CityCode = f.ACity,
                                               CityName = f.ACityName
                                           }, // f.ACityID, f.ACityNameEng
                                           ArrivalTime = TimeSpan.Parse(f.ATimeString),
                                           DepartAirport = new Airport
                                           {
                                               AirportCode = f.DPort,
                                               AirportName = f.DPortName,
                                           },
                                           DepartCity = new City
                                           {
                                               CityCode = f.DCity,
                                               CityName = f.DCityName
                                           },
                                           DepartTime = TimeSpan.Parse(f.DTimeString),
                                           FlightNo = f.FlightNo,
                                           ArrivalDays = f.Nextday,
                                           Baggage = flightBaseInfo.Baggage,
                                           Stopovers = (from s in f.Stops
                                                        select new Stopover
                                                        {
                                                            Airport = s.Airport,
                                                            Duration = TimeSpan.FromMinutes(s.Duration),
                                                        }).ToList(),
                                           FlightSeat = (FlightSeat)flightBaseInfo.ClassGrade,
                                           FlightDuration = new TimeSpan(f.JourneyTime, 0, 0),
                                       }).ToList()
                        };

            if (!sc.ContainsNotLowestPrice)
            {
                // 相同的FlightsPlan（参考FlightsPlan.Equals 的实现），只取最低价的
                plans = from p1 in plans
                        group p1 by p1 into g1
                        let p1 = g1.FirstOrDefault()
                        where p1 != null
                        select p1;
            }

            flightsSegment.FlightsPlans = plans.ToList();
            return flightsSegment;
        }
    }
}