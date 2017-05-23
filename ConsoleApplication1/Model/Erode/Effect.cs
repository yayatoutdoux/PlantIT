using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace ConsoleApplication1
{
    public class Effect
    {
        #region properties
        public Plant Target { get; set; }
        public Point Point { get; set; }
        public int? Value { get; set; }

        #endregion

        #region ctor
        public Effect(Plant target, Point point, int? value)
        {
            Target = target;
            Point = point;
            Value = value;
        }
    }
    #endregion
}
