
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV.Cuda;

namespace ConsoleApplication1
{
    class Packing
    {
        #region properties
        public List<Plant> PlantList { get; set; }
        public Garden Garden { get; set; }
        public PlacementTree Tree { get; set; }

        #endregion

        #region ctor
        public Packing(List<Plant> plantList, Garden garden)
        {
            Garden = garden;
            PlantList = plantList;
            Tree = new PlacementTree(garden, plantList);
            ComputePacking();
        }
        #endregion

        #region other
        public PlacementNode ComputePacking()
        {
            //FastTest
            var currentNode = Tree.CurrentNode;
            while (currentNode.PlantsToPlace.Count != 0 && currentNode.IsAllErodesEmpties)
            {
                foreach (var erosion in currentNode.Erosions.Where(x => currentNode.PlantsToPlace.Contains(x.Key)))
                {
                    
                }
            }
            return null;
        }
        
        private PlacementNode FindBestPlacementNodes(List<PlacementNode> localPlacementNodes)
        {
            throw new NotImplementedException();
        }

        private void BackTrack(PlacementNode PlacementNode)
        {
            //for each position in garden
            
        }

        public bool FastTest(PlacementNode PlacementNode)
        {
            //Compute sum of area in each dim
            var dimCount = 0;
            for (var i = PlacementNode.MinDim; i <= PlacementNode.MaxDim; i++, dimCount++)
            {
                var area = 0;
                /*foreach (var model in PlacementNode.Plants.SelectMany(v => v.Model).Where(x => x.Key == i).Select(c => c.Value))
                {
                    area += model.Total.ToInt32();
                }*/
                if (i < 0 && area > PlacementNode.Garden.RootArea)
                    return false;
                if (i >= 0 && area > PlacementNode.Garden.SoilArea)
                    return false;
            }
            return true;
        }
        #endregion
    }
}
