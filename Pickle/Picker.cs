﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Pickle
{
    public sealed class Picker<T>
    {
        IEnumerable<T> Items => items.Select(r => r.Key);

        Dictionary<T, double> items;
        Random rand;

        bool dirty = false;
        double sum = 0;
        PickleItemRanges<T> ranges;

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
            items = new Dictionary<T, double>();
            ranges = new PickleItemRanges<T>();

            this.rand = rand;
        }

        public void AddItem(T item, double weight)
        {
            if (weight < 0)
                throw new ArgumentOutOfRangeException(nameof(weight));

            if (Items.Contains(item))
                throw new ArgumentException($"Item {item} is already added");

            items.Add(item, weight);
        }

        public bool RemoveItem(T item) => items.Remove(item);

        /// <summary>
        /// Removes all items in the picker.
        /// </summary>
        public void ClearItems() => items.Clear();

        public void UpdateWeight(T item, double weight)
        {
            if (!Items.Contains(item))
                throw new ArgumentException("Item hasn't been added to the picker");

            if (weight < 0)
                throw new ArgumentOutOfRangeException(nameof(weight));

            items[item] = weight;
        }

        /// <summary>
        /// Returns true if the picker contains any items.
        /// </summary>
        /// <returns></returns>
        public bool HasItems() => items.Any();

        /// <summary>
        /// Returns a random item from the picker, based on the probabilities of each item.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown when the sum of item probabilities isn't 100.</exception>
        public T NextItem()
        {
            if (!HasItems())
                throw new InvalidOperationException("Picker contains no items"); // TODO: should this throw an exception or just return default(T)?

            if (dirty)
            {
                sum = items.Select(p => p.Value).Sum();

                ranges.Clear();
                foreach (var item in items.Select(p => new { Item = p.Key, Weight = p.Value }))
                    ranges.Add(item.Item, item.Weight);

                dirty = false;
            }

            double val = rand.NextDouble() * sum;

            return ranges[val];
        }

        /// <summary>
        /// Returns random items from the picker, based on the probabilities of each item.
        /// </summary>
        /// <param name="count">How many items to return.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when count is less than one.</exception>
        public IEnumerable<T> NextItems(int count)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException("count");

            for (int i = 0; i < count; i++)
                yield return NextItem();
        }
    }
}
