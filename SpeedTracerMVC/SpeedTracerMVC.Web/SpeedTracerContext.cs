namespace SpeedTracerMVC.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Diagnostics;

    public class Tracer : IDisposable
    {
        private Tracer parent;
        private DateTime startTime;
        private DateTime endTime;
        private string section;
        private List<SpeedTracerData> childData = new List<SpeedTracerData>();

        private Tracer()
            : this(null)
        {
        }

        private Tracer(string section)
        {
            this.section = section;

            if (HttpContext.Current.Items["SpeedTracerContext"] == null)
            {
                HttpContext.Current.Items["SpeedTracerContext"] = this;
            }
            else
            {
                this.parent = Current;
                Current = this;
            }

            this.startTime = DateTime.Now;
            Debug.WriteLine(section + ":StartTime:" + this.startTime.ToUnix().ToString());
        }

        public static Tracer Trace(string section)
        {
            return new Tracer(section);
        }

        public static Tracer Current
        {
            get
            {
                var context = default(Tracer);
                if ((context = HttpContext.Current.Items["SpeedTracerContext"] as Tracer) == null)
                {
                    Current = context = new Tracer();
                }

                return context;
            }
            set
            {
                HttpContext.Current.Items["SpeedTracerContext"] = value;
            }
        }


        public void Dispose()
        {
            this.endTime = DateTime.Now;
            Debug.WriteLine(section + ":EndTime:" + this.endTime.ToUnix().ToString());

            if (this.parent != null)
            {
                this.parent.SaveChildData(this.section, this.startTime, this.endTime, childData);

                Current = this.parent;
            }
            else
            {
                var data = new SpeedTracerData
                {
                    Section = this.section,
                    EndTime = this.endTime,
                    StartTime = this.startTime,
                    Children = this.childData
                };

                HttpContext.Current.Application["Trace:" + HttpContext.Current.Items["TraceId"]] = data;
            }
        }

        private void SaveChildData(string section, DateTime startTime, DateTime endTime, IEnumerable<SpeedTracerData> childData)
        {
            this.childData.Add(new SpeedTracerData { Section = section, StartTime = startTime, EndTime = endTime, Children = childData });
        }
    }
}