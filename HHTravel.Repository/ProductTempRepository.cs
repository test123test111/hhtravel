using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using HHTravel.CRM.Booking_Online.DataAccess.HardCode;
using HHTravel.CRM.Booking_Online.DataAccess.Providers;
using HHTravel.CRM.Booking_Online.DomainModel;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;
using HHTravel.CRM.Booking_Online.Infrastructure.Exceptions;
using HHTravel.CRM.Booking_Online.Infrastructure;

namespace HHTravel.CRM.Booking_Online.Repository
{
    public class ProductTempRepository : ProductRepository
    {
        public override IEnumerable<Hotel> GetHotels(int productId)
        {
            var picProvider = new PicturesProvider(ProductDbContext);

            // ProductId->*子产品1->1产品(酒店)
            var query = from hotelProduct in this.AvailableHotels
                        join attach in ProductDbContext.Product_Attach_Product
                        on hotelProduct.ProductId equals attach.ItemProductId

                        // left join 取酒店图片
                        join pic in picProvider.FindBy(PictureObjectType.产品, null) // todo: null 需要修改为对应酒店图片的"Location"
                        on hotelProduct.ProductId equals pic.ObjectId into gPic
                        where attach.ProductId == productId

                        // 按行程段的顺序排序，同一行程段按段内序号排序
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
                                         where pic != null
                                         select new ImageInfo { Title = pic.Title, Url = pic.URL }
                         };

            var hotelWrapperList = query.ToList();

            foreach (var hotelWrapper in hotelWrapperList)
            {
                // 获取酒店的照片
                hotelWrapper.Hotel.PhotoList = hotelWrapper.PhotoList.ToList();
            }

            // 去重
            var c = hotelWrapperList.GroupBy(hd => hd.Hotel.HotelId).Select(g => g.First().Hotel);
            return c;
        }

        public override IEnumerable<HotelSegment> GetHotelSegments(int productId)
        {
            var queryAttachesPriceCache = from prc in this.QueryAttachesPriceCache(productId)
                                          select prc;
            var noCache = (queryAttachesPriceCache.Count() == 0);
            if (noCache)
            {
                return new List<HotelSegment>();
            }

            // [Obsolete]行程段：ProductId->*行程段，再根据其下是否有酒店，判定是否为期望行程段
            // 行程段：ProductId->*行程段，根据行程段类型（酒店、线路、延住）筛选出的即为期望行程段 （行程段类型为TravelType3新增）
            string[] hotelSegmentTypes = new string[] { "酒店", "线路", "延长住宿" };
            var query = from seg in ProductDbContext.Product_Section

                        // left join 找子产品
                        join attach in ProductDbContext.Product_Attach_Product
                        on seg.SectionId equals attach.SectionId into gAttach

                        // 相当于join 时按sectionId 和productId 两个条件进行
                        let gAttach2 = gAttach.Where(a => a.ProductId == seg.ProductId)

                        // left join 取段的日价（最低价）
                        join prc in queryAttachesPriceCache
                        on seg.SectionId equals prc.SectionID into gPriceDate

                        where seg.ProductId == productId && hotelSegmentTypes.Contains(seg.SectionType)
                        orderby seg.SeqNo   // 行程段排序
                        select new
                        {
                            SegmentId = seg.SectionId,
                            DestinationCity = seg.DestCity,
                            Nights = seg.MinLodgingDays.Value,
                            SegmentType = seg.SectionType,
                            Description = seg.SectionDesc,

                            // 酒店：行程段1->*子产品1->1产品（酒店）
                            Hotels = (from a in gAttach2
                                      join hotelProduct in ProductDbContext.Product
                                      on a.ItemProductId equals hotelProduct.ProductId
                                      join spec in ProductDbContext.Product_Spec
                                      on hotelProduct.ProductId equals spec.ProductId into gSpec   // 房型
                                      orderby a.SeqNo   // 酒店排序
                                      select new
                                      {
                                          HotelId = hotelProduct.ProductId,
                                          HotelName = hotelProduct.ProductName,
                                          Description = hotelProduct.ProductFeature,
                                          RoomClasses = (from spec in gSpec
                                                         join pr in ProductDbContext.Product_Price
                                                         on spec.SpecId equals pr.SpecId into gPrice
                                                         select new
                                                         {
                                                             RoomClassId = spec.SpecId,
                                                             RoomClassName = spec.SpecType,
                                                             BreakfastDinnerTip = spec.SpecNote,
                                                             MaxOccupancy = spec.MaxPersonNum ?? 0,
                                                             PriceRows = gPrice,     // 房型日价,
                                                         })
                                      }),
                            PriceDates = (from pd in gPriceDate
                                          select new PriceDate
                                          {
                                              Date = pd.PriceDate,
                                              Price = pd.MinPrice
                                          }),
                        };
            var list = query.ToList();
            var q2 = (from segData in list
                      where segData.Hotels.Count() > 0
                      select new HotelSegment
                      {
                          SegmentId = segData.SegmentId,
                          City = segData.DestinationCity,
                          Nights = segData.Nights,
                          HotelSegmentType = (segData.SegmentType == "酒店" ? HotelSegmentType.酒店 :
                                                    (segData.SegmentType == "延长住宿" ? HotelSegmentType.延长住宿 : HotelSegmentType.线路)),
                          Description = segData.Description,
                          HotelList = (from hotelData in segData.Hotels
                                       select new Hotel
                                       {
                                           HotelId = hotelData.HotelId,
                                           HotelName = hotelData.HotelName,
                                           Description = hotelData.Description,
                                           RoomClassList = (from rcData in hotelData.RoomClasses
                                                            select new RoomClass
                                                            {
                                                                RoomClassId = rcData.RoomClassId,
                                                                RoomClassName = rcData.RoomClassName,
                                                                BreakfastDinnerTip = rcData.BreakfastDinnerTip,
                                                                MaxOccupancy = rcData.MaxOccupancy,
                                                                PriceDateList = (from pr in rcData.PriceRows
                                                                                 from date in new DateRange(pr.EffectDate, pr.ExpireDate)
                                                                                 group pr by date into g
                                                                                 select new PriceDate
                                                                                 {
                                                                                     Date = g.Key,
                                                                                     Price = g.Min(pr => pr.Price),
                                                                                 }).ToList(),
                                                            }).ToList(),
                                       }).ToList(),
                          PriceDateList = (from pdData in segData.PriceDates
                                           select pdData).ToList(),
                      }).ToList();
            return q2;
        }

        public override IEnumerable<Ticket> GetTickets(int productId)
        {
            var query = from ticket in this.AllTickets()
                        join attach in ProductDbContext.Product_Attach_Product
                        on ticket.ProductId equals attach.ItemProductId
                        join prc in ProductDbContext.Product_AttachPrice_Cache    // 只有TravelType3、TravelType2的机票才有价格
                        on ticket.ProductId equals prc.ProductId into gPriceDate
                        join pf in this.ProductDbContext.Product_Flight
                        on ticket.ProductId equals pf.ProductId into gFlight
                        where attach.ProductId == productId

                        // todo: orderby attach.SectionID , attach.SeqNo
                        select new
                        {
                            Ticket = new Ticket
                            {
                                TicketId = attach.ProductId,
                                FlightSeatName = ticket.ProductFeature,
                                AdditionalPurchasesNote = ticket.ProductDesc,
                            },
                            PriceDatesCache = gPriceDate,
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
                                       select new
                                       {
                                           IsGo = (pf.Direction == "去程"),
                                           AirlineName = airline != null ? airline.CnName : pf.AirLine,
                                           pf.FlightNo,
                                           DepartAirport = new { AirportName = dport != null ? dport.CnName : pf.Dport },
                                           ArrivalAirport = new { AirportName = aport != null ? aport.CnName : pf.Aport },
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

            int hour = DateTime.Now.Hour;
            foreach (var ticketWrapper in tList)
            {
                ticketWrapper.Ticket.PriceDateList = (from pd in ticketWrapper.PriceDatesCache
                                                      where pd.CacheHour == hour
                                                      select new PriceDate
                                                      {
                                                          Date = pd.PriceDate,
                                                          Price = pd.MinPrice
                                                      }).ToList();
                ticketWrapper.Ticket.FlightList = (from f in ticketWrapper.Flights
                                                   select new Flight
                                                   {
                                                       IsGo = f.IsGo,
                                                       Airline = new Airline { AirlineName = f.AirlineName },
                                                       FlightNo = f.FlightNo,
                                                       DepartAirport = new Airport
                                                       {
                                                           AirportName = f.DepartAirport.AirportName,
                                                       },
                                                       ArrivalAirport = new Airport
                                                       {
                                                           AirportName = f.ArrivalAirport.AirportName,
                                                       },
                                                       DepartTime = f.DepartTime,
                                                       ArrivalTime = f.ArrivalTime,
                                                   }).ToList();
            }
            var c = tList.Select(f => f.Ticket).ToList();
            return c;
        }

        public override TicketSegment GetTicketSegment(int productId)
        {
            var queryAttachesPriceCache = from prc in this.QueryAttachesPriceCache(productId)
                                          select prc;
            var noCache = (queryAttachesPriceCache.Count() == 0);
            if (noCache)
            {
                return null;
            }

            var query = from sec in ProductDbContext.Product_Section
                        join pap in ProductDbContext.Product_Attach_Product
                        on sec.SectionId equals pap.SectionId into gAttach
                        let gAttach2 = gAttach.Where(a => a.ProductId == sec.ProductId)
                        join prc in queryAttachesPriceCache
                        on sec.SectionId equals prc.SectionID into gPriceDate
                        where sec.ProductId == productId
                        select new
                        {
                            SegmentId = sec.SectionId,
                            Tickets = (from a in gAttach2
                                       join p in ProductDbContext.Product
                                       on a.ItemProductId equals p.ProductId
                                       join pr in ProductDbContext.Product_Price
                                       on p.ProductId equals pr.ProductId into gPrice
                                       join pf in this.ProductDbContext.Product_Flight
                                       on p.ProductId equals pf.ProductId into gFlight
                                       where p.ResourceType == (int)ProductResourceType.航班
                                       select new
                                       {
                                           TicketId = p.ProductId,
                                           FlightClassName = p.ProductFeature,
                                           AdditionalPurchasesNote = p.ProductDesc,
                                           PriceRows = gPrice,
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
                                                      select new
                                                      {
                                                          IsGo = (pf.Direction == "去程"),
                                                          Airline = new { AirlineName = airline != null ? airline.CnName : pf.AirLine },
                                                          FlightNo = pf.FlightNo,
                                                          DepartAirport = new { AirportName = dport != null ? dport.CnName : pf.Dport },
                                                          ArrivalAirport = new { AirportName = aport != null ? aport.CnName : pf.Aport },
                                                          DepartTime = pf.Dtime ?? default(TimeSpan),
                                                          ArrivalTime = pf.Atime ?? default(TimeSpan),
                                                      }),
                                       }),
                            PriceDates = (from pd in gPriceDate
                                          select new PriceDate
                                          {
                                              Date = pd.PriceDate,
                                              Price = pd.MinPrice
                                          }),
                        };
            var list = query.ToList();

            var q2 = (from segData in list
                      where segData.Tickets.Count() > 0
                      select new TicketSegment
                      {
                          SegmentId = segData.SegmentId,
                          TicketList = (from ticketData in segData.Tickets
                                        select new Ticket
                                        {
                                            TicketId = ticketData.TicketId,
                                            FlightSeatName = ticketData.FlightClassName,
                                            AdditionalPurchasesNote = ticketData.AdditionalPurchasesNote,
                                            FlightList = (from f in ticketData.Flights
                                                          select new Flight
                                                          {
                                                              IsGo = f.IsGo,
                                                              Airline = new Airline { AirlineName = f.Airline.AirlineName },
                                                              FlightNo = f.FlightNo,
                                                              DepartAirport = new Airport
                                                              {
                                                                  AirportName = f.DepartAirport.AirportName,
                                                              },
                                                              ArrivalAirport = new Airport
                                                              {
                                                                  AirportName = f.ArrivalAirport.AirportName,
                                                              },
                                                              DepartTime = f.DepartTime,
                                                              ArrivalTime = f.ArrivalTime,
                                                          }).ToList(),
                                            PriceDateList = (from pr in ticketData.PriceRows
                                                             from date in new DateRange(pr.EffectDate, pr.ExpireDate)
                                                             group pr by date into g
                                                             select new PriceDate
                                                             {
                                                                 Date = g.Key,
                                                                 Price = g.Min(pr => pr.Price),
                                                             }).ToList(),
                                        }).ToList(),
                          PriceDateList = (from pdData in segData.PriceDates
                                           select pdData).ToList(),
                      }).FirstOrDefault();
            return q2;
        }

        internal override IQueryable<Entity.ProductWrapper> AllWrapperedProducts()
        {
            DateTime today = DateTime.Now.Date;

            var query = (from p in ProductDbContext.Product
                         where p.ResourceType == (int)ProductResourceType.产品
                         && p.ProductNo != ""
                         && p.ExpireDate.HasValue
                         && p.EffectDate.HasValue
                         && p.IsValid.HasValue && p.IsValid.Value
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

            var query2 = from p in query
                         join prc in this.QueryProductsPriceCache()
                         on p.ProductId equals prc.ProductId into gPriceDate
                         where gPriceDate.Count() > 0
                         let minDate = gPriceDate.Min(dd => dd.PriceDate)
                         let maxDate = gPriceDate.Max(dd => dd.PriceDate)
                         let tempFirstDepartureDate = EntityFunctions.AddDays(today, p.AdvanceDays)
                         let firstDepartureDate = (minDate < tempFirstDepartureDate) ? tempFirstDepartureDate : minDate
                         let lastDepartureDate = maxDate
                         select new Entity.ProductWrapper
                         {
                             FirstDepartureDate = firstDepartureDate,
                             LastDepartureDate = lastDepartureDate,
                             Product = p,
                             PricesCache = gPriceDate,
                             MinPrice = gPriceDate.Min(pc => pc.MinPrice)
                         };
            return query2;
        }

        internal override List<PriceDate> GetPriceDateList(Entity.ProductWrapper product)
        {
            var query = from prc in product.PricesCache
                        select new PriceDate
                        {
                            Date = prc.PriceDate,
                            Price = prc.MinPrice,
                            ProductBasicPrice = (prc.MinBasePrice ?? 0),
                            MinGroupNum = (prc.MinPersonNum ?? 0),
                        };
            return query.ToList();
        }

        protected override Entity.ProductWrapper FindWrappedProduct(string productNo)
        {
            Entity.ProductWrapper entity = null;

            Entity.Product prod;
            List<Entity.Product_Price_Cache> prodPriceCacheList;

            var ps = (from p in ProductDbContext.Product
                      where p.ProductNo == productNo
                      select p);
            prod = ps.FirstOrDefault();
            if (prod == null) return entity;

            var prcs = from prc in this.QueryProductsPriceCache()
                       where prc.ProductId == prod.ProductId
                       select prc;
            var noCache = (prcs.Count() == 0);
            if (noCache)
            {
                return entity;
            }

            prodPriceCacheList = prcs.ToList();

            var minDate = prodPriceCacheList.Min(dd => dd.PriceDate);
            var maxDate = prodPriceCacheList.Max(dd => dd.PriceDate);
            DateTime today = DateTime.Now.Date;
            var tempFirstDepartureDate = today.AddDays(prod.AdvanceDays ?? 0);
            var firstDepartureDate = (minDate < tempFirstDepartureDate) ? tempFirstDepartureDate : minDate;
            var lastDepartureDate = maxDate;

            entity = new Entity.ProductWrapper
                         {
                             Product = prod,
                             FirstDepartureDate = firstDepartureDate,
                             LastDepartureDate = lastDepartureDate,
                             PricesCache = prodPriceCacheList,
                             MinPrice = prodPriceCacheList.Min(pc => pc.MinPrice)
                         };

            return entity;
        }

        protected override IEnumerable<Entity.ProductWrapper> SearchByDepartureDate(IQueryable<Entity.ProductWrapper> query, DateTime? beginDate, DateTime? endDate)
        {
            var a = from pw in query
                    join pr in this.QueryProductsPriceCache()
                    on pw.Product.ProductId equals pr.ProductId into gDepartureDate // 按出发日期分组
                    where gDepartureDate.Any(dd => dd.PriceDate >= beginDate && dd.PriceDate <= endDate)
                    select pw;
            return a;
        }

        protected override void SetProductMinPrice(Product product, Entity.ProductWrapper pw, List<PriceDate> departureDateList)
        {
            product.MinPrice = pw.MinPrice;

            // todo: product.MinMarketPrice
        }

        public override IEnumerable<FlightsSegment> GetFlightsSegments(int productId)
        {
            // ?为什么先判断
            var queryAttachesPriceCache = from prc in this.QueryAttachesPriceCache(productId)
                                          select prc;
            var noCache = (queryAttachesPriceCache.Count() == 0);
            if (noCache)
            {
                return null;
            }

            var query = from sec in ProductDbContext.Product_Section
                        join attachPriceCache in queryAttachesPriceCache
                        on sec.SectionId equals attachPriceCache.SectionID into gPriceDate
                        join attach in ProductDbContext.Product_Attach_Product
                        on sec.SectionId equals attach.SectionId into gAttach

                        // 确保航程顺序
                        orderby sec.SeqNo
                        where sec.ProductId == productId && sec.SectionType == "系统机票"
                        select new
                        {
                            DepartCityName = sec.StartCity,
                            DepartCityCode = sec.StartCityCode,
                            ArrivalCityName = sec.DestCity,
                            ArrivalCityCode = sec.DestCityCode,
                            AdjustableDays = sec.DepartureAdjust ?? 0,
                        };

            int segmentNo = 1;
            var segs = from aSeg in query.ToList()
                       select new FlightsSegment
                       {
                           SegmentNo = segmentNo++,
                           DepartCity = new City { CityName = aSeg.DepartCityName, CityCode = aSeg.DepartCityCode },
                           ArrivalCity = new City { CityName = aSeg.ArrivalCityName, CityCode = aSeg.ArrivalCityCode },
                           AdjustableDays = aSeg.AdjustableDays,
                       };

            return segs;
        }

        public override GroundServiceSegment GetGroundServiceSegment(int productId)
        {
            // ?为什么先判断
            var queryAttachesPriceCache = from prc in this.QueryAttachesPriceCache(productId)
                                          select prc;
            var noCache = (queryAttachesPriceCache.Count() == 0);
            if (noCache)
            {
                return null;
            }

            var query = from sec in ProductDbContext.Product_Section
                        join attachPriceCache in queryAttachesPriceCache
                        on sec.SectionId equals attachPriceCache.SectionID into gPriceDate
                        join attach in ProductDbContext.Product_Attach_Product
                        on sec.SectionId equals attach.SectionId into gAttach

                        where sec.ProductId == productId && sec.SectionType == "地面"

                        select new
                        {
                            GroundServices = from attach in gAttach
                                             join childProduct in ProductDbContext.Product
                                             on attach.ItemProductId equals childProduct.ProductId
                                             join pr in ProductDbContext.Product_Price
                                             on childProduct.ProductId equals pr.ProductId into gPrice2
                                             select new
                                             {
                                                 GroundServiceId = childProduct.ProductId,
                                                 ServiceName = childProduct.ProductName,
                                                 Description = childProduct.ProductFeature,
                                                 PriceRows = gPrice2,
                                             },
                            PriceDates = (from pd in gPriceDate
                                          select new PriceDate
                                          {
                                              Date = pd.PriceDate,
                                              Price = pd.MinPrice,
                                              ChildPrice = pd.MinPriceChild ?? pd.MinPrice,
                                          }),
                        };

            // 一个产品只有一个地面服务段
            var aSeg = query.FirstOrDefault();
            if (aSeg == null)
            {
                return null;
            }

            var seg = new GroundServiceSegment
            {
                GroundServiceList = (from gs in aSeg.GroundServices
                                     select new GroundService
                                     {
                                         GroundServiceId = gs.GroundServiceId,
                                         ServiceName = gs.ServiceName,
                                         Description = gs.Description,
                                         PriceDateList = (from prs in gs.PriceRows
                                                          from date in new DateRange(prs.EffectDate, prs.ExpireDate)
                                                          select new PriceDate
                                                          {
                                                              Date = date,
                                                              Price = prs.Price,
                                                              StayPricePerDay = (prs.PriceStay ?? 0),
                                                              ChildPrice = (prs.PriceChild ?? 0),
                                                              ChildStayPricePerDay = (prs.PriceStayChild ?? 0),
                                                          }).ToList()
                                     }).ToList(),
                PriceDateList = (from pdData in aSeg.PriceDates
                                 select pdData).ToList(),
            };

            return seg;
        }

        #region 对缓存表作预筛选

        private IQueryable<Entity.Product_AttachPrice_Cache> QueryAttachesPriceCache(int productId)
        {
            Func<ProductDbEntities, IQueryable<Entity.Product_AttachPrice_Cache>> getQuery = (dbContext) =>
            {
                return from ppc in this.ProductDbContext.Product_AttachPrice_Cache
                       where ppc.ProductId == productId
                       select ppc;
            };

            // 10分钟为一个段 [13分的minSecId = 1] [59分的minSecId = 5] [03分的minSecId = 0]
            int minSecId = DateTime.Now.Minute / 10;
            var query = getQuery(this.ProductDbContext);
            if (query.Count(prc => prc.CacheHour == minSecId) == 0)   // 当前时间没有数据，则取最小时间
            {
                var queryNoCached = getQuery(this.ProductDbContext);
                minSecId = queryNoCached.Min(prc => (short?)prc.CacheHour) ?? 0;
            }

            query = from ppc in query
                    where ppc.CacheHour == minSecId
                    select ppc;
            return query;
        }

        private IQueryable<Entity.Product_Price_Cache> QueryProductsPriceCache()
        {
            Func<ProductDbEntities, IQueryable<Entity.Product_Price_Cache>> getQuery = (dbContext) =>
            {
                return from ppc in dbContext.Product_Price_Cache
                       where (ppc.MinBasePrice.Value > 0 && ppc.MinAttachPrice == 0) || (ppc.IsComplete ?? false)
                       select ppc;
            };

            // 10分钟为一个段 [13分的minSecId = 1] [59分的minSecId = 5] [03分的minSecId = 0]
            int minuteSecId = DateTime.Now.Minute / 10;
            var query = getQuery(this.ProductDbContext);
            if (query.Count(prc => prc.CacheHour == minuteSecId) == 0)   // 当前时间没有数据，则取最小时间
            {
                //SysLog.WriteTrace("PriceCache Miss1", string.Format("minuteSecId: {0}, Server Time: {1}", minuteSecId, DateTime.Now));

                var queryNoCached = getQuery(this.ProductDbContext);
                minuteSecId = queryNoCached.Min(prc => prc.CacheHour);

                //SysLog.WriteTrace("PriceCache get minMinuteSecId", string.Format("minuteSecId: {0}, Server Time: {1}", minuteSecId, DateTime.Now));
            }

            query = from ppc in query
                    where ppc.CacheHour == minuteSecId
                    select ppc;

            return query;
        }

        #endregion 对缓存表作预筛选
    }
}