using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using HHTravel.CRM.Booking_Online.Site.Models;
using HHTravel.CRM.Booking_Online.Model;
using HHTravel.CRM.Booking_Online.Model.Exceptions;

namespace HHTravel.CRM.Booking_Online.Site.Controllers
{
    public class OrderController : BaseController
    {
        //
        // GET: /Order/

        public ActionResult Index(string productNo, string date)
        {
            DateTime departureDate = DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture);

            var p = ProductService.GetProduct(productNo);

            OrderModel om = new OrderModel
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Days = p.Days,
                DepartureDate = departureDate,
            };

            if (p.TravelType != TravelType.单品游)
            {
                return View(om);
            }
            else
            {
                om.Description = p.Description;
                return View("Index2", om);
            }
        }

        [HttpPost]
        public ActionResult Submit(OrderModel om)
        {
            if (!CaptchaController.Validate(om.Captcha))
            {
                ModelState.AddModelError("Captcha", "验证码错误");
            }

            if (!om.AdultNum.HasValue || om.AdultNum.Value <= 0)
            {
                if (!om.ChildNum.HasValue || om.ChildNum.Value <= 0)
                {
                    ModelState.AddModelError("ChildNum", "请输入参加人数");
                }
            }

            if (!om.ChildNum.HasValue || om.ChildNum.Value <= 0)
            {
                if (!om.AdultNum.HasValue || om.AdultNum.Value <= 0)
                {
                    ModelState.AddModelError("ChildNum", "请输入参加人数");
                }
            }

            if (om.StayReturnDate.HasValue)
            {
                if (!om.IsStay.HasValue)
                {
                    ModelState.AddModelError("IsStay", "是否需安排续住");
                }
                if (!om.AirTicketOneself.HasValue)
                {
                    ModelState.AddModelError("AirTicketOneself", "是否仅参加当地行程，机票自理");
                }
                // 延迟回程日期必须在（出发日期+行程天数）之后
                if (om.StayReturnDate.Value <= om.DepartureDate.AddDays(om.Days - 1))
                {
                    ModelState.AddModelError("StayReturnDate", "延迟回程日期须在正常回程日期之后");
                }
            }

            if (!this.ModelState.IsValid)
            {
                return View("Index", om);
            }

            Order order = InsertOrder(om);
            SendEmail("IndexSuccess", om, order);
            return View("IndexSuccess", om);
        }

        [HttpPost]
        public ActionResult Index2(string productNo, DateTime departureDate, DateTime returnDate, SelectedRoomClassModel[] roomClasses, DateTime? stayReturnDate, SelectedTicketModel[] tickets)
        {
            tickets = tickets ?? new SelectedTicketModel[] { };
            // 过滤掉房间数为0的房型
            List<SelectedRoomClassModel> selectedRoomClasses =
                roomClasses
                .Where(r => r.RoomCount > 0 || r.StayCount > 0)
                .ToList();
            List<SelectedTicketModel> selectedTickets = tickets.Where(t => t.Count > 0).ToList();

            var p = ProductService.GetProduct(productNo);
            // 没有选择房型
            if (selectedRoomClasses.Count == 0)
            {
                throw new NotImplementedException();
            }
            // 附加上房型相关的信息 (通过RoomClass属性） todo: 考虑通过hidden input传入，避免重复查询
            var allRoomClasses = (from r in ProductService.GetRoomClasses(p.ProductId, departureDate, true)
                                  select r).ToList();
            selectedRoomClasses = (from r in allRoomClasses
                                   join sr in selectedRoomClasses
                                   on r.RoomClassId equals sr.RoomClassId
                                   select new SelectedRoomClassModel
                                   {
                                       RoomClassId = r.RoomClassId,
                                       RoomClassName = r.RoomClassName,
                                       Price = r.Price,
                                       StayPrice = r.StayPrice,
                                       RoomCount = sr.RoomCount,
                                       StayCount = sr.StayCount
                                   }).ToList();

            List<SelectedRoomClassModel> stayRoomClasses = (from r in selectedRoomClasses
                                                            where r.StayCount > 0
                                                            select r).ToList();
            selectedRoomClasses = (from r in selectedRoomClasses
                                   where r.RoomCount > 0
                                   select r).ToList();

            // 附加上机票相关的信息
            var allTickets = (from t in ProductService.GetTickets(p.ProductId, departureDate, true)
                              select t).ToList();
            selectedTickets = (from t in allTickets
                               join st in selectedTickets
                               on t.TicketId equals st.TicketId
                               select new SelectedTicketModel
                               {
                                   TicketId = t.TicketId,
                                   Price = t.Price,
                                   FlightList = t.FlightList,
                                   AdditionalPurchasesNote = t.AdditionalPurchasesNote,
                                   FlightClassId = t.FlightClassId,
                                   FlightClassName = t.FlightClassName,
                                   Count = st.Count
                               }).ToList();

            OrderModel om = new OrderModel
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Days = p.Days,
                DepartureDate = departureDate,
                ReturnDate = returnDate,

                StayReturnDate = stayReturnDate,
                Description = p.Description,
                SelectedRoomClasses = selectedRoomClasses,
                StayRoomClasses = stayRoomClasses,
                SelectedTickets = selectedTickets,
            };

            if (selectedRoomClasses.Any(sr => sr.StayCount > 0) && stayReturnDate.HasValue)
            {
                om.IsStay = true;
            }

            return View(om);
        }

        [HttpPost]
        public ActionResult Submit2(OrderModel om)
        {
            if (!CaptchaController.Validate(om.Captcha))
            {
                ModelState.AddModelError("Captcha", "验证码错误");
            }

            if (!om.AdultNum.HasValue || om.AdultNum.Value <= 0)
            {
                if (!om.ChildNum.HasValue || om.ChildNum.Value <= 0)
                {
                    ModelState.AddModelError("ChildNum", "请输入参加人数");
                }
            }

            if (!om.ChildNum.HasValue || om.ChildNum.Value <= 0)
            {
                if (!om.AdultNum.HasValue || om.AdultNum.Value <= 0)
                {
                    ModelState.AddModelError("ChildNum", "请输入参加人数");
                }
            }

            if (om.StayReturnDate.HasValue)
            {
                if (!om.IsStay.HasValue)
                {
                    ModelState.AddModelError("IsStay", "是否需安排续住");
                }
                if (!om.AirTicketOneself.HasValue)
                {
                    ModelState.AddModelError("AirTicketOneself", "是否仅参加当地行程，机票自理");
                }
                // 延迟回程日期必须在（出发日期+行程天数）之后
                if (om.StayReturnDate.Value <= om.DepartureDate.AddDays(om.Days - 1))
                {
                    ModelState.AddModelError("StayReturnDate", "延迟回程日期须在正常回程日期之后");
                }
            }

            if (!this.ModelState.IsValid)
            {
                return View("Index2", om);
            }

            Order order = InsertOrder(om);
            SendEmail("Index2Success", om, order);
            return View("Index2Success", om);
        }

        public ActionResult Customize(int productId)
        {
            var p = ProductService.GetProduct(productId);

            CustomizeOrderModel om = new CustomizeOrderModel
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
            };

            return View(om);
        }

        [HttpPost]
        public ActionResult Customize(CustomizeOrderModel coModel)
        {
            if (!CaptchaController.Validate(coModel.Captcha))
            {
                ModelState.AddModelError("Captcha", "验证码错误");
                // 清空错误的输入
                coModel.Captcha = "";
            }

            if (!coModel.AdultNum.HasValue || coModel.AdultNum.Value <= 0)
            {
                if (!coModel.ChildNum.HasValue || coModel.ChildNum.Value <= 0)
                {
                    ModelState.AddModelError("ChildNum", "请输入参加人数");
                }
            }

            if (!coModel.ChildNum.HasValue || coModel.ChildNum.Value <= 0)
            {
                if (!coModel.AdultNum.HasValue || coModel.AdultNum.Value <= 0)
                {
                    ModelState.AddModelError("ChildNum", "请输入参加人数");
                }
            }

            /* 量身定做时，用户可提前回程日期或延后回程日期
            // 延迟回程日期必须在（出发日期+行程天数）之后
            if (om.StayReturnDate.Value < om.DepartureDate.Value.AddDays(om.Days - 1))
            {
                ModelState.AddModelError("StayReturnDate", "延迟回程日期须在正常回程日期之后");
            }*/

            if (!this.ModelState.IsValid)
            {
                var r = View(coModel);
                r.MasterName = "~/Views/Shared/_LayoutPopup.cshtml";
                return r;
            }

            Product product = ProductService.GetProduct(coModel.ProductId);

            var order = new Order
            {
                AdultNum = coModel.AdultNum,
                ChildNum = coModel.ChildNum,
                ContactFavorite = coModel.ContactFavorite.Value,
                ConvinientTime = coModel.ConvinientTime.Value,
                DepartureDate = coModel.DepartureDate.Value,
                Email = coModel.Email,
                FirstName = coModel.FirstName,
                FirstNameEn = coModel.FirstNameEn,
                //OrderId
                PhoneNumber = coModel.PhoneNumber,
                RequestFrom = coModel.RequestFrom,
                SpecialHope = coModel.SpecialHope,  
                SpecialMemento = coModel.SpecialMemento,
                Suranme = coModel.Suranme,
                SurnameEn = coModel.SurnameEn,
                Price = product.MinPrice, // todo: 订单中的价格等于对应旅行产品在用户指定日期的价格
                Product = product,
            };

            order = OrderService.Insert(order);
            // 读取订单号
            coModel.OrderNo = order.OrderNo;
            SendEmail("CustomizeSuccess", coModel, order);
            // 此处受限不能使用RedirectToAction http://stackoverflow.com/questions/2056421/why-are-redirect-results-not-allowed-in-child-actions-in-asp-net-mvc-2
            return View("CustomizeSuccess", coModel);
        }

        private Order InsertOrder(OrderModel om)
        {
            Product product = ProductService.GetProduct(om.ProductId);

            var order = new Order
            {
                AdultNum = om.AdultNum,
                AirTicketOneself = om.AirTicketOneself,
                ChildNum = om.ChildNum,
                ContactFavorite = om.ContactFavorite.Value,
                ConvinientTime = om.ConvinientTime.Value,
                Days = om.Days,
                DepartureDate = om.DepartureDate,
                Email = om.Email,
                FirstName = om.FirstName,
                FirstNameEn = om.FirstNameEn,
                IsStay = om.IsStay,
                //OrderId
                PhoneNumber = om.PhoneNumber,
                RequestFrom = om.RequestFrom,
                ReturnDate = om.StayReturnDate ?? om.ReturnDate,    // 没有延住则取预定的回程日期
                SpecialMemento = om.StayNote,  // todo: confirm
                Suranme = om.Suranme,
                SurnameEn = om.SurnameEn,
                Price = product.MinPrice, // 订单中的价格等于对应旅行产品的价格
                Product = product,
            };

            if (product.TravelType != TravelType.单品游)
            {
                order = OrderService.Insert(order);
            }
            else
            {
                var orderItems = new List<OrderItem>();
                // 酒店
                if (om.SelectedRoomClasses.Count > 0)
                {
                    var hotel = ProductService.GetHotels(product.ProductId).FirstOrDefault();

                    if (hotel == null)
                    {
                        throw new DataException("单品游需要有且只应该有一个酒店");
                    }

                    foreach (var roomClass in om.SelectedRoomClasses)
                    {
                        var oi = new OrderItem
                        {
                            AdultNum = order.AdultNum,
                            AdultPrice = roomClass.Price,
                            Amount = roomClass.RoomCount,
                            ChildNum = order.ChildNum,
                            //ChildPrice = 
                            DepartureDate = order.DepartureDate,
                            IsHotelStay = order.IsStay,
                            ReturnDate = order.ReturnDate,
                            HotelDays = order.Days, // todo: 住宿日期是否应考虑延住后的退房日期
                            ProductId = hotel.HotelId,
                            SpecId = roomClass.RoomClassId,
                        };
                        orderItems.Add(oi);
                    }
                }
                // 机票
                if (om.SelectedTickets != null && om.SelectedTickets.Count > 0)
                {
                    foreach (var ticket in om.SelectedTickets)
                    {
                        var oi = new OrderItem
                        {
                            AdultNum = order.AdultNum,
                            AdultPrice = ticket.Price,
                            Amount = ticket.Count,
                            ChildNum = order.ChildNum,
                            //ChildPrice = 
                            DepartureDate = om.DepartureDate,
                            ProductId = ticket.TicketId,
                            SpecId = ticket.TicketId,
                        };
                        orderItems.Add(oi);
                    }
                }

                order = OrderService.Insert(order, orderItems);
            }
            // 读取订单号
            om.OrderNo = order.OrderNo;

            return order;
        }

        private void SendEmail(string emailTemplateName, object model, Order order)
        {
            string body = RenderRazorViewToString(emailTemplateName, model);

            Email email = new Email
            {
                Sender = null,
                Recipients = order.Email,
                Subject = string.Format("鸿鹄产品【{0}】订单提交邮件", order.Product.ProductName),
                Body = body,
                EmailType = EmailType.Order,
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,  
            };
            EmailService.Send(email);
        }
    }
}
