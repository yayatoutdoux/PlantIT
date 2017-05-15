﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace ConsoleApplication1
{
    public class OccupyingAction : ICloneable, IComparable
    {
        #region properties

        public List<Distance> Distances { get; set; }
        public IEnumerable<Distance> Contacts { get; set; }

        public Plant Plant { get; set; }
        public Point Point { get; set; }
        public Distance Dmin { get; set; }
        public double CavingDegree { get; set; }
        public uint CornerDegree { get; set; }
        public uint EdgesDegree { get; set; }
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

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            OccupyingAction coa = obj as OccupyingAction;

            //Cave deg
            GetDegrees(coa);
            GetDegrees(this);

            if (Math.Abs(coa.CavingDegree - CavingDegree) < 0.01)
            {
                //Corner deg
                if (coa.CornerDegree == CornerDegree)
                {
                    return Plant.Model[0].CompareTo(coa.Plant.Model[0]);

                }
                return CornerDegree.CompareTo(coa.CornerDegree); ;

            }
            return CavingDegree.CompareTo(coa.CavingDegree);
        }

        private void GetDegrees(OccupyingAction coa)
        {
            var types = coa.Contacts.Select(x => x.SideType);
            var groups = types.GroupBy(x => x);

            coa.CavingDegree = groups.Count() > 2 ? 1 : 1 - coa.Distances.Min(x => x.Value) / ((coa.Plant.Model[0] * 2 + 1) * (coa.Plant.Model[0] * 2 + 1));
            if (groups.Count() == 2)
            {
                coa.CornerDegree = 1;
            }
            else if (groups.Count() == 3)
            {
                coa.CornerDegree = 2;
            }
            else
            {
                coa.CornerDegree = 4;
            }
        }
    }
}
