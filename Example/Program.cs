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

            // take a value from the picker and display it
            MyEnum item = picker.NextItem();
            Console.WriteLine("You got {0}!", item);

            // take multiple values and display them
            MyEnum[] items = picker.NextItems(10).ToArray();
            Console.WriteLine("You also got {0} and {1}!", String.Join(", ", items.Take(9)), items.Last());

            // create a new categorised picker
            // you need to give a method that takes in your item and returns a name for that item
            // this could be a simple method in your item, such as YourItem.GetName()
            CategorisedPicker<MyEnum> catPick = new CategorisedPicker<MyEnum>(m => m.ToString());

            // add a category called "Test" to the picker
            catPick.AddCategory("Test");

            // add a category called "Another" to the previously added category "Test"
            catPick.AddCategory("Another", "Test");

            // add two items to the "Another" category, both with 50% chance of being picked
            catPick.AddItem("Test/Another", MyEnum.Item1, 50);
            catPick.AddItem("Test/Another", MyEnum.Item2, 50);

            // pick one item from the picker
            // this will go through all categories, and picks an item from the first category it find that contains items
            Console.WriteLine(catPick.NextItem());

            Console.ReadKey();
        }
    }
}
