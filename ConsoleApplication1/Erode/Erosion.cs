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
        public Mat ErodeMap { get; set; }
        public uint Size { get; set; } = 0;
        public List<KeyValuePair<int, Mat>> Erode3D { get; set; }



        public Erosion(PlantList plantList, Placement placement)
        {

        }

        public Erosion(Plant plant, Garden garden)
        {
            Erode3D = new List<KeyValuePair<int, Mat>>();
            ErodeMap = new Mat(garden.SoilMap.Rows, garden.SoilMap.Cols, DepthType.Cv8U, 1);
            ErodeMap.SetTo(new MCvScalar(255));
            foreach (var level in garden.Model)
            {
                Mat erode = new Mat();
                level.Value.CopyTo(erode);
                CvInvoke.Erode(level.Value, erode, plant.Model.Where(x => x.Key == level.Key).Select(x => x.Value).FirstOrDefault(), new Point(1, 1), 1, BorderType.Constant, new MCvScalar(0));

                Erode3D.Add(new KeyValuePair<int, Mat>(level.Key, erode));
            }

            foreach (var erode in Erode3D)
            {
                CvInvoke.BitwiseAnd(ErodeMap, erode.Value, ErodeMap);
                Size = ErodeMap.GetValueCount((byte)255);
            }
        }
    }
}
