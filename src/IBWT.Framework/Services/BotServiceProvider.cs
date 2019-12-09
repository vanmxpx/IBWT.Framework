﻿using System;
using IBWT.Framework.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace IBWT.Framework.Services
{
    public class BotServiceProvider : IBotServiceProvider
    {
        private readonly IServiceProvider _container;

        private readonly IServiceScope _scope;

        public BotServiceProvider(IApplicationBuilder app)
        {
            _container = app.ApplicationServices;
        }

        public BotServiceProvider(IServiceScope scope)
        {
            _scope = scope;
        }

        public object GetService(Type serviceType) =>
            _scope != null
            ? _scope.ServiceProvider.GetService(serviceType)
            : _container.GetService(serviceType);

        public IBotServiceProvider CreateScope() =>
            new BotServiceProvider(_container.CreateScope());

        public void Dispose()
        {
            _scope?.Dispose();
        }
    }
}