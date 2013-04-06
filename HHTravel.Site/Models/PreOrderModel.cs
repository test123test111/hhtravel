using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HHTravel.Infrastructure;
using HHTravel.Infrastructure.Crosscutting;
using HHTravel.Site.Controllers;

namespace HHTravel.Site.Models
{
    public class PreOrderModel
    {
        public PreOrderModel()
        {
            this.TicketModels = new List<TicketModel>();
        }

        [Required]
        public short Days { get; set; }

        /// <summary>
        /// 出发日期
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy年MM月dd日}")]
        [Date(ErrorMessage = "错误的日期格式")]
        [DataType(DataType.Date, ErrorMessage = "错误的日期格式2")]
        [Required]
        public DateTime DepartureDate { get; set; }

        /// <summary>
        /// 回程日期
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy年MM月dd日}")]
        [Date(ErrorMessage = "错误的日期格式")]
        [DataType(DataType.Date, ErrorMessage = "错误的日期格式2")]
        [Required]
        public DateTime PlanReturnDate { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string ProductNo { get; set; }

        [Required]
        public TravelType TravelType { get; set; }

        #region TravelType3、TravelType2

        /// <summary>
        /// 参加人数（成人）
        /// </summary>
        [RegularExpression(@"^[\d]+$", ErrorMessage = "请输入参加人数")]
        public short AdultCount { get; set; }

        /// <summary>
        /// 参加人数（孩童）
        /// </summary>
        [RegularExpression(@"^[\d]+$", ErrorMessage = "请输入参加人数")]
        public short ChildCount { get; set; }

        #endregion TravelType3、TravelType2

        #region TravelType3

        /// <summary>
        /// 详细行程
        /// </summary>
        [Obsolete]
        public string OldDescription { get; set; }

        /// <summary>
        /// 产品价格
        /// </summary>
        public decimal ProductPrice { get; set; }

        /// <summary>
        /// 房型
        /// </summary>
        public List<RoomClassModel> RoomClassModels { get; set; }

        public DateTime? StayReturnDate { get; set; }

        /// <summary>
        /// 延住房型
        /// </summary>
        public List<RoomClassModel> StayRoomClassModels { get; set; }

        /// <summary>
        /// 加购机票
        /// </summary>
        public List<TicketModel> TicketModels { get; set; }

        #endregion TravelType3

        #region TravelType2

        public List<RoomClassPostModel> RoomClassPostModelList { get; set; }

        //[Required(ErrorMessage = "请选择机票")]
        public int SelectedTicketId { get; set; }

        #endregion TravelType2

        public virtual void Validate(ModelStateDictionary modelState)
        {
            var travelType = this.TravelType;

            if (travelType == TravelType.TravelType3)
            {
                if (string.IsNullOrEmpty(this.OldDescription))
                {
                    throw new ArgumentNullException("Product.Description");
                }

                bool hasSelectedRoom = this.RoomClassModels.Any(rc => rc.Count > 0);
                if (!hasSelectedRoom)
                {
                    modelState.AddModelError("RoomClasses", "请选择入住的房间");
                }

                hasSelectedRoom = this.StayRoomClassModels.Any(rc => rc.StayCount > 0);

                if (this.StayReturnDate.HasValue && !hasSelectedRoom)
                {
                    modelState.AddModelError("StayRoomClasses", "请选择延住的房间");
                }
                else if (!this.StayReturnDate.HasValue && hasSelectedRoom)
                {
                    modelState.AddModelError("StayReturnDate", "请选择退房日期");
                }
            }
            else if (travelType == TravelType.TravelType2)
            {
                if (this.AdultCount <= 0 && this.ChildCount <= 0)
                {
                    modelState.AddModelError("ChildCount", "请输入参加人数");
                }

                // 每个行程段都有选酒店
                bool hasSelectedRoom = this.RoomClassPostModelList
                                            .GroupBy(rc => rc.SegmentId)
                                            .All(seg => seg.Any(rc => rc.Count > 0));
                if (!hasSelectedRoom)
                {
                    throw new ArgumentNullException("Product.SelectedRoomClass", "请选择酒店");
                }

                if (this.SelectedTicketId == 0)
                {
                    throw new ArgumentNullException("Product.TicketId", "请选择机票");
                }
            }
        }
    }

    public class RoomClassPostModel
    {
        public string BreakfastDinnerTip { get; set; }

        public int Count { get; set; }

        public int HotelId { get; set; }

        public decimal SegmentPrice { get; set; }

        public int RoomClassId { get; set; }

        public string RoomClassName { get; set; }

        public int SegmentId { get; set; }

        public decimal SegmentPriceDelta { get; set; }
    }
}