using ClientService.Handlers;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using ServiceFabric.Services.Communication.Client;

namespace ClientService.Controllers
{
    [ServiceRequestActionFilter]
    public class StatefullController : ApiController
    {        
        private static readonly Uri serviceUri = new Uri("fabric:/ServiceFabric.HttpClient.Sample/WebApiStateful");        
        private static readonly HttpCommunicationClientFactory communicationFactory = 
            new HttpCommunicationClientFactory(delegatingHandlers: () => new[] { new MyHandler() });

        private ServicePartitionKey partitionKey = new ServicePartitionKey(long.MinValue);

        // GET api/statefull/{id}
        public async Task<string> Get(int id)
        {
            var partitionClient = new HttpServicePartitionClient(communicationFactory,
                    serviceUri, partitionKey, TargetReplicaSelector.RandomReplica);

            var result = await partitionClient.InvokeWithRetryAsync(
                async c => await c.Http.GetStringAsync($"api/values/{id}"));

            return result;
        }
    }
}
