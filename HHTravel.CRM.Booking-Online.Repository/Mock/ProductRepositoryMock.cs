using System.Collections.Generic;
using System.Linq;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using HHTravel.CRM.Booking_Online.DataAccess.HardCode;
using HHTravel.CRM.Booking_Online.DataAccess.Mock;
using HHTravel.CRM.Booking_Online.DomainModel;
using HHTravel.CRM.Booking_Online.Entity;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;
using HHTravel.CRM.Booking_Online.Infrastructure.Mock;

namespace HHTravel.CRM.Booking_Online.Repository.Mock
{
    public class ProductRepositoryMock : ProductRepository
    {
        private const string c_cacheFilePath = @"D:/log/hhtravel/products.txt";
        private List<Product_Property> _ppList = new List<Product_Property>();
        private List<DomainModel.Product> _productList = new List<DomainModel.Product>();
        private List<string> _productNoList = new List<string>();

        public ProductRepositoryMock()
            : base()
        {
            //Init();
        }

        //private static IEnumerable<Model.Product> s_Cache = null;

        //public void RefreshCache()
        //{
        //    File.Delete(c_cacheFilePath);
        //    this.Init();
        //}

        //public override IEnumerable<Model.Product> All()
        //{
        //    return _productList;
        //}

        //public override IEnumerable<Model.Product> FindByDepartureDate(DateTime beginDate, DateTime endDate)
        //{
        //    return base.FindByDepartureDate(beginDate, endDate);
        //}

        /// <summary>
        /// 获取所有（有图片的）酒店
        /// </summary>
        /// <param name="productId">!Ignore</param>
        /// <returns></returns>
        public override IEnumerable<Hotel> GetHotels(int productId)
        {
            PicturesProviderMock repoMockPic = new PicturesProviderMock(ProductDbContext);
            ProductDbEntities dbContext = (ProductDbEntities)ProductDbContext;

            var a = (from pw in this.AllWrapperedProducts()
                     let p = pw.Product
                     join attach in dbContext.Product_Attach_Product
                     on p.ProductId equals attach.ItemProductId
                     where p.ResourceType == (int)ProductResourceType.酒店       // 酒店
                            && attach.ProductId == productId
                     select new Hotel
                     {
                         HotelId = p.ProductId,
                         HotelName = p.ProductName,
                         Description = p.ProductDesc,
                         Feature = p.ProductFeature,
                         Url = p.Link,
                     });

            var hotelList = a.ToList();
            foreach (var hotel in hotelList)
            {
                var b = repoMockPic.FindBy(hotel.HotelId, "产品", null)   // todo: null 需要修改为对应酒店图片的"Location"
                           .Select(pic => new ImageInfo
                           {
                               Title = pic.Title,
                               Url = pic.URL,
                           });
                hotel.PhotoList = b.ToList();
            }

            return hotelList;
        }

        protected override IEnumerable<Interest> GetInterests(IEnumerable<Entity.Product_Property> props)
        {
            var listInterest = new InterestRepository(ProductDbContext).All();
            var list = MockHelper.GetRandomList<Interest>(listInterest);
            return list;
        }

        protected override IEnumerable<ImageInfo> GetPhotos(int productId)
        {
            IList<ImageInfo> list = new List<ImageInfo> {
                new ImageInfo{ Url="http://www.eztravel.com.tw/img/pm/FRN/FRN0000009484D01.gif", Title="1"},
                new ImageInfo{ Url="http://www.eztravel.com.tw/img/pm/FRN/FRN0000009484D02.gif", Title="图片描述2"},
                new ImageInfo{ Url="http://www.eztravel.com.tw/img/pm/FRN/FRN0000009484D03.gif", Title="图片描述3"},
                new ImageInfo{ Url="http://www.eztravel.com.tw/img/pm/FRN/FRN0000009484D04.gif", Title="图片描述4"},
                new ImageInfo{ Url="http://www.eztravel.com.tw/img/pm/FRN/FRN0000009484D05.gif" , Title="图片描述5"},
                new ImageInfo{ Url="http://www.eztravel.com.tw/img/pm/FRN/FRN0000009484D01.gif", Title="图片描述6"},
                new ImageInfo{ Url="http://www.eztravel.com.tw/img/pm/FRN/FRN0000009484D02.gif", Title="图片描述7"},
                new ImageInfo{ Url="http://www.eztravel.com.tw/img/pm/FRN/FRN0000009484D03.gif", Title="图片描述8"},
                new ImageInfo{ Url="http://www.eztravel.com.tw/img/pm/FRN/FRN0000009484D04.gif", Title="图片描述9"},
                new ImageInfo{ Url="http://www.eztravel.com.tw/img/pm/FRN/FRN0000009484D05.gif" , Title="图片描述10"},
            };
            list = MockHelper.GetRandomList<ImageInfo>(productId, list);
            return list;
        }

        protected override IEnumerable<ScheduleItem> GetScheduleItems(int productId)
        {
            var scheduleItemList = new List<ScheduleItem> {
                        #region ScheduleItemList

		                new ScheduleItem{
                            Sort = 1,
                            Name = "上海/巴黎/波尔多 Bonjour France！",
                            Infos = new ScheduleItemInfos {
                                SpotsInfo    = "上午展开蔚蓝海岸嘉年华之都尼斯市区观光。   参观在海滨大道的旁边的☆尼斯花市、★夏卡尔美术馆。开车经过英国人散步道Promenade des Anglais，八线大道长达五公里，沿途艺廊、商店林立。   下午返回蒙地卡罗。自由活动，可于在酒店享用设施，或漫步于商店林立的街道，或前往赌城试手气，或悠闲地享受阳光与沙滩。",
                                CateringInfo = "早餐：酒店自助式早餐　　午餐：蔚蓝海岸地方美食料　　晚餐：米其林1星日餐",
                                HotelInfo    = "Hotel Metropole",
                            }
                        },
                        new ScheduleItem{
                            Sort = 2,
                            Name = "波尔多 顶级葡萄酒之旅",
                            Infos = new ScheduleItemInfos {
                                SpotsInfo    = "全日鹫巢村落一日游。   首先前往有世上最美丽村庄－圣保罗（41km/约1小时），漫步村内宛如迷宫曲折小径，居高临下俯瞰地中海。   参观举世最佳小型现代美术馆之一，隐身在圣保罗山丘的金松林间的★马格财团美术馆。   午后前往凡斯（5.2km/约10分钟），参观由野兽派巨匠马蒂斯规划设计的★罗萨利玫瑰经教堂Chapelle du Rosair。   傍晚返回蒙特卡洛（45km/约1小时）。",
                                CateringInfo = "早餐：酒店自助式早餐　　午餐：圣保罗农庄餐厅　　晚餐：法国海鲜餐",
                                HotelInfo    = "Hotel Metropole",
                            },
                        },
                        new ScheduleItem{
                            Sort = 3,
                            Name = "波尔多 顶级葡萄酒之旅",
                            Infos = new ScheduleItemInfos {
                                SpotsInfo    = "全日鹫巢村落一日游。   首先前往有世上最美丽村庄－圣保罗（41km/约1小时），漫步村内宛如迷宫曲折小径，居高临下俯瞰地中海。   参观举世最佳小型现代美术馆之一，隐身在圣保罗山丘的金松林间的★马格财团美术馆。   午后前往凡斯（5.2km/约10分钟），参观由野兽派巨匠马蒂斯规划设计的★罗萨利玫瑰经教堂Chapelle du Rosair。   傍晚返回蒙特卡洛（45km/约1小时）。",
                                CateringInfo = "早餐：酒店自助式早餐　　午餐：圣保罗农庄餐厅　　晚餐：法国海鲜餐",
                                HotelInfo    = "Hotel Metropole",
                            },
                        },
                        new ScheduleItem{
                            Sort = 4,
                            Name = "波尔多 顶级葡萄酒之旅",
                            Infos = new ScheduleItemInfos {
                                SpotsInfo    = "全日鹫巢村落一日游。   首先前往有世上最美丽村庄－圣保罗（41km/约1小时），漫步村内宛如迷宫曲折小径，居高临下俯瞰地中海。   参观举世最佳小型现代美术馆之一，隐身在圣保罗山丘的金松林间的★马格财团美术馆。   午后前往凡斯（5.2km/约10分钟），参观由野兽派巨匠马蒂斯规划设计的★罗萨利玫瑰经教堂Chapelle du Rosair。   傍晚返回蒙特卡洛（45km/约1小时）。",
                                CateringInfo = "早餐：酒店自助式早餐　　午餐：圣保罗农庄餐厅　　晚餐：法国海鲜餐",
                                HotelInfo    = "Hotel Metropole",
                            },
                        },
                        new ScheduleItem{
                            Sort = 5,
                            Name = "波尔多 顶级葡萄酒之旅",
                            Infos = new ScheduleItemInfos {
                                SpotsInfo    = "全日鹫巢村落一日游。   首先前往有世上最美丽村庄－圣保罗（41km/约1小时），漫步村内宛如迷宫曲折小径，居高临下俯瞰地中海。   参观举世最佳小型现代美术馆之一，隐身在圣保罗山丘的金松林间的★马格财团美术馆。   午后前往凡斯（5.2km/约10分钟），参观由野兽派巨匠马蒂斯规划设计的★罗萨利玫瑰经教堂Chapelle du Rosair。   傍晚返回蒙特卡洛（45km/约1小时）。",
                                CateringInfo = "早餐：酒店自助式早餐　　午餐：圣保罗农庄餐厅　　晚餐：法国海鲜餐",
                                HotelInfo    = "Hotel Metropole",
                            },
                        },
                        new ScheduleItem{
                            Sort = 6,
                            Name = "波尔多 顶级葡萄酒之旅",
                            Infos = new ScheduleItemInfos {
                                SpotsInfo    = "全日鹫巢村落一日游。   首先前往有世上最美丽村庄－圣保罗（41km/约1小时），漫步村内宛如迷宫曲折小径，居高临下俯瞰地中海。   参观举世最佳小型现代美术馆之一，隐身在圣保罗山丘的金松林间的★马格财团美术馆。   午后前往凡斯（5.2km/约10分钟），参观由野兽派巨匠马蒂斯规划设计的★罗萨利玫瑰经教堂Chapelle du Rosair。   傍晚返回蒙特卡洛（45km/约1小时）。",
                                CateringInfo = "早餐：酒店自助式早餐　　午餐：圣保罗农庄餐厅　　晚餐：法国海鲜餐",
                                HotelInfo    = "Hotel Metropole",
                            },
                        },
                        new ScheduleItem{
                            Sort = 7,
                            Name = "波尔多 顶级葡萄酒之旅",
                            Infos = new ScheduleItemInfos {
                                SpotsInfo    = "全日鹫巢村落一日游。   首先前往有世上最美丽村庄－圣保罗（41km/约1小时），漫步村内宛如迷宫曲折小径，居高临下俯瞰地中海。   参观举世最佳小型现代美术馆之一，隐身在圣保罗山丘的金松林间的★马格财团美术馆。   午后前往凡斯（5.2km/约10分钟），参观由野兽派巨匠马蒂斯规划设计的★罗萨利玫瑰经教堂Chapelle du Rosair。   傍晚返回蒙特卡洛（45km/约1小时）。",
                                CateringInfo = "早餐：酒店自助式早餐　　午餐：圣保罗农庄餐厅　　晚餐：法国海鲜餐",
                                HotelInfo    = "Hotel Metropole",
                            },
                        },
                        new ScheduleItem{
                            Sort = 8,
                            Name = "波尔多 顶级葡萄酒之旅",
                            Infos = new ScheduleItemInfos {
                                SpotsInfo    = "全日鹫巢村落一日游。   首先前往有世上最美丽村庄－圣保罗（41km/约1小时），漫步村内宛如迷宫曲折小径，居高临下俯瞰地中海。   参观举世最佳小型现代美术馆之一，隐身在圣保罗山丘的金松林间的★马格财团美术馆。   午后前往凡斯（5.2km/约10分钟），参观由野兽派巨匠马蒂斯规划设计的★罗萨利玫瑰经教堂Chapelle du Rosair。   傍晚返回蒙特卡洛（45km/约1小时）。",
                                CateringInfo = "早餐：酒店自助式早餐　　午餐：圣保罗农庄餐厅　　晚餐：法国海鲜餐",
                                HotelInfo    = "Hotel Metropole",
                            },
                        },
                        new ScheduleItem{
                            Sort = 9,
                            Name = "波尔多 顶级葡萄酒之旅",
                            Infos = new ScheduleItemInfos {
                                SpotsInfo    = "全日鹫巢村落一日游。   首先前往有世上最美丽村庄－圣保罗（41km/约1小时），漫步村内宛如迷宫曲折小径，居高临下俯瞰地中海。   参观举世最佳小型现代美术馆之一，隐身在圣保罗山丘的金松林间的★马格财团美术馆。   午后前往凡斯（5.2km/约10分钟），参观由野兽派巨匠马蒂斯规划设计的★罗萨利玫瑰经教堂Chapelle du Rosair。   傍晚返回蒙特卡洛（45km/约1小时）。",
                                CateringInfo = "早餐：酒店自助式早餐　　午餐：圣保罗农庄餐厅　　晚餐：法国海鲜餐",
                                HotelInfo    = "Hotel Metropole",
                            },
                        },

	#endregion ScheduleItemList
                    };
            return scheduleItemList;
        }

        //private void Init()
        //{
        //    FileStream stream;
        //    XmlSerializer seri = new XmlSerializer(typeof(List<Model.Product>));

        //    if (File.Exists(c_cacheFilePath))
        //    {
        //        using (StreamReader sr = new StreamReader(c_cacheFilePath))
        //        {
        //            _productList = (List<Model.Product>)seri.Deserialize(sr);
        //            if ((this.InternalRepository as InternalProductRepositoryMock).ProductPropertyList.Count == 0)
        //            {
        //                _productList.ForEach(p =>
        //                {
        //                    _productNoList.Add(p.ProductNo);
        //                    AddToProjectPropertyList(p);
        //                });
        //            }
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        stream = File.Create(c_cacheFilePath);
        //    }

        //    var listInterest = new InterestRepository(DbContext).All();
        //    var listGroup = new DestinationGroupRepository(DbContext).All();
        //    var listTravelType = new TravelTypeRepository(DbContext).All();
        //    var listDeparturePlace = new DeparturePlaceRepository(DbContext).All();

        //    var a = new List<Model.Product>();
        //    DateTime beginDate = DateTime.Parse("2012-08-01");
        //    DateTime endDate = DateTime.Parse("2013-12-12");
        //    int[] days = new int[] { 8, 9, 10, 12, 18 };
        //    int[] prices = new int[] { 12000, 18000, 98000, 120000, 180000, 1980000, 1200000, 1800000, 19800000 };
        //    string[] citys = new string[] { "波尔多", "巴黎", "东京", "纽约", "都柏林" };
        //    string desp = "abcdefghijklmnopqrstuvwxyz....abcdefghijklmnopqrstuvwxyz....abcdefghijklmnopqrstuvwxyz....abcdefghijklmnopqrstuvwxyz....";

        //    for (int i = 1; i <= 500; i++)
        //    {
        //        int productId = i;
        //        string productNo = string.Format("FN88{0:D3}", i);
        //        _productNoList.Add(productNo);

        //        var p = new Model.Product
        //        {
        //            ProductId = productId,
        //            //ProductName = string.Format("[{0}]xxxx游", i),
        //            ProductNo = productNo,
        //            // 120 chars
        //            Description = desp,
        //            DeparturePlace = MockHelper.GetRandomItem<DeparturePlace>(listDeparturePlace),
        //            TravelType = MockHelper.GetRandomItem<TravelType>(listTravelType),
        //            DepartureDateList = MockHelper.GetRandomDateList(beginDate, endDate),
        //            ScheduleItemList = GetScheduleItems(productId).ToList(),
        //            PhotoList = GetPhotos(productId).ToList(),
        //            Days = MockHelper.GetRandomItem<int>(i, days),
        //            GoFilghtList = new List<FilghtInfo> { },
        //            ReturnFilghtList = new List<FilghtInfo> { },
        //            HotelList = MockHelper.GetRandomList<Hotel>(i, this.GetHotels(productId)),
        //            InterestList = MockHelper.GetRandomList<Interest>(listInterest),
        //            DestinationGroup = MockHelper.GetRandomItem<DestinationGroup>(listGroup),
        //            DestinationCity = MockHelper.GetRandomItem<string>(citys, true),
        //            Price = MockHelper.GetRandomItem<int>(i, prices),
        //            OrderNotes = GetInfoByInfoType(productId, ProductInfoType.订购须知),
        //            CostNotes1 = GetInfoByInfoType(productId, ProductInfoType.费用包含),
        //            CostNotes2 = GetInfoByInfoType(productId, ProductInfoType.费用不包含),
        //        };
        //        p.DestinationRegion = MockHelper.GetRandomItem<DestinationRegion>(p.DestinationGroup.RegionList);
        //        a.Add(p);

        //        p.ProductName = string.Format("[{0}]{1}{2}{3}", p.ProductId, p.DestinationGroup.Name, (p.DestinationRegion != null) ? p.DestinationRegion.Name : "!分组下未配置区域", p.TravelType.Name);
        //        AddToProjectPropertyList(p);
        //    }

        //    _productList = a.ToList();

        //    using (StreamWriter sw = new StreamWriter(stream))
        //    {
        //        seri.Serialize(sw, _productList);
        //    }
        //}

        //// todo: beat it
        //public virtual void AddToProjectPropertyList(Model.Product p)
        //{
        //    Action<int, int> addToProductPropertyList = (productId, propertyId) =>
        //    {
        //        _ppList.Add(new Product_Property
        //        {
        //            ProductId = productId,
        //            PropertyId = propertyId,
        //        });
        //    };

        //    addToProductPropertyList(p.ProductId, p.TravelType.TravelTypeId);
        //    addToProductPropertyList(p.ProductId, p.DeparturePlace.DeparturePlaceId);
        //    foreach (var interest in p.InterestList)
        //    {
        //        addToProductPropertyList(p.ProductId, interest.InterestId);
        //    }
        //    addToProductPropertyList(p.ProductId, p.DestinationGroup.GroupId);
        //    if (p.DestinationRegion != null)
        //    {
        //        addToProductPropertyList(p.ProductId, p.DestinationRegion.RegionId);
        //    }
        //}
    }
}