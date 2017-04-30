using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace ConsoleApplication1
{
    public class Erosion
    {
        #region properties
        public Mat[] ErodeMap { get; set; }
        public List<Point> ErodePoints { get; set; }
        public int Size { get; set; } = 0;
        #endregion

        #region ctor
        public Erosion(Plant plant, Mat[] placements)
        {
            //Create erode map
            ErodePoints = new List<Point>();
            ErodeMap = new Mat(
                placement.Size,
                DepthType.Cv8U,
                Constants.SoilLayerCount
            );
            var erosions = new VectorOfMat();
            CvInvoke.Split(ErodeMap, erosions);

            var placements = new VectorOfMat();
            CvInvoke.Split(placement, placements);

            for (var i = 0; i < erosions.Size; i++)
            {
                erosions[i].SetTo(new MCvScalar(0));
                for (var j = 0; j < erosions[i].Height; j++)
                {
                    for (var k = 0; k < erosions[i].Width; k++)
                    {
                        if (placements[i].GetValue(j, k) == int.MaxValue)
                        {
                            erosions[i].SetValue(j, k, (byte)255);
                        }
                    }
                }
                CvInvoke.Erode(erosions[i], erosions[i], plant.Model.First()
                    , new Point(1, 1), 1,
                    BorderType.Constant, new MCvScalar(0));
            }
            CvInvoke.Merge(erosions, ErodeMap);
        }
    }
    #endregion
}
