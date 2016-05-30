using DnsRip.Interfaces;
using DnsRip.MVC.Interfaces;
using DnsRip.MVC.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DnsRip.MVC.Responses
{
    public class RawRunResponseFactory : IRawRunResponseFactory
    {
        public RawRunResponseFactory(IResolverFactory resolverFactory)
        {
            _resolverFactory = resolverFactory;
            _responses = new List<RawRunResponse>();

            if (Cache == null)
                Cache = new List<RawRunResponse>();
        }

        public static List<RawRunResponse> Cache;
        public const int CacheTime = 5;

        private readonly IResolverFactory _resolverFactory;
        private readonly List<RawRunResponse> _responses;

        public IEnumerable<RawRunResponse> Create(RunRequest request)
        {
            var resolver = _resolverFactory.Create(request.Server);

            if (!resolver.Validator.IsDomain(request.Server) && !resolver.Validator.IsIp(request.Server))
                return new[]
                {
                    new RawRunResponse
                    {
                        IsValid = false,
                        Error = "Invalid Server"
                    }
                };
            
            for (var i = 0; i < request.Domains.Length; i++)
            {
                var response = new RawRunResponse
                {
                    Query = request.Domains[i],
                    Type = request.Types[i],
                    Server = request.Server,
                    Expires = DateTime.UtcNow.AddMinutes(CacheTime),
                    IsValid = true
                };

                if (!resolver.Validator.IsDomain(request.Domains[i]) && !resolver.Validator.IsIp(request.Domains[i]))
                {
                    response.IsValid = false;
                    response.Error = "Invalid domain or ip";
                }

                if (!resolver.Validator.IsValidType(request.Types[i]))
                {
                    response.IsValid = false;
                    response.Error = "Invalid type";
                }

                if (response.IsValid)
                {
                    var domain = request.Domains[i];
                    var type = request.Types[i];

                    var cached =
                        Cache.Where(
                            c =>
                                c.Query == domain && c.Type == type && c.Server == request.Server &&
                                DateTime.UtcNow < c.Expires).SelectMany(c => c.Response).ToList();

                    if (cached.Any())
                    {
                        response.Response = cached;
                    }
                    else
                    {
                        response.Response = resolver.Resolve(domain, (QueryType)Enum.Parse(typeof(QueryType), type));
                        Cache.Add(response);
                    }
                }

                _responses.Add(response);
            }

            return _responses;
        }
    }
}