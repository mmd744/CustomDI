using System;
using System.Collections.Generic;
using System.Text;

namespace DIDemo.TypesToTest
{
    #region Some service
    public interface ISomeService
    {
        void SomeMethod();
    }

    public class SomeService : ISomeService
    {
        public void SomeMethod()
        {
            Console.WriteLine("Some method called");
        }
    }
    #endregion

    #region Country service
    public interface ICountryService
    {
        string[] GetAll();
    }

    public class CountryService : ICountryService
    {
        public readonly ISomeService someService;

        public CountryService(SomeService someService)
        {
            this.someService = someService;
        }

        public string[] GetAll() => new string[]
        {
            "Azerbaijan", "USA", "Ukraine", "Germany", "Japan", "China"
        };
    }
    #endregion
}
