using System;
using System.Collections.Generic;
using System.Linq;

namespace DIDemo
{
    internal static class ServiceCollection
    {
        internal static List<object> Singletons { get; set; } = new List<object>();
        internal static List<DiRegistry> DiRegistries { get; set; } = new List<DiRegistry>();

        /// <summary>
        /// T is interface or a class implementing that interface.
        /// </summary>
        public static T GetService<T>()
        {
            DiRegistry diRegistry = null;

            if (typeof(T).IsInterface)
            {
                diRegistry = DiRegistries.LastOrDefault(x => x.Interface == typeof(T));
            }
            else if (typeof(T).IsClass)
            {
                diRegistry = DiRegistries.SingleOrDefault(x => x.Implementation == typeof(T));
            }

            var type = diRegistry.Implementation;

            var existingObj = Singletons.FirstOrDefault(s => s.GetType() == type);
            if (diRegistry.LifeTime == LifeTime.Singleton)
            {
                if (existingObj != null)
                {
                    return (T)existingObj;
                }
                else
                {
                    var newSingleton = Activator.CreateInstance(type);
                    Singletons.Add(newSingleton);
                    return (T)newSingleton;
                }
            }
            
            var obj = Activator.CreateInstance(type);
            return (T)obj;
        }
    }

    internal enum LifeTime
    {
        Singleton,
        Transient
    }

    internal class DiRegistry
    {
        public Type Interface { get; set; }
        public Type Implementation { get; set; }
        public LifeTime LifeTime { get; set; }
    }
}
