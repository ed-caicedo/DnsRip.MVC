using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace DnsRip.MVC.Attributes
{
    public class ErrorAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnException(actionExecutedContext);
            actionExecutedContext.Response= new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }
}