using System;
using System.Collections.Generic;
using System.Linq;
using Emgu.CV;

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
        public int SoilArea { get; set; }
        public int RootArea { get; set; }
        #endregion

        #region ctor
        public Garden(Mat soilMap)
        {
            SoilMap = soilMap;
            ComputeSoilSizes(soilMap);
        }

        public Garden()
        {
        }
        #endregion

        #region other
        public void ComputeSoilSizes(Mat soilMap)
        {
            //SoilArea = ;
            //RootArea = ;
        }
        #endregion
    }
}
