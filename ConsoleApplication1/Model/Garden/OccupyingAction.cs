using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ConsoleApplication1
{
    public class OccupyingAction : ICloneable
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
        public double EdgesSizeDegree { get; set; }
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

            Contacts = Distances.Where(x => x.Value == 0 && (int)x.SideType < 4);
        }
        #endregion

        public object Clone()
        {
            return MemberwiseClone();
        }

        /*public int CompareTo(OccupyingAction coa, PlacementNode node)
        {
            //Cave deg
            GetDegrees(coa);
            GetDegrees(this);
            //II
            if (node.GetInteractionScoreCoa(this) == node.GetInteractionScoreCoa(coa))
            {
                if (Math.Abs(coa.CavingDegree - CavingDegree) < 0.01)
                {
                    if (coa.CornerDegree == CornerDegree)
                    {
                        if (coa.EdgesDegree == EdgesDegree)
                        {
                            return Plant.Model[0].CompareTo(coa.Plant.Model[0]);
                        }
                        return EdgesDegree.CompareTo(coa.EdgesDegree); ;
                    }
                    return CornerDegree.CompareTo(coa.CornerDegree); ;
                }
                return CavingDegree.CompareTo(coa.CavingDegree);
            }
            return node.GetInteractionScoreCoa(this).CompareTo(node.GetInteractionScoreCoa(coa));
        }*/

        //placem
        public int CompareTo(OccupyingAction coa, PlacementNode node)
        {
            //Cave deg
            GetDegrees(coa);
            GetDegrees(this);
            
            if (Math.Abs(coa.CavingDegree - CavingDegree) < 0.01)
            {
                if (coa.CornerDegree == CornerDegree)
                {
                    if (coa.EdgesDegree == EdgesDegree)
                    {
                        if (Plant.Model[0] == coa.Plant.Model[0])
                        {
                            return node.GetInteractionScoreCoa(this).CompareTo(node.GetInteractionScoreCoa(coa));
                        }
                        return Plant.Model[0].CompareTo(coa.Plant.Model[0]);
                    }
                    return EdgesDegree.CompareTo(coa.EdgesDegree); ;
                }
                return CornerDegree.CompareTo(coa.CornerDegree); ;
            }
            return CavingDegree.CompareTo(coa.CavingDegree);
        }

        private void GetDegrees(OccupyingAction coa)
        {
            var groups = coa.Contacts.GroupBy(x => x.SideType).Count();

            coa.CavingDegree = groups > 2 ? 1 : 1 - coa.Distances.Min(x => x.Value) / ((coa.Plant.Model[0] * 2 + 1));
            if (groups == 1)
            {
                coa.CornerDegree = 0;
            }
            if (groups == 2)
            {
                coa.CornerDegree = 1;
            }
            else if (groups == 3)
            {
                coa.CornerDegree = 2;
            }
            else
            {
                coa.CornerDegree = 4;
            }
            coa.EdgesDegree = (uint)(coa.Contacts.GroupBy(x => x.Plant).Count() + coa.Contacts.Where(x => x.Plant == null).GroupBy(x => x.SideType).Count());
        }
    }
}
