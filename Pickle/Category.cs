using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pickle
{
    public sealed class Category<T>
    {
        IEnumerable<T> Items
        {
            get { return ranges.Select(r => r.Item); }
        }

        List<Category<T>> childCats; // lol cats
        List<Range<T>> ranges;

        public Category()
        {
            ranges = new List<Range<T>>();
            childCats = new List<Category<T>>();
        }

        public void AddItem(T item, double prob)
        {
            if (prob > 100 || prob < 1)
                throw new ArgumentOutOfRangeException("prob");

            if (Items.Contains(item))
                throw new ArgumentException(String.Format("Item {0} is already added", item));

            Range<T> previous = ranges.LastOrDefault();

            if (previous == null)
                ranges.Add(new Range<T>(item, 0, prob));
            else
                ranges.Add(new Range<T>(item, previous.HighBound, previous.HighBound + prob));
        }

        public void RemoveItem(T item)
        {
            if (!Items.Contains(item))
                throw new ArgumentException(String.Format("Item {0} doesn't exist in the picker", item));

            int index = Items.ToList().IndexOf(item);
            ranges.RemoveAll(r => r.Item.Equals(item));
            UpdateRanges();
        }

        public void UpdateProbability(T item, double prob)
        {
            if (!Items.Contains(item))
                throw new ArgumentException(String.Format("Item {0} doesn't exist in the picker", item));

            ranges.Single(r => r.Item.Equals(item)).HighBound = prob;
            UpdateRanges();
        }

        public void ClearItems()
        {
            ranges.Clear();
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

                Console.WriteLine("{0}: {1} to {2} ({3})", range.Item.ToString(), range.LowBound, range.HighBound, range.Size);
            }
        }
    }
}
