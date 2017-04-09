using System;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;

namespace ServiceFabric.Services.Communication.Client
{
    public class HttpServicePartitionClientFactory : IHttpServicePartitionClientFactory
    {
        private readonly ICommunicationClientFactory<HttpCommunicationClient> communicationClientFactory;
        private readonly Uri serviceUri;
        private readonly TargetReplicaSelector targetReplicaSelector;
        private readonly string listenerName;
        private readonly OperationRetrySettings retrySettings;

        public HttpServicePartitionClientFactory(
            ICommunicationClientFactory<HttpCommunicationClient> communicationClientFactory, 
            Uri serviceUri, 
            TargetReplicaSelector targetReplicaSelector = TargetReplicaSelector.Default,
            string listenerName = null, 
            OperationRetrySettings retrySettings = null)
        {
            this.communicationClientFactory = communicationClientFactory;
            this.serviceUri = serviceUri;
            this.targetReplicaSelector = targetReplicaSelector;
            this.listenerName = listenerName;
            this.retrySettings = retrySettings;
        }

        public IHttpServicePartitionClient Create(ServicePartitionKey partitionKey = null)
        {
            return new HttpServicePartitionClient(communicationClientFactory, serviceUri, partitionKey,
                targetReplicaSelector, listenerName, retrySettings);
        }
    }
}