using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCorePerformanceTest
{
    public class Utils
    {
        public static Dictionary<int, Dictionary<int, List<double>>> InitExecuteTime(int[] Times , int colCount)
        {
            Dictionary<int, Dictionary<int, List<double>>> mapExecuteTime = new Dictionary<int, Dictionary<int, List<double>>>();
            foreach (int iT in Times)
            {
                mapExecuteTime.Add(iT, new Dictionary<int, List<double>>());
                for( int i = 1; i <= colCount; i++  )
                    mapExecuteTime[iT].Add(i, new List<double>());
            }
            return mapExecuteTime;
        }
        public static double Average(Dictionary<int, Dictionary<int, List<double>>> mapExecuteTime, int iT, int v)
        {
            double avg = mapExecuteTime[iT][v].Sum();
            avg = avg - mapExecuteTime[iT][v].Max();
            avg = avg - mapExecuteTime[iT][v].Min();
            avg = avg / (mapExecuteTime[iT][v].Count - 2);
            return avg;
        }

        public static void SystemInformation()
        {
            Console.WriteLine("CurrentDirectory:" + Environment.CurrentDirectory);
            Console.WriteLine("CurrentManagedThreadId:" + Environment.CurrentManagedThreadId);
            Console.WriteLine("GetFolderPath-Startup:" + Environment.GetFolderPath(Environment.SpecialFolder.Startup));
            Console.WriteLine("Is64BitOperatingSystem:" + Environment.Is64BitOperatingSystem);
            Console.WriteLine("Is64BitProcess:" + Environment.Is64BitProcess);
            Console.WriteLine("MachineName:" + Environment.MachineName);
            Console.WriteLine("OSVersion:" + Environment.OSVersion);
            Console.WriteLine("ProcessorCount:" + Environment.ProcessorCount);
            Console.WriteLine("SystemDirectory:" + Environment.SystemDirectory);
            Console.WriteLine("SystemPageSize:" + Environment.SystemPageSize);
            Console.WriteLine("UserDomainName:" + Environment.UserDomainName);
            Console.WriteLine("UserInteractive:" + Environment.UserInteractive);
            Console.WriteLine("UserName:" + Environment.UserName);
            Console.WriteLine("Version:" + Environment.Version);
            Console.WriteLine("WorkingSet:" + Environment.WorkingSet);
            Console.Read();
            Console.ReadKey();
        }
    }
}
