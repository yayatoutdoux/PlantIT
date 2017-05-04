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
            node.FinalPlacement = BackTrack(node);
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
            return currentNode;
        }

        public PlacementNode FindBestPlacement(PlacementNode placement)
        {
            var node = new PlacementNode(placement);



            
            //Fill quality
            PlacementQuality bestQualityPlacement = null;
            var maxQuality = -1;
            //Pour chaque plante à placer
            foreach (var erosion in node.Erosions.Where(x => node.PlantsToPlace.Contains(x.Key)))
            {
                //Pour chaque placement possible
                foreach (var point in erosion.Value.ErodePoints)
                {
                    //Is feasable COA
                    //Creer list de points interessants avec le coté
                    var plantInContact = new Dictionary<Plant, int>();
                    foreach (var position in node.Positions)//Pour chaque plante déja placée
                    {
                        var distance =
                            Math.Max(Math.Abs(point.X - position.Value.X) - (position.Key.Model[0] + erosion.Key.Model[0] + 1), 0) +
                            Math.Max(Math.Abs(point.Y - position.Value.Y) - (position.Key.Model[0] + erosion.Key.Model[0] + 1), 0);
                        if (distance == 0)
                        {
                            if(position.Value.X > point.X //+d
                                && position.Value.Y >= point.Y - erosion.Key.Model[0] - (position.Value.X - (point.X + erosion.Key.Model[0]  + 1) )
                                && position.Value.Y <= point.Y + erosion.Key.Model[0] + (position.Value.X - (point.X + erosion.Key.Model[0] + 1)))
                            {
                                plantInContact.Add(position.Key, 1);
                            }
                            if (position.Value.X < point.X //+d
                                && position.Value.Y >= point.Y - erosion.Key.Model[0] - (position.Value.X - (point.X + erosion.Key.Model[0] + 1))
                                && position.Value.Y <= point.Y + erosion.Key.Model[0] + (position.Value.X - (point.X + erosion.Key.Model[0] + 1)))
                            {
                                plantInContact.Add(position.Key, 2);
                            }

                            if (point.Y < position.Value.Y //+d
                                && position.Value.X >= point.X - erosion.Key.Model[0] - (position.Value.Y - (point.Y + erosion.Key.Model[0] + 1))
                                && position.Value.X <= point.X + erosion.Key.Model[0] + (position.Value.Y - (point.Y + erosion.Key.Model[0] + 1)))
                            {
                                plantInContact.Add(position.Key, 3);
                            }
                            if (point.Y > position.Value.Y //+d
                                && position.Value.X >= point.X - erosion.Key.Model[0] - (position.Value.Y - (point.Y + erosion.Key.Model[0] + 1))
                                && position.Value.X <= point.X + erosion.Key.Model[0] + (position.Value.Y - (point.Y + erosion.Key.Model[0] + 1)))
                            {
                                plantInContact.Add(position.Key, 4);
                            }
                        }

                    }
                    var cavn = plantInContact.Values.ToList().Distinct().Count();
                    if (cavn > maxQuality)
                    {
                        bestQualityPlacement = new PlacementQuality
                        {
                            Erosion = erosion.Value,
                            Plant = erosion.Key,
                            Position = point,
                            Quality = cavn
                        };
                    }
                    if (cavn == 2)
                    {
                        //if feasableCOA
                    }
                }
            }

            //Place the best 
            node.Place(bestQualityPlacement.Plant, new Point(bestQualityPlacement.Position.X, bestQualityPlacement.Position.Y));
            return node;
        }
    }
}
