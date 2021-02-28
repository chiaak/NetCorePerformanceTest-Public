using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePerformanceTest
{
    public class RedisTester : IDisposable
    {
        private class DProductDay
        {
            #region // 建构式
            /// <summary>
            /// 建构式
            /// </summary>
            /// <param name="DayDate">日期</param>
            /// <param name="SymID">商品編碼</param>
            /// <param name="Refer">參考價</param>
            /// <param name="Close">收盤價</param>
            /// <param name="Yield">殖利率</param>
            /// <param name="ModiTime">更新時間</param>
            /// <param name="SourceType">來源類型：一般(0)，遞補(1)。</param>
            /// <param name="ret">收益率</param>
            public DProductDay(DateTime DayDate, int SymID, decimal Refer, decimal Close, decimal Yield, DateTime ModiTime, decimal ret, int SourceType = 1)
            {
                this.DayDate = DayDate;
                this.SymID = SymID;
                this.Refer = Refer;
                this.Close = Close;
                this.Yield = Yield;
                this.ModiTime = ModiTime;
                this.SourceType = SourceType;
                this.ret = ret;
            }
            #endregion

            #region DProductDay Members
            /// <summary>
            /// 來源類型：一般(0)，遞補(1)。
            /// </summary>
            public int SourceType { get; set; }
            /// <summary>
            /// 日期
            /// </summary>
            public DateTime DayDate { get; set; }
            /// <summary>
            /// 商品編碼
            /// </summary>
            public int SymID { get; set; }
            /// <summary>
            /// 參考價
            /// </summary>
            public decimal Refer { get; set; }
            /// <summary>
            /// 收盤價
            /// </summary>
            public decimal Close { get; set; }
            /// <summary>
            /// 殖利率
            /// </summary>
            public decimal Yield { get; set; }
            /*
            /// <summary>
            /// 收益率
            /// </summary>
            public decimal Return
            {
                get
                {
                    if (this.Refer != 0)
                    {
                        return ((this.Close - this.Refer) / this.Refer);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            */
            /// <summary>
            /// 收益率
            /// </summary>
            public decimal ret { get; set; }
            /// <summary>
            /// 更新時間
            /// </summary>
            public DateTime ModiTime { get; set; }
            #endregion
            /// <summary>
            /// 判斷是否相等
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            public bool Equals(DProductDay data)
            {
                if (this.DayDate.ToOADate() != data.DayDate.ToOADate() || this.SymID != data.SymID || this.Close != data.Close)
                    return false;
                if (this.Refer != data.Refer || this.Yield != data.Yield || this.SourceType != data.SourceType || this.ret != data.ret)
                    return false;
                return true;
            }
        }
        public void Dispose()
        {
        }
        public void Run()
        {
            long t = DateTime.Now.Ticks;
            List<DProductDay> arr = new List<DProductDay>();
            for (int i = 0; i < 100000; i++)
            {
                arr.Add(new DProductDay(DateTime.Today, i, i * 1.1M, i * 1.2M, 1 * 1.3M, DateTime.Now, i * 0.1M));
            }
            long ts = DateTime.Now.Ticks - t;
            Console.WriteLine("Prepare Data:100000:" + (ts / 10000000.0).ToString("0.00000") + " sec");
            //  Start Redis 
            ConfigurationOptions Config = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                SyncTimeout = 10000,
                ConnectTimeout = 600,
                AllowAdmin = true
            };
            Config.EndPoints.Add("127.0.0.1", 6379);// 10.10.8.100
            ConnectionMultiplexer _redisServer = ConnectionMultiplexer.Connect(Config);
            IDatabase db = _redisServer.GetDatabase(13);
            //  Loop Push Data To Redis Server
            LoopToRedis(db, arr);
            // Task Push To Redis
            // UBUBTU 会有 Exception的问题
             TaskToRedis(db, arr);
            // Batch 20 Push To Redis
            BatchToRedis(db, arr, 20);
            // Batch Task Push To Redis
            BatchTaskToRedis(db, arr, 20);
            // Batch 50 Push To Redis
            BatchToRedis(db, arr, 50);
            // Batch Task Push To Redis
            BatchTaskToRedis(db, arr, 50);
            // Batch 100 Push To Redis
            BatchToRedis(db, arr, 100);
            // Batch Task Push To Redis
            BatchTaskToRedis(db, arr, 100);

            ConsoleKeyInfo key = Console.ReadKey();
        }

        private void BatchTaskToRedis(IDatabase db, List<DProductDay> arr, int BatchSize)
        {
            long t = DateTime.Now.Ticks;
            int cIdx = 0;
            HashEntry[] hash = new HashEntry[BatchSize];
            List<Task> arrTasks = new List<Task>();
            for (int i = 0; i < 100000; i++)
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(arr[i]);
                hash[cIdx] = new HashEntry(arr[i].SymID.ToString(), json);
                if (cIdx == BatchSize)
                {
                    Task tx = db.HashSetAsync("ProductDay", hash);
                    arrTasks.Add(tx);
                    cIdx = 0;
                }
            }
            if (cIdx != 0)
            {
                Task tx = db.HashSetAsync("ProductDay", hash.Take(cIdx).ToArray());
                arrTasks.Add(tx);
            }
            Task.WaitAll(arrTasks.ToArray());
            t = DateTime.Now.Ticks - t;
            Console.WriteLine("Batch Task  Push:" + BatchSize.ToString() + " 計時:" + (t / 10000000.0).ToString("0.00000") + " sec");
            db.KeyDeleteAsync("ProductDay");
            System.Threading.Thread.Sleep(2 * 1000);
        }
        private void BatchToRedis(IDatabase db, List<DProductDay> arr, int BatchSize)
        {
            long t = DateTime.Now.Ticks;
            int cIdx = 0;
            HashEntry[] hash = new HashEntry[BatchSize];
            for (int i = 0; i < 100000; i++)
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(arr[i]);
                hash[cIdx] = new HashEntry(arr[i].SymID.ToString(), json);
                if (cIdx == BatchSize)
                {
                    db.HashSetAsync("ProductDay", hash);
                    cIdx = 0;
                }
            }
            if (cIdx != 0)
                db.HashSetAsync("ProductDay", hash.Take(cIdx).ToArray());
            t = DateTime.Now.Ticks - t;
            Console.WriteLine("Batch By : " + BatchSize.ToString() + "HashSetAsync :  計時:" + (t / 10000000.0).ToString("0.00000") + " sec");
            db.KeyDeleteAsync("ProductDay");
            System.Threading.Thread.Sleep(2 * 1000);
        }

        private void TaskToRedis(IDatabase db, List<DProductDay> arr)
        {
            long t = DateTime.Now.Ticks;
            List<Task> arrTasks = new List<Task>();
            for (int i = 0; i < 100000; i++)
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(arr[i]);
                Task xt = db.HashSetAsync("ProductDay", arr[i].SymID.ToString(), json);
                arrTasks.Add(xt);
            }
            Task.WaitAll(arrTasks.ToArray());
            t = DateTime.Now.Ticks - t;
            Console.WriteLine("Task For Loop HashSetAsync : 100000  計時:" + (t / 10000000.0).ToString("0.00000") + " sec");
            db.KeyDeleteAsync("ProductDay");
            System.Threading.Thread.Sleep(2 * 1000);// .Delay(10 * 1000);
        }
        private void LoopToRedis(IDatabase db, List<DProductDay> arr)
        {
            long t = DateTime.Now.Ticks;
            for (int i = 0; i < 100000; i++)
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(arr[i]);
                db.HashSetAsync("ProductDay", arr[i].SymID.ToString(), json);
            }
            t = DateTime.Now.Ticks - t;
            Console.WriteLine("For Loop HashSetAsync: 100000 :   計時: " + (t / 10000000.0).ToString("0.00000") + " sec");
            db.KeyDeleteAsync("ProductDay");
            System.Threading.Thread.Sleep(2 * 1000);// .Delay(10 * 1000);
        }
    }
}
