using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using IBWT.Framework.Abstractions;

namespace Quickstart.AspNetCore.Handlers
{
    public class ExceptionHandler : IUpdateHandler
    {
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            var u = context.Update;

            try
            {
                await next(context, cancellationToken);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
                Console.ResetColor();
                _logger.LogError(e, "An error occured in handling update {0}.", u.Id);
            }
        }
    }
}