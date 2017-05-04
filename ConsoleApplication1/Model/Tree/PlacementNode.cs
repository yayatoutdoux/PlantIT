using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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
        public Dictionary<Plant, Point> Positions { get; set; }
        public Dictionary<Plant, Erosion> Erosions { get; set; }
        public PlacementTree Tree { get; set; }
        public Garden Garden { get; set; }
        public bool IsAllErodesEmpties { get; set; }
        public Placement Placement { get; set; }
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

            Placement = placementNode.Placement;
            Tree = placementNode.Tree;
            IsAllErodesEmpties = placementNode.IsAllErodesEmpties;
            Positions = new Dictionary<Plant, Point>(placementNode.Positions);
        }

        //Create base PlacementNode
        public PlacementNode(Placement placement, Garden garden, List<Plant> plantList, PlacementTree tree)
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

            //Place plant ?
            Placement = placement;

            //Erode
            Erosions = ComputeErodes(Placement);
        }
        #endregion

        #region Erode
        //Compute erodes of plants in the garden
        internal Dictionary<Plant, Erosion> ComputeErodes(Placement placement)
        {
            var erosions = new Dictionary<Plant, Erosion>();
            var isAllErodesEmpties = true;
            foreach (var plant in PlantsToPlace.Concat(PlantsPlaced))
            {
                erosions[plant] = new Erosion(plant, placement);
                if (erosions[plant].ErodePoints.Count > 0 && PlantsToPlace.Contains(plant))
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

        /*private Placement PutInPlacement(Plant plant, Point position)
        {
            var placement = new Placement(Placement);
            for (var i = 0; i < placement.Placements.Length; i++)
            {
                for (var j = 0; j < placement.Placements[i].Height; j++)
                {
                    for (var k = 0; k < placement.Placements[i].Width; k++)
                    {
                        //TODO
                        if (position.X == j && position.Y == k)
                        {
                            Placement.Placements[i].SetValue(j, k, plant.Id);
                            Placement.Placements[i].SetValue(j, k + 1, plant.Id);
                            Placement.Placements[i].SetValue(j, k + 2, plant.Id);
                            Placement.Placements[i].SetValue(j + 2, k, plant.Id);
                            Placement.Placements[i].SetValue(j + 2, k + 1, plant.Id);
                            Placement.Placements[i].SetValue(j + 2, k + 2, plant.Id);
                            Placement.Placements[i].SetValue(j + 1, k, plant.Id);
                            Placement.Placements[i].SetValue(j + 1, k + 2, plant.Id);
                            Placement.Placements[i].SetValue(j + 1, k + 1, plant.Id);
                        }
                    }
                }
            }
            return placement;
        }
        */
        
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
            //Compute sum of area in each dim
            /*var dimCount = 0;
            for (var i = placementNode.MinDim; i <= placementNode.MaxDim; i++, dimCount++)
            {
                var area = 0;
                foreach (var model in PlacementNode.Plants.SelectMany(v => v.Model).Where(x => x.Key == i).Select(c => c.Value))
                {
                    area += model.Total.ToInt32();
                }
                if (i < 0 && area > placementNode.Garden.RootArea)
                    return false;
                if (i >= 0 && area > placementNode.Garden.SoilArea)
                    return false;
            }*/
        }
    }
}

