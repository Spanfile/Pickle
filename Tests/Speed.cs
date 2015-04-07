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
            int itemCount = 1000000;

            Picker<MyEnum> picker = new Picker<MyEnum>();

            picker.AddItem(MyEnum.Item1, 25);
            picker.AddItem(MyEnum.Item2, 25);
            picker.AddItem(MyEnum.Item3, 25);
            picker.AddItem(MyEnum.Item4, 25);

            Console.WriteLine("Generating {0} values...", itemCount);

            Stopwatch timer = Stopwatch.StartNew();

            picker.NextItems(itemCount);

            timer.Stop();

            Console.WriteLine("Time: {0}ms", timer.Elapsed.TotalMilliseconds);
        }
    }
}
