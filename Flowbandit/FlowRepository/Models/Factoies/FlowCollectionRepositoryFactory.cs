using FlowRepository.Repositories.Models.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Models.Factoies
{
    public static class FlowCollectionRepositoryFactory
    {
        public static T GetRepository<T, TEntityContext>(TEntityContext context)
            where T : DataRepository<TEntityContext>
            where TEntityContext : DbContext, new()
        {
            return Activator.CreateInstance(typeof(T), context) as T;
        }
    }
}
