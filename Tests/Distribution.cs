using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pickle;

namespace Tests
{
    public class Distribution : ITest
    {
        public void Run()
        {
            int itemCount = 1000000;

            Picker<MyEnum> picker = new Picker<MyEnum>();

            picker.AddItem(MyEnum.Item1, 50);
            picker.AddItem(MyEnum.Item2, 30);
            picker.AddItem(MyEnum.Item3, 15);
            picker.AddItem(MyEnum.Item4, 5);

            Dictionary<MyEnum, int> items = Enum.GetValues(typeof(MyEnum)).Cast<MyEnum>().ToDictionary(n => n, n => 0);

            Console.WriteLine("Generating {0} values...", itemCount);

            foreach (MyEnum item in picker.NextItems(itemCount))
                items[item] += 1;

            Console.WriteLine(String.Join("\n", items.Select(p => $"{p.Key}: {p.Value}, {(p.Value / (double)itemCount) * 100d}%")));
        }
    }
}
