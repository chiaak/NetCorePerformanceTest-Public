using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCorePerformanceTest
{
    public class SumPerformance : PerformanceBase, IDisposable
    {
        public void Dispose()
        {
        }
        public SumPerformance()
        {
            arrTimes = new List<int>()
            { 1000, 10000, 100000, 1000000, 9999999 };
            iExecTimes = 10;
            arrTestFunction = new List<Tuple<string, emTestType, Func<int, Func<object>, string>>>()
            {
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("int - Linq" , emTestType.Performance, int_Linq) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("int - For" , emTestType.Performance, int_For) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("int -Parallel" , emTestType.Performance, int_Parallel) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("double-Linq" , emTestType.Performance, double_Linq) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("double-For" , emTestType.Performance, double_For) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("double-Parallel" , emTestType.Performance, double_Parallel) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("decimal-Linq" , emTestType.Performance, decimal_Linq) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("decimal-For" , emTestType.Performance, decimal_For) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("decimal-Parallel" , emTestType.Performance, decimal_Parallel) ,
           };
        }
        public void Run()
        {
            PerformanceRunner runner = new PerformanceRunner(
                arrTimes, iExecTimes, InitData, arrTestFunction, DataGetter, Message);
            string[][] retResult = runner.Run();
            ShowResult(retResult);
            Console.ReadKey();
        }
        List<int> arrI = null;
        List<double> arrD = null;
        List<decimal> arrDe = null;
        Random rnd = new Random(100);

        protected override int InitData(int iT)
        {
            arrI = new List<int>(iT);
            arrD = new List<double>(iT);
            arrDe = new List<decimal>(iT);

            for (int i = 0; i < iT; i++)
            {
                arrI.Add(rnd.Next(10));
                arrD.Add(rnd.NextDouble() * 100000);
                arrDe.Add((decimal)arrD[i]);
            }
            return 1;
        }

        #region // Decimal

        private string decimal_Parallel(int iT, Func<object> GetDate)
        {
            decimal dv = arrDe.AsParallel().Sum();
            return "";
        }

        private string decimal_For(int iT, Func<object> GetDate)
        {
            decimal v = 0;
            foreach (int i in arrDe)
                v += i;
            return "";
        }

        private string decimal_Linq(int iT, Func<object> GetDate)
        {
            decimal dv = 0;
            for (int i = 0; i < iT; i++)
            {
                dv += arrDe[i];
            }
            return "";
        }
        #endregion

        #region // Double

        private string double_Parallel(int iT, Func<object> GetDate)
        {
            double dv = arrD.AsParallel().Sum();
            return "";
        }

        private string double_For(int iT, Func<object> GetDate)
        {
            double v = 0;
            foreach (int i in arrD)
                v += i;
            return "";
        }

        private string double_Linq(int iT, Func<object> GetDate)
        {
            double dv = 0;
            for (int i = 0; i < iT; i++)
            {
                dv += arrD[i];
            }
            return "";
        }
        #endregion


        #region // Int

        private string int_Parallel(int iT, Func<object> GetDate)
        {
            int dv = arrI.AsParallel().Sum();
            return "";
        }

        private string int_For(int iT, Func<object> GetDate)
        {
            int v = 0;
            foreach (int i in arrI)
                v += i;
            return "";
        }

        private string int_Linq(int iT, Func<object> GetDate)
        {
            int v = arrI.Sum();
            return "";
        }
        #endregion
    }
}
