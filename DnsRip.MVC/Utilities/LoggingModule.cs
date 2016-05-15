using Autofac.Core;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DnsRip.MVC.Utilities
{
    public class LoggingModule : Autofac.Module
    {
        public LoggingModule()
        {
            XmlConfigurator.Configure();
            _loggers = new Dictionary<Type, ILog>();
        }

        private static IDictionary<Type, ILog> _loggers;

        protected override void AttachToComponentRegistration(IComponentRegistry registry, IComponentRegistration registration)
        {
            registration.Preparing += OnComponentPreparing;
        }

        private static void OnComponentPreparing(object sender, PreparingEventArgs e)
        {
            e.Parameters = e.Parameters.Union(
                new[]
                {
                    new ResolvedParameter(
                        (p, i) => p.ParameterType == typeof(ILog),
                        (p, i) => GetLogger(p.Member.DeclaringType)
                        ),
                });
        }

        private static ILog GetLogger(Type type)
        {
            if (!_loggers.ContainsKey(type))
                _loggers.Add(type, LogManager.GetLogger(type));

            return _loggers[type];
        }
    }
}