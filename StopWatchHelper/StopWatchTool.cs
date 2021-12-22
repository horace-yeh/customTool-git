using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StopWatchHelper
{
    public class StopWatchTool
    {
        #region Example

        //public void Test()
        //{
        //    Benchmark(() =>
        //    {
        //        Console.WriteLine("test");
        //    }, "test");
        //}

        #endregion Example

        public static void Benchmark(Action act,string label)
        {
            var sw = new Stopwatch();
            sw.Start();
            act();
            sw.Stop();
            Console.WriteLine($"{label} : {sw.ElapsedMilliseconds} ms");
        }
    }
}
