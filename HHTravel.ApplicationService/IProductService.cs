using System;
using System.Collections.Generic;
using System.ServiceModel;
using HHTravel.DomainModel;
using HHTravel.Infrastructure.Crosscutting;

namespace HHTravel.ApplicationService
{
    [ServiceContract]
    public interface IProductService
    {
        /// <summary>
        /// 根据出发日期范围获取产品
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [OperationContract]
        IList<Product> FindByDepartureDate(DateTime beginDate, DateTime endDate,
            Pager pager);

        /// <summary>
        /// 根据目的地获取产品
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="regionId"></param>
        /// <returns></returns>
        [OperationContract]
        IList<Product> FindByDestination(int groupId, int? regionId,
            Pager pager);

        /// <summary>
        /// 根据旅行主题获取产品
        /// </summary>
        /// <param name="interestId"></param>
        /// <returns></returns>
        [OperationContract]
        IList<Product> FindByInterest(int interestId,
            Pager pager);

        /// <summary>
        /// 根据产品名称获取产品
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        [OperationContract]
        IList<Product> FindByProductName(string productName,
            Pager pager);

        /// <summary>
        /// 根据旅游型态获取产品
        /// </summary>
        /// <param name="travelTypeId"></param>
        /// <returns></returns>
        [OperationContract]
        IList<Product> FindByTravelType(int travelTypeId,
            Pager pager);

        /// <summary>
        /// 获取所有出发地
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IList<DepartureCity> GetAllDepartureCitys();

        /// <summary>
        /// 获取所有出发月份
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IList<DepartureMonth> GetAllDepartureMonths();

        /// <summary>
        /// 获取所有目的地所属分组
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IList<DestinationGroup> GetAllDestinationGroups();

        /// <summary>
        /// 获取所有旅行主题
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IList<Interest> GetAllInterests();

        /// <summary>
        /// 获取所有产品编号
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IList<string> GetAllProductNos();

        /// <summary>
        /// 获取所有旅游型态
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IList<TravelType> GetAllTravelTypes();

        /// <summary>
        /// 获取指定产品的酒店信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [OperationContract]
        IList<Hotel> GetHotels(int productId);

        /// <summary>
        /// 获取产品的酒店行程段
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [OperationContract]
        IList<HotelSegment> GetHotelSegments(int productId);

        /// <summary>
        /// 根据产品Id获取产品
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [OperationContract]
        Product GetProduct(int productId);

        /// <summary>
        /// 根据产品序列号获取产品
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [OperationContract]
        Product GetProduct(string productNo);

        /// <summary>
        /// 获取指定产品的房型信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [OperationContract]
        IList<RoomClass> GetRoomClasses(int productId);

        /// <summary>
        /// 获取指定产品的指定日期的房型信息
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [OperationContract]
        IList<RoomClass> GetRoomClasses(int productId, DateTime date);

        /// <summary>
        /// 获取指定产品的日程项
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="sort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        [OperationContract]
        IList<ScheduleItem> GetScheduleItems(int productId, Pager pager);

        /// <summary>
        /// 获取指定产品的机票
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        IList<Ticket> GetTickets(int productId);

        /// <summary>
        /// 获取指定产品的指定日期的机票
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="date"></param>
        /// <param name="onlyWithMinPrice">true: 同房型只是筛选出价格最低的</param>
        /// <returns></returns>
        [OperationContract]
        IList<Ticket> GetTickets(int productId, DateTime date);

        /// <summary>
        /// 获取产品的机票行程段
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [OperationContract]
        TicketSegment GetTicketSegment(int productId);

        /// <summary>
        /// 搜索产品
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IList<Product> Search(SearchCondition searchCondition,
            Pager pager);

        /// <summary>
        /// 搜索机票
        /// </summary>
        /// <param name="sc"></param>
        /// <returns></returns>
        [OperationContract]
        FlightsSegment SearchFlightsPlans(FlightsPlanSearchCondition sc);

        [OperationContract]
        HotelSegment GetHotelSegment(int productId);

        [OperationContract]
        HotelSegment GetDelayHotelSegment(int productId, int delayDays);

        [OperationContract]
        IList<FlightsSegment> GetFlightsSegments(int productId);

        [OperationContract]
        GroundServiceSegment GetGroundServiceSegment(int productId);
    }
}