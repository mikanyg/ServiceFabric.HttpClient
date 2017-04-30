using Microsoft.ServiceFabric.Services.Communication.Client;

namespace ServiceFabric.Services.Communication.Client
{
    public class HttpCommunicationClientValidatingEventArgs : CommunicationClientEventArgs<HttpCommunicationClient>
    {
        public bool IsValid { get; set; }
    }
}