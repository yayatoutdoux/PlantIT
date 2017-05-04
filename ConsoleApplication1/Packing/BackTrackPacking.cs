using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ConsoleApplication1
{
    
    class BackTrackPacking
    {
        public PlacementTree Tree { get; set; }
    
        public BackTrackPacking(PlacementTree tree)
        {
            Tree = tree;
        }

        public void ComputeBackTrackPacking(PlacementNode node)
        {
            node.FinalPlacement = node;
            var nono = BackTrack(node);
        }

        //Prends un node dans un état et va jusqu'au bout en retourn un placemejt final
        private PlacementNode BackTrack(PlacementNode node)
        {
            var currentNode = node;
            if (currentNode == null)
                throw new Exception("First node cannot be null");

            while (currentNode.FastTest() && currentNode.PlantsToPlace.Count != 0 && !currentNode.IsAllErodesEmpties)
            {
                //Pour chaque elem erosion des plantes pas encore placé dans le current node
                /*foreach (var erosion in currentNode.Erosions.Where(x => currentNode.PlantsToPlace.Contains(x.Key)))
                {
                    foreach (var point in erosion.Value.ErodePoints)
                    {
                        

                    }
                }*/




                var newNode = new PlacementNode(currentNode);
                var erosion = currentNode.Erosions.Where(x => currentNode.PlantsToPlace.Contains(x.Key) && x.Value.ErodePoints.Count != 0).First();

               
                newNode.Place(erosion.Key, new Point(erosion.Value.ErodePoints.First().X, erosion.Value.ErodePoints.First().Y));
                Tree.Add(node);
                currentNode = newNode;
            }
            return currentNode;
        }
    }
}
