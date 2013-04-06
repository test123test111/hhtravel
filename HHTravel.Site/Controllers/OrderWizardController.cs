using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using HHTravel.CRM.Booking_Online.DomainModel;
using HHTravel.CRM.Booking_Online.Infrastructure;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;
using HHTravel.CRM.Booking_Online.Site.Models;
using HHTravel.CRM.Booking_Online.Site.Models.OrderWizard;
using Newtonsoft.Json;

namespace HHTravel.CRM.Booking_Online.Site.Controllers
{
    public class OrderWizardController : ControllerBase
    {
        private FlightsPlanModelBuilder _filghtsPlanModelBuilder = new FlightsPlanModelBuilder();

        [HttpPost]
        public JsonResult GetFlightsPlans(Guid sessionId, int segmentNo, string routeId = null, string flightsSegmentPlanJson = null)
        {
            var context = WizardContext.Read(sessionId);

            var sc = new FlightsPlanSearchCondition
            {
                ProductId = context.Step1Model.ProductId,
                AdultCount = context.Step1Model.AdultCount,
                ChildCount = context.Step1Model.ChildCount ?? 0,
                CurrentSegmentNo = segmentNo,
                FlightsSegments = (from segModel in context.FlightsSegmentModels
                                   select new FlightsSegment
                                   {
                                       SegmentNo = segModel.SegmentNo,
                                       ArrivalCity = segModel.ArrivalCity,
                                       DepartCity = segModel.DepartCity,
                                       DepartDate = segModel.DepartDate,
                                   }).ToList(),
                FlightSeat = context.Step2Model.FlightSeat,
            };

            if (segmentNo > 1)
            {
                var prevSegmentNo = segmentNo - 1;
                var prevSegment = sc.FlightsSegments.Single(fs => fs.SegmentNo == prevSegmentNo);
                prevSegment.SelectedRouteId = routeId;
                prevSegment.Flights = JsonConvert.DeserializeObject<FlightsPlan>(flightsSegmentPlanJson).Flights;
            }

            var seg = ProductService.SearchFlightsPlans(sc);
            var flightsSegmentPlanModels = _filghtsPlanModelBuilder.CreateFrom(seg.FlightsPlans);
            return Json(flightsSegmentPlanModels);
        }

        [HttpPost]
        public ActionResult Step1(Step1PostModel postModel)
        {
            Product product = ProductService.GetProduct(postModel.ProductId);

            var context = WizardContext.Read(postModel.SessionId);
            postModel.Validate(this.ModelState);
            if (!this.ModelState.IsValid)
            {
                // 回到日历页面
                return RedirectToAction("Index", "ProductDetails", new { productNo = product.ProductNo, tab = 3 });
            }

            context.ProductNo = product.ProductNo;
            context.ProductName = product.ProductName;
            context.Step1Model = postModel;

            var hotelSegmentsModel = BuildHotelSegmentModel(context.Step1Model);
            var flightsSegmentModels = BuildFlightsSegmentModels(context.Step1Model, hotelSegmentsModel);

            context.FlightsSegmentModels = flightsSegmentModels;
            context.Write(postModel.SessionId);

            var groundServiceModels = BuildGroundServiceModels(context.Step1Model);
            var builder = new Step2ModelBuilder(context, hotelSegmentsModel, flightsSegmentModels, groundServiceModels);
            Step2Model model2 = builder.CreateFrom(null);

            return View("Step2", model2);
        }

        [HttpPost]
        public ActionResult Step2(Step2PostModel postModel)
        {
            var context = WizardContext.Read(postModel.SessionId);

            int adultCount = context.Step1Model.AdultCount;
            int childCount = context.Step1Model.ChildCount ?? 0;
            var hotelSegmentsModel = BuildHotelSegmentModel(context.Step1Model);
            var flightsSegmentModels = BuildFlightsSegmentModels(context.Step1Model, hotelSegmentsModel);
            var groundServiceModels = BuildGroundServiceModels(context.Step1Model);
            var personCount = adultCount + childCount;

            postModel.Validate(this.ModelState, personCount);
            if (!ModelState.IsValid)
            {
                var builder = new Step2ModelBuilder(context, hotelSegmentsModel, flightsSegmentModels, groundServiceModels);
                Mapper.CreateMap<Step2PostModel, Step2Model>();
                Step2Model model = Mapper.Map<Step2PostModel, Step2Model>(postModel);
                model = builder.Rebuild(model);

                return View(model);
            }
            // 安全原因，服务器端重新计算价格
            decimal totalPriceInStep2 = CalcTotalPriceInStep2(
                hotelSegmentsModel.HotelSegmentModel, postModel.HotelPostModel,
                hotelSegmentsModel.DelayHotelSegmentModel, postModel.DelayHotelPostModel,
                groundServiceModels, postModel.SelectedGroundServiceId,
                adultCount, childCount);

            postModel.TotalPrice = totalPriceInStep2;
            context.Step2Model = postModel;
            context.Write(postModel.SessionId);

            ViewBag.TopNavModel = null;
            if (postModel.IsChooseFlights)
            {
                var builder = new Step3ModelBuilder(context, flightsSegmentModels);
                Step3Model model3 = builder.CreateFrom(null);

                return View("Step3", model3);
            }
            else
            {
                var builder = new Step4ModelBuilder(context, totalPriceInStep2);
                var step4Model = builder.CreateFrom(null);

                return View("Step4", step4Model);
            }
        }

        // 酒店+地面产品+航班过滤条件
        [HttpPost]
        public ActionResult Step3(Step3PostModel postModel)
        {
            var context = WizardContext.Read(postModel.SessionId);
            int adultCount = context.Step1Model.AdultCount;
            int childCount = context.Step1Model.ChildCount ?? 0;

            postModel.Validate(this.ModelState);
            if (!ModelState.IsValid)
            {
                var hotelSegmentsModel = BuildHotelSegmentModel(context.Step1Model);
                var flightsSegmentModels = BuildFlightsSegmentModels(context.Step1Model, hotelSegmentsModel);
                var builder = new Step3ModelBuilder(context, flightsSegmentModels);
                Mapper.CreateMap<Step3PostModel, Step3Model>();
                Step3Model model = Mapper.Map<Step3PostModel, Step3Model>(postModel);
                model = builder.Rebuild(model);

                return View(model);
            }

            context.Step3Model = postModel;
            context.Write(postModel.SessionId);

            var builder2 = new Step4ModelBuilder(context, postModel.TotalPrice);
            Step4Model model4 = builder2.CreateFrom(null);

            return View("Step4", model4);
        }

        // 航班
        [HttpPost]
        public ActionResult Step4(Step4PostModel postModel)
        {
            var context = WizardContext.Read(postModel.SessionId);
            int adultCount = context.Step1Model.AdultCount;
            int childCount = context.Step1Model.ChildCount ?? 0;
            decimal totalPrice = context.Step2Model.IsChooseFlights ? context.Step3Model.TotalPrice : context.Step2Model.TotalPrice;

            postModel.Validate(this.ModelState);
            if (!ModelState.IsValid)
            {
                var builder = new Step4ModelBuilder(context, totalPrice);
                Mapper.CreateMap<Step4PostModel, Step4Model>();
                Step4Model model = Mapper.Map<Step4PostModel, Step4Model>(postModel);
                model = builder.Rebuild(model);

                return View(model);
            }

            context.Step4Model = postModel;
            context.Write(postModel.SessionId);

            Product product = ProductService.GetProduct(context.Step1Model.ProductId);
            var hotelSegmentsModel = BuildHotelSegmentModel(context.Step1Model);
            var flightsSegmentModels = BuildFlightsSegmentModels(context.Step1Model, hotelSegmentsModel);
            var groundServiceModels = BuildGroundServiceModels(context.Step1Model);

            var step5ModelBuilder = new Step5ModelBuilder(context, product.Feature, hotelSegmentsModel, flightsSegmentModels, groundServiceModels);
            Step5Model model5 = step5ModelBuilder.CreateFrom(null);

            return View("Step5", model5);
        }

        // 乘客信息、订单附加信息
        [HttpPost]
        public ActionResult Step5(Step5PostModel postModel)
        {
            WizardContext context = WizardContext.Read(postModel.SessionId);
            int adultCount = context.Step1Model.AdultCount;
            int childCount = context.Step1Model.ChildCount ?? 0;
            decimal totalPrice = context.Step2Model.IsChooseFlights ? context.Step3Model.TotalPrice : context.Step2Model.TotalPrice;
            Product product = ProductService.GetProduct(context.Step1Model.ProductId);
            
            var hotelSegmentsModel = BuildHotelSegmentModel(context.Step1Model);
            var flightsSegmentModels = BuildFlightsSegmentModels(context.Step1Model, hotelSegmentsModel);
            var groundServiceModels = BuildGroundServiceModels(context.Step1Model);

            var step5ModelBuilder = new Step5ModelBuilder(context, product.Feature, hotelSegmentsModel, flightsSegmentModels, groundServiceModels);
            Mapper.CreateMap<Step5PostModel, Step5Model>();
            Step5Model model = Mapper.Map<Step5PostModel, Step5Model>(postModel);
            model = step5ModelBuilder.Rebuild(model);

            postModel.Validate(this.ModelState);
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Order order = OrderBuilder.Build(model, product);
            OrderService.Add(order);

            ViewBag.OrderNo = order.OrderNo;
            model.UsedByEmail = true;
            SendOrderEmail("Step6", model, order);

            model.UsedByEmail = false;

            return View("Step6", model);
        }

        #region Model Building

        private List<FlightsSegmentModel> BuildFlightsSegmentModels(Step1PostModel model1, HotelSegmentsModel hotelSegmentsModel)
        {
            var hotelSegmentModel = hotelSegmentsModel.HotelSegmentModel;
            var delayHotelSegmentModel = hotelSegmentsModel.DelayHotelSegmentModel;
            var flightsSegs = ProductService.GetFlightsSegments(model1.ProductId);
            Contract.Assert(flightsSegs.Count == 2);

            Func<int, int, DateTime> getFligthSegmentDepartDate = (segmentNo, adjustableDays) =>
            {
                switch (segmentNo)
                {
                    case 1:
                        return hotelSegmentModel.CheckinDate.AddDays(0 - adjustableDays);
                    case 2:
                        return (delayHotelSegmentModel != null ? delayHotelSegmentModel.CheckoutDate : hotelSegmentModel.CheckoutDate).AddDays(adjustableDays);
                    default:
                        throw new ArgumentOutOfRangeException("segmentNo", segmentNo, "业务规则：TravelType3只有两个航段");
                }
            };

            var query = from seg in flightsSegs
                        select new FlightsSegmentModel
                        {
                            SegmentNo = seg.SegmentNo,
                            DepartCity = seg.DepartCity,
                            ArrivalCity = seg.ArrivalCity,
                            DepartDate = getFligthSegmentDepartDate(seg.SegmentNo, seg.AdjustableDays),
                        };
            return query.ToList();
        }

        private List<Models.OrderWizard.GroundServiceModel> BuildGroundServiceModels(Step1PostModel model1)
        {
            List<Models.OrderWizard.GroundServiceModel> modelList;
            var builder = new GroundServiceModelBuilder(model1.BeginDate, model1.AdultCount, model1.ChildCount ?? 0);
            var groundServiceSegment = ProductService.GetGroundServiceSegment(model1.ProductId);
            modelList = builder.CreateFrom(groundServiceSegment);

            Contract.Assert(modelList != null);

            return modelList;
        }

        private HotelSegmentsModel BuildHotelSegmentModel(Step1PostModel model1)
        {
            // TravelType3只有一个酒店段（线路+酒店）
            var hotelSegmentModelBuilder = new HotelSegmentsModelBuilder(model1.BeginDate, model1.DelayDays);
            var hotelSegs = ProductService.GetHotelSegments(model1.ProductId);

            var model = hotelSegmentModelBuilder.CreateFrom(hotelSegs);
            return model;
        }

        #endregion Model Building

        // todo: 算价逻辑重构到合适的时机（运行时）和地方
        private decimal CalcTotalPriceInStep2(
            HotelSegmentModel hotelSegmentModel,
            HotelPostModel hotelPostModel,
            HotelSegmentModel delayHotelSegmentModel,
            HotelPostModel delayHotelPostModel,
            List<GroundServiceModel> groundServiceModels,
            int selectedGroundServiceId,
            int adultCount,
            int childCount)
        {
            decimal totalPrice;
            Func<HotelSegmentModel, HotelPostModel, decimal> calc = (viewModel, postModel) =>
            {
                var hotelTotalPrice = (from hotel in viewModel.HotelModelList
                                       where hotel.HotelId == postModel.HotelId
                                       from room in hotel.RoomClassModelList
                                       join selectedRoom in postModel.RoomClassPostModels
                                       on room.RoomClassId equals selectedRoom.RoomClassId
                                       select room.SegmentPrice * selectedRoom.RoomCount)
                                       .Sum(roomTotelPrice => roomTotelPrice);
                return hotelTotalPrice;
            };
            totalPrice = calc(hotelSegmentModel, hotelPostModel);
            if (delayHotelSegmentModel != null)
            {
                totalPrice += calc(delayHotelSegmentModel, delayHotelPostModel);
            }

            if (groundServiceModels.Count > 0)
            {
                var groundServiceModel = groundServiceModels.Single(gs => gs.GroundServiceId == selectedGroundServiceId);
                decimal groundServiceTotalPrice = adultCount * groundServiceModel.AdultUnitPrice +
                                                    childCount * groundServiceModel.ChildUnitPrice;
                totalPrice += groundServiceTotalPrice;
            }
            return totalPrice;
        }
    }
}