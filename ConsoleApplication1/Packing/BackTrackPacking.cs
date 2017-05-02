

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
            return node;
        }
    }
}
