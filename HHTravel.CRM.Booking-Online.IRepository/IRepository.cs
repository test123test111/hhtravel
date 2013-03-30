using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HHTravel.CRM.Booking_Online.IRepository
{
    public interface IRepository<T> where T : class, new()
	{
        IEnumerable<T> All();

        T Find(int id);
	}
}
