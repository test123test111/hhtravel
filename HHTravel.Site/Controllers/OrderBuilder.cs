using System;
using System.Linq;
using HHTravel.ApplicationService.ApplicationServiceImp;
using HHTravel.DomainModel;
using HHTravel.Infrastructure.Crosscutting;
using HHTravel.Site.Models;
using HHTravel.Site.Models.OrderWizard;

namespace HHTravel.Site.Controllers
{
    public class OrderBuilder
    {
        private OrderModel _om;
        private Product _p;

        //private RoomClass minPriceRoom;

        public OrderBuilder(OrderModel model, Product product)
        {
            this._om = model;
            this._p = product;
        }

        public static Order Build(Step5Model model, Product product)
        {
            var order = new Order
            {
                DepartDate = model.BeginDate,
                PlanReturnDate = model.BeginDate.AddDays(product.Days),
                ActualReturnDate = model.EndDate,
                Days = (short)model.Days,

                FirstName = model.BasicInfoModel.FirstName,
                FirstNameEn = model.BasicInfoModel.FirstNameEn,
                LastName = model.BasicInfoModel.LastName,
                LastNameEn = model.BasicInfoModel.LastNameEn,

                AdultNum = (short)model.AdultCount,
                ChildNum = (short)model.ChildCount,
                ContactFavorite = (ContactFavorite)model.BasicInfoModel.ContactFavorite,
                ConvinientTime = (ConvinientTime)model.BasicInfoModel.ConvinientTime,
                Email = model.BasicInfoModel.Email,
                PhoneNumber = model.BasicInfoModel.PhoneNumber,
                IsHotelStay = (model.DelayHotelSegment != null),
                AirTicketOneself = (!model.FlightsSegmentPlans.Any()), // 没有使用系统机票则视为机票自理
                StayNote = model.SpecialRequirement,

                Amount = model.TotalPrice,

                OrderType = 0,
                Product = product,
            };

            order.PassengerList = (from m in model.PassengerModelList
                                   select new Passenger
                                   {
                                       FirstName = m.Name,
                                       LastName = m.NameEn,
                                   }).ToList();

            OrderItem orderItem = null;

            #region 住宿

            var hotelSegment = model.HotelSegment;
            var hotel = hotelSegment.HotelModelList.First();

            foreach (var room in hotel.RoomClassModelList)
            {
                // 住X晚的累计单价
                // !todo: 读取酒店的名称作为ProductName
                orderItem = new OrderItem(hotelSegment.HotelSegmentType.ToString(), hotel.HotelId, hotel.HotelName, room.RoomClassId, room.RoomClassName, room.Count, room.SegmentPrice,
                        hotelSegment.CheckinDate, hotelSegment.CheckoutDate, "间");
                orderItem.Times = hotelSegment.Nights;

                order.OrderItemList.Add(orderItem);
            }

            var delayHotelSegment = model.DelayHotelSegment;
            if (delayHotelSegment != null)
            {
                hotel = delayHotelSegment.HotelModelList.First();

                foreach (var room in hotel.RoomClassModelList)
                {
                    // 住X晚的累计单价
                    // !todo: 读取酒店的名称作为ProductName
                    orderItem = new OrderItem(delayHotelSegment.HotelSegmentType.ToString(), hotel.HotelId, hotel.HotelName, room.RoomClassId, room.RoomClassName, room.Count, room.SegmentPrice,
                            hotelSegment.CheckinDate, hotelSegment.CheckoutDate, "间");
                    orderItem.Times = hotelSegment.Nights;

                    order.OrderItemList.Add(orderItem);
                }
            }

            #endregion 住宿

            #region 机票

            for (int i = 0; i < model.FlightsSegmentPlans.Count; i++)
            {
                var segment = model.FlightsSegments[i];
                var plan = model.FlightsSegmentPlans[i];
                string remark = string.Format("舱等: {0}, 出发: {1}, 到达: {2}", plan.FlightSeatName, plan.DepartTime, plan.ArrivalTime);

                orderItem = new OrderItem("机票", null, plan.FlightNo, null, "成人", model.AdultCount, plan.AdultPrice,
                                       segment.DepartDate, null, "人");
                orderItem.Times = 1;
                orderItem.Remark = remark + string.Format(", 票价: {0}, 税: {1}, 附加费: {2}", plan.AdultPrice, plan.AdultTax, plan.AdultFuelSurcharges);

                order.OrderItemList.Add(orderItem);

                if (model.ChildCount > 0)
                {
                    orderItem = new OrderItem("机票", null, plan.FlightNo, null, "儿童", model.ChildCount, plan.ChildPrice,
                                                          segment.DepartDate, null, "人");
                    orderItem.Times = 1;
                    orderItem.Remark = remark + string.Format(", 票价: {0}, 税: {1}, 附加费: {2}", plan.ChildPrice, plan.ChildTax, plan.ChildFuelSurcharges);

                    order.OrderItemList.Add(orderItem);
                }
            }

            #endregion 机票

            #region 地面产品

            var groundProduct = model.GroundService;

            // !todo: 地面产品的名称
            if (groundProduct != null)
            {
                orderItem = new OrderItem("地面", groundProduct.GroundServiceId, groundProduct.ServiceName, null, "成人", model.AdultCount, groundProduct.AdultUnitPrice,
                                        null, null, "人");
                orderItem.Times = 1;
                order.OrderItemList.Add(orderItem);

                if (model.ChildCount > 0)
                {
                    orderItem = new OrderItem("地面", groundProduct.GroundServiceId, groundProduct.ServiceName, null, "儿童", model.ChildCount, groundProduct.ChildUnitPrice,
                                                       null, null, "人");
                    orderItem.Times = 1;
                    order.OrderItemList.Add(orderItem);
                }
            }

            #endregion 地面产品

            return order;
        }

        public Order Build()
        {
            var order = new Order
            {
                DepartDate = _om.PreOrderModel.DepartureDate,
                PlanReturnDate = _om.PreOrderModel.PlanReturnDate,
                Days = _om.PreOrderModel.Days,

                FirstName = _om.BasicInfoModel.FirstName,
                FirstNameEn = _om.BasicInfoModel.FirstNameEn,
                LastName = _om.BasicInfoModel.LastName,
                LastNameEn = _om.BasicInfoModel.LastNameEn,

                AdultNum = _om.PreOrderModel.AdultCount,
                ChildNum = _om.PreOrderModel.ChildCount,
                ContactFavorite = (ContactFavorite)_om.BasicInfoModel.ContactFavorite,
                ConvinientTime = (ConvinientTime)_om.BasicInfoModel.ConvinientTime,
                Email = _om.BasicInfoModel.Email,
                PhoneNumber = _om.BasicInfoModel.PhoneNumber,

                ActualReturnDate = _om.StayInfoModel.StayReturnDate,
                IsHotelStay = _om.StayInfoModel.IsHotelStay ?? false,
                AirTicketOneself = _om.StayInfoModel.AirTicketOneself,
                StayNote = _om.StayInfoModel.StayNote,

                Amount = _om.Amount,

                RequestFrom = _om.RequestFrom,

                OrderType = 0,
                Product = _p,
            };

            var travelType = _p.TravelType;

            // !兼容老的TravelType2产品: 订单明细的处理方式与TravelType1相同
            var productDatePrice = _p.GetPriceDate(_om.PreOrderModel.DepartureDate);
            if (_p.TravelType == TravelType.TravelType2 && productDatePrice.ProductBasicPrice != 0)
            {
                travelType = TravelType.TravelType1;
            }

            order.PassengerList = (from m in this._om.OtherCustomerModelList
                                   select new Passenger
                                   {
                                       FirstName = m.FirstName,
                                       LastName = m.LastName,
                                       FirstNameEn = m.FirstNameEn,
                                       LastNameEn = m.LastNameEn,
                                   }).ToList();

            AppendOrderItems(order, travelType);

            order.Amount = order.OrderItemList.Sum(oi => oi.Amount);

            return order;
        }

        private void AppendOrderItems(Order order, TravelType travelType)
        {
            if (travelType == TravelType.TravelType2)
            {
                return;
            }

            // !容错
            if (travelType == TravelType.TravelType1 || travelType == TravelType.TravelType4)
            {
                var roomClasses = new ProductServiceImp().GetRoomClasses(_p.ProductId, order.DepartDate);
                if (roomClasses.Count == 0)
                {
                    return;
                }
            }

            OrderItem orderItem;
            if (travelType == TravelType.TravelType1 || travelType == TravelType.TravelType4)
            {
                int roomCount = order.AdultNum + order.ChildNum;

                // 非TravelType3产品，订单子项只包含最低价的房型，且不包含机票
                RoomClass minPriceRoom;
                var rooms = new ProductServiceImp().GetRoomClasses(_p.ProductId, order.DepartDate);
                minPriceRoom = rooms.Min();

                Func<decimal, OrderItem> createRoomClassOrderItem = (unitPrice) =>
                {
                    // !todo: 应为酒店的ProductId和ProductName
                    // !用户下TravelType1/4的单时，系统不需要也不支持用户选规格,系统自动使用最低价的房型作为保存订单使用的规格
                    var oi = new OrderItem("线路", null, null, minPriceRoom.RoomClassId, minPriceRoom.RoomClassName, roomCount, unitPrice,
                        order.DepartDate, order.PlanReturnDate, "人");
                    oi.Times = (oi.EndDate.Value - oi.BeginDate.Value).Days;
                    return oi;
                };

                // 订单子项的金额计算，按照房型和舱等的最低价
                var minPriceDate = minPriceRoom.GetPriceDate(order.DepartDate);

                orderItem = createRoomClassOrderItem(minPriceDate.Price);
                order.OrderItemList.Add(orderItem);

                if (order.IsHotelStay)
                {
                    orderItem = createRoomClassOrderItem(minPriceDate.StayPricePerDay);

                    // 改写部分属性
                    orderItem.ProductType = "延住";

                    // !todo: 应为酒店的ProductId和ProductName
                    //ProductId = room.,
                    //ProductName = room.
                    orderItem.BeginDate = order.PlanReturnDate.AddDays(1);
                    orderItem.EndDate = order.ActualReturnDate;
                    orderItem.Times = (orderItem.EndDate.Value - orderItem.BeginDate.Value).Days;

                    order.OrderItemList.Add(orderItem);
                }

                // !重新计算订单金额
                order.Amount = order.OrderItemList.Sum(oi => oi.Amount);
            }
            else if (travelType == TravelType.TravelType3)
            {
                // 房型
                if (_om.PreOrderModel.RoomClassModels.Count() > 0)
                {
                    foreach (var room in _om.PreOrderModel.RoomClassModels)
                    {
                        // todo: 应为酒店的ProductId和ProductName
                        orderItem = new OrderItem("酒店", null, null, room.RoomClassId, room.RoomClassName, room.Count, room.SegmentPrice,
                        order.DepartDate, order.PlanReturnDate, "间");
                        orderItem.Times = (orderItem.EndDate.Value - orderItem.BeginDate.Value).Days;

                        order.OrderItemList.Add(orderItem);
                    }

                    if (_om.PreOrderModel.StayRoomClassModels != null)
                    {
                        foreach (var room in _om.PreOrderModel.StayRoomClassModels)
                        {
                            // todo: 此处Nullable的使用需要重新考虑
                            var stayCount = room.StayCount ?? 0;
                            if (stayCount <= 0) continue;

                            decimal stayPrice = room.StayPrice ?? 0;

                            // !todo: 应为延住的日期范围
                            orderItem = new OrderItem("延住", null, null, room.RoomClassId, room.RoomClassName, stayCount, stayPrice,
                                order.DepartDate, order.PlanReturnDate, "间");

                            order.OrderItemList.Add(orderItem);
                        }
                    }
                }

                // 机票
                if (_om.PreOrderModel.TicketModels.Count > 0)
                {
                    // TravelType3的机票不区分成人价和儿童价
                    int ticketCount = order.AdultNum + order.ChildNum;
                    foreach (var ticket in _om.PreOrderModel.TicketModels)
                    {
                        var ticketUnitPrice = ticket.Price ?? 0;

                        // 规格按舱等，而不是成人/儿童的维度
                        // !todo: 回程机票的起飞日期应为行程的结束日期
                        orderItem = orderItem = new OrderItem("机票", null, null, null, ticket.FlightClassName, ticketCount, ticketUnitPrice,
                                order.DepartDate, order.PlanReturnDate, "人");
                        orderItem.Times = 1;

                        order.OrderItemList.Add(orderItem);
                    }
                }

                // 重新计算订单金额
                order.Amount = order.OrderItemList.Sum(oi => oi.Amount);
            }
        }
    }
}