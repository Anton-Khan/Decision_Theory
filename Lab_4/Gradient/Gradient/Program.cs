using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradient
{
    class Program
    {
        private static int n = 2;
        private static double h = 0.4;
        private static double E = 0.1;
        private static List<double[]> x = new List<double[]>();
        private static List<double> f = new List<double>();


        int iter = 0;
        private static double CalculateFunction(double[] x)
        {
            //return x[0] * x[0] - x[0] * x[1] + 3 * x[1] * x[1] - x[0];

            //return 3 * x[0] * x[0] + x[0] * x[1] + 3 * x[1] * x[1] - 8 * x[0];

            //return Math.Pow(x[0], 4) + Math.Pow(x[0], 2) * x[1] - 6 * Math.Pow(x[0], 2) - 1.2 * x[0] * x[1] + Math.Pow(x[1], 2);

            //return 2 * Math.Pow(x[0], 4) + Math.Pow(x[1], 4) + Math.Pow(x[0], 2) * x[1] - 5 * x[0] * x[1] + 3 * Math.Pow(x[0], 2) + 8 * x[1];

            //return 4 * Math.Pow(x[0], 2) + 4 * Math.Pow(x[1], 2) + Math.Pow(x[2], 2) - 2 * x[0] * x[2] - 5 * x[0] * x[1] - 8 * x[2];

            return 2.5 * Math.Pow(x[0], 2) - 3.1 * x[0] * x[1] + Math.Pow(x[1], 2) - 5.1 * x[0];
        }

        private static double[] CalculateGradient(double[] x)
        {
            double[] result = new double[n];
            //result[0] = 2 * x[0] - x[1] - 1; result[1] = -x[0] + 6 * x[1];

            //result[0] = 6 * x[0] - x[1] - 8; result[1] = x[0] + 6 * x[1];
            result[0] = 2.5 * 2 * x[0] - 3.1 * x[1] - 5.1; result[1] = -3.1 * x[0] + 2 * x[1];


            return result;

        }

        private static double[] NextPoint(double [] xc)
        {
            double[] result = new double[n];
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

            if (Math.Sqrt(sum) <= E)
                return true;

            return false;
        }


        static void Main(string[] args)
        {

            x.Add(new double[n]);
            x[0][0] = -1;
            x[0][1] = -1;
            f.Add(CalculateFunction(x[0]));


            for (int i = 0; i < 1200; i++)
            {
                x.Add(NextPoint(x.Last()));
                f.Add(CalculateFunction(x.Last()));

                if(f[f.Count-2] > f.Last())
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
                    h = h / 2.0;
                }
            }

            Console.ReadLine();
        }
    }
}
