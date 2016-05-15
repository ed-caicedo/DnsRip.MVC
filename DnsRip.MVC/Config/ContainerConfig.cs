using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using DnsRip.MVC.Utilities;

namespace DnsRip.MVC
{
    public class ContainerConfig
    {
        public static IContainer Register()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModule(new LoggingModule());

            return builder.Build();
        }
    }
}