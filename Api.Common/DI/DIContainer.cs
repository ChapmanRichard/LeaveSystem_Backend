using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Common.DI
{
    public interface IDIContainer
    {
        IServiceProvider Container { get; }
        object GetInstance(Type type);
        TService GetInstance<TService>() where TService : class;
        IEnumerable<object> GetAllInstances(Type serviceType);
        IEnumerable<TService> GetAllInstances<TService>() where TService : class;
    }

    public class ServiceProviderDIContainer : IDIContainer
    {
        private readonly IServiceProvider _provider;

        public ServiceProviderDIContainer(IServiceProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public IServiceProvider Container => _provider;

        public object GetInstance(Type type)
        {
            return _provider.GetRequiredService(type);
        }

        public TService GetInstance<TService>() where TService : class
        {
            return _provider.GetRequiredService<TService>();
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _provider.GetServices(serviceType).Cast<object>();
        }

        public IEnumerable<TService> GetAllInstances<TService>() where TService : class
        {
            return _provider.GetServices<TService>();
        }
    }

    public static class DIContainer
    {
        // 在程序启动时赋值： new ServiceProviderDIContainer(app.Services)
        public static IDIContainer Instance { get; set; }
    }
}
