using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public enum MyEnum
    {
        Item1,
        Item2,
        Item3,
        Item4
    }

    class Program
    {
        static List<ITest> tests;

        static void Main(string[] args)
        {
            tests = new List<ITest>();

            tests.Add(new Speed());
            tests.Add(new Distribution());

            start:
            Console.WriteLine("What do you want to test?");

            int index = 1;
            foreach (ITest test in tests)
            {
                Console.WriteLine("{0}: {1}", index, test.GetType().Name);
                index += 1;
            }

            selection:
            int selection;
            if (!Int32.TryParse(Console.ReadLine(), out selection))
                goto selection;

            if (selection < 1 || selection > tests.Count)
                goto selection;

            Console.WriteLine("Testing {0}...\n", tests[selection - 1].GetType().Name);

            tests[selection - 1].Run();

            Console.WriteLine();

            goto start;
        }
    }
}
