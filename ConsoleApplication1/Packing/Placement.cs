using System;
using System.Drawing;
using System.Linq;

namespace ConsoleApplication1
{
    public class Placement
    {
        #region properties

        public PlantList Plants { get; set; }
        public Garden Garden { get; set; }
        public uint DimCount { get; set; }
        public uint MaxDim { get; set; }
        public int MinDim { get; set; }
        public bool IsAllErodesEmpties { get; set; }
        public uint PlacedPlantCount { get; set; } = 0;

        #endregion

        #region ctor
        //Copy contructor
        public Placement(Placement placement)
        {
            Plants = placement.Plants;
            Garden = placement.Garden;
            DimCount = placement.DimCount;
            MaxDim = placement.MaxDim;
            MinDim = placement.MinDim;
            IsAllErodesEmpties = placement.IsAllErodesEmpties;
        }

        //Create base placement
        public Placement(PlantList plants, Garden garden)
        {
            //Plants
            Plants = plants;
            ComputeDimInfos(plants);

            garden.SetGardenMaps(MinDim, MaxDim);
            Garden = garden;
        }
#endregion

        #region erode
        //Compute erodes of plants in the garden
        internal void ComputeErodes()
        {
            var isAllErodesEmpties = true;
            foreach (var plant in Plants)
            {
                plant.Erosion = new Erosion(plant, Garden);
                if (plant.Erosion.Size > 0)
                    isAllErodesEmpties = false;
            }
            IsAllErodesEmpties = isAllErodesEmpties;
        }

        //Update erode when plant is added
        private void UpdateErodes(Plant plant)
        {
            var isAllErodesEmpties = true;
            //foreach plant with erosion
            foreach (var otherPlant in Plants.Where(x => x.Erosion.Size > 0))//TODO only plants not positionned ?
            {
                //Si plant placée est sur erode ou êrturb erode suppr erode
                plant.Erosion = new Erosion(plant, Garden);
                if (otherPlant.Erosion.Size > 0)
                    isAllErodesEmpties = false;
            }
            IsAllErodesEmpties = isAllErodesEmpties;
        }


        #endregion

        #region place
        public void Place(Plant plant, Point position)
        {
            plant.Position = position;
            plant.PositionOrder = PlacedPlantCount++;
            Garden.DrawPlant(plant, this);
            UpdateErodes(plant);
        }

        internal void Place(Placement bestPlacement)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region other
        //Compute min and max dim of the plant list
        public void ComputeDimInfos(PlantList plants)
        {
            var maxDim = 0;
            var minDim = 0;
            var dimCount = 0;
            foreach (var plant in plants)
            {
                var keys = plant.Model.Select(x => x.Key);
                var dimCountLocal = plant.Model.Count;
                if (dimCountLocal > dimCount)
                    dimCount = dimCountLocal;
                var minDimLocal = keys.Min();
                if (minDimLocal < minDim)
                    minDim = minDimLocal;

                var maxDimLocal = keys.Max();
                if (maxDimLocal > maxDim)
                    maxDim = maxDimLocal;
            }
            DimCount = (uint)dimCount;
            MinDim = minDim;
            MaxDim = (uint)maxDim;
        }
        #endregion
    }
}

