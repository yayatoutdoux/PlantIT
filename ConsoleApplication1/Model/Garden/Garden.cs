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
        public List<KeyValuePair<int, Mat>> Model { get; set; }//3D model with plants ????????????????,,,

        //Already placed plants
        public List<Plant> PlantList { get; set; }
        #endregion

        #region ctor
        public Garden(Mat soilMap)
        {
            SoilMap = soilMap;
            ComputeSoilSizes(soilMap);
            Model = new List<KeyValuePair<int, Mat>>();
        }

        public Garden()
        {
        }
        #endregion

        #region other
        internal void SetGardenMaps(int minDim, uint maxDim)
        {
            for (var i = minDim; i <= maxDim; i++)
            {
                Model.Add(new KeyValuePair<int, Mat>(i, SoilMap));
            }
            Model = Model.OrderBy(o => o.Key).ToList();
        }

        public void ComputeSoilSizes(Mat soilMap)
        {

        }

        internal void DrawPlant(Plant plant, Placement placement)
        {
            foreach (var dim in Model)
            {
                var modelInDim = plant.Model.SingleOrDefault(x => x.Key == dim.Key).Value;
                for (var i = 0; i < modelInDim.Cols; i++)
                {
                    for (var j = 0; j < modelInDim.Rows; j++)
                    {
                        if (modelInDim.GetValue(i, j) == (byte) 255)
                            modelInDim.SetValue(i, j, (byte)((255-155) + placement.PlacedPlantCount));
                    }
                }
                modelInDim.CopyTo(dim.Value);
            }

        }
        #endregion
    }
}
