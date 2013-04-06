using System;
using System.Collections.Generic;
using System.Linq;
using HHTravel.DomainModel;
using HHTravel.Infrastructure;
using HHTravel.Infrastructure.Exceptions;
using HHTravel.IRepository;

namespace HHTravel.DomainService
{
    public class ProductManager
    {
        public IEnumerable<RoomClass> GetRoomClasses(int productId, DateTime? date)
        {
            // 避免多次查库
            IEnumerable<RoomClass> roomClasses = null;
            RepositoryCaller.Call<IProductRepository>((repo) =>
            {
                roomClasses = repo.GetRoomClasses(productId).ToList();
            });

            Func<PriceDate, bool> condition = dd =>
                                  !date.HasValue
                                  || (date.HasValue && date == dd.Date);

            roomClasses = from rc in roomClasses
                          group rc by rc into gRoomClass
                          let rc = gRoomClass.Key
                          where rc.PriceDateList.Any(condition)  // 过滤掉无价格的房型
                          select rc;

            if (!roomClasses.Any())
            {
                ExceptionSwitcher<DataInvalidException>.TryThrow(GlobalConfig.IsProductEnvironment,
                    new DataInvalidException("房型信息缺失或无效"));
            }

            return roomClasses;
        }

        /// <summary>
        /// 获取指定产品的指定日期的机票
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public IEnumerable<Ticket> GetTickets(int productId, DateTime? date)
        {
            IEnumerable<Ticket> tickets = null;
            RepositoryCaller.Call<IProductRepository>((repo) =>
            {
                tickets = repo.GetTickets(productId);
            });

            Func<PriceDate, bool> condition = dd =>
                                  !date.HasValue
                                  || (date.HasValue && date == dd.Date);

            tickets = from t in tickets
                      group t by t into gTicket
                      let t = gTicket.Key

                      // 除TravelType3外，机票都无价格 todo: 修改注释
                      where !t.PriceDateList.Any() || t.PriceDateList.Any(condition) // 过滤掉有价格但指定日期无价格的机票
                      select t;

            if (!tickets.Any())
            {
                ExceptionSwitcher<DataInvalidException>.TryThrow(GlobalConfig.IsProductEnvironment,
                    new DataInvalidException("机票信息缺失或无效"));
            }

            return tickets;
        }
    }
}