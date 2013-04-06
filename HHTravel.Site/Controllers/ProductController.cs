using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using HHTravel.DomainModel;
using HHTravel.Infrastructure;
using HHTravel.Infrastructure.Crosscutting;
using HHTravel.Site.Models;

namespace HHTravel.Site.Controllers
{
    public class ProductController : ProductControllerBase
    {
        /// <summary>
        /// 按分类搜索
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="propertyValue"></param>
        /// <param name="childValue"></param>
        /// <param name="travelType"></param>
        /// <param name="sort"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult FindBy(int? propertyType, int? propertyValue, int? childValue,
            int? travelType, string departureCity,
            int? sort, bool? descending, int? pageIndex)
        {
            IEnumerable<Product> plist = new List<Product>();
            var pager = new Pager(sort, descending, pageIndex, 5);

            DepartureCity? city = DepartureCity.Parse(departureCity);
            CookieManager.DepartureCity = city;

            if (!propertyType.HasValue)
            {
                BuildDefaultTopNavModel(city, null);
            }
            else
            {
                if (propertyType == 1)  // 按目的地
                {
                    int? groupId = propertyValue;
                    int? regionId = childValue;
                    BuildDestionationTopNavModelAndMeta(city, groupId, regionId, null);

                    SearchCondition sc = new SearchCondition
                    {
                        DepartureCity = city,
                        DestinationGroup = groupId,
                        DestinationRegion = regionId,
                        TravelType = travelType,
                    };
                    plist = ProductService.Search(sc, pager);
                }
                else if (propertyType == 2) // 按旅行主题
                {
                    int? interestId = propertyValue;
                    BuildInterestTopNavModelAndMeta(city, interestId);

                    SearchCondition sc = new SearchCondition
                    {
                        DepartureCity = city,
                        TravelType = travelType,
                        Interest = interestId,
                    };
                    plist = ProductService.Search(sc, pager);
                }
                else if (propertyType == 3) // 按出发月份
                {
                    string inputDateString = string.Format("{0}-{1}-{2}", propertyValue, (childValue.HasValue) ? childValue.Value : 1, 1);
                    DateTime beginDate;
                    if (!DateTime.TryParse(inputDateString, out beginDate))
                    {
                        BuildDefaultTopNavModel(city, SiteColumn.出发月份);
                    }
                    else
                    {
                        BuildDepartureMonthTopNavModelAndMeta(city, beginDate);

                        DateTime endDate = beginDate.AddMonths(1);
                        SearchCondition sc = new SearchCondition
                        {
                            DepartureCity = city,
                            TravelType = travelType,
                            BeginDate = beginDate,
                            EndDate = endDate,
                        };
                        plist = ProductService.Search(sc, pager);
                    }
                }
                else
                {
                    BuildDefaultTopNavModel(city, null);
                }
            }

            #region Build View Model

            ProductListModel model = new ProductListModel
            {
                ProductModelList = (from p in plist
                                    select new ProductModel(p)).ToList(),
                TravelType = travelType,
                DepartureCity = city,
                Sort = sort,
                Descending = pager.Descending,
                PagerModel = new PagerModel
                {
                    PageIndex = pageIndex ?? 0,
                    PageCount = pager.PageCount,
                    BaseUrl = Url.Action("FindBy", new
                    {
                        departureCity = departureCity,
                        propertyType = propertyType,
                        propertyValue = propertyValue,
                        childValue = childValue,
                        travelType = string.Format("{0}", travelType),
                        pageIndex = string.Format("{0}", pageIndex),
                        sort = string.Format("{0}", sort),
                        descending = descending,
                    }),
                    GetUrl = PrepareUrl4ListBar,
                },
            };

            #endregion Build View Model

            return View("List", model);
        }

        /// <summary>
        /// 无结果页面
        /// </summary>
        /// <returns></returns>
        public ActionResult NoResults()
        {
            BuildDefaultTopNavModel(null, null);
            return View("List", new ProductListModel());
        }

        /// <summary>
        /// 组合搜索
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public ActionResult Search(SearchPostModel post)
        {
            ProductListModel model;
            IList<Product> plist = new List<Product>();

            int? destinationGroupId = post.DestinationGroupId;
            int? destinationRegionId = post.DestinationRegionId;
            DepartureCity? city = DepartureCity.Parse(post.DepartureCity);
            CookieManager.DepartureCity = city;
            var pager = new Pager(post.Sort, post.Descending, post.PageIndex, 5);

            // 环游世界，忽略其他搜索条件，根据目的地查找
            if (post.DestinationGroupId.HasValue && post.DestinationGroupId.Value == 43)
            {
                if (!post.DestinationRegionId.HasValue)
                {
                    return RedirectToAction("FindBy", new { propertyType = 1, propertyValue = 43 });
                }
                else if (post.DestinationRegionId.Value == 94)
                {
                    return RedirectToAction("FindBy", new { propertyType = 1, propertyValue = 43, childValue = 94 });
                }
                else if (post.DestinationRegionId.Value == 95)
                {
                    return RedirectToAction("FindBy", new { propertyType = 1, propertyValue = 43, childValue = 95 });
                }
            }
            else
            {
                SearchCondition sc = new SearchCondition(post.DepartureCity, post.DestinationGroupId, post.DestinationRegionId,
                    post.TravelType, post.Interestid,
                    post.BeginDate, post.EndDate, post.DaysSection, post.ProductName);
                plist = ProductService.Search(sc, pager);
            }

            if (!string.IsNullOrEmpty(post.ProductName) && plist.Count > 0)
            {
                var firstProd = ProductService.GetProduct(plist.First().ProductNo);
                firstProd.ProductName = post.ProductName;

                // 面包屑中显示产品名称，但不显示出发地
                BuildProductTopNavModelAndMeta(city, firstProd);
            }
            else
            {
                BuildDestionationTopNavModelAndMeta(city, post.DestinationGroupId, post.DestinationRegionId, null);
            }

            model = new ProductListModel
            {
                ProductModelList = (from p in plist
                                    select new ProductModel(p)).ToList(),
                TravelType = post.TravelType,
                DepartureCity = city,
                IsShowLinkOfOtherDepartureCity = string.IsNullOrEmpty(post.ProductName),
                Sort = post.Sort,
                Descending = post.Descending,
                PagerModel = new PagerModel
                {
                    PageIndex = post.PageIndex ?? 0,
                    PageCount = pager.PageCount,
                    BaseUrl = Url.Action("Search", new
                    {
                        departureCity = post.DepartureCity,
                        destinationGroupId = post.DestinationGroupId,
                        destinationRegionId = post.DestinationRegionId,
                        beginDate = post.BeginDate,
                        endDate = post.EndDate,
                        interestid = post.Interestid,
                        travelType = post.TravelType,
                        daysSection = post.DaysSection,
                        productName = post.ProductName,
                        pageIndex = post.PageIndex,
                        sort = post.Sort,
                        descending = post.Descending,
                    }),
                    GetUrl = PrepareUrl4ListBar,
                },
            };

            return View("List", model);
        }

        /// <summary>
        /// 生成_ListBar 的各项url
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        private string PrepareUrl4ListBar(string name, string value, string baseUrl)
        {
            string newUrl;

            string patternFormat = @"{0}=(true|false|[\d]+|BJS|SHA|null)?";
            string pattern = string.Format(patternFormat, name);
            var r = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            string pair = string.Format("{0}={1}", name, value);
            if (r.IsMatch(baseUrl))
            {
                newUrl = r.Replace(baseUrl, pair);
            }
            else
            {
                newUrl = baseUrl.Contains("?") ? baseUrl + "&" + pair : baseUrl + "?" + pair;
            }

            #region 刷新分页组件

            patternFormat = @"{0}=(true|false|[\d]+)?&?";

            // 切换旅游型态时，刷新分页组件
            if (string.Equals(name, "travelType", StringComparison.OrdinalIgnoreCase))
            {
                pattern = string.Format(patternFormat, "pageIndex");
                r = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                newUrl = r.Replace(newUrl, "");
            }

            // 切换出发城市时，刷新分页组件
            if (string.Equals(name, "departureCity", StringComparison.OrdinalIgnoreCase))
            {
                pattern = string.Format(patternFormat, "pageIndex");
                r = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                newUrl = r.Replace(newUrl, "");
            }

            // 切换排序项时，刷新分页组件
            if (string.Equals(name, "sort", StringComparison.OrdinalIgnoreCase))
            {
                pattern = string.Format(patternFormat, "pageIndex");
                r = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                newUrl = r.Replace(newUrl, "");
            }

            #endregion 刷新分页组件

            return newUrl;
        }
    }
}