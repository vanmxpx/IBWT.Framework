using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Quickstart.AspNetCore.Data.Entities;


namespace Quickstart.AspNetCore.Data.Repository
{
    public class OrderRepository : IDataRepository<Order>
    {
        private readonly ILogger<OrderRepository> _logger;
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context, ILogger<OrderRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public IEnumerable<Order> All()
        {
            try
            {
                return _context.Orders.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error on get All Orders.");
                throw;
            }
        }
        public Order Get(long id)
        {
            try
            {
                return _context.Orders
                    .FirstOrDefault(e => e.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Get Order: {id}.");
                throw;
            }
        }

        public Order Add(Order entity)
        {
            try
            {
                var value = Get(entity.Id);
                if (value == null)
                {
                    var result = _context.Orders.Add(entity);
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
                _logger.LogError(e, $"Error on Add new Order: {entity.ToString()}.");
                throw;
            }
        }

        public void Update(Order entity)
        {
            try
            {
                _context.Orders.Update(entity);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Update Order: {entity.ToString()}.");
                throw;
            }
        }

        public void Delete(Order entity)
        {
            try
            {
                _context.Orders.Remove(entity);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Delete Order: {entity.ToString()}.");
                throw;
            }
        }
        public Order[] Find(Func<Order, bool> predicator)
        {
            try
            {
                return _context.Orders.Where(predicator).ToArray();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error on Find Order: {predicator.ToString()}.");
                throw;
            }
        }
    }
}