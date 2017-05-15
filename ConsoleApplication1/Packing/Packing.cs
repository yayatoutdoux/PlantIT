
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using Emgu.CV;
using Emgu.CV.Cuda;

namespace ConsoleApplication1
{
    class Packing
    {
        #region properties

        //Garden
        public List<Plant> PlantList { get; set; }
        public Garden Garden { get; set; }
        public PlacementTree Tree { get; set; }

        //Backtrack
        public BackTrackPacking BackTrackPacking { get; set; }

        //Result
        public PlacementNode FinalNode { get; set; }

        #endregion

        #region ctor
        public Packing(List<Plant> plantList, Garden garden)
        {
            Garden = garden;
            PlantList = plantList;
            Tree = new PlacementTree(garden, plantList);

            //BackTrackPacking = new BackTrackPacking(Tree);
            FinalNode = ComputePacking();
        }
        #endregion

        #region other
        public PlacementNode ComputePacking()
        {
            var currentNode = Tree.CurrentNode;
            if (currentNode == null)
                throw  new Exception("First node cannot be null");

            while (/*currentNode.FastTest() && */currentNode.PlantsToPlace.Count != 0 && currentNode.OccupyingActions.Count != 0)
            {
                
                 currentNode = ComputeBackTrack(currentNode);
                
            }
            return currentNode;
        }

        //Return node if better than best (node)
        private PlacementNode ComputeBackTrack(PlacementNode node)
        {
            var currentNode = node;
            Tree.Add(currentNode);
            while (/*currentNode.FastTest() && */currentNode.PlantsToPlace.Count != 0 && currentNode.OccupyingActions.Count != 0)
            {
                var bestCoa = currentNode.OccupyingActions.First();
                foreach (var coa in currentNode.OccupyingActions.Skip(1))
                {
                    Console.WriteLine("j k: " + coa.Point.X + " " + coa.Point.Y);

                    if (coa.CompareTo(bestCoa) == 1)
                    {
                        bestCoa = coa;
                    }
                }
                currentNode = new PlacementNode(currentNode, bestCoa);
                Tree.Add(currentNode);
            }
            //Best ?
            if (node.PlantsPlaced.Sum(x => x.Model[0]) < currentNode.PlantsPlaced.Sum(x => x.Model[0]))
                return currentNode;
            return node;

        }

        #endregion
    }
}
