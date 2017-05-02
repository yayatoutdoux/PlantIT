using System.Collections.Generic;

namespace ConsoleApplication1
{
    public class PlacementTree
    {
        #region properties
        public List<PlacementNode> Nodes { get; set; }
        public PlacementNode CurrentNode { get; set; }
        #endregion

        #region ctor
        //Copy contructor
        public PlacementTree(PlacementTree tree)
        {

        }

        public PlacementTree(Placement placement, Garden garden, List<Plant> plantList)
        {
            //Create first node
            Nodes = new List<PlacementNode>();
            var node = new PlacementNode(placement, garden, plantList, this);
            Nodes.Add(node);
            CurrentNode = node;
        }
        #endregion

        public void Add(PlacementNode nodeToPlace, PlacementNode nodeParent = null)
        {
            Nodes.Add(nodeToPlace);
        }
    }
}

