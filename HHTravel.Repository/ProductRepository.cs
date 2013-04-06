using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using HHTravel.CRM.Booking_Online.DataAccess;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using HHTravel.CRM.Booking_Online.DataAccess.HardCode;
using HHTravel.CRM.Booking_Online.DataAccess.Providers;
using HHTravel.CRM.Booking_Online.DomainModel;
using HHTravel.CRM.Booking_Online.Infrastructure;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;
using HHTravel.CRM.Booking_Online.Infrastructure.Exceptions;
using HHTravel.CRM.Booking_Online.IRepository;
using Microsoft.Practices.Unity;

namespace HHTravel.CRM.Booking_Online.Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), InjectionConstructor]
        public ProductRepository()
        {
            this.ProductDbContext = DbContextFactory.Create<ProductDbEntities>();
        }

        internal ProductRepository(ProductDbEntities productDbContext)
        {
            this.ProductDbContext = productDbContext;
        }

        public override IEnumerable<Product> All()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据产品编号查找产品
        /// </summary>
        /// <param name="productNo"></param>
        /// <returns></returns>
        public virtual Product Find(string productNo)
        {
            Product product = null;
            Entity.ProductWrapper entity = this.FindWrappedProduct(productNo);

            if (entity == null)
            {
                ExceptionSwitcher<NotSupportedException>.TryThrow(false,    // 不抛出，交给表现层处理
                     new NotSupportedException(string.Format("product not exists. productNo: {0}", productNo))
                );
                return product;
            }

            int productId = entity.Product.ProductId;
            product = Map(entity.Product);

            if (!string.IsNullOrEmpty(entity.Product.SetOffDays))
            {
                DateTime date = DateTime.MinValue;  // just for init variable
                var setOffDays = entity.Product.SetOffDays.Split('|');
                product.SetOffDateList = (from str in setOffDays
                                          where !string.IsNullOrWhiteSpace(str)
                                          && DateTime.TryParse(str.Trim(), out date)
                                          select date)
                                         .ToList();
            }

            //p.MainPhoto = this.GetMainPhoto(productId);
            // 行程图相册
            //p.PhotoList = this.GetPhotos(productId).ToList();
            // 订购须知
            var queryInfos = (from i in ProductDbContext.Product_Info
                              where i.ProductId == productId
                              select new { i.InfoTypeName, i.Info });

            if (queryInfos.Any())
            {
                var infos = queryInfos.ToList();
                Func<string, string> getInfo = (infoType) =>
                {
                    var aa = from i in infos
                             where i.InfoTypeName == infoType
                             select i.Info;
                    return aa.Any() ? string.Join("<br/>", aa.ToArray()) : null;
                };
                product.OrderNotes = getInfo(ProductInfoType.订购须知);
                product.CostNotes1 = getInfo(ProductInfoType.费用包含);
                product.CostNotes2 = getInfo(ProductInfoType.费用不包含);
                product.Consultation = getInfo(ProductInfoType.旅游资讯);
                product.VisaNotes = getInfo(ProductInfoType.签证须知);

                //product.HotelServiceNote = getInfo(ProductInfoType.酒店服务设施);
            }

            // 产品各图
            var picProvider = new PicturesProvider(ProductDbContext);
            var queryPics = picProvider.FindBy(productId, PictureObjectType.产品);
            if (queryPics.Any())
            {
                var pics = queryPics.ToList();
                Func<Entity.Picture, ImageInfo> getImageInfo = (pic) =>
                {
                    if (pic == null || string.IsNullOrEmpty(pic.URL)) return null;
                    return new ImageInfo { Title = pic.Title, Url = pic.URL };
                };
                product.MainImage = getImageInfo(pics.FirstOrDefault(i => i.Location == PictureLocation.主图));
                product.RouteMap = getImageInfo(pics.FirstOrDefault(i => i.Location == PictureLocation.路线图));
                product.MainPhoto = getImageInfo(pics.FirstOrDefault(i => i.Location == PictureLocation.行程图 && i.Title == "1"));
                product.PhotoList = (from pic in pics.Where(i => i.Location == PictureLocation.行程图 && i.Title != "1")
                                     select getImageInfo(pic)).ToList();
            }

            var queryProps = (from pp in ProductDbContext.Product_Property
                              where pp.ProductId == productId
                              select pp);

            if (!queryProps.Any())
            {
                ExceptionSwitcher<DataInvalidException>.TryThrow(GlobalConfig.IsProductEnvironment,
                    new DataInvalidException(string.Format("product: {0} have not properties", entity.Product.ProductId)));
            }
            else
            {
                var props = queryProps.ToList();
                product.InterestList = GetInterests(props).ToList();
                product.DestinationGroupList = this.GetDestinationGroups(props).ToList();
                product.DestinationRegionList = this.GetDestinationRegions(props).ToList();
            }

            // 可出发日期集合
            var departureDateList = this.GetPriceDateList(entity);
            product.DepartureDateList = departureDateList;

            SetProductMinPrice(product, entity, departureDateList);

            return product;
        }

        public override Product Find(int productId)
        {
            var a = this.FindProductEntity(productId);
            return Map(a);
        }

        public IEnumerable<string> GetAllProductNos()
        {
            var query = this.AllWrapperedProducts().Select(pp => pp.Product);
            var a = from p in query
                    orderby p.ProductId
                    select p.ProductNo;
            return a;
        }

        #region 获取关联属性

        public virtual GroundServiceSegment GetGroundServiceSegment(int productId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取酒店信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual IEnumerable<Hotel> GetHotels(int productId)
        {
            var picProvider = new PicturesProvider(ProductDbContext);
            var a = (from hotelProduct in this.AvailableHotels
                     join attach in ProductDbContext.Product_Attach_Product
                     on hotelProduct.ProductId equals attach.ItemProductId
                     let hotelId = hotelProduct.ProductId
                     join pic in picProvider.FindBy(PictureObjectType.产品, null) // todo: null 需要修改为对应酒店图片的"Location"
                     on hotelProduct.ProductId equals pic.ObjectId into gPic
                     where attach.ProductId == productId
                     orderby attach.SectionId, attach.SeqNo
                     select new
                     {
                         Hotel = new Hotel
                         {
                             HotelId = hotelProduct.ProductId,
                             HotelName = hotelProduct.ProductName,
                             Description = hotelProduct.ProductDesc,
                             Feature = hotelProduct.ProductFeature,
                             Url = hotelProduct.Link,
                         },
                         PhotoList = from pic in gPic.DefaultIfEmpty()
                                     select new ImageInfo { Title = pic.Title, Url = pic.URL }
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
        /// 获取产品的酒店行程段
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual IEnumerable<HotelSegment> GetHotelSegments(int productId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取房型信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual IEnumerable<RoomClass> GetRoomClasses(int productId)
        {
            var query = from s in this.ProductDbContext.Product_Spec
                        join pr in ProductDbContext.Product_Price
                        on s.SpecId equals pr.SpecId into gPrice
                        where s.ProductId == productId && s.IsValid.HasValue && s.IsValid.Value
                        orderby s.SpecId    // 按价格升序排列房型信息
                        select new
                        {
                            RoomClass = new RoomClass
                            {
                                RoomClassId = s.SpecId,
                                RoomClassName = s.SpecType,
                                BreakfastDinnerTip = s.SpecNote,
                            },
                            PriceRows = gPrice
                        };
            var rcList = query.ToList();

            if (rcList.Count == 0)
            {
                ExceptionSwitcher<DataInvalidException>.TryThrow(GlobalConfig.IsProductEnvironment,
                    new DataInvalidException("房型信息缺失或无效"));
            }

            foreach (var rc in rcList)
            {
                foreach (var ps in rc.PriceRows)
                {
                    var date = ps.EffectDate;
                    while (date <= ps.ExpireDate)
                    {
                        var dDate = rc.RoomClass.PriceDateList.SingleOrDefault(dd => dd.Date == date);
                        if (dDate == null)
                        {
                            rc.RoomClass.PriceDateList.Add(new PriceDate
                            {
                                Date = date,
                                Price = ps.Price,
                                StayPricePerDay = (ps.PriceStay ?? 0),
                                ChildPrice = (ps.PriceChild ?? 0),
                                ChildStayPricePerDay = (ps.PriceStayChild ?? 0),
                            });
                        }
                        else if (dDate.Price > ps.Price)
                        {
                            dDate.Price = ps.Price;
                        }

                        // day++
                        date = date.AddDays(1);
                    }
                }
            }

            var b = rcList.Select(r => r.RoomClass);
            return b;
        }

        /// <summary>
        /// 获取产品的日程项
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public IEnumerable<ScheduleItem> GetScheduleItems(int productId,
            Pager pager)
        {
            var query = this.GetScheduleItemsWithoutInfos(productId);

            if (pager != null)
            {
                int pageCount;
                query = PagerHelper.LazyTakePage<Entity.Product_Schedule>(query, pager.PageSize, pager.PageIndex, out pageCount) as IQueryable<Entity.Product_Schedule>;
                pager.PageCount = pageCount;
            }

            var a = GetScheduleItems(query);
            return a;
        }

        /// <summary>
        /// 获取指定产品的机票
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual IEnumerable<Ticket> GetTickets(int productId)
        {
            var query = from ticket in this.AllTickets()
                        join attach in ProductDbContext.Product_Attach_Product
                         on ticket.ProductId equals attach.ItemProductId
                        join pp in ProductDbContext.Product_Price
                        on ticket.ProductId equals pp.ProductId into gPrice //left join 只有TravelType3的机票才有价格
                        join pf in this.ProductDbContext.Product_Flight
                        on ticket.ProductId equals pf.ProductId into gFlight
                        where attach.ProductId == productId
                        orderby attach.SectionId, attach.SeqNo
                        select new
                        {
                            Ticket = new Ticket
                            {
                                TicketId = ticket.ProductId,
                                FlightSeatName = ticket.ProductFeature,

                                //Price = (pp != null) ? pp.Price : 0,
                                //EffectDate = (pp != null) ? pp.EffectDate : DateTime.MinValue,
                                //ExpireDate = (pp != null) ? pp.ExpireDate : DateTime.MinValue,
                                AdditionalPurchasesNote = ticket.ProductDesc,
                            },
                            Prices = gPrice,
                            Flights = (from pf in gFlight
                                       join dport in ProductDbContext.Flt_Airport
                                       on pf.Dport equals dport.AirportCode into gAirport1 //left join
                                       from dport in gAirport1.DefaultIfEmpty()
                                       join aport in ProductDbContext.Flt_Airport
                                       on pf.Aport equals aport.AirportCode into gAirport2 //left join
                                       from aport in gAirport2.DefaultIfEmpty()
                                       join airline in ProductDbContext.Flt_AirLine
                                       on pf.AirLine equals airline.AirLineCode into gAirline //left join
                                       from airline in gAirline.DefaultIfEmpty()
                                       orderby pf.Direction descending, pf.SeqNo ascending
                                       select new Flight
                                       {
                                           IsGo = (pf.Direction == "去程"),
                                           Airline = new Airline { AirlineName = airline != null ? airline.CnName : pf.AirLine },
                                           FlightNo = pf.FlightNo,
                                           DepartAirport = new Airport { AirportName = dport != null ? dport.CnName : pf.Dport },
                                           ArrivalAirport = new Airport { AirportName = aport != null ? aport.CnName : pf.Aport },
                                           DepartTime = pf.Dtime ?? default(TimeSpan),
                                           ArrivalTime = pf.Atime ?? default(TimeSpan),
                                       })
                        };

            var tList = query.ToList();

            if (tList.Count == 0)
            {
                ExceptionSwitcher<DataInvalidException>.TryThrow(GlobalConfig.IsProductEnvironment,
                    new DataInvalidException("机票信息缺失或无效"));
            }

            foreach (var f in tList)
            {
                foreach (var ps in f.Prices)
                {
                    var date = ps.EffectDate;
                    while (date <= ps.ExpireDate)
                    {
                        var dDate = f.Ticket.PriceDateList.SingleOrDefault(dd => dd.Date == date);
                        if (dDate == null)
                        {
                            f.Ticket.PriceDateList.Add(new PriceDate
                            {
                                Date = date,
                                Price = ps.Price,
                            });
                        }
                        else if (dDate.Price > ps.Price)
                        {
                            dDate.Price = ps.Price;
                        }

                        // day++
                        date = date.AddDays(1);
                    }
                }

                //f.TicketProduct.DepartureDateList =
                f.Ticket.FlightList = f.Flights.ToList();
            }
            var c = tList.Select(f => f.Ticket).ToList();
            return c;
        }

        /// <summary>
        /// 获取产品的机票行程段
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual TicketSegment GetTicketSegment(int productId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取当前日期之后的可出发日期集合
        /// </summary>
        /// <param name="product"></param>
        /// <param name="beginDate">起始日期</param>
        /// <returns></returns>
        internal virtual List<PriceDate> GetPriceDateList(Entity.ProductWrapper product)
        {
            List<PriceDate> list = new List<PriceDate>();
            if (product == null)
            {
                throw new ArgumentNullException("product");
            }

            // 没价格的产品，没有可出发日期
            if (product.Prices.Count == 0)
            {
                return list;
            }

            // 价格的有效日期和产品的有效日期无交集，没有可出发日期
            var maxDateHasPrice = product.Prices.Max(pp => pp.ExpireDate);
            var minDateHasPrice = product.Prices.Min(pp => pp.EffectDate);
            if (maxDateHasPrice < product.Product.EffectDate
                            || minDateHasPrice > product.Product.ExpireDate)
            {
                return list;
            }

            var firstDepartureDate = product.FirstDepartureDate.Value;
            var lastDepartureDate = product.LastDepartureDate;

            int days = (int)(lastDepartureDate - firstDepartureDate).TotalDays;
            var b = product.Prices;
            if (b.Any())
            {
                // 排除掉不在价格有效期内的日期
                for (int i = 0; i <= days; i++)
                {
                    var date = firstDepartureDate.AddDays(i);
                    if (b.Any(
                        pr => date >= pr.EffectDate && date <= pr.ExpireDate))
                    {
                        var departureDate = new PriceDate
                        {
                            Date = date,

                            //MinPrice =
                        };
                        list.Add(departureDate);
                    }
                }
            }
            else
            {
                return list;
            }

            #region 排除掉不出发的日期

            // 优化：通过过滤，降低某些情况下sql的复杂度
            var a = product.ClosedDepartureDates.Where(
                nd => nd.DepartureDate.Date >= firstDepartureDate && nd.DepartureDate <= lastDepartureDate);
            if (a.Any())
            {
                var undepartureDateList = a.Select(nd => nd.DepartureDate).ToList();
                list.RemoveAll(dd => undepartureDateList.Contains(dd.Date));
            }

            #endregion 排除掉不出发的日期

            return list;
        }

        /// <summary>
        /// 获取产品的所有目的地分组
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        protected IEnumerable<DestinationGroup> GetDestinationGroups(IEnumerable<Entity.Product_Property> props)
        {
            var repoGroup = new DestinationGroupRepository(ProductDbContext);

            // linq 2 o
            var c = from pp in props
                    join dg in repoGroup.All()
                    on pp.PropertyId equals dg.GroupId
                    select dg;

            return c;
        }

        /// <summary>
        /// 获取产品的所有目的地区域
        /// </summary>
        /// <param name="props"></param>
        /// <returns></returns>
        protected IEnumerable<DestinationRegion> GetDestinationRegions(IEnumerable<Entity.Product_Property> props)
        {
            var repoGroup = new DestinationGroupRepository(ProductDbContext);

            // linq 2 o
            var c = from pp in props
                    join dr in repoGroup.All().SelectMany(g => g.RegionList)
                    on pp.PropertyId equals dr.RegionId
                    select dr;

            return c;
        }

        /// <summary>
        /// 获取旅行主题
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        protected virtual IEnumerable<Interest> GetInterests(IEnumerable<Entity.Product_Property> props)
        {
            InterestRepository repoInterest = new InterestRepository(ProductDbContext);

            // linq 2 o
            var c = from pp in props
                    join i in repoInterest.All()
                    on pp.PropertyId equals i.InterestId
                    select i;

            return c;
        }

        /// <summary>
        /// 获取主行程图
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        protected ImageInfo GetMainPhoto(int productId)
        {
            var picProvider = new PicturesProvider(ProductDbContext);
            var a = from pic in picProvider.FindBy(productId, PictureObjectType.产品, PictureLocation.行程图)
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
            var picProvider = new PicturesProvider(ProductDbContext);
            var a = from pic in picProvider.FindBy(productId, PictureObjectType.产品, PictureLocation.行程图)
                    where pic.Title != "1"
                    select new ImageInfo { Title = pic.Title, Url = pic.URL };
            var b = a.ToList();
            return b;
        }

        /// <summary>
        /// 获取日程项
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        protected virtual IEnumerable<ScheduleItem> GetScheduleItems(int productId)
        {
            var query = GetScheduleItemsWithoutInfos(productId);
            var b = GetScheduleItems(query);
            return b;
        }

        protected virtual IEnumerable<ScheduleItem> GetScheduleItems(IQueryable<Entity.Product_Schedule> withoutInfos)
        {
            var query = from s in withoutInfos
                        join eScheduleDetail in ProductDbContext.Product_Schedule_Detail
                        on s.ScheduleId equals eScheduleDetail.ScheduleId into gScheduleDetail
                        select new
                        {
                            ScheduleItem = new ScheduleItem
                            {
                                Sort = s.DayNo,
                                Name = s.Title,
                            },
                            DetailsList = (from sd in gScheduleDetail
                                           orderby sd.TakeOffTime
                                           select new
                                           {
                                               sd.ScheduleType,
                                               sd.Description
                                           })
                        };
            var b = query.ToList();

            Func<string, string> getDesp;
            b.ForEach(si =>
            {
                // because of si and si.DetailsList are Anonymous Type, so used Cloure to resolve type reference.
                getDesp = (scheduleType) =>
                {
                    string desp = null;
                    var an = si.DetailsList.FirstOrDefault(d => string.Equals(d.ScheduleType, scheduleType));
                    if (an != null)
                    {
                        desp = an.Description;
                    }
                    return desp;
                };
                si.ScheduleItem.Infos = new ScheduleItemInfos
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

        protected virtual IQueryable<Entity.Product_Schedule> GetScheduleItemsWithoutInfos(int productId)
        {
            var query = from s in ProductDbContext.Product_Schedule
                        where s.ProductId == productId
                        orderby s.DayNo
                        select s;
            return query;
        }

        #endregion 获取关联属性

        #region 筛选、搜索

        public IEnumerable<Product> FindByDepartureCity(DepartureCity? departureCity,
            Pager pager)
        {
            string city = departureCity != null ? departureCity.Value.CityCode : null;
            var query = this.AllWrapperedProducts();
            query = this.FindByDepartureCity(query, city);
            var b = this.LazyPage(query, pager);
            return Map(b);
        }

        public IEnumerable<Product> FindByDepartureDate(DateTime beginDate, DateTime endDate,
                    Pager pager)
        {
            var query = this.AllWrapperedProducts();
            query = from p in query
                    where (beginDate >= p.Product.EffectDate && beginDate <= p.Product.ExpireDate) ||
                            (endDate >= p.Product.EffectDate && endDate <= p.Product.ExpireDate)
                    select p;
            query = this.LazyPage(query, pager);
            return Map(query);
        }

        public IEnumerable<Product> FindByDestinationGroup(int groupId,
                    Pager pager)
        {
            var query = from pw in this.FindByProperty(groupId)
                        select pw;
            query = this.LazyPage(query, pager);
            return Map(query);
        }

        public IEnumerable<Product> FindByDestinationRegion(int regionId,
                    Pager pager)
        {
            var query = from pw in this.FindByProperty(regionId)
                        select pw;
            query = this.LazyPage(query, pager);
            return Map(query);
        }

        public IEnumerable<Product> FindByInterest(int interestId,
                    Pager pager)
        {
            var query = from p in this.FindByProperty(interestId)
                        select p;
            query = this.LazyPage(query, pager);
            return Map(query);
        }

        /// <summary>
        /// 根据产品名称获取产品
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public IEnumerable<Product> FindByProductName(string productName, Pager pager)
        {
            var query = this.AllWrapperedProducts();
            query = this.FindByProductName(query, productName);
            query = this.LazyPage(query, pager);
            return Map(query);
        }

        public IEnumerable<Product> FindByTravelType(int travelTypeId,
                    Pager pager)
        {
            var query = this.AllWrapperedProducts();
            query = this.FindByTravelType(query, travelTypeId);
            var b = this.LazyPage(query, pager);
            return Map(b);
        }

        public virtual IEnumerable<Product> Search(SearchCondition searchCondition,
                    Pager pager)
        {
            IEnumerable<Product> products;

            var query = this.AllWrapperedProducts();

            query = (searchCondition.DepartureCity != null) ? this.FindByDepartureCity(query, searchCondition.DepartureCity.Value.CityCode) : query;
            query = searchCondition.DestinationGroup.HasValue ? this.FindByProperty(query, searchCondition.DestinationGroup.Value) : query;
            query = searchCondition.DestinationRegion.HasValue ? this.FindByProperty(query, searchCondition.DestinationRegion.Value) : query;
            query = searchCondition.TravelType.HasValue ? this.FindByTravelType(query, searchCondition.TravelType.Value) : query;
            query = searchCondition.Interest.HasValue ? this.FindByProperty(query, searchCondition.Interest.Value) : query;

            query = searchCondition.MinDays.HasValue ? query.Where(pw => pw.Product.TripDays >= searchCondition.MinDays.Value) : query;
            query = searchCondition.MaxDays.HasValue ? query.Where(pw => pw.Product.TripDays <= searchCondition.MaxDays.Value) : query;

            query = !string.IsNullOrEmpty(searchCondition.ProductName) ? this.FindByProductName(query, searchCondition.ProductName) : query;

            // 出发日期相关的搜索处理
            if (searchCondition.BeginDate.HasValue || searchCondition.EndDate.HasValue)
            {
                var pws = SearchByDepartureDate(query, searchCondition.BeginDate, searchCondition.EndDate);
                var b = this.Page(pws, pager);
                products = Map(b);
            }
            else
            {
                if (pager.SortRule == SortRule.ProductMinDepartureDate)
                {
                    var pws = SortByMinDepartureDate(query);
                    var b = this.Page(pws, pager);
                    products = Map(b);
                }
                else
                {
                    var b = this.LazyPage(query, pager);
                    products = Map(b);
                }
            }

            return products;
        }

        protected virtual IEnumerable<Entity.ProductWrapper> SearchByDepartureDate(IQueryable<Entity.ProductWrapper> query,
            DateTime? beginDate, DateTime? endDate)
        {
            IEnumerable<KeyValuePair<Entity.ProductWrapper, List<PriceDate>>> tempProductDateList = null;

            if (beginDate.HasValue)
            {
                // 性能优化：缩小拉取数据的范围
                query = from pw in query
                        where beginDate <= pw.LastDepartureDate
                        select pw;
            }

            if (endDate.HasValue)
            {
                // 性能优化：缩小拉取数据的范围
                query = from p in query
                        where endDate >= p.FirstDepartureDate
                        select p;
            }

            if (beginDate.HasValue)
            {
                var tempList = query.ToList();
                tempProductDateList = from p in tempList
                                      let dateList = GetPriceDateList(p)
                                      where dateList.Any(d => d.Date >= beginDate)
                                      select new KeyValuePair<Entity.ProductWrapper, List<PriceDate>>(p, dateList);
            }

            if (endDate.HasValue)
            {
                if (tempProductDateList == null)
                {
                    var tempList = query.ToList();
                    tempProductDateList = from p in tempList
                                          let dateList = GetPriceDateList(p)
                                          where dateList.Any(d => d.Date <= endDate)
                                          select new KeyValuePair<Entity.ProductWrapper, List<PriceDate>>(p, dateList);
                }
                else
                {
                    tempProductDateList = from dp in tempProductDateList
                                          where dp.Value.Any(d => d.Date <= endDate)
                                          select dp;
                }
            }

            var list = tempProductDateList.ToList();

            // 校正可出发日期的起始值，以用于按日期排序
            list.ForEach(dp => { dp.Key.Product.EffectDate = dp.Value.Min(dd => dd.Date); });
            return list.Select(dp => dp.Key);
        }

        protected virtual IEnumerable<Entity.ProductWrapper> SortByMinDepartureDate(IQueryable<Entity.ProductWrapper> query)
        {
            var tempList = query.ToList();  // do query
            IEnumerable<KeyValuePair<Entity.ProductWrapper, List<PriceDate>>> tempProductDateList = null;
            tempProductDateList = from p in tempList
                                  let dateList = GetPriceDateList(p)
                                  where dateList.Any()
                                  select new KeyValuePair<Entity.ProductWrapper, List<PriceDate>>(p, dateList);

            var list = tempProductDateList.ToList();

            // 校正可出发日期的起始值，以用于按日期排序
            list.ForEach(dp => { dp.Key.Product.EffectDate = dp.Value.Min(dd => dd.Date); });
            return list.Select(dp => dp.Key);
        }

        #endregion 筛选、搜索

        #region protected virtual

        /// <summary>
        /// 所有有效的产品
        /// 有有效日期范围、有价格、价格有有效范围、上架、有效
        /// </summary>
        /// <returns></returns>
        internal virtual IQueryable<Entity.ProductWrapper> AllWrapperedProducts()
        {
            DateTime today = DateTime.Now.Date;
            var query = (from p in ProductDbContext.Product.Include(p => p.Product_Price)
                         where p.ResourceType == (int)ProductResourceType.产品
                         && p.ProductNo != ""
                         && p.ExpireDate.HasValue
                         && p.EffectDate.HasValue
                         && p.IsValid.HasValue && p.IsValid.Value
                         && p.Product_Price.Any(pr => pr.Price > 0)
                         select p);

            if (GlobalConfig.IsProductEnvironment)
            {
                // 过滤掉未上架的产品
                query = from p in query
                        where p.IsUp.HasValue && p.IsUp.Value
                        select p;
            }

            if (!GlobalConfig.ContainsProductsIsSingleProduct)
            {
                // 过滤掉TravelType3的产品
                query = from p in query
                        where p.TripType != (int)TravelType.TravelType3
                        select p;
            }

            // 修正产品有效期
            var query2 = from p in query
                         let tempFirstDepartureDate = EntityFunctions.AddDays(today, p.AdvanceDays)
                         let firstDepartureDate = (p.EffectDate < tempFirstDepartureDate)
                                                  ? tempFirstDepartureDate : p.EffectDate
                         let lastDepartureDate = p.ExpireDate.Value

                         // 产品有效期修正后，产品的过期日期不小于产品的起始日期
                         where lastDepartureDate >= firstDepartureDate
                         select new Entity.ProductWrapper
                         {
                             Product = p,
                             Prices = p.Product_Price,
                             MinPrice = p.Product_Price.Min(pr => pr.Price),
                             ClosedDepartureDates = p.Product_NoDeparture,
                             FirstDepartureDate = firstDepartureDate,
                             LastDepartureDate = lastDepartureDate,
                         };

            // 筛选出可出发日期，没有全部被关闭的产品
            var query3 = query2.Where(pp => pp.ClosedDepartureDates.Count(pn => pn.DepartureDate >= pp.FirstDepartureDate
                                                                        && pn.DepartureDate <= pp.LastDepartureDate)
                             < EntityFunctions.DiffDays(pp.FirstDepartureDate, pp.LastDepartureDate) + 1);

            // 至少有一个出发日期是有价格的
            var query4 = query3.Where(pp => pp.Prices.Count(pr => pr.EffectDate > pp.LastDepartureDate
                                                  || pr.ExpireDate < pp.FirstDepartureDate)
                             < pp.Prices.Count());

            return query4;
        }

        /// <summary>
        /// 根据目的地分组名称查找产品
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        internal virtual IQueryable<Entity.Product> FindByDestinationGroup(string groupName)
        {
            DateTime now = DateTime.Now;
            var query = this.AllWrapperedProducts().Select(pp => pp.Product);
            query = (from p in query
                     where p.Destination1 == groupName
                     select p);
            return query;
        }

        protected virtual IQueryable<Entity.Product> AvailableHotels
        {
            get
            {
                return (from hotelProduct in ProductDbContext.Product
                        where hotelProduct.ResourceType == (int)ProductResourceType.酒店

                            //&& p.ExpireDate.HasValue
                            //&& p.EffectDate.HasValue
                            //&& p.ExpireDate.Value > p.EffectDate.Value
                            && hotelProduct.IsUp.HasValue && hotelProduct.IsUp.Value
                            && hotelProduct.IsValid.HasValue && hotelProduct.IsValid.Value
                        select hotelProduct);
            }
        }

        protected virtual IQueryable<Entity.Product> AllTickets()
        {
            DateTime now = DateTime.Now;
            var query = (from p in ProductDbContext.Product
                         where p.ResourceType == (int)ProductResourceType.航班

                             //&& p.ExpireDate.HasValue
                             //&& p.EffectDate.HasValue
                             //&& p.ExpireDate.Value > p.EffectDate.Value
                             //&& p.IsUp.HasValue && p.IsUp.Value    // 机票产品没有上架/下架操作
                             && p.IsValid.HasValue && p.IsValid.Value
                         select p);
            return query;
        }

        /// <summary>
        /// 根据出发地名称查找产品
        /// </summary>
        /// <param name="products"></param>
        /// <param name="departureCity"></param>
        /// <returns></returns>
        protected virtual IQueryable<Entity.ProductWrapper> FindByDepartureCity(IQueryable<Entity.ProductWrapper> products, string departureCity)
        {
            var a = from p in products
                    where p.Product.StartCity == departureCity
                    select p;
            return a;
        }

        /// <summary>
        /// 根据目的地区域名称查找产品
        /// </summary>
        /// <param name="regionName"></param>
        /// <returns></returns>
        protected IQueryable<Entity.Product> FindByDestinationRegion(string regionName)
        {
            DateTime now = DateTime.Now;
            var query = this.AllWrapperedProducts().Select(pp => pp.Product);
            query = (from p in query
                     where p.Destination2 == regionName
                     select p);
            return query;
        }

        protected virtual IQueryable<Entity.ProductWrapper> FindByProductName(IQueryable<Entity.ProductWrapper> products, string productName)
        {
            var query = (from pw in products
                         where pw.Product.ProductName.Contains(productName)
                         select pw);
            return query;
        }

        protected virtual IQueryable<Entity.ProductWrapper> FindByProperty(int propertyId)
        {
            var query = this.AllWrapperedProducts();
            query = FindByProperty(query, propertyId);
            return query;
        }

        /// <summary>
        /// 根据属性Id查找产品
        /// </summary>
        /// <param name="products"></param>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        protected virtual IQueryable<Entity.ProductWrapper> FindByProperty(IQueryable<Entity.ProductWrapper> products, int propertyId)
        {
            var query = (from p in products
                         join prodprop in ProductDbContext.Product_Property
                         on p.Product.ProductId equals prodprop.ProductId
                         where prodprop.PropertyId == propertyId
                         select p);

            return query;
        }

        /// <summary>
        /// 根据travelType查找产品
        /// </summary>
        /// <param name="products"></param>
        /// <param name="travelTypeId"></param>
        /// <returns></returns>
        protected virtual IQueryable<Entity.ProductWrapper> FindByTravelType(IQueryable<Entity.ProductWrapper> products, int travelTypeId)
        {
            var a = from p in products
                    where p.Product.TripType == travelTypeId || 
                        (travelTypeId == 14 && (p.Product.TripType == (int)TravelType.TravelType1 || p.Product.TripType == (int)TravelType.TravelType4))
                    select p;
            return a;
        }

        protected virtual Entity.Product FindProductEntity(int id)
        {
            Entity.Product entity;
            var query = (from p in ProductDbContext.Product
                         where p.ProductId == id
                         select p);
            entity = query.SingleOrDefault();
            return entity;
        }

        /// <summary>
        /// 根据产品编号查找产品
        /// </summary>
        /// <param name="productNo"></param>
        /// <returns></returns>
        protected virtual Entity.ProductWrapper FindWrappedProduct(string productNo)
        {
            Entity.ProductWrapper entity;
            DateTime today = DateTime.Now.Date;
            var query = (from p in ProductDbContext.Product
                         let tempFirstDepartureDate = EntityFunctions.AddDays(today, p.AdvanceDays)
                         where p.ProductNo == productNo
                         select new Entity.ProductWrapper
                         {
                             Product = p,
                             Prices = p.Product_Price,
                             ClosedDepartureDates = p.Product_NoDeparture,
                             FirstDepartureDate = (p.EffectDate < tempFirstDepartureDate)
                                                  ? tempFirstDepartureDate : p.EffectDate,
                             LastDepartureDate = p.ExpireDate.Value
                         });
            query = from p in query
                    where p.LastDepartureDate >= p.FirstDepartureDate
                    select p;
            entity = query.SingleOrDefault();
            return entity;
        }

        /// <summary>
        /// DO映射到BO
        /// </summary>
        /// <param name="eProduct"></param>
        /// <returns></returns>
        protected virtual Product Map(Entity.Product eProduct)
        {
            if (eProduct == null)
                return null;

            var p = new Product
            {
                ProductId = eProduct.ProductId,
                ProductName = eProduct.ProductName,
                ProductNo = eProduct.ProductNo,
                OldDescription = eProduct.ProductDesc,
                Feature = eProduct.ProductFeature,
                Note = eProduct.ProductNote,
                Days = (eProduct.TripDays ?? 0),
                AdvanceDays = eProduct.AdvanceDays,
                AllowChild = eProduct.AllowChild ?? true,   // 默认允许
                MaxPersonNum = eProduct.MaxPersonNum,
                MinLodgingDays = eProduct.MinLodgingDays,
                MaxDelayDays = (eProduct.ExtendTripDays ?? 0),
                TravelType = (TravelType)eProduct.TripType.Value,
                TravelChildType = (TravelChildType)eProduct.TripClass.Value,
                DepartureCity = eProduct.StartCity,
                DestinationCity = eProduct.DestCity,
                HasDiscount = eProduct.IsBackCash ?? false,
                IsRecommended = eProduct.Recommend ?? false,
            };
            return p;
        }

        protected virtual IEnumerable<Product> Map(IEnumerable<Entity.ProductWrapper> productEntities)
        {
            IEnumerable<Product> products;

            // 产品各图
            var picProvider = new PicturesProvider(ProductDbContext);
            Func<Entity.Picture, ImageInfo> getImageInfo = (pic) =>
            {
                if (pic == null || string.IsNullOrEmpty(pic.URL)) return null;
                return new ImageInfo { Title = pic.Title, Url = pic.URL };
            };

            IQueryable<Entity.ProductWrapper> query = productEntities as IQueryable<Entity.ProductWrapper>;
            if (query != null)
            {
                var q = from p in query
                        join pic in picProvider.FindBy(PictureObjectType.产品)
                        on p.Product.ProductId equals pic.ObjectId into gPics
                        join prop in ProductDbContext.Product_Property
                        on p.Product.ProductId equals prop.ProductId into gProps
                        select new
                        {
                            ProductWrapper = p,
                            Pictures = gPics.DefaultIfEmpty(),
                            Properties = gProps.DefaultIfEmpty(),
                        };
                var list = q.ToList();

                products = list.Select(p =>
                {
                    int productId = p.ProductWrapper.Product.ProductId;
                    var props = p.Properties;
                    var pics = p.Pictures;

                    var product = Map(p.ProductWrapper.Product);

                    if (!props.Any() || props.First() == null)
                    {
                        ExceptionSwitcher<DataInvalidException>.TryThrow(GlobalConfig.IsProductEnvironment,
                            new DataInvalidException(string.Format("product: {0} have not properties", productId)));
                    }
                    else
                    {
                        product.InterestList = GetInterests(props).ToList();

                        //product.DestinationGroupList = GetDestinationGroups(props).ToList();
                    }

                    if (pics.Any())
                    {
                        product.MainImage = getImageInfo(pics.FirstOrDefault(i => i != null && i.Location == PictureLocation.主图));

                        //product.RouteMap = getImageInfo(p.Pictures.FirstOrDefault(i => i.Location == PictureLocation.路线图));
                        //product.MainPhoto = getImageInfo(p.Pictures.FirstOrDefault(i => i.Location == PictureLocation.行程图 && i.Title == "1"));
                        //product.PhotoList = (from pic in p.Pictures.Where(i => i.Location == PictureLocation.行程图 && i.Title != "1")
                        //                     select getImageInfo(pic)).ToList();
                    }

                    // 可出发日期集合
                    var departureDateList = this.GetPriceDateList(p.ProductWrapper);
                    product.DepartureDateList = departureDateList;

                    SetProductMinPrice(product, p.ProductWrapper, departureDateList);

                    return product;
                });
            }
            else
            {
                var list = (from p in productEntities
                            join pic in picProvider.FindBy(PictureObjectType.产品)
                            on p.Product.ProductId equals pic.ObjectId into gPics
                            join prop in ProductDbContext.Product_Property
                            on p.Product.ProductId equals prop.ProductId into gProps
                            select new
                            {
                                ProductWrapper = p,
                                Pictures = gPics.DefaultIfEmpty(),
                                Properties = gProps.DefaultIfEmpty(),
                            }).ToList();

                products = list.Select(p =>
                {
                    int productId = p.ProductWrapper.Product.ProductId;
                    var props = p.Properties;
                    var pics = p.Pictures;

                    var product = Map(p.ProductWrapper.Product);

                    if (!props.Any() || props.First() == null)
                    {
                        ExceptionSwitcher<DataInvalidException>.TryThrow(GlobalConfig.IsProductEnvironment,
                            new DataInvalidException(string.Format("product: {0} have not properties", productId)));
                    }
                    else
                    {
                        product.InterestList = GetInterests(props).ToList();

                        //product.DestinationGroupList = GetDestinationGroups(props).ToList();
                    }

                    if (pics.Any())
                    {
                        product.MainImage = getImageInfo(pics.FirstOrDefault(i => i != null && i.Location == PictureLocation.主图));

                        //product.RouteMap = getImageInfo(p.Pictures.FirstOrDefault(i => i.Location == PictureLocation.路线图));
                        //product.MainPhoto = getImageInfo(p.Pictures.FirstOrDefault(i => i.Location == PictureLocation.行程图 && i.Title == "1"));
                        //product.PhotoList = (from pic in p.Pictures.Where(i => i.Location == PictureLocation.行程图 && i.Title != "1")
                        //                     select getImageInfo(pic)).ToList();
                    }

                    // 可出发日期集合
                    var departureDateList = this.GetPriceDateList(p.ProductWrapper);
                    product.DepartureDateList = departureDateList;

                    SetProductMinPrice(product, p.ProductWrapper, departureDateList);

                    return product;
                });
            }
            return products;
        }

        #region 分页

        protected virtual IQueryable<Entity.ProductWrapper> LazyPage(IQueryable<Entity.ProductWrapper> query, Pager pager)
        {
            if (pager == null)
            {
                return query;
            }

            // 排序
            if (pager.SortRule == SortRule.ProductDefault)
            {
                query = query.OrderByDescending(p => p.Product.Weight)
                                .ThenBy(p => p.Product.EffectDate)
                                .ThenBy(p => p.Product.TripDays);
            }
            else
            {
                if (pager.SortRule == SortRule.ProductTripDays)
                {
                    query = (pager.Descending) ? query.OrderByDescending(p => p.Product.TripDays) : query.OrderBy(p => p.Product.TripDays);
                }
                else if (pager.SortRule == SortRule.ProductMinDepartureDate)
                {
                    query = (pager.Descending) ? query.OrderByDescending(p => p.Product.EffectDate) : query.OrderBy(p => p.Product.EffectDate);
                }
                else if (pager.SortRule == SortRule.ProductMinPrice)
                {
                    query = (pager.Descending) ? query.OrderByDescending(p => p.MinPrice) : query.OrderBy(p => p.MinPrice);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("pager.SortRule");
                }
            }

            int pageCount;
            query = PagerHelper.LazyTakePage(query, pager.PageSize, pager.PageIndex, out pageCount);
            pager.PageCount = pageCount;

            return query;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="query"></param>
        /// <param name="sort"></param>
        /// <param name="asc"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        protected virtual IEnumerable<Entity.ProductWrapper> Page(IEnumerable<Entity.ProductWrapper> query, Pager pager)
        {
            if (pager == null)
            {
                throw new ArgumentNullException("pager");
            }

            // 排序
            if (pager.SortRule == SortRule.ProductDefault)
            {
                query = query.OrderByDescending(p => p.Product.Weight)
                                .ThenBy(p => p.Product.EffectDate)
                                .ThenBy(p => p.Product.TripDays);
            }
            else
            {
                if (pager.SortRule == SortRule.ProductTripDays)
                {
                    query = (pager.Descending) ? query.OrderByDescending(p => p.Product.TripDays) : query.OrderBy(p => p.Product.TripDays);
                }
                else if (pager.SortRule == SortRule.ProductMinDepartureDate)
                {
                    query = (pager.Descending) ? query.OrderByDescending(p => p.Product.EffectDate) : query.OrderBy(p => p.Product.EffectDate);
                }
                else if (pager.SortRule == SortRule.ProductMinPrice)
                {
                    query = (pager.Descending) ? query.OrderByDescending(p => p.MinPrice) : query.OrderBy(p => p.MinPrice);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("pager.SortRule");
                }
            }

            int pageCount;
            query = PagerHelper.TakePage(query, pager.PageSize, pager.PageIndex, out pageCount);
            pager.PageCount = pageCount;

            return query;
        }

        #endregion 分页

        /// <summary>
        /// 获取产品的最低价
        /// </summary>
        /// <param name="pw"></param>
        protected virtual void SetProductMinPrice(Product product, Entity.ProductWrapper pw, List<PriceDate> departureDateList)
        {
            int productId = pw.Product.ProductId;
            if (!pw.Prices.Any())
            {
                ExceptionSwitcher<DataInvalidException>.TryThrow(GlobalConfig.IsProductEnvironment,
                    new DataInvalidException(string.Format("product: {0} have not prices", productId)));
            }
            else
            {
                if (departureDateList.Any())
                {
                    var effectPrices = pw.Prices.Where(pr => pr.ExpireDate >= departureDateList.Min(dd => dd.Date));
                    if (effectPrices.Any())
                    {
                        product.MinPrice = effectPrices.Min(pr => pr.Price);
                        product.MinMarketPrice = effectPrices.Min(pr => pr.PriceMarketing);
                    }
                }
            }
        }

        #endregion protected virtual

        public virtual IEnumerable<FlightsSegment> GetFlightsSegments(int productId)
        {
            throw new NotImplementedException();
        }
    }
}