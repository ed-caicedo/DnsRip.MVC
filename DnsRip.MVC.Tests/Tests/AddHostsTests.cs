using System;
using DnsRip.MVC.Utilities;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DnsRip.MVC.Tests.Tests
{
    [TestFixture]
    public class AddHostsTests
    {
        [Test]
        public void ShouldAddAddtionalHostsFromRoot1()
        {
            var expected = new[]
            {
                "www.google.com",
                "m.google.com",
                "blog.google.com",
                "ftp.google.com",
                "imap.google.com",
                "pop.google.com",
                "smtp.google.com",
                "mail.google.com",
                "webmail.google.com"
            };

            var addHosts = new AdditionalHosts();
            var hosts = addHosts.Find("google.com");

            Console.WriteLine(JsonConvert.SerializeObject(hosts));

            Assert.That(hosts, Is.EquivalentTo(expected));
        }

        [Test]
        public void ShouldAddAddtionalHostsFromRoot2()
        {
            var expected = new[]
            {
                "www.google.co",
                "m.google.co",
                "blog.google.co",
                "ftp.google.co",
                "imap.google.co",
                "pop.google.co",
                "smtp.google.co",
                "mail.google.co",
                "webmail.google.co"
            };

            var addHosts = new AdditionalHosts();
            var hosts = addHosts.Find("google.co");

            Console.WriteLine(JsonConvert.SerializeObject(hosts));

            Assert.That(hosts, Is.EquivalentTo(expected));
        }

        [Test]
        public void ShouldAddAddtionalHostsAndRootFromSubdomain()
        {
            var expected = new[]
            {
                "google.com",
                "m.google.com",
                "blog.google.com",
                "ftp.google.com",
                "imap.google.com",
                "pop.google.com",
                "smtp.google.com",
                "mail.google.com",
                "webmail.google.com"
            };

            var addHosts = new AdditionalHosts();
            var hosts = addHosts.Find("www.google.com");

            Console.WriteLine(JsonConvert.SerializeObject(hosts));

            Assert.That(hosts, Is.EquivalentTo(expected));
        }
    }
}