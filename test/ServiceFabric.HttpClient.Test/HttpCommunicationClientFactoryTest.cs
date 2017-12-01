using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using ServiceFabric.HttpClient.Communication.Client;
using Shouldly;

namespace ServiceFabric.HttpClient.Test
{
    [TestFixture]
    public class HttpCommunicationClientFactoryTest
    {
        class TestableFactory : HttpCommunicationClientFactory
        {
            public TestableFactory(Func<IEnumerable<DelegatingHandler>> delegatingHandlers = null)
                : base(delegatingHandlers: delegatingHandlers) { }

            public new Task<HttpCommunicationClient> CreateClientAsync(string endpoint, CancellationToken cancellationToken)
            {
                return base.CreateClientAsync(endpoint, cancellationToken);
            }

            public new bool ValidateClient(HttpCommunicationClient client)
            {
                return base.ValidateClient(client);
            }

            public new bool ValidateClient(string endpoint, HttpCommunicationClient client)
            {
                return base.ValidateClient(endpoint, client);
            }
        }

        [TestCase("http://localhost/serviceendpoint")]
        [TestCase("http://localhost/serviceendpoint/")]
        [TestCase("http://localhost/serviceendpoint//")]
        [TestCase("http://localhost")]
        [TestCase("http://localhost:80")]
        [TestCase("http://localhost/")]
        [TestCase("http://localhost:80/")]
        public async Task CreateClientAsync_DifferentEndpointFormats_BaseAddressHasSingleTrailingSlash(string endpoint)
        {
            // Arrange
            var sut = new TestableFactory();
            // Act
            var client = await sut.CreateClientAsync(endpoint, CancellationToken.None);
            // Assert
            client.Http.BaseAddress.AbsoluteUri.ShouldEndWith("/");
            client.Http.BaseAddress.AbsoluteUri.ShouldNotEndWith("//");
        }
    }
}
