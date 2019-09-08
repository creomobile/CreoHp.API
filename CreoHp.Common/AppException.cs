using System;
using System.Net;

namespace CreoHp.Common
{
    public class AppException : Exception
    {
        public ErrorInfo ErrorInfo { get; }

        public HttpStatusCode HttpStatusCode { get; }

        public AppException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
            : this(new ErrorInfo {Message = message}, httpStatusCode)
        {
        }

        public AppException(string message, string code, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
            : this(new ErrorInfo {Message = message, Code = code}, httpStatusCode)
        {
        }

        public AppException(ErrorInfo errorInfo, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
            : base(errorInfo.Message)
        {
            ErrorInfo = errorInfo;
            HttpStatusCode = httpStatusCode;
        }
    }
}