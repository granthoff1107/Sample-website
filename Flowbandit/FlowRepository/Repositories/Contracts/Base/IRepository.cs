using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FlowRepository.Repositories.Contracts.Base
{
    public interface IRepository<TContext> : IDisposable
        where TContext : DbContext
    {
        TContext Context { get; }

        IQueryable<T> All<T>() where T : class;

        IQueryable<T> AllIncluding<T>(params Expression<Func<T, object>>[] include) where T : class;

        T FindByID<T>(object Key) where T : class;

        void Add<T>(T Entity) where T : class;

        void Delete<T>(T Entity) where T : class;

        void SaveChanges();

    }
}
 