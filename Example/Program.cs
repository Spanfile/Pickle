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
            var picker = new Picker<MyEnum>();

            // add the four weighted enum values to it
            picker.AddItem(MyEnum.Item1, 1);
            picker.AddItem(MyEnum.Item2, 2);
            picker.AddItem(MyEnum.Item3, 3);
            picker.AddItem(MyEnum.Item4, 4);
            // the weights total to 10, meaning that
            // MyEnum.Item1 has a chance of 1/10 to be returned
            // MyEnum.Item2 has a chance of 2/10 to be returned
            // MyEnum.Item3 has a chance of 3/10 to be returned
            // MyEnum.Item4 has a chance of 4/10 to be returned

            // take a value from the picker and display it
            var item = picker.NextItem();
            Console.WriteLine($"You got {item}!");

            // take multiple values and display them
            var items = picker.NextItems(5).ToArray();
            Console.WriteLine($"You also got {string.Join(", ", items.Take(4))} and {items.Last()}!");

            // create a new categorised picker
            // you need to give a method that takes in your item and returns a name for that item
            // the method below simply calls .ToString() on the item
            var catPick = new CategorisedPicker<MyEnum>(m => m.ToString());

            // add a category called "Another" to a category called "Test". the picker will add the missing category "Test" automatically.
            catPick.AddCategory("Test/Another");
            catPick.AddCategory("Test/YetAnother");

            // add a category called "MoreTest" to the previously added category "Another". this time, the picker will throw an exception if the path contains missing categories.
            catPick.AddCategory("Test/Another/MoreTest", false);
            // the following would fail
            // catPick.AddCategory("Test/Invalid/WhatAreYouDoing", false);

            // add two items to the "Another/MoreTest" category, both with equal weight
            catPick.AddItem("Test/Another/MoreTest", MyEnum.Item1, 1);
            catPick.AddItem("Test/Another/MoreTest", MyEnum.Item2, 1);

            // add two items to the "Test/YetAnother" category, both with equal weight
            catPick.AddItem("Test/YetAnother", MyEnum.Item3, 1);
            catPick.AddItem("Test/YetAnother", MyEnum.Item4, 1);

            // right now, our category hierarchy looks like this
            /*
            Test
                Another
                    MoreTest
                        Item: Item1, weight: 1
                        Item: Item2, weight: 1
                YetAnother
                    Item: Item3, weight: 1
                    Item: Item4, weight: 1
            */

            // pick one item from the picker
            // this will go through all categories, and picks an item from the first category it find that contains items
            // looking at our hierarchy above, the first category that contains items is MoreTest
            Console.WriteLine($"From Test/Another/MoreTest: {catPick.NextItem()}");

            // pick one item from the picker, this time specifying a certain category of where to find items from
            Console.WriteLine($"From Test/YetAnother: {catPick.NextItem("Test/YetAnother")}");

            Console.ReadKey();
        }
    }
}
