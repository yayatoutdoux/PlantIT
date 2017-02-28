
using System;
using System.CodeDom;
using System.Linq;

namespace ConsoleApplication1
{
    public class Placement
    {
        public PlantList Plants { get; set; }
        public Garden Garden { get; set; }
        public uint DimCount { get; set; }
        public uint MaxDim { get; set; }
        public int MinDim { get; set; }

        public Placement(PlantList plants, Garden garden)
        {
            Plants = plants;
            ComputeDimInfos(plants);
            garden.SetGardenMaps(MinDim, MaxDim);
            Garden = garden;
        }

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
            DimCount = (uint) dimCount;
            MinDim = minDim;
            MaxDim = (uint) maxDim;
        }

        internal void ComputeErodes()
        {
            foreach (var plant in Plants)
            {
                plant.Erosion = new Erosion(plant, Garden);
            }
        }
    }
}

 