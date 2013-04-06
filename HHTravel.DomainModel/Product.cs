using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using HHTravel.Infrastructure.Crosscutting;

namespace HHTravel.DomainModel
{
    /// <summary>
    /// 旅行产品
    /// </summary>
    [DataContract]
    public class Product : IAggregateRoot, IPriceDate
    {
        public Product()
        {
            this.DestinationGroupList = new List<DestinationGroup>();
            this.DestinationRegionList = new List<DestinationRegion>();
            this.InterestList = new List<Interest>();
            this.PhotoList = new List<HHTravel.Infrastructure.Crosscutting.ImageInfo>();
            this.SetOffDateList = new List<DateTime>();

            this.PriceDateList = new List<PriceDate>();
        }

        /// <summary>
        /// 提前预订天数
        /// </summary>
        [DataMember]
        public short? AdvanceDays { get; set; }

        /// <summary>
        /// 旅游咨询
        /// </summary>
        [DataMember]
        public string Consultation { get; set; }

        /// <summary>
        /// 费用包含
        /// </summary>
        [DataMember]
        public string CostNotes1 { get; set; }

        /// <summary>
        /// 费用不包含
        /// </summary>
        [DataMember]
        public string CostNotes2 { get; set; }

        /// <summary>
        /// 行程天数
        /// </summary>
        [DataMember]
        public short Days { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        [DataMember]
        public string DepartureCity { get; set; }

        /// <summary>
        /// 出发日期
        /// </summary>
        [DataMember]
        public List<PriceDate> DepartureDateList { get; set; }

        /// <summary>
        /// (TravelType3的费用包含)
        /// </summary>
        [DataMember]
        [Obsolete]
        public string OldDescription { get; set; }

        /// <summary>
        /// 目的地城市
        /// </summary>
        [DataMember]
        public string DestinationCity { get; set; }

        /// <summary>
        /// 目的地分组
        /// </summary>
        [DataMember]
        public List<DestinationGroup> DestinationGroupList { get; set; }

        /// <summary>
        /// 目的地区域
        /// </summary>
        [DataMember]
        public List<DestinationRegion> DestinationRegionList { get; set; }

        /// <summary>
        /// 产品特色
        /// </summary>
        [DataMember]
        public string Feature { get; set; }

        /// <summary>
        /// 可延住天数
        /// </summary>
        public int MaxDelayDays { get; set; }

        /// <summary>
        /// 去程航班
        /// </summary>
        [DataMember]
        public List<Flight> GoFilghtList { get; set; }

        /// <summary>
        /// 是否有折扣
        /// </summary>
        [DataMember]
        public bool HasDiscount { get; set; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        [DataMember]
        public bool IsRecommended { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        [DataMember]
        public List<Interest> InterestList { get; set; }

        /// <summary>
        /// 主图
        /// </summary>
        [DataMember]
        public ImageInfo MainImage { get; set; }

        /// <summary>
        /// 主行程图
        /// </summary>
        [DataMember]
        public ImageInfo MainPhoto { get; set; }

        /// <summary>
        /// 最大成行人数
        /// </summary>
        public int? MaxPersonNum { get; set; }

        /// <summary>
        /// 是否允许携带儿童
        /// </summary>
        public bool AllowChild { get; set; } 

        /// <summary>
        /// 最小住宿天数
        /// </summary>
        [DataMember]
        public short? MinLodgingDays { get; set; }

        /// <summary>
        /// 市场价
        /// </summary>
        [DataMember]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? MinMarketPrice { get; set; }

        /// <summary>
        /// 底价
        /// </summary>
        [DataMember]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int MinPrice { get; set; }

        /// <summary>
        /// 产品备注
        /// </summary>
        [DataMember]
        public string Note { get; set; }

        /// <summary>
        /// 订购须知
        /// </summary>
        [DataMember]
        public string OrderNotes { get; set; }

        /// <summary>
        /// 行程图
        /// </summary>
        [DataMember]
        public List<ImageInfo> PhotoList { get; set; }

        /// <summary>
        /// 获取 PriceDateList
        /// </summary>
        [DataMember]
        public List<PriceDate> PriceDateList { get; set; }

        /// <summary>
        /// 旅行产品Id
        /// </summary>
        [DataMember]
        public int ProductId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [DataMember]
        public string ProductName { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        [DataMember]
        public string ProductNo { get; set; }

        /// <summary>
        /// 返程航班
        /// </summary>
        [DataMember]
        public List<Flight> ReturnFilghtList { get; set; }

        /// <summary>
        /// 路线图
        /// </summary>
        [DataMember]
        public ImageInfo RouteMap { get; set; }

        /// <summary>
        /// 行程段
        /// </summary>
        public List<HotelSegment> SegmentList { get; set; }

        /// <summary>
        /// 额满日期
        /// </summary>
        public List<DateTime> SetOffDateList { get; set; }

        /// <summary>
        /// 旅游型态
        /// </summary>
        [DataMember]
        public TravelType TravelType { get; set; }

        [DataMember]
        public TravelChildType TravelChildType { get; set; }

        /// <summary>
        /// 签证须知
        /// </summary>
        [DataMember]
        public string VisaNotes { get; set; }

        public PriceDate GetPriceDate(DateTime date)
        {
            PriceDate dDate = null;
            dDate = (from dd in this.DepartureDateList
                     where dd.Date == date
                     select dd).SingleOrDefault();
            return dDate;
        }
    }
}