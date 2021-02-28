using System;
using System.Collections.Generic;
using System.Text;

namespace NetCorePerformanceTest
{
    public class StringProcessCompare :PerformanceBase, IDisposable
    {
        

        public StringProcessCompare()
        {
            arrTimes = new List<int>()
            { 1000,10000 ,100000}; 
            iExecTimes = 10;
            arrTestFunction = new List<Tuple<string, emTestType, Func<int, Func<object>, string>>>()
            {
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("Direct+(WF)", emTestType.Performance , DirectPlus) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("Direct+(WF)_L", emTestType.Performance , DirectPlus_L) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("Direct+(NF)" , emTestType.Performance, DirectPlus_NF) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("Direct+(NF)_L" , emTestType.Performance, DirectPlus_NF_L) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("Direct+(SF)" , emTestType.Performance, DirectPlus_SF) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("Direct+(SF)_L" , emTestType.Performance, DirectPlus_SF_L) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("StringFormat" , emTestType.Performance, StringFormat) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("StringFormat_L" , emTestType.Performance, StringFormat_L) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("string.Format(InF)" , emTestType.Performance, StringFormat_InF) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("string.Format(InF)_L" , emTestType.Performance, StringFormat_InF_L) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("Interpolated" , emTestType.Performance, Interp) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("Interpolated_L" , emTestType.Performance, Interp_L) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("Interpolated2" , emTestType.Performance, Interpolated2) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("Interpolated2_L" , emTestType.Performance, Interpolated2_L) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("Special1" , emTestType.Performance, Special1) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("Special2" , emTestType.Performance, Special2) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("StringBuilder_L" , emTestType.Performance, StringBuilder_L) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("Direct+ MultiLine_L" , emTestType.Performance, DirectPlus_MultiLine_L) ,
            };
        }
        public void Dispose()
        {

        }
        #region // Local Define

        private class DTest
        {
            public int idx { get; set; }
            public string sIdx { get; set; }
            public string sIdx2 { get; set; }
            public string sIdx3 { get; set; }
            public DateTime DayDate { get; set; }
            public DateTime DayDate2 { get; set; }
            public DateTime DayDate3 { get; set; }
            public double dVal { get; set; }
            public double dVal2 { get; set; }
            public double dVal3 { get; set; }
            public decimal ddVal { get; set; }
            public decimal ddVal2 { get; set; }
            public decimal ddVal3 { get; set; }
        }

        private class DLTest
        {
            public int idx { get; set; }
            public string sIdx { get; set; }
            public string sIdx2 { get; set; }
            public string sIdx3 { get; set; }
            public string sIdx4 { get; set; }
            public string sIdx5 { get; set; }
            public string sIdx6 { get; set; }
            public DateTime DayDate { get; set; }
            public DateTime DayDate2 { get; set; }
            public DateTime DayDate3 { get; set; }
            public DateTime DayDate4 { get; set; }
            public DateTime DayDate5 { get; set; }
            public DateTime DayDate6 { get; set; }
            public double dVal { get; set; }
            public double dVal2 { get; set; }
            public double dVal3 { get; set; }
            public double dVal4 { get; set; }
            public double dVal5 { get; set; }
            public double dVal6 { get; set; }
            public decimal ddVal { get; set; }
            public decimal ddVal2 { get; set; }
            public decimal ddVal3 { get; set; }
            public decimal ddVal4 { get; set; }
            public decimal ddVal5 { get; set; }
            public decimal ddVal6 { get; set; }
        }
        #endregion

        #region // tool Function
        private DTest CreateForTests(int i)
        {
            DTest t = new DTest();
            t.DayDate = DateTime.Now.AddDays(rnd.Next(10, 20000)).AddHours(rnd.Next(0, 10)).AddMinutes(rnd.Next(0, 40));
            t.DayDate2 = DateTime.Now.AddDays(rnd.Next(10, 20000)).AddHours(rnd.Next(0, 10)).
                 AddMinutes(rnd.Next(0, 40));
            t.DayDate3 = DateTime.Now.AddDays(rnd.Next(10, 20000)).AddHours(rnd.Next(0, 10)).
                AddMinutes(rnd.Next(0, 40));
            t.ddVal = (decimal)rnd.NextDouble() * 1000000M;
            t.ddVal2 = (decimal)rnd.NextDouble() * 1000000M;
            t.ddVal3 = (decimal)rnd.NextDouble() * 1000000M;
            t.dVal = rnd.NextDouble() * 100000;
            t.dVal2 = rnd.NextDouble() * 100000;
            t.dVal3 = rnd.NextDouble() * 100000;
            t.idx = rnd.Next(0, 1000000);
            t.sIdx = "XXXYYYZZZ" + (rnd.NextDouble() * 100000).ToString();
            t.sIdx2 = "XXXYYYZZZ2" + (rnd.NextDouble() * 100000).ToString();
            t.sIdx3 = "XXXYYYZZZ3" + (rnd.NextDouble() * 100000).ToString();
            return t;
        }
        private DLTest CreateForTestL(int i)
        {
            DLTest t = new DLTest();
            t.DayDate = DateTime.Now.AddDays(rnd.Next(10, 20000)).AddHours(rnd.Next(0, 10)).AddMinutes(rnd.Next(0, 40));
            t.DayDate2 = DateTime.Now.AddDays(rnd.Next(10, 20000)).AddHours(rnd.Next(0, 10)).
                                    AddMinutes(rnd.Next(0, 40));
            t.DayDate3 = DateTime.Now.AddDays(rnd.Next(10, 20000)).AddHours(rnd.Next(0, 10)).
                                    AddMinutes(rnd.Next(0, 40));
            t.DayDate4 = DateTime.Now.AddDays(rnd.Next(10, 20000)).AddHours(rnd.Next(0, 10)).
                                    AddMinutes(rnd.Next(0, 40));
            t.DayDate5 = DateTime.Now.AddDays(rnd.Next(10, 20000)).AddHours(rnd.Next(0, 10)).
                                    AddMinutes(rnd.Next(0, 40));
            t.DayDate6 = DateTime.Now.AddDays(rnd.Next(10, 20000)).AddHours(rnd.Next(0, 10)).
                                    AddMinutes(rnd.Next(0, 40));
            t.ddVal = (decimal)rnd.NextDouble() * 1000000M;
            t.ddVal2 = (decimal)rnd.NextDouble() * 1000000M;
            t.ddVal3 = (decimal)rnd.NextDouble() * 1000000M;
            t.ddVal4 = (decimal)rnd.NextDouble() * 1000000M;
            t.ddVal5 = (decimal)rnd.NextDouble() * 1000000M;
            t.ddVal6 = (decimal)rnd.NextDouble() * 1000000M;

            t.dVal = rnd.NextDouble() * 100000;
            t.dVal2 = rnd.NextDouble() * 100000;
            t.dVal3 = rnd.NextDouble() * 100000;
            t.dVal4 = rnd.NextDouble() * 100000;
            t.dVal5 = rnd.NextDouble() * 100000;
            t.dVal6 = rnd.NextDouble() * 100000;

            t.idx = rnd.Next(0, 1000000);

            t.sIdx = "XXXYYYZZZ" + (rnd.NextDouble() * 100000).ToString();
            t.sIdx2 = "XXXYYYZZZ2" + (rnd.NextDouble() * 100000).ToString();
            t.sIdx3 = "XXXYYYZZZ3" + (rnd.NextDouble() * 100000).ToString();
            t.sIdx4 = "XXXYYYZZZ3" + (rnd.NextDouble() * 100000).ToString();
            t.sIdx5 = "XXXYYYZZZ3" + (rnd.NextDouble() * 100000).ToString();
            t.sIdx6 = "XXXYYYZZZ3" + (rnd.NextDouble() * 100000).ToString();
            return t;
        }

        List<DTest> arrS = new List<DTest>();
        List<DLTest> arrL = new List<DLTest>();
        protected override int InitData(int iT)
        {
            arrS.Clear();
            arrL.Clear();
            for (int i = 0; i < iT; i++)
            {
                arrS.Add(CreateForTests(i));
                arrL.Add(CreateForTestL(i));
            }
            return 1;
        }
        #endregion

        private Random rnd = null;
        public void Run()
        {
            rnd = new Random((int)DateTime.Now.Ticks);
            PerformanceRunner runner = new PerformanceRunner(
                    arrTimes, iExecTimes, InitData, arrTestFunction, null, Message);
            string[][] retResult = runner.Run();
            //CommonTools.ShowOnGrid(dataGridView1, retResult);
            Console.WriteLine("Samples:");
            Console.WriteLine("Direct+:idx.ToString() , sIdx(1,2,3),DayDate(1,2,3).ToString(),ddVal(1,2,3).ToString(#,##0.000000)..");
            Console.WriteLine("Direct+(SF):idx , sIdx(1,2,3),DayDate(1,2,3),ddVal(1,2,3)..");
            Console.WriteLine("Direct+(NF):idx , sIdx(1,2,3),DayDate(1,2,3).ToString(),ddVal(1,2,3)..");
            Console.WriteLine("string.Format with ToString:string.Format({0},{1},{2},{3}:{4},{5},{6}X{7}X{8}X{9},{10},{11},{12}..");
            Console.WriteLine("string.Format with Format:string.Format({0},{1},{2},{3}:{4:yyyy-MM-dd HH:mm:ss},{5:yyyy-MM-dd HH:mm:ss},{6:yyyy-MM-dd HH:mm:ss}X...");
            Console.WriteLine("Interpolated(Format):${arrS[i].idx},{arrS[i].sIdx},{arrS[i].sIdx2},{arrS[i].sIdx3}:{arrS[i].DayDate:yyyy-MM-dd HH:ss:ss}:{arrS[i].DayDate2:yyyy-MM-dd HH:ss:ss}...");
            Console.WriteLine("Interpolated(ToString):${arrS[i].idx},{arrS[i].sIdx},{arrS[i].sIdx2},{arrS[i].sIdx3}:{arrS[i].DayDate.ToString(yyyy-MM-dd HH:ss:ss)}:{arrS[i].DayDate2:yyyy-MM-dd HH:ss:ss}:...");
            Console.WriteLine("Times   Direct+    Direct+(NF)  Direct+(SF)  string.Format(T)    string.Format(F)  Interpolated(Format)  Interpolated(ToString) ,Special1   Special2");
            ShowResult(retResult);
            
        }

        #region // Short Sample
        private string DirectPlus(int iT, Func<object> GetDate)
        {
            //List<DTest> arrS = (List<DTest>)GetDate();
            List<string> arr = new List<string>(2);
            for (int i = 0; i < iT; i++)
            { // string.Format("{0},{1}:{2},{3},{4}",
                string t = arrS[i].idx.ToString() + "," +
                    arrS[i].sIdx + ":" + arrS[i].sIdx2 + "," + arrS[i].sIdx3 +
                    arrS[i].DayDate.ToString("yyyy-MM-dd HH:mm:ss") + "," + arrS[i].DayDate2.ToString("yyyy-MM-dd HH:mm:ss") + "," + arrS[i].DayDate3.ToString("yyyy-MM-dd HH:mm:ss") + ","
                    + arrS[i].ddVal.ToString("#,##0.000000") + "," + arrS[i].ddVal2.ToString("#,##0.000000") + "," + arrS[i].ddVal3.ToString("#,##0.000000") + "," +
                    arrS[i].dVal.ToString("#,##0.000000") + "," + arrS[i].dVal2.ToString("#,##0.000000") + "," + arrS[i].dVal3.ToString("#,##0.000000");
                arr.Add(t);
                if (arr.Count == 2)
                    arr.Clear();
            }
            return "";
        }

        private string DirectPlus_NF(int iT, Func<object> GetDate)
        {
            //List<DTest> arrS = (List<DTest>)GetDate();
            List<string> arr = new List<string>(2);
            for (int i = 0; i < iT; i++)
            { // string.Format("{0},{1}:{2},{3},{4}",
                string t = arrS[i] + "," +
                    arrS[i].sIdx + ":" + arrS[i].sIdx2 + "," + arrS[i].sIdx3 +
                    arrS[i].DayDate.ToString("yyyy-MM-dd HH:mm:ss") + "," + arrS[i].DayDate2.ToString("yyyy-MM-dd HH:mm:ss") + "," + arrS[i].DayDate3.ToString("yyyy-MM-dd HH:mm:ss") + ","
                    + arrS[i].ddVal + "," + arrS[i].ddVal2 + "," + arrS[i].ddVal3 + "," +
                    arrS[i].dVal + "," + arrS[i].dVal2 + "," + arrS[i].dVal3;
                arr.Add(t);
                if (arr.Count == 2)
                    arr.Clear();
            }
            return "";
        }
        private string DirectPlus_SF(int iT, Func<object> GetDate)
        {
            //List<DTest> arrS = (List<DTest>)GetDate();
            List<string> arr = new List<string>(2);
            for (int i = 0; i < iT; i++)
            { // string.Format("{0},{1}:{2},{3},{4}",
                string t = arrS[i].idx.ToString() + "," +
                    arrS[i].sIdx + ":" + arrS[i].sIdx2 + "," + arrS[i].sIdx3 +
                    arrS[i].DayDate.ToString("yyyy-MM-dd HH:mm:ss") + "," + arrS[i].DayDate2.ToString("yyyy-MM-dd HH:mm:ss") + "," + arrS[i].DayDate3.ToString("yyyy-MM-dd HH:mm:ss") + ","
                    + arrS[i].ddVal.ToString() + "," + arrS[i].ddVal2.ToString() + "," + arrS[i].ddVal3.ToString() + "," +
                    arrS[i].dVal.ToString() + "," + arrS[i].dVal2.ToString() + "," + arrS[i].dVal3.ToString();
                arr.Add(t);
                if (arr.Count == 2)
                    arr.Clear();
            }
            return "";
        }

        private string StringFormat(int iT, Func<object> GetDate)
        {
            //List<DTest> arrS = (List<DTest>)GetDate();
            List<string> arr = new List<string>(2);
            for (int i = 0; i < iT; i++)
            {
                string t = string.Format("{0},{1},{2},{3}:{4},{5},{6}X{7}X{8}X{9},{10},{11},{12}", arrS[i].idx.ToString(),
                    arrS[i].sIdx, arrS[i].sIdx2, arrS[i].sIdx3,
                    arrS[i].DayDate.ToString("yyyy-MM-dd HH:mm:ss"), arrS[i].DayDate2.ToString("yyyy-MM-dd HH:mm:ss"), arrS[i].DayDate3.ToString("yyyy-MM-dd HH:mm:ss"),
                    arrS[i].ddVal.ToString("#,##0.000000"), arrS[i].ddVal2.ToString("#,##0.000000"), arrS[i].ddVal3.ToString("#,##0.000000"),
                    arrS[i].dVal.ToString("#,##0.000000"), arrS[i].dVal2.ToString("#,##0.000000"), arrS[i].dVal3.ToString("#,##0.000000"));
                arr.Add(t);
                if (arr.Count == 2)
                    arr.Clear();
            }
            return "";
        }

        private string StringFormat_InF(int iT, Func<object> GetDate)
        {
            //List<DTest> arrS = (List<DTest>)GetDate();
            List<string> arr = new List<string>(2);
            for (int i = 0; i < iT; i++)
            {
                string t = string.Format("{0},{1},{2},{3}:{4:yyyy-MM-dd HH:mm:ss},{5:yyyy-MM-dd HH:mm:ss},{6:yyyy-MM-dd HH:mm:ss}X{7:#,##0.000000}X{8:#,##0.000000}X{9:#,##0.000000},{10:#,##0.000000},{11:#,##0.000000},{12:#,##0.000000}", arrS[i].idx.ToString(),
                    arrS[i].sIdx, arrS[i].sIdx2, arrS[i].sIdx3,
                    arrS[i].DayDate, arrS[i].DayDate2, arrS[i].DayDate3,
                    arrS[i].ddVal, arrS[i].ddVal2, arrS[i].ddVal3,
                    arrS[i].dVal, arrS[i].dVal2, arrS[i].dVal3);
                arr.Add(t);
                if (arr.Count == 2)
                    arr.Clear();
            }
            return "";
        }

        private string Interp(int iT, Func<object> GetDate)
        {
            //List<DTest> arrS = (List<DTest>)GetDate();
            List<string> arr = new List<string>(2);
            for (int i = 0; i < iT; i++)
            {
                string t = $"{arrS[i].idx},{arrS[i].sIdx},{arrS[i].sIdx2},{arrS[i].sIdx3}:{arrS[i].DayDate:yyyy-MM-dd HH:ss:ss}:{arrS[i].DayDate2:yyyy-MM-dd HH:ss:ss}:{arrS[i].DayDate3:yyyy-MM-dd HH:ss:ss},{arrS[i].ddVal:#,##0.000000},{arrS[i].ddVal2:#,##0.000000},{arrS[i].ddVal3:#,##0.000000},{arrS[i].dVal:#,##0.0000},{arrS[i].dVal2:#,##0.0000},{arrS[i].dVal3:#,##0.0000}";
                arr.Add(t);
                if (arr.Count == 2)
                    arr.Clear();
            }
            return "";
        }
        private string Interpolated2(int iT, Func<object> GetDate)
        {
            //List<DTest> arrS = (List<DTest>)GetDate();
            List<string> arr = new List<string>(2);
            for (int i = 0; i < iT; i++)
            {
                string t = $"{arrS[i].idx},{arrS[i].sIdx},{arrS[i].sIdx2},{arrS[i].sIdx3}:{arrS[i].DayDate.ToString("yyyy-MM-dd HH:ss:ss")}:{arrS[i].DayDate2.ToString("yyyy-MM-dd HH:ss:ss")}:{arrS[i].DayDate3.ToString("yyyy-MM-dd HH:ss:ss")},{arrS[i].ddVal.ToString("#,##0.000000")},{arrS[i].ddVal2.ToString("#,##0.000000")},{arrS[i].ddVal3.ToString("#,##0.000000")},{arrS[i].dVal.ToString("#,##0.0000")},{arrS[i].dVal2.ToString("#,##0.0000")},{arrS[i].dVal3.ToString("#,##0.0000")}";
                arr.Add(t);
                if (arr.Count == 2)
                    arr.Clear();
            }
            return "";
        }

        private string Special1(int iT, Func<object> GetDate)
        {
            //List<DTest> arrS = (List<DTest>)GetDate();
            List<string> arr = new List<string>(2);
            for (int i = 0; i < iT; i++)
            { // string.Format("{0},{1}:{2},{3},{4}",
                string t = arrS[i].idx + "," +
                    arrS[i].sIdx + ":" + arrS[i].sIdx2 + "," + arrS[i].sIdx3 +
                    arrS[i].DayDate.Date.ToOADate() + "," + arrS[i].DayDate2.Date.ToOADate() + "," + arrS[i].DayDate3.Date.ToOADate() + ","
                    + arrS[i].ddVal + "," + arrS[i].ddVal2 + "," + arrS[i].ddVal3 + "," +
                    arrS[i].dVal + "," + arrS[i].dVal2 + "," + arrS[i].dVal3;
                arr.Add(t);
                if (arr.Count == 2)
                    arr.Clear();
            }
            return "";
        }
        private string Special2(int iT, Func<object> GetDate)
        {
            // List<DTest> arrS = (List<DTest>)GetDate();
            List<string> arr = new List<string>(2);
            for (int i = 0; i < iT; i++)
            { // string.Format("{0},{1}:{2},{3},{4}",
                string t = arrS[i].idx + "," +
                    arrS[i].sIdx.ToString() + ":" + arrS[i].sIdx2.ToString() + "," + arrS[i].sIdx3.ToString() +
                    arrS[i].DayDate.Date.ToOADate().ToString() + "," + arrS[i].DayDate2.Date.ToOADate().ToString() + "," + arrS[i].DayDate3.Date.ToOADate().ToString() + ","
                    + arrS[i].ddVal.ToString() + "," + arrS[i].ddVal2.ToString() + "," + arrS[i].ddVal3.ToString() + "," +
                    arrS[i].dVal.ToString() + "," + arrS[i].dVal2.ToString() + "," + arrS[i].dVal3.ToString();
                arr.Add(t);
                if (arr.Count == 2)
                    arr.Clear();
            }
            return "";
        }
        #endregion

        #region // Test Functions

        /// <summary>
        /// 直接串接
        /// </summary>
        /// <param name="iT"></param>
        /// <param name="GetDate"></param>
        /// <returns></returns>
        private string DirectPlus_L(int iT, Func<object> GetDate)
        {
            List<DLTest> arrS = arrL;// (List<DLTest>)GetDate();
            List<string> arr = new List<string>(2);
            for (int i = 0; i < iT; i++)
            { // string.Format("{0},{1}:{2},{3},{4}",
                string t = arrS[i].idx.ToString() + "," +
                    arrS[i].sIdx + ":" + arrS[i].sIdx2 + "," + arrS[i].sIdx3 + "," +
                    arrS[i].sIdx4 + ":" + arrS[i].sIdx5 + "," + arrS[i].sIdx6 + "," +
                    arrS[i].DayDate.ToString("yyyy-MM-dd HH:mm:ss") + "," +
                    arrS[i].DayDate2.ToString("yyyy-MM-dd HH:mm:ss") + "," +
                    arrS[i].DayDate3.ToString("yyyy-MM-dd HH:mm:ss") + "," +
                    arrS[i].DayDate4.ToString("yyyy-MM-dd HH:mm:ss") + "," +
                    arrS[i].DayDate5.ToString("yyyy-MM-dd HH:mm:ss") + "," +
                    arrS[i].DayDate6.ToString("yyyy-MM-dd HH:mm:ss") + "," +
                    arrS[i].ddVal.ToString("#,##0.000000") + "," + arrS[i].ddVal2.ToString("#,##0.000000") + "," + arrS[i].ddVal3.ToString("#,##0.000000") + "," +
                    arrS[i].ddVal4.ToString("#,##0.000000") + "," + arrS[i].ddVal5.ToString("#,##0.000000") + "," + arrS[i].ddVal6.ToString("#,##0.000000") + "," +
                    arrS[i].dVal.ToString("#,##0.000000") + "," + arrS[i].dVal2.ToString("#,##0.000000") + "," + arrS[i].dVal3.ToString("#,##0.000000") + "," +
                    arrS[i].dVal4.ToString("#,##0.000000") + "," + arrS[i].dVal5.ToString("#,##0.000000") + "," + arrS[i].dVal6.ToString("#,##0.000000");
                arr.Add(t);
                if (arr.Count == 2)
                    arr.Clear();
            }
            return "";
        }
        /// <summary>
        /// 直接串接-不加Format
        /// </summary>
        /// <param name="iT"></param>
        /// <param name="GetDate"></param>
        /// <returns></returns>
        private string DirectPlus_NF_L(int iT, Func<object> GetDate)
        {
            List<DLTest> arrS = arrL;// (List<DLTest>)GetDate();
            List<string> arr = new List<string>(2);
            for (int i = 0; i < iT; i++)
            { // string.Format("{0},{1}:{2},{3},{4}",
                string t = arrS[i] + "," +
                    arrS[i].sIdx + ":" + arrS[i].sIdx2 + "," + arrS[i].sIdx3 + "," +
                    arrS[i].sIdx4 + ":" + arrS[i].sIdx5 + "," + arrS[i].sIdx6 + "," +
                    arrS[i].DayDate.ToString("yyyy-MM-dd HH:mm:ss") + "," + arrS[i].DayDate2.ToString("yyyy-MM-dd HH:mm:ss") + "," + arrS[i].DayDate3.ToString("yyyy-MM-dd HH:mm:ss") + "," +
                    arrS[i].DayDate4.ToString("yyyy-MM-dd HH:mm:ss") + "," + arrS[i].DayDate5.ToString("yyyy-MM-dd HH:mm:ss") + "," + arrS[i].DayDate6.ToString("yyyy-MM-dd HH:mm:ss") + "," +
                    arrS[i].ddVal + "," + arrS[i].ddVal2 + "," + arrS[i].ddVal3 + "," +
                    arrS[i].ddVal4 + "," + arrS[i].ddVal5 + "," + arrS[i].ddVal6 + "," +
                    arrS[i].dVal + "," + arrS[i].dVal2 + "," + arrS[i].dVal3 + "," +
                    arrS[i].dVal4 + "," + arrS[i].dVal5 + "," + arrS[i].dVal6;
                arr.Add(t);
                if (arr.Count == 2)
                    arr.Clear();
            }
            return "";
        }
        /// <summary>
        /// 直接串接 - ToString 不加 Format
        /// </summary>
        /// <param name="iT"></param>
        /// <param name="GetDate"></param>
        /// <returns></returns>
        private string DirectPlus_SF_L(int iT, Func<object> GetDate)
        {
            List<DLTest> arrS = arrL;// (List<DLTest>)GetDate();
            List<string> arr = new List<string>(2);
            for (int i = 0; i < iT; i++)
            { // string.Format("{0},{1}:{2},{3},{4}",
                string t = arrS[i].idx.ToString() + "," +
                    arrS[i].sIdx + ":" + arrS[i].sIdx2 + "," + arrS[i].sIdx3 + "," +
                    arrS[i].sIdx4 + ":" + arrS[i].sIdx5 + "," + arrS[i].sIdx6 + "," +
                    arrS[i].DayDate.ToString("yyyy-MM-dd HH:mm:ss") + "," + arrS[i].DayDate2.ToString("yyyy-MM-dd HH:mm:ss") + "," + arrS[i].DayDate3.ToString("yyyy-MM-dd HH:mm:ss") + "," +
                    arrS[i].DayDate4.ToString("yyyy-MM-dd HH:mm:ss") + "," + arrS[i].DayDate5.ToString("yyyy-MM-dd HH:mm:ss") + "," + arrS[i].DayDate6.ToString("yyyy-MM-dd HH:mm:ss") + "," +
                    arrS[i].ddVal.ToString() + "," + arrS[i].ddVal2.ToString() + "," + arrS[i].ddVal3.ToString() + "," +
                    arrS[i].ddVal4.ToString() + "," + arrS[i].ddVal5.ToString() + "," + arrS[i].ddVal6.ToString() + "," +
                    arrS[i].dVal.ToString() + "," + arrS[i].dVal2.ToString() + "," + arrS[i].dVal3.ToString() + "," +
                    arrS[i].dVal4.ToString() + "," + arrS[i].dVal5.ToString() + "," + arrS[i].dVal6.ToString();
                arr.Add(t);
                if (arr.Count == 2)
                    arr.Clear();
            }
            return "";
        }
        /// <summary>
        /// String.Format ,格式 写在外面
        /// </summary>
        /// <param name="iT"></param>
        /// <param name="GetDate"></param>
        /// <returns></returns>
        private string StringFormat_L(int iT, Func<object> GetDate)
        {
            List<DLTest> arrS = arrL;// (List<DLTest>)GetDate();
            List<string> arr = new List<string>(2);
            for (int i = 0; i < iT; i++)
            {
                string t = string.Format("{0},{1},{2},{3},{4},{5},{6}:{7},{8},{9},{10},{11},{12}X{13}X{14}X{15}X{16}X{17}X{18},{19},{20},{21},{22},{24},{24}", arrS[i].idx.ToString(),
                    arrS[i].sIdx, arrS[i].sIdx2, arrS[i].sIdx3,
                    arrS[i].sIdx4, arrS[i].sIdx5, arrS[i].sIdx6,
                    arrS[i].DayDate.ToString("yyyy-MM-dd HH:mm:ss"), arrS[i].DayDate2.ToString("yyyy-MM-dd HH:mm:ss"), arrS[i].DayDate3.ToString("yyyy-MM-dd HH:mm:ss"),
                    arrS[i].DayDate4.ToString("yyyy-MM-dd HH:mm:ss"), arrS[i].DayDate5.ToString("yyyy-MM-dd HH:mm:ss"), arrS[i].DayDate6.ToString("yyyy-MM-dd HH:mm:ss"),
                    arrS[i].ddVal.ToString("#,##0.000000"), arrS[i].ddVal2.ToString("#,##0.000000"), arrS[i].ddVal3.ToString("#,##0.000000"),
                    arrS[i].ddVal4.ToString("#,##0.000000"), arrS[i].ddVal5.ToString("#,##0.000000"), arrS[i].ddVal6.ToString("#,##0.000000"),
                    arrS[i].dVal.ToString("#,##0.000000"), arrS[i].dVal2.ToString("#,##0.000000"), arrS[i].dVal3.ToString("#,##0.000000"),
                    arrS[i].dVal4.ToString("#,##0.000000"), arrS[i].dVal5.ToString("#,##0.000000"), arrS[i].dVal6.ToString("#,##0.000000"));
                arr.Add(t);
                if (arr.Count == 2)
                    arr.Clear();
            }
            return "";
        }
        /// <summary>
        /// String.Format ,格式 写在里面
        /// </summary>
        /// <param name="iT"></param>
        /// <param name="GetDate"></param>
        /// <returns></returns>
        private string StringFormat_InF_L(int iT, Func<object> GetDate)
        {
            List<DLTest> arrS = arrL;// (List<DLTest>)GetDate();
            List<string> arr = new List<string>(2);
            for (int i = 0; i < iT; i++)
            {
                string t = string.Format("{0},{1},{2},{3},{4},{5},{6}:{7:yyyy-MM-dd HH:mm:ss},{8:yyyy-MM-dd HH:mm:ss},{9:yyyy-MM-dd HH:mm:ss}:{10:yyyy-MM-dd HH:mm:ss},{11:yyyy-MM-dd HH:mm:ss},{12:yyyy-MM-dd HH:mm:ss}X{13:#,##0.000000}X{14:#,##0.000000}X{15:#,##0.000000}X{16:#,##0.000000}X{17:#,##0.000000}X{18:#,##0.000000},{19:#,##0.000000},{20:#,##0.000000},{21:#,##0.000000},{22:#,##0.000000},{24:#,##0.000000},{24:#,##0.000000}", arrS[i].idx.ToString(),
                    arrS[i].sIdx, arrS[i].sIdx2, arrS[i].sIdx3,
                    arrS[i].sIdx4, arrS[i].sIdx5, arrS[i].sIdx6,
                    arrS[i].DayDate, arrS[i].DayDate2, arrS[i].DayDate3,
                    arrS[i].DayDate4, arrS[i].DayDate5, arrS[i].DayDate6,
                    arrS[i].ddVal, arrS[i].ddVal2, arrS[i].ddVal3,
                    arrS[i].ddVal4, arrS[i].ddVal5, arrS[i].ddVal6,
                    arrS[i].dVal, arrS[i].dVal2, arrS[i].dVal3,
                    arrS[i].dVal4, arrS[i].dVal5, arrS[i].dVal6);
                arr.Add(t);
                if (arr.Count == 2)
                    arr.Clear();
            }
            return "";
        }

        /// <summary>
        /// String.Interp 内部格式
        /// </summary>
        /// <param name="iT"></param>
        /// <param name="GetDate"></param>
        /// <returns></returns>
        private string Interp_L(int iT, Func<object> GetDate)
        {
            List<DLTest> arrS = arrL;// (List<DLTest>)GetDate();
            List<string> arr = new List<string>(2);
            for (int i = 0; i < iT; i++)
            {
                string t = $"{arrS[i].idx},{arrS[i].sIdx},{arrS[i].sIdx2},{arrS[i].sIdx3},{arrS[i].sIdx4},{arrS[i].sIdx5},{arrS[i].sIdx6}:{arrS[i].DayDate:yyyy-MM-dd HH:ss:ss}:{arrS[i].DayDate2:yyyy-MM-dd HH:ss:ss}:{arrS[i].DayDate3:yyyy-MM-dd HH:ss:ss}:{arrS[i].DayDate4:yyyy-MM-dd HH:ss:ss}:{arrS[i].DayDate5:yyyy-MM-dd HH:ss:ss}:{arrS[i].DayDate6:yyyy-MM-dd HH:ss:ss},{arrS[i].ddVal:#,##0.000000},{arrS[i].ddVal2:#,##0.000000},{arrS[i].ddVal3:#,##0.000000},{arrS[i].ddVal4:#,##0.000000},{arrS[i].ddVal5:#,##0.000000},{arrS[i].ddVal6:#,##0.000000},{arrS[i].dVal:#,##0.0000},{arrS[i].dVal2:#,##0.0000},{arrS[i].dVal3:#,##0.0000},{arrS[i].dVal4:#,##0.0000},{arrS[i].dVal5:#,##0.0000},{arrS[i].dVal6:#,##0.0000}";
                arr.Add(t);
                if (arr.Count == 2)
                    arr.Clear();
            }
            return "";
        }
        /// <summary>
        /// String.Interp ToString格式
        /// </summary>
        /// <param name="iT"></param>
        /// <param name="GetDate"></param>
        /// <returns></returns>
        private string Interpolated2_L(int iT, Func<object> GetDate)
        {
            List<DLTest> arrS = arrL;// (List<DLTest>)GetDate();
            List<string> arr = new List<string>(2);
            for (int i = 0; i < iT; i++)
            {
                string t = $"{arrS[i].idx},{arrS[i].sIdx},{arrS[i].sIdx2},{arrS[i].sIdx3},{arrS[i].sIdx4},{arrS[i].sIdx5},{arrS[i].sIdx6}:{arrS[i].DayDate.ToString("yyyy-MM-dd HH:ss:ss")}:{arrS[i].DayDate2.ToString("yyyy-MM-dd HH:ss:ss")}:{arrS[i].DayDate3.ToString("yyyy-MM-dd HH:ss:ss")}:{arrS[i].DayDate4.ToString("yyyy-MM-dd HH:ss:ss")}:{arrS[i].DayDate5.ToString("yyyy-MM-dd HH:ss:ss")}:{arrS[i].DayDate6.ToString("yyyy-MM-dd HH:ss:ss")},{arrS[i].ddVal.ToString("#,##0.000000")},{arrS[i].ddVal2.ToString("#,##0.000000")},{arrS[i].ddVal3.ToString("#,##0.000000")},{arrS[i].ddVal4.ToString("#,##0.000000")},{arrS[i].ddVal5.ToString("#,##0.000000")},{arrS[i].ddVal6.ToString("#,##0.000000")},{arrS[i].dVal.ToString("#,##0.0000")},{arrS[i].dVal2.ToString("#,##0.0000")},{arrS[i].dVal3.ToString("#,##0.0000")},{arrS[i].dVal4.ToString("#,##0.0000")},{arrS[i].dVal5.ToString("#,##0.0000")},{arrS[i].dVal6.ToString("#,##0.0000")}";
                arr.Add(t);
                if (arr.Count == 2)
                    arr.Clear();
            }
            return "";
        }
        /// <summary>
        /// String Builder MultiLine
        /// </summary>
        /// <param name="iT"></param>
        /// <param name="GetDate"></param>
        /// <returns></returns>
        private string StringBuilder_L(int iT, Func<object> GetDate)
        {
            List<DLTest> arrS = arrL;// (List<DLTest>)GetDate();
            List<string> arr = new List<string>(2);
            for (int i = 0; i < iT; i++)
            {
                StringBuilder sb = new StringBuilder(arrS[i].idx.ToString());
                sb.Append(arrS[i].sIdx);
                sb.Append(arrS[i].sIdx2);
                sb.Append(arrS[i].sIdx3);
                sb.Append(arrS[i].sIdx4);
                sb.Append(arrS[i].sIdx5);
                sb.Append(arrS[i].sIdx6);
                sb.Append(arrS[i].DayDate.ToString("yyyy-MM-dd HH:ss:ss"));
                sb.Append(arrS[i].DayDate2.ToString("yyyy-MM-dd HH:ss:ss"));
                sb.Append(arrS[i].DayDate3.ToString("yyyy-MM-dd HH:ss:ss"));
                sb.Append(arrS[i].DayDate4.ToString("yyyy-MM-dd HH:ss:ss"));
                sb.Append(arrS[i].DayDate5.ToString("yyyy-MM-dd HH:ss:ss"));
                sb.Append(arrS[i].DayDate6.ToString("yyyy-MM-dd HH:ss:ss"));
                sb.Append(arrS[i].ddVal.ToString("#,##0.000000"));
                sb.Append(arrS[i].ddVal2.ToString("#,##0.000000"));
                sb.Append(arrS[i].ddVal3.ToString("#,##0.000000"));
                sb.Append(arrS[i].ddVal4.ToString("#,##0.000000"));
                sb.Append(arrS[i].ddVal5.ToString("#,##0.000000"));
                sb.Append(arrS[i].ddVal6.ToString("#,##0.000000"));
                sb.Append(arrS[i].dVal.ToString("#,##0.000000"));
                sb.Append(arrS[i].dVal2.ToString("#,##0.000000"));
                sb.Append(arrS[i].dVal3.ToString("#,##0.000000"));
                sb.Append(arrS[i].dVal4.ToString("#,##0.000000"));
                sb.Append(arrS[i].dVal5.ToString("#,##0.000000"));
                sb.Append(arrS[i].dVal6.ToString("#,##0.000000"));
                arr.Add(sb.ToString());
                if (arr.Count == 2)
                    arr.Clear();
            }
            return "";
        }

        /// <summary>
        /// 直接串接
        /// </summary>
        /// <param name="iT"></param>
        /// <param name="GetDate"></param>
        /// <returns></returns>
        private string DirectPlus_MultiLine_L(int iT, Func<object> GetDate)
        {
            List<DLTest> arrS = arrL;// (List<DLTest>)GetDate();
            List<string> arr = new List<string>(2);
            for (int i = 0; i < iT; i++)
            { // string.Format("{0},{1}:{2},{3},{4}",
                string t = arrS[i].idx.ToString() + ",";
                t = t + arrS[i].sIdx + ":";
                t = t + arrS[i].sIdx2 + ",";
                t = t + arrS[i].sIdx3 + ",";
                t = t + arrS[i].sIdx4 + ":";
                t = t + arrS[i].sIdx5 + ",";
                t = t + arrS[i].sIdx6 + ",";
                t = t + arrS[i].DayDate.ToString("yyyy-MM-dd HH:mm:ss") + ",";
                t = t + arrS[i].DayDate2.ToString("yyyy-MM-dd HH:mm:ss") + ",";
                t = t + arrS[i].DayDate3.ToString("yyyy-MM-dd HH:mm:ss") + ",";
                t = t + arrS[i].DayDate4.ToString("yyyy-MM-dd HH:mm:ss") + ",";
                t = t + arrS[i].DayDate5.ToString("yyyy-MM-dd HH:mm:ss") + ",";
                t = t + arrS[i].DayDate6.ToString("yyyy-MM-dd HH:mm:ss") + ",";
                t = t + arrS[i].ddVal.ToString("#,##0.000000") + ",";
                t = t + arrS[i].ddVal2.ToString("#,##0.000000") + ",";
                t = t + arrS[i].ddVal3.ToString("#,##0.000000") + ",";
                t = t + arrS[i].ddVal4.ToString("#,##0.000000") + ",";
                t = t + arrS[i].ddVal5.ToString("#,##0.000000") + ",";
                t = t + arrS[i].ddVal6.ToString("#,##0.000000") + ",";
                t = t + arrS[i].dVal.ToString("#,##0.000000") + ",";
                t = t + arrS[i].dVal2.ToString("#,##0.000000") + ",";
                t = t + arrS[i].dVal3.ToString("#,##0.000000") + ",";
                t = t + arrS[i].dVal4.ToString("#,##0.000000") + ",";
                t = t + arrS[i].dVal5.ToString("#,##0.000000") + ",";
                t = t + arrS[i].dVal6.ToString("#,##0.000000");
                arr.Add(t);
                if (arr.Count == 2)
                    arr.Clear();
            }
            return "";
        }
        #endregion
    }
}
