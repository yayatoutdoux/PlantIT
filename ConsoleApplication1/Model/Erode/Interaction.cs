using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace ConsoleApplication1
{
    public class Interaction
    {
        #region properties
        public int Type { get; set; }
        public bool IsGive { get; set; }
        public int?[] DistanceFunction { get; set; }
        public int MaxInteraction { get; set; }

        #endregion

        #region ctor
        public Interaction(int type, bool isGive, int?[] distanceFunction, int maxInteraction)
        {
            Type = type;
            IsGive = isGive;
            DistanceFunction = distanceFunction;
            MaxInteraction = maxInteraction;
        }
    }
    #endregion
}
