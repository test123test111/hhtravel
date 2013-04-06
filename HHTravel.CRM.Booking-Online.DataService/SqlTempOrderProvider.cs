using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.DataAccess;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using HHTravel.CRM.Booking_Online.DataAccess.Providers.OrderDB;

namespace HHTravel.CRM.Booking_Online.DataService
{
    public class SqlTempOrderProvider : ITempOrderProvider
    {
        OrdersTempProvider _provider = new OrdersTempProvider();

        public void AddOrUpdate(Guid sessionId, string content)
        {
            var tempOrder = _provider.Find(sessionId);
            if (tempOrder != null)
            {
                tempOrder.Content = content;
                tempOrder.CreateTime = DateTime.Now;
                _provider.Update(tempOrder);
            }
            else
            {
                _provider.Insert(new Entity.Orders_Temp
                {
                    SessionId = sessionId,
                    Content = content,
                    CreateTime = DateTime.Now,
                });
            }
        }

        public string GetContent(Guid sessionId)
        {
            string content = null;

            var tempOrder = _provider.Find(sessionId);

            if (tempOrder != null)
            {
                content = tempOrder.Content;
            }

            return content;
        }
    }
}
