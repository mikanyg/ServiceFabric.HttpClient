using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;

namespace ServiceFabric.Services.Communication.Client
{
    public class HttpCommunicationClientFactory : CommunicationClientFactoryBase<HttpCommunicationClient>
    {
        private readonly Func<IEnumerable<DelegatingHandler>> delegatingHandlers;

        public HttpCommunicationClientFactory(
            IServicePartitionResolver resolver = null,
            IEnumerable<IExceptionHandler> exceptionHandlers = null,
            Func<IEnumerable<DelegatingHandler>> delegatingHandlers = null)
            : base(resolver, CreateExceptionHandlers(exceptionHandlers))
        {
            this.delegatingHandlers = delegatingHandlers;            
        }

        /// <summary>
        /// Event handler that is fired when the Communication client has been created.
        /// </summary>
        public event EventHandler<CommunicationClientEventArgs<HttpCommunicationClient>> ClientCreated;

        /// <summary>
        /// Event handler that is fired when the Communication client is being validated. 
        /// Acts as an extension point to determine whether a cached communication client is still valid.
        /// </summary>
        public event EventHandler<CommunicationClientValidatingEventArgs<HttpCommunicationClient>> ClientValidating;

        protected override void AbortClient(HttpCommunicationClient client)
        {            
            // HTTP clients don't hold persistent connections, so no action is taken.
        }

        protected override Task<HttpCommunicationClient> CreateClientAsync(string endpoint, CancellationToken cancellationToken)
        {
            TraceMessage($"Creating {typeof(HttpCommunicationClient)} with internal service endpoint located at '{endpoint}'.");
            
            var baseUri = new Uri(endpoint.TrimEnd('/') + "/");
            var handlers = delegatingHandlers?.Invoke()?.ToArray() ?? new DelegatingHandler[0];

            var client = HttpClientFactory.Create(handlers);
            client.BaseAddress = baseUri;

            var communicationClient = new HttpCommunicationClient(client);
            ClientCreated?.Invoke(this, new CommunicationClientEventArgs<HttpCommunicationClient> {Client = communicationClient});

            return Task.FromResult(communicationClient);
        }

        protected override bool ValidateClient(HttpCommunicationClient client)
        {
            // HTTP clients don't hold persistent connections, so no validation needs to be done.
            return OnValidateClient(client);
        }

        protected override bool ValidateClient(string endpoint, HttpCommunicationClient client)
        {            
            // HTTP clients don't hold persistent connections, so no validation needs to be done.
            return OnValidateClient(client);
        }

        private bool OnValidateClient(HttpCommunicationClient client)
        {
            if (ClientValidating == null) return true;

            var args = new CommunicationClientValidatingEventArgs<HttpCommunicationClient>
            {
                Client = client,
                IsValid = true
            };

            ClientValidating(this, args);

            return args.IsValid;
        }

        private static IEnumerable<IExceptionHandler> CreateExceptionHandlers(IEnumerable<IExceptionHandler> userDefinedHandlers)
        {
            var list = new List<IExceptionHandler>();

            if (userDefinedHandlers != null)
            {
                list.AddRange(userDefinedHandlers);
            }

            return list.Union(new[] {new HttpRequestExceptionHandler()}); 
        }

        private static void TraceMessage(string message)
        {
            Trace.TraceInformation($"ServiceFabric.HttpClient, {nameof(HttpCommunicationClientFactory)}: {message}");
        }
    }
}
