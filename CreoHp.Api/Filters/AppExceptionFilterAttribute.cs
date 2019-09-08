using System;
using CreoHp.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CreoHp.Api.Filters
{
    public class AppExceptionFilterAttribute : ExceptionFilterAttribute
    {
        readonly ILogger _logger;

        public AppExceptionFilterAttribute(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
            if (!(context.Exception is AppException appException)) return;
            var errorInfo = appException.ErrorInfo;
            context.Result = new ObjectResult(errorInfo) {StatusCode = (int) appException.HttpStatusCode};
            _logger.LogWarning(appException.Message);
        }
    }
}