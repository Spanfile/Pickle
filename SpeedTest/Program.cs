using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Pickle;

namespace SpeedTest
{
    enum MyEnum
    {
        Item1,
        Item2,
        Item3,
        Item4,
    }

    class Program
    {
        static void Main(string[] args)
        {
            int iterations = 1000000;

            Picker<MyEnum> picker = new Picker<MyEnum>();

            picker.AddItem(MyEnum.Item1, 25);
            picker.AddItem(MyEnum.Item2, 25);
            picker.AddItem(MyEnum.Item3, 25);
            picker.AddItem(MyEnum.Item4, 25);

            Console.WriteLine("Generating {0} values...", iterations);

            Stopwatch timer = Stopwatch.StartNew();

            for (int i = 0; i < iterations; i++)
                picker.NextItem();

            timer.Stop();

            Console.WriteLine("Time: {0}ms", timer.Elapsed.TotalMilliseconds);
            Console.ReadKey();
        }
    }
}
