using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataValidator.Models.FlightsPlan;
using HHTravel.Infrastructure.Crosscutting;
using Newtonsoft.Json;
using HHTravel.FlightsPlanService;
using HHTravel.DataService;
using HHTravel.Infrastructure;
using DataValidator.Models;
using HHTravel.ApplicationService;
using HHTravel.ApplicationService.ApplicationServiceImp;

namespace DataValidator.Controllers
{
    public class FlightsPlanController : Controller
    {
        private IProductService _productService;
        public FlightsPlanController()
        {
            DbConnectionStringProvider.Instance = new MockDbConnectionStringProvider("LOCAL");
            FlightsPlanSearchModel.DbServers = DbServerSelectListFactory.Create();
        }

        protected IProductService ProductService
        {
            get
            {
                if (_productService == null)
                {
                    _productService = new ProductServiceImp();
                }
                return _productService;
            }
        }
        public ActionResult Index()
        {
            var model = new FlightsPlanSearchModel
            {
                FlightSegmentPostModels = new List<FlightsPostModel> {
                    new FlightsPostModel{ SegmentNo = 1 },
                    new FlightsPostModel{ SegmentNo = 2 },
                    new FlightsPostModel{ SegmentNo = 3 },
                },
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Search(FlightsPlanSearchModel postModel)
        {
            DbConnectionStringProvider.Instance = new MockDbConnectionStringProvider(postModel.DbServer);


            var product = ProductService.GetProduct(postModel.ProductNo);
            if (product == null)
            {
                this.ModelState.AddModelError("ProductNo", "产品不存在");
            }

            if (!ModelState.IsValid)
            {
                return View("Index", postModel);
            }

            FlightsPlanSearchCondition sc = new FlightsPlanSearchCondition
            {
                AdultCount = postModel.AdultCount,
                ChildCount = postModel.ChildCount,
                CurrentSegmentNo = 1,
                FlightSeat = postModel.FlightSeat,
                FlightsSegments = (from fspm in postModel.FlightSegmentPostModels
                                   where !string.IsNullOrEmpty(fspm.ArrivalCityCode)
                                   select new FlightsSegment
                                   {
                                       SegmentNo = fspm.SegmentNo,
                                       ArrivalCity = City.Parse(fspm.ArrivalCityCode),
                                       DepartCity = City.Parse(fspm.DepartCityCode),
                                       DepartDate = fspm.DepartDate,
                                   }).ToList(),
                ProductId = product.ProductId,
                ContainsNotLowestPrice = true,
            };

            var query = Search(sc);

            FligthsPlanListModel model = new FligthsPlanListModel
            {
                DbServer = postModel.DbServer,  // !TEMP: 向下一次提交传递
                IsShowPlansJson = postModel.IsShowPlansJson,
                PlanModels = query.ToList(),
                SearchCondition = sc,
            };

            return View("List", model);
        }

        [HttpPost]
        public ActionResult SearchNextSegment(FlightsPlanSearchModel2 postModel)
        {
            DbConnectionStringProvider.Instance = new MockDbConnectionStringProvider(postModel.DbServer);

            var sc = JsonConvert.DeserializeObject<FlightsPlanSearchCondition>(postModel.SearchConditionJson);
            sc.CurrentSegmentNo++; // Note!
            var plan = JsonConvert.DeserializeObject<FlightsPlan>(postModel.FlightsPlanJson);

            var lastSegmentIndex = (int)(sc.CurrentSegmentNo - 1) - 1;
            var lastSeg = sc.FlightsSegments[lastSegmentIndex];
            lastSeg.SelectedRouteId = plan.RouteId;
            lastSeg.Flights = plan.Flights;

            var query = Search(sc);

            FligthsPlanListModel model = new FligthsPlanListModel
            {
                DbServer = postModel.DbServer,
                IsShowPlansJson = postModel.IsShowPlansJson,
                PlanModels = query.ToList(),
                SearchCondition = sc,
            };

            return View("List", model);
        }

        private IEnumerable<FlightsPlanModel> Search(FlightsPlanSearchCondition sc)
        {
            sc.Validate();

            IFlightsPlanFilterProvider filterProvider = new SqlFlightsPlanFilterProvider();
            IFlightsPlanService repo = new FlightsPlanServiceImpl(filterProvider);

            var seg = repo.Search(sc);

            var query = from plan in seg.FlightsPlans
                        select new FlightsPlanModel
                        {
                            TotalFuelSurcharges = plan.TotalFuelSurcharges,
                            TotalPrice = plan.TotalPrice,
                            TotalTax = plan.TotalTax,
                            AdultCount = plan.AdultCount,
                            AdultFuelSurcharges = plan.AdultFuelSurcharges,
                            AdultPrice = plan.AdultPrice,
                            AdultTax = plan.AdultTax,
                            ChildCount = plan.ChildCount,
                            ChildFuelSurcharges = plan.ChildFuelSurcharges,
                            ChildPrice = plan.ChildPrice,
                            ChildTax = plan.ChildTax,

                            //IsDirect = (plan.Flights.Count == 1),
                            FlightModels = (from f in plan.Flights
                                            select new FlightModel
                                            {
                                                ActualAirline = f.ActualAirline,
                                                ActualFlightNo = f.ActualFlightNo,

                                                //AircraftModel = f.CraftType,
                                                Aircraft = f.Aircraft,
                                                Airline = f.Airline,
                                                ArrivalAirport = f.ArrivalAirport,
                                                ArrivalCity = f.ArrivalCity,
                                                ArrivalTimeString = f.ArrivalTime.ToString(@"hh\:mm"),
                                                DepartAirport = f.DepartAirport,
                                                DepartCity = f.DepartCity,
                                                DepartTimeString = f.DepartTime.ToString(@"hh\:mm"),

                                                //FlightDuration =
                                                FlightNo = f.FlightNo,
                                                ArrivalDays = f.ArrivalDays,
                                                Baggage = f.Baggage,

                                                FlightClass = f.FlightSeat,

                                                //MaxSeats =
                                                //MinSeats =
                                            }).ToList(),

                            FlightsPlanJson = JsonConvert.SerializeObject(plan),
                        };

            return query;
        }
    }
}
