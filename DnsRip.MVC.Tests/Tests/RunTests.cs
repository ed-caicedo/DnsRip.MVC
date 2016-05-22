using DnsRip.MVC.Requests;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using DnsRip.MVC.Models;
using DnsRip.MVC.Responses;

namespace DnsRip.MVC.Tests.Tests
{
    [TestFixture]
    public class RunTests
    {
        private IEnumerable<RunResponse> _responses;

        [OneTimeSetUp]
        public void GetReponses()
        {
            var request = new RunRequest
            {
                Domains = new[] { "google.com", "www.google.com", "www.google.com", "www.yahoo.com", "www.yahoo.com", "www.yahoo.com", "www.google.com", "google.com", "www.google.com", "invalid" },
                Types = new[] { "A", "A", "CNAME", "CNAME", "CNAME", "A", "CNAME", "CNAME", "CNAME", "invalid" },
                Server = "8.8.8.8"
            };

            var responseFactory = new RunResponseFactory();
            _responses = responseFactory.Create(request);
        }

        [Test]
        public void CountResponses()
        {
            Console.Write(JsonConvert.SerializeObject(_responses, Formatting.Indented));

            Assert.That(_responses.Count, Is.EqualTo(10));
            Assert.That(_responses.Where(r => r.IsValid).Select(r => r).Count(), Is.EqualTo(9));
            Assert.That(_responses.Where(r => !r.IsValid).Select(r => r).Count(), Is.EqualTo(1));
        }

        [Test]
        public void Organize()
        {
            var reponseFactory = new RunResponseViewModelFactory();
            var results = reponseFactory.Create(_responses).ToList();

            Console.Write(JsonConvert.SerializeObject(results, Formatting.Indented));

            Assert.That(results.Where(r => r.Query == "google.com").Select(r => r.Records.Count()).Single(), Is.EqualTo(2));
            Assert.That(results.Where(r => r.Query == "www.google.com").Select(r => r.Records.Count()).Single(), Is.EqualTo(2));
            Assert.That(results.Where(r => r.Query == "www.yahoo.com").Select(r => r.Records.Count()).Single(), Is.EqualTo(1));
            Assert.That(results.Where(r => r.Query == "fd-fp3.wg1.b.yahoo.com").Select(r => r.Records.Count()).Single(), Is.AtLeast(2));
            Assert.That(results.Where(r => r.Query == "invalid").Select(r => r.IsValid).Single(), Is.False);
        }
    }
}