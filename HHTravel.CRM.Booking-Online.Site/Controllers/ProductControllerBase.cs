using System;
using System.Collections.Generic;
using System.Linq;
using HHTravel.CRM.Booking_Online.DomainModel;
using HHTravel.CRM.Booking_Online.Infrastructure;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;
using HHTravel.CRM.Booking_Online.Site.Models;

namespace HHTravel.CRM.Booking_Online.Site.Controllers
{
    public class ProductControllerBase : ControllerBase
    {
        #region Build TopNavModel

        // 按栏目提供缺省的TopNavModel
        protected void BuildDefaultTopNavModel(DepartureCity? departureCity, SiteColumn? siteColumn)
        {
            switch (siteColumn)
            {
                case SiteColumn.目的地:
                case SiteColumn.旅行主题:
                case SiteColumn.出发月份:
                    ViewBag.TopNavModel = new TopNavModel(
                        SiteColumnService.GetTopImage(SiteColumn.出发月份),
                        departureCity,
                        new List<BreadcrumbModel>{
                            new BreadcrumbModel(siteColumn.Value, null)
                        });
                    break;

                default:
                    ViewBag.TopNavModel = new TopNavModel(SiteColumnService.GetTopImage(SiteColumn.出发月份));
                    break;
            }
        }

        // 按出发月份搜索时
        protected void BuildDepartureMonthTopNavModelAndMeta(DepartureCity? departureCity, DateTime beginDate)
        {
            string text = beginDate.ToString("yyyy-MM月");
            ViewBag.TopNavModel = new TopNavModel(
                               SiteColumnService.GetTopImage(SiteColumn.出发月份),
                               departureCity,
                               new List<BreadcrumbModel> {
                                  new BreadcrumbModel(SiteColumn.出发月份, Url.Action("DepartMonth", "Home")),
                                  new BreadcrumbModel(AlterLastBreadcrumbText(text), null),
                               });

            ViewBag.Title = string.Format("{0}份出发 | 鸿鹄逸遊 HHtravel", text);

            var meta = SiteColumnService.GetHtmlMeta(SiteColumn.出发月份);
            if (meta != null)
            {
                ViewBag.Keywords = meta.Keywords;
                ViewBag.Description = meta.Description;
            }
        }

        // 按目的地搜索时
        protected void BuildDestionationTopNavModelAndMeta(DepartureCity? departureCity,
    int? groupId, int? regionId, Product product)
        {
            var group = groupId.HasValue ?
                ProductService.GetAllDestinationGroups().FirstOrDefault(g => g.GroupId == groupId) : null;
            var region = (regionId.HasValue && group != null) ?
                group.RegionList.FirstOrDefault(r => r.RegionId == regionId.Value) : null;
            BuildDestionationTopNavModel(departureCity, group, region, product);

            var propId = regionId ?? groupId;
            if (propId.HasValue)
            {
                var meta = HtmlMetaService.GetPropertyHtmlMeta(propId.Value) ?? HtmlMetaService.GetPropertyHtmlMeta(groupId.Value);
                if (meta != null)
                {
                    ViewBag.Title = meta.Title;
                    ViewBag.Keywords = meta.Keywords;
                    ViewBag.Description = meta.Description;
                }
            }
        }

        // 按主题搜索时
        protected void BuildInterestTopNavModelAndMeta(DepartureCity? departureCity, int? interestId)
        {
            if (!interestId.HasValue)
            {
                BuildDefaultTopNavModel(departureCity, SiteColumn.旅行主题);
                return;
            }
            else
            {
                var meta = HtmlMetaService.GetPropertyHtmlMeta(interestId.Value);
                if (meta != null)
                {
                    ViewBag.Title = meta.Title;
                    ViewBag.Keywords = meta.Keywords;
                    ViewBag.Description = meta.Description;
                }
            }

            Interest interest = ProductService.GetAllInterests().SingleOrDefault(i => i.InterestId == interestId);

            if (interest == null)
            {
                BuildDefaultTopNavModel(departureCity, SiteColumn.旅行主题);
                return;
            }

            var interestTopImage = ImageService.GetTopImage(interest.InterestId);
            ViewBag.TopNavModel = new TopNavModel(
                    interestTopImage,
                    departureCity,
                    new List<BreadcrumbModel> {
                            new BreadcrumbModel(SiteColumn.旅行主题, Url.Action("Interest", "Home")),
                            new BreadcrumbModel(AlterLastBreadcrumbText(interest.Name), null),
                        });
        }

        // 展示产品详情页时
        protected void BuildProductTopNavModelAndMeta(DepartureCity? departureCity, Product product)
        {
            DestinationGroup group = null;
            DestinationRegion region = null;
            if (product != null)
            {
                group = product.DestinationGroupList.FirstOrDefault();
                region = product.DestinationRegionList.FirstOrDefault();
                // !容错
                if (region == null && group != null)
                {
                    group.RegionList.FirstOrDefault();
                }
            }

            BuildDestionationTopNavModel(departureCity, group, region, product);

            // SEO
            if (product != null)
            {
                List<string> keywords = new List<string>();
                if (region != null)
                {
                    keywords.Add(region.Name);
                }
                keywords.AddRange(product.InterestList.Select(i => i.Name));
                var hotels = ProductService.GetHotels(product.ProductId);
                if (hotels.Any())
                {
                    keywords.AddRange(hotels.Select(h => h.HotelName));
                }

                // todo: 性能优化; DRY: 与HtmlExtension.cs
                string desp = product.Feature.Replace("<br/>", " ", StringComparison.OrdinalIgnoreCase);
                desp = desp.Replace("<br>", " ", StringComparison.OrdinalIgnoreCase);
                if (desp.Length > 100)
                {
                    desp = desp.Substring(0, 100);
                }

                HtmlMeta meta = new HtmlMeta
                {
                    Title = string.Format("{0} | 鸿鹄逸遊 HHtravel", product.ProductName),
                    Keywords = string.Join<string>(",", keywords),
                    Description = desp
                };

                ViewBag.Title = meta.Title;
                ViewBag.Keywords = meta.Keywords;
                ViewBag.Description = meta.Description;
            }
        }

        #endregion Build TopNavModel

        private static string AlterLastBreadcrumbText(string text)
        {
            if (!text.Contains("高端"))
                return string.Format("{0}高端旅游", text);
            return text;
        }

        // 按目的地搜索时
        private void BuildDestionationTopNavModel(DepartureCity? departureCity,
            DestinationGroup group, DestinationRegion region, Product product)
        {
            if (product == null && group == null && region == null)
            {
                BuildDefaultTopNavModel(departureCity, SiteColumn.目的地);
                return;
            }

            var groupTopImage = (group != null) ? ImageService.GetTopImage(group.GroupId) : null;
            var regionTopImage = (region != null) ? (ImageService.GetTopImage(region.RegionId) ?? groupTopImage) : groupTopImage;
            var productTopImage = (product != null) ? (ImageService.GetProductTopImage(product.ProductId) ?? regionTopImage) : regionTopImage;

            var topNavModel = new TopNavModel(
                productTopImage,
                departureCity,
                new List<BreadcrumbModel>{
                    new BreadcrumbModel(SiteColumn.目的地, Url.Action("Destination", "Home")),
                });

            if (group != null)
            {
                var groupName = (region == null && product == null) ? AlterLastBreadcrumbText(group.Name) : group.Name;
                topNavModel.Breadcrumbs.Add(new BreadcrumbModel(groupName, Url.Action("FindBy", "Product",
                    new { propertyType = 1, propertyValue = group.GroupId })));
            }
            if (region != null)
            {
                var regionName = (product == null) ? AlterLastBreadcrumbText(region.Name) : region.Name;
                topNavModel.Breadcrumbs.Add(new BreadcrumbModel(regionName, Url.Action("FindBy", "Product",
                     new { propertyType = 1, propertyValue = group.GroupId, childValue = region.RegionId })));
            }
            if (product != null)
            {
                topNavModel.Breadcrumbs.Add(new BreadcrumbModel(product.ProductName, null));
            }

            ViewBag.TopNavModel = topNavModel;
        }
    }
}