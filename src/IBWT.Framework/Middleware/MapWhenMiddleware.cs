using System;
using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework.Abstractions;

namespace IBWT.Framework.Middleware
{
    internal class MapWhenMiddleware : IUpdateHandler
    {
        private readonly Predicate<IUpdateContext> _predicate;

        private readonly UpdateDelegate _branch;

        public MapWhenMiddleware(Predicate<IUpdateContext> predicate, UpdateDelegate branch)
        {
            _predicate = predicate;
            _branch = branch;
        }

        public Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken = default) =>
            _predicate(context) ? _branch(context) : next(context);
    }
}