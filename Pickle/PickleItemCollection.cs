using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pickle
{
    struct ItemRange<T>
    {
        public T item;
        public double start;
        public double end;

        public ItemRange(T item, double start, double end)
        {
            this.item = item;
            this.start = start;
            this.end = end;
        }
    }

    public class PickleItemRanges<T>
    {
        List<ItemRange<T>> ranges;

        public PickleItemRanges()
        {
            ranges = new List<ItemRange<T>>();
        }

        public void Add(T item, double weight)
        {
            var last = ranges.Sum(r => r.end - r.start);
            ranges.Add(new ItemRange<T>(item, last, last + weight));
        }

        public void Clear() => ranges.Clear();

        public T this[double val] => ranges.Where(r => val >= r.start && val < r.end).Single().item;
    }
}
