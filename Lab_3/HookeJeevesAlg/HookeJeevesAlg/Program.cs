using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookeJeevesAlg
{
    class Program
    {
        private static int n = 2;
        private static double m = 0.5;
        private static double E = 0.1;
        private static double[] h = { 0.2, 0.2};
        private static double d = 2;
        private static int dir = 0;
        private static List<double[]> x = new List<double[]>();
        private static List<double> f = new List<double>();

        private static double[] p;
        


        private static double CalculateFunction(double[] x)
        {
            //return x[0] * x[0] - x[0] * x[1] + 3 * x[1] * x[1] - x[0];

            //return 3 * x[0] * x[0] + x[0] * x[1] + 3 * x[1] * x[1] - 8 * x[0];


            return 2.5 * Math.Pow(x[0], 2) - 3.1 * x[0] * x[1] + Math.Pow(x[1], 2) - 5.1 * x[0];
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
            foreach (var item in f)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("_______________");
        }





        static void Main(string[] args)
        {
             
            x.Add(new double[] { -1, -1 });
            f.Add(CalculateFunction(x.Last()));
            

            x.Add(x.Last().Clone() as double[]);
            f.Add(CalculateFunction(x[1]));
            for (int i = 0; i < 3200; i++)
            {
                Console.WriteLine("_______________________________________________");
                Console.WriteLine("It = " + i/3);
                Console.WriteLine("Direction = " + dir);
                Console.WriteLine("h0 = " + h[0] + " h1 = " + h[1] );
                if (dir == 0 )
                {
                    x[1] = x[0].Clone() as double[];   
                }

                f[1] = (CalculateFunction(x[1]));


                try
                {
                    x[1][dir] = x[1][dir] + h[dir];
                    f[1] = CalculateFunction(x.Last());
                
                
                     DrawX();
                     DrawF();

                     if (!(f[1] < f[0]))
                     {
                         x[1][dir] = x[1][dir] - 2 * h[dir];
                         f[1] = (CalculateFunction(x.Last()));
                         DrawX();
                         DrawF();
                         if (!(f[1] < f[0]))
                         {
                             x[1][dir] = x[1][dir] + h[dir];
                             f[1] = (CalculateFunction(x.Last()));
                             DrawX();
                             DrawF();
                         }
                     }
                }
                catch (Exception)
                {


                }

                if (!(dir < n))
                {
                    bool eqv = true;
                    for (int j = 0; j < n; j++)
                    {
                        if (Math.Round(x[0][j], 4) != Math.Round(x[1][j], 4))
                        {
                            eqv = false;
                            break;
                        }
                    }
                    Console.WriteLine("Search is " + !eqv);
                    if (eqv)
                    {
                        try
                        {
                            
                            for (int j = 0; j < n; j++)
                            {
                                h[j] = h[j] / d;
                            }
                            dir = 0;
                            continue;
                        }
                        catch (Exception)
                        {
                        }
                        
                    }
                    else
                    {
                        double[] temp2 = new double[n];
                        for (int j = 0; j < n; j++)
                        {
                            
                            temp2[j] = x[1][j] + m * (x[1][j] - x[0][j]);
                            
                        }
                        foreach (var item in temp2)
                        {
                            Console.WriteLine("P = " + item);
                        }
                        var nf = CalculateFunction(temp2);
                        Console.WriteLine("F(p) = " + nf );

                        if (nf < f[1]) 
                        {
                            x[0] = temp2;
                            f[0] = CalculateFunction(x[0]);
                        }
                        else
                        {
                            x[0] = x[1];
                            f[0] = CalculateFunction(x[0]);
                        }

                        bool eq = true;
                        foreach (var item in h)
                        {
                            if (!(item < E))
                            {
                                eq = false;
                                break;
                            }
                        }

                        if(eq)
                        {
                            foreach (var item in x[1])
                            {
                                Console.WriteLine("X = " + item);
                            }
                            Console.WriteLine("Fmin = " + f[1]);
                            break;
                        }
                        else
                        {
                            dir = 0;
                            continue;
                        }

                    }
                }
                else
                {
                    
                        dir++;
                    
                    
                }

                Console.WriteLine("_______________________________________________");
                //Console.ReadLine();
            }

            

            Console.ReadLine();

        }

       
    }
}