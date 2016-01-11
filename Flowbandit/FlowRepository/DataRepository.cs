using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data.Entity;
using FlowRepository;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace FlowRepository
{
    public class DataRepository : IRepository
    {
        protected FlowCollectionEntities _context;

        public FlowCollectionEntities Context
        {
            get { return _context; }
        }

        #region IDisposable
        bool disposed = false;

        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here. 
                //
                if (_context != null)
                {
                    _context.Dispose();
                }
            }

            // Free any unmanaged objects here. 
            //
            disposed = true;

        }

        #endregion //IDisposable

        public DataRepository()
        {
            _context = new FlowCollectionEntities();
        }


        public IQueryable<T> All<T>() where T : class
        {
            return _context.Set<T>();
        }

        public IDbSet<T> Entities<T>() where T : class
        {
            return _context.Set<T>();
        }

        public void Add<T>(T Entity) where T : class
        {
            Entities<T>().Add(Entity);
        }

        public void Delete<T>(T Entity) where T : class
        {
            Entities<T>().Remove(Entity);
        }

        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorMessage = string.Empty;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}",
                        validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                throw new Exception(errorMessage, dbEx);
            }
        }

        public IQueryable<T> AllIncluding<T>(params Expression<Func<T, object>>[] include) where T : class
        {
            IQueryable<T> RetVal = _context.Set<T>();

            foreach (var i in include)
            {
                RetVal = RetVal.Include(i);
            }

            return RetVal;
        }

        protected ObjectContext _ObjectContext
        {
            get
            {
                var adapter = (IObjectContextAdapter)_context;
                var objectContext = adapter.ObjectContext;
                return adapter.ObjectContext;
            }
        }
        public T FindByID<T>(object key) where T : class
        {

            var set = _ObjectContext.CreateObjectSet<T>().EntitySet;
            var pk = set.ElementType.KeyMembers[0]; // careful here maybe count can be o or more then 0
            EntityKey entityKey = new EntityKey(set.EntityContainer.Name + "." + set.Name, pk.Name, key);
            return (T)_ObjectContext.GetObjectByKey(entityKey);

        }
    }
}