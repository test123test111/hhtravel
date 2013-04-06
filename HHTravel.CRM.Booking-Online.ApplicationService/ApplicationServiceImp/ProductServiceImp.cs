using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using HHTravel.CRM.Booking_Online.DataService;
using HHTravel.CRM.Booking_Online.DomainModel;
using HHTravel.CRM.Booking_Online.DomainService;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;
using HHTravel.CRM.Booking_Online.IRepository;
using HHTravel.FlightsPlanService;

namespace HHTravel.CRM.Booking_Online.ApplicationService.ApplicationServiceImp
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ProductServiceImp : IProductService
    {
        private ProductManager _productManager = new ProductManager();

        /// <summary>
        /// 根据出发日期范围获取产品
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IList<Product> FindByDepartureDate(DateTime beginDate, DateTime endDate,
            Pager pager)
        {
            IList<Product> a = null;
            RepositoryCaller.Call<IProductRepository>((repo) =>
            {
                a = repo.FindByDepartureDate(beginDate, endDate,
                   pager).ToList();
            });
            return a;
        }

        /// <summary>
        /// 根据目的地获取产品
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public IList<Product> FindByDestination(int groupId, int? regionId,
            Pager pager)
        {
            IList<Product> a = null;
            RepositoryCaller.Call<IProductRepository>((repo) =>
            {
                if (regionId.HasValue)
                {
                    a = repo.FindByDestinationRegion(regionId.Value,
                            pager).ToList();
                }
                else
                {
                    a = repo.FindByDestinationGroup(groupId,
                            pager).ToList();
                }
            });
            return a;
        }

        /// <summary>
        /// 根据旅行主题获取产品
        /// </summary>
        /// <param name="travelTypeId"></param>
        /// <returns></returns>
        public IList<Product> FindByInterest(int interestId,
            Pager pager)
        {
            IList<Product> a = null;
            RepositoryCaller.Call<IProductRepository>((repo) =>
            {
                a = repo.FindByInterest(interestId,
                        pager).ToList();
            });
            return a;
        }

        /// <summary>
        /// 根据产品名称获取产品
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public IList<Product> FindByProductName(string productName, Pager pager)
        {
            IList<Product> a = null;
            RepositoryCaller.Call<IProductRepository>((repo) =>
            {
                a = repo.FindByProductName(productName,
                        pager).ToList();
            });
            return a;
        }

        /// <summary>
        /// 根据旅游型态获取产品
        /// </summary>
        /// <param name="travelTypeId"></param>
        /// <returns></returns>
        public IList<Product> FindByTravelType(int travelTypeId,
            Pager pager)
        {
            IList<Product> a = null;
            RepositoryCaller.Call<IProductRepository>((repo) =>
            {
                a = repo.FindByTravelType(travelTypeId,
                        pager).ToList();
            });
            return a;
        }

        /// <summary>
        /// 获取所有出发地
        /// </summary>
        /// <returns></returns>
        public IList<DepartureCity> GetAllDepartureCitys()
        {
            var a = DepartureCity.All();
            return a.ToList();
        }

        /// <summary>
        /// 获取所有出发月份
        /// </summary>
        /// <returns></returns>
        public IList<DepartureMonth> GetAllDepartureMonths()
        {
            int nowYear = DateTime.Now.Year;
            int nowMonth = DateTime.Now.Month;

            IList<DepartureMonth> a = null;
            RepositoryCaller.Call<IDepartureMonthRepository>((repo) =>
            {
                var ms = repo.All();
                a = (from d in ms
                     where d.Year > nowYear || (d.Year == nowYear && d.Month >= nowMonth)
                     select d).ToList();
            });
            return a;
        }

        /// <summary>
        /// 获取所有目的地所属分组
        /// </summary>
        /// <returns></returns>
        public IList<DestinationGroup> GetAllDestinationGroups()
        {
            IList<DestinationGroup> a = null;
            RepositoryCaller.Call<IDestinationGroupRepository>((repo) =>
            {
                a = repo.All().ToList();
            });
            return a;
        }

        /// <summary>
        /// 获取所有旅行主题
        /// </summary>
        /// <returns></returns>
        public IList<Interest> GetAllInterests()
        {
            IList<Interest> a = null;
            RepositoryCaller.Call<IInterestRepository>((repo) =>
            {
                a = repo.All().ToList();
            });
            return a;
        }

        /// <summary>
        /// 获取所有产品编号
        /// </summary>
        /// <returns></returns>
        public IList<string> GetAllProductNos()
        {
            IList<string> a = null;
            RepositoryCaller.Call<IProductRepository>((repo) =>
            {
                a = repo.GetAllProductNos().ToList();
            });
            return a;
        }

        /// <summary>
        /// 获取所有旅游型态
        /// </summary>
        /// <returns></returns>
        public IList<TravelType> GetAllTravelTypes()
        {
            var a = Enum.GetValues(typeof(TravelType)).Cast<TravelType>();
            return a.ToList();
        }

        /// <summary>
        /// 获取指定产品的酒店信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IList<Hotel> GetHotels(int productId)
        {
            IList<Hotel> hotels = null;
            RepositoryCaller.Call<IProductRepository>((repo) =>
            {
                hotels = repo.GetHotels(productId).ToList();
            });
            return hotels;
        }

        /// <summary>
        /// 获取产品的行程段
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IList<HotelSegment> GetHotelSegments(int productId)
        {
            IList<HotelSegment> a = null;
            RepositoryCaller.Call<IProductRepository>((repo) =>
            {
                a = repo.GetHotelSegments(productId).ToList();
            });
            return a;
        }

        /// <summary>
        /// 获取产品
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Product GetProduct(int productId)
        {
            Product a = null;
            RepositoryCaller.Call<IProductRepository>((repo) =>
            {
                a = repo.Find(productId);
            });
            return a;
        }

        /// <summary>
        /// 获取产品
        /// </summary>
        /// <param name="productNo"></param>
        /// <returns></returns>
        public Product GetProduct(string productNo)
        {
            Product a = null;
            RepositoryCaller.Call<IProductRepository>((repo) =>
            {
                a = repo.Find(productNo);
            });
            return a;
        }

        /// <summary>
        /// 获取房型信息
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="onlyWithMinPrice">true: 同房型只是筛选出价格最低的房型</param>
        /// <returns></returns>
        public IList<RoomClass> GetRoomClasses(int productId)
        {
            var roomClasses = _productManager.GetRoomClasses(productId, null);
            return roomClasses.ToList();
        }

        /// <summary>
        /// 获取指定日期的房型信息
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="date"></param>
        /// <param name="onlyWithMinPrice">true: 同房型只是筛选出价格最低的</param>
        /// <returns></returns>
        public IList<RoomClass> GetRoomClasses(int productId, DateTime date)
        {
            var roomClasses = _productManager.GetRoomClasses(productId, date);
            return roomClasses.ToList();
        }

        /// <summary>
        /// 获取指定产品的日程项
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public IList<ScheduleItem> GetScheduleItems(int productId,
            Pager pager)
        {
            IList<ScheduleItem> a = null;
            RepositoryCaller.Call<IProductRepository>((repo) =>
            {
                a = repo.GetScheduleItems(productId, pager).ToList();
            });
            return a;
        }

        public IList<Ticket> GetTickets(int productId)
        {
            var tickets = _productManager.GetTickets(productId, null);
            return tickets.ToList();
        }

        /// <summary>
        /// 获取指定产品的指定日期的机票
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public IList<Ticket> GetTickets(int productId, DateTime date)
        {
            var tickets = _productManager.GetTickets(productId, date);
            return tickets.ToList();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public TicketSegment GetTicketSegment(int productId)
        {
            TicketSegment a = null;
            RepositoryCaller.Call<IProductRepository>((repo) =>
            {
                a = repo.GetTicketSegment(productId);
            });
            return a;
        }

        /// <summary>
        /// 搜索产品
        /// </summary>
        /// <returns></returns>
        public IList<Product> Search(SearchCondition searchCondition, Pager pager)
        {
            IList<Product> a = null;
            RepositoryCaller.Call<IProductRepository>((repo) =>
            {
                a = repo.Search(searchCondition, pager).ToList();
            });
            return a;
        }

        public FlightsSegment SearchFlightsPlans(FlightsPlanSearchCondition sc)
        {
            FlightsSegment segment = null;
            sc.Validate();

            IFlightsPlanFilterProvider filterProvider = new SqlFlightsPlanFilterProvider();
            HHTravel.CRM.Booking_Online.Infrastructure.Aspect.Counting("FlightsPlanServiceImpl.Search", () =>
            {
                segment = new FlightsPlanServiceImpl(filterProvider).Search(sc);
            });
            return segment;
        }

        public HotelSegment GetHotelSegment(int productId)
        {
            HotelSegment seg = null;

            //seg = new HotelSegment
            //{
            //    SegmentId = 1,
            //    Nights = 5,
            //    MinPrice = 11000,
            //    HotelList = new List<Hotel> {
            //            new Hotel { HotelId = 1, HotelName = "马尔代夫 Viceroy Hotels Resort Residences Maldives",
            //                RoomClassList = new List<RoomClass>{
            //                    new RoomClass { RoomClassId = 1, RoomClassName = "单人房", PriceDateList= new List<PriceDate>{
            //                        new PriceDate{ Date= DateTime.Parse("2013-03-22"), Price = 5000, StayPricePerDay = 6000, },
            //                        new PriceDate{ Date= DateTime.Parse("2013-03-27"), Price = 6000, StayPricePerDay = 6000, },
            //                        new PriceDate{ Date= DateTime.Parse("2013-03-30"), Price = 6000, StayPricePerDay = 8000, },
            //                    }},
            //                    new RoomClass { RoomClassId = 2, RoomClassName = "双人房", PriceDateList= new List<PriceDate>{
            //                        new PriceDate{ Date= DateTime.Parse("2013-03-22"), Price = 7000, StayPricePerDay = 8000, },
            //                        new PriceDate{ Date= DateTime.Parse("2013-03-27"), Price = 8000, StayPricePerDay = 9000, },
            //                        new PriceDate{ Date= DateTime.Parse("2013-03-30"), Price = 8000, StayPricePerDay = 9100, },
            //                    }},
            //                },},
            //            new Hotel { HotelId = 2, HotelName = "港丽度假村 Conrad Maldives Rangali Island",
            //                RoomClassList = new List<RoomClass>{
            //                    new RoomClass { RoomClassId = 3, RoomClassName = "单人房",PriceDateList= new List<PriceDate>{
            //                        new PriceDate{ Date= DateTime.Parse("2013-03-27"), Price = 8000, StayPricePerDay = 9000, },
            //                        new PriceDate{ Date= DateTime.Parse("2013-03-29"), Price = 8000, StayPricePerDay = 9100, },
            //                    }},
            //                    new RoomClass { RoomClassId = 4, RoomClassName = "双人房", PriceDateList= new List<PriceDate>{
            //                        new PriceDate{ Date= DateTime.Parse("2013-03-27"), Price = 10000, StayPricePerDay = 11000, },
            //                        new PriceDate{ Date= DateTime.Parse("2013-03-30"), Price = 11000, StayPricePerDay = 12000, },
            //                    }},
            //                },},
            //        },
            //};

            return seg;
        }

        public HotelSegment GetDelayHotelSegment(int productId, int delayDays)
        {
            var seg = new HotelSegment
            {
                SegmentId = 2,
                Nights = delayDays,
                HotelList = new List<Hotel> {
                    new Hotel { HotelId = 1, HotelName = "马尔代夫 Viceroy Hotels Resort Residences Maldives",
                        RoomClassList = new List<RoomClass>{
                            new RoomClass { RoomClassId = 1, RoomClassName = "单人房", PriceDateList= new List<PriceDate>{
                                new PriceDate{ Price = 8000 },
                            }},
                            new RoomClass { RoomClassId = 2, RoomClassName = "双人房", PriceDateList= new List<PriceDate>{
                                new PriceDate{ Price = 13000 },
                            }},
                        },},
                    new Hotel { HotelId = 3, HotelName = "港丽度假村2 Conrad Maldives Rangali Island",
                        RoomClassList = new List<RoomClass>{
                            new RoomClass { RoomClassId = 3, RoomClassName = "单人房", PriceDateList= new List<PriceDate>{
                                new PriceDate{ Price = 9000 },
                            }},
                            new RoomClass { RoomClassId = 4, RoomClassName = "双人房", PriceDateList= new List<PriceDate>{
                                new PriceDate{ Price = 15000 },
                            }},
                        },},
                },
            };
            return seg;
        }

        public IList<Infrastructure.Crosscutting.FlightsSegment> GetFlightsSegments(int productId)
        {
            List<FlightsSegment> list = null;

            //list = new List<FlightsSegment> {
            //    new FlightsSegment{ SegmentNo = 1, DepartCity = new City("SHA", "上海"), ArrivalCity = new City("HKG", "香港"), AdjustableDays = -1},
            //    new FlightsSegment{ SegmentNo = 2, DepartCity = new City("TPE", "台北"), ArrivalCity = new City("SHA", "上海"), AdjustableDays = 0},
            //};

            RepositoryCaller.Call<IProductRepository>((repo) =>
            {
                list = repo.GetFlightsSegments(productId).ToList();
            });

            return list;
        }

        public GroundServiceSegment GetGroundServiceSegment(int productId)
        {
            GroundServiceSegment seg = null;

            //seg = new GroundServiceSegment
            //{
            //    PriceDateList = new List<PriceDate>{
            //                    new PriceDate{ Date= DateTime.Parse("2013-03-22"), Price = 3000 },
            //                },
            //    GroundServiceList = new List<GroundService> {
            //        new GroundService{ GroundServiceId = 1, ServiceName = "海口+博鳌+七仙岭+三亚5日（地接服务）1",PriceDateList= new List<PriceDate>{
            //                        new PriceDate{ Date= DateTime.Parse("2013-03-22"), Price = 3000 },
            //                        new PriceDate{ Date= DateTime.Parse("2013-03-22"), Price = 4000 },
            //                    } },
            //        new GroundService{ GroundServiceId = 2, ServiceName = "海口+博鳌+七仙岭+三亚5日（地接服务）2", PriceDateList= new List<PriceDate>{
            //                        new PriceDate{ Date= DateTime.Parse("2013-03-22"), Price = 5000 },
            //                        new PriceDate{ Date= DateTime.Parse("2013-03-22"), Price = 6000 },
            //                    } },
            //        new GroundService{ GroundServiceId = 3, ServiceName = "海口+博鳌+七仙岭+三亚5日（地接服务）3", PriceDateList= new List<PriceDate>{
            //                        new PriceDate{ Date= DateTime.Parse("2013-03-22"), Price = 9000 },
            //                    }  },
            //                }
            //};

            RepositoryCaller.Call<IProductRepository>((repo) =>
            {
                seg = repo.GetGroundServiceSegment(productId);
            });
            return seg;
        }
    }
}