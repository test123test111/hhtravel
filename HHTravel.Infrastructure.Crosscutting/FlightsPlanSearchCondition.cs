using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting
{
    public class FlightsPlanSearchCondition
    {
        public int AdultCount { get; set; }

        public int ChildCount { get; set; }

        public int CurrentSegmentNo { get; set; }

        public FlightSeat FlightSeat { get; set; }

        public List<FlightsSegment> FlightsSegments { get; set; }

        public bool ContainsNotLowestPrice { get; set; }

        public int ProductId { get; set; }
        public void Validate()
        {
            if (this.ProductId <= 0)
            {
                throw new ArgumentOutOfRangeException("ProductId");
            }

            if (this.AdultCount == 0)
            {
                if (this.ChildCount > 0)
                {
                    throw new NotSupportedException("无成人陪伴的儿童只能直接向航空公司预订");
                }
                else
                {
                    throw new InvalidOperationException("未输入乘客人数");
                }
            }

            if (this.CurrentSegmentNo < 1)
            {
                throw new ArgumentOutOfRangeException("CurrentSegmentNo", "航段编号从1开始");
            }
        }
    }
}