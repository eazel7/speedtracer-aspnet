using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using SpeedTracer;

namespace SpeedTracerMVC.Web.Controllers
{
    public class HomeController : Controller
    {
        [Tracing]
        [AcceptVerbs(HttpVerbs.Head | HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index()
        {
            using (Tracer.Trace("HomeController.Index"))
            {
                using (Tracer.Trace("First sleep of 3 seconds"))
                {
                    Thread.Sleep(3000);
                }

                using (Tracer.Trace("Second sleep of aprox 2 seconds"))
                {
                    using (Tracer.Trace("Inner sleep of 0.5 seconds"))
                    {
                        Thread.Sleep(500);
                    }
                    using (Tracer.Trace("Inner sleep of 1.5 seconds"))
                    {
                        Thread.Sleep(1500);
                    }
                }

                return View();
            }
        }
    }
}
