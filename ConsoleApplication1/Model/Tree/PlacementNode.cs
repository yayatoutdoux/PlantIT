using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleApplication1
{
    public class PlacementNode
    {
        #region properties
        public List<PlacementNode> Childrens { get; set; }
        public List<PlacementNode> Parents { get; set; }
        public List<Plant> PlantsToPlace { get; set; }
        public List<Plant> PlantsPlaced { get; set; }
        public List<Point> Borders { get; set; }
        public Dictionary<Plant, Point> Positions { get; set; }
        public Dictionary<Plant, Erosion> Erosions { get; set; }
        public PlacementTree Tree { get; set; }
        public Garden Garden { get; set; }
        public List<OccupyingAction> OccupyingActions { get; set; }
        #endregion

        #region ctor
        //Create base PlacementNode
        public PlacementNode(Garden garden, List<Plant> plantList, PlacementTree tree)
        {
            //Tree
            Tree = tree;
            Childrens = new List<PlacementNode>();
            Parents = new List<PlacementNode>();

            //Fill Plant list
            PlantsToPlace = plantList;
            PlantsPlaced = new List<Plant>();
            Positions = new Dictionary<Plant, Point>();
            Garden = garden;

            //Erode
            Borders = garden.Borders;
            Erosions = InitErodes();
            OccupyingActions = InitOccupyingActions();
        }

        public PlacementNode(PlacementNode placementNode, OccupyingAction coa)
        {
            Garden = placementNode.Garden;
            Borders = new List<Point>(Garden.Borders);
            OccupyingActions = new List<OccupyingAction>(placementNode.OccupyingActions.Select(x => (OccupyingAction)x.Clone()));

            //Tree
            placementNode.Childrens.Add(this);
            Parents = new List<PlacementNode> { placementNode };
            Childrens = new List<PlacementNode>();
            Tree = placementNode.Tree;

            //Plant
            PlantsToPlace = new List<Plant>(placementNode.PlantsToPlace);
            PlantsPlaced = new List<Plant>(placementNode.PlantsPlaced);
            

            Erosions = new Dictionary<Plant, Erosion>();
            foreach (var erosion in placementNode.Erosions)
                Erosions.Add(erosion.Key, new Erosion(erosion.Value));

            Positions = new Dictionary<Plant, Point>(placementNode.Positions);
            
            Place(coa);
        }

        private void Place(OccupyingAction coa)
        {
            Positions.Add(coa.Plant, coa.Point);
            PlantsPlaced.Add(coa.Plant);
            PlantsToPlace.Remove(coa.Plant);

            //Erosion
            Erosions.Remove(coa.Plant);
            UpdateErosions(coa.Plant, coa.Point);

            //Update COA
            UpdateCOAs(coa);
        }

        private void UpdateCOAs(OccupyingAction coa)
        {
            //remove border points
            var borderDistancesToRemove = coa.Contacts.Where(x => x.Plant == null);
            var borderPointToRemove = borderDistancesToRemove.Select(x => x.Point);
            foreach (var pt in borderPointToRemove)
            {
                Borders.Remove(pt);
            }
            
            //Modify existing coas
            for (var i = OccupyingActions.Count - 1; i >= 0; i--)
            {
                //Delete coa where point is too close to placed, 

                if (OccupyingActions[i].Plant == coa.Plant ||
                    Math.Max(
                        Math.Abs(OccupyingActions[i].Point.X - coa.Point.X),
                        Math.Abs(OccupyingActions[i].Point.Y - coa.Point.Y)
                    ) - (OccupyingActions[i].Plant.Model[0] + coa.Plant.Model[0] + 1) < 0)
                {
                    OccupyingActions.RemoveAt(i);
                    continue;
                }

                //remove distance that are removed border
                for (var j = OccupyingActions[i].Distances.Count - 1; j >= 0; j--)
                {
                    if (borderPointToRemove.Contains(OccupyingActions[i].Distances[j].Point))
                    {
                        OccupyingActions[i].Distances.RemoveAt(j);
                    }
                }

                //add plant to distance
                OccupyingActions[i].Distances.Add(new Distance(coa.Point, coa.Plant, OccupyingActions[i].Point, OccupyingActions[i].Plant));
                
                OccupyingActions[i].Contacts = OccupyingActions[i].Distances.Where(x => x.Value == 0 && (int)x.SideType < 4);

                //remove empt coa and not angles
                if (!TestCOA(OccupyingActions[i]))
                {
                    OccupyingActions.RemoveAt(i);
                }
                
            }

            //Create new COAs
            foreach (var erosion in Erosions)
            {
                var points = erosion.Value.ErodePoints.Where(x =>
                    Math.Max(Math.Abs(x.X - coa.Point.X) - (coa.Plant.Model[0] + 1 + erosion.Key.Model[0]), 0)
                    + Math.Max(Math.Abs(x.Y - coa.Point.Y) - (coa.Plant.Model[0] + 1 + erosion.Key.Model[0]), 0) == 0

                    && Math.Abs(x.X - coa.Point.X) - (coa.Plant.Model[0] + 1 + erosion.Key.Model[0]) != 
                    Math.Abs(x.Y - coa.Point.Y) - (coa.Plant.Model[0] + 1 + erosion.Key.Model[0])
                );
                foreach (var pt in points)
                {
                    var newCoa = new OccupyingAction(pt, erosion.Key, this);
                    if (TestCOA(newCoa))
                    {
                        OccupyingActions.Add(newCoa);
                    }
                }
            }
        }

        private bool TestCOA(OccupyingAction occupyingAction)
        {
            var types = occupyingAction.Contacts
                .Select(x => x.SideType);
            var typesCount = types
                .GroupBy(x => x)
                .Count();
            if (typesCount > 1)
            {
                if (typesCount > 2)
                    return true;
                else
                {
                    if (types.Contains(SideType.TOP))
                    {
                        if (types.Contains(SideType.RIGHT) || types.Contains(SideType.LEFT))
                        {
                            return true;
                        }
                    }
                    if (types.Contains(SideType.BOTTOM))
                    {
                        if (types.Contains(SideType.RIGHT) || types.Contains(SideType.LEFT))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private List<OccupyingAction> InitOccupyingActions()
        {
            var list = new List<OccupyingAction>();
            foreach (var erosion in Erosions)
            {
                foreach (var point in erosion.Value.ErodePoints)
                {
                    var coa = new OccupyingAction(point, erosion.Key, this);
                    if(TestCOA(coa))
                        list.Add(coa);
                }
            }
            return list;
        }
        #endregion

        #region Erode
        //Compute erodes of plants in the garden
        internal Dictionary<Plant, Erosion> InitErodes()
        {
            var erosions = new Dictionary<Plant, Erosion>();
            foreach (var plant in PlantsToPlace)
            {
                erosions[plant] = new Erosion(plant, Garden);
            }
            return erosions;
        }
        #endregion
        private void UpdateErosions(Plant plant, Point position)
        {
            foreach (var erosion in Erosions)
            {
                for (var i = 0; i < erosion.Value.ErodePoints.Count; i++)//Pour chaque point de l'erosion
                {
                    var distance = Math.Max(
                        Math.Abs(erosion.Value.ErodePoints[i].X - position.X),
                        Math.Abs(erosion.Value.ErodePoints[i].Y - position.Y)
                    );

                    //On regardesi on peut tj la laisser blanche
                    for (var k = 0; k < erosion.Key.Model.Length; k++)
                    {
                        if (erosion.Key.Model[k] + plant.Model[k] + 1 > distance)//Peut plus etre placés
                        {
                            Erosions[erosion.Key].ErodePoints[i] = new Point(-1, -1);
                        }
                    }
                }
                Erosions[erosion.Key].ErodePoints.RemoveAll(x => x == new Point(-1, -1));
            }
        }
    }
}

