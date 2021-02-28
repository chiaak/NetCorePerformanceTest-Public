using System;

namespace NetCorePerformanceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            redo:
            Console.Clear();
            Console.WriteLine("Performance Test Model Select");
            Console.WriteLine("1 Matrix Multiplier");
            Console.WriteLine("2 string.Format vs InterpolatedStrings ");
            Console.WriteLine("3 Redis Tester");
            Console.WriteLine("4 DeepClone");
            Console.WriteLine("5 SumPerformance");
            Console.WriteLine("6 SwitchCompare");
            Console.WriteLine("7 DataTable Compress - For gRPC");
            Console.WriteLine("8 Tuple vs NamedTuple vs Class- 2020-03-26");
            Console.WriteLine("9 SpanPerformance  2020-08-27");
            Console.WriteLine("A System.Text.JSON  2020-11-25");
            Console.WriteLine("Z System Information");
            ConsoleKeyInfo key = Console.ReadKey();
            switch( key.Key)
            {
                case ConsoleKey.D1:
                    using (MatrixMultiplier m = new MatrixMultiplier())
                    {
                        m.Run();
                    }
                    break;
                case ConsoleKey.D2:
                    using (StringProcessCompare m = new StringProcessCompare())
                    {
                        m.Run();
                    }
                    break;
                case ConsoleKey.D3:
                    using (RedisTester m = new RedisTester())
                    {
                        m.Run();
                    }
                    break;
                case ConsoleKey.D4:
                    using (DeepClone.DeepCloneTester m = new DeepClone.DeepCloneTester())
                    {
                        m.Run();
                    }
                    break;
                case ConsoleKey.D5:
                    using (SumPerformance m = new SumPerformance())
                    {
                        m.Run();
                    }
                    break;
                case ConsoleKey.D6:
                    using (SwitchCompare m = new SwitchCompare())
                    {
                        m.Run();
                    }
                    break;
                case ConsoleKey.D7:
                    using (DataTableCompareTester m = new DataTableCompareTester())
                    {
                        m.Run();
                    }
                    break;
                case ConsoleKey.D8:
                    using (TupleCompare m = new TupleCompare())
                    {
                        m.Run();
                    }
                    break;
                case ConsoleKey.D9:
                    using (SpanTest.SpanTest1 m = new SpanTest.SpanTest1())
                    {
                        m.Run();
                    }
                    break;
                case ConsoleKey.A:
                    using (JsonTest m = new JsonTest())
                    {
                        m.Run();
                    }
                    break;
                case ConsoleKey.Z:
                    Utils.SystemInformation();
                    break;
                case ConsoleKey.Escape:
                    return;
            }
            goto redo;
        }
    }
}
