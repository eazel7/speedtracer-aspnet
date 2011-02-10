namespace SpeedTracer
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    public class SpeedTracerController : Controller
    {
        private static object ToPlainObject(SpeedTracerData trace)
        {
            if (trace == null)
            {
                return null;
            }

            var startTime = trace.StartTime.ToUnix() * 1000 + trace.StartTime.Millisecond;
            var endTime = trace.EndTime.ToUnix() * 1000 + trace.EndTime.Millisecond;
            var duration = endTime - startTime;

            return new
            {
                range = new
                {
                    duration = duration,
                    start = startTime,
                    end = endTime + duration
                },
                id = Guid.NewGuid().ToString("n"),
                operation = new
                {
                    sourceCodeLocation = new
                    {
                        className = trace.ClassName,
                        methodName = trace.MethodName,
                        lineNumber = trace.LineNumber
                    },
                    type = "METHOD",
                    label = trace.Section
                },
                children = trace.Children.Select(c => ToPlainObject(c))
            };
        }

        public ActionResult ReadTrace(string id)
        {
            if (this.Request.HttpMethod == "HEAD")
            {
                return new EmptyResult();
            }

            var trace = HttpContext.Application["Trace:" + id] as SpeedTracerData;

            return this.Json(new
            {
                trace = new
                {
                    date = trace.StartTime.ToUnix(),
                    application = "MyApp",
                    range = new
                    {
                        duration = (trace.EndTime - trace.StartTime).TotalMilliseconds,
                        start = trace.StartTime.ToUnix(),
                        end = trace.EndTime.ToUnix()
                    },
                    id = Guid.NewGuid().ToString("n"),
                    frameStack = ToPlainObject(trace)
                }
            },
            JsonRequestBehavior.AllowGet);
        }
    }
}
