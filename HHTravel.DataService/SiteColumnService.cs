﻿using System;
using System.Linq;
using HHTravel.DataAccess;
using HHTravel.DataAccess.DbContexts;
using HHTravel.DataAccess.HardCode;
using HHTravel.DataAccess.Providers;
using HHTravel.Infrastructure.Crosscutting;

namespace HHTravel.DataService
{
    public class SiteColumnService
    {
        public static HtmlMeta GetHTMLMeta(SiteColumn siteColumn)
        {
            string columnName = Enum.GetName(typeof(SiteColumn), siteColumn);
            HtmlMeta meta = HtmlMetaService.GetPropertyHTMLMeta(columnName, PropertyPath.网站栏目);
            return meta;
        }

        public static ImageInfo GetTopImage(SiteColumn siteColumn)
        {
            string columnName = Enum.GetName(typeof(SiteColumn), siteColumn);
            using (var cxt = DbContextFactory.Create<ProductDbEntities>())
            using (PicturesProvider picsProvider = new PicturesProvider(cxt))
            using (PropertiesProvider propsProvider = new PropertiesProvider(cxt))
            {
                var topImage = (from p in picsProvider.All()
                                join prop in propsProvider.All()
                                on p.ObjectId equals prop.PropertyId
                                where prop.PropertyName == columnName
                                   && p.Location == PictureLocation.营销图
                                   && p.ObjectType == PictureObjectType.属性
                                select new ImageInfo
                                {
                                    Title = p.Title,
                                    Url = p.URL,
                                }).SingleOrDefault();

                return topImage;
            }
        }
    }
}