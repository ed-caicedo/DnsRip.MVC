using System;
using DnsRip.MVC.Interfaces;
using DnsRip.MVC.Requests;
using System.Collections.Generic;
using System.Linq;

namespace DnsRip.MVC.Responses
{
    public class RunResponseFactory : IRunResponseFactory
    {
        public RunResponseFactory(IRawRunResponseFactory rawRunResponseFactory)
        {
            _rawRunResponseFactory = rawRunResponseFactory;
            _results = new List<RunResponse>();
        }
        
        private readonly List<RunResponse> _results;
        private readonly IRawRunResponseFactory _rawRunResponseFactory;

        public IEnumerable<RunResponse> Create(RunRequest request)
        {
            var responses = _rawRunResponseFactory.Create(request);
            
            foreach (var response in responses)
            {
                AddQuery(response.Query, response.IsValid, response.Error);

                if (!response.IsValid)
                    continue;

                foreach (var resp in response.Response)
                {
                    if (response.Query != resp.Host)
                    {
                        AddQuery(resp.Host, true, null);
                        AddRecord(resp.Host, resp.Type, resp.Record);
                    }
                    else
                    {
                        AddRecord(response.Query, resp.Type, resp.Record);
                    }
                }

                if (!response.Response.Any())
                    AddRecord(response.Query, response.Type, "No Response");
            }

            return _results;
        }

        private void AddRecord(string query, QueryType type, string record)
        {
            AddRecord(query, type.ToString(), record);
        }

        private void AddRecord(string query, string type, string record)
        {
            var result = _results.Single(r => r.Query == query);
            var records = result.Records?.ToList() ?? new List<RunRecord>();

            if (records.All(r => r.Type != type || r.Result != record))
            {
                records.Add(new RunRecord
                {
                    Type = type,
                    Result = record
                });

                result.Records = records;
            }
        }

        private void AddQuery(string query, bool isValid, string error)
        {
            if (_results.All(r => r.Query != query))
            {
                _results.Add(new RunResponse
                {
                    Query = query,
                    IsValid = isValid,
                    Error = error
                });
            }
        }
    }
}