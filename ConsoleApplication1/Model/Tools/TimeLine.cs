using System;
using System.Collections.Generic;

namespace ConsoleApplication1
{
    public class TimeLine
    {
        public IEnumerable<Step> Steps { get; set; }
    }

    public class Step
    {
        public Graphic Graphic { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
    }
}
