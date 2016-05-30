using DnsRip.Interfaces;
using DnsRip.MVC.Interfaces;
using DnsRip.MVC.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using DnsRip.Models;

namespace DnsRip.MVC.Responses
{
    public class RawRunResponseFactory : IRawRunResponseFactory
    {
        public RawRunResponseFactory(IResolverFactory resolverFactory)
        {
            _resolverFactory = resolverFactory;
            _responses = new List<RawRunResponse>();
        }

        private readonly IResolverFactory _resolverFactory;
        private readonly List<RawRunResponse> _responses;

        public IEnumerable<RawRunResponse> Create(RunRequest request)
        {
            var resolver = _resolverFactory.Create(request.Server);

            for (var i = 0; i < request.Domains.Length; i++)
            {
                var response = new RawRunResponse
                {
                    Query = request.Domains[i],
                    Type = request.Types[i],
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
                    var resolved = resolver.Resolve(request.Domains[i], (QueryType)Enum.Parse(typeof(QueryType), request.Types[i]));
                    var validated = new List<ResolveResponse>();

                    foreach (var res in resolved)
                    {
                        if ((resolver.Validator.IsDomain(res.Host) || resolver.Validator.IsIp(res.Host)) &&
                            resolver.Validator.IsValidType(res.Type.ToString()))
                            validated.Add(res);
                    }

                    response.Response = validated;
                }

                _responses.Add(response);
            }

            return _responses;
        }
    }
}