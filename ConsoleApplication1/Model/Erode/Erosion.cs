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
        //public List<Point> ErodePoints { get; set; }
        public int Size { get; set; } = 0;
        #endregion

        #region ctor
        public Erosion(Plant plant, Mat[] placements)
        {
            ErodeMap = new Mat[Constants.SoilLayerCount];
            //Create erode map
            
            for (var i = 0; i < ErodeMap.Length; i++)
            {
                ErodeMap[i] = new Mat(
                    placements[0].Size,
                    DepthType.Cv8U,
                    1
                );
                ErodeMap[i].SetTo(new MCvScalar(0));

                for (var j = 0; j < ErodeMap[i].Height; j++)
                {
                    for (var k = 0; k < ErodeMap[i].Width; k++)
                    {
                        if (placements[i].GetValue(j, k) == int.MaxValue)
                        {
                            ErodeMap[i].SetValue(j, k, (byte)255);
                        }
                    }
                }
                CvInvoke.Erode(ErodeMap[i], ErodeMap[i], plant.Model.First()
                    , new Point(1, 1), 1,
                    BorderType.Constant, new MCvScalar(0));
            }
        }
    }
    #endregion
}
