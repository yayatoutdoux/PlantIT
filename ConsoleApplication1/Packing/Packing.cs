
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
                foreach (var coa in currentNode.OccupyingActions)
                {
                    Console.WriteLine("j k: " + coa.Point.X + " " + coa.Point.Y);
                    ComputeBackTrack(coa, currentNode);
                }
                currentNode = currentNode.FinalPlacement;
            }
            return currentNode;
        }

        //Compute final node
        private void ComputeBackTrack(OccupyingAction coa, PlacementNode node)
        {
            var next = new PlacementNode(node, coa);//create new node place n update coa
            Tree.Add(next);
            node.FinalPlacement = next;

        }

        #endregion
    }
}
