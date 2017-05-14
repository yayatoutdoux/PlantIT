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
        public PlacementNode FinalPlacement { get; set; }
        public List<PlacementNode> Childrens { get; set; }
        public List<PlacementNode> Parents { get; set; }
        public List<Plant> PlantsToPlace { get; set; }
        public List<Plant> PlantsPlaced { get; set; }
        public List<Point> Borders { get; set; }
        public Dictionary<Plant, Point> Positions { get; set; }
        public Dictionary<Plant, Erosion> Erosions { get; set; }
        public PlacementTree Tree { get; set; }
        public Garden Garden { get; set; }
        public bool IsAllErodesEmpties { get; set; }
        public List<OccupyingAction> OccupyingActions { get; set; }
        #endregion

        #region ctor
        //Copy contructor
        public PlacementNode(PlacementNode placementNode)
        {
            Garden = placementNode.Garden;
            FinalPlacement = this;
            placementNode.FinalPlacement = this;
            placementNode.Childrens.Add(this);
            Parents = new List<PlacementNode> { placementNode };
            Childrens = new List<PlacementNode>();
            PlantsToPlace = new List<Plant>(placementNode.PlantsToPlace);
            PlantsPlaced = new List<Plant>(placementNode.PlantsPlaced);

            Erosions = new Dictionary<Plant, Erosion>();
            foreach (var erosion in placementNode.Erosions)
                Erosions.Add(erosion.Key, new Erosion(erosion.Value));

            Tree = placementNode.Tree;
            IsAllErodesEmpties = placementNode.IsAllErodesEmpties;
            Positions = new Dictionary<Plant, Point>(placementNode.Positions);
        }

        //Create base PlacementNode
        public PlacementNode(Garden garden, List<Plant> plantList, PlacementTree tree)
        {
            //Tree
            Tree = tree;
            FinalPlacement = this;
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

        private List<OccupyingAction> InitOccupyingActions()
        {
            var list = new List<OccupyingAction>();
            foreach (var erosion in Erosions)
            {
                foreach (var point in erosion.Value.ErodePoints)
                {
                    var coa = ComputeCOA(erosion, point);
                    if(coa != null)
                        list.Add(coa);
                }
            }
            return list;
        }

        private OccupyingAction ComputeCOA(KeyValuePair<Plant, Erosion> erosion, Point point)
        {
            var coa = new OccupyingAction(point, erosion.Key, this);
            var contacts = coa.Distances.Where(x => x.Value == 0);
            if (contacts.Count() <= 1)
                return null;

            var types = contacts.Select(x => x.SideType);
            if (types.Contains(SideType.TOP))
            {
                if (types.Contains(SideType.RIGHT) || types.Contains(SideType.LEFT))
                {
                    return coa;
                }
            }
            if (types.Contains(SideType.BOTTOM))
            {
                if (types.Contains(SideType.RIGHT) || types.Contains(SideType.LEFT))
                {
                    return coa;
                }
            }
            return null;
        }
        #endregion

        #region Erode
        //Compute erodes of plants in the garden
        internal Dictionary<Plant, Erosion> InitErodes()
        {
            var erosions = new Dictionary<Plant, Erosion>();
            var isAllErodesEmpties = true;

            foreach (var plant in PlantsToPlace)
            {
                erosions[plant] = new Erosion(plant, Garden);
                if (erosions[plant].ErodePoints.Count > 0)
                    isAllErodesEmpties = false;
            }

            IsAllErodesEmpties = isAllErodesEmpties;
            return erosions;
        }
        #endregion

        #region Place
        //Place une plante dans un noeud de larbre : met à jour l'erosion de chaque plante de ce noeud
        public void Place(Plant plant, Point position)
        {
            //Placement
            Positions.Add(plant, position);

            //Erosion
            UpdateErosions(plant, position);

            //Move plant
            PlantsPlaced.Add(PlantsToPlace.First(x => x == plant));
            PlantsToPlace.Remove(plant);
        }
        
        //Met à jour l'erosion avec nouvelle plant
        private void UpdateErosions(Plant plant, Point position)
        {
            var isAllErodesEmpties = true;

            foreach (var erosion in Erosions.Where(x => PlantsToPlace.Contains(x.Key)))//Erode de chaque plants no placed
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
                if (erosion.Value.ErodePoints.Count > 0 && erosion.Key != plant)
                    isAllErodesEmpties = false;
            }
            IsAllErodesEmpties = isAllErodesEmpties;
        }
        #endregion

        public bool FastTest()
        {
            return true;
        }
    }
}

