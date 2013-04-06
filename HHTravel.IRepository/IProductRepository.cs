using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.DomainModel;
using HHTravel.Infrastructure.Crosscutting;

namespace HHTravel.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        /// <summary>
        /// 根据产品序列号查找产品
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Product Find(string productNo);

        /// <summary>
        /// 根据出发地查找产品
        /// </summary>
        /// <param name="departureCity"></param>
        /// <returns></returns>
        IEnumerable<Product> FindByDepartureCity(DepartureCity? departureCity,
            Pager pager);

        /// <summary>
        /// 根据出发日期范围查找产品
        /// </summary>
        /// <param name="departureMonthId"></param>
        /// <returns></returns>
        IEnumerable<Product> FindByDepartureDate(DateTime beginDate, DateTime endDate,
            Pager pager);

        /// <summary>
        /// 根据目的地分组查找产品
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        IEnumerable<Product> FindByDestinationGroup(int groupId,
            Pager pager);

        /// <summary>
        /// 根据目的地区域查找产品
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        IEnumerable<Product> FindByDestinationRegion(int regionId,
            Pager pager);

        /// <summary>
        /// 根据旅行主题查找产品
        /// </summary>
        /// <param name="interestId"></param>
        /// <returns></returns>
        IEnumerable<Product> FindByInterest(int interestId,
            Pager pager);

        /// <summary>
        /// 根据产品名称获取产品
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        IEnumerable<Product> FindByProductName(string productName, Pager pager);

        /// <summary>
        /// 根据旅游型态查找产品
        /// </summary>
        /// <param name="travelTypeId"></param>
        /// <returns></returns>
        IEnumerable<Product> FindByTravelType(int travelTypeId,
            Pager pager);

        /// <summary>
        /// 获取所有产品号
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetAllProductNos();

        /// <summary>
        /// 获取产品的酒店
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        IEnumerable<Hotel> GetHotels(int productId);

        /// <summary>
        /// 获取产品的住宿行程段
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        IEnumerable<HotelSegment> GetHotelSegments(int productId);

        /// <summary>
        /// 获取房型信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        IEnumerable<RoomClass> GetRoomClasses(int productId);

        /// <summary>
        /// 获取产品的日程项
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        IEnumerable<ScheduleItem> GetScheduleItems(int productId, Pager pager);

        /// <summary>
        /// 获取产品的机票
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        IEnumerable<Ticket> GetTickets(int productId);

        /// <summary>
        /// 获取产品的机票行程段
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        TicketSegment GetTicketSegment(int productId);

        /// <summary>
        /// 搜索产品
        /// </summary>
        /// <param name="departureCity">出发地</param>
        /// <param name="groupId">目的地所属分组</param>
        /// <param name="regionId">目的地所属区域</param>
        /// <param name="travelTypeId">旅游型态</param>
        /// <param name="beginDate">出发区间开始日期</param>
        /// <param name="endDate">出发区间结束日期</param>
        /// <param name="interestid">旅行主题</param>
        /// <returns></returns>
        IEnumerable<Product> Search(SearchCondition searchCondition,
            Pager pager);

        GroundServiceSegment GetGroundServiceSegment(int productId);

        IEnumerable<FlightsSegment> GetFlightsSegments(int productId);
    }
}