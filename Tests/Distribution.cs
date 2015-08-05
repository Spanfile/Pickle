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
            var itemCount = 100000;

            var picker = new Picker<MyEnum>();

            picker.AddItem(MyEnum.Item1, 1);
            picker.AddItem(MyEnum.Item2, 2);
            picker.AddItem(MyEnum.Item3, 3);
            picker.AddItem(MyEnum.Item4, 4);

            var items = Enum.GetValues(typeof(MyEnum)).Cast<MyEnum>().ToDictionary(n => n, n => 0);

            Console.WriteLine($"Generating {itemCount:N0} values...");

            foreach (var item in picker.NextItems(itemCount))
                items[item] += 1;

            Console.WriteLine(string.Join("\n", items.Select(p => $"{p.Key}: {p.Value}, {(p.Value / (double)itemCount) * 100d}%")));
        }
    }
}
