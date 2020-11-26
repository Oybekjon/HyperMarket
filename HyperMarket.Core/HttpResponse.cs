using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
namespace HyperMarket
{
    public class HttpResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public Dictionary<string, string> ResponseHeaders { get; set; }
        public Stream ResponseStream { get; set; }
        public bool ErrorOccurred { get; set; }
        public HttpResponse() { }
        public HttpResponse(HttpStatusCode statusCode, Dictionary<string, string> responseHeaders, Stream responseStream)
        {
            StatusCode = statusCode;
            ResponseHeaders = responseHeaders;
            ResponseStream = responseStream;
        }
    }
}