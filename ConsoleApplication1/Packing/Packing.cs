
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication1
{
    class Packing
    {
        public PlantList PlantList { get; set; }
        public Garden Garden { get; set; }

        //All ends of backtrack
        public List<Placement> PossiblePlacements { get; set; } = new List<Placement>();

        public Placement BasePlacement { get; set; }
        public Placement FinalPlacement { get; set; }

        public Packing(PlantList plantList, Garden garden)
        {
            //First placement
            Garden = garden;
            PlantList = plantList;
            BasePlacement = new Placement(plantList, garden);
            PossiblePlacements.Insert(0, BasePlacement);
            FinalPlacement = ComputePacking(BasePlacement);
        }

        public Placement ComputePacking(Placement placement)
        {
            placement.ComputeErodes();
            if (!placement.IsAllErodesEmpties)
            {
                var localGarden = new List<Garden>();
                foreach (var plant in placement.Plants.Where(x => x.Position == null))
                {
                    var tempPlacement = new Placement(placement);
                }
            }
            return null;
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
    }
}
