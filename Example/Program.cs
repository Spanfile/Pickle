using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pickle;

namespace Example
{
    enum MyEnum
    {
        Item1,
        Item2,
        Item3,
        Item4
    }

    class Program
    {
        static void Main(string[] args)
        {
            // create a new picker that returns MyEnums
            Picker<MyEnum> picker = new Picker<MyEnum>();

            // add the four enum values to it with their probabilities as percentages
            picker.AddItem(MyEnum.Item1, 50);
            picker.AddItem(MyEnum.Item2, 20);
            picker.AddItem(MyEnum.Item3, 15);
            picker.AddItem(MyEnum.Item4, 15);

            // take a certain amount of items from the picker and count how many of them are returned
            Dictionary<MyEnum, int> items = Enum.GetValues(typeof(MyEnum)).Cast<MyEnum>().ToDictionary(n => n, n => 0);

            int iterations = 100000;
            for (int i = 0; i < iterations; i++)
                items[picker.NextItem()] += 1;

            // show the returned items, how many of them were returned and their percentage of the total amount
            Console.WriteLine(String.Join("\n", items.Select(p => String.Format("{0}: {1}, {2}%", p.Key, p.Value, (p.Value / (double)iterations) * 100d))));
            Console.ReadKey();
        }
    }
}
