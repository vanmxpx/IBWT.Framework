using System;

namespace IBWT.Framework.Abstractions
{
    public interface IBotServiceProvider : IServiceProvider, IDisposable
    {
        IBotServiceProvider CreateScope();
    }
}
