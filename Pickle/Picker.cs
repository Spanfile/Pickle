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

        Random rand;

        /// <summary>
        /// Create a new Picker
        /// </summary>
        public Picker()
        {
            items = new Dictionary<T, double>();
            ranges = new List<Range<T>>();

            rand = new Random();
        }

        /// <summary>
        /// Add an item to the picker
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <param name="prob">The probability for the item to be returned</param>
        /// <exception cref="Picke.PickleException">Thrown when given item is already in the picker's list or when chance is out of bounds</exception>
        public void AddItem(T item, double prob)
        {
            if (prob > 100)
                throw new PickleException(String.Format("Probability for item {0} is too high (> 100)", item));

            if (prob < 1)
                throw new PickleException(String.Format("Probability for item {0} is too low (< 1)", item));

            if (items.ContainsKey(item))
                throw new PickleException(String.Format("Item {0} is already added", item));

            items.Add(item, prob);
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

        /// <summary>
        /// Returns a random item from the list of items based on their probabilities
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Pickle.PickleException">Thrown when the sum of all items probabilites isn't 100</exception>
        /// <exception cref="Pickle.CriticalPickleException">Thrown if the picker finds multiple valid items for one chosen value. This should never be thrown!</exception>
        public T NextItem()
        {
            if (items.Values.Sum() != 100)
                throw new PickleException("Sum of item probabilites isn't 100");

            double val = rand.Next(0, 100);

            var validItems = ranges.Where(r => r.Contains(val)).Select(r => r.Item);

            if (validItems.Count() > 1)
                throw new CriticalPickleException("Multiple valid items found.");

            return validItems.First();
        }

        public IEnumerable<T> NextItems(int count)
        {
            if (count < 1)
                throw new PickleException("Count is too low");

            for (int i = 0; i < count; i++)
                yield return NextItem();
        }
    }
}
