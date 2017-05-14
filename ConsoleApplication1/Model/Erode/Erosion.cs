using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace ConsoleApplication1
{
    public class Erosion
    {
        #region properties
        public List<Point> ErodePoints { get; set; }
        #endregion

        #region ctor
        public Erosion(Plant plant, Garden garden)
        {
            ErodePoints = new List<Point>();

            var erodeMap = new Mat(garden.SoilMap.Size, DepthType.Cv8U, 1 );

            erodeMap.SetTo(new MCvScalar(0));
 
            for (var j = 0; j < erodeMap.Height; j++)
            {
                for (var k = 0; k < erodeMap.Width; k++)
                {
                    if (garden.SoilMap.GetValue(j, k) != 0)
                    {
                        erodeMap.SetValue(j, k, (byte)255);
                    }
                }
            }
            var structuringElement = 
                CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(2 * plant.Model[0] + 1, 2 * plant.Model[0] + 1), new Point(plant.Model[0], plant.Model[0]));

            CvInvoke.Erode(erodeMap, erodeMap, structuringElement
                , new Point(plant.Model[0], plant.Model[0]), 1,
                BorderType.Constant, new MCvScalar(0));
           
            
            for (var j = 0; j < erodeMap.Height; j++)
            {
                for (var k = 0; k < erodeMap.Width; k++)
                {
                    if (erodeMap.GetValue(j, k) != 0)
                    {
                        ErodePoints.Add(new Point(j, k));
                    }
                }
            }
        }

        public Erosion(Erosion erosion)
        {
            ErodePoints = new List<Point>(erosion.ErodePoints);
        }
    }
    #endregion
}
