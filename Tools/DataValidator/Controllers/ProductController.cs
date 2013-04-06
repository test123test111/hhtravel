using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DataValidator.Models;
using HHTravel.CRM.Booking_Online.ApplicationService;
using HHTravel.CRM.Booking_Online.ApplicationService.ApplicationServiceImp;
using HHTravel.CRM.Booking_Online.DomainModel;
using HHTravel.CRM.Booking_Online.Infrastructure;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;
using HHTravel.CRM.Booking_Online.Infrastructure.Exceptions;
using HHTravel.CRM.Booking_Online.Infrastructure.Web.Mvc;

namespace DataValidator.Controllers
{
    public class ProductController : Controller
    {
        private IProductService _productService;


        public ProductController()
        {
            DbConnectionStringProvider.Instance = new MockDbConnectionStringProvider("LOCAL");
            SearchBoxModel.DepartureCitys = SelectListFactory.Create<DepartureCity>(ProductService.GetAllDepartureCitys(), "CityCode", "CityName", true);
            SearchBoxModel.DestinationGroups = SelectListFactory.Create<DestinationGroup>(ProductService.GetAllDestinationGroups(), "GroupId", "Name", true);
            SearchBoxModel.TravelTypes = SelectListFactory.Create<TravelType, short>(ProductService.GetAllTravelTypes(), true);
            SearchBoxModel.DaysSections = SelectListFactory.Create<DaysSection, int>(true);
            SearchBoxModel.Interests = SelectListFactory.Create<Interest>(ProductService.GetAllInterests(), "InterestId", "Name", true);
            SearchBoxModel.DbServers = DbServerSelectListFactory.Create();
        }

        protected IProductService ProductService
        {
            get
            {
                if (_productService == null)
                {
                    _productService = new ProductServiceImp();
                }
                return _productService;
            }
        }

        //
        // GET: /Product/

        public ActionResult Index()
        {
            ProductListModel plm = new ProductListModel();

            return View(plm);
        }

        public ActionResult Search(string departureCity,
            int? destinationGroup, int? destinationRegion,
            int? travelType, int? interest,
            DateTime? beginDate, DateTime? endDate,
            int? daysSection,
            string dbServer,
            int? sort, bool? descending, int? pageIndex)
        {
            ProductListModel plm = new ProductListModel();

            DepartureCity? city = DepartureCity.Parse(departureCity);
            int? minDays;
            int? maxDays;
            DaysSection? dsec = (daysSection.HasValue) ? (DaysSection)daysSection : (DaysSection?)null;
            dsec.ParseRange(out minDays, out maxDays);

            // !HARDCODE
            Func<string> getDomainBase = () =>
            {
                switch (dbServer)
                {
                    case "LOCAL":
                        return "http://local.hhtravel.com";
                    case "DEV":
                        return "http://hhtravel.dev.sh.ctriptravel.com/HHTravel.CRM.Booking.Online";
                    case "TEST":
                        return "http://hhtravel.test.sh.ctriptravel.com/HHTravel.CRM.Booking.Online";
                    case "PROD TEST":
                        return "http://test.hhtravel.com";
                    case "PROD":
                        return "http://www.hhtravel.com";
                    default:
                        throw new ArgumentOutOfRangeException("dbServer");
                }
            };
            plm.DomainBase = getDomainBase();
            DbConnectionStringProvider.Instance = new MockDbConnectionStringProvider(dbServer);

            var searchCondition = new SearchCondition(departureCity, destinationGroup, destinationRegion,
                travelType, interest, beginDate, endDate, daysSection, null);
            var pager = new Pager(sort, descending, pageIndex, ProductListModel.PageSize);
            var plist = ProductService.Search(searchCondition,
                pager);
            foreach (var p in plist)
            {
                var pm = new ProductModel
                {
                    ProductId = p.ProductId,
                    ProductNo = p.ProductNo,
                    ProductName = p.ProductName,
                    MinPrice = p.MinPrice,
                    Days = p.Days,
                    AdvanceDays = p.AdvanceDays,
                    SetOffDateList = p.SetOffDateList,
                    DepartureDateList = p.DepartureDateList,
                    TravelType = p.TravelType,
                    InterestList = p.InterestList,

                    //ScheduleItemList = ProductService.GetScheduleItems(p.ProductId, null),
                };
                try
                {
                    pm.HotelList = ProductService.GetHotels(p.ProductId);
                }
                catch (DataInvalidException ex)
                {
                    pm.Errors += ex.Message;
                }
                try
                {
                    pm.RoomClassList = ProductService.GetRoomClasses(p.ProductId);
                }
                catch (DataInvalidException ex)
                {
                    pm.Errors += ex.Message;
                }
                try
                {
                    pm.TicketList = ProductService.GetTickets(p.ProductId);
                }
                catch (DataInvalidException ex)
                {
                    pm.Errors += ex.Message;
                }
                plm.ProductList.Add(pm);
            }

            return View("Index", plm);
        }
    }
}