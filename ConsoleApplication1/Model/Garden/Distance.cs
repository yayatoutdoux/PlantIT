using System;
using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace ConsoleApplication1
{
    public enum SideType
    {
        TOP,
        BOTTOM,
        RIGHT,
        LEFT,
        TOPRIGHT,
        TOPLEFT,
        BOTTOMRIGHT,
        BOTTOMLEFT
    }

    public class Distance
    {
        #region properties
        public int Value { get; set; }
        public SideType SideType { get; set; }
        public Plant Plant { get; set; }
        public Point Point { get; set; }
        #endregion

        #region ctor
        public Distance(Point point, Plant plant, Point border)
        {
            Point = border;
            Plant = null;
            Value = Math.Max(Math.Abs(point.X - border.X) - (plant.Model[0] + 1), 0) + Math.Max(Math.Abs(point.Y - border.Y) - (plant.Model[0] + 1), 0);
            SideType = GetSideType(point, border, plant);
        }

        /*public Distance(OccupyingAction coa, OccupyingAction coaToModify)
        {
            Point = coa.Point;
            Plant = coa.Plant;
            Value = Math.Max(Math.Abs(coaToModify.Point.X - coa.Point.X) - (coa.Plant.Model[0] + 2 + coaToModify.Plant.Model[0]), 0) 
                + Math.Max(Math.Abs(coaToModify.Point.Y - coa.Point.Y) - (coa.Plant.Model[0] + 2 + coaToModify.Plant.Model[0]), 0);
            SideType = GetSideType(coaToModify.Point, coa.Point, coaToModify.Plant);
        }*/

        public Distance(Point positon, Plant plant, Point point, Plant currentPlant)
        {
            Point = positon;
            Plant = plant;
            Value = Math.Max(Math.Abs(Point.X - point.X) - (plant.Model[0] + 2 + Plant.Model[0]), 0)
                + Math.Max(Math.Abs(Point.Y - point.Y) - (plant.Model[0] + 2 + Plant.Model[0]), 0);
            SideType = GetSideType(point, Point, Plant);
        }

        public SideType GetSideType(Point point, Point position, Plant coaPlant)
        {
            if (point.X > position.X)//left
            {
                if (point.Y - coaPlant.Model[0] > position.Y)//top
                {
                    if (position.X < point.X - coaPlant.Model[0])
                    {
                        return SideType.TOPLEFT;
                    }
                    return SideType.TOP;
                    
                }
                if (point.Y + coaPlant.Model[0] < position.Y)//bottom
                {
                    if (position.X < point.X - coaPlant.Model[0])
                    {
                        return SideType.BOTTOMLEFT;
                    }
                    return SideType.BOTTOM;
                    
                }
                return SideType.LEFT;
                
            }

            if (point.Y - coaPlant.Model[0] > position.Y)//top
            {
                if (position.X > point.X + coaPlant.Model[0])
                {
                    return SideType.TOPRIGHT;
                }
                return SideType.TOP;
                
            }
            if (point.Y + coaPlant.Model[0] < position.Y)//bottom
            {
                if (position.X > point.X + coaPlant.Model[0])
                {
                    return SideType.BOTTOMRIGHT;
                }
                return SideType.BOTTOM;
                
            }
            return SideType.RIGHT;
        }
        #endregion
    }
}
