using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HHTravel.DomainModel;

namespace HHTravel.Site.Models.OrderWizard
{
    public class GroundServiceModelBuilder : IModelBuilder<List<GroundServiceModel>, GroundServiceSegment>
    {
        private DateTime _date;
        private int _adultCount;
        private int _childCount;

        public GroundServiceModelBuilder( DateTime date, int adultCount, int childCount)
        {
            _date = date;
            _adultCount = adultCount;
            _childCount = childCount;
        }

        public List<GroundServiceModel> CreateFrom(GroundServiceSegment entity)
        {
            List<GroundServiceModel> modelList = new List<GroundServiceModel>();
            var groundServiceSegment = entity;

            if (groundServiceSegment == null)
            {
                return modelList;
            }

            var minPriceDate = groundServiceSegment.GetPriceDate(_date);
            var minAdultUnitPriceInSegment = minPriceDate.Price;
            var minChildUnitPriceInSegment = minPriceDate.ChildPrice;
            var minTotalPriceInSegment = minAdultUnitPriceInSegment * _adultCount + minChildUnitPriceInSegment * _childCount;

            var query = from gs in groundServiceSegment.GroundServiceList
                        let priceDate = gs.GetPriceDate(_date)
                        let adultUnitPrice = priceDate.Price
                        let childUnitPrice = priceDate.ChildPrice
                        let totalPrice = adultUnitPrice * _adultCount + childUnitPrice * _childCount
                        let totalPriceDelta = totalPrice - minTotalPriceInSegment
                        orderby totalPrice
                        select new GroundServiceModel
                        {
                            GroundServiceId = gs.GroundServiceId,
                            PurchaseCount = _adultCount + _childCount,
                            ServiceName = gs.ServiceName,
                            Description = gs.Description,
                            AdultUnitPrice = adultUnitPrice,
                            ChildUnitPrice = childUnitPrice,
                            TotalPrice = totalPrice,
                            TotalPriceDelta = totalPriceDelta,
                            Checked = (totalPriceDelta == 0),
                        };
            modelList = query.ToList();
            return modelList;
        }

        public List<GroundServiceModel> Rebuild(List<GroundServiceModel> model)
        {
            throw new NotImplementedException();
        }
    }
}