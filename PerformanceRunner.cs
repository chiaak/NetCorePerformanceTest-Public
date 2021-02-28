using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePerformanceTest
{
    /// <summary>
    /// 新增给多种回传资料
    /// </summary>
    public enum emTestType
    {
        /// <summary>
        /// 测试执行时间
        /// </summary>
        Performance = 1,
        /// <summary>
        /// 记录 Log
        /// </summary>
        Log = 2,
        /// <summary>
        /// 都要
        /// </summary>
        Both = 3,
        /// <summary>
        /// 回传的数值给加总之后平均
        /// </summary>
        NumbersForAvg = 4,
    }
    /// <summary>
    /// 通用测试介面 用来统一处理执行 ， 比较，展现
    /// </summary>
    public class PerformanceRunner
    {
        private List<int> arrTimes = null;
        /// <summary>
        /// 计算平均时运行次数
        /// </summary>
        private int innerTime;
        /// <summary>
        /// 每次执行的 初始工具
        /// </summary>
        Func<int, int> InitProcess;
        Func<object> DataGetter;
        Func<string, int> Messager;
        List<Tuple<string, emTestType, Func<int, Func<object>, string>>> arrTestFunction = null;
        public PerformanceRunner(List<int> arrTimes, int innerTime, Func<int, int> InitProcess,
                List<Tuple<string, emTestType, Func<int, Func<object>, string>>> arrTestFunction,
                Func<object> DataGetter, Func<string, int> Messager)
        {
            this.arrTimes = arrTimes;
            this.innerTime = innerTime;
            this.InitProcess = InitProcess;
            this.arrTestFunction = arrTestFunction;
            this.DataGetter = DataGetter;
            this.Messager = Messager;
        }

        public string[][] Run()
        {
            string msg = "Pharse {0} , Function:{1} , ExecTimes:{2},Used Time:{3}";
            //List<DTest> arrS = new List<DTest>();
            // dataGridView1.Rows.Clear();
            // rnd = new Random((int)DateTime.Now.Ticks);
            #region // Init Result Grid
            Dictionary<int, Dictionary<string, List<double>>> mapExecuteTime = new Dictionary<int, Dictionary<string, List<double>>>();
            Dictionary<string, string> mapExecuteLog = new Dictionary<string, string>();
            string[][] retResult = new string[arrTimes.Count + 1][];
            int idx = 0;
            retResult[0] = new string[arrTestFunction.Count + 1];
            retResult[0][0] = "Times";
            for (int i = 0; i < arrTestFunction.Count; i++)
                retResult[0][i + 1] = arrTestFunction[i].Item1;
            idx++;
            foreach (int iT in arrTimes)
            {
                retResult[idx] = new string[arrTestFunction.Count + 1];
                retResult[idx][0] = iT.ToString("#,##0");
                mapExecuteTime.Add(iT, new Dictionary<string, List<double>>());
                foreach (Tuple<string, emTestType, Func<int, Func<object>, string>> fun in arrTestFunction)
                {
                    if (fun.Item2 != emTestType.Log)
                        mapExecuteTime[iT].Add(fun.Item1, new List<double>());
                }
                idx++;
            }
            #endregion

            for (int x = 0; x < innerTime; x++)
            {
                foreach (int iT in arrTimes)
                {
                    if (InitProcess != null)
                    {
                        InitProcess(iT);
                    }
                    foreach (Tuple<string, emTestType, Func<int, Func<object>, string>> fun in arrTestFunction)
                    {
                        if (fun.Item2 == emTestType.Log)
                        {
                            if (mapExecuteLog.ContainsKey(fun.Item1) == false)
                                mapExecuteLog.Add(fun.Item1, fun.Item3(iT, DataGetter));
                            continue;
                        }
                        else if (fun.Item2 == emTestType.Both)
                        {
                            if (mapExecuteLog.ContainsKey(fun.Item1) == false)
                                mapExecuteLog.Add(fun.Item1, fun.Item3(iT, DataGetter));
                        }
                        else if (fun.Item2 == emTestType.NumbersForAvg)
                        {
                            string s = fun.Item3(iT, DataGetter);
                            double d = 0;
                            double.TryParse(s, out d);
                            mapExecuteTime[iT][fun.Item1].Add(d);
                            continue;
                        }
                        long t = DateTime.Now.Ticks;
                        fun.Item3(iT, DataGetter);
                        double interval = GetExecuteTime(t);
                        mapExecuteTime[iT][fun.Item1].Add(interval);
                        Messager(string.Format(msg, x, fun.Item1, iT, interval));

                    }
                }
            }
            for (int i = 0; i < arrTimes.Count; i++)
            {// 座标由 (1,1)开始
                for (int x = 0; x < arrTestFunction.Count; x++)
                {
                    if (arrTestFunction[x].Item2 == emTestType.Performance)
                    {
                        double mean = Average(mapExecuteTime, arrTimes[i], arrTestFunction[x].Item1);
                        retResult[i + 1][x + 1] = mean.ToString("0.00000");
                    }
                    else if (arrTestFunction[x].Item2 == emTestType.NumbersForAvg)
                    {
                        double mean = mapExecuteTime[arrTimes[i]][arrTestFunction[x].Item1].Average();//
                        //Average(mapExecuteTime, arrTimes[i], arrTestFunction[x].Item1);
                        retResult[i + 1][x + 1] = mean.ToString("0.000");
                    }
                    else
                    {
                        retResult[i + 1][x + 1] = mapExecuteLog[arrTestFunction[x].Item1];
                    }
                }
            }
            return retResult;
        }
        /// <summary>
        /// 回传执行秒数
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public static double GetExecuteTime(long ticks)
        {
            long ts = DateTime.Now.Ticks - ticks;
            return (ts / 10000000.0);
        }
        public double Average(Dictionary<int, Dictionary<string, List<double>>> mapExecuteTime, int iT, string v)
        {
            double avg = mapExecuteTime[iT][v].Sum();
            avg = avg - mapExecuteTime[iT][v].Max();
            avg = avg - mapExecuteTime[iT][v].Min();
            avg = avg / (mapExecuteTime[iT][v].Count - 2);
            return avg;
        }
    }
}
