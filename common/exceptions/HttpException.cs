using System;
using System.Net;

namespace si.birokrat.next.common.exceptions {
    public class HttpException : Exception {
        public HttpException(HttpStatusCode statusCode, string message)
            : base(message) {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
    }
}
