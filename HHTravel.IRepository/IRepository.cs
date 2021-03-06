﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.DomainModel;

namespace HHTravel.IRepository
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot, new()
    {
        IEnumerable<T> All();

        T Find(int id);
    }
}