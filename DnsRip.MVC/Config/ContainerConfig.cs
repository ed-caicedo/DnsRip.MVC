using Autofac;
using Autofac.Integration.Mvc;
using DnsRip.Interfaces;
using DnsRip.MVC.Attributes;
using DnsRip.MVC.Interfaces;
using DnsRip.MVC.Responses;
using DnsRip.MVC.Utilities;
using FileHelpers;
using System.Reflection;
using System.Web.Mvc;

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
            builder.RegisterGeneric(typeof(FileHelperEngine<>)).As(typeof(IFileHelperEngine<>)).InstancePerRequest();
            builder.RegisterType<Parser>().As<IParser>().InstancePerRequest();
            builder.RegisterType<AdditionalHosts>().As<IAdditionalHosts>().InstancePerRequest();
            builder.RegisterType<ResolverFactory>().As<IResolverFactory>().InstancePerRequest();
            builder.RegisterType<RunCsvReponseStream<RunCsvResponse>>().As<IRunCsvReponseStream>().ExternallyOwned()
                .InstancePerRequest();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(t => t.Name.EndsWith("Factory"))
                .AsImplementedInterfaces().InstancePerRequest();

            var container = builder.Build();
            return container;
        }
    }
}