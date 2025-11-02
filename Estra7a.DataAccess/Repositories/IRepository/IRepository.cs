using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Estra7a.DataAccess.Data;

namespace Estra7a.DataAccess.Repositories.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? Filter=null, string? IncludeProp=null, string includeProperties = null);
        T GetById(Expression<Func<T, bool>> Filter, string? IncludeProp = null,bool tracked=false);
        void Remove(T entity);
        void Add(T entity);
        void RemoveRange(IEnumerable<T> entities);
       
    }
}
