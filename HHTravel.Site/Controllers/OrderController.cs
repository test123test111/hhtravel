using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HHTravel.DomainModel;
using HHTravel.Infrastructure;
using HHTravel.Infrastructure.Crosscutting;
using HHTravel.Site.Models;

namespace HHTravel.Site.Controllers
{
    public class OrderController : ControllerBase
    {
        //
        // GET: /Order/

        public ActionResult Customize(string productNo)
        {
            var p = ProductService.GetProduct(productNo);

            CustomizeOrderModel om = new CustomizeOrderModel
            {
                ProductNo = p.ProductNo,
                ProductName = p.ProductName,
            };

            return View(om);
        }

        [HttpPost]
        public ActionResult Customize(CustomizeOrderModel om)
        {
            if (!CaptchaController.Validate(om.Captcha))
            {
                ModelState.AddModelError("Captcha", "验证码错误");

                // 清空错误的输入
                om.Captcha = "";
            }

            if (om.AdultNum <= 0 && om.ChildNum <= 0)
            {
                ModelState.AddModelError("ChildNum", "请输入参加人数");
            }

            /* 量身定做时，用户可提前回程日期或延后回程日期

            // 延迟回程日期必须在（出发日期+行程天数）之后
            if (om.StayReturnDate.Value < om.RetureDate)
            {
                ModelState.AddModelError("StayReturnDate", "延迟回程日期须在正常回程日期之后");
            }*/

            if (!this.ModelState.IsValid)
            {
                var r = View(om);
                r.MasterName = "~/Views/Shared/_LayoutPopup.cshtml";
                return r;
            }

            Product product = ProductService.GetProduct(om.ProductNo);

            var order = new Order
            {
                AdultNum = om.AdultNum,
                ChildNum = om.ChildNum,
                ContactFavorite = (ContactFavorite)om.ContactFavorite,
                ConvinientTime = (ConvinientTime)om.ConvinientTime,
                Days = om.Days.Value,
                DepartDate = om.DepartureDate.Value, // 用户选定的日期
                Email = om.Email,
                FirstName = om.FirstName,
                FirstNameEn = om.FirstNameEn,

                //OrderId
                PhoneNumber = om.PhoneNumber,
                RequestFrom = om.RequestFrom,
                SpecialHope = om.SpecialHope,
                SpecialMemento = om.SpecialMemento,
                PlanReturnDate = om.DepartureDate.Value.AddDays(product.Days - 1),//按产品的天数计算
                ActualReturnDate = om.DepartureDate.Value.AddDays(om.Days.Value - 1),//按用户选择的天数计算
                LastName = om.LastName,
                LastNameEn = om.LastNameEn,

                OrderType = 1,
                Product = product,
                OrderItemList = new List<OrderItem>()
            };

            order = OrderService.Add(order);

            // 读取订单号
            om.OrderNo = order.OrderNo;

            om.UsedByEmail = true;
            SendOrderEmail("CustomizeSuccess", om, order);

            om.UsedByEmail = false;

            om.AdultNum = om.AdultNum;
            om.ChildNum = om.ChildNum;

            // 此处受限不能使用RedirectToAction http://stackoverflow.com/questions/2056421/why-are-redirect-results-not-allowed-in-child-actions-in-asp-net-mvc-2
            return View("CustomizeSuccess", om);
        }

        #region TravelType1/4

        public ActionResult Index1(PreOrderModel preOrderModel)
        {
            preOrderModel.Validate(this.ModelState);

            if (!this.ModelState.IsValid)
            {
                throw new NotSupportedException();
            }

            OrderModel om = new OrderModel
            {
                PreOrderModel = preOrderModel,
            };

            return View(om);
        }

        [HttpPost]
        public ActionResult Submit1(OrderModel om)
        {
            if (om.PreOrderModel.AdultCount <= 0 && om.PreOrderModel.ChildCount <= 0)
            {
                ModelState.AddModelError("PreOrderModel.ChildCount", "请输入参加人数");
            }

            om.BasicInfoModel.Validate(this.ModelState);
            om.StayInfoModel.Validate(this.ModelState, om.PreOrderModel.PlanReturnDate);
            om.Validate(this.ModelState);

            if (!this.ModelState.IsValid)
            {
                return View("Index1", om);
            }

            Order order = InsertOrder(om);

            om.UsedByEmail = true;
            SendOrderEmail("Index1Success", om, order);

            om.UsedByEmail = false;

            return View("Index1Success", om);
        }

        #endregion TravelType1/4

        #region TravelType2

        [HttpPost]
        public ActionResult Index2(PreOrderModel preOrderModel)
        {
            preOrderModel.Validate(this.ModelState);

            if (!this.ModelState.IsValid)
            {
                throw new NotSupportedException();
            }

            var ticketSegment = ProductService.GetTicketSegment(preOrderModel.ProductId);
            var builder = new TicketSegmentModelBuilder(preOrderModel.DepartureDate);
            var ticketSegmentModel = builder.CreateFrom(ticketSegment);
            var selectedTicketModel = ticketSegmentModel.TicketModelList.First(tm => tm.TicketId == preOrderModel.SelectedTicketId);

            var selectedRooms = from rc in preOrderModel.RoomClassPostModelList
                                where rc.Count > 0
                                select rc;

            var hotelSegments = ProductService.GetHotelSegments(preOrderModel.ProductId);
            var builder2 = new HotelSegmentModelsBuilder(preOrderModel.DepartureDate);
            var hotelSegmentModelList = builder2.CreateFrom(hotelSegments);
            hotelSegmentModelList = (from seg in hotelSegmentModelList
                                     join rc in selectedRooms
                                     on seg.SegmentId equals rc.SegmentId into gRoomClass
                                     let selectedHotelId = gRoomClass.Select(rc => rc.HotelId).First()
                                     let selectedHotelModel = seg.HotelModelList.First(hotel => hotel.HotelId == selectedHotelId)
                                     select new HotelSegmentModel
                                     {
                                         DestinationCity = seg.DestinationCity,
                                         HotelModelList = seg.HotelModelList,
                                         Nights = seg.Nights,
                                         SelectedHotelModel = selectedHotelModel,
                                         SelectedRoomClassPostModelList = (from rc in gRoomClass
                                                                           select rc).ToList(),
                                     }).ToList();

            // 计算订单价格 todo: ?价格计算应该在这里吗
            decimal totalPrice;
            int personCount = preOrderModel.AdultCount + preOrderModel.ChildCount;

            decimal ticketsTotalPrice = selectedTicketModel.Price.Value * personCount;
            decimal hotelsTotalPrice = (from seg in hotelSegmentModelList
                                        from rc in seg.SelectedRoomClassPostModelList
                                        select (int)(rc.SegmentPrice * rc.Count)).Sum(); // 段价*房间数

            totalPrice = ticketsTotalPrice + hotelsTotalPrice;

            OrderModel om = new OrderModel
            {
                PreOrderModel = preOrderModel,
                SelectedTicketModel = selectedTicketModel,
                HotelSegmentModelList = hotelSegmentModelList,
                Amount = totalPrice,
                OtherCustomerModelList = (from i in new IntRange(1, personCount)
                                          select new CustomerModel()).ToList(),
            };

            return View(om);
        }

        [HttpPost]
        public ActionResult Submit2(OrderModel om)
        {
            om.BasicInfoModel.Validate(this.ModelState);
            om.StayInfoModel.Validate(this.ModelState, om.PreOrderModel.PlanReturnDate);
            om.Validate(this.ModelState);

            if (!this.ModelState.IsValid)
            {
                return View("Index2", om);
            }

            Order order = InsertOrder(om);

            om.OtherCustomerModelList = om.OtherCustomerModelList.Where(cm => !string.IsNullOrEmpty(cm.FirstName)
                                                                                || !string.IsNullOrEmpty(cm.LastName)
                                                                                || !string.IsNullOrEmpty(cm.FirstNameEn)
                                                                                || !string.IsNullOrEmpty(cm.LastNameEn))
                                                                 .ToList();

            om.UsedByEmail = true;
            SendOrderEmail("Index2Success", om, order);

            om.UsedByEmail = false;

            // todo: 应用PRG模式
            return View("Index2Success", om);
        }

        #endregion TravelType2

        #region TravelType3

        [HttpPost]
        public ActionResult Index3(PreOrderModel preOrderModel)
        {
            preOrderModel.Validate(this.ModelState);

            if (!this.ModelState.IsValid)
            {
                throw new NotSupportedException();
            }

            OrderModel om = new OrderModel
            {
                PreOrderModel = preOrderModel,
            };

            return View(om);
        }

        /// <summary>
        /// TravelType3
        /// </summary>
        /// <param name="om"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit3(OrderModel om)
        {
            var tickets = om.PreOrderModel.TicketModels ?? new List<TicketModel>();

            // 过滤掉房间数为0的房型
            var selectedRoomClasses = om.PreOrderModel.RoomClassModels
                                     .Where(r => r.Count > 0)
                                     .ToList();
            var selectedStayRoomClasses = om.PreOrderModel.StayRoomClassModels ?? new List<RoomClassModel>();
            selectedStayRoomClasses = selectedStayRoomClasses
                                 .Where(r => r.StayCount > 0)
                                 .ToList();
            var selectedTickets = tickets
                                     .Where(t => t.Count > 0)
                                     .ToList();

            // 没有选择房型
            if (selectedRoomClasses.Count == 0)
            {
                throw new NotImplementedException();
            }

            //todo: om.PreOrderModel.Validate(this.ModelState);
            om.BasicInfoModel.Validate(this.ModelState);

            // TravelType3的延住信息只包含延住日期
            if (om.StayInfoModel.StayReturnDate.HasValue)
            {
                // 延迟回程日期必须在（出发日期+行程天数）之后
                if (om.StayInfoModel.StayReturnDate.Value <= om.PreOrderModel.PlanReturnDate)
                {
                    ModelState.AddModelError("StayReturnDate", "延迟回程日期须在正常回程日期之后");
                }
            }

            if (om.StayInfoModel.StayReturnDate.HasValue && selectedRoomClasses.Count > 0)
            {
                om.StayInfoModel.IsHotelStay = true;
            }

            if (!this.ModelState.IsValid)
            {
                return View("Index3", om);
            }

            Order order = InsertOrder(om);

            om.UsedByEmail = true;
            SendOrderEmail("Index3Success", om, order);

            om.UsedByEmail = false;

            return View("Index3Success", om);
        }

        #endregion TravelType3

        private Order InsertOrder(OrderModel om)
        {
            var p = ProductService.GetProduct(om.PreOrderModel.ProductNo);
            var order = new OrderBuilder(om, p).Build();
            order = OrderService.Add(order);

            // 读取订单号
            om.OrderNo = order.OrderNo;

            return order;
        }
    }
}