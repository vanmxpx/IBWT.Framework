﻿using System;
using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework.Abstractions;

namespace IBWT.Framework.Middleware
{
    internal class UseWhenMiddleware : IUpdateHandler
    {
        private readonly Predicate<IUpdateContext> _predicate;

        private readonly UpdateDelegate _branch;

        public UseWhenMiddleware(Predicate<IUpdateContext> predicate, UpdateDelegate branch)
        {
            _predicate = predicate;
            _branch = branch;
        }

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken = default)
        {
            if (_predicate(context))
            {
                await _branch(context).ConfigureAwait(false);
            }

            await next(context).ConfigureAwait(false);
        }
    }
}