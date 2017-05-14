using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //SoilMap
            var soilMap = new Mat(new Size(50, 50), DepthType.Cv8U, 1);
            soilMap.SetTo(new MCvScalar(0));
            for (var i = 3; i < 13; i++)
            {
                for (var j = 3; j < 13; j++)
                {
                    soilMap.SetValue(i, j, (byte)255);
                }
            }

            //Garden
            var garden = new Garden(soilMap);

            //Plants 2147483647
            var plant0 = new Plant { Id = 2147483640 / 16, Model = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };
            var plant00 = new Plant { Id = 2147483640 / 16, Model = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };
            var plant000 = new Plant { Id = 2147483640 / 16, Model = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };
            var plant0000 = new Plant { Id = 2147483640 / 16, Model = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };
            var plant00000 = new Plant { Id = 2147483640 / 16, Model = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };

            var plant1 = new Plant { Id = 2147483640/2, Model = new [] { 1, 1 , 1, 1, 1, 1, 1, 1, 1, 1 } };
            var plant1d = new Plant { Id = 2147483640 / 2, Model = new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } };
            var plant1dd = new Plant { Id = 2147483640 / 2, Model = new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } };
            var plant1ddd = new Plant { Id = 2147483640 / 2, Model = new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } };
            var plant2 = new Plant { Id = 2147483640/4, Model = new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } };
            var plantd2 = new Plant { Id = 2147483640 / 4, Model = new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } };
            var plant3 = new Plant { Id = 2147483640 / 8, Model = new[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 } };
            var plantd3 = new Plant { Id = 2147483640 / 8, Model = new[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 } };
            var plantdd3 = new Plant { Id = 2147483640 / 8, Model = new[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 } };
            var plantddd3 = new Plant { Id = 2147483640 / 8, Model = new[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 } };

            //Packing
            var packing = new Packing(new List<Plant>
            {
                plantd3,
                plantdd3,
                plant3,
                plant1,
                plant2,
                plantd2,
                plant1d,
                plant1ddd,
                plant0,
                plant00,
                plant000,
                plant0000,
                plant00000
            }, garden);

            stopWatch.Stop();

            var aa = new Mat(garden.SoilMap.Size, DepthType.Cv8U, 1);
            garden.SoilMap.CopyTo(aa);
            foreach (var posit in packing.FinalNode.Positions)
            {
                for (var i = posit.Value.X - posit.Key.Model[0]; i < posit.Value.X - posit.Key.Model[0] + posit.Key.Model[0]*2 + 1; i++)
                {
                    for (var j = posit.Value.Y - posit.Key.Model[0]; j < posit.Value.Y - posit.Key.Model[0] + posit.Key.Model[0] * 2 + 1; j++)
                    {
                        aa.SetValue(i, j, (byte) (posit.Key.Id/255));
                    }
                }
            }

            CvInvoke.Imwrite("C:\\jj\\img.bmp", aa);

            Console.WriteLine(stopWatch.ElapsedMilliseconds + "ms");
            Console.WriteLine(stopWatch.ElapsedMilliseconds/1000 + "s");

            PrintInWindows("name", aa);
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