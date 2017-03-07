using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace ConsoleApplication1
{

    public class Erosion
    {
        #region properties
        //0 : no placable, 255 : placable, not evalued, 100 - 200 plants ids
        public Mat ErodeMap { get; set; }
        public List<Point> ErodePoints { get; set; }
        public int Size { get; set; } = 0;
        public List<KeyValuePair<int, Mat>> Erode3D { get; set; }

        #endregion

        #region ctor

        public Erosion(PlantList plantList, Placement placement)
        {

        }

        public Erosion(Plant plant, Garden garden)
        {
            Erode3D = new List<KeyValuePair<int, Mat>>();
            ErodePoints = new List<Point>();
            ErodeMap = new Mat(garden.SoilMap.Rows, garden.SoilMap.Cols, DepthType.Cv8U, 1);
            ErodeMap.SetTo(new MCvScalar(255));

            foreach (var level in garden.Model)
            {
                var erode = new Mat(
                    new Size(level.Value.Cols, level.Value.Rows), 
                    DepthType.Cv8U, 
                    1
                );
                erode.SetTo(new MCvScalar(0));
                
                //tresh

                CvInvoke.Erode(level.Value, erode,
                    plant.Model.Where(x => x.Key == level.Key).Select(x => x.Value).FirstOrDefault(), new Point(1, 1), 1,
                    BorderType.Constant, new MCvScalar(0));

                //Compute points
                for (int i = 0; i < erode.Cols; i++)
                {
                    for (int j = 0; j < erode.Rows; j++)
                    {
                        if (erode.GetValue(i, j) == 255)
                            ErodePoints.Add(new Point(i, j));
                    }
                }

                Erode3D.Add(new KeyValuePair<int, Mat>(level.Key, erode));
            }

            foreach (var erode in Erode3D)
            {
                CvInvoke.BitwiseAnd(ErodeMap, erode.Value, ErodeMap);
            }

        }
    }
    #endregion
}
