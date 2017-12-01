using Microsoft.ServiceFabric.Services.Communication.Client;

namespace ServiceFabric.HttpClient.Communication.Client
{
    public class HttpCommunicationClientValidatingEventArgs : CommunicationClientEventArgs<HttpCommunicationClient>
    {
        public bool IsValid { get; set; }
    }
}