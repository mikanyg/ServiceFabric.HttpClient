using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Client;

namespace ServiceFabric.Services.Communication.Client
{
    public class HttpRequestExceptionHandler : IExceptionHandler
    {
        private const string SfCustomHeader = "X-ServiceFabric";
        private const string Status404HeaderValue = "ResourceNotFound";

        public bool TryHandleException(ExceptionInformation exceptionInformation, OperationRetrySettings retrySettings, out ExceptionHandlingResult result)
        {
            TraceMessage("Determining exception handling strategy.");

            var ex = exceptionInformation.Exception as HttpRequestException;

            //TODO: Implement proper exception handling strategy
            
            //if (ex?.Response != null)
            //{
            //    TraceMessage($"Response status code is '{ex.Response.StatusCode}'");

            //    switch (ex.Response.StatusCode)
            //    {
            //        case HttpStatusCode.NotFound:
            //            if (HasResourceNotFoundHeader(ex.Response))
            //            {
            //                TraceMessage($"'{SfCustomHeader}:{Status404HeaderValue}' is present in response header, indicating a RESTfull 404 response.");
            //                break;
            //            }
            //            TraceMessage($"'{SfCustomHeader}:{Status404HeaderValue}' is not present in response header, indicating a service has been moved.");
            //            result = new ExceptionHandlingRetryResult(ex, false, retrySettings, retrySettings.DefaultMaxRetryCount);
            //            return true;
            //        case HttpStatusCode.ServiceUnavailable:
            //            result = new ExceptionHandlingRetryResult(ex, false, retrySettings, retrySettings.DefaultMaxRetryCount);
            //            return true;
            //        default:
            //            break;
            //    }
            //}

            TraceMessage("No exception handling strategy was applied.");

            result = null;
            return false;
        }

        //private bool HasResourceNotFoundHeader(HttpResponseMessageWrapper response)
        //{
        //    return response.Headers.ContainsKey(SfCustomHeader) &&
        //           response.Headers[SfCustomHeader].Contains(Status404HeaderValue);
        //}

        private static void TraceMessage(string message)
        {
            Trace.TraceInformation($"ServiceFabric.HttpClient, {nameof(HttpRequestExceptionHandler)}: {message}");
        }
    }
}
