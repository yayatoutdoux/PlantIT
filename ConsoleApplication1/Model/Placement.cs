using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace ConsoleApplication1
{
    public class Placement : IDisposable
    {
        private Placement placement;

        public Mat[] Placements { get; set; }

        public Placement(Garden garden)
        {
            //Create 10 channel Mat
            Placements = new Mat[Constants.SoilLayerCount];

            for (var i = 0; i < Constants.SoilLayerCount; i++)
            {
                Placements[i] = new Mat(
                    garden.SoilMap.Size,
                    DepthType.Cv32S,
                    1
                );

                if (i == 0)
                {
                    Placements[i].SetTo(new MCvScalar(int.MaxValue));
                    continue;
                }
                Placements[i].SetTo(new MCvScalar(0));

                for (var j = 0; j < Placements[i].Height; j++)
                {
                    for (var k = 0; k < Placements[i].Width; k++)
                    {
                        if (garden.SoilMap.GetValue(j, k) == (byte)255)
                        {
                            Placements[i].SetValue(j, k, int.MaxValue);
                        }
                    }
                }
                CvInvoke.BitwiseAnd(Placements[i], Placements[0], Placements[0]);
            }
        }

        public Placement(Placement placement)
        {
            Placements = new Mat[Constants.SoilLayerCount];
            for (var i = 0; i < Constants.SoilLayerCount; i++)
            {
                Placements[i] = new Mat(
                    placement.Placements[i].Size,
                    DepthType.Cv32S,
                    1
                );
                placement.Placements[i].CopyTo(Placements[i]);
            }
        }

        public void Dispose()
        {
            foreach (var plc in Placements)
            {
                plc.Dispose();
            }
        }
    }
}