
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ConsoleApplication1
{
    class Packing
    {
        #region properties
        public PlantList PlantList { get; set; }
        public Garden Garden { get; set; }

        //All ends of backtrack
        public List<Placement> PossiblePlacements { get; set; } = new List<Placement>();
        //Placements
        public Placement BasePlacement { get; set; }
        public Placement FinalPlacement { get; set; }
        #endregion

        #region ctor
        public Packing(PlantList plantList, Garden garden)
        {
            //First placement
            Garden = garden;
            PlantList = plantList;
            BasePlacement = new Placement(plantList, garden);
            PossiblePlacements.Insert(0, BasePlacement);
            FinalPlacement = ComputePacking(BasePlacement);
        }
        #endregion

        #region other
        public Placement ComputePacking(Placement placement)
        {
            placement.ComputeErodes();
            if (!placement.IsAllErodesEmpties)
            {
                var localPlacements = new List<Placement>();

                //All no placed plants
                foreach (var plant in placement.Plants.Where(x => x.Position == null))
                {
                    //All position in erosion
                    for (var i = 0; i < plant.Erosion.ErodeMap.Cols; i++)
                    {
                        for (var j = 0; j < plant.Erosion.ErodeMap.Rows; j++)
                        {
                            var tempPlacement = new Placement(placement);
                            if (plant.Erosion.ErodeMap.GetValue(i, j) == (byte)255)
                            {
                                tempPlacement.Place(plant, new Point(i ,j));
                                BackTrack(tempPlacement);
                                localPlacements.Add(tempPlacement);
                            }
                        }
                    }
                }
                var bestPlacement = FindBestPlacements(localPlacements);
                placement.Place(bestPlacement);
            }
            return null;
        }
        
        private Placement FindBestPlacements(List<Placement> localPlacements)
        {
            throw new NotImplementedException();
        }

        private void BackTrack(Placement placement)
        {
            //for each position in garden
            for (var i = 0; i < placement.Garden; i++)
            {
                for (var j = 0; j < plant.Erosion.ErodeMap.Rows; j++)
                {
                    if (plant.Erosion.ErodeMap.GetValue(i, j) != (byte)0)
                    {

                    }
                }
            }
            foreach (var plant in placement.Plants.Where(x => x.Position == null))
            {
                
            }
        }

        public bool FastTest(Placement placement)
        {
            //Compute sum of area in each dim
            var dimCount = 0;
            for (var i = placement.MinDim; i <= placement.MaxDim; i++, dimCount++)
            {
                var area = 0;
                foreach (var model in placement.Plants.SelectMany(v => v.Model).Where(x => x.Key == i).Select(c => c.Value))
                {
                    area += model.Total.ToInt32();
                }
                if (i < 0 && area > placement.Garden.RootArea)
                    return false;
                if (i >= 0 && area > placement.Garden.SoilArea)
                    return false;
            }
            return true;
        }
        #endregion
    }
}
