
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        //Result
        public PlacementNode FinalNode { get; set; }
        public PlacementNode FinalNodeInter { get; set; }

        #endregion

        #region ctor
        public Packing(List<Plant> plantList, Garden garden)
        {
            Garden = garden;
            PlantList = plantList;
            Tree = new PlacementTree(garden, plantList);
            FinalNodeInter = null;
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
            var allFinals = new Dictionary<PlacementNode, OccupyingAction>();
            
            foreach (var coa in currentNode.OccupyingActions.Skip(1))
            {
                var finalNode = GetFinalNode(currentNode, coa);
                allFinals.Add(finalNode, coa);
                if (FinalNodeInter == null || FinalNodeInter.GetInteractionScore() < finalNode.GetInteractionScore())
                    FinalNodeInter = finalNode;
            }
            Console.WriteLine("fzf");

            //Best ?
            var maxInter = allFinals.Max(x => x.Key.GetInteractionScore());
            var bestOfbest = allFinals.Where(x => x.Key.GetInteractionScore() == maxInter);
            if(bestOfbest.Count() == 1)
                return new PlacementNode(currentNode, bestOfbest.First().Value); ;
            var maxArea = bestOfbest.Max(x => x.Key.PlantsPlaced.Sum(y => y.Model[0]));
            var bestsCoa = bestOfbest.Where(x => x.Key.PlantsPlaced.Sum(y => y.Model[0]) == maxArea);
            return new PlacementNode(currentNode, bestsCoa.First().Value); 
        }

        private PlacementNode GetFinalNode(PlacementNode currentNode, OccupyingAction oa)
        {
            var newNode = new PlacementNode(currentNode, oa);
            while (/*currentNode.FastTest() && */newNode.PlantsToPlace.Count != 0 && newNode.OccupyingActions.Count != 0)
            {
                var bestCoa = newNode.OccupyingActions.First();

                foreach (var coa in newNode.OccupyingActions.Skip(1))
                {
                    if (coa.CompareTo(bestCoa, newNode) == 1)
                    {
                        bestCoa = coa;
                    }
                }
                newNode = new PlacementNode(newNode, bestCoa);
                Tree.Add(currentNode);
            }
            return newNode;
        }
        #endregion
    }
}
