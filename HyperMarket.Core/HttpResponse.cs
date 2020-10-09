using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
namespace HyperMarket {
    public class HttpResponse {
        public HttpStatusCode StatusCode { get; set; }
        public Dictionary<String, String> ResponseHeaders { get; set; }
        public Stream ResponseStream { get; set; }
        public Boolean ErrorOccurred { get; set; }
        public HttpResponse() { }
        public HttpResponse(HttpStatusCode statusCode, Dictionary<String, String> responseHeaders, Stream responseStream) {
            StatusCode = statusCode;
            ResponseHeaders = responseHeaders;
            ResponseStream = responseStream;
        }
    }
}