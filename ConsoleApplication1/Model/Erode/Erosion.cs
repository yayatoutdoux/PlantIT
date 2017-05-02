﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace ConsoleApplication1
{
    public class Erosion
    {
        #region properties
        public Mat ErodeMap { get; set; }
        //public List<Point> ErodePoints { get; set; }
        public int Size { get; set; } = 0;
        #endregion

        #region ctor
        public Erosion(Plant plant, Placement placements)
        {
            var erodeMaps = new Mat[Constants.SoilLayerCount];
            //Create erode map
            
            for (var i = 0; i < erodeMaps.Length; i++)
            {
                erodeMaps[i] = new Mat(
                    placements.Placements[0].Size,
                    DepthType.Cv8U,
                    1
                );
                if (i == 0)
                {
                    erodeMaps[i].SetTo(new MCvScalar(255));
                    continue;
                }

                erodeMaps[i].SetTo(new MCvScalar(0));

                for (var j = 0; j < erodeMaps[i].Height; j++)
                {
                    for (var k = 0; k < erodeMaps[i].Width; k++)
                    {
                        if (placements.Placements[i].GetValue(j, k) != 0)
                        {
                            erodeMaps[i].SetValue(j, k, (byte)255);
                        }
                    }
                }
                var structuringElement = 
                    CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(2 * plant.Model[i] + 1, 2 * plant.Model[i] + 1), new Point(plant.Model[i], plant.Model[i]));

                CvInvoke.Erode(erodeMaps[i], erodeMaps[i], structuringElement
                    , new Point(1, 1), 1,
                    BorderType.Constant, new MCvScalar(0));
                CvInvoke.BitwiseAnd(erodeMaps[i], erodeMaps[0], erodeMaps[0]);
                erodeMaps[i].Dispose();
            }
            ErodeMap = erodeMaps[0];
        }
    }
    #endregion
}
