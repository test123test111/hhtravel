using System;
using System.ServiceModel;
using System.Collections.Generic;
using HHTravel.CRM.Booking_Online.Model;

namespace HHTravel.CRM.Booking_Online.Business.IApplicationService
{
    [ServiceContract]
    public interface IProductService
    {
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
        /// 根据出发日期范围获取产品
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<Product> FindByDepartureDate(DateTime beginDate, DateTime endDate,
            Pager pager);
        /// <summary>
        /// 根据目的地获取产品
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="regionId"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<Product> FindByDestination(int groupId, int? regionId,
            Pager pager);
        /// <summary>
        /// 根据旅行主题获取产品
        /// </summary>
        /// <param name="interestId"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<Product> FindByInterest(int interestId,
            Pager pager);
        /// <summary>
        /// 根据旅游型态获取产品
        /// </summary>
        /// <param name="travelTypeId"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<Product> FindByTravelType(int travelTypeId,
            Pager pager);
        /// <summary>
        /// 获取所有出发月份
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<DepartureMonth> GetAllDepartureMonths();
        /// <summary>
        /// 获取所有出发地
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<DepartureCity> GetAllDepartureCitys();
        /// <summary>
        /// 获取产品的目的地分组
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<DestinationGroup> GetDestinationGroups(int productId);
        /// <summary>
        /// 获取产品的目的地区域
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<DestinationRegion> GetDestinationRegions(int productId);
        /// <summary>
        /// 获取所有目的地所属分组
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<DestinationGroup> GetAllDestinationGroups();
        /// <summary>
        /// 获取所有旅行主题
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<Interest> GetAllInterests();
        /// <summary>
        /// 获取所有旅游型态
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<TravelType> GetAllTravelTypes();
        /// <summary>
        /// 搜索产品
        /// </summary>
        /// <param name="departurePlaceName">出发地</param>
        /// <param name="groupId">目的地所属分组</param>
        /// <param name="regionId">目的地所属区域</param>
        /// <param name="travelTypeId">旅游型态</param>
        /// <param name="beginDate">出发区间开始日期</param>
        /// <param name="endDate">出发区间结束日期</param>
        /// <param name="interestid">旅行主题</param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<Product> Search(string departurePlaceName, int? groupId, int? regionId, int? travelTypeId, DateTime? beginDate, DateTime? endDate, int? interestid,
            Pager pager);
        /// <summary>
        /// 获取指定产品的行程项
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="sort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        [OperationContract]
        IEnumerable<ScheduleItem> GetScheduleItems(int productId, Pager pager);
        /// <summary>
        /// 获取指定产品的机票
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="onlyWithMinPrice">true: 同房型只是筛选出价格最低的</param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<Ticket> GetTickets(int productId, bool onlyWithMinPrice);
        /// <summary>
        /// 获取指定产品的指定日期的机票
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="date"></param>
        /// <param name="onlyWithMinPrice">true: 同房型只是筛选出价格最低的</param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<Ticket> GetTickets(int productId, DateTime? date, bool onlyWithMinPrice);
        /// <summary>
        /// 获取指定产品的酒店
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<Hotel> GetHotels(int productId);
        /// <summary>
        /// 获取指定产品的最少成行人数
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [OperationContract]
        int GetMinPersonNumList(int productId);
        /// <summary>
        /// 获取指定产品的房型信息
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="onlyWithMinPrice">true: 同房型只是筛选出价格最低的</param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<RoomClass> GetRoomClasses(int productId, bool onlyWithMinPrice);

        /// <summary>
        /// 获取指定产品的指定日期的房型信息
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="date"></param>
        /// <param name="onlyWithMinPrice">true: 同房型只是筛选出价格最低的</param>
        /// <returns></returns>
        [OperationContract]
        IEnumerable<RoomClass> GetRoomClasses(int productId, DateTime? date, bool onlyWithMinPrice);
    }
}
