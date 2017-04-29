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

        #endregion

        #region ctor
        public Erosion(Plant plant, Garden garden)
        {
            //Create erode map
            ErodePoints = new List<Point>();
            ErodeMap = new Mat(garden.SoilMap.Size, DepthType.Cv8U, 10);
            ErodeMap.SetTo(new MCvScalar(0));

            for (int i = 0; i < 9; i++)
            {
                
            }

            //CvInvoke.Erode(garden.SoilMap, ErodeMap,
            //    , new Point(1, 1), 1,
            //    BorderType.Constant, new MCvScalar(0));


        }
    }
    #endregion
}
