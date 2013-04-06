using System.Linq;
using HHTravel.DataAccess.DbContexts;
using HHTravel.DataAccess.HardCode;
using HHTravel.Entity;

namespace HHTravel.DataAccess.Providers
{
    public class PropertiesProvider : ProductDbProviderBase<Property>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public PropertiesProvider()
            : base(DbContextFactory.Create<ProductDbEntities>())
        {
        }

        public PropertiesProvider(ProductDbEntities productDbContext)
            : base(productDbContext)
        {
        }

        public override IQueryable<Property> All()
        {
            var a = from p in base.All()
                    where p.IsValid.HasValue && p.IsValid.Value
                    select p;
            return a;
        }

        public IQueryable<Property> FindBy(PropertyPath parentPropertyPath)
        {
            string parentPath = ((int)parentPropertyPath).ToString();
            var query = this.FindBy(parentPath);
            return query;
        }

        protected IQueryable<Property> FindBy(string parentPath)
        {
            var a = from p in this.All()
                    where p.ParentPropertyPath == parentPath
                    select p;
            return a;
        }

        public override Property Find(params object[] keyValues)
        {
            var a = from p in this.All()
                    where p.PropertyId == (int)keyValues[0]
                    select p;
            var b = a.SingleOrDefault();
            return b;
        }
    }
}