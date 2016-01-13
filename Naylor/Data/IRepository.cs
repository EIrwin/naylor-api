using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Naylor.Data
{
    public interface IRepository
    {
        IQueryable AsQueryable();

        object Save(object entity);

        void Delete(object entity);
    }

    public interface IRepository<T> : IRepository
    {
        new IQueryable<T> AsQueryable();

        T Save(T entity);

        void Delete(T entity);

        void Delete(Expression<Func<T, bool>> query);
    }
}
