using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public interface IModelBuilder<TViewModel, TEntity>
    {
        TViewModel CreateFrom(TEntity entity);
        TViewModel Rebuild(TViewModel model);
    }
}
