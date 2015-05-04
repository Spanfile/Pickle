using System;
using System.Collections.Generic;
using System.Linq;

namespace Pickle
{
    public sealed class Picker<T>
    {
        IEnumerable<T> Items => ranges.Select(r => r.Item);

        List<Range<T>> ranges;
        Random rand;

        bool changes = false;

        /// <summary>
        /// Creates a new Picker which can be used to pick items based on their probabilities of being picked. Uses the default Random object.
        /// </summary>
        public Picker()
            : this(new Random())
        {
        }

        /// <summary>
        /// Creates a new Picker which can be used to pick items based on their probabilites of being picked. You can supply your own Random object.
        /// </summary>
        /// <param name="rand">Your Random object.</param>
        public Picker(Random rand)
        {
            ranges = new List<Range<T>>();

            this.rand = rand;
        }

        /// <summary>
        /// Adds an item to the picker.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="prob">The probability for the item to be returned.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when prob is less than one or higher than 100.</exception>
        /// <exception cref="System.ArgumentException">Thrown when item has already been added.</exception>
        public void AddItem(T item, double prob)
        {
            if (prob > 100 || prob < 1)
                throw new ArgumentOutOfRangeException("prob");

            if (Items.Contains(item))
                throw new ArgumentException($"Item {item} is already added");

            Range<T> prev = ranges.LastOrDefault();
            if (prev == null)
                ranges.Add(new Range<T>(item, 0, prob));
            else
                ranges.Add(new Range<T>(item, prev.HighBound, prev.HighBound + prob));
        }

        /// <summary>
        /// Removes an item from the picker. Remember to update the probabilities of other items in the picker to maintain valid probabilities.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <exception cref="System.ArgumentException">Thrown when item doesn't exist in the picker.</exception>
        public void RemoveItem(T item)
        {
            if (!Items.Contains(item))
                throw new ArgumentException($"Item {item} doesn't exist in the picker");

            ranges.RemoveAll(r => r.Item.Equals(item));
            UpdateRanges();
        }

        /// <summary>
        /// Removes all items in the picker.
        /// </summary>
        public void ClearItems()
        {
            ranges.Clear();
        }

        /// <summary>
        /// Updates the probability of an existing item in the picker.
        /// </summary>
        /// <param name="item">The item of which's probability to update.</param>
        /// <param name="prob">The new probability of the item.</param>
        /// <exception cref="System.ArgumentException">Thrown when the given item isn't in the picker.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when prob is less than one or more than 100.</exception>
        public void UpdateProbability(T item, double prob)
        {
            if (!Items.Contains(item))
                throw new ArgumentException("Item hasn't been added to the picker");

            if (prob > 100 || prob < 1)
                throw new ArgumentOutOfRangeException("prob");

            ranges.Single(r => r.Item.Equals(item)).HighBound = prob;
            UpdateRanges();
        }

        /// <summary>
        /// Returns true if the picker contains any items.
        /// </summary>
        /// <returns></returns>
        public bool HasItems()
        {
            return ranges.Any();
        }

        void UpdateRanges()
        {
            double prev = 0;
            foreach (Range<T> range in ranges)
            {
                if (prev > 0)
                {
                    range.Move(range.LowBound - prev);
                    prev = range.HighBound;
                }
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
                if (ranges.Select(r => r.Size).Sum() != 100)
                    throw new InvalidOperationException("Sum of item probabilites isn't 100");

                changes = false;
            }

            double val = rand.NextDouble() * 100d;

            return ranges.Where(r => r.Contains(val)).Select(r => r.Item).Single();
        }

        /// <summary>
        /// Returns random items from the picker, based on the probabilities of each item.
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
