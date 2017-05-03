using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

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
            PlacementQuality bestQualityPlacement = null;
            var maxQuality = -1;
            //Pour chaque plante à placer
            var first = false;
            foreach (var erosion in node.Erosions.Where(x => node.PlantsToPlace.Contains(x.Key)))
            {
                //Pour chaque placement possible
                foreach (var point in erosion.Value.ErodePoints)
                {
                    if (!first)
                    {
                        first = true;
                        bestQualityPlacement = new PlacementQuality
                        {
                            Erosion = erosion.Value,
                            Plant = erosion.Key,
                            Position = point,
                            Quality = 0
                        };
                    }
                    //Is feasable COA
                    //Rechercher dans chaque angle si il y a 2 rect ?
                    //Iterer dans les cases à coté et voir si il y a 


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
                    if (cavn > 2)
                    {
                        int tt = 0;
                    }

                    //Compute quality  Angle  dist a chaque plante deja placé
                    /*var quality = 0;
                    var angle2D = false;
                    var edge2D = false;
                    foreach (var position in node.Positions)//Pour chaque plante déja placée
                    {
                        var distance = Math.Max(
                            Math.Abs(point.X - position.Value.X),
                            Math.Abs(point.Y - position.Value.Y)
                        );
                        if (Math.Abs(erosion.Key.Model[0] + position.Key.Model[0] + 1 - distance) == 0)//Coller à une plante deja placé
                        {
                            quality++;
                        }
                    }
                    if (quality > maxQuality) //Coller à au moins une plante
                    {
                        maxQuality = quality;
                        bestQualityPlacement = new PlacementQuality
                        {
                            Erosion = erosion.Value,
                            Plant = erosion.Key,
                            Position = point,
                            Quality = quality
                        };
                    }*/
                }
            }

            //Place the best 
            node.Place(bestQualityPlacement.Plant, new Point(bestQualityPlacement.Position.X, bestQualityPlacement.Position.Y));
            return node;
        }
    }
}
