namespace SpeedTracerMVC.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Diagnostics;
    using System.Text.RegularExpressions;

    public class Tracer : IDisposable
    {
        private Tracer parent;
        private DateTime startTime;
        private DateTime endTime;
        private string section;
        private List<SpeedTracerData> childData = new List<SpeedTracerData>();
        private string className;
        private string methodName;
        private string lineNumber;

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

            try
            {
                var stackTrace = Environment.StackTrace.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                var regex = new Regex(@"^\s*at\s+(.*)\s+in\s+(.*)\s(\d+)$");

                var rawStack = stackTrace[4];
                var match = regex.Match(rawStack);

                this.methodName = match.Groups[1].Value;
                this.className = match.Groups[2].Value;
                this.lineNumber = match.Groups[3].Value;
            }
            catch
            {
            }
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

            if (this.parent != null)
            {
                this.parent.SaveChildData(this.section, this.startTime, this.endTime, childData);

                Current = this.parent;
            }
            else
            {
                var data = new SpeedTracerData { Section = section, StartTime = startTime, EndTime = endTime, Children = childData, ClassName = this.className, MethodName = this.methodName, LineNumber = this.lineNumber };

                HttpContext.Current.Application["Trace:" + HttpContext.Current.Items["TraceId"]] = data;
            }
        }

        private void SaveChildData(string section, DateTime startTime, DateTime endTime, IEnumerable<SpeedTracerData> childData)
        {
            this.childData.Add(new SpeedTracerData { Section = section, StartTime = startTime, EndTime = endTime, Children = childData, ClassName = this.className, MethodName = this.methodName, LineNumber = this.lineNumber });
        }
    }
}