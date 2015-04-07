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

        bool changes = false;
        double itemSum = 0;

        /// <summary>
        /// Creates a new Picker which can be used to pick items based on their probabilities of being picked
        /// </summary>
        public Picker()
        {
            items = new Dictionary<T, double>();
            ranges = new List<Range<T>>();

            rand = new Random();
        }

        /// <summary>
        /// Adds an item to the picker.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="prob">The probability for the item to be returned.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when prob is less than 1 or higher than 100.</exception>
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

        /// <summary>
        /// Removes an item from the picker. Remember to update the probabilities of other items in the picker to maintain valid probabilities.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <exception cref="System.ArgumentException">Thrown when item doesn't exist in the picker.</exception>
        public void RemoveItem(T item)
        {
            if (!items.ContainsKey(item))
                throw new ArgumentException(String.Format("Item {0} doesn't exist in the picker", item));

            items.Remove(item);
            UpdateRanges();
        }

        /// <summary>
        /// Removes all items in the picker.
        /// </summary>
        public void ClearItems()
        {
            items.Clear();
            ranges.Clear();
        }

        /// <summary>
        /// Updates the probability of an existing item in the picker.
        /// </summary>
        /// <param name="item">The item of which's probability to update.</param>
        /// <param name="prob">The new probability of the item.</param>
        /// <exception cref="System.ArgumentException">Thrown when the given item isn't in the picker.</exception>
        public void UpdateProbability(T item, double prob)
        {
            if (!items.ContainsKey(item))
                throw new ArgumentException("Item hasn't been added to the picker");

            items[item] = prob;
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

            changes = true;
        }

        /// <summary>
        /// Returns a random item from the picker, based on the probabilities of each item.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the sum of item probabilities isn't 100.</exception>
        public T NextItem()
        {
            if (changes)
            {
                itemSum = items.Values.Sum();

                if (itemSum != 100)
                    throw new InvalidOperationException("Sum of item probabilites isn't 100");

                changes = false;
            }

            double val = rand.NextDouble() * 100d;

            return ranges.Where(r => r.Contains(val)).Select(r => r.Item).Single();
        }

        /// <summary>
        /// Returns a random item from the picker, based on the probabilities of each item.
        /// </summary>
        /// <param name="count">How many items to return.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when count is less than one.</exception>
        public IEnumerable<T> NextItems(int count)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException("count");

            for (int i = 0; i < count; i++)
                yield return NextItem();
        }
    }
}
