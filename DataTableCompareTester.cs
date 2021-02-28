using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace NetCorePerformanceTest
{
    public class DataTableCompareTester:PerformanceBase, IDisposable
    {
        public void Dispose()
        {
        }
        public DataTableCompareTester()
        {
            arrTimes = new List<int>()
            { 1000,10000,100000,200000 };
            iExecTimes = 10;
            arrTestFunction = new List<Tuple<string, emTestType, Func<int, Func<object>, string>>>()
            {
                #region // DataTable + GZip
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("01DataTable+GZip:Query", emTestType.Performance , DataTable_Query) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("02               RowCount", emTestType.NumbersForAvg , DataTable_QueryCount) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("03               Serialize", emTestType.Performance , DataTable_Serialize) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("04               SerializedSize" , emTestType.NumbersForAvg, DataTable_SerializedSize) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("05               Compress" , emTestType.Performance, GZip) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("06               CompressedSize" , emTestType.NumbersForAvg, GetCompressedSize) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("07               Decompress" , emTestType.Performance, UnGZip) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("08               CheckData" , emTestType.Log, DeCompressCheck) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("09               DeSerialize" , emTestType.Performance, DataTable_DeSerialize) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("10               DeSerializeCheck" , emTestType.Log, DataTable_DeSerializeCheck) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("11               ConvertToList" , emTestType.Performance, DataTable_ToList) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("12               TotalTimeUsed" , emTestType.NumbersForAvg, GetTotalTime) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("13               CompressUsed" , emTestType.NumbersForAvg, GetCompressUsedTime1) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("14               CompressUsedRatio" , emTestType.NumbersForAvg, GetCompressUsedTime2) ,
                #endregion
                #region // Simple + GZip
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("21SimpleDT+GZip:Query", emTestType.Performance , SimpleDT_Query) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("22               RowCount", emTestType.NumbersForAvg , SimpleDT_QueryCount) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("23               Serialize", emTestType.Performance , SimpleDT_Serialize) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("24               SerializedSize" , emTestType.NumbersForAvg, DataTable_SerializedSize) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("25               Compress" , emTestType.Performance, GZip) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("26               CompressedSize" , emTestType.NumbersForAvg, GetCompressedSize) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("27               Decompress" , emTestType.Performance, UnGZip) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("28               CheckData" , emTestType.Log, DeCompressCheck) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("29               DeSerialize" , emTestType.Performance, SimpleDT_DeSerialize) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("30               DeSerializeCheck" , emTestType.Log, SimpleDT_DeSerializeCheck) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("31               ConvertToList" , emTestType.Performance, SimpleDT_ToList) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("32               TotalTimeUsed" , emTestType.NumbersForAvg, GetTotalTime) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("33               CompressUsed" , emTestType.NumbersForAvg, GetCompressUsedTime1) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("34               CompressUsedRatio" , emTestType.NumbersForAvg, GetCompressUsedTime2) ,
                #endregion
                #region // DataTable Deflate
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("41DataTable+Deflate:Query", emTestType.Performance , DataTable_Query) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("42               RowCount", emTestType.NumbersForAvg , DataTable_QueryCount) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("43               Serialize", emTestType.Performance , DataTable_Serialize) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("44               SerializedSize" , emTestType.NumbersForAvg, DataTable_SerializedSize) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("45               Compress" , emTestType.Performance, Deflact) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("46               CompressedSize" , emTestType.NumbersForAvg, GetCompressedSize) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("47               Decompress" , emTestType.Performance, UnDeflact) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("48               CheckData" , emTestType.Log, DeCompressCheck) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("49               DeSerialize" , emTestType.Performance, DataTable_DeSerialize) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("50               DeSerializeCheck" , emTestType.Log, DataTable_DeSerializeCheck) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("51               ConvertToList" , emTestType.Performance, DataTable_ToList) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("52               TotalTimeUsed" , emTestType.NumbersForAvg, GetTotalTime) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("53               CompressUsed" , emTestType.NumbersForAvg, GetCompressUsedTime1) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("54               CompressUsedRatio" , emTestType.NumbersForAvg, GetCompressUsedTime2) ,
                #endregion
                #region // SimpleDT + Deflate
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("61SimpleDT+Deflate:Query", emTestType.Performance , SimpleDT_Query) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("62               RowCount", emTestType.NumbersForAvg , SimpleDT_QueryCount) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("63               Serialize", emTestType.Performance , SimpleDT_Serialize) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("64               SerializedSize" , emTestType.NumbersForAvg, DataTable_SerializedSize) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("65               Compress" , emTestType.Performance, Deflact) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("66               CompressedSize" , emTestType.NumbersForAvg, GetCompressedSize) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("67               Decompress" , emTestType.Performance, UnDeflact) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("68               CheckData" , emTestType.Log, DeCompressCheck) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("69               DeSerialize" , emTestType.Performance, SimpleDT_DeSerialize) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("70               DeSerializeCheck" , emTestType.Log, SimpleDT_DeSerializeCheck) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("71               ConvertToList" , emTestType.Performance, SimpleDT_ToList) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("72               TotalTimeUsed" , emTestType.NumbersForAvg, GetTotalTime) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("73               CompressUsed" , emTestType.NumbersForAvg, GetCompressUsedTime1) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("74               CompressUsedRatio" , emTestType.NumbersForAvg, GetCompressUsedTime2) ,
                #endregion
            };
        }
        string sqltmp = "Select top {0} * from prdBase";
        string sqlstr = "Select top 50000 * from prdBase";
        public void Run()
        {
        redo:
            Console.WriteLine("Select Mode: " + sqltmp);
            Console.WriteLine("1. Change SQL");
            Console.WriteLine("2. RunTester");
            Console.WriteLine("3. Buffer Apart Test");
            Console.WriteLine("4. SimpleDataTable DataAdaptor vs DataReader - 2020-04-09");
            Console.WriteLine("q or ESC. Exit");
            ConsoleKeyInfo key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.D1:
                    sqlstr = Console.ReadLine();
                    break;
                case ConsoleKey.D2:
                    Exec();
                    break;
                case ConsoleKey.D3:
                    ExecApart();
                    break;
                case ConsoleKey.D4:
                    DataReaderTest();
                    break;
                case ConsoleKey.Q:
                case ConsoleKey.Escape:
                    return;
            }
            goto redo;
        }
        private void DataReaderTest()
        {
            Console.WriteLine("");
            // 1. Open Connection
            SqlConnection conn = new SqlConnection("Data Source=127.0.0.1;Initial catalog=Solvency2;user id=Sol2User;password=Sol2User;Min Pool Size=60;Max Pool Size=1000;Packet Size=32768;Connect Timeout=60000");
            conn.Open();
            // 1. 比较 DataTable -> 序列化 -> GZip -> 數據傳輸 ->解壓縮 -> 反序列化 -> 物件化 
            Console.WriteLine("Items            RowCount  ReadTime   ConvertTime                    ");
            UseSqlDataAdapter(conn);
            // 2. 比較 SimpleDataTable -> 序列化 -> GZip -> 數據傳輸 ->解壓縮 -> 反序列化 -> 物件化 
            UseSqlDataReader(conn);
            // 5. 比较 DataTable -> 序列化 -> GZip -> 數據傳輸 ->解壓縮 -> 反序列化 -> 物件化 
            //idx = dataGridView1.Rows.Add();
            //CompareDataTable_Zip(conn, dataGridView1.Rows[idx]);
            // 6. 比較 SimpleDataTable -> 序列化 -> GZip -> 數據傳輸 ->解壓縮 -> 反序列化 -> 物件化 
            //idx = dataGridView1.Rows.Add();
            //CompareSimpleDT_GZip(conn, dataGridView1.Rows[idx]);
            Console.WriteLine("");
        }
        private void UseSqlDataReader( SqlConnection conn )
        {
            Console.WriteLine("");
            Console.Write("UseSqlDataReader  ");
            double x = 24.0 * 60.0 * 60.0;
            // 1. Query Data
            double ts = DateTime.Now.ToOADate();
            simpleDT myTable = QueryDataByDataReader(conn, sqlstr);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format(" {0,-6} ", myTable.Rows.Count.ToString("0,000")));
            Console.Write(string.Format("  {0} ", (ts * x).ToString("0.0000")));
            // 7. 物件化
            ts = DateTime.Now.ToOADate();
            List<DprdBase> arrPrds = CopyRecordSet(myTable);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format("    {0}", (ts * x).ToString("0.0000")));
            if (arrPrds.Count == myTable.Rows.Count)
                Console.Write(" OK");
            else
                Console.Write(" NO");
        }
        private void UseSqlDataAdapter(SqlConnection conn)
        {
            Console.WriteLine("");
            Console.Write("UseSqlDataAdapter ");
            double x = 24.0 * 60.0 * 60.0;
            // 1. Query Data
            double ts = DateTime.Now.ToOADate();
            simpleDT myTable = new simpleDT(QueryData(conn, sqlstr));
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format(" {0,-6} ", myTable.Rows.Count.ToString("0,000")));
            Console.Write(string.Format("  {0} ", (ts * x).ToString("0.0000")));
            // 7. 物件化
            ts = DateTime.Now.ToOADate();
            List<DprdBase> arrPrds = CopyRecordSet(myTable);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format("    {0}", (ts * x).ToString("0.0000")));
            if (arrPrds.Count == myTable.Rows.Count)
                Console.Write(" OK");
            else
                Console.Write(" NO");
            
        }

        private void ExecApart()
        {
            Console.WriteLine("");
            double x = 24.0 * 60.0 * 60.0;
            SqlConnection conn = new SqlConnection("Data Source=127.0.0.1;Initial catalog=Solvency2;user id=Sol2User;password=Sol2User;Min Pool Size=60;Max Pool Size=1000;Packet Size=32768;Connect Timeout=60000");
            conn.Open();
            // 1. Query Data
            double ts = DateTime.Now.ToOADate();
            DataTable myTable = QueryData(conn, sqlstr);
            // 3. Get DataTable Serialized Data\
            ts = DateTime.Now.ToOADate();
            byte[] buffer = BinSer<DataTable>(myTable);
            ts = DateTime.Now.ToOADate() - ts;
            Memory<byte> membuf = buffer.AsMemory();
            MemoryStream msout = new MemoryStream(buffer.Length);
            for(int start = 0; start <= buffer.Length; start += 12_000_000)
            {
                int len = 12_000_000;
                bool bend = false;
                if (start + 12_000_000 > buffer.Length)
                {
                    len = buffer.Length - start;
                }
                msout.Write(membuf.Slice(start, len).ToArray(), 0, len);//.Write(buffer, start, len);
                //rrDataTable rrData = new rrDataTable() { TotalSize = buffer.Length, CurrentSize = len, Bend = bend };
                //rrData.BinData = Google.Protobuf.ByteString.CopyFrom(membuf.Slice(start, len).ToArray());
                //
                //await responseStream.WriteAsync(rrData);
            }
            buffer = msout.ToArray();
            ts = DateTime.Now.ToOADate();
            DataTable undt2 = BinDeSer<DataTable>(buffer);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format("   {0,-6} ", undt2.Rows.Count.ToString("00,000,000")));

        }
        SqlConnection conn ;

        private void Exec()
        {
            // 1. Open Connection
            conn = new SqlConnection("Data Source=127.0.0.1;Initial catalog=Solvency2;user id=Sol2User;password=Sol2User;Min Pool Size=60;Max Pool Size=1000;Packet Size=32768;Connect Timeout=60000");
            conn.Open();
            PerformanceRunner runner = new PerformanceRunner(
                    arrTimes, iExecTimes, InitData, arrTestFunction, null, Message);
            string[][] retResult = runner.Run();
            ShowResult(retResult);
            /* 2020-02-27 Marked By JAK 改用新的方法
            Console.WriteLine("");
            // 1. Open Connection
            conn = new SqlConnection("Data Source=127.0.0.1;Initial catalog=Solvency2;user id=Sol2User;password=Sol2User;Min Pool Size=60;Max Pool Size=1000;Packet Size=32768;Connect Timeout=60000");
            conn.Open();
            // 1. 比较 DataTable -> 序列化 -> GZip -> 數據傳輸 ->解壓縮 -> 反序列化 -> 物件化 
            Console.WriteLine("Items            RowCount  QueryTime  DataSize   Serialize  Compress   UsedTime   Decompress   Deserialize  ConvertObj   TotalTime  CompressUsed");
            CompareDataTable_GZip(conn);
            // 2. 比較 SimpleDataTable -> 序列化 -> GZip -> 數據傳輸 ->解壓縮 -> 反序列化 -> 物件化 
            CompareSimpleDT_GZip(conn);
            // 3. 比較 SimpleDataTable -> 序列化 -> Deflate -> 數據傳輸 ->解壓縮 -> 反序列化 -> 物件化 
            CompareDataTable_Deflate(conn);
            // 4. 比較 SimpleDataTable -> 序列化 -> Deflate -> 數據傳輸 ->解壓縮 -> 反序列化 -> 物件化 
            CompareSimpleDT_Deflate(conn);
            // 5. 比较 DataTable -> 序列化 -> GZip -> 數據傳輸 ->解壓縮 -> 反序列化 -> 物件化 
            //idx = dataGridView1.Rows.Add();
            //CompareDataTable_Zip(conn, dataGridView1.Rows[idx]);
            // 6. 比較 SimpleDataTable -> 序列化 -> GZip -> 數據傳輸 ->解壓縮 -> 反序列化 -> 物件化 
            //idx = dataGridView1.Rows.Add();
            //CompareSimpleDT_GZip(conn, dataGridView1.Rows[idx]);
            */
        }
        #region Overrided Function
        /// <summary>
        /// 改成用 iT 控制 sqlstri
        /// </summary>
        /// <param name="iT"></param>
        /// <returns></returns>
        protected override int InitData(int iT)
        {
            sqlstr = string.Format(sqltmp, iT.ToString());
            return 1;
        }
        #endregion

        #region // DataTable - Functions
        /// <summary>
        /// 整个流程的时间计数
        /// </summary>
        long TotalTicks = 0;
        DataTable myTable = null;
        DataTable undt = null;
        /// <summary>
        /// Step 1 Query Data
        /// </summary>
        /// <param name="iT"></param>
        /// <param name="GetDate"></param>
        /// <returns></returns>
        private string DataTable_Query(int iT, Func<object> GetDate)
        {
            TotalTicks = DateTime.Now.Ticks;
            myTable = QueryData(conn, sqlstr);
            return "";
        }
        /// <summary>
        /// 回传资料数量
        /// </summary>
        /// <param name="iT"></param>
        /// <param name="GetDate"></param>
        /// <returns></returns>
        private string DataTable_QueryCount(int iT, Func<object> GetDate)
        {
            return myTable.Rows.Count.ToString("#,##0");
        }
        /// <summary>
        /// 检查原始 与 解压缩反序列化之后的资料笔数
        /// </summary>
        /// <param name="iT"></param>
        /// <param name="GetDate"></param>
        /// <returns></returns>
        private string DataTable_DeSerializeCheck(int iT, Func<object> GetDate)
        {
            if (undt.Rows.Count == myTable.Rows.Count)
                return "OK";
            return "No";
        }
        /// <summary>
        /// 转换成物件
        /// </summary>
        /// <param name="iT"></param>
        /// <param name="GetDate"></param>
        /// <returns></returns>
        private string DataTable_ToList(int iT, Func<object> GetDate)
        {
            List<DprdBase> arrPrds = CopyRecordSet(undt);
            return "";
        }
        #endregion

        #region // SimpleDT
        simpleDT simpdt = null;
        simpleDT undt2;
        /// <summary>
        /// Step 1 Query Data
        /// Simple DT 的查询程 DataTable 在转换成 SimpleDT
        /// </summary>
        /// <param name="iT"></param>
        /// <param name="GetDate"></param>
        /// <returns></returns>
        private string SimpleDT_Query(int iT, Func<object> GetDate)
        {
            TotalTicks = DateTime.Now.Ticks;
            DataTable myTable = QueryData(conn, sqlstr);
            simpdt = new simpleDT(myTable);
            return "";
        }
        private string SimpleDT_QueryCount(int iT, Func<object> GetDate)
        {
            return simpdt.Rows.Count.ToString("#,##0");
        }
        /// <summary>
        /// Step 2 Get binDataTable Serialized Data\
        /// </summary>
        /// <param name="iT"></param>
        /// <param name="GetDate"></param>
        /// <returns></returns>
        private string SimpleDT_Serialize(int iT, Func<object> GetDate)
        {
            arr = BinSer(simpdt);
            return "";
        }
        private string SimpleDT_DeSerialize(int iT, Func<object> GetDate)
        {
            undt2 = BinDeSer<simpleDT>(unziparr);
            return "";
        }
        private string SimpleDT_DeSerializeCheck(int iT, Func<object> GetDate)
        {
            if (undt2.Rows.Count == simpdt.Rows.Count)
                return "OK";
            else
                return "No";
        }
        private string SimpleDT_ToList(int iT, Func<object> GetDate)
        {
            List<DprdBase> arrPrds = CopyRecordSet(undt2);
            return "";
        }
        #endregion
        #region // Binary Serialize
        byte[] arr = null;
        /// <summary>
        /// Serialize
        /// </summary>
        /// <param name="iT"></param>
        /// <param name="GetDate"></param>
        /// <returns></returns>
        private string DataTable_Serialize(int iT, Func<object> GetDate)
        {
            arr = BinSer(myTable);
            return "";
        }
        /// <summary>
        /// 6. 反序列化
        /// </summary>
        /// <param name="iT"></param>
        /// <param name="GetDate"></param>
        /// <returns></returns>
        private string DataTable_DeSerialize(int iT, Func<object> GetDate)
        {
            undt = BinDeSer<DataTable>(unziparr);
            return "";
        }
        /// <summary>
        /// Serialized Array Size
        /// </summary>
        /// <param name="iT"></param>
        /// <param name="GetDate"></param>
        /// <returns></returns>
        private string DataTable_SerializedSize(int iT, Func<object> GetDate)
        {
            return arr.Length.ToString("#,##0");
        }
        #endregion

        #region // General Compress 
        /// <summary>
        /// 压缩解压缩使用的时间计数
        /// </summary>
        long CompressTicks = 0;
        /// <summary>
        /// 压缩解压缩实际秒数
        /// </summary>
        double dcompressdSeconds = 0;
        /// <summary>
        /// 压缩后的空间
        /// </summary>
        private byte[] ziparr;
        /// <summary>
        /// 解压缩之后的资料置放
        /// </summary>
        byte[] unziparr;
        /// <summary>
        /// 检查压缩前 解压缩后的资料大小
        /// </summary>
        /// <param name="iT"></param>
        /// <param name="GetDate"></param>
        /// <returns></returns>
        private string DeCompressCheck(int iT, Func<object> GetDate)
        {
            if (unziparr.Length == arr.Length)
                return "OK";
            return "No";
        }
        #endregion

        #region // GZip Compress - Decompress
        /// <summary>
        /// GZip 压缩
        /// </summary>
        /// <param name="iT"></param>
        /// <param name="GetDate"></param>
        /// <returns></returns>
        private string GZip(int iT, Func<object> GetDate)
        {
            CompressTicks = DateTime.Now.Ticks;
            ziparr = GZipCompress(arr);
            return "";
        }
        /// <summary>
        /// 回传压缩后的空间大小
        /// </summary>
        /// <param name="iT"></param>
        /// <param name="GetDate"></param>
        /// <returns></returns>
        private string GetCompressedSize(int iT, Func<object> GetDate)
        {
            return ziparr.Length.ToString("#,##0");
        }
        
        /// <summary>
        /// GZip 解压缩
        /// </summary>
        /// <param name="iT"></param>
        /// <param name="GetDate"></param>
        /// <returns></returns>
        private string UnGZip(int iT, Func<object> GetDate)
        {
            // 5. 解压缩
            unziparr = GZipDecompress(ziparr);
            dcompressdSeconds = PerformanceRunner.GetExecuteTime(CompressTicks);
            return "";
        }
        #endregion

        #region // DeflateCompress
        private string Deflact(int iT, Func<object> GetDate)
        {
            CompressTicks = DateTime.Now.Ticks;
            ziparr = DeflateCompress(arr);
            return "";
        }
        private string UnDeflact(int iT, Func<object> GetDate)
        {
            unziparr = DeflateDecompress(ziparr);
            dcompressdSeconds = PerformanceRunner.GetExecuteTime(CompressTicks);

            return "";
        }
        #endregion

        #region // Common Function
        double dtTotalTimes = 0;
        /// <summary>
        /// 整个流程的执行时间
        /// </summary>
        /// <param name="iT"></param>
        /// <param name="GetDate"></param>
        /// <returns></returns>
        private string GetTotalTime(int iT, Func<object> GetDate)
        {
            dtTotalTimes = PerformanceRunner.GetExecuteTime(TotalTicks);
            return dtTotalTimes.ToString("#,##0.000000");
        }
        /// <summary>
        /// 压缩解压缩的总时间占整个流程时间的百分比
        /// </summary>
        /// <param name="iT"></param>
        /// <param name="GetDate"></param>
        /// <returns></returns>
        private string GetCompressUsedTime2(int iT, Func<object> GetDate)
        {
            return ((dcompressdSeconds / dtTotalTimes) * 100.0).ToString("00.0000");
        }
        /// <summary>
        /// 压缩解压缩的总时间
        /// </summary>
        /// <param name="iT"></param>
        /// <param name="GetDate"></param>
        /// <returns></returns>
        private string GetCompressUsedTime1(int iT, Func<object> GetDate)
        {
            return (dcompressdSeconds).ToString("0.0000");
        }
        #endregion
        private void CompareSimpleDT_Deflate(SqlConnection conn)
        {
            Console.WriteLine("");
            Console.Write("SimpleDT+Deflate  ");
            double x = 24.0 * 60.0 * 60.0;
            // 2-1. Query Data
            double totalTimes = 0;
            double compresstimes = 0;
            double ts = DateTime.Now.ToOADate();
            DataTable myTable = QueryData(conn, sqlstr);
            simpleDT simpdt = new simpleDT(myTable);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format(" {0,-6} ", simpdt.Rows.Count.ToString("0,000")));
            Console.Write(string.Format("  {0} ", (ts * x).ToString("0.0000")));
            totalTimes += ts;
            // 2-2. Get binDataTable Serialized Data\
            ts = DateTime.Now.ToOADate();
            byte[] arr = BinSer(simpdt);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format("   {0,-6} ", arr.Length.ToString("00,000,000")));
            Console.Write(string.Format(" {0} ", (ts * x).ToString("0.0000")));
            totalTimes += ts;
            // 2-3. Comparess 
            ts = DateTime.Now.ToOADate();
            byte[] ziparr = GZipCompress(arr);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format("  {0,-6} ", ziparr.Length.ToString("0,000,000")));
            Console.Write(string.Format("   {0} ", (ts * x).ToString("0.0000")));
            totalTimes += ts;
            compresstimes += ts;
            // 2-4. DeComparess 
            ts = DateTime.Now.ToOADate();
            byte[] unziparr = GZipDecompress(ziparr);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format("    {0}", (ts * x).ToString("0.0000")));
            if (unziparr.Length == arr.Length)
                Console.Write(" OK");
            else
                Console.Write("NO");
            totalTimes += ts;
            compresstimes += ts;
            // 2-5 DeSerialize
            ts = DateTime.Now.ToOADate();
            simpleDT undt2 = BinDeSer<simpleDT>(unziparr);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format("    {0}", (ts * x).ToString("0.0000")));
            if (undt2.Rows.Count == simpdt.Rows.Count)
                Console.Write(" OK");
            else
                Console.Write(" NO");
            totalTimes += ts;
            // 7. 物件化
            ts = DateTime.Now.ToOADate();
            List<DprdBase> arrPrds = CopyRecordSet(undt2);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format("    {0}", (ts * x).ToString("0.0000")));
            if (arrPrds.Count == myTable.Rows.Count)
                Console.Write(" OK");
            else
                Console.Write(" NO");
            totalTimes += ts;
            Console.Write(string.Format("    {0} ", (totalTimes * x).ToString("0.0000")));
            Console.Write(string.Format(" {1}% {0} ", (compresstimes / totalTimes * 100.0).ToString("00.00"), (compresstimes * x).ToString("0.00")));
        }

        private void CompareDataTable_Deflate(SqlConnection conn)
        {
            Console.WriteLine("");
            Console.Write("DataTable+Deflate ");
            double x = 24.0 * 60.0 * 60.0;
            double totalTimes = 0;
            double compresstimes = 0;
            // 1. Query Data
            double ts = DateTime.Now.ToOADate();
            DataTable myTable = QueryData(conn, sqlstr);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format(" {0,-6} ", myTable.Rows.Count.ToString("0,000")));
            Console.Write(string.Format("  {0} ", (ts * x).ToString("0.0000")));
            totalTimes += ts;
            // 3. Get DataTable Serialized Data\
            ts = DateTime.Now.ToOADate();
            byte[] arr = BinSer(myTable);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format("   {0,-6} ", arr.Length.ToString("00,000,000")));
            Console.Write(string.Format(" {0} ", (ts * x).ToString("0.0000")));
            totalTimes += ts;
            // 4. Comparess 
            ts = DateTime.Now.ToOADate();
            byte[] ziparr = DeflateCompress(arr);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format("  {0,-6} ", ziparr.Length.ToString("0,000,000")));
            Console.Write(string.Format("   {0} ", (ts * x).ToString("0.0000")));
            totalTimes += ts;
            compresstimes += ts;
            // 5. 解压缩
            ts = DateTime.Now.ToOADate();
            byte[] unziparr = DeflateDecompress(ziparr);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format("    {0}", (ts * x).ToString("0.0000")));
            if (unziparr.Length == arr.Length)
                Console.Write(" OK");
            else
                Console.Write("NO");
            totalTimes += ts;
            compresstimes += ts;
            // 6. 反序列化
            ts = DateTime.Now.ToOADate();
            DataTable undt = BinDeSer<DataTable>(unziparr);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format("    {0}", (ts * x).ToString("0.0000")));
            if (undt.Rows.Count == myTable.Rows.Count)
                Console.Write(" OK");
            else
                Console.Write(" NO");

            totalTimes += ts;
            // 7. 物件化
            ts = DateTime.Now.ToOADate();
            List<DprdBase> arrPrds = CopyRecordSet(undt);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format("    {0}", (ts * x).ToString("0.0000")));
            if (arrPrds.Count == myTable.Rows.Count)
                Console.Write(" OK");
            else
                Console.Write(" NO");
            totalTimes += ts;
            Console.Write(string.Format("    {0} ", (totalTimes * x).ToString("0.0000")));
            Console.Write(string.Format(" {1}% {0} ", (compresstimes / totalTimes * 100.0).ToString("00.00"), (compresstimes * x).ToString("0.00")));
        }

        private void CompareSimpleDT_GZip(SqlConnection conn)
        {
            Console.WriteLine("");
            Console.Write("SimpleDT+GZip     ");
            double x = 24.0 * 60.0 * 60.0;
            // 2-1. Query Data
            double totalTimes = 0;
            double compresstimes = 0;

            double ts = DateTime.Now.ToOADate();
            DataTable myTable = QueryData(conn, sqlstr);
            simpleDT simpdt = new simpleDT(myTable);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format(" {0,-6} ", simpdt.Rows.Count.ToString("0,000")));
            Console.Write(string.Format("  {0} ", (ts * x).ToString("0.0000")));
            totalTimes += ts;
            // 2-2. Get binDataTable Serialized Data\
            ts = DateTime.Now.ToOADate();
            byte[] arr = BinSer(simpdt);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format("   {0,-6} ", arr.Length.ToString("00,000,000")));
            Console.Write(string.Format(" {0} ", (ts * x).ToString("0.0000")));
            totalTimes += ts;
            // 2-3. Comparess 
            ts = DateTime.Now.ToOADate();
            byte[] ziparr = GZipCompress(arr);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format("  {0,-6} ", ziparr.Length.ToString("0,000,000")));
            Console.Write(string.Format("   {0} ", (ts * x).ToString("0.0000")));
            totalTimes += ts;
            compresstimes += ts;
            // 2-4. DeComparess 
            ts = DateTime.Now.ToOADate();
            byte[] unziparr = GZipDecompress(ziparr);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format("    {0}", (ts * x).ToString("0.0000")));
            if (unziparr.Length == arr.Length)
                Console.Write(" OK");
            else
                Console.Write("NO");
            totalTimes += ts;
            compresstimes += ts;
            // 2-5 DeSerialize
            ts = DateTime.Now.ToOADate();
            simpleDT undt2 = BinDeSer<simpleDT>(unziparr);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format("    {0}", (ts * x).ToString("0.0000")));
            if (undt2.Rows.Count == simpdt.Rows.Count)
                Console.Write(" OK");
            else
                Console.Write(" NO");
            totalTimes += ts;
            // 7. 物件化
            ts = DateTime.Now.ToOADate();
            List<DprdBase> arrPrds = CopyRecordSet(undt2);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format("    {0}", (ts * x).ToString("0.0000")));
            if (arrPrds.Count == myTable.Rows.Count)
                Console.Write(" OK");
            else
                Console.Write(" NO");
            totalTimes += ts;
            Console.Write(string.Format("    {0} ", (totalTimes * x).ToString("0.0000")));
            Console.Write(string.Format(" {1}% {0} ", (compresstimes / totalTimes * 100.0).ToString("00.00"), (compresstimes * x).ToString("0.00")));
        }

        private void CompareDataTable_GZip(SqlConnection conn)
        {
            Console.WriteLine("");
            Console.Write("DataTable+GZip    ");
            double x = 24.0 * 60.0 * 60.0;
            double totalTimes = 0;
            double compresstimes = 0;
            // 1. Query Data
            double ts = DateTime.Now.ToOADate();
            DataTable myTable = QueryData(conn, sqlstr);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format(" {0,-6} ", myTable.Rows.Count.ToString("0,000")));
            Console.Write(string.Format("  {0} ", (ts * x).ToString("0.0000")));
            totalTimes += ts;
            // 3. Get DataTable Serialized Data\
            ts = DateTime.Now.ToOADate();
            byte[] arr = BinSer<DataTable>(myTable);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format("   {0,-6} ", arr.Length.ToString("00,000,000")));
            Console.Write(string.Format(" {0} ", (ts * x).ToString("0.0000")));
            totalTimes += ts;
            // 4. Comparess 
            ts = DateTime.Now.ToOADate();
            byte[] ziparr = GZipCompress(arr);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format("  {0,-6} ", ziparr.Length.ToString("0,000,000")));
            Console.Write(string.Format("   {0} ", (ts * x).ToString("0.0000")));
            totalTimes += ts;
            compresstimes += ts;
            // 5. 解压缩
            ts = DateTime.Now.ToOADate();
            byte[] unziparr = GZipDecompress(ziparr);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format("    {0}", (ts * x).ToString("0.0000")));
            if (unziparr.Length == arr.Length)
                Console.Write(" OK");
            else
                Console.Write( "NO");
            compresstimes += ts;
            totalTimes += ts;
            // 6. 反序列化
            ts = DateTime.Now.ToOADate();
            DataTable undt = BinDeSer<DataTable>(unziparr);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format("    {0}", (ts * x).ToString("0.0000")));
            if (undt.Rows.Count == myTable.Rows.Count)
                Console.Write(" OK");
            else
                Console.Write(" NO");
            totalTimes += ts;
            // 7. 物件化
            ts = DateTime.Now.ToOADate();
            List<DprdBase> arrPrds = CopyRecordSet(undt);
            ts = DateTime.Now.ToOADate() - ts;
            Console.Write(string.Format("    {0}", (ts * x).ToString("0.0000")));
            if (arrPrds.Count == myTable.Rows.Count)
                Console.Write(" OK");
            else
                Console.Write(" NO");
            totalTimes += ts;
            Console.Write(string.Format("    {0} ", (totalTimes * x).ToString("0.0000")));
            Console.Write(string.Format(" {1}% {0} ", (compresstimes / totalTimes * 100.0).ToString("00.00"), (compresstimes * x).ToString("0.00")));
        }

        #region // Compress
        private byte[] DeflateCompress(byte[] arr)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.IO.Compression.DeflateStream zip =
                new System.IO.Compression.DeflateStream(ms, System.IO.Compression.CompressionLevel.Optimal);
            zip.Write(arr, 0, arr.Length);
            zip.Close();
            return ms.ToArray();
        }

        private byte[] DeflateDecompress(byte[] ziparr)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(ziparr);
            System.IO.Compression.DeflateStream zip = new System.IO.Compression.DeflateStream(ms, System.IO.Compression.CompressionMode.Decompress);
            System.IO.MemoryStream msout = new System.IO.MemoryStream();
            zip.CopyTo(msout);
            return msout.ToArray();
        }

        
        private byte[] GZipDecompress(byte[] ziparr)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(ziparr);
            System.IO.Compression.GZipStream zip = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Decompress);
            System.IO.MemoryStream msout = new System.IO.MemoryStream();
            zip.CopyTo(msout);
            return msout.ToArray();

        }
        private byte[] GZipCompress(byte[] arr)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.IO.Compression.GZipStream zip =
                new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Compress, true);
            zip.Write(arr, 0, arr.Length);
            zip.Close();
            return ms.ToArray();
        }
        #endregion

        #region // Serialize
        private T BinDeSer<T>(byte[] unziparr)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(unziparr, 0, unziparr.Length);
                ms.Flush();
                ms.Position = 0;
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(ms);
            }
        }
        private byte[] BinSer<T>(T myTable)
        {
            MemoryStream fs = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, myTable);
            return fs.GetBuffer();
        }
        #endregion

        #region // SQL Server Process
        private List<DprdBase> CopyRecordSet(simpleDT tbl)
        {
            List<DprdBase> arrRes = new List<DprdBase>();
            #region // 整批複製資料到元件上
            DprdBase node = null;
            foreach (object[] dr in tbl.Rows)
            {
                node = new DprdBase();
                try
                {
                    #region // 將 DataRow資料複製到元件上
                    try
                    {
                        node.ProductID = Convert.ToInt64(dr[0]);
                        node.TypeID = Convert.ToInt64(dr[1]);
                        node.Code = dr[2].ToString();
                        node.Name = dr[3].ToString();
                        node.SName = dr[4].ToString();
                        node.CurrencyCode = dr[5].ToString();
                        node.Status = (int)dr[6];
                        node.ListedDate = Convert.ToDateTime(dr[7]);
                        node.UnListedDate = Convert.ToDateTime(dr[8]);
                        node.ModiTime = Convert.ToDecimal(dr[9]);
                        node.CounterPartyID = Convert.ToInt64(dr[11]);
                        node.CounterPartyName = dr[10].ToString();
                        node.ExchangeID = Convert.ToInt64(dr[12]);
                        node.NationCode = dr[13].ToString();
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    continue;
                }
                arrRes.Add(node);
            }
            #endregion
            return arrRes;
        }
        private List<DprdBase> CopyRecordSet(DataTable tbl)
        {
            List<DprdBase> arrRes = new List<DprdBase>();
            #region // 整批複製資料到元件上
            DprdBase node = null;
            foreach (DataRow dr in tbl.Rows)
            {
                node = new DprdBase();
                try
                {
                    #region // 將 DataRow資料複製到元件上
                    try
                    {
                        node.ProductID = Convert.ToInt64(dr[0]);
                        node.TypeID = Convert.ToInt64(dr[1]);
                        node.Code = dr[2].ToString();
                        node.Name = dr[3].ToString();
                        node.SName = dr[4].ToString();
                        node.CurrencyCode = dr[5].ToString();
                        node.Status = (int)dr[6];
                        node.ListedDate = Convert.ToDateTime(dr[7]);
                        node.UnListedDate = Convert.ToDateTime(dr[8]);
                        node.ModiTime = Convert.ToDecimal(dr[9]);
                        node.CounterPartyID = Convert.ToInt64(dr[11]);
                        node.CounterPartyName = dr[10].ToString();
                        node.ExchangeID = Convert.ToInt64(dr[12]);
                        node.NationCode = dr[13].ToString();
                        /*
                        node.ProductID = Convert.ToInt64(dr["ProductID"]);
                        node.TypeID = Convert.ToInt64(dr["TypeID"]);
                        node.Code = dr["Code"].ToString();
                        node.Name = dr["Name"].ToString();
                        node.SName = dr["SName"].ToString();
                        node.CurrencyCode = dr["CurrencyCode"].ToString();
                        node.Status = (int)dr["Status"];
                        node.ListedDate = Convert.ToDateTime(dr["ListedDate"]);
                        node.UnListedDate = Convert.ToDateTime(dr["UnListedDate"]);
                        node.ModiTime = Convert.ToDecimal(dr["ModiTime"]);
                        node.CounterPartyID = Convert.ToInt64(dr["CounterPartyID"]);
                        node.CounterPartyName = dr["CounterPartyName"].ToString();
                        node.ExchangeID = Convert.ToInt64(dr["ExchangeID"]);
                        node.NationCode = dr["NationCode"].ToString();
                        */
                    }
                    catch (Exception ex)
                    {
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    continue;
                }
                arrRes.Add(node);
            }
            #endregion
            return arrRes;
        }

        private DataTable QueryData(SqlConnection conn, string actSQL)
        {
            DataTable myTable = null;
            using (SqlCommand myCommand = conn.CreateCommand())
            {
                myCommand.CommandTimeout = 0;
                myCommand.CommandText = actSQL;
                using (SqlDataAdapter myReader = new SqlDataAdapter(myCommand))
                {
                    myTable = new DataTable();
                    myReader.Fill(myTable);
                }
            }
            return myTable;
        }
        private simpleDT QueryDataByDataReader(SqlConnection conn, string actSQL)
        {
            simpleDT myTable = new simpleDT(); ;
            using (SqlCommand myCommand = conn.CreateCommand())
            {
                myCommand.CommandTimeout = 0;
                myCommand.CommandText = actSQL;
                using (SqlDataReader myReader = myCommand.ExecuteReader())
                {
                    foreach( DbColumn col in myReader.GetColumnSchema())
                        myTable.AddColumn(col.ColumnName, col.DataTypeName);
                    int colCount = myReader.FieldCount;
                    while( myReader.Read())
                    {
                        object[] obj = new object[colCount];
                        for( int i = 0; i < colCount;i++ )
                            obj[i] = myReader.GetValue(i);
                        myTable.AddRow(obj);
                    }
                }
            }
            return myTable;
        }
        #endregion
    }

    /// <summary>
    /// 商品總項
    /// </summary>
    [Serializable]
    public class DprdBase
    {
        #region // 資料庫欄位對應
        /// <summary>
        /// 商品ID(PK)
        /// </summary>
        public long ProductID { get; set; }
        /// <summary>
        /// 種類ID,prdType.TypeID
        /// </summary>
        public long TypeID { get; set; }
        /// <summary>
        /// 代碼
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 商品狀態 1 Alive
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 上市日期
        /// </summary>
        public DateTime ListedDate { get; set; }
        /// <summary>
        /// 下市日期
        /// </summary>
        public DateTime UnListedDate { get; set; }

        /// <summary>
        /// 修改時間
        /// </summary>
        public decimal ModiTime { get; set; }
        /// <summary>
        /// 簡稱
        /// </summary>
        public string SName { get; set; }
        /// <summary>
        /// 幣別
        /// </summary>
        public string CurrencyCode { get; set; }
        /// <summary>
        /// 发行人名称
        /// </summary>
        public string CounterPartyName { get; set; }
        /// <summary>
        /// 交易对手ID
        /// </summary>
        public long CounterPartyID = -1;
        /// <summary>
        /// 交易所ID
        /// </summary>
        public long ExchangeID { get; set; }

        /// <summary>
        /// 国别
        /// </summary>
        public string NationCode { get; set; }
        #endregion

        #region // 唯一 ID 定義 - ID<->ProductID
        /// <summary>
        /// ProductID
        /// </summary>
        public long ID
        {
            get { return ProductID; }
            set { ProductID = value; }
        }
        #endregion

        #region // 建構式
        /// <summary>
        /// 建構式
        /// </summary>
        public DprdBase()
        {
        }

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="ProductID">商品ID(PK)</param>
        /// <param name="TypeID">種類ID,prdType.TypeID</param>
        /// <param name="Code">代碼</param>
        /// <param name="Name">名稱</param>
        /// <param name="Status">商品狀態 1 Alive</param>
        /// <param name="ListedDate">上市日期</param>
        /// <param name="UnListedDate">下市日期</param>
        /// <param name="CounterPartyName">发行人名称</param>
        /// <param name="ModiTime">修改時間</param>
        /// <param name="SName">簡稱</param>
        /// <param name="CurrencyCode">幣別</param>
        /// <param name="CounterPartyID">交易对手ID</param>
        /// <param name="ExchangeID">交易所ID</param>
        /// <param name="NationCode">国别</param>
        public DprdBase(long ProductID, long TypeID, string Code, string Name, int Status,
                        DateTime ListedDate, DateTime UnListedDate, string CounterPartyName, decimal ModiTime,
                        string SName, string CurrencyCode, long CounterPartyID, long ExchangeID, string NationCode)
        {
            this.ProductID = ProductID;
            this.TypeID = TypeID;
            this.Code = Code;
            this.Name = Name;
            this.Status = Status;
            this.ListedDate = ListedDate;
            this.UnListedDate = UnListedDate;
            this.CounterPartyName = CounterPartyName;
            this.ModiTime = ModiTime;
            this.SName = SName;
            this.CurrencyCode = CurrencyCode;
            this.CounterPartyID = CounterPartyID;
            this.ExchangeID = ExchangeID;
            this.NationCode = NationCode;
        }
        #endregion

        /// <summary>
        /// 复制自己一份
        /// </summary>
        /// <returns></returns>
        public DprdBase Clone()
        {
            return (DprdBase)MemberwiseClone();
        }
    }

    [Serializable]
    public class simpleDT
    {
        //public List<string> arrColumnName { get; set; }
        public List<string> arrDataType { get; set; } = new List<string>();
        public Dictionary<string, int> mapIndex { get; set; } = new Dictionary<string, int>();
        public List<object[]> Rows { get; set; } = new List<object[]>();
        public void AddColumn( string ColumnName ,string DataTypeName)
        {
            mapIndex.Add(ColumnName, mapIndex.Count);
            arrDataType.Add(DataTypeName);
        }

        internal void AddRow(object[] obj)
        {
            Rows.Add(obj);
        }

        public simpleDT()
        { }
        public simpleDT(DataTable dt)
        {
            //arrColumnName = new List<string>();
            int idx = 0;
            foreach (DataColumn col in dt.Columns)
            {
                //arrColumnName.Add(col.ColumnName);
                mapIndex.Add(col.ColumnName, idx);
                arrDataType.Add(col.DataType.FullName);
                idx++;
            }
            foreach (DataRow row in dt.Rows)
            {
                if (row == null)
                    continue;
                // if (row.RowState != DataRowState.Deleted)
                Rows.Add(row.ItemArray);
            }
        }
    }
}
