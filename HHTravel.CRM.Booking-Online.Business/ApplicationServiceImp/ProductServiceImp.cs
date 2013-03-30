using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.Model;
using HHTravel.CRM.Booking_Online.IRepository;
using HHTravel.CRM.Booking_Online.Business.IApplicationService;
using System.ServiceModel;
using HHTravel.CRM.Booking_Online.Model.Exceptions;

namespace HHTravel.CRM.Booking_Online.Business.ApplicationServiceImp
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ProductServiceImp : IProductService
    {
        #region repos
        private IProductRepository _repoProduct;

        private IProductRepository RepoProduct
        {
            get
            {
                if (_repoProduct == null)
                    _repoProduct = RepositoryFactory.GetRepository<IProductRepository>();
                return _repoProduct;
            }
        }

        private IDepartureCityRepository _repoDepartureCity;

        public IDepartureCityRepository RepoDepartureCity
        {
            get
            {
                if (_repoDepartureCity == null)
                    _repoDepartureCity = RepositoryFactory.GetRepository<IDepartureCityRepository>();
                return _repoDepartureCity;
            }
        }
        private IDestinationGroupRepository _repoDestinationGroup;

        public IDestinationGroupRepository RepoDestinationGroup
        {
            get
            {
                if (_repoDestinationGroup == null)
                    _repoDestinationGroup = RepositoryFactory.GetRepository<IDestinationGroupRepository>();
                return _repoDestinationGroup;
            }
        }
        private IInterestRepository _repoInterest;

        public IInterestRepository RepoInterest
        {
            get
            {
                if (_repoInterest == null)
                    _repoInterest = RepositoryFactory.GetRepository<IInterestRepository>();
                return _repoInterest;
            }
        }
        private IDepartureMonthRepository _repoDepartureMonth;

        public IDepartureMonthRepository RepoDepartureMonth
        {
            get
            {
                if (_repoDepartureMonth == null)
                    _repoDepartureMonth = RepositoryFactory.GetRepository<IDepartureMonthRepository>();
                return _repoDepartureMonth;
            }
        }
        #endregion

        /// <summary>
        /// 获取产品
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Product GetProduct(int productId)
        {
            var a = this.RepoProduct.Find(productId);
            return a;
        }

        /// <summary>
        /// 获取产品
        /// </summary>
        /// <param name="productNo"></param>
        /// <returns></returns>
        public Product GetProduct(string productNo)
        {
            var a = this.RepoProduct.Find(productNo);
            return a;
        }

        /// <summary>
        /// 根据目的地获取产品
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="regionId"></param>
        /// <returns></returns>
        public IEnumerable<Product> FindByDestination(int groupId, int? regionId,
            Pager pager)
        {
            IEnumerable<Product> a;

            if (regionId.HasValue)
            {
                a = this.RepoProduct.FindByDestinationRegion(regionId.Value,
                        pager);
            }
            else
            {
                a = this.RepoProduct.FindByDestinationGroup(groupId,
                        pager);
            }

            return a;
        }
        /// <summary>
        /// 根据旅行主题获取产品
        /// </summary>
        /// <param name="travelTypeId"></param>
        /// <returns></returns>
        public IEnumerable<Product> FindByInterest(int interestId,
            Pager pager)
        {
            var a = this.RepoProduct.FindByInterest(interestId,
                        pager);
            return a;
        }
        /// <summary>
        /// 根据旅游型态获取产品
        /// </summary>
        /// <param name="travelTypeId"></param>
        /// <returns></returns>
        public IEnumerable<Product> FindByTravelType(int travelTypeId,
            Pager pager)
        {
            var a = this.RepoProduct.FindByTravelType(travelTypeId,
                        pager);
            return a;
        }
        /// <summary>
        /// 根据出发日期范围获取产品
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<Product> FindByDepartureDate(DateTime beginDate, DateTime endDate,
            Pager pager)
        {
            var a = this.RepoProduct.FindByDepartureDate(beginDate, endDate,
                pager);
            return a;
        }

        /// <summary>
        /// 获取所有旅游型态
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TravelType> GetAllTravelTypes()
        {
            var a = Enum.GetValues(typeof(TravelType)).Cast<TravelType>();
            return a;
        }

        /// <summary>
        /// 获取所有出发地
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DepartureCity> GetAllDepartureCitys()
        {
            var a = this.RepoDepartureCity.All();
            return a;
        }

        /// <summary>
        /// 获取产品的目的地分组
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IEnumerable<DestinationGroup> GetDestinationGroups(int productId)
        {
            var a = this.RepoProduct.GetDestinationGroups(productId);
            return a;
        }
        /// <summary>
        /// 获取产品的目的地区域
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IEnumerable<DestinationRegion> GetDestinationRegions(int productId)
        {
            var a = this.RepoProduct.GetDestinationRegions(productId);
            return a;
        }
        /// <summary>
        /// 获取所有目的地所属分组
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DestinationGroup> GetAllDestinationGroups()
        {
            var a = this.RepoDestinationGroup.All();
            return a;
        }

        /// <summary>
        /// 获取所有旅行主题
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Interest> GetAllInterests()
        {
            var a = this.RepoInterest.All();
            return a;
        }

        /// <summary>
        /// 获取所有出发月份
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DepartureMonth> GetAllDepartureMonths()
        {
            int nowYear = DateTime.Now.Year;
            int nowMonth = DateTime.Now.Month;

            var a = this.RepoDepartureMonth.All();
            var b = (from d in a
                     where d.Year > nowYear || (d.Year == nowYear && d.Month >= nowMonth)
                     select d).ToList();
            return b;
        }

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
        public IEnumerable<Product> Search(string departurePlaceName, int? groupId, int? regionId, int? travelTypeId, DateTime? beginDate, DateTime? endDate, int? interestid,
            Pager pager)
        {
            var a = this.RepoProduct.Search(departurePlaceName, groupId, regionId, travelTypeId, beginDate, endDate, interestid,
                pager);
            return a;
        }

        /// <summary>
        /// 获取行程项
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="sort"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public IEnumerable<ScheduleItem> GetScheduleItems(int productId,
            Pager pager)
        {
            var a = this.RepoProduct.GetScheduleItemList(productId, pager);
            return a;
        }

        /// <summary>
        /// 获取指定产品的机票
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="onlyWithMinPrice">true: 同房型只是筛选出价格最低的</param>
        /// <returns></returns>
        public IEnumerable<Ticket> GetTickets(int productId, bool onlyWithMinPrice)
        {
            IEnumerable<Ticket> tickets = GetTickets(productId, null, onlyWithMinPrice);
            return tickets;
        }
        /// <summary>
        /// 获取指定产品的指定日期的机票
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="date"></param>
        /// <param name="onlyWithMinPrice">true: 同房型只是筛选出价格最低的</param>
        /// <returns></returns>
        [OperationContract]
        public IEnumerable<Ticket> GetTickets(int productId, DateTime? date, bool onlyWithMinPrice)
        {
            IEnumerable<Ticket> tickets = this.RepoProduct.GetTickets(productId);
            if (onlyWithMinPrice)
            {
                // 同机票只选取最低价的
                // step1 计算出每个机票的最低价
                var a = (from t in tickets
                         group t by t.TicketId into g
                         from rr in g
                         select new
                         {
                             TicketId = g.Key,
                             MinPrice = g.Min(pr => pr.Price),
                         }).Distinct();
                // step2 筛选出最低价对应的房型(可能有多个)
                tickets = from t in tickets
                          join aa in a
                          on t.TicketId equals aa.TicketId
                          where t.Price == aa.MinPrice
                          orderby t.Price
                          select t;
            }

            if (!tickets.Any())
            {
                throw new DataException("机票信息缺失或无效");
            }
            return tickets;
        }
        /// <summary>
        /// 获取产品的酒店
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IEnumerable<Hotel> GetHotels(int productId)
        {
            IEnumerable<Hotel> a = this.RepoProduct.GetHotels(productId);
            return a;
        }
        /// <summary>
        /// 获取最少成行人数
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public int GetMinPersonNumList(int productId)
        {
            var a = this.RepoProduct.GetMinPersonNumList(productId);
            return a;
        }
        /// <summary>
        /// 获取房型信息
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="onlyWithMinPrice">true: 同房型只是筛选出价格最低的房型</param>
        /// <returns></returns>
        public IEnumerable<RoomClass> GetRoomClasses(int productId, bool onlyWithMinPrice)
        {
            IEnumerable<RoomClass> roomClasses = GetRoomClasses(productId, null, onlyWithMinPrice);
            return roomClasses;
        }

        /// <summary>
        /// 获取指定日期的房型信息
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="date"></param>
        /// <param name="onlyWithMinPrice">true: 同房型只是筛选出价格最低的房型</param>
        /// <returns></returns>
        public IEnumerable<RoomClass> GetRoomClasses(int productId, DateTime? date, bool onlyWithMinPrice)
        {
            // 避免多次查库
            IEnumerable<RoomClass> roomClasses = this.RepoProduct.GetRoomClasses(productId).ToList();

            if (onlyWithMinPrice)
            {
                // 同房型只选取最低价的
                // step1 计算出每个房型的最低价
                var a = (from r in roomClasses
                         where !date.HasValue ||
                            (date.HasValue && date >= r.EffectDate && date <= r.ExpireDate)
                         group r by r.RoomClassId into g
                         from rr in g
                         select new
                         {
                             RoomClassId = g.Key,
                             MinPrice = g.Min(pr => pr.Price),
                         }).Distinct();
                // step2 筛选出最低价对应的房型(可能有多个)
                roomClasses = from r in roomClasses
                              join aa in a
                              on r.RoomClassId equals aa.RoomClassId
                              where r.Price == aa.MinPrice
                              orderby r.Price
                              select r;
            }

            if (!roomClasses.Any())
            {
                throw new DataException("房型信息缺失或无效");
            }

            return roomClasses;
        }
    }
}
