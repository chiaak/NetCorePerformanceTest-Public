using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePerformanceTest
{
    public class TupleCompare : IDisposable
    {
        public void Dispose()
        {
        }
        public void Run()
        {
            Console.WriteLine("");
            int[] times = new int[] { 1000, 10000, 100000, 500000 };
            Console.WriteLine("Items           ii Tuple  NamedTuple  Class   iis Tuple  NamedTuple   Class   isdas Tuple   NamedTuple  Class");

            foreach (int iT in times)
            {
                List<Tuple<int, int>> arriiTuple = new List<Tuple<int, int>>();
                List<(int x, int y)> arriiNTuple = new List<(int x, int y)>();
                List<tag_i_i> arriiClass = new List<tag_i_i>();
                //
                List<Tuple<int, int, string>> arriisTuple = new List<Tuple<int, int, string>>();
                List<(int x, int y, string msg)> arriisNTuple = new List<(int x, int y, string msg)>();
                List<tag_i_i_s> arriisClass = new List<tag_i_i_s>();
                //
                List<Tuple<int, string, decimal, string>> arrisdsTuple = new List<Tuple<int, string, decimal, string>>();
                List<(int x, string msg, decimal d, string msg2)> arrisdsNTuple = new List<(int x, string msg, decimal d, string msg2)>();
                List<tag_i_s_d_s> arrisdsClass = new List<tag_i_s_d_s>();
                //
                Console.Write(iT.ToString("000000") + "--Create  ");

                #region // i i rowCreate
                long t = DateTime.Now.Ticks;
                for (int i = 0; i < iT; i++)
                {
                    arriiTuple.Add(new Tuple<int, int>(i + 10, i + 1000));
                }
                long ts = DateTime.Now.Ticks - t;
                Console.Write((ts / 10000000.0).ToString("0.00000    "));
                t = DateTime.Now.Ticks;
                for (int i = 0; i < iT; i++)
                {
                    arriiNTuple.Add((i + 10, i + 1000));
                }
                ts = DateTime.Now.Ticks - t;
                Console.Write((ts / 10000000.0).ToString("0.00000   "));
                t = DateTime.Now.Ticks;
                for (int i = 0; i < iT; i++)
                {
                    arriiClass.Add(new tag_i_i() { x = i + 10, y = i + 1000 });
                }
                ts = DateTime.Now.Ticks - t;
                Console.Write((ts / 10000000.0).ToString("0.00000   "));
                #endregion

                #region // i i s rowCreate
                t = DateTime.Now.Ticks;
                for (int i = 0; i < iT; i++)
                {
                    arriisTuple.Add(new Tuple<int, int, string>(i + 10, i + 1000, (i + 2000).ToString()));
                }
                ts = DateTime.Now.Ticks - t;
                Console.Write((ts / 10000000.0).ToString("0.00000    "));
                t = DateTime.Now.Ticks;
                for (int i = 0; i < iT; i++)
                {
                    arriisNTuple.Add((i + 10, i + 1000, (i + 2000).ToString()));
                }
                ts = DateTime.Now.Ticks - t;
                Console.Write((ts / 10000000.0).ToString("0.00000     "));
                t = DateTime.Now.Ticks;
                for (int i = 0; i < iT; i++)
                {
                    arriisClass.Add(new tag_i_i_s() { x = i + 10, y = i + 1000, msg = (i + 2000).ToString() });
                }
                ts = DateTime.Now.Ticks - t;
                Console.Write((ts / 10000000.0).ToString("0.00000  "));
                #endregion

                #region // i s d s  rowCreate
                t = DateTime.Now.Ticks;
                for (int i = 0; i < iT; i++)
                {
                    arrisdsTuple.Add(new Tuple<int, string, decimal, string>(i + 10, (i + 2000).ToString(), i * 16.6M, (i - 3000).ToString()));
                }
                ts = DateTime.Now.Ticks - t;
                Console.Write((ts / 10000000.0).ToString("0.00000       "));
                t = DateTime.Now.Ticks;
                for (int i = 0; i < iT; i++)
                {
                    arrisdsNTuple.Add((i + 10, (i + 2000).ToString(), i * 16.6M, (i - 3000).ToString()));
                }
                ts = DateTime.Now.Ticks - t;
                Console.Write((ts / 10000000.0).ToString("0.00000   "));
                t = DateTime.Now.Ticks;
                for (int i = 0; i < iT; i++)
                {
                    arrisdsClass.Add(new tag_i_s_d_s() { x = i + 10, msg = (i + 2000).ToString(), d = i * 16.6M, msg2 = (i - 3000).ToString() });
                }
                ts = DateTime.Now.Ticks - t;
                Console.Write((ts / 10000000.0).ToString("0.00000"));
                #endregion
                Console.WriteLine("");
                Console.Write(iT.ToString("000000") + "--  Use   ");
                #region // i i Row Use
                StringBuilder sb = new StringBuilder();
                t = DateTime.Now.Ticks;
                for (int i = 0; i < iT; i++)
                {
                    sb.AppendLine(string.Format("X={0} , Y = {1}", arriiTuple[i].Item1.ToString(), arriiTuple[i].Item2.ToString()));
                }
                ts = DateTime.Now.Ticks - t;
                Console.Write((ts / 10000000.0).ToString("0.00000    "));
                sb = new StringBuilder();
                t = DateTime.Now.Ticks;
                for (int i = 0; i < iT; i++)
                {
                    sb.AppendLine(string.Format("X={0} , Y = {1}", arriiNTuple[i].x.ToString(), arriiNTuple[i].y.ToString()));
                }
                ts = DateTime.Now.Ticks - t;
                Console.Write((ts / 10000000.0).ToString("0.00000   ") );
                sb = new StringBuilder();
                t = DateTime.Now.Ticks;
                for (int i = 0; i < iT; i++)
                {
                    sb.AppendLine(string.Format("X={0} , Y = {1}", arriiClass[i].x.ToString(), arriiClass[i].y.ToString()));
                }
                ts = DateTime.Now.Ticks - t;
                Console.Write((ts / 10000000.0).ToString("0.00000   "));
                #endregion

                #region // i i s Row Use
                sb = new StringBuilder();
                t = DateTime.Now.Ticks;
                for (int i = 0; i < iT; i++)
                {
                    sb.AppendLine(string.Format("X={0} , Y = {1} , MSG = {2} ",
                            arriisTuple[i].Item1.ToString(), arriisTuple[i].Item2.ToString(), arriisTuple[i].Item3));
                }
                ts = DateTime.Now.Ticks - t;
                Console.Write((ts / 10000000.0).ToString("0.00000    "));
                sb = new StringBuilder();
                t = DateTime.Now.Ticks;
                for (int i = 0; i < iT; i++)
                {
                    sb.AppendLine(string.Format("X={0} , Y = {1} , MSG = {2}",
                                arriisNTuple[i].x.ToString(), arriisNTuple[i].y.ToString(), arriisNTuple[i].msg));
                }
                ts = DateTime.Now.Ticks - t;
                Console.Write((ts / 10000000.0).ToString("0.00000     "));
                sb = new StringBuilder();
                t = DateTime.Now.Ticks;
                for (int i = 0; i < iT; i++)
                {
                    sb.AppendLine(string.Format("X={0} , Y = {1} , MSG = {2}",
                                arriisClass[i].x.ToString(), arriisClass[i].y.ToString(), arriisClass[i].msg));
                }
                ts = DateTime.Now.Ticks - t;
                Console.Write((ts / 10000000.0).ToString("0.00000  "));
                #endregion

                #region // i s d s Row Use
                sb = new StringBuilder();
                t = DateTime.Now.Ticks;
                for (int i = 0; i < iT; i++)
                {
                    sb.AppendLine(string.Format("X={0} , MSG = {1} , D = {2} , MSG2 = {3}",
                            arrisdsTuple[i].Item1.ToString(), arrisdsTuple[i].Item2, arrisdsTuple[i].Item3.ToString(), arrisdsTuple[i].Item4));
                }
                ts = DateTime.Now.Ticks - t;
                Console.Write((ts / 10000000.0).ToString("0.00000       "));
                sb = new StringBuilder();
                t = DateTime.Now.Ticks;
                for (int i = 0; i < iT; i++)
                {
                    sb.AppendLine(string.Format("X={0} , MSG = {1} , D = {2} , MSG2 = {3}",
                                arrisdsNTuple[i].x.ToString(), arrisdsNTuple[i].msg, arrisdsNTuple[i].d.ToString(), arrisdsNTuple[i].msg2));
                }
                ts = DateTime.Now.Ticks - t;
                Console.Write((ts / 10000000.0).ToString("0.00000   "));
                sb = new StringBuilder();
                t = DateTime.Now.Ticks;
                for (int i = 0; i < iT; i++)
                {
                    sb.AppendLine(string.Format("X={0} , MSG = {1} , D = {2} , MSG2 = {3}",
                                arrisdsClass[i].x.ToString(), arrisdsClass[i].msg, arrisdsClass[i].d.ToString(), arrisdsClass[i].msg2));
                }
                ts = DateTime.Now.Ticks - t;
                Console.Write((ts / 10000000.0).ToString("0.00000 "));
                #endregion
                Console.WriteLine("");
            }
            Console.ReadKey();
        }

        private class tag_i_i
        {
            public int x;
            public int y;
        }
        private class tag_i_i_s
        {
            public int x;
            public int y;
            public string msg;
        }

        private class tag_i_s_d_s
        {
            public int x;
            public string msg;
            public decimal d;
            public string msg2;
        }
        private void Test_i_s_d_s(int iT)
        {
            

            
        }
    }
}
