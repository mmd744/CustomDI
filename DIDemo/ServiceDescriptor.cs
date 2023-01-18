using System;
using System.Collections.Generic;
using System.Text;

namespace DIDemo
{
    public class ServiceDescriptor
    {
        public LifeTime LifeTime { get; }
        public Type Interface { get; }
        public Type Implementation { get; }
        public DateTime RegistrationTime { get; }

        public ServiceDescriptor(
            LifeTime lifeTime, 
            Type interfaceType, 
            Type implementationType)
        {
            LifeTime = lifeTime;
            Interface = interfaceType;
            Implementation = implementationType;
            RegistrationTime = DateTime.Now;
        }
    }
}
