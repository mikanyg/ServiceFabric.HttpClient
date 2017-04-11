using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Fabric;
using System.Net.Http;
using Microsoft.ServiceFabric.Services.Communication.Client;

namespace ServiceFabric.Services.Communication.Client
{
    public class HttpCommunicationClient : ICommunicationClient
    {
        public HttpCommunicationClient(HttpClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            HttpClient = client;
            Properties = new ConcurrentDictionary<string, object>();
        }

        public HttpClient HttpClient { get; }

        /// <summary>
        /// Generic properties bag for the client.
        /// </summary>
        public IDictionary<string, object> Properties { get; }

        ResolvedServiceEndpoint ICommunicationClient.Endpoint { get;set; }

        string ICommunicationClient.ListenerName { get; set; }        

        ResolvedServicePartition ICommunicationClient.ResolvedServicePartition { get; set; }        
    }    
}