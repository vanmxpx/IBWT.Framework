using System;
using System.Collections.Generic;

namespace Quickstart.AspNetCore.Data.Repository
{
    public interface IDataRepository<TEntity>
    {
        IEnumerable<TEntity> All();
        TEntity Get(Func<TEntity, bool> expression);
        TEntity Add(TEntity entity);
        TEntity[] Find(Func<TEntity, bool> predicator);
        TEntity Update(TEntity entity);
        void Delete(Func<TEntity, bool> predicator);
    }
}