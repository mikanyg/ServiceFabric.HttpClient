using Microsoft.ServiceFabric.Services.Communication.Client;

namespace ServiceFabric.Services.Communication.Client
{
    public class CommunicationClientValidatingEventArgs<TCommunicationClient> : CommunicationClientEventArgs<TCommunicationClient>
        where TCommunicationClient : ICommunicationClient
    {
        public bool IsValid { get; set; }
    }
}