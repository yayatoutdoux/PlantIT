using System;
using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace ConsoleApplication1
{
    public class Garden
    {
        #region properties
        //General infos
        public Graphic Graphic { get; set; }

        //Other infos
        public GpsPosition GpsPostion { get; set; }

        //Map of the garden
        public Mat SoilMap { get; set; } // 0 rien, 1 plant, 2 racine
        public List<Point> Borders { get; set; }
        #endregion

        #region ctor
        public Garden(Mat soilMap)
        {
            SoilMap = soilMap;
            Borders = new List<Point>();

            var border = new Mat(SoilMap.Size, DepthType.Cv8U, 1);
            soilMap.CopyTo(border);

            var structuringElement =
                CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), new Point(1, 1));

            CvInvoke.Dilate(border, border, structuringElement
                , new Point(1, 1), 1,
                BorderType.Constant, new MCvScalar(0));

            CvInvoke.Subtract(border, SoilMap, border);

            for (var j = 0; j < border.Height; j++)
            {
                for (var k = 0; k < border.Width; k++)
                {
                    if (border.GetValue(j, k) != 0)
                    {
                        Borders.Add(new Point(j, k));
                    }
                }
            }
        }

        public Garden()
        {
        }


        public Garden(Garden garden)
        {
        }
        #endregion
    }
}
