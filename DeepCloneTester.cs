using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCorePerformanceTest.DeepClone
{
    public class DeepCloneTester :PerformanceBase ,  IDisposable
    {
        public void Dispose()
        {
        }
        public DeepCloneTester()
        {
            arrTimes = new List<int>()
            { 100,1000, 10000 };
            iExecTimes = 20;
            arrTestFunction = new List<Tuple<string,emTestType, Func<int, Func<object>, string>>>()
            {
                new  Tuple<string,emTestType, Func<int, Func<object>, string>>("JSON-Simple" ,emTestType.Performance, SimpleJson) ,
                new  Tuple<string,emTestType, Func<int, Func<object>, string>>("BinarySerizer-Simple" ,emTestType.Performance, BinarySerizer_S) ,
                new  Tuple<string,emTestType, Func<int, Func<object>, string>>("Expression-Simple" ,emTestType.Performance, Expression_S) ,
                new  Tuple<string,emTestType, Func<int, Func<object>, string>>("JsonComplex" ,emTestType.Performance, JsonComplex) ,
                new  Tuple<string,emTestType, Func<int, Func<object>, string>>("Expression-Complex" ,emTestType.Performance, Expression_C) ,
            };
        }
        List<SimpleClass> arrS = new List<DeepClone.SimpleClass>();
        List<ComplexClass> arrC = new List<DeepClone.ComplexClass>();
        protected override int InitData(int iT)
        {
            arrS.Clear();
            arrC.Clear();
            for (int i = 0; i < iT; i++)
            {
                arrS.Add(SimpleClass.CreateForTests(i));
                arrC.Add(ComplexClass.CreateForTests());
            }
            return 1;
        }
        public void Run()
        {
            PerformanceRunner runner = new PerformanceRunner(
                arrTimes, iExecTimes, InitData, arrTestFunction, DataGetter, Message);
            string[][] retResult = runner.Run();
            ShowResult(retResult);
        }
        private string SimpleJson(int iT, Func<object> GetDate)
        {
            List<SimpleClass> arrS2 = arrS.DeepCloneJSON();
            return "";
        }
        private string BinarySerizer_S(int iT, Func<object> GetDate)
        {
            List<SimpleClass> arrS2 = arrS.DeepCloneBinarySerializer();
            return "";
        }
        private string Expression_S(int iT, Func<object> GetDate)
        {
            List<SimpleClass> arrS2 = arrS.DeepCloneExpressionTree();
            return "";
        }
        private string JsonComplex(int iT, Func<object> GetDate)
        {
            List<ComplexClass> arrC2 = arrC.DeepCloneBinarySerializer();
            return "";
        }
        private string Expression_C(int iT, Func<object> GetDate)
        {
            List<ComplexClass> arrC2 = arrC.DeepCloneExpressionTree();
            return "";
        }

    }
}
