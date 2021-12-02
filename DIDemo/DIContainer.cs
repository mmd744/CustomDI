using System;
using System.Collections.Generic;
using System.Text;

namespace DIDemo
{
    public class DIContainer
    {
        private static DIContainer _instance = null;
        private DIContainer() { }
        public static DIContainer GenerateContainer()
        {
            if (_instance != null)
                return _instance;

            else
            {
                var newContainer = new DIContainer();
                _instance = newContainer;
                return _instance;
            }
        }
        public void RegisterSingleton<TInterface, TImplementation>()
        {
            ServiceCollection.DiRegistries.Add(new DiRegistry
            {
                Interface = typeof(TInterface),
                Implementation = typeof(TImplementation),
                LifeTime = LifeTime.Singleton
            });
        }

        public void RegisterTransient<TInterface, TImplementation>()
        {
            ServiceCollection.DiRegistries.Add(new DiRegistry
            {
                Interface = typeof(TInterface),
                Implementation = typeof(TImplementation),
                LifeTime = LifeTime.Transient
            });
        }
    }
}
