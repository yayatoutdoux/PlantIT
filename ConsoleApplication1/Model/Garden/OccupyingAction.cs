using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace ConsoleApplication1
{
    public class OccupyingAction
    {
        #region properties

        public List<Distance> Distances { get; set; }

        #endregion

        #region ctor
        public OccupyingAction()
        {
            
        }

        public OccupyingAction(Point point, Plant key, PlacementNode placementNode)
        {
            Distances = new List<Distance>();
            foreach (var border in placementNode.Borders)
            {
                Distances.Add(new Distance(point, key, border));
            }
        }
        #endregion
    }
}
