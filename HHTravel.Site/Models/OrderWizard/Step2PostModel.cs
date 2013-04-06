using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HHTravel.Infrastructure.Crosscutting;

namespace HHTravel.Site.Models.OrderWizard
{
    public class Step2PostModel : IPostModel
    {
        public Guid SessionId { get; set; }

        public int SelectedGroundServiceId { get; set; }

        public HotelPostModel HotelPostModel { get; set; }

        public HotelPostModel DelayHotelPostModel { get; set; }

        // !非post提交来的
        public decimal TotalPrice { get; set; }

        public bool IsChooseFlights { get; set; }

        public FlightSeat FlightSeat { get; set; }

        public void Validate(System.Web.Mvc.ModelStateDictionary modelState)
        {
            if (HotelPostModel == null)
            {
                modelState.AddModelError("HotelPostModel.HotelId", "请选择酒店");
            }
            else if (HotelPostModel.RoomClassPostModels.All(r => r.RoomCount == 0))
            {
                modelState.AddModelError("HotelPostModel.RoomClassPostModels.RoomCount", "请选择房间");
            }

            if (DelayHotelPostModel != null)
            {
                if (DelayHotelPostModel.RoomClassPostModels.All(r => r.RoomCount == 0))
                {
                    modelState.AddModelError("DelayHotelPostModel.RoomClassPostModels.RoomCount", "请选择房间");
                }
            }
        }

        public void Validate(System.Web.Mvc.ModelStateDictionary modelState, int personCount)
        {
            if (personCount <= 0)
            {
                throw new ArgumentOutOfRangeException("personCount");
            }

            this.Validate(modelState);

            int totalRoomCount = HotelPostModel.RoomClassPostModels.Sum(model => model.RoomCount);
            if (totalRoomCount > personCount)
            {
                modelState.AddModelError("HotelPostModel.RoomClassPostModels.RoomCount", "选择的人数超过了出行的人数");
            }

            if (DelayHotelPostModel != null)
            {
                totalRoomCount = DelayHotelPostModel.RoomClassPostModels.Sum(model => model.RoomCount);
                if (totalRoomCount > personCount)
                {
                    modelState.AddModelError("DelayHotelPostModel.RoomClassPostModels.RoomCount", "选择的人数超过了出行的人数");
                }
            }
        }
    }
}