using System;
using System.Collections.Generic;
using HHTravel.DomainModel;
using HHTravel.IRepository;

namespace HHTravel.Repository
{
    public class DepartureMonthRepositoryLocal : RepositoryBase<DepartureMonth>, IDepartureMonthRepository
    {
        public override IEnumerable<DepartureMonth> All()
        {
            List<DepartureMonth> list = new List<DepartureMonth>();
            DateTime begin = DateTime.Now;

            for (int i = 0; i < 12; i++)
            {
                DateTime next = begin.AddMonths(i);
                list.Add(new DepartureMonth
                {
                    Year = next.Year,
                    Month = next.Month,
                    Name = string.Format("{0}-{1}月", next.Year, next.Month)
                });
            }

            return list;
        }

        public override DepartureMonth Find(int id)
        {
            throw new NotImplementedException();
        }
    }
}