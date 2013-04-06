using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HHTravel.Infrastructure.Crosscutting;
using Newtonsoft.Json;

namespace HHTravel.Site.Models.OrderWizard
{
    public class FlightsPlanModelBuilder : IModelBuilder<List<FlightsPlanModel>, IList<FlightsPlan>>
    {
        public List<FlightsPlanModel> CreateFrom(IList<FlightsPlan> entity)
        {
            List<FlightsPlanModel> models = new List<FlightsPlanModel>();
            var plans = entity;

            if (!plans.Any())
            {
                return models;
            }

            decimal minTotalPrice = plans.Min(p => p.TotalPrice);
            var q = (from plan in plans
                     let firstFlight = plan.Flights.First()
                     let lastFlight = plan.Flights.Last()
                     select new FlightsPlanModel
                     {
                         RouteId = plan.RouteId,
                         AdultFuelSurcharges = plan.AdultFuelSurcharges,
                         AdultPrice = plan.AdultPrice,
                         AdultTax = plan.AdultTax,
                         ChildFuelSurcharges = plan.ChildFuelSurcharges,
                         ChildPrice = plan.ChildPrice,
                         ChildTax = plan.ChildTax,
                         TotalFuelSurcharges = plan.TotalFuelSurcharges,
                         TotalPrice = plan.TotalPrice,
                         TotalPriceDelta = (plan.TotalPrice - minTotalPrice),
                         TotalTax = plan.TotalTax,
                         Airline = plan.Airline,
                         FlightNo = firstFlight.FlightNo,
                         DepartTime = firstFlight.DepartTime.ToString(@"hh\:mm"),
                         ArrivalTime = lastFlight.ArrivalTime.ToString(@"hh\:mm"),
                         DepartAirport = firstFlight.DepartAirport,
                         ArrivalAirport = lastFlight.ArrivalAirport,
                         FlightSeat = firstFlight.FlightSeat,
                         Flights = (from flight in plan.Flights
                                    select new FlightModel
                                    {
                                        ActualAirline = flight.ActualAirline,
                                        ActualFlightNo = flight.ActualFlightNo,
                                        Aircraft = flight.Aircraft,
                                        Airline = flight.Airline,
                                        ArrivalAirport = flight.ArrivalAirport,
                                        ArrivalCity = flight.ArrivalCity,
                                        ArrivalDays = flight.ArrivalDays,
                                        ArrivalTime = flight.ArrivalTime,
                                        Baggage = flight.Baggage,
                                        DepartAirport = flight.DepartAirport,
                                        DepartCity = flight.DepartCity,
                                        DepartTime = flight.DepartTime,
                                        FlightSeat = flight.FlightSeat,
                                        FlightDuration = flight.FlightDuration,
                                        FlightNo = flight.FlightNo,
                                        Stopovers = (from s in flight.Stopovers
                                                     select new StopoverModel
                                                     {
                                                         Airport = s.Airport,
                                                         Duration = string.Format("{0}小时{1}分钟", s.Duration.Hours, s.Duration.Minutes)
                                                     }).ToList(),
                                    }).ToList(),
                         FlightsSegmentPlanJson = JsonConvert.SerializeObject(plan),
                     });

            models = q.ToList();
            return models;
        }

        public List<FlightsPlanModel> Rebuild(List<FlightsPlanModel> model)
        {
            throw new NotImplementedException();
        }
    }
}