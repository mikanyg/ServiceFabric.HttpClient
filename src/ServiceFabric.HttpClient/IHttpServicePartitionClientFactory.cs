using Microsoft.ServiceFabric.Services.Client;

namespace ServiceFabric.HttpClient.Communication.Client
{
    public interface IHttpServicePartitionClientFactory
    {
        IHttpServicePartitionClient Create(ServicePartitionKey partitionKey = null);
    }
}