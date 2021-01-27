using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Quickstart.AspNetCore.Data.Repository
{
    public class BaseRepository<TEntity> : IDataRepository<TEntity> where TEntity : class
    {
        public DbSet<TEntity> Collection { get; }
        protected readonly ILogger _logger;
        protected readonly ApplicationDbContext _context;
        public BaseRepository(
            ApplicationDbContext context,
            ILogger logger
        )
        {
            _context = context;
            _logger = logger;
            Collection = _context.Set<TEntity>();
        }

        public IEnumerable<TEntity> All()
        {
            try
            {
                return Collection.AsNoTracking().ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on get All {typeof(TEntity).FullName}s.");
                throw;
            }
        }

        public TEntity Get(Func<TEntity, bool> expression)
        {
            try
            {
                return Collection
                    .FirstOrDefault(expression);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Get {typeof(TEntity).FullName}: {expression.ToString()}.");
                throw;
            }
        }

        public TEntity Add(TEntity entity)
        {
            try
            {
                var result = Collection.Add(entity);
                _context.SaveChanges();
                return result.Entity;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Add new {typeof(TEntity).FullName}: {entity.ToString()}.");
                throw;
            }
        }

        public TEntity[] Find(Func<TEntity, bool> predicator)
        {
            try
            {
                return Collection.AsNoTracking().Where(predicator).ToArray();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Find {typeof(TEntity).FullName}: {predicator.ToString()}.");
                throw;
            }
        }

        public TEntity Update(TEntity entity)
        {
            try
            {
                entity = Collection.Update(entity).Entity;
                _context.SaveChanges();
                return entity;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Update {typeof(TEntity).FullName}: {entity.ToString()}.");
                throw;
            }
        }

        public void Delete(Func<TEntity, bool> predicator)
        {
            try
            {
                var entities = Find(predicator);
                Collection.RemoveRange(entities);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Delete {typeof(TEntity).FullName}: {predicator.ToString()}.");
                throw;
            }
        }
    }
}