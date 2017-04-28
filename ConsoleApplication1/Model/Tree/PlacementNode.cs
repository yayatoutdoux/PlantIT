using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

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
        public KeyValuePair<Plant, Point> Positions { get; set; }
        public KeyValuePair<Plant, Erosion> Erosions { get; set; }
        public Mat Placement { get; set; }
        public PlacementTree Tree { get; set; }
        public Garden Garden { get; set; }
        public uint DimCount { get; set; }
        public uint MaxDim { get; set; }
        public int MinDim { get; set; }
        public bool IsAllErodesEmpties { get; set; }
        public uint PlacedPlantCount { get; set; } = 0;
        #endregion

        #region ctor
        //Copy contructor
        public PlacementNode(PlacementNode PlacementNode)
        {
            Garden = PlacementNode.Garden;
            DimCount = PlacementNode.DimCount;
            MaxDim = PlacementNode.MaxDim;
            MinDim = PlacementNode.MinDim;
            IsAllErodesEmpties = PlacementNode.IsAllErodesEmpties;
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
            Positions = new KeyValuePair<Plant, Point>();
            Garden = garden;

            //Placement
            Placement = CreatePlacement();

            //Erode
            Erosions = ComputeErodes();
        }

        private Mat CreatePlacement()
        {
            var placement = new Mat(
                Garden.SoilMap.Size,
                DepthType.Cv32S,
                10
            );
            //Fill placement
            for (int i = 1; i < placement.Size.Height; i++)
            {
                for (int j = 1; j < placement.Size.Width; j++)
                {
                    placement.SetValue(i, j, Garden.SoilMap.GetValue(i, j) == 255 ? int.MaxValue : 0);
                }
            }
            return placement;
        }
        #endregion

        #region erode
        //Compute erodes of plants in the garden
        internal KeyValuePair<Plant, Erosion> ComputeErodes()
        {

            foreach (var plant in Plants)
            {
                plant.Erosion = new Erosion(plant, Garden);
                if (plant.Erosion.Size > 0)
                    isAllErodesEmpties = false;
            }
            IsAllErodesEmpties = isAllErodesEmpties;
        }

        //Update erode when plant is added
        private void UpdateErodes(Plant plant)
        {
            var isAllErodesEmpties = true;
            //foreach plant with erosion
            /*foreach (var otherPlant in Plants.Where(x => x.Erosion.Size > 0))//TODO only plants not positionned ?
            {
                //Si plant placée est sur erode ou êrturb erode suppr erode
                plant.Erosion = new Erosion(plant, Garden);
                if (otherPlant.Erosion.Size > 0)
                    isAllErodesEmpties = false;
            }*/
            IsAllErodesEmpties = isAllErodesEmpties;
        }


        #endregion

        #region place
        public void Place(Plant plant, Point position)
        {
            plant.Position = position;
            plant.PositionOrder = PlacedPlantCount++;
            Garden.DrawPlant(plant, this);
            UpdateErodes(plant);
        }

        internal void Place(PlacementNode bestPlacementNode)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region other
        //Compute min and max dim of the plant list
        public void ComputeDimInfos(List<Plant> plants)
        {
            var maxDim = 0;
            var minDim = 0;
            var dimCount = 0;
            foreach (var plant in plants)
            {
                var keys = plant.Model.Select(x => x.Key);
                var dimCountLocal = plant.Model.Count;
                if (dimCountLocal > dimCount)
                    dimCount = dimCountLocal;
                var minDimLocal = keys.Min();
                if (minDimLocal < minDim)
                    minDim = minDimLocal;

                var maxDimLocal = keys.Max();
                if (maxDimLocal > maxDim)
                    maxDim = maxDimLocal;
            }
            DimCount = (uint)dimCount;
            MinDim = minDim;
            MaxDim = (uint)maxDim;
        }
        #endregion
    }
}

