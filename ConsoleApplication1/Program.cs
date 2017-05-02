﻿using System;
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
            for (var i = 1; i < 12; i++)
            {
                for (var j = 1; j < 12; j++)
                {
                    soilMap.SetValue(i, j, (byte)255);
                }
            }

            //Garden
            var garden = new Garden(soilMap);


            //Plants
            //p1 2147483647
            //var k = 1;
            //var plantMap1 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(2 * k + 1, 2 * k + 1), new Point(k, k));
            var plant1 = new Plant {
                Id = 2147483640/2,
                Model = new [] { 1, 1 , 1, 1, 1, 1, 1, 1, 1, 1 }
            };

            //p2
            //k = 1;
            //var plantMap2 = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(2 * k + 1, 2 * k + 1), new Point(k, k));
            var plant2 = new Plant
            {
                Id = 2147483640/4,
                Model = new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
            };
            //Packing
            var packing = new Packing(new List<Plant> { plant1, plant2 }, garden);

            var aa = new Mat();
            //PrintInWindows("name", packing.FinalNode.Erosions.First().Value.);
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