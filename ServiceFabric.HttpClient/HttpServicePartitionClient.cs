using System;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;

namespace ServiceFabric.Services.Communication.Client
{
    public class HttpServicePartitionClient : ServicePartitionClient<HttpCommunicationClient>, IHttpServicePartitionClient
    {
        public HttpServicePartitionClient(
            ICommunicationClientFactory<HttpCommunicationClient> communicationClientFactory, 
            Uri serviceUri,
            ServicePartitionKey partitionKey = null, 
            TargetReplicaSelector targetReplicaSelector = TargetReplicaSelector.Default, 
            string listenerName = null, 
            OperationRetrySettings retrySettings = null)
            : base(communicationClientFactory, serviceUri, partitionKey, targetReplicaSelector, listenerName, retrySettings)
        {
            if (communicationClientFactory == null) throw new ArgumentNullException(nameof(communicationClientFactory));
            if (serviceUri == null) throw new ArgumentNullException(nameof(serviceUri));
        }
    }
}
