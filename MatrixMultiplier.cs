using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePerformanceTest
{
    public class MatrixMultiplier:PerformanceBase, IDisposable
    {
        public void Dispose()
        {
            
        }
        public MatrixMultiplier()
        {
            arrTimes = new List<int>()
            { 48,50,96,101,192,202,384,405,768,810,1536, 1621 };
            iExecTimes = 10;
            arrTestFunction = new List<Tuple<string, emTestType, Func<int, Func<object>, string>>>()
            {
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("Base[,]" ,emTestType.Performance, BaseMultiPly) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("Base[][]" ,emTestType.Performance, BaseMultiPly2) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("Base Parallel[,]" ,emTestType.Performance, BaseParallelMultiPly) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("Base Parallel[][]" ,emTestType.Performance, BaseParallelMultiPly2) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("Math.NET" ,emTestType.Performance, MathNET) ,
                // IKJ 的太慢了
                //new  Tuple<string, emTestType, Func<int, Func<object>, string>>("IKj" , BaseMultiPlyx) ,
            };
        }
        public void Run()
        {
            PerformanceRunner runner = new PerformanceRunner(
                    arrTimes, iExecTimes, InitData, arrTestFunction, null, Message);
            string[][] retResult = runner.Run();
            ShowResult(retResult);
            /*
            Console.WriteLine("Select Mode: ");
            Console.WriteLine("1. With Memory Copy: 50, 101, 202,404, 809,2001");
            Console.WriteLine("2. No Memory Copy: 48, 104, 200, 400, 800, 2000");
            Console.WriteLine("q or ESC. Exit");
            ConsoleKeyInfo key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.D1:
                    Exec(new int[] { 50, 101, 202, 404, 809, 2001 });
                    break;
                case ConsoleKey.D2:
                    Exec(new int[] { 48, 104, 200, 400, 800, 2000 });
                    break;
                case ConsoleKey.Q:
                case ConsoleKey.Escape:
                    return;
            }
            goto redo;
            */
        }
        Random rnd = new Random(-1);
        double[,] A;
        double[,] B;
        double[][] xA;
        double[][] xB;
        MathNet.Numerics.LinearAlgebra.Matrix<double> mA;
        MathNet.Numerics.LinearAlgebra.Matrix<double> mB;
        protected override int InitData(int iT)
        {
            A = GenMatrix(rnd, iT);
            B = GenMatrix(rnd, iT);
            xA = ConvertM(A);
            xB = ConvertM(B);
            mA = MathNet.Numerics.LinearAlgebra.Matrix<double>.Build.SparseOfArray(A);
            mB = MathNet.Numerics.LinearAlgebra.Matrix<double>.Build.SparseOfArray(B);
            return 1;
        }

        #region // MathNET
        private string MathNET(int iT, Func<object> GetDate)
        {
            MathNet.Numerics.LinearAlgebra.Matrix<double> mC = mA.Multiply(mB);
            return "";
        }
        #endregion

        #region // Base Parallel[][]

        private string BaseParallelMultiPly2(int iT, Func<object> GetDate)
        {
            double[][] c = BaseParallelMultiPly(xA, xB);
            return "";
        }

        private double[][] BaseParallelMultiPly(double[][] a, double[][] b)
        {
            int row1 = a.GetLength(0);
            int col1 = a[0].GetLength(0);
            int col2 = b[0].GetLength(0);
            double[][] ret = new double[row1][];
            for (int i = 0; i < row1; i++)
                ret[i] = new double[col2];
            Parallel.For(0, row1, (i) =>
            {
                Parallel.For(0, col2, (j) =>
                {
                    double x = 0;
                    for (int m = 0; m < col1; m++)
                    {
                        x += a[i][m] * b[m][j];
                    }
                    ret[i][j] = x;
                });
            });
            return ret;
        }
        #endregion

        #region // Base Parallel[,]

        private string BaseParallelMultiPly(int iT, Func<object> GetDate)
        {
            double[,] c = BaseParallelMultiPly(A, B);
            return "";
        }

        private double[,] BaseParallelMultiPly(double[,] a, double[,] b)
        {

            int row1 = a.GetLength(0);
            int col1 = a.GetLength(1);
            int col2 = b.GetLength(1);
            double[,] ret = new double[row1, col2];
            Parallel.For(0, row1, (i) =>
            {
                Parallel.For(0, col2, (j) =>
                {
                    double x = 0;
                    for (int m = 0; m < col1; m++)
                    {
                        x += a[i, m] * b[m, j];
                    }
                    ret[i, j] = x;
                });
            });
            return ret;
        }
        #endregion

        #region // Base[][]

        private string BaseMultiPly2(int iT, Func<object> GetDate)
        {
            double[][] c = BaseMultiPly(xA, xB);
            return "";
        }

        private double[][] BaseMultiPly(double[][] a, double[][] b)
        {
            int row1 = a.GetLength(0);
            int col1 = a[0].GetLength(0);
            int col2 = b[0].GetLength(0);
            double[][] ret = new double[row1][];
            for (int i = 0; i < row1; i++)
                ret[i] = new double[col2];
            // From j -> j -> m To i->m->j ---- Faster
            for (int i = 0; i < row1; i++)
            {
                for (int m = 0; m < col1; m++)
                {
                    for (int j = 0; j < col2; j++)
                    {
                        ret[i][j] += a[i][m] * b[m][j];
                        //Old_Multi_Count++;
                    }
                }
            }
            return ret;
        }
        #endregion

        #region // JKI 

        private string BaseMultiPlyx(int iT, Func<object> GetDate)
        {
            double[,] D = BaseMultiPly2(A, B);
            return "";
        }

        private double[,] BaseMultiPly2(double[,] a, double[,] b)
        {
            int row1 = a.GetLength(0);
            int col1 = a.GetLength(1);
            int col2 = b.GetLength(1);
            double[,] ret = new double[row1, col2];
            for (int i = 0; i < row1; i++)
            {
                for (int j = 0; j < col2; j++)
                {
                    for (int m = 0; m < col1; m++)
                    {
                        ret[i, j] += a[i, m] * b[m, j];
                    }
                }
            }
            return ret;
        }
        #endregion

        #region // BaseMultiPly

        private string BaseMultiPly(int iT, Func<object> GetDate)
        {
            double[,] c = BaseMultiPly(A, B);
            return "";
        }

        private double[,] BaseMultiPly(double[,] a, double[,] b)
        {
            int row1 = a.GetLength(0);
            int col1 = a.GetLength(1);
            int col2 = b.GetLength(1);
            double[,] ret = new double[row1, col2];
            for (int i = 0; i < row1; i++)
            {
                for (int m = 0; m < col1; m++)
                {
                    for (int j = 0; j < col2; j++)
                    {
                        ret[i, j] += a[i, m] * b[m, j];
                        //Old_Multi_Count++;
                    }
                }
            }
            return ret;
        }
        #endregion;

        #region // Basic Tools
        /// <summary>
        /// Gen N*N Matrix Data
        /// </summary>
        /// <param name="rnd"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private double[,] GenMatrix(Random rnd, int size)
        {
            double[,] M = new double[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    M[i, j] = rnd.NextDouble() * 10;
                }
            }
            return M;
        }
        private double[][] ConvertM(double[,] a)
        {
            double[][] ret = new double[a.GetLength(0)][];
            int x = a.GetLength(0);
            int y = a.GetLength(1);
            for (int i = 0; i < x; i++)
            {
                ret[i] = new double[y];
                for (int j = 0; j < y; j++)
                    ret[i][j] = a[i, j];
            }

            return ret;
        }
        private void CompareMatrix(double[,] c, double[,] xD)
        {
            for (int i = 0; i < c.GetLength(0); i++)
            {
                for (int j = 0; j < c.GetLength(1); j++)
                {
                    if (c[i, j] != xD[i, j])
                    {
                        Console.WriteLine("Error");
                        return;
                    }
                }
            }
        }
        private void CompareMatrix(double[,] c, double[][] xD)
        {
            for (int i = 0; i < c.GetLength(0); i++)
            {
                for (int j = 0; j < c.GetLength(1); j++)
                {
                    if (c[i, j] != xD[i][j])
                    {
                        Console.WriteLine("Error");
                        return;
                    }
                }
            }
        }
        #endregion
    }
}
