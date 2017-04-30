using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ServiceFabric.Services.Communication.Client;

namespace ClientService.Controllers
{
    [ServiceRequestActionFilter]
    public class IocController : ApiController
    {        
        private readonly IHttpServicePartitionClient partitionClient;

        public IocController(IHttpServicePartitionClient partitionClient)
        {
            this.partitionClient = partitionClient;
        }

        // GET api/ioc
        public async Task<IEnumerable<string>> Get()
        {
            var response = await partitionClient.InvokeWithRetryAsync(
                async c => await c.Http.GetAsync("api/values"));

            var result = await response.Content.ReadAsAsync<IEnumerable<string>>();

            return result;
        }                
    }
}
