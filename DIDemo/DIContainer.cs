namespace DIDemo
{
    public class DIContainer
    {
        private static DIContainer _instance = null;
        private static readonly object padlock = new object();
        private DIContainer() { }
        //public static DIContainer GenerateContainer()
        //{
        //    if (_instance != null)
        //        return _instance;

        //    else
        //    {
        //        var newContainer = new DIContainer();
        //        _instance = newContainer;
        //        return _instance;
        //    }
        //}
        public static DIContainer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DIContainer();
                        }
                    }
                }
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
