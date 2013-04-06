using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HHTravel.DomainModel;
using HHTravel.Infrastructure.Crosscutting;

namespace HHTravel.Site.Models
{
    /// <summary>
    /// 有住宿的行程段
    /// </summary>
    public class HotelSegmentModel
    {
        public HotelSegmentModel()
        {
            HotelModelList = new List<HotelModel>();
            SelectedRoomClassPostModelList = new List<RoomClassPostModel>();
        }

        public DateTime CheckinDate { get; set; }

        public DateTime CheckoutDate { get; set; }

        public string CheckinDateString { get { return this.CheckinDate.ToString("yyyy-MM-dd"); } }

        public string CheckoutDateString { get { return this.CheckoutDate.ToString("yyyy-MM-dd"); } }

        public string DestinationCity { get; set; }

        public List<HotelModel> HotelModelList { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal MinTotalPrice { get; set; }

        public int Nights { get; set; }

        public int SegmentId { get; set; }

        public HotelSegmentType HotelSegmentType { get; set; }

        public string Description { get; set; }

        public HotelModel SelectedHotelModel { get; set; }

        public List<RoomClassPostModel> SelectedRoomClassPostModelList { get; set; }
    }
}