using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePerformanceTest.SpanTest
{
    public class SpanTest1 : IDisposable
    {
        public void Dispose()
        {
        }
        public void Run()
        {
            Console.WriteLine("Samples:");
            Console.WriteLine("Times   ListAdd  SpanAdd  Encrypt  EncryptSpan  IndexOf  IndexOfSpan");
            //Console.WriteLine("1000000 00.0000  00.0000  00.0000     00.0000   00.0000   00.0000");

            int[] times = new int[] { 5000, 50000, 100000 };
            Dictionary<int, Dictionary<int, List<double>>> mapExecuteTime = Utils.InitExecuteTime(times, 9);

            for (int x = 0; x < 10; x++)
            {
                foreach (int size in times)
                {
                    // Direct+
                    long t = DateTime.Now.Ticks;
                    funListAdd(size);
                    long ts = DateTime.Now.Ticks - t;
                    mapExecuteTime[size][1].Add(ts / 10000000.0);

                    t = DateTime.Now.Ticks;
                    funSpanAdd(size);
                    ts = DateTime.Now.Ticks - t;
                    mapExecuteTime[size][2].Add(ts / 10000000.0);

                    t = DateTime.Now.Ticks;
                    funEncrypt(size);
                    ts = DateTime.Now.Ticks - t;
                    mapExecuteTime[size][3].Add(ts / 10000000.0);

                    // string.Format with ToString
                    t = DateTime.Now.Ticks;
                    funEncryptSpan(size);
                    ts = DateTime.Now.Ticks - t;
                    mapExecuteTime[size][4].Add(ts / 10000000.0);
                    // string.Format with Format
                    t = DateTime.Now.Ticks;
                    funIndexOf(size);
                    ts = DateTime.Now.Ticks - t;
                    mapExecuteTime[size][5].Add(ts / 10000000.0);
                    // Interpolated(Format)
                    t = DateTime.Now.Ticks;
                    funIndexOfSpan(size);
                    long tc = DateTime.Now.Ticks - t;
                    mapExecuteTime[size][6].Add(ts / 10000000.0);
                }
                Console.Title = "Pharse: " + x.ToString() + " of  10";
            }

            foreach (int size in times)
            {
                StringBuilder sb = new StringBuilder();
                //Console.WriteLine("  00.0000");
                sb.Append(string.Format("{0:-6}", size.ToString("0000000")));
                double mean = Utils.Average(mapExecuteTime, size, 1);
                sb.Append(string.Format(" {0} ", mean.ToString("00.0000")));
                mean = Utils.Average(mapExecuteTime, size, 2);
                sb.Append(string.Format(" {0} ", mean.ToString("00.0000")));
                mean = Utils.Average(mapExecuteTime, size, 3);
                sb.Append(string.Format(" {0} ", mean.ToString("00.0000")));
                mean = Utils.Average(mapExecuteTime, size, 4);
                sb.Append(string.Format("  {0} ", mean.ToString("00.0000")));
                mean = Utils.Average(mapExecuteTime, size, 5);
                sb.Append(string.Format("    {0} ", mean.ToString("00.0000")));
                mean = Utils.Average(mapExecuteTime, size, 6);
                sb.Append(string.Format("  {0}", mean.ToString("00.0000")));
                Console.WriteLine(sb.ToString());
            }
            Console.Beep();
            ConsoleKeyInfo key = Console.ReadKey();
        }

        private void funIndexOfSpan(int iT)
        {
            ReadOnlySpan<char> sub = "2020-08-07".AsSpan();
            for (int i = 0; i < iT; i++)
            {
                ReadOnlySpan<char> s = string.Format("Today is 2020-08-07 Afternoon:" + i.ToString("#,##0.00"), "HAHAHA").AsSpan();
                int x = s.IndexOf(sub);
            }
        }

        private void funIndexOf(int iT)
        {
            for (int i = 0; i < iT; i++)
            {
                string s = string.Format("Today is 2020-08-07 Afternoon:" + i.ToString("#,##0.00"), "HAHAHA");
                int x = s.IndexOf("2020-08-07");
            }
        }

        private void funEncryptSpan(int iT)
        {
            List<string> arrS = new List<string>();
            List<string> arrSS = new List<string>();
            for (int i = 0; i < iT; i++)
            {
                arrS.Add(CryptSpan.EncryptString("Today is 2020-08-07 Afternoon:" + i.ToString("#,##0.00"), "HAHAHA"));
                arrSS.Add(CryptSpan.DecryptString(arrS[i], "HAHAHA"));
            }
        }

        private void funEncrypt(int iT)
        {
            List<string> arrS = new List<string>();
            List<string> arrSS = new List<string>();
            for (int i = 0; i < iT; i++)
            {
                arrS.Add(Crypt.EncryptString("Today is 2020-08-07 Afternoon:" + i.ToString("#,##0.00"), "HAHAHA"));
                arrSS.Add(Crypt.DecryptString(arrS[i], "HAHAHA"));
            }
        }

        private void funSpanAdd(int iT)
        {
            Span<DfndBase> arr = new Span<DfndBase>();
            for (int i = 0; i < iT; i++)
            {
                DfndBase d = new DfndBase(1, "XC", "NAME", 1, 1, 1, DateTime.Today, DateTime.Today, DateTime.Today);
                arr.Fill(d);
            }
            long x = 0;
            foreach (DfndBase d in arr)
                x += d.FundID;
        }

        private void funListAdd(int iT)
        {
            List<DfndBase> arr = new List<DfndBase>();
            for (int i = 0; i < iT; i++)
            {
                DfndBase d = new DfndBase(1, "XC", "NAME", 1, 1, 1, DateTime.Today, DateTime.Today, DateTime.Today);
                arr.Add(d);
            }
            long x = 0;
            foreach (DfndBase d in arr)
                x += d.FundID;
        }

        /// <summary>
        /// 基金基本
        /// </summary>
        public class DfndBase
        {
            #region // 資料庫欄位對應
            /// <summary>
            /// 基金id(PK)
            /// </summary>
            public long FundID;
            /// <summary>
            /// 基金代码
            /// </summary>
            public string Code { get; set; }
            /// <summary>
            /// 基金名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 基金状态
            /// </summary>
            public int Status = 1;
            /// <summary>
            /// 基金型态
            /// </summary>
            public long FundStyle;
            /// <summary>
            /// 基金管理人
            /// </summary>
            public long ManagerID;
            /// <summary>
            /// 修改时间
            /// </summary>
            public DateTime ModiTime;
            /// <summary>
            /// 基金开始日期
            /// </summary>
            public DateTime StartDate { get; set; }
            /// <summary>
            /// 基金结束日期
            /// </summary>
            public DateTime EndDate { get; set; }
            #endregion

            #region // 唯一 ID 定義 - ID<->FundID
            /// <summary>
            /// ID(PK)
            /// </summary>
            public long ID
            {
                get { return FundID; }
                set { FundID = value; }
            }
            #endregion

            #region // 建構式
            /// <summary>
            /// 建構式
            /// </summary>
            public DfndBase()
            {
            }

            /// <summary>
            /// 建構式
            /// </summary>
            /// <param name="FundID">基金id(PK)</param>
            /// <param name="Code">基金代码</param>
            /// <param name="Name">基金名称</param>
            /// <param name="Status">基金状态</param>
            /// <param name="FundStyle">基金型态</param>
            /// <param name="ManagerID">基金管理人</param>
            /// <param name="ModiTime">修改时间</param>
            /// <param name="StartDate">开始日期</param>
            /// <param name="EndDate">结束日期</param>
            public DfndBase(long FundID, string Code, string Name, int Status,
                        long FundStyle, long ManagerID, DateTime ModiTime, DateTime StartDate,
                        DateTime EndDate)
            {
                this.FundID = FundID;
                this.Code = Code;
                this.Name = Name;
                this.Status = Status;
                this.FundStyle = FundStyle;
                this.ManagerID = ManagerID;
                this.ModiTime = ModiTime;
                this.StartDate = StartDate;
                this.EndDate = EndDate;

            }
            #endregion
        }
    }
}
