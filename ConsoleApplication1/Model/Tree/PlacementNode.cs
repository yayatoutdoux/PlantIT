using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

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
        public Mat[] Placement { get; set; }
        public PlacementTree Tree { get; set; }
        public Garden Garden { get; set; }
        public uint DimCount { get; set; }
        public uint MaxDim { get; set; }
        public int MinDim { get; set; }
        public bool IsAllErodesEmpties { get; set; }
        public uint PlacedPlantCount { get; set; }
        #endregion

        #region ctor
        //Copy contructor
        public PlacementNode(PlacementNode placementNode)
        {
            Garden = placementNode.Garden;
            DimCount = placementNode.DimCount;
            MaxDim = placementNode.MaxDim;
            MinDim = placementNode.MinDim;
            FinalPlacement = this;
            placementNode.FinalPlacement = this;
            placementNode.Childrens.Add(this);
            Parents = new List<PlacementNode> { placementNode };
            Childrens = new List<PlacementNode>();
            PlantsToPlace = new List<Plant>(placementNode.PlantsToPlace);
            PlantsPlaced = new List<Plant>(placementNode.PlantsPlaced);
            Placement = new Mat[Constants.SoilLayerCount];
            //for()
            Erosions = new Dictionary<Plant, Erosion>(placementNode.Erosions);
            Tree = placementNode.Tree;
            IsAllErodesEmpties = true;
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

            //Placement
            Placement = CreatePlacement();

            //Erode
            Erosions = ComputeErodes();
        }

        private Mat[] CreatePlacement()
        {
            //Create 10 channel Mat
            var placements = new Mat[Constants.SoilLayerCount];
            
            for (var i = 0; i < Constants.SoilLayerCount; i++)
            {
                placements[i] = new Mat(
                    Garden.SoilMap.Size,
                    DepthType.Cv32S,
                    1
                );
                placements[i].SetTo(new MCvScalar(0));
                if(i == 0) continue;
                for (var j = 0; j < placements[i].Height; j++)
                {
                    for (var k = 0; k < placements[i].Width; k++)
                    {
                        if (Garden.SoilMap.GetValue(j, k) == (byte) 255)
                        {
                            placements[i].SetValue(j, k, int.MaxValue);
                        }
                    }
                }
                CvInvoke.BitwiseAnd(placements[i], placements[0], placements[0]);
            }
            
            return placements; 
        }
        #endregion

        #region Erode
        //Compute erodes of plants in the garden
        internal Dictionary<Plant, Erosion> ComputeErodes()
        {
            var erosions = new Dictionary<Plant, Erosion>();
            var isAllErodesEmpties = true;
            foreach (var plant in PlantsToPlace.Concat(PlantsPlaced))
            {
                erosions[plant] = new Erosion(plant, Placement);
                if (erosions[plant].Size > 0)
                    isAllErodesEmpties = false;
            }
            IsAllErodesEmpties = isAllErodesEmpties;
            return erosions;
        }
        #endregion

        #region Place
        public void Place(Plant plant, Point position)
        {
            //Placement
            PutInPlacement(plant, position);

            //Erosion
            UpdateErosion(plant, position);

            //Move plant
            PlantsPlaced.Add(PlantsToPlace.First(x => x == plant));
            PlantsToPlace.Remove(plant);
        }

        private void PutInPlacement(Plant plant, Point position)
        {
            //add 
            var placements = new VectorOfMat();


            for (var i = 0; i < placements.Size; i++)
            {
                for (var j = 0; j < placements[i].Height; j++)
                {
                    for (var k = 0; k < placements[i].Width; k++)
                    {
                        //TODO
                        if (position.X == j && position.Y == k)
                        {
                            placements[i].SetValue(j, k, plant.Id);
                            placements[i].SetValue(j, k + 1, plant.Id);
                            placements[i].SetValue(j, k + 2, plant.Id);
                            placements[i].SetValue(j + 2, k, plant.Id);
                            placements[i].SetValue(j + 2, k + 1, plant.Id);
                            placements[i].SetValue(j + 2, k + 2, plant.Id);
                            placements[i].SetValue(j + 1, k, plant.Id);
                            placements[i].SetValue(j + 1, k + 2, plant.Id);
                            placements[i].SetValue(j + 1, k + 1, plant.Id);
                        }
                        
                    }
                }
            }
        }

        private void UpdateErosion(Plant plant, Point position)
        {
            ComputeErodes();
        }


        #endregion

        #region Other
        //Compute min and max dim of the plant list
        public void ComputeDimInfos(List<Plant> plants)
        {
            var maxDim = 0;
            var minDim = 0;
            var dimCount = 0;
            /*foreach (var plant in plants)
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
            }*/
            DimCount = (uint)dimCount;
            MinDim = minDim;
            MaxDim = (uint)maxDim;
        }
        #endregion
    }
}

