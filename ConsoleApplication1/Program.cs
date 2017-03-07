using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Test1();
            Console.WriteLine();
            Console.WriteLine();
            Console.ReadLine();
        }

        public static void Test1()
        {
            //SoilMap
            var soilMap = new Mat(new Size(500, 500), DepthType.Cv8U, 1);
            soilMap.SetTo(new MCvScalar(0));
            for (int i = 1; i < 25; i++)
            {
                for (int j = 1; j < 25; j++)
                {
                    soilMap.SetValue(i, j, (byte)255);
                }
            }

            //Garden
            var garden = new Garden(soilMap);

            //Plants
            //p1
            var model1 = new List<KeyValuePair<int, Mat>>();
            var k = 1;
            var plantMap1 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(2 * k + 1, 2 * k + 1), new Point(k, k));
            model1.Add(new KeyValuePair<int, Mat>(0, plantMap1));
            model1.Add(new KeyValuePair<int, Mat>(-1, plantMap1));
            model1.Add(new KeyValuePair<int, Mat>(1, plantMap1));
            model1 = model1.OrderBy(o => o.Key).ToList();
            var plant1 = new Plant(1, model1);

            //p2
            var model2 = new List<KeyValuePair<int, Mat>>();
            k = 2;
            var plantMap2 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(2 * k + 1, 2 * k + 1), new Point(k, k));
            model2.Add(new KeyValuePair<int, Mat>(0, plantMap2));
            model2.Add(new KeyValuePair<int, Mat>(-1, plantMap2));
            model2.Add(new KeyValuePair<int, Mat>(1, plantMap2));
            model2 = model2.OrderBy(o => o.Key).ToList();

            var plant2 = new Plant(2, model2);

            var plantList = new PlantList() { plant1, plant2 };

            //Packing
            var packing = new Packing(plantList, garden);

        }

        public static void StructuringElement()
        {
            var k = 2;
            Mat mat = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(2 * k + 1, 2 * k + 1),
                new Point(k, k));
            PrintInWindows("ji^nijpnjpi", mat);
        }


        public static void PrintInWindows(string name, Mat mat)
        {
            CvInvoke.NamedWindow(name);
            CvInvoke.Imshow(name, mat);
            CvInvoke.WaitKey(0);
            CvInvoke.DestroyWindow(name);
        }


        public void GenerrateRect()
        {
            var win1 = "Test Window";
            CvInvoke.NamedWindow(win1);

            Mat img = new Mat(100, 100, DepthType.Cv8U, 1);
            img.SetTo(new MCvScalar(0));


            for (int k = 0; k < 100; k++)
            {
                for (int i = 0; i < k; i++)
                {
                    for (int j = 0; j < 100; j++)
                    {
                        img.SetValue(i, j, (byte)255);
                    }
                    CvInvoke.Imwrite("C:\\jj\\img_" + k + " " + i + ".jpg", img);
                }
                img.SetTo(new MCvScalar(0));
            }


            CvInvoke.Imshow(win1, img);
            CvInvoke.WaitKey(0);
            CvInvoke.DestroyWindow(win1);
        }
    }
}