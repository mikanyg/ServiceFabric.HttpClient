using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Fabric;
using Microsoft.ServiceFabric.Services.Communication.Client;

namespace ServiceFabric.HttpClient.Communication.Client
{
    public class HttpCommunicationClient : ICommunicationClient
    {
        public HttpCommunicationClient(System.Net.Http.HttpClient client)
        {
            Http = client ?? throw new ArgumentNullException(nameof(client));
            Properties = new ConcurrentDictionary<string, object>();
        }

        public System.Net.Http.HttpClient Http { get; }

        /// <summary>
        /// Generic properties bag for the client.
        /// </summary>
        public IDictionary<string, object> Properties { get; }

        ResolvedServiceEndpoint ICommunicationClient.Endpoint { get;set; }

        string ICommunicationClient.ListenerName { get; set; }        

        ResolvedServicePartition ICommunicationClient.ResolvedServicePartition { get; set; }        
    }    
}