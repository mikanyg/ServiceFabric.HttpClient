using Microsoft.ServiceFabric.Services.Client;

namespace ServiceFabric.Services.Communication.Client
{
    public interface IHttpServicePartitionClientFactory
    {
        IHttpServicePartitionClient Create(ServicePartitionKey partitionKey = null);
    }
}