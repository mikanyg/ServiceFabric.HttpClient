using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ServiceFabric.Services.Communication.Client;

namespace ClientService.Controllers
{
    [ServiceRequestActionFilter]
    public class StatelessController : ApiController
    {
        private static readonly Uri serviceUri = new Uri("fabric:/ServiceFabric.HttpClient.Sample/WebApi");
        private static readonly HttpCommunicationClientFactory communicationFactory = new HttpCommunicationClientFactory();        

        // GET api/stateless
        public async Task<IEnumerable<string>> Get()
        {
            var partitionClient = new HttpServicePartitionClient(communicationFactory, serviceUri);

            var response = await partitionClient.InvokeWithRetryAsync(
                async c => await c.Http.GetAsync("api/values"));

            var result = await response.Content.ReadAsAsync<IEnumerable<string>>();

            return result;
        }                
    }
}
