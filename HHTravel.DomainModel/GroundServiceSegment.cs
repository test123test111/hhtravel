using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace HHTravel.DomainModel
{
    public class GroundServiceSegment : SegmentBase
    {
        public GroundServiceSegment()
        {
            this.PriceDateList = new List<PriceDate>();
        }

        [DataMember]
        public List<GroundService> GroundServiceList { get; set; }
    }
}