using Microsoft.Extensions.Logging;
using Quickstart.AspNetCore.Data.Entities;

namespace Quickstart.AspNetCore.Data.Repository
{
    public class TGUserReposiroty : BaseRepository<TGUser>
    {
        public TGUserReposiroty(
            ApplicationDbContext context,
            ILogger<TGUserReposiroty> logger) : base(context, logger)
        { }

    }
}