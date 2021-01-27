using Microsoft.Extensions.Logging;
using Quickstart.AspNetCore.Data.Entities;

namespace Quickstart.AspNetCore.Data.Repository
{
    public class OrderRepository : BaseRepository<Order>
    {
        public OrderRepository(
            ApplicationDbContext context,
            Logger<OrderRepository> logger
        ) : base(context, logger)
        { }
    }
}