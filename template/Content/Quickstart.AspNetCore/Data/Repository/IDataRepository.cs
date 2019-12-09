using System;
using System.Collections.Generic;

namespace Quickstart.AspNetCore.Data.Repository
{
    public interface IDataRepository<TEntity>
    {
        IEnumerable<TEntity> All();
        TEntity Get(long id);
        TEntity Add(TEntity entity);
        TEntity[] Find(Func<TEntity, bool> predicator);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}