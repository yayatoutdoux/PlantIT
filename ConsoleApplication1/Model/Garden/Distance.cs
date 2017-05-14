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
        public Distance(int value, SideType sideType, Plant plant, Point point)
        {
            Value = value;
            SideType = sideType;
            Plant = plant;
            Point = point;
        }

        public Distance(Point point, Plant key, Point border)
        {
            Point = border;
            Plant = null;
            Value = Math.Max(Math.Abs(point.X - border.X) - (key.Model[0] + 1), 0) + Math.Max(Math.Abs(point.Y - border.Y) - (key.Model[0] + 1), 0);
            if (point.X > border.X)//left
            {
                if (point.Y - key.Model[0] > border.Y)//top
                {
                    if (border.X < point.X - key.Model[0])
                    {
                        SideType = SideType.TOPLEFT;
                    }
                    else
                    {
                        SideType = SideType.TOP;
                    }
                }
                else if (point.Y + key.Model[0] < border.Y)//bottom
                {
                    if (border.X < point.X - key.Model[0])
                    {
                        SideType = SideType.BOTTOMLEFT;
                    }
                    else
                    {
                        SideType = SideType.BOTTOM;
                    }
                }
                else//left
                {
                    SideType = SideType.LEFT;
                }
            }
            else//right
            {
                if (point.Y - key.Model[0] > border.Y)//top
                {
                    if (border.X > point.X + key.Model[0])
                    {
                        SideType = SideType.TOPRIGHT;
                    }
                    else
                    {
                        SideType = SideType.TOP;
                    }
                }
                else if (point.Y + key.Model[0] < border.Y)//bottom
                {
                    if (border.X > point.X + key.Model[0])
                    {
                        SideType = SideType.BOTTOMRIGHT;
                    }
                    else
                    {
                        SideType = SideType.BOTTOM;
                    }
                }
                else//left
                {
                    SideType = SideType.RIGHT;
                }
            }
        }
        #endregion
    }
}
