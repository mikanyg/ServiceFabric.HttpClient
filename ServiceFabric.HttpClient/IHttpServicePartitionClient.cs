using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceFabric.Services.Communication.Client
{
    public interface IHttpServicePartitionClient
    {
        void InvokeWithRetry(Action<HttpCommunicationClient> func, params Type[] doNotRetryExceptionTypes);
        TResult InvokeWithRetry<TResult>(Func<HttpCommunicationClient, TResult> func, params Type[] doNotRetryExceptionTypes);
        Task InvokeWithRetryAsync(Func<HttpCommunicationClient, Task> func, params Type[] doNotRetryExceptionTypes);
        Task InvokeWithRetryAsync(Func<HttpCommunicationClient, Task> func, CancellationToken cancellationToken, params Type[] doNotRetryExceptionTypes);
        Task<TResult> InvokeWithRetryAsync<TResult>(Func<HttpCommunicationClient, Task<TResult>> func, params Type[] doNotRetryExceptionTypes);
        Task<TResult> InvokeWithRetryAsync<TResult>(Func<HttpCommunicationClient, Task<TResult>> func, CancellationToken cancellationToken, params Type[] doNotRetryExceptionTypes);
    }
}
