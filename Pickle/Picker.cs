using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pickle
{
    public sealed class Picker<T>
    {
        Dictionary<T, double> items;
        List<Range<T>> ranges;

        public Picker()
        {
            items = new Dictionary<T, double>();
            ranges = new List<Range<T>>();
        }

        public void AddItem(T item, double chance)
        {
            if (chance > 100)
                throw new PickleException(String.Format("Probability for item {0} is too high (> 100)", item));

            if (chance < 1)
                throw new PickleException(String.Format("Probability for item {0} is too low (< 1)", item));

            if (items.ContainsKey(item))
                throw new PickleException(String.Format("Item {0} is already added", item));

            items.Add(item, chance);
            UpdateRanges();
        }

        void UpdateRanges()
        {
            ranges.Clear();

            double prev = 0;
            foreach (var pair in items.Select(k => new { Item = k.Key, Prob = k.Value }))
            {
                ranges.Add(new Range<T>(pair.Item, prev, prev + pair.Prob));
                prev += pair.Prob;
            }
        }

        public T NextItem()
        {
            if (items.Values.Sum() != 100)
                throw new PickleException("Sum of item probabilites isn't 100");

            Random rand = new Random();
            double val = rand.Next(0, 100);

            var validItems = ranges.Where(r => r.Contains(val)).Select(r => r.Item);

            if (validItems.Count() > 1)
                throw new CriticalPickleException("Multiple valid items found.");

            return validItems.First();
        }
    }
}
