using System;
using System.Collections.Generic;
using System.Linq;

namespace DIDemo
{
    public class DIContainer
    {
        // private constructor for not allowing object creation from outside using 'new' keyword,
        // client can only use CreateContainer() static method to get or create objects of this class
        private DIContainer() { }

        // instance object which is only controlled by code inside this class
        private static DIContainer instance;

        // collection of registered services
        private List<ServiceDescriptor> services;

        // list for holding singleton objects to provide when requested by client
        private List<object> singletons;

        private void AddToSingletons(object obj)
        {
            if (!singletons.Any(s => s.GetType() == obj.GetType()))
                singletons.Add(obj);
        }


        public static DIContainer CreateContainer()
        {
            if (instance is null)
            {
                instance = new DIContainer
                {
                    services = new List<ServiceDescriptor>(),
                    singletons = new List<object>()
                };
            }

            return instance;
        }

        public void Register<TInterface, TImplementation>(LifeTime lifeTime)
        {
            if (instance is null)
                throw new InvalidOperationException("Create container instance and register some services first.");

            if (services.Any(s => s.Interface == typeof(TInterface) && s.Implementation == typeof(TImplementation)))
                throw new InvalidOperationException($"This dependency [{typeof(TInterface).Name}-{typeof(TImplementation).Name}] is already registered.");

            services.Add(
                new ServiceDescriptor(
                    lifeTime,
                    typeof(TInterface),
                    typeof(TImplementation))
                );
        }

        /// <summary>
        /// Gets an object of the latest registered implementation of specified interface.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public TInterface Resolve<TInterface>()
        {
            if (instance is null)
                throw new InvalidOperationException("Create container instance and register some services first.");

            if (!typeof(TInterface).IsInterface)
                throw new InvalidOperationException("You must specify abstract (interface) type.");

            var serviceDescriptor = services
                .OrderByDescending(sd => sd.RegistrationTime)
                .FirstOrDefault(s => s.Interface == typeof(TInterface)); // take last implementation

            if (serviceDescriptor is null)
                throw new NotImplementedException($"Service of this type is not registered.");

            if (serviceDescriptor.LifeTime == LifeTime.Singleton)
            {
                var obj = singletons.FirstOrDefault(x => x.GetType() == serviceDescriptor.Implementation);
                if (obj != null)
                    return (TInterface)obj;
            }

            return (TInterface)Instantiate(serviceDescriptor.Implementation, serviceDescriptor.LifeTime);
        }

        private object Instantiate(Type type, LifeTime lifeTime)
        {
            // get deafult constructor of type
            var defaultCtor = type.GetConstructors()[0];
            // get parameters
            var defaultParams = defaultCtor.GetParameters();
            // instantiate parameter types recursively
            var parameters = defaultParams.Select(dp =>
            {
                var currentSubServiceDescriptor = services.FirstOrDefault(
                    s => s.Implementation == dp.ParameterType);

                if (currentSubServiceDescriptor is null)
                    throw new InvalidOperationException($"Type is not registered: '{dp.ParameterType.Name}'");

                object newObj = Instantiate(dp.ParameterType, currentSubServiceDescriptor.LifeTime);

                if (currentSubServiceDescriptor.LifeTime == LifeTime.Singleton)
                    AddToSingletons(newObj);

                return newObj;

            }).ToArray();

            // create and return object

            var result = Activator.CreateInstance(type, parameters);
            if (lifeTime == LifeTime.Singleton)
                AddToSingletons(result);

            return result;
        }
    }
}
