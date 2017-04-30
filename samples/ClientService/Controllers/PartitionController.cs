using ClientService.Handlers;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using System;
using System.Web.Http;
using ServiceFabric.Services.Communication.Client;

namespace ClientService.Controllers
{
    [ServiceRequestActionFilter]
    public class PartitionController : ApiController
    {        
        private static readonly Uri serviceUri = new Uri("fabric:/ServiceFabric.HttpClient.Sample/WebApiStateful");        
        private static readonly IHttpServicePartitionClientFactory partitionClientFactory;

        static PartitionController()
        {            
            var communicationFactory = new HttpCommunicationClientFactory(delegatingHandlers: () => new[] { new MyHandler() });
            partitionClientFactory = new HttpServicePartitionClientFactory(communicationFactory, serviceUri, TargetReplicaSelector.RandomReplica);
        }

        // GET api/partition/{id} 
        public string Get(int id)
        {
            var partitionClient = partitionClientFactory.Create(new ServicePartitionKey(id));

            var task = partitionClient.InvokeWithRetry(c => c.Http.GetStringAsync($"api/values/{id}"));

            return task.Result;
        }        
    }
}
