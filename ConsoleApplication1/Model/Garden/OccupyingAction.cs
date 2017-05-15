using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace ConsoleApplication1
{
    public class OccupyingAction : ICloneable
    {
        #region properties

        public List<Distance> Distances { get; set; }
        public IEnumerable<Distance> Contacts { get; set; }

        public Plant Plant { get; set; }
        public Point Point { get; set; }
        #endregion

        #region ctor
        public OccupyingAction()
        {
            
        }

        public OccupyingAction(Point point, Plant plant, PlacementNode placementNode)
        {
            Distances = new List<Distance>();
            Plant = plant;
            Point = point;

            //Border distances
            foreach (var border in placementNode.Borders)
            {
                Distances.Add(new Distance(point, plant, border));
            }

            //Plant placed distances
            foreach (var plantPoint in placementNode.Positions)
            {
                Distances.Add(new Distance(plantPoint.Value, plantPoint.Key, point, plant));
            }

            Contacts = Distances.Where(x => x.Value == 0);
        }
        #endregion

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
