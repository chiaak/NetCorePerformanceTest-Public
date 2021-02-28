using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePerformanceTest
{
    public class JsonTest : PerformanceBase, IDisposable
    {
        public void Dispose()
        {

        }
        public JsonTest()
        {
            arrTimes = new List<int>()
            { 1000, 10000, 40000 };
            iExecTimes = 20;
            arrTestFunction = new List<Tuple<string, emTestType, Func<int, Func<object>, string>>>()
            {
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("JSON-Serial-S",emTestType.Performance , JSON_Serial_S) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("JSON-DeSerial-S" ,emTestType.Performance, JSON_DeSerial_S) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("MS.JSON-Serial-S" ,emTestType.Performance, MSJSON_Serial_S) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("MS.JSON-DeSerial-S" ,emTestType.Performance, MSJSON_DeSerial_S) ,
                new  Tuple<string, emTestType, Func<int, Func<object>, string>>("JSON-Serial-C" ,emTestType.Performance, JSON_Serial_C) ,
                //new  Tuple<string, Func<int, Func<object>, int>>("JSON-DeSerial-C" , JSON_DeSerial_C) ,
            };
        }
        List<SimpleClass> arrS = new List<SimpleClass>();
        List<DeepClone.ComplexClass> arrC = new List<DeepClone.ComplexClass>();
        List<SimpleClass> arrS2;
        List<DeepClone.ComplexClass> arrC2;

        public void Run()
        {
            PerformanceRunner runner = new PerformanceRunner(
                arrTimes, iExecTimes, InitData, arrTestFunction, DataGetter, Message);
            string[][] retResult = runner.Run();
            ShowResult(retResult);
            ConsoleKeyInfo key = Console.ReadKey();
        }
        #region // Functions
        string strSerialize = "";
        private string JSON_Serial_S(int iT, Func<object> GetDate)
        {
            strSerialize = Newtonsoft.Json.JsonConvert.SerializeObject(arrS);
            return "";
        }
        private string JSON_DeSerial_S(int iT, Func<object> GetDate)
        {
            arrS2 = (List<SimpleClass>)Newtonsoft.Json.JsonConvert.DeserializeObject<List<SimpleClass>>(strSerialize);
            return "";
        }
        private string MSJSON_Serial_S(int iT, Func<object> GetDate)
        {
            strSerialize = System.Text.Json.JsonSerializer.Serialize(arrS);
            return "";
        }
        private string MSJSON_DeSerial_S(int iT, Func<object> GetDate)
        {
            arrS2 = System.Text.Json.JsonSerializer.Deserialize<List<SimpleClass>>(strSerialize);
            return "";
        }
        private string JSON_Serial_C(int iT, Func<object> GetDate)
        {
            strSerialize = Newtonsoft.Json.JsonConvert.SerializeObject(arrC);
            return "";
        }
        private string JSON_DeSerial_C(int iT, Func<object> GetDate)
        {
            arrC2 = (List<DeepClone.ComplexClass>)Newtonsoft.Json.JsonConvert.DeserializeObject<List<DeepClone.ComplexClass>>(strSerialize);
            return "";
        }
        #endregion
        protected override int InitData(int iT)
        {
            arrS.Clear();
            arrC.Clear();
            for (int i = 0; i < iT; i++)
            {
                arrS.Add(SimpleClass.CreateForTests(i));
                arrC.Add(DeepClone.ComplexClass.CreateForTests());
            }
            return 1;
        }

        [Serializable]
        public class SimpleClass
        {
            public string PropertyPublic { get; set; }

            protected bool PropertyProtected { get; set; }

            public int FieldPublic { get; set; }

            private string FieldPrivate { get; set; }

            public SimpleClass() { }
            public SimpleClass(int propertyPrivate, bool propertyProtected, string fieldPrivate)
            {
                PropertyProtected = propertyProtected;
                FieldPrivate = fieldPrivate + "_" + typeof(SimpleClass);
            }

            public static SimpleClass CreateForTests(int seed)
            {
                return new SimpleClass(seed, seed % 2 == 1, "seed_" + seed)
                {
                    FieldPublic = -seed,
                    PropertyPublic = "seed_" + seed + "_public"
                };
            }

            public bool CheckData(SimpleClass s2)
            {
                if (this.FieldPublic != s2.FieldPublic)
                    return false;
                // if (this.FieldPrivate != s2.FieldPrivate)
                //    return false;
                //if (this.PropertyProtected != s2.PropertyProtected)
                //    return false;
                if (this.PropertyPublic != s2.PropertyPublic)
                    return false;
                return true;
            }

        }
    }

}
