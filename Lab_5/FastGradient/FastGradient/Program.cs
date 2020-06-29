﻿using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;



namespace FastGradient
{
    class Program
    {
        private static int n = 2;
        private static double h = 0.4;
        private static double E = 0.1;
        private static List<double[]> x = new List<double[]>();
        private static List<double> f = new List<double>();

        static void DrawX()
        {
            Console.WriteLine("______Xs_______");
            List<double[]> temp = new List<double[]>();
            if (x.Count > 5)
            {
                for (int i = x.Count - 5; i < x.Count; i++)
                {
                    temp.Add(x[i]);
                }
            }
            else
            {
                temp = x;
            }
            foreach (var item in temp)
            {
                Console.WriteLine();
                foreach (var i in item)
                {
                    Console.WriteLine(i);
                }

            }
            Console.WriteLine("_______________");
        }

        static void DrawF()
        {
            Console.WriteLine("______Fs_______");
            List<double> temp = new List<double>();
            if (x.Count > 5)
            {
                for (int i = f.Count - 5; i < f.Count; i++)
                {
                    temp.Add(f[i]);
                }
            }
            else
            {
                temp = f;
            }
            foreach (var item in temp)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("_______________");
        }

        private static double CalculateFunction(double[] x)
        {
            //return x[0] * x[0] - x[0] * x[1] + 3 * x[1] * x[1] - x[0];

            //return 3 * x[0] * x[0] + x[0] * x[1] + 3 * x[1] * x[1] - 8 * x[0];
            
            return 2.5 * Math.Pow(x[0], 2) - 3.1 * x[0] * x[1] + Math.Pow(x[1], 2) - 5.1 * x[0];
        }
        
        private static double[] CalculateGradient(double[] xc)
        {
            double[] result = new double[n];

            //result[0] = 2 * xc[0] - xc[1] - 1; result[1] = -xc[0] + 6 * xc[1];

            //result[0] = 6 * xc[0] + xc[1] - 8; result[1] = xc[0] + 6 * xc[1];

            result[0] = 2.5 * 2 * xc[0] - 3.1 * xc[1] - 5.1; result[1] = -3.1 * xc[0] + 2 * xc[1];
            
            return result;

        }

        private static double[] getSecond()
        {
            double[] result = new double[n];
            //result[0] = 2; result[1] = 6 ;
            //result[0] = 6; result[1] = 6;
            result[0] = 2.5*2; result[1] = 2;

            return result;
        }

        private static double[] getMash()
        {
            double[] result = new double[n];
            //result[0] = -2 ; result[1] = -1;
            //result[0] = 1; result[1] = 1;
            result[0] = -3.1; result[1] = -3.1;

            return result;

        }


        private static double[] NextPoint(double[] xc)
        {
            double[] result = new double[n];
            foreach (var item in CalculateGradient(xc))
            {
                Console.WriteLine("Gr = " + item);
            }
            Console.WriteLine();
            for (int i = 0; i < n; i++)
            {
                result[i] = x.Last()[i] - h * CalculateGradient(xc)[i];
            }
            return result;
        }

        private static bool IsEnd()
        {
            double sum = 0;
            for (int i = 0; i < n; i++)
            {

                sum += Math.Pow(CalculateGradient(x.Last())[i], 2);

            }
            foreach (var item in CalculateGradient(x.Last()))
            {
                Console.WriteLine("NewGR = " + item);
            }
            Console.WriteLine(Math.Sqrt(sum) + " < " + E + " = " + (Math.Sqrt(sum) < E));
            if (Math.Sqrt(sum) <= E)
                return true;

            return false;
        }


        static void Main(string[] args)
        {

            x.Add(new double[] { -1, -1 });

            f.Add(CalculateFunction(x[0]));

            DrawX();
            DrawF();

            if (IsEnd())
            {
                Console.WriteLine("Iteration " + 0);
                foreach (var item in x.Last())
                {
                    Console.WriteLine("x = " + item);
                }
                Console.WriteLine("Fmin = " + f.Last());
                Console.ReadLine();
                return;
            }

            
            for (int i = 0; i < 3200; i++)
            {
                Console.WriteLine("It = " + i);
                //Console.ReadLine();
                CalcH();


                x.Add(NextPoint(x.Last()));

                f.Add(CalculateFunction(x.Last()));

                DrawX();
                DrawF();

                Console.WriteLine(f[f.Count - 2] + " > " + f.Last() + " = " + (f[f.Count - 2] > f.Last()));
                if (f[f.Count - 2] > f.Last())
                {

                    if (IsEnd())
                    {
                        Console.WriteLine("Iteration " + i);
                        foreach (var item in x.Last())
                        {
                            Console.WriteLine("x = " + item);
                        }
                        Console.WriteLine("Fmin = " + f.Last());
                        break;
                    }
                    else
                    {
                        continue;
                    }

                }
                else
                {
                    CalcH();
                }
            }

            Console.ReadLine();

        }

        private static void CalcH()
        {
            double[,] matrix = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                    {

                        matrix[i, j] = getMash()[i];

                    }
                    else
                    {
                        matrix[i, j] = getSecond()[i];
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine( "Matrix");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(matrix[i,j]+ " ");
                }
                Console.WriteLine();
            }
            

            double[] temp = new double[n];
            for (int i = 0; i < n; i++)
            {
                temp[i] = 0;
                for (int j = 0; j < n; j++)
                {
                    temp[i] += matrix[j, i] * CalculateGradient(x.Last())[j];
                }
            }
            double sum2 = 0;
            for (int i = 0; i < n; i++)
            {

                sum2 += temp[i] * CalculateGradient(x.Last())[i];

            }
            

            double sum = 0;
            for (int i = 0; i < n; i++)
            {

                sum += CalculateGradient(x.Last())[i] * CalculateGradient(x.Last())[i];

            }
            Console.WriteLine();
            Console.WriteLine("Sum 1 = " + sum);
            Console.WriteLine("Sum 2 = " + sum2);
            Console.WriteLine();
            

            h = sum / sum2;
            Console.WriteLine("H = " + h);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

        }
    }
}

