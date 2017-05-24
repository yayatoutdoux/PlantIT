using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
        }

        public static void Test1()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //SoilMap
            var soilMap = new Mat(new Size(50, 50), DepthType.Cv8U, 1);
            soilMap.SetTo(new MCvScalar(0));
            for (var i = 3; i < 17; i++)
            {
                for (var j = 3; j < 17; j++)
                {
                    soilMap.SetValue(i, j, (byte)255);
                }
            }

            //Garden
            var garden = new Garden(soilMap);

            //Interaction
            var inter = new Interaction(1, true, new int?[] { 1,0,0,0, 0}, 4);
            var inter2 = new Interaction(1, false, new int?[] { 1,0,0,0, 0}, 4);

            //Plants 2147483647
            var plant0 = new Plant { Id = 2147483640 / 16+ 1, Model = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, Interactions = new List<Interaction>() { inter } };
            var plant00 = new Plant { Id = 2147483640 / 16+6, Model = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, Interactions = new List<Interaction>() { inter } };
            var plant000 = new Plant { Id = 2147483640 / 16+44, Model = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, Interactions = new List<Interaction>() { inter } };
            var plant0000 = new Plant { Id = 2147483640 / 16+12, Model = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, Interactions = new List<Interaction>() { inter } };
            var plant00000 = new Plant { Id = 2147483640 / 16, Model = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, Interactions = new List<Interaction>() { inter } };
            var plant1 = new Plant { Id = 2147483640/2 + 6, Model = new [] { 1, 1 , 1, 1, 1, 1, 1, 1, 1, 1 }, Interactions = new List<Interaction>() { inter } };
            var plant1d = new Plant { Id = 2147483640 / 2, Model = new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, Interactions = new List<Interaction>() { inter } };
            var plant1dd = new Plant { Id = 2147483640 / 2 + 1, Model = new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, Interactions = new List<Interaction>() { inter } };
            var plant1ddd = new Plant { Id = 2147483640 / 2 + 2, Model = new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, Interactions = new List<Interaction>() { inter } };
            var plant2 = new Plant { Id = 2147483640/4, Model = new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, Interactions = new List<Interaction>() { inter } };
            var plantd2 = new Plant { Id = 2147483640 / 4, Model = new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, Interactions = new List<Interaction>() { inter } };
            var plant3 = new Plant { Id = 2147483640 / 8 + 1, Model = new[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 }, Interactions = new List<Interaction>() { inter2 } };
            var plantd3 = new Plant { Id = 2147483640 / 8 + 2, Model = new[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 }, Interactions = new List<Interaction>() { inter2 } };
            var plantdd3 = new Plant { Id = 2147483640 / 8, Model = new[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 }, Interactions = new List<Interaction>() { inter2 } };
            var plantddd3 = new Plant { Id = 2147483640 / 8 + 3, Model = new[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 }, Interactions = new List<Interaction>() { inter2 } };

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
            Console.WriteLine(stopWatch.ElapsedMilliseconds + "ms");
            Console.WriteLine(stopWatch.ElapsedMilliseconds / 1000 + "s");

            var aa = packing.FinalNode.GetPositionMat();
            CvInvoke.Imwrite("C:\\jj\\img" + packing.FinalNode.GetInteractionScore() + ".bmp", aa);

            aa = packing.FinalNodeInter.GetPositionMat();
            CvInvoke.Imwrite("C:\\jj\\img2" + packing.FinalNodeInter.GetInteractionScore() + ".bmp", aa);
        }


        public static void PrintInWindows(string name, Mat mat)
        {
            CvInvoke.NamedWindow(name);
            CvInvoke.Imshow(name, mat);
            CvInvoke.WaitKey(0);
            CvInvoke.DestroyWindow(name);
        }
    }
}