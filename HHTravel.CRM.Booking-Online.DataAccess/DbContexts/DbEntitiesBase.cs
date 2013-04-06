using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using EFCachingProvider;
using EFCachingProvider.Caching;
using EFProviderWrapperToolkit;
using EFTracingProvider;
using HHTravel.Base.Common.Framework.Logging;

namespace HHTravel.CRM.Booking_Online.DataAccess
{
    public abstract class DbEntitiesBase : DbContext
    {
        private TextWriter logOutput;

        protected DbEntitiesBase(string connectionString)
            : base(connectionString)
        {
        }

        protected DbEntitiesBase(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
        }

        // http://stackoverflow.com/questions/5400530/validation-failed-for-one-or-more-entities-while-saving-changes-to-sql-server-da
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        // Trace.TraceInformation
                        SysLog.WriteTrace(string.Format("Class: {0}, Property: {1}, Error: {2}",
                                            validationErrors.Entry.Entity.GetType().FullName,
                                            validationError.PropertyName,
                                            validationError.ErrorMessage));
                    }
                }

                throw;
            }
        }

        #region Tracing Extensions

        public event EventHandler<CommandExecutionEventArgs> CommandExecuting
        {
            add { this.TracingConnection.CommandExecuting += value; }
            remove { this.TracingConnection.CommandExecuting -= value; }
        }

        public event EventHandler<CommandExecutionEventArgs> CommandFailed
        {
            add { this.TracingConnection.CommandFailed += value; }
            remove { this.TracingConnection.CommandFailed -= value; }
        }

        public event EventHandler<CommandExecutionEventArgs> CommandFinished
        {
            add { this.TracingConnection.CommandFinished += value; }
            remove { this.TracingConnection.CommandFinished -= value; }
        }

        public TextWriter Log
        {
            get { return this.logOutput; }
            set
            {
                if ((this.logOutput != null) != (value != null))
                {
                    if (value == null)
                    {
                        CommandExecuting -= AppendToLog;
                    }
                    else
                    {
                        CommandExecuting += AppendToLog;
                    }
                }

                this.logOutput = value;
            }
        }

        private EFTracingConnection TracingConnection
        {
            get { return ((IObjectContextAdapter)this).ObjectContext.UnwrapConnection<EFTracingConnection>(); }
        }

        private void AppendToLog(object sender, CommandExecutionEventArgs e)
        {
            if (this.logOutput != null)
            {
                this.logOutput.WriteLine(e.ToTraceString().TrimEnd());
                this.logOutput.WriteLine();
            }
        }

        #endregion Tracing Extensions

        #region Caching Extensions

        public ICache Cache
        {
            get { return CachingConnection.Cache; }
            set { CachingConnection.Cache = value; }
        }

        public CachingPolicy CachingPolicy
        {
            get { return CachingConnection.CachingPolicy; }
            set { CachingConnection.CachingPolicy = value; }
        }

        private EFCachingConnection CachingConnection
        {
            get { return ((IObjectContextAdapter)this).ObjectContext.UnwrapConnection<EFCachingConnection>(); }
        }

        #endregion Caching Extensions
    }
}