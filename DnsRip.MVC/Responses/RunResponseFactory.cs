using DnsRip.MVC.Requests;
using System;
using System.Collections.Generic;

namespace DnsRip.MVC.Responses
{
    public class RunResponseFactory
    {
        public RunResponseFactory()
        {
            _responses = new List<RunResponse>();
        }

        private readonly List<RunResponse> _responses;

        public IEnumerable<RunResponse> Create(RunRequest request)
        {
            for (var i = 0; i < request.Domains.Length; i++)
            {
                var resolver = new Resolver(new[] { request.Server });
                var response = new RunResponse
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
                    response.Response = resolver.Resolve(request.Domains[i],
                        (QueryType)Enum.Parse(typeof(QueryType), request.Types[i]));
                }

                _responses.Add(response);
            }

            return _responses;
        }
    }
}