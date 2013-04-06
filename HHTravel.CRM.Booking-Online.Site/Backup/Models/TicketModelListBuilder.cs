using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;
using HHTravel.CRM.Booking_Online.DomainModel;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class TicketModelListBuilder : IModelBuilder<List<TicketModel>, IList<Ticket>>
    {
        private DateTime _departureDate;
        private TravelType _travelType;

        public TicketModelListBuilder(DateTime departureDate, TravelType travelType)
        {
            this._departureDate = departureDate;
            this._travelType = travelType;
        }

        public List<TicketModel> CreateFrom(IList<Ticket> entity)
        {
            List<TicketModel> ticketModels;
            var tickets = entity;

            var queryTicket = from t in tickets
                              let firstFlight = t.FlightList.First()
                              let lastFlight = t.FlightList.Last()
                              select new TicketModel
                              {
                                  TicketId = t.TicketId,
                                  FlightClassName = t.FlightSeatName,
                                  AdditionalPurchasesNote = t.AdditionalPurchasesNote,
                                  Price = t.GetPrice(this._departureDate, this._travelType),
                                  FlightModelList = (from f in t.FlightList
                                                     let isFirst = f == firstFlight
                                                     let isLast = f == lastFlight
                                                     select new FlightModel
                                                     {
                                                         Airline = f.Airline.AirlineName,
                                                         ArrivalAirport = f.ArrivalAirport.AirportName,
                                                         ArrivalTime = f.ArrivalTime,
                                                         DepartureAirport = f.DepartAirport.AirportName,
                                                         DepartureTime = f.DepartTime,
                                                         FlightNo = f.FlightNo,
                                                         SectionName = isFirst ? "去程" : (isLast ? "回程" : "中间程")
                                                     }).ToList(),
                              };
            ticketModels = queryTicket.ToList();
            return ticketModels;
        }

        public List<TicketModel> Rebuild(List<TicketModel> model)
        {
            throw new NotImplementedException();
        }
    }
}