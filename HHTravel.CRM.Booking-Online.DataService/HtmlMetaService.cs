using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using HHTravel.CRM.Booking_Online.DataAccess;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using HHTravel.CRM.Booking_Online.DataAccess.HardCode;
using HHTravel.CRM.Booking_Online.DataAccess.Providers;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace HHTravel.CRM.Booking_Online.DataService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class HtmlMetaService
    {
        public static HtmlMeta GetPropertyHTMLMeta(int propertyId)
        {
            var meta = Get(SEOObjectType.属性, propertyId);
            return meta;
        }

        internal static HtmlMeta GetPropertyHTMLMeta(string propertyName, PropertyPath propertyPath)
        {
            HtmlMeta meta = new HtmlMeta();
            using (PropertiesProvider tbl = new PropertiesProvider())
            {
                var propId = (from p in tbl.FindBy(propertyPath)
                              where p.PropertyName == propertyName
                              select p.PropertyId).FirstOrDefault();
                if (propId != 0)
                {
                    meta = GetPropertyHTMLMeta(propId);
                }
            }
            return meta;
        }

        private static HtmlMeta Get(SEOObjectType seoObjectType, int objectId)
        {
            HtmlMeta meta;
            string objectType = Enum.GetName(typeof(SEOObjectType), seoObjectType);
            var list = LoadAll();
            meta = (from m in list
                    where m.ObjectType == objectType
                    && m.ObjectId == objectId
                    select m).FirstOrDefault();
            return meta;
        }

        private static List<HtmlMeta> LoadAll()
        {
            using (var cxt = DbContextFactory.Create<ProductDbEntities>())
            {
                var query = (from s in cxt.SEO
                             select new HtmlMeta
                             {
                                 ObjectId = s.SEOObjectId,
                                 ObjectType = s.SEOObjectType,
                                 Title = s.SEOTitle,
                                 Keywords = s.SEOKeyword,
                                 Description = s.SEODescription,
                             });
                return query.ToList();
            }
        }
    }
}