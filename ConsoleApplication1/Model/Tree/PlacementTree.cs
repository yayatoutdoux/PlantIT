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

        public PlacementTree(Garden garden, List<Plant> plantList)
        {
            Nodes = new List<PlacementNode>();

            //Create first node
            var node = new PlacementNode(garden, plantList, this);
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

