using System;
using System.Collections.Generic;

namespace DIDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = DIContainer.Instance;
            container.RegisterTransient<ICountryService, CountryService>();


            var countryService = ServiceCollection.GetService<CountryService>();

            foreach(var country in countryService.GetCountriesList())
            {
                Console.WriteLine(country);
            }
        }
    }

    interface ICountryService
    {
        List<string> GetCountriesList();
    }

    class CountryService : ICountryService
    {
        public List<string> GetCountriesList()
        {
            return new List<string>
            {
                "Azerbaijan", "Russia"
            };
        }
    }
}
