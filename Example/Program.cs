﻿using System;
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

            Console.ReadKey();
        }
    }
}
