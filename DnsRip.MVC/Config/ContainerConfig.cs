using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using DnsRip.MVC.Utilities;
using System.Reflection;
using DnsRip.Interfaces;
using DnsRip.MVC.Interfaces;
using DnsRip.MVC.Responses;

namespace DnsRip.MVC
{
    public class ContainerConfig
    {
        public static IContainer Register()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<AutofacWebTypesModule>();
            builder.RegisterModule(new LoggingModule());
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();
            builder.RegisterType<ParseResponseFactory>().As<IParseResponseFactory>().InstancePerRequest();
            builder.RegisterType<AdditionalHosts>().As<IAdditionalHosts>().InstancePerRequest();
            builder.RegisterType<Parser>().As<IParser>().InstancePerRequest();
            builder.RegisterType<RunResponseFactory>().As<IRunResponseFactory>().InstancePerRequest();
            builder.RegisterType<RawRunResponseFactory>().As<IRawRunResponseFactory>().InstancePerRequest();
            

            return builder.Build();
        }
    }
}