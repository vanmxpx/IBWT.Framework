using System;

namespace IBWT.Framework.Abstractions
{
    public interface IBotBuilder
    {
        IBotBuilder Use(Func<UpdateDelegate, UpdateDelegate> middleware);

        IBotBuilder Use<THandler>()
            where THandler : IUpdateHandler;

        IBotBuilder Use<THandler>(THandler handler)
            where THandler : IUpdateHandler;

        UpdateDelegate Build();
    }
}