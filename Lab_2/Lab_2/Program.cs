using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_2
{



    class Program
    {
        private static int n = 3;
        private static double m = 1;
        private static double b = 2.8;
        private static double y = 0.4;
        private static double E = 0.1;
        private static List<double[]> x = new List<double[]>();
        private static List<double> function = new List<double>();

        private static int  minI;
        private static int  maxI;
        private static double Fs;

        private static double firstIncrements;
        private static double secondIncrements;


        private static double CalculateFunction(double[] x)
        {
            //return x[0] * x[0] - x[0] * x[1] + 3 * x[1] * x[1] - x[0];

            //return 3 * x[0] * x[0] + x[0] * x[1] + 3 * x[1] * x[1] - 8 * x[0];

            //return Math.Pow(x[0], 4) + Math.Pow(x[0], 2) * x[1] - 6 * Math.Pow(x[0], 2) - 1.2 * x[0] * x[1] + Math.Pow(x[1], 2);

            //return 2 * Math.Pow(x[0], 4) + Math.Pow(x[1], 4) + Math.Pow(x[0], 2) * x[1] - 5 * x[0] * x[1] + 3 * Math.Pow(x[0], 2) + 8 * x[1];

            return 4 * Math.Pow(x[0], 2) + 4 * Math.Pow(x[1], 2) + Math.Pow(x[2], 2) - 2 * x[0] * x[2] - 5 * x[0] * x[1] - 8 * x[2];

            //return Math.Pow(x[0], 2) - x[0] * x[1] + 3 * Math.Pow(x[1], 2) - x[0];
        }

        private static void FirstStep()
        {
            for (int i = 0; i < n + 1; i++)
            {
                x.Add(new double[n]);
            }

            x[0][0] = 1;
            x[0][1] = 1;
            x[0][2] = 1;

        }

        static void Main(string[] args)
        {
            FirstStep();

            CalculateIncrements();
            CalculateXs();

            for (int i = 0; i < n + 1; i++)
            {
                function.Add(CalculateFunction(x[i]));
            }

            
            

            int counter = 0;

            do
            {
                minI = function.IndexOf(function.Min());
                maxI = function.IndexOf(function.Max());

                List<double> temp = new List<double>();

                for (int i = 0; i < n + 1; i++)
                {
                    if (i == minI || i == maxI)
                        continue;
                    temp.Add(function[i]);
                }

                Fs = function[function.IndexOf(temp.Max())];




                counter++;
                if (Iteration())
                {
                    break;
                }



            } while (true);

            Console.WriteLine( counter );
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
            double sigm;
            double sum = 0;
            for (int i = 0; i < n+1; i++)
            {
                sum += function[i] - CalculateFunction(CalculateMassCenter(x));
            }

            sigm = Math.Sqrt((1.0 / (n + 1) * Math.Pow(sum, 2)));
            Console.WriteLine(sigm);
            if (sigm < E)
                return true;

            return false;
        }

        

        private static bool EndStep()
        {



            if (isEnd(CalculateFunction(CalculateMassCenter(x))))
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

            for (int i = x.Count - (n + 1); i < n + 1; i++)
            {
                if (k != i)
                {
                    cmData.Add(x[i].Clone() as double[]);
                }
            }


            var cm = CalculateMassCenter(cmData);
            var xc = CalculateReflection(cm);
            var fc = CalculateFunction(xc);

           




            if (function[k] > fc)
            {
                x[k] = xc;
                function[k] = fc;


                if (function[k] < function[minI])
                {
                    var exp = Expand(cm, k);
                    var expFunc = CalculateFunction(exp);
                    if (expFunc < function[k])
                    {
                        function[k] = expFunc;
                        x[k] = exp;
                    }



                }
                else
                {
                    if (Fs < fc && fc < function[maxI])
                    {
                        var cons = Сonstrict(cm, k);
                        var consFunc = CalculateFunction(cons);
                        if (consFunc < function[k])
                        {
                            function[k] = consFunc;
                            x[k] = cons;
                        }
                        else
                        {
                            Reduction();
                        }

                    }
                    else
                    {
                        Reduction();
                    }
                }


                if (EndStep())
                    return true;
            }
            else
            {
                if (Fs < fc && fc < function[maxI])
                {
                    var cons = Сonstrict(cm, k);
                    var consFunc = CalculateFunction(cons);
                    if (consFunc < function[k])
                    {
                        function[k] = consFunc;
                        x[k] = cons;
                    }
                    else
                    {
                        Reduction();
                    }

                }
                else
                {
                    Reduction();
                }

                if (EndStep())
                    return true;
            }
            return false;
        }


        private static double[] Expand(double[] cm, int k)
        {
            double[] result = new double[n];

            for (int i = 0; i < n; i++)
            {
                result[i] = cm[i] + b * (x[k][i] - cm[i]);
                
            }
            return result;
        }

        private static double[] Сonstrict(double[] cm, int k)
        {
            double[] result = new double[n];

            for (int i = 0; i < n; i++)
            {
                result[i] = cm[i] + y * (x[k][i] - cm[i]);
                
            }
            return result;
        }

        private static void Reduction()
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


        }


    }

}




/*
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
                foreach (var item in x)
                {

                    foreach (var it in item)
                    {
                        Console.WriteLine(it);
                    }
                }
                foreach (var item in function)
                {
                    Console.WriteLine("F(x) " + item);
                }

                Console.WriteLine();
     */
