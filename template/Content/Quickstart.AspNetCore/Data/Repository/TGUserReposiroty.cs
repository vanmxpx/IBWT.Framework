using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Quickstart.AspNetCore.Data.Entities;

namespace Quickstart.AspNetCore.Data.Repository
{
    public class TGUserReposiroty : IDataRepository<TGUser>
    {
        private readonly ILogger<TGUserReposiroty> _logger;
        private readonly ApplicationDbContext _context;

        public TGUserReposiroty(
            ApplicationDbContext context,
            ILogger<TGUserReposiroty> logger)
        {
            _logger = logger;
            _context = context;
        }

        public IEnumerable<TGUser> All()
        { 
            try
            {
                return _context.TGUsers.ToList(); 
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on get All TGUsers.");
                throw;
            }
        } 
        public TGUser Get(long id)
        {
            try
            {
                return _context.TGUsers
                    .FirstOrDefault(e => e.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Get TGUser: {id}.");
                throw;
            }
        }

        public TGUser Add(TGUser entity)
        {
            try
            {
                var value = Get(entity.Id);
                if (value == null)
                {
                    var result = _context.TGUsers.Add(entity);
                    _context.SaveChanges();
                    return result.Entity;
                }
                else
                {
                    var result = Get(entity.Id);
                    return result;
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Add new TGUser: {entity.ToString()}.");
                throw;
            }
        }

        public void Update(TGUser entity)
        {
            try
            {
                _context.TGUsers.Update(entity);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Update TGUser: {entity.ToString()}.");
                throw;
            }
        }

        public void Delete(TGUser entity)
        {
            try
            {
                _context.TGUsers.Remove(entity);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Delete TGUser: {entity.ToString()}.");
                throw;
            }
        }
        public TGUser[] Find(Func<TGUser, bool> predicator)
        {
            try
            {
                return _context.TGUsers.Where(predicator).ToArray();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Find TGUser: {predicator.ToString()}.");
                throw;
            }
        }
    }
}