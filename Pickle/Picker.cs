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
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when prob is less than 1 or higher than 100</exception>
        /// <exception cref="System.ArgumentException">Thrown when item has already been added</exception>
        public void AddItem(T item, double prob)
        {
            if (prob > 100 || prob < 1)
                throw new ArgumentOutOfRangeException("prob");

            if (items.ContainsKey(item))
                throw new ArgumentException(String.Format("Item {0} is already added", item));

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
        /// Returns a random item from the list of items, based on their probabilities
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the sum of item probabilities isn't 100</exception>
        public T NextItem()
        {
            if (items.Values.Sum() != 100)
                throw new InvalidOperationException("Sum of item probabilites isn't 100");

            double val = rand.Next(0, 100);

            var validItems = ranges.Where(r => r.Contains(val)).Select(r => r.Item);

            if (validItems.Count() > 1)
                throw new InvalidOperationException("Multiple valid items found. This should never happen!");

            return validItems.First();
        }

        /// <summary>
        /// Returns multiple random items from the list of items, based on their probabilities
        /// </summary>
        /// <param name="count">How many items to return</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when count is less than one</exception>
        public IEnumerable<T> NextItems(int count)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException("count");

            for (int i = 0; i < count; i++)
                yield return NextItem();
        }
    }
}
