using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace NewtoneRafsonAlg
{
    class Program
    {
        private static int n = 2;
        private static double h = 0.4;
        private static double E = 0.1;
        private static List<double[]> x = new List<double[]>();
        private static List<double> f = new List<double>();
        private static double[] P;

        private static double[,] matrix;

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
            if (f.Count > 5)
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

            //result[0] = 6 * xc[0] - xc[1] - 8; result[1] = xc[0] + 6 * xc[1];

            result[0] = 2.5 * 2 * xc[0] - 3.1 * xc[1] - 5.1; result[1] = -3.1 * xc[0] + 2 * xc[1];

            return result;

        }

        private static double[] getSecond()
        {
            double[] result = new double[n];
            //result[0] = 2; result[1] = 6 ;
            //result[0] = 6; result[1] = 6;
            result[0] = 2.5 * 2; result[1] = 2;

            return result;
        }

        private static double[] getMash()
        {
            double[] result = new double[n];
            //result[0] = -2 ; result[1] = -1;
            //result[0] = -1; result[1] = 1;
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


        static void CreateMatrix()
        {

            matrix = new double[n, n];

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
            Console.WriteLine("Matrix");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        static bool IsOptimal(double[,] matr)
        {
            Matrix<double> a = Matrix<double>.Build.DenseOfArray(matr);

            if (matr[0, 0] > 0 && a.Determinant() > 0)
            {
                return true;
            }
            return false;
        }

        private static double Deteminant(double[,] matr)
        {
            return matr[1, 1] * matr[2, 2] - matr[1, 2] * matr[2, 1];
        }

        static void Main(string[] args)
        {
            P = new double[n];
            
            x.Add(new double[] { -1, -1 });


            f.Add(CalculateFunction(x.Last()));
            DrawX();
            DrawF();



            for (int i = 0; i < 210; i++)
            {
                Console.WriteLine("\n\n___________________________________________________");
                Console.WriteLine("\nIt = " + i);


                if (IsEnd())
                {
                    Console.WriteLine("Iteration " + i);
                    foreach (var item in x.Last())
                    {
                        Console.WriteLine("x = " + item);
                    }
                    Console.WriteLine("Fmin = " + f.Last());
                    Console.ReadLine();
                    return;
                }

                CreateMatrix();
                Exec(IsOptimal(matrix));
                


            }
            Console.ReadLine();
        }

        private static void CalcH()
        {
            
            double[] temp = new double[n];
            for (int i = 0; i < n; i++)
            {
                temp[i] = 0;
                for (int j = 0; j < n; j++)
                {
                    temp[i] += matrix[j, i] * P[j];
                }
            }

            double sum = 0;
            for (int i = 0; i < n; i++)
            {

                sum += CalculateGradient(x.Last())[i] * P[i];

            }

            double sum2 = 0;
            for (int i = 0; i < n; i++)
            {

                sum2 += temp[i] * P[i];

            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine("Sum 1 = " + sum);
            Console.WriteLine("Sum 2 = " + sum2);
            Console.WriteLine();


            h = sum / sum2;
            Console.WriteLine("H = " + h);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;

            
        }

        private static void CalcH(double[,] m)
        {
            double[] temp = new double[n];
            for (int i = 0; i < n; i++)
            {
                temp[i] = 0;
                for (int j = 0; j < n; j++)
                {
                    temp[i] += m[j, i] * CalculateGradient(x.Last())[j];
                }
            }

            double sum = 0;
            for (int i = 0; i < n; i++)
            {

                sum += CalculateGradient(x.Last())[i] * CalculateGradient(x.Last())[i];

            }

            double sum2 = 0;
            for (int i = 0; i < n; i++)
            {

                sum2 += temp[i] * CalculateGradient(x.Last())[i];

            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("E-Matrix");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(m[i, j] + " ");
                }
                Console.WriteLine();
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

        private static void Exec(bool opt)
        {
            if (opt)
            {
                Console.WriteLine("-----------------------------------");
                Console.ForegroundColor = ConsoleColor.Red;
                Vector<double> xk = Vector<double>.Build.DenseOfArray(x.Last());
                Vector<double> gr = Vector<double>.Build.DenseOfArray(CalculateGradient(x.Last()));
                Matrix<double> m = Matrix<double>.Build.DenseOfArray(matrix);
                var invMatr = m.Inverse();
                Console.WriteLine(m);
                Console.WriteLine("Inverse");
                Console.WriteLine(invMatr);
                Console.ForegroundColor = ConsoleColor.White;
                var Pv = invMatr.Multiply(gr);
                P = Pv.ToArray();
                Console.WriteLine(invMatr);
                Console.WriteLine("Multyply\n");
                Console.WriteLine(gr);
                Console.WriteLine("Equals\n");
                Console.WriteLine(Pv);
                CalcH();
                var res = xk - (Pv.Multiply(h));
                Console.WriteLine(xk);
                Console.WriteLine("Minus\n");
                Console.WriteLine(Pv);
                Console.WriteLine("Equals\n");
                Console.WriteLine(res);
                Console.WriteLine("-----------------------------------");
                x.Add(res.ToArray<double>());
                f.Add(CalculateFunction(x.Last()));

                Console.ForegroundColor = ConsoleColor.Blue;
                DrawX();
                DrawF();
                Console.ForegroundColor = ConsoleColor.White;

            }
            else
            {
                Console.WriteLine("-----------------------------------");
                Console.ForegroundColor = ConsoleColor.Red;
                Vector<double> xk = Vector<double>.Build.DenseOfArray(x.Last());
                Vector<double> gr = Vector<double>.Build.DenseOfArray(CalculateGradient(x.Last()));
                Matrix<double> m = Matrix<double>.Build.DenseOfArray(matrix);
                var invMatr = m.Inverse();
                Console.WriteLine(m);
                Console.WriteLine("Inverse");
                Console.WriteLine(invMatr);
                Console.ForegroundColor = ConsoleColor.White;
                var Pv = invMatr.Multiply(gr);
                P = Pv.ToArray();
                Console.WriteLine(invMatr);
                Console.WriteLine("Multyply\n");
                Console.WriteLine(gr);
                Console.WriteLine("Equals\n");
                Console.WriteLine(Pv);
                CalcH(new double[,] { { 1, 0 }, { 0, 1 } });
                var res = xk - (Pv.Multiply(h));
                Console.WriteLine(xk);
                Console.WriteLine("Minus\n");
                Console.WriteLine(Pv);
                Console.WriteLine("Equals\n");
                Console.WriteLine(res);
                Console.WriteLine("-----------------------------------");
                x.Add(res.ToArray<double>());
                f.Add(CalculateFunction(x.Last()));

                Console.ForegroundColor = ConsoleColor.Blue;
                DrawX();
                DrawF();
                Console.ForegroundColor = ConsoleColor.White;
            }

            
        }

    }
}


