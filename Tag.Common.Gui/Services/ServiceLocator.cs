using System;
using System.Collections.Generic;

namespace Tag.Common.Gui.Services
{
    public sealed class ServiceLocator
    {
        private readonly Dictionary<Type, Func<object>> createFuncs = new Dictionary<Type, Func<object>>();
        private readonly Dictionary<Type, Dictionary<string, Lazy<object>>> instances = new Dictionary<Type, Dictionary<string, Lazy<object>>>();

        private static readonly Lazy<ServiceLocator> current = new Lazy<ServiceLocator>(() => new ServiceLocator());
        public static ServiceLocator Current => current.Value;

        private void Register(Type contractType, Func<object> func)
        {
            if (!createFuncs.ContainsKey(contractType))
                createFuncs[contractType] = func;

            if (!instances.ContainsKey(contractType))
                instances[contractType] = new Dictionary<string, Lazy<object>>();
        }

        private void Register(Type contractType, Type serviceType)
        {
            Register(contractType, () => Activator.CreateInstance(serviceType));
        }

        public void Register<TContract, TService>() where TService : new()
        {
            var contractType = typeof(TContract);
            var serviceType = typeof(TService);

            Register(contractType, serviceType);
        }

        public void Register<TContract>(Func<object> createInstanceFunc)
        {
            var contractType = typeof(TContract);

            Register(contractType, createInstanceFunc);
        }

        public void Register<TService>() where TService : new()
        {
            Register<TService, TService>();
        }
        
        public T Get<T>(string key = "") where T : class
        {
            var contractType = typeof(T);

            if (createFuncs.TryGetValue(contractType, out var createFunc) && instances.TryGetValue(contractType, out var dict))
            {
                if (dict.TryGetValue(key, out var lazy))
                {
                    return (T)lazy.Value;
                }
                else
                {
                    dict[key] = new Lazy<object>(createFunc);
                    return (T)dict[key].Value;
                }
            }

            throw new Exception($"Couldn't find service with type of {typeof(T).FullName}");
        }
    }
}