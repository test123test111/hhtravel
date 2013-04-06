﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using EFCachingProvider;
using EFCachingProvider.Caching;
using EFProviderWrapperToolkit;
using EFTracingProvider;
using HHTravel.CRM.Booking_Online.Entity;

namespace HHTravel.CRM.Booking_Online.DataAccess.DbContexts
{
    public partial class OrderDbEntities : DbEntitiesBase
    {
        internal OrderDbEntities()
            : base("name=OrderDbEntities")
        {
        }
    
        internal OrderDbEntities(string connectionString) : base(connectionString)
    	{
    	
    	}
    
    	internal OrderDbEntities(DbConnection existingConnection, bool contextOwnsConnection)
                : base(existingConnection, contextOwnsConnection)
        {
        }
    
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Orders_Product> Orders_Product { get; set; }
        public DbSet<Orders_Passenger> Orders_Passenger { get; set; }
        public DbSet<Orders_Temp> Orders_Temp { get; set; }
    }
}
