namespace SpeedTracerMVC.Web
{
    using System;
    using System.Collections.Generic;

    public class SpeedTracerData
    {
        public SpeedTracerData()
        {
            this.Children = new SpeedTracerData[0];
        }

        public string Section
        {
            get;
            set;
        }

        public DateTime StartTime
        {
            get;
            set;
        }

        public DateTime EndTime
        {
            get;
            set;
        }

        public IEnumerable<SpeedTracerData> Children
        {
            get;
            set;
        }
    }
}
