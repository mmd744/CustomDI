using DIDemo.TypesToTest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DIDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var container = DIContainer.CreateContainer();
                container.Register<ICountryService, CountryService>(LifeTime.Singleton);
                container.Register<ISomeService, SomeService>(LifeTime.Transient);

                var countryService1 = container.Resolve<ICountryService>();
                Console.WriteLine(countryService1.GetHashCode());
                var countryService2 = container.Resolve<ICountryService>();
                Console.WriteLine(countryService2.GetHashCode());

                var someService1 = container.Resolve<ISomeService>();
                Console.WriteLine(someService1.GetHashCode());
                var someService2 = container.Resolve<ISomeService>();
                Console.WriteLine(someService2.GetHashCode());
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
