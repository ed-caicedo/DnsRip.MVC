using Autofac;
using Autofac.Integration.Mvc;
using DnsRip.Interfaces;
using DnsRip.MVC.Interfaces;
using DnsRip.MVC.Responses;
using DnsRip.MVC.Utilities;
using System.Reflection;
using System.Web.Mvc;
using DnsRip.MVC.Attributes;
using log4net;

namespace DnsRip.MVC
{
    public class ContainerConfig
    {
        public static IContainer Register()
        {
            var builder = new ContainerBuilder();

            builder.RegisterFilterProvider();
            builder.RegisterModule(new LoggingModule());
            builder.RegisterModule<AutofacWebTypesModule>();
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();
            builder.Register(c => new ErrorAttribute()).AsExceptionFilterFor<Controller>().InstancePerRequest();
            builder.RegisterType<ParseResponseFactory>().As<IParseResponseFactory>().InstancePerRequest();
            builder.RegisterType<AdditionalHosts>().As<IAdditionalHosts>().InstancePerRequest();
            builder.RegisterType<Parser>().As<IParser>().InstancePerRequest();
            builder.RegisterType<RunResponseFactory>().As<IRunResponseFactory>().InstancePerRequest();
            builder.RegisterType<RawRunResponseFactory>().As<IRawRunResponseFactory>().InstancePerRequest();
            builder.RegisterType<ResolverFactory>().As<IResolverFactory>().InstancePerRequest();

            return builder.Build();
        }
    }
}