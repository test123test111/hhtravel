using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using HHTravel.CRM.Booking_Online.DataAccess.HardCode;
using HHTravel.CRM.Booking_Online.DataAccess.TableWorkers;
using HHTravel.CRM.Booking_Online.IRepository;
using HHTravel.CRM.Booking_Online.Model;
using Microsoft.Practices.Unity;
using HHTravel.CRM.Booking_Online.DataAccess.TableWorkers.ProductDb;
using HHTravel.CRM.Booking_Online.Model.Exceptions;

namespace HHTravel.CRM.Booking_Online.DataAccess.Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        private ProductTableWorker _tableWorker;
        internal virtual ProductTableWorker TableWorker
        {
            get
            {
                if (_tableWorker == null)
                {
                    _tableWorker = new ProductTableWorker(this.ProductDbContext);
                }
                return _tableWorker;
            }
        }

        [InjectionConstructor]
        public ProductRepository()
        {
            this.ProductDbContext = DbContextFactory.Create<ProductDbEntities>();
        }

        internal ProductRepository(ProductDbEntities productDbContext)
        {
            this.ProductDbContext = productDbContext;
        }


        /// <summary>
        /// 根据产品编号查找产品
        /// </summary>
        /// <param name="productNo"></param>
        /// <returns></returns>
        public virtual Product Find(string productNo)
        {
            Product p = null;
            var a = this.TableWorker.Find(productNo);
            if (a != null)
            {

                int productId = a.ProductId;
                p = Map(a);
                // 行程项
                p.ScheduleItemList = this.GetScheduleItems(productId).ToList();
                // 酒店
                p.HotelList = this.GetHotels(productId).ToList();
                p.MainPhoto = this.GetMainPhoto(productId);
                // 行程图相册
                p.PhotoList = this.GetPhotos(productId).ToList();
                // 订购须知
                p.OrderNotes = this.TableWorker.GetInfoByInfoType(productId, ProductInfoType.订购须知);
                p.CostNotes1 = this.TableWorker.GetInfoByInfoType(productId, ProductInfoType.费用包含);
                p.CostNotes2 = this.TableWorker.GetInfoByInfoType(productId, ProductInfoType.费用不包含);
                p.Consultation = this.TableWorker.GetInfoByInfoType(productId, ProductInfoType.旅游资讯);
                p.VisaNotes = this.TableWorker.GetInfoByInfoType(productId, ProductInfoType.签证须知);
                //p.HotelServiceNote = this.TableWorker.GetInfoByInfoType(productId, ProductInfoType.酒店服务设施);
            }

            return p;
        }
        /// <summary>
        /// DO映射到BO
        /// </summary>
        /// <param name="eProduct"></param>
        /// <returns></returns>
        protected Product Map(Entity.Product eProduct)
        {
            if (eProduct == null)
                return null;

            var p = new Product
                    {
                        ProductId = eProduct.ProductId,
                        ProductName = eProduct.ProductName,
                        ProductNo = eProduct.ProductNo,
                        Description = eProduct.ProductDesc,
                        Feature = eProduct.ProductFeature,
                        Note = eProduct.ProductNote,
                        Days = eProduct.TripDays ?? 0,
                        AdvanceDays = eProduct.AdvanceDays,
                        MaxPersonNum = eProduct.MaxPersonNum,
                        MinLodgingDays = eProduct.MinLodgingDays,
                        DepartureDateList = this.TableWorker.GetDepartureDateList(eProduct),
                        TravelType = (TravelType)eProduct.TripType.Value,
                        InterestList = GetInterests(eProduct.ProductId).ToList(),
                        DepartureCity = eProduct.StartCity,
                        DestinationCity = eProduct.DestCity,
                        HasDiscount = eProduct.IsBackCash ?? false,
                    };
            // 价格
            var pi = this.TableWorker.GetSpecsMinPriceItem(eProduct);
            p.MinPrice = (pi != null) ? pi.Price : 0;
            p.MinMarketPrice = (pi != null) ?pi.PriceMarketing : null;

            // 产品各图
            var picWorker = new PictureTableWorker(ProductDbContext);
            var pics = picWorker.FindBy(eProduct.ProductId, PictureObjectType.产品);
            p.MainImage = GetImageInfo(pics.FirstOrDefault(i => i.Location == PictureLocation.主图));
            p.TopImage = GetImageInfo(pics.FirstOrDefault(i => i.Location == PictureLocation.营销图));
            p.RouteMap = GetImageInfo(pics.FirstOrDefault(i => i.Location == PictureLocation.路线图));

            return p;
        }

        protected IEnumerable<Product> Map(IEnumerable<Entity.Product> eProductList)
        {
            var a = from p in eProductList
                    select Map(p);
            return a;
        }

        #region 获取关联属性
        /// <summary>
        /// 获取最少成行人数 (自由行）
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public int GetMinPersonNumList(int productId)
        {
            int minPersonNum = 0;
            var repoSpec = new ProductSpecTableWorker(ProductDbContext);
            var a = from s in repoSpec.All()
                    where s.ProductId == productId
                    select (int)(s.MinPersonNum ?? 0);
            if (a.Count() > 0)
            {
                minPersonNum = a.Min();
            }

            return minPersonNum;
        }
        /// <summary>
        /// 获取产品的目的地分组
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IEnumerable<DestinationGroup> GetDestinationGroups(int productId)
        {
            var repoGroup = new DestinationGroupRepository(ProductDbContext);
            // linq 2 e
            var a = (from pp in ProductDbContext.Product_Property
                     where pp.ProductId == productId
                     select pp);
            var b = a.ToList();
            // linq 2 o
            var c = from pp in b
                    join dg in repoGroup.All()
                    on pp.PropertyId equals dg.GroupId
                    select dg;

            return c;
        }
        /// <summary>
        /// 获取产品的目的地区域
        /// todo: 考虑到使用场景，可尽心性能优化：传入RegionList
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IEnumerable<DestinationRegion> GetDestinationRegions(int productId)
        {
            var repoGroup = new DestinationGroupRepository(ProductDbContext);
            // linq 2 e
            var a = (from pp in ProductDbContext.Product_Property
                     where pp.ProductId == productId
                     select pp);
            var b = a.ToList();

            var g = GetDestinationGroups(productId).First();
            // linq 2 o
            var c = from pp in b
                    join dr in g.RegionList
                    on pp.PropertyId equals dr.RegionId
                    select dr;

            return c;
        }
        /// <summary>
        /// 获取旅行主题
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        protected virtual IEnumerable<Interest> GetInterests(int productId)
        {
            InterestRepository repoInterest = new InterestRepository(ProductDbContext);
            // linq 2 e
            var a = (from pp in ProductDbContext.Product_Property
                     where pp.ProductId == productId
                     select pp);
            var b = a.ToList();
            // linq 2 o
            var c = from pp in b
                    join i in repoInterest.All()
                    on pp.PropertyId equals i.InterestId
                    select i;

            return c;
        }
        /// <summary>
        /// 获取日程项
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        protected virtual IEnumerable<ScheduleItem> GetScheduleItems(int productId)
        {
            var a = from s in ProductDbContext.Product_Schedule
                    join eScheduleDetail in ProductDbContext.Product_Schedule_Detail
                    on s.ScheduleId equals eScheduleDetail.ScheduleId into gScheduleDetail
                    where s.ProductId == productId
                    select new
                    {
                        ScheduleItem = new ScheduleItem
                        {
                            Sort = s.DayNo,
                            Name = s.Title,
                        },
                        DetailsList = (from sd in gScheduleDetail
                                       orderby sd.TakeOffTime
                                       select new { sd.ScheduleType, sd.Description })
                    };
            var b = a.ToList();

            b.ForEach(s =>
            {
                Func<string, string> getDesp = (scheduleType) =>
                {
                    string desp = null;
                    var detailItem = s.DetailsList.FirstOrDefault(d => string.Equals(d.ScheduleType, scheduleType));
                    if (detailItem != null)
                    {
                        desp = detailItem.Description;
                    }
                    return desp;
                };
                s.ScheduleItem.Infos = new ScheduleItemInfos
                {
                    SpotsInfo = getDesp("景点"),
                    FlightInfo = getDesp("航班"),
                    HotelInfo = getDesp("酒店"),
                    CateringInfo = getDesp("餐饮"),
                };
            });
            var c = b.Select(s => s.ScheduleItem);
            return c;
        }
        /// <summary>
        /// 获取主行程图
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        protected ImageInfo GetMainPhoto(int productId)
        {
            var picWorker = new PictureTableWorker(ProductDbContext);
            var a = from pic in picWorker.FindBy(productId, PictureObjectType.产品, PictureLocation.行程图)
                    where pic.Title == "1"
                    select new ImageInfo { Title = pic.Title, Url = pic.URL };
            var b = a.FirstOrDefault();
            return b;
        }
        /// <summary>
        /// 获取行程图
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        protected virtual IEnumerable<ImageInfo> GetPhotos(int productId)
        {
            var picWorker = new PictureTableWorker(ProductDbContext);
            var a = from pic in picWorker.FindBy(productId, PictureObjectType.产品, PictureLocation.行程图)
                    where pic.Title != "1"
                    select new ImageInfo { Title = pic.Title, Url = pic.URL };
            var b = a.ToList();
            return b;
        }
        /// <summary>
        /// 获取酒店信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual IEnumerable<Hotel> GetHotels(int productId)
        {
            var picWorker = new PictureTableWorker(ProductDbContext);
            var a = (from p in this.TableWorker.AllHotels()
                     join attach in ProductDbContext.Product_Attach_Product
                     on p.ProductId equals attach.ItemProductId
                     join pic in picWorker.FindBy(PictureObjectType.产品, null) // todo: null 需要修改为对应酒店图片的"Location"
                     on p.ProductId equals pic.ObjectId into gPic
                     where attach.ProductId == productId
                     select new
                     {
                         Hotel = new Hotel
                             {
                                 HotelId = p.ProductId,
                                 HotelName = p.ProductName,
                                 Description = p.ProductDesc,
                                 Feature = p.ProductFeature,
                                 Url = p.Link,
                             },
                         PhotoList = from pic2 in gPic
                                     select new ImageInfo { Title = pic2.Title, Url = pic2.URL }
                     });
            var b = a.ToList();
            // 获取酒店的照片
            foreach (var h in b)
            {
                h.Hotel.PhotoList = h.PhotoList.ToList();
            }

            var c = b.Select(h => h.Hotel);
            return c;
        }
        /// <summary>
        /// 获取指定产品的机票
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual IEnumerable<Ticket> GetTickets(int productId)
        {
            var pfWorker = new ProductFlightTableWorker(ProductDbContext);

            var a = from p in this.TableWorker.AllTickets()
                    join attach in ProductDbContext.Product_Attach_Product
                     on p.ProductId equals attach.ItemProductId
                    join pp in ProductDbContext.Product_Price
                    on p.ProductId equals pp.ProductId into gPrice //left join 只有单品游的机票才有价格
                    from pp in gPrice.DefaultIfEmpty()
                    join spec in ProductDbContext.Product_Spec
                    on p.ProductId equals spec.ProductId into gSpec // left join
                    from spec in gSpec.DefaultIfEmpty()
                    join pf in pfWorker.All()
                    on p.ProductId equals pf.ProductId into gFlight
                    where attach.ProductId == productId
                    select new
                    {
                        TicketProduct = new Ticket
                        {
                            TicketId = p.ProductId,
                            FlightClassId = (spec != null) ? spec.SpecId : (int?)null,
                            FlightClassName = (spec != null) ? spec.SpecName : null,
                            Price = pp.Price,
                            EffectDate = pp.EffectDate,
                            ExpireDate = pp.ExpireDate,
                            AdditionalPurchasesNote = p.ProductDesc,
                        },
                        FlightList = (from pf in gFlight
                                      join dport in ProductDbContext.Flt_Airport
                                      on pf.Dport equals dport.AirportCode
                                      join aport in ProductDbContext.Flt_Airport
                                      on pf.Aport equals aport.AirportCode
                                      join airline in ProductDbContext.Flt_AirLine
                                      on pf.AirLine equals airline.AirLineCode
                                      orderby pf.Direction descending, pf.SeqNo ascending
                                      select new Flight
                                      {
                                          IsGo = (pf.Direction == "去程"),
                                          Airline = airline.CnName,
                                          FlightNo = pf.FlightNo,
                                          DepartureAirport = dport.CnName,
                                          ArrivalAirport = aport.CnName,
                                          DepartureTime = pf.Dtime,
                                          ArrivalTime = pf.Atime,
                                      })
                    };

            var b = a.ToList();

            if (b.Count == 0)
            {
                throw new DataException("机票信息缺失或无效");
            }

            foreach (var f in b)
            {
                f.TicketProduct.FlightList = f.FlightList.ToList();
            }
            var c = b.Select(f => f.TicketProduct).ToList();
            return c;
        }
        /// <summary>
        /// 获取房型信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IEnumerable<RoomClass> GetRoomClasses(int productId)
        {
            var specWorker = new ProductSpecTableWorker(ProductDbContext);

            //var a = from s in specWorker.All()
            //        join pr in ProductDbContext.Product_Price
            //        on s.SpecId equals pr.SpecId into gPrice
            //        where s.ProductId == productId
            //        select new
            //        {
            //            Spec = s,
            //            PriceItem = gPrice.OrderBy(p => p.Price).FirstOrDefault(),
            //        };
            //var b = a.ToList();
            //var c = from bb in b
            //        select new RoomClass
            //        {
            //            RoomClassId = bb.Spec.SpecId,
            //            RoomClassName = bb.Spec.SpecType,
            //            BreakfastDinnerTip = bb.Spec.SpecNote,
            //            Price = bb.PriceItem.Price,
            //            StayPrice = bb.PriceItem.PriceStay ?? 0,
            //            EffectDate = bb.PriceItem.EffectDate,
            //            ExpireDate = bb.PriceItem.ExpireDate,
            //        };

            var a = from s in specWorker.All()
                    join pr in ProductDbContext.Product_Price
                    on s.SpecId equals pr.SpecId
                    where s.ProductId == productId
                    orderby s.SpecId, pr.Price    // 按价格升序排列房型信息
                    select new RoomClass
                    {
                        RoomClassId = s.SpecId,
                        RoomClassName = s.SpecType,
                        BreakfastDinnerTip = s.SpecNote,
                        Price = pr.Price,
                        StayPrice = pr.PriceStay ?? 0,
                        EffectDate = pr.EffectDate,
                        ExpireDate = pr.ExpireDate,
                    };

            return a;
        }
        /// <summary>
        /// 获取产品的行程项
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="sort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public IEnumerable<ScheduleItem> GetScheduleItemList(int productId,
            Pager pager)
        {
            var a = this.GetScheduleItems(productId);
            var b = a.OrderBy(s => s.Sort);

            PagerHelper.PageSize = 7; // !HARDCODE
            int pageCount;
            var c = PagerHelper.TakePage(b, pager.PageIndex, out pageCount);
            pager.PageCount = pageCount;
            return c;
        }
        #endregion

        #region 筛选、搜索
        public IEnumerable<Product> FindByDepartureCity(string departureCity,
            Pager pager)
        {
            var a = this.TableWorker.FindByDepartureCity(this.TableWorker.All(), departureCity);
            var b = this.TableWorker.Page(a, pager);
            return Map(b);
        }

        public IEnumerable<Product> FindByTravelType(int travelTypeId,
            Pager pager)
        {
            var a = this.TableWorker.FindByTravelType(this.TableWorker.All(), travelTypeId);
            var b = this.TableWorker.Page(a, pager);
            return Map(b);
        }

        public IEnumerable<Product> FindByInterest(int interestId,
            Pager pager)
        {
            var a = from p in this.TableWorker.FindByProperty(interestId)
                    select p;
            var b = this.TableWorker.Page(a, pager);
            return Map(b);
        }

        public IEnumerable<Product> FindByDestinationGroup(int groupId,
            Pager pager)
        {
            //var a = this.InternalRepository.FindByDestinationGroup(groupName);
            var a = from p in this.TableWorker.FindByProperty(groupId)
                    select p;
            var b = this.TableWorker.Page(a, pager);
            return Map(b);
        }

        public IEnumerable<Product> FindByDestinationRegion(int regionId,
            Pager pager)
        {
            //var a = from p in this.InternalRepository.FindByDestinationRegion(regionName)
            var a = from p in this.TableWorker.FindByProperty(regionId)
                    select p;
            var b = this.TableWorker.Page(a, pager);
            return Map(b);
        }

        public IEnumerable<Product> FindByDepartureDate(DateTime beginDate, DateTime endDate,
            Pager pager)
        {
            var a = from p in this.TableWorker.All()
                    where p.Product_NoDeparture.Any(d => d.DepartureDate >= beginDate && d.DepartureDate <= endDate)
                    select p;
            var b = this.TableWorker.Page(a, pager);
            return Map(b);
        }

        public IEnumerable<Product> Search(string departurePlaceName, int? groupId, int? regionId, int? travelTypeId, DateTime? beginDate, DateTime? endDate, int? interestid,
            Pager pager)
        {
            var a = this.TableWorker.All();
            a = departurePlaceName != null ? this.TableWorker.FindByDepartureCity(a, departurePlaceName) : a;
            a = groupId.HasValue ? this.TableWorker.FindByProperty(a, groupId.Value) : a;
            // todo: 区域若不在分组下，则不对产品列表进行区域过滤
            a = regionId.HasValue ? this.TableWorker.FindByProperty(a, regionId.Value) : a;
            a = travelTypeId.HasValue ? this.TableWorker.FindByTravelType(a, travelTypeId.Value) : a;
            a = interestid.HasValue ? this.TableWorker.FindByProperty(a, interestid.Value) : a;
            //&& beginDate.HasValue && .Value == p.TravelType.TravelTypeId

            // todo: 出发日期相关的搜索处理
            var b = this.TableWorker.Page(a, pager);
            return Map(b);
        }
        #endregion

        public override Product Find(int productId)
        {
            var a = this.TableWorker.Find(productId);
            return Map(a);
        }

        public override IEnumerable<Product> All()
        {
            throw new NotImplementedException();
        }


        private ImageInfo GetImageInfo(HHTravel.CRM.Booking_Online.Entity.Picture pic)
        {
            if (pic == null || string.IsNullOrEmpty(pic.URL)) return null;
            return new ImageInfo { Title = pic.Title, Url = pic.URL };
        }
    }
}
