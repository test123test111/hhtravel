using System.Collections.Generic;
using System.Text.RegularExpressions;
using HHTravel.CRM.Booking_Online.DomainModel;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class ProductModel : Product
    {
        // 用于将产品名称中的【xxxx】、x天及之后、x日及之后的部分剪掉
        private static readonly Regex s_regexFixProductName = new Regex(@"(\u3010.*\u3011)?(.*?)\s*\d+\s*[\u5929\u65e5].*", RegexOptions.Compiled);

        public ProductModel(Product p)
        {
            if (p == null)
            {
                return;
            }

            ProductId = p.ProductId;

            ProductNo = p.ProductNo;
            ProductName = p.ProductName;
            TravelType = p.TravelType;
            TravelChildType = p.TravelChildType;
            MinPrice = p.MinPrice;
            DepartureCity = p.DepartureCity;
            DestinationCity = p.DestinationCity;

            ShortenName = s_regexFixProductName.Replace(p.ProductName, "$2");
            HasDiscount = p.HasDiscount;
            IsRecommended = p.IsRecommended;

            InterestList = p.InterestList;
            DestinationGroupList = p.DestinationGroupList;
            DestinationRegionList = p.DestinationRegionList;

            Days = p.Days;
            AllowChild = p.AllowChild;
            MinLodgingDays = p.MinLodgingDays;
            MaxPersonNum = p.MaxPersonNum;
            MaxDelayDays = p.MaxDelayDays;
            DepartureDateList = p.DepartureDateList;

            MainImage = p.MainImage;
            MainPhoto = p.MainPhoto;
            PhotoList = p.PhotoList;
            RouteMap = p.RouteMap;

            OldDescription = p.OldDescription;
            Feature = p.Feature;
            Note = p.Note;
            VisaNotes = p.VisaNotes;
            OrderNotes = p.OrderNotes;
            CostNotes1 = p.CostNotes1;
            CostNotes2 = p.CostNotes2;
        }

        public string ShortenName { get; set; }

        /// <summary>
        /// 酒店
        /// </summary>
        public IList<Hotel> HotelList { get; set; }

        /// <summary>
        /// 日程
        /// </summary>
        public List<ScheduleItem> ScheduleItemList { get; set; }
    }
}