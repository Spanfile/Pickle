using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pickle
{
    // cats
    // cats everywhere
    internal class Category<T>
    {
        public Category<T> Parent
        {
            get { return parent; }
        }

        public string Path
        {
            get
            {
                if (parent == null)
                    return Name;

                return parent.Path + "/" + Name;
            }
        }

        IEnumerable<T> Items
        {
            get { return ranges.Select(r => r.Item); }
        }

        IEnumerable<string> CatNames
        {
            get { return childCats.Select(c => c.Name); }
        }

        public string Name { get; set; }

        Category<T> parent;

        List<Category<T>> childCats; // lol cats
        List<Range<T>> ranges;

        Random rand;

        public Category(string name, Category<T> parent, Random rand)
        {
            ranges = new List<Range<T>>();
            childCats = new List<Category<T>>();

            Name = name;

            this.rand = rand;
            this.parent = parent;

            Console.WriteLine(Path);
        }

        public void AddCategory(string name)
        {
            if (childCats.Select(c => c.Name).Contains(name))
                throw new ArgumentException(String.Format("Category {0} already contains child category {1}", Name, name));

            childCats.Add(new Category<T>(name, this, rand));
        }

        public void RemoveCategory(string name)
        {
            if (!childCats.Select(c => c.Name).Contains(name))
                throw new ArgumentException(String.Format("Category {0} doesn't contain a child category {1}", Name, name));
        }

        public void Remove()
        {
            if (parent != null)
                parent.RemoveCategory(Name);
        }

        public Category<T> FindCat(string path)
        {
            string[] pathArgs = path.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            if (pathArgs.Length < 1)
                return this;

            string find = pathArgs.First();

            if (!childCats.Select(c => c.Name).Contains(find))
                return null;

            return childCats.Where(c => c.Name == find).Single().FindCat(String.Join("/", pathArgs.Skip(1)));
        }

        public T FindItem(string name, Func<T, string> nameFunc)
        {
            var itemNames = Items.Select(i => new { Item = i, Match = nameFunc(i) == name });
            T match = itemNames.Single(a => a.Match).Item;
            if (match == null)
                throw new ArgumentException(String.Format("Item {0} doesn't exist in the category"));

            return match;
        }

        public void AddItem(T item, double prob)
        {
            if (prob > 100 || prob < 1)
                throw new ArgumentOutOfRangeException("prob");

            if (Items.Contains(item))
                throw new ArgumentException(String.Format("Item {0} is already added", item));

            Range<T> previous = ranges.LastOrDefault();

            Console.WriteLine("{0}: Adding item {1}", Path, item);

            if (previous == null)
                ranges.Add(new Range<T>(item, 0, prob));
            else
                ranges.Add(new Range<T>(item, previous.HighBound, previous.HighBound + prob));
        }

        public void RemoveItem(T item)
        {
            if (!Items.Contains(item))
                throw new ArgumentException(String.Format("Item {0} doesn't exist in the category", item));

            int index = Items.ToList().IndexOf(item);
            ranges.RemoveAll(r => r.Item.Equals(item));
            UpdateRanges();
        }

        public void UpdateProbability(T item, double prob)
        {
            if (!Items.Contains(item))
                throw new ArgumentException(String.Format("Item {0} doesn't exist in the category", item));

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
