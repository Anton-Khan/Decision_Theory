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
        private static List<double> function = new List<double>();

        private static double firstIncrements;
        private static double secondIncrements;


        private static double CalculateFunction(double[] x)
        {
            //return x[0] * x[0] - x[0] * x[1] + 3 * x[1] * x[1] - x[0];

            //return 3 * x[0] * x[0] + x[0] * x[1] + 3 * x[1] * x[1] - 8 * x[0];


            return 2.5 * Math.Pow(x[0], 2) - 3.1 * x[0] * x[1] + Math.Pow(x[1], 2) - 5.1 * x[0];
        }

        private static void FirstStep()
        {
            for (int i = 0; i < n + 1; i++)
            {
                x.Add(new double[n]);
            }

            x[0][0] = -1;
            x[0][1] = -1;
            
        }

        static void DrawX()
        {
            Console.WriteLine("______Xs_______");
            foreach (var item in x)
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
            foreach (var item in function)
            {               
                Console.WriteLine(item);
            }
            Console.WriteLine("_______________");
        }

        static void Main(string[] args)
        {
            FirstStep();
            

            CalculateIncrements();
            Console.WriteLine("fi = " + firstIncrements);
            Console.WriteLine("si = " + secondIncrements);
            CalculateXs();
            DrawX();

            for (int i = 0; i < n + 1; i++)
            {
                function.Add(CalculateFunction(x[i]));
            }
            DrawF();
            Console.WriteLine( "\n");
            int it = 0;
            do
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("__________________");
                Console.WriteLine("it = " + it);
                if (Iteration())
                {
                    Console.WriteLine("it = " + it);
                    break;
                }
                DrawX();
                DrawF();
                it++;

                

            } while (true);

            Console.ReadLine();
        }


        private static void CalculateIncrements()
        {
            firstIncrements = (Math.Sqrt(n + 1) - 1) / (n * Math.Sqrt(2)) * m;
            secondIncrements = (Math.Sqrt(n + 1) + n - 1) / (n * Math.Sqrt(2)) * m;
        }

        private static void CalculateXs()
        {

            for (int i = 1; i < n + 1; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j + 1)
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

        private static bool isEnd(double f)
        {
            Console.WriteLine("isEnd ?");
            for (int i = function.Count - (n + 1); i < function.Count; i++)
            {
                var a = Math.Abs(function[i] - f);
                
                Console.WriteLine(a + " > " + E );
                if (a >= E)
                {
                    Console.WriteLine("isEnd NO");
                    return false;
                }
            }
            Console.WriteLine("isEnd YES");
            return true;
        }


        private static bool EndStep()
        {
            var cmc = CalculateMassCenter(x);
            var cf = CalculateFunction(cmc);
            Console.Write("Fulle CM = ");
            foreach (var item in cmc)
            {
                Console.Write(" " + item);
            }
            Console.WriteLine("\nFullFunc = " + cf);

            if(isEnd(cf))
            {

                int t = 0;
                foreach (var item in x[function.IndexOf(function.Min())])
                {
                    Console.WriteLine("Answer is  x" + t + " = " + item);
                    t++;
                }
                Console.WriteLine("Answer is  f(x) = " + function.Min());
                return true;
            }
            else
                return false;


        }


        private static bool Iteration()
        {
            

            List<double[]> cmData = new List<double[]>();

            int k = function.IndexOf(function.Max());
            Console.WriteLine("take " + k + " func");

            for (int i = x.Count - (n + 1); i < n + 1; i++)
            {
                if (k != i)
                {
                    cmData.Add(x[i].Clone() as double[]);
                }
            }
            
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var item in CalculateMassCenter(cmData))
            {
                Console.WriteLine("Cm = " + item);
            }
            Console.WriteLine();

            var xc = CalculateReflection(CalculateMassCenter(cmData));
            var fc = CalculateFunction(xc);
            Console.ForegroundColor = ConsoleColor.Blue;
            foreach (var item in xc)
            {
                Console.WriteLine("Ref = " + item);
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Fc = " + fc);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("__________________");



            if (function[k] > fc)
            {
                x[k] = xc;
                function[k] = fc;
                if (EndStep())
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
                            var a = x[r][j];
                            var b = x[i][j];
                            result[j] = x[r][j] + 0.5 * (x[i][j] - x[r][j]);
                        }
                        function[i] = CalculateFunction(result);
                        newSimplex.Add(result);

                    }
                    else
                    {
                        newSimplex.Add(x[r]);
                    }
                }

                for (int i = 0; i < n + 1; i++)
                {

                    x[i] = newSimplex[i];

                }

                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("____Reduc_____");
                DrawX();
                DrawF();
                Console.WriteLine("____-----_____");
                Console.ForegroundColor = ConsoleColor.White;
                

                if (EndStep())
                    return true;

                
            }
            return false;
        }
    }
}
