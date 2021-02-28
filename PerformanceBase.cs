using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePerformanceTest
{
    /// <summary>
    /// 供继承用的
    /// </summary>
    public class PerformanceBase
    {
        /// <summary>
        /// 测试基准次数
        /// </summary>
        protected List<int> arrTimes = new List<int>();
        /// <summary>
        /// 每组执行次数用来平均
        /// </summary>
        protected int iExecTimes = 10;
        /// <summary>
        /// 实际要测试的 Functions
        /// </summary>
        protected List<Tuple<string, emTestType, Func<int, Func<object>, string>>> arrTestFunction;
        /// <summary>
        /// 每个 Function 取得资料的函式
        /// </summary>
        /// <returns></returns>
        protected virtual object DataGetter()
        {
            return null;
        }
        /// <summary>
        /// 每个Pharse 初始话资料
        /// </summary>
        /// <param name="iT"></param>
        /// <returns></returns>
        protected virtual int InitData(int iT)
        {
            return 1;
        }

        /// <summary>
        /// 显示步骤的讯息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected virtual int Message(string msg)
        {
            Console.Title = msg;
            return 1;
        }

        protected void ShowResult(string[][] retResult)
        {
            Console.WriteLine("");
            for (int i = 0; i < retResult[0].Length; i++)
            {
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < retResult.Length; j++)
                {
                    if (j == 0)
                        sb.Append(string.Format("{0,35} ", retResult[j][i]));
                    else
                        sb.Append(string.Format("{0,12} ", retResult[j][i]));
                }
                Console.WriteLine(sb.ToString());
            }
            Console.Beep();
            ConsoleKeyInfo key = Console.ReadKey();
        }
    }
}
