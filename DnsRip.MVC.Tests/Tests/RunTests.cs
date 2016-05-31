using DnsRip.MVC.Requests;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Text;
using DnsRip.MVC.Responses;
using NUnit.Framework.Constraints;

namespace DnsRip.MVC.Tests.Tests
{
    [TestFixture]
    public class RunTests
    {
        private RunRequest _request;

        [OneTimeSetUp]
        public void GetReponses()
        {
            _request = new RunRequest
            {
                Domains = new[] { "google.com", "www.google.com", "www.google.com", "www.yahoo.com", "www.yahoo.com", "www.yahoo.com", "www.google.com", "google.com", "www.google.com", "invalid" },
                Types = new[] { "A", "A", "CNAME", "CNAME", "CNAME", "A", "CNAME", "CNAME", "CNAME", "invalid" },
                Server = "8.8.8.8"
            };

        }

        [Test]
        public void CountResponses()
        {
            var resolverFactory = new ResolverFactory();
            var responseFactory = new RawRunResponseFactory(resolverFactory);
            var response = responseFactory.Create(_request).ToList();

            Console.Write(JsonConvert.SerializeObject(response, Formatting.Indented));

            Assert.That(response.Count, Is.EqualTo(10));
            Assert.That(response.Where(r => r.IsValid).Select(r => r).Count(), Is.EqualTo(9));
            Assert.That(response.Where(r => !r.IsValid).Select(r => r).Count(), Is.EqualTo(1));
        }

        [Test]
        public void Organize()
        {
            var resolverFactory = new ResolverFactory();
            var rawRunResponseFactory = new RawRunResponseFactory(resolverFactory);
            var runResponseFactory = new RunResponseFactory(rawRunResponseFactory);
            var results = runResponseFactory.Create(_request).ToList();

            Console.Write(JsonConvert.SerializeObject(results, Formatting.Indented));

            Assert.That(results.Where(r => r.Query == "google.com").Select(r => r.Records.Count()).Single(), Is.EqualTo(2));
            Assert.That(results.Where(r => r.Query == "www.google.com").Select(r => r.Records.Count()).Single(), Is.EqualTo(2));
            Assert.That(results.Where(r => r.Query == "www.yahoo.com").Select(r => r.Records.Count()).Single(), Is.EqualTo(1));
            Assert.That(results.Where(r => r.Query == "fd-fp3.wg1.b.yahoo.com").Select(r => r.Records.Count()).Single(), Is.AtLeast(2));
            Assert.That(results.Where(r => r.Query == "invalid").Select(r => r.IsValid).Single(), Is.False);
        }

        [Test]
        public void Flatten()
        {
            var resolverFactory = new ResolverFactory();
            var rawRunResponseFactory = new RawRunResponseFactory(resolverFactory);
            var runResponseFactory = new RunResponseFactory(rawRunResponseFactory);
            var runResponse = runResponseFactory.Create(_request).ToList();

            using (var runCsvResponseFactory = new RunCsvResponseFactory())
            {
                runCsvResponseFactory.Create(runResponse);

                using (var reader = new StreamReader(runCsvResponseFactory.Stream, Encoding.UTF8))
                {
                    var result =  reader.ReadToEnd();

                    Console.Write(result);
                    Assert.That(result.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Length, Is.EqualTo(10));
                }
            }
        }
    }
}