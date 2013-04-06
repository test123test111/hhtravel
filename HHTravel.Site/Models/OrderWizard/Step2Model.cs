using System.Collections.Generic;

namespace HHTravel.Site.Models.OrderWizard
{
    public class Step2Model : Step2PostModel, IStepModel
    {
        public Step2Model()
        {
            this.IsChooseFlights = true;
        }

        public HotelSegmentModel HotelSegment { get; set; }

        public HotelSegmentModel DelayHotelSegment { get; set; }

        public List<GroundServiceModel> GroundServiceModels { get; set; }

        public List<FlightsSegmentModel> FlightsSegmentModels { get; set; }

        public string ProductName { get; set; }

        public string ProductNo { get; set; }

        public int AdultCount { get; set; }

        public int ChildCount { get; set; }

        public int StepNo
        {
            get { return 2; }
        }

        public int BeginSubstepNo { get; set; }
    }

    public class HotelPostModel
    {
        public int HotelId { get; set; }

        public List<RoomClassPostModel> RoomClassPostModels { get; set; }
    }

    public class RoomClassPostModel
    {
        public int RoomClassId { get; set; }

        public int RoomCount { get; set; }
    }
}