

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
            var tatata = node.Erosions.Where(x => x.Value.ErodePoints.Count > 0 && node.PlantsToPlace.Contains(x.Key)).First();
            node.Place(
                tatata.Key, 
                new Point(
                    tatata.Value.ErodePoints.First().X,
                    tatata.Value.ErodePoints.First().Y
                )
            );

            //Place the best 
            /*foreach (var erosion in node.Erosions.Where(x => node.PlantsToPlace.Contains(x.Key)))
            {
                foreach (var point in erosion.Value.ErodePoints)
                {
                    node.Place(erosion.Key, new Point(point.X, point.Y));
                }
            }*/

            return node;
        }
    }
}
