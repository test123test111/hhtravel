using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using HHTravel.Infrastructure.Crosscutting;

namespace HHTravel.Site.Models.OrderWizard
{
    public class FlightsSegmentModel
    {
        public int SegmentNo { get; set; }

        public City DepartCity { get; set; }

        public City ArrivalCity { get; set; }

        [DataType(DataType.Date)]
        public DateTime DepartDate { get; set; }

        public FlightsSegmentModel()
        {
            this.SegmentNo = 1;
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(this.SegmentNo > 0);
        }
    }
}