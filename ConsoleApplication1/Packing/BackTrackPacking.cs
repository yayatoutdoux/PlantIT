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
            var nono = BackTrack(node);
        }

        //Prends un node dans un état et va jusqu'au bout en retourn un placemejt final
        private PlacementNode BackTrack(PlacementNode node)
        {
            var currentNode = node;
            if (currentNode == null)
                throw new Exception("First node cannot be null");

            return currentNode;
        }
    }
}
