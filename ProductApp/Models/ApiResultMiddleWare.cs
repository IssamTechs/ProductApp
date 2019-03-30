using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Reporting.API.Models
{
    public class ApiResultMiddleWare : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var succeeded = context.HttpContext.Response.StatusCode == (int)HttpStatusCode.OK;
            var status = succeeded ? "Succeeded" : "Failed";
            var errorCode = succeeded ? 0 : context.HttpContext.Response.StatusCode;
            switch (context.Result)
            {
                case ObjectResult objectResult:
                    context.Result = new ObjectResult(new
                    {
                        data = objectResult.Value,
                        status = objectResult.StatusCode == (int)HttpStatusCode.OK ? "Succeeded" : "Failed",
                        errorCode = objectResult.StatusCode == (int)HttpStatusCode.OK ? 0 : objectResult.StatusCode,
                        errorMessage = objectResult.StatusCode == (int)HttpStatusCode.OK ? null : objectResult.Value
                    });
                    break;

                case EmptyResult emptyResult:
                    context.Result = new ObjectResult(new
                    {
                        data = "",
                        status,
                        errorCode,
                        errorMessage = (object)null,
                    });
                    break;

                case ContentResult contentResult:
                    context.Result = new ObjectResult(new
                    {
                        data = contentResult.Content,
                        status = contentResult.StatusCode == (int)HttpStatusCode.OK ? "Succeeded" : "Failed",
                        errorCode = contentResult.StatusCode == (int)HttpStatusCode.OK ? 0 : contentResult.StatusCode,
                        errorMessage = contentResult.StatusCode == (int)HttpStatusCode.OK ? null : contentResult.Content
                    });
                    break;

                case StatusCodeResult statusCodeResult:
                    context.Result = new ObjectResult(new
                    {
                        data = statusCodeResult.StatusCode,
                        status = statusCodeResult.StatusCode == (int)HttpStatusCode.OK ? "Succeeded" : "Failed",
                        errorCode = statusCodeResult.StatusCode == (int)HttpStatusCode.OK ? 0 : statusCodeResult.StatusCode,
                        errorMessage = statusCodeResult.StatusCode == (int)HttpStatusCode.OK ? null : statusCodeResult.StatusCode.ToString()
                    });
                    break;

                default:
                    context.Result = new ObjectResult(new
                    {
                        data = context.Result,
                        status,
                        errorCode,
                        errorMessage = succeeded ? null : context.Result
                    });
                    break;
            }
            base.OnResultExecuting(context);
        }
    }
}