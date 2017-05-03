using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ConsoleApplication1
{
    class PlacementQuality
    {
         public Plant Plant { get; set; }
         public Erosion Erosion { get; set; }
         public Point Position { get; set; }
         public int Quality { get; set; }
    }
    class BackTrackPacking
    {
        public PlacementTree Tree { get; set; }
    
        public BackTrackPacking(PlacementTree tree)
        {
            Tree = tree;
        }

        public void ComputeBackTrackPacking()
        {
            var lastNode = Tree.Nodes.Last();
            lastNode.FinalPlacement = BackTrack(lastNode);
        }

        public PlacementNode BackTrack(PlacementNode node)
        {
            var currentNode = node;
            while (currentNode.FastTest() && currentNode.PlantsToPlace.Count != 0 && !currentNode.IsAllErodesEmpties)
            {
                //Find best placement(plantlist, erosions)
                var bestNode = FindBestPlacement(currentNode);
                Tree.Add(bestNode);
                currentNode = bestNode;
            }
            return node;
        }

        public PlacementNode FindBestPlacement(PlacementNode placement)
        {
            var node = new PlacementNode(placement);

            //Fill quality
            var qualities = new List<PlacementQuality>();

            foreach (var erosion in node.Erosions.Where(x => node.PlantsToPlace.Contains(x.Key)))
            {
                foreach (var point in erosion.Value.ErodePoints)
                {
                    //Compute quality
                    //Angle
                    //dist a chaque plante deja placé
                    var quality = 0;
                    foreach (var position in node.Positions)
                    {
                        var distance = Math.Max(
                            Math.Abs(point.X - position.Value.X),
                            Math.Abs(point.Y - position.Value.Y)
                        );
                        if (distance == 0)
                        {
                            quality++;
                        }
                    }
                    qualities.Add(new PlacementQuality()
                    {
                        Erosion = erosion.Value,
                        Plant = erosion.Key,
                        Position = point,
                        Quality = quality
                    });
                }
            }

            //Place the best 
            var best = qualities.First(x => x.Quality == qualities.Max(y => y.Quality));
            node.Place(best.Plant, new Point(best.Position.X, best.Position.Y));
            return node;
        }
    }
}
