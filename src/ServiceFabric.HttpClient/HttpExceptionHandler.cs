using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using Microsoft.ServiceFabric.Services.Communication.Client;

namespace ServiceFabric.HttpClient.Communication.Client
{
    public class HttpExceptionHandler : IExceptionHandler
    {
        private const string SfCustomHeader = "X-ServiceFabric";
        private const string Status404HeaderValue = "ResourceNotFound";

        public bool TryHandleException(ExceptionInformation exceptionInformation, OperationRetrySettings retrySettings, out ExceptionHandlingResult result)
        {
            TraceMessage("Determining exception handling strategy.");

            if (exceptionInformation.Exception is TimeoutException)
            {
                result = new ExceptionHandlingRetryResult(exceptionInformation.Exception, false, retrySettings, retrySettings.DefaultMaxRetryCount);
                return true;
            }
            else if (exceptionInformation.Exception is ProtocolViolationException)
            {
                result = new ExceptionHandlingThrowResult();
                return true;
            }
            else if (exceptionInformation.Exception is SocketException)
            {
                result = new ExceptionHandlingRetryResult(exceptionInformation.Exception, false, retrySettings, retrySettings.DefaultMaxRetryCount);
                return true;
            }
            else if (exceptionInformation.Exception is HttpRequestException)
            {                
                result = new ExceptionHandlingRetryResult(exceptionInformation.Exception, false, retrySettings, retrySettings.DefaultMaxRetryCount);
                return true;
            }

            WebException we = exceptionInformation.Exception as WebException;

            if (we == null)
            {
                we = exceptionInformation.Exception.InnerException as WebException;
            }

            if (we != null)
            {
                HttpWebResponse errorResponse = we.Response as HttpWebResponse;                

                if (we.Status == WebExceptionStatus.ProtocolError)
                {
                    if (errorResponse.StatusCode == HttpStatusCode.NotFound)
                    {
                        if (HasResourceNotFoundHeader(errorResponse))
                        {
                            TraceMessage($"'{SfCustomHeader}:{Status404HeaderValue}' is present in response header, indicating a RESTfull 404 response.");
                            result = null;
                            return false;
                        }
                        TraceMessage($"'{SfCustomHeader}:{Status404HeaderValue}' is not present in response header, indicating a service has been moved.");
                        result = new ExceptionHandlingRetryResult(exceptionInformation.Exception, false, retrySettings, retrySettings.DefaultMaxRetryCount);
                        return true;
                    }

                    if (errorResponse.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        // The address is correct, but the server processing failed.
                        // This could be due to conflicts when writing the word to the dictionary.
                        // Retry the operation without re-resolving the address.
                        result = new ExceptionHandlingRetryResult(exceptionInformation.Exception, true, retrySettings, retrySettings.DefaultMaxRetryCount);
                        return true;
                    }
                }

                if (we.Status == WebExceptionStatus.Timeout ||
                    we.Status == WebExceptionStatus.RequestCanceled ||
                    we.Status == WebExceptionStatus.ConnectionClosed ||
                    we.Status == WebExceptionStatus.ConnectFailure)
                {
                    result = new ExceptionHandlingRetryResult(exceptionInformation.Exception, false, retrySettings, retrySettings.DefaultMaxRetryCount);
                    return true;
                }
            }

            TraceMessage("No exception handling strategy was applied.");

            result = null;
            return false;
        }

        private bool HasResourceNotFoundHeader(HttpWebResponse response)
        {
            return response.Headers[SfCustomHeader] != null &&
                   response.Headers[SfCustomHeader].Contains(Status404HeaderValue);
        }

        private static void TraceMessage(string message)
        {
            Trace.TraceInformation($"ServiceFabric.HttpClient, {nameof(HttpExceptionHandler)}: {message}");
        }
    }    
}
