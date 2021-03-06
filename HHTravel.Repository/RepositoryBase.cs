﻿using System.Collections.Generic;
using HHTravel.DataAccess.DbContexts;
using HHTravel.DomainModel;
using HHTravel.IRepository;

namespace HHTravel.Repository
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : IAggregateRoot, new()
    {
        protected CustomerDbEntities CustomerDbContext { get; set; }

        protected GovDbEntities GovDbContext { get; set; }

        protected OrderDbEntities OrderDbContext { get; set; }

        protected ProductDbEntities ProductDbContext { get; set; }

        public abstract IEnumerable<T> All();

        public void Dispose()
        {
            if (this.ProductDbContext != null)
            {
                this.ProductDbContext.Dispose();
            }

            if (this.CustomerDbContext != null)
            {
                this.CustomerDbContext.Dispose();
            }

            if (this.OrderDbContext != null)
            {
                this.OrderDbContext.Dispose();
            }

            if (this.OrderDbContext != null)
            {
                this.OrderDbContext.Dispose();
            }
        }

        public abstract T Find(int id);
    }
}