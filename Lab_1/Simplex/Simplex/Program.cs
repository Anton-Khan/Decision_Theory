using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplex
{
    


    class Program
    {
        private static int n = 2;
        private static double m = 0.25;
        private static double E = 0.1;
        private static List<double[]> x = new List<double[]>();
        private static List<double> function = new  List<double>();
                
        private static double firstIncrements;
        private static double secondIncrements;
                
        
        
        static void Main(string[] args)
        {
            for (int i = 0; i < n+1; i++)
            {
                x.Add(new double[n]);
                
            }

            x[0][0] = 0;
            x[0][1] = 0;

            CalculateIncrements();
            CalculateXs();

            for (int i = 0; i < n + 1; i++)
            {
                function.Add(CalculateFunction(x[i]));
            }

            do
            {
                if (Iteration())
                {
                    break;
                }

                

            } while (true);

            Console.ReadLine();
        }

        private static bool isEnd(double f)
        {
            for (int i = function.Count-(n+1); i < function.Count; i++)
            {
                if (Math.Abs(function[i] - f) >= E)
                    return false;
            }
            return true;
        }

        private static void Swap(List<double> arr , int f, int s )
        {
            var temp = arr[f];
            arr[f] = arr[s];
            arr[s] = temp;
        }

        private static void Swap(List<double[]> arr, int f, int s)
        {
            var temp = arr[f];
            arr[f] = arr[s];
            arr[s] = temp;
        }

        private static void CalculateIncrements()
        {
            firstIncrements = (Math.Sqrt(n + 1) - 1) / (n * Math.Sqrt(2)) * m;
            secondIncrements = (Math.Sqrt(n + 1) + n - 1) / (n * Math.Sqrt(2)) * m;
        }

        private static void CalculateXs()
        {
            
            for (int i = 1; i < n+1; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if(i == j+1)
                    {
                        x[i][j] = x[0][j] + firstIncrements;
                    }
                    else
                    {
                        x[i][j] = x[0][j] + secondIncrements;
                    }
                }

            }
        }

        private static double CalculateFunction(double[] x)
        {
            return x[0] * x[0] - x[0] * x[1] + 3 * x[1] * x[1] - x[0];
        }

        private static double[] CalculateMassCenter(List<double[]> xs)
        {
            double[] result = new double[n];
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < xs.Count; j++)
                {
                    sum += xs[j][i];
                }
                
                result[i] = (1.0 / xs.Count) * sum;
                
                
            }
            return result;
        }

        private static double[] CalculateReflection(double[] xc)
        {
            double[] result = new double[n];
            for (int i = 0; i < n; i++)
            {
                result[i] = 2 * xc[i] - x[function.IndexOf(function.Max())][i];
            }
            return result;
        }

        private static bool EndStep(double[] xc, double fc, int k)
        {
            x[k] = xc;
            function[k] = fc;


            List<double[]> cmData_2 = new List<double[]>();
            for (int i = x.Count - (n + 1); i < x.Count; i++)
            {
                cmData_2.Add(x[i]);
            }
            if (isEnd(CalculateFunction(CalculateMassCenter(cmData_2))))
            {

                Console.WriteLine("Answer is " + function.Min());
                return true;
            }
            else
                return false;


        }


        private static bool Iteration()
        {
            

            List<double[]> cmData = new List<double[]>();

            int k = function.IndexOf(function.Max());

            for (int i = x.Count-(n+1); i < n+1; i++)
            {
                if (k != i)
                {
                    cmData.Add(x[i].Clone() as double[]);
                }
            }

            

            var xc = CalculateReflection(CalculateMassCenter(cmData));
            var fc = CalculateFunction(xc);

            if (function[k] > function[function.Count - 1])
            {
                if (EndStep(xc, fc, k))
                    return true;
            }
            else
            {
                int r = function.IndexOf(function.Min());

                List<double[]> newSimplex = new List<double[]>();

                for (int i = 0; i < n + 1; i++)
                {
                    if (i != r)
                    {
                        double[] result = new double[n];

                        for (int j = 0; j < n; j++)
                        {
                            result[j] = x[r][j] * 0.5 * (x[i][j] - x[r][j]);
                        }
                        function[i] = CalculateFunction(result);
                        newSimplex.Add(result);

                    }
                }
                //newSimplex.Add(new double[n]);
                for (int i = 0; i < n + 1; i++)
                {
                    if (i != r)
                    {
                        x[i] = newSimplex[i];
                    }
                }
                if (EndStep(x[r], function[r], r))
                    return true;
                
            }

            return false;
             
        }


    }
}
