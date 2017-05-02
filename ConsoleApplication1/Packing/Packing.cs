
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Cuda;

namespace ConsoleApplication1
{
    class Packing
    {
        #region properties
        public List<Plant> PlantList { get; set; }
        public Garden Garden { get; set; }
        public PlacementTree Tree { get; set; }
        public Placement Placement { get; set; }

        public PlacementNode FinalNode { get; set; }

        #endregion

        #region ctor
        public Packing(List<Plant> plantList, Garden garden)
        {
            Garden = garden;
            PlantList = plantList;
            Placement = new Placement(garden);
            Tree = new PlacementTree(Placement, garden, plantList);
            FinalNode = ComputePacking();
        }
        #endregion

        #region other
        public PlacementNode ComputePacking()
        {
            //FastTest
            var currentNode = Tree.CurrentNode;
            if (currentNode == null)
            {
                throw  new Exception("First node cannot be null");
            }

            while (currentNode.PlantsToPlace.Count != 0 || currentNode.IsAllErodesEmpties)
            {
                foreach (var erosion in currentNode.Erosions.Where(x => currentNode.PlantsToPlace.Contains(x.Key)))
                {
                    var map = erosion.Value.ErodeMap;
                    for (var j = 0; j < map.Height; j++)
                    {
                        for (var k = 0; k < map.Width; k++)
                        {
                            if (map.GetValue(j, k) == (byte)255)
                            {
                                Console.WriteLine("j k: " + j + " " + k);

                                var node = new PlacementNode(currentNode);
                                
                                node.Place(erosion.Key, new Point(j, k));
                                Tree.Add(node);
                                //BT
                            }
                        }
                    }
                }

                currentNode = FindBestPlacementNodes(currentNode.Childrens);
                if (!currentNode.Childrens.Any())
                    return currentNode;
            }
            return currentNode;
        }
        
        private PlacementNode FindBestPlacementNodes(List<PlacementNode> placementNodes)
        {
            return placementNodes.First();
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
