//using Mathic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extreme.Mathematics;
namespace TestMatriks
{
    class Program
    {
        static void Main(string[] args)
        {
            //double[,] x =
            //{
            //    { 1,1,1},
            //    { 1,0,1},
            //    { 1,1,0},
            //    { 1,1,0},
            //    { 0,1,0},
            //    { 0,0,0},
            //    { 0,1,0},
            //    { 1,1,0},
            //    { 0,0,0}

            //};

            //double[] y =
            //{
            //    1,1,1,2,2,2,3,3,3
            //};
            //double[,] weight =
            //{
            //    {-0.4,0.2,0.1 },
            //    {-0.2,0,0.4},
            //    { -0.3,0.3,-0.1}
            //};
            //var hInit = x.Multiply(weight.Transpose());
            //var _h = new double[hInit.GetLength(0), hInit.GetLength(1)];
            //Parallel.For(0, _h.GetLength(0), i =>
            //{
            //    Parallel.For(0, _h.GetLength(1), j =>
            //    {
            //        _h[i, j] = 1 / (1 + Math.Exp(-hInit[i, j]));
            //    });
            //});
            //var watch = new Stopwatch();
            //watch.Start();
            //var _hTrans = _h.TransposeParallel();
            //var hPlus = _hTrans.MultiplyParallel(_h).Inverse().MultiplyParallel(_hTrans);
            //Console.WriteLine(watch.ElapsedMilliseconds);
            //watch.Restart();
            //var hPlus2 = (_h.Transpose().Multiply(_h)).Inverse().Multiply(_h.Transpose());
            //Console.WriteLine(watch.ElapsedMilliseconds);
            //watch.Stop();
            //var betaHatt = hPlus.Dot(y);
            //var b = a;

            //double[,] a =
            //{
            //    {1,1,4,5,6},
            //    {4,3,0,0,6},
            //    { 7,8,9,8,7},
            //    { 6,5,4,3,2},
            //    {1,2,3,5,6 }

            //};
            DateTime now = DateTime.Now;
            double[,] a;

            // 
            // 'Expert' interface, spdmatrixinverse()
            // 

            // only upper triangle is used; a[1][0] is initialized by NAN,
            // but it can be arbitrary number
            a = new double[1000, 1000];
            Random ran = new Random();
            Parallel.For(0, a.GetLength(0), i =>
            {
                Parallel.For(0, a.GetLength(1), j =>
                {
                    var temp = 0.0;
                    temp = ran.NextDouble() * (200 - 100) + 100;
                    a[i, j] = temp;
                });
            });
            var b = a;
            // after this line A will contain [[0.66,-0.33],[NAN,0.66]]
            // only upper triangle is modified
            Matrix c = 
            Console.WriteLine("Found inverse in " + DateTime.Now.Subtract(now).TotalSeconds.ToString() + " seconds");
            Console.ReadKey();
            //var watch = new Stopwatch();
            //watch.Start();
            //var temp = a.Inverse();
            //Console.WriteLine(watch.ElapsedMilliseconds);
            //watch.Restart();
            //var temp1 = a.Inverse();
            //Console.WriteLine(watch.ElapsedMilliseconds);
            ////var c = a.MultiplyParallel(b);
            ////var d = a.MultiplyParallel(b);
            ////for (int i = 0; i < temp.GetLength(0); i++)
            ////{
            ////    for (int j = 0; j < temp.GetLength(1); j++)
            ////    {
            ////        Console.Write(temp[i, j] + ", ");
            ////    }
            ////    Console.WriteLine();
            ////}
            ////Console.WriteLine();
            ////for (int i = 0; i < temp.GetLength(0); i++)
            ////{
            ////    for (int j = 0; j < temp.GetLength(1); j++)
            ////    {
            ////        Console.Write(temp1[i, j] + ", ");
            ////    }
            ////    Console.WriteLine();
            ////}
            //int sw = 0;
            //for(int i = 0; i < temp.GetLength(0); i++)
            //{
            //    for(int j = 0; j < temp.GetLength(1); j++)
            //    {
            //        if(temp[i,j]== temp1[i, j])
            //        {
            //            sw++;
            //        }
            //    }
            //}
            //if (sw == temp.Length)
            //{
            //    Console.WriteLine("DONE");
            //}
            Console.WriteLine("Finish");
            Console.ReadKey();
        }
    }
}
