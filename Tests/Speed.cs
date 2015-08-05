using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Pickle;

namespace Tests
{
    public class Speed : ITest
    {
        public void Run()
        {
            var itemCount = int.MaxValue;

            var picker = new Picker<MyEnum>();

            picker.AddItem(MyEnum.Item1, 1);
            picker.AddItem(MyEnum.Item2, 2);
            picker.AddItem(MyEnum.Item3, 3);
            picker.AddItem(MyEnum.Item4, 4);

            Console.WriteLine($"Generating {itemCount:N0} values...");

            var timer = Stopwatch.StartNew();

            picker.NextItems(itemCount);

            timer.Stop();

            Console.WriteLine($"Time: {timer.Elapsed.TotalMilliseconds}ms");
        }
    }
}
