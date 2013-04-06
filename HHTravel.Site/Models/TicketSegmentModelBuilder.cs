using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HHTravel.DomainModel;

namespace HHTravel.Site.Models
{
    public class TicketSegmentModelBuilder : IModelBuilder<TicketSegmentModel, TicketSegment>
    {
        private DateTime _departureDate;

        public TicketSegmentModelBuilder(DateTime departureDate)
        {
            this._departureDate = departureDate;
        }

        public TicketSegmentModel CreateFrom(TicketSegment entity)
        {
            var ticketSegment = entity;
            
            var minTicketPrice = ticketSegment.GetPriceDate(_departureDate).Price;
            var model = new TicketSegmentModel
            {
                TicketModelList = (from t in ticketSegment.TicketList
                                   let firstFlight = t.FlightList.First()
                                   let lastFlight = t.FlightList.Last()
                                   let ticketPriceDate = t.GetPriceDate(_departureDate)
                                   where ticketPriceDate != null    // 排除掉指定日没有价格的机票
                                   select new TicketModel
                                   {
                                       TicketId = t.TicketId,
                                       FlightClassName = t.FlightSeatName,
                                       AdditionalPurchasesNote = t.AdditionalPurchasesNote,
                                       Price = ticketPriceDate.Price,
                                       PriceDelta = ticketPriceDate.Price - minTicketPrice,
                                       FlightModelList = (from f in t.FlightList
                                                          let isFirst = f == firstFlight
                                                          let isLast = f == lastFlight
                                                          select new FlightModel
                                                          {
                                                              FlightNo = f.FlightNo,
                                                              Airline = f.Airline.AirlineName,
                                                              DepartureAirport = f.DepartAirport.AirportName,
                                                              ArrivalAirport = f.ArrivalAirport.AirportName,
                                                              DepartureTime = f.DepartTime,
                                                              ArrivalTime = f.ArrivalTime,
                                                              SectionName = isFirst ? "去程" : (isLast ? "回程" : "中间程"),
                                                          }).ToList(),
                                   }).OrderBy(tm => tm.PriceDelta).ToList(),
            };

            model.MinPriceTicketModule = (from t in model.TicketModelList
                                          where t.PriceDelta == 0
                                          select t).First();
            return model;
        }

        public TicketSegmentModel Rebuild(TicketSegmentModel model)
        {
            throw new NotImplementedException();
        }
    }
}