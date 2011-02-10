namespace SpeedTracer
{
    using System;
    using System.Web.Mvc;

    public class TracingAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.Controller is SpeedTracerController)
            {
                return;
            }

            base.OnActionExecuting(filterContext);

            var id = Guid.NewGuid().ToString("n");
            filterContext.HttpContext.Response.AppendHeader("X-TraceUrl", "/speedtracer?id=" + id);
            filterContext.HttpContext.Items["TraceId"] = id;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }
    }
}