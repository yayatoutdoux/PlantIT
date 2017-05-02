
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
        public BackTrackPacking BackTrackPacking { get; set; }

        public PlacementNode FinalNode { get; set; }

        #endregion

        #region ctor
        public Packing(List<Plant> plantList, Garden garden)
        {
            Garden = garden;
            PlantList = plantList;
            Placement = new Placement(garden);
            Tree = new PlacementTree(Placement, garden, plantList);
            BackTrackPacking = new BackTrackPacking(Tree);
            FinalNode = ComputePacking();
        }
        #endregion

        #region other
        public PlacementNode ComputePacking()
        {
            var currentNode = Tree.CurrentNode;
            if (currentNode == null)
                throw  new Exception("First node cannot be null");

            while (currentNode.FastTest() && currentNode.PlantsToPlace.Count != 0 && !currentNode.IsAllErodesEmpties)
            {
                //Pour chaque elem erosion des plantes pas encore placé dans le current node
                foreach (var erosion in currentNode.Erosions.Where(x => currentNode.PlantsToPlace.Contains(x.Key)))
                {
                    foreach (var point in erosion.Value.ErodePoints)
                    {
                        ComputeBackTrack(point, erosion, currentNode);
                    }
                }
                if (!currentNode.Childrens.Any())
                    break;
                currentNode = FindBestPlacementNodes(currentNode.Childrens);
            }
            return currentNode;
        }
        
        private PlacementNode FindBestPlacementNodes(IEnumerable<PlacementNode> placementNodes)
        {
            return placementNodes.First();
        }

        private void ComputeBackTrack(Point point, KeyValuePair<Plant, Erosion> erosion, PlacementNode currentNode)
        {
            Console.WriteLine("j k: " + point.X + " " + point.Y);

            //Create new node
            var node = new PlacementNode(currentNode);
            node.Place(erosion.Key, new Point(point.X, point.Y));
            Tree.Add(node);

            BackTrackPacking.ComputeBackTrackPacking();
        }
        #endregion
    }
}
