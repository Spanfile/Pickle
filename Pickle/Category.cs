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
        public Category<T> Parent => parent;

        public string Path => parent == null ? Name : parent.Path + "/" + Name;

        IEnumerable<string> CatNames => childCats.Select(c => c.Name);

        public string Name { get; set; }

        Category<T> parent;

        Picker<T> picker;
        List<Category<T>> childCats; // lol cats

        Random rand;

        public Category(string name, Category<T> parent, Random rand)
        {
            picker = new Picker<T>();
            childCats = new List<Category<T>>();

            Name = name;

            this.rand = rand;
            this.parent = parent;

            //Console.WriteLine(Path);
        }

        public Category<T> AddCategory(string name)
        {
            if (childCats.Select(c => c.Name).Contains(name))
                throw new ArgumentException($"Category {Name} already contains child category {name}");

            var cat = new Category<T>(name, this, rand);
            childCats.Add(cat);
            return cat;
        }

        public void RemoveCategory(string name)
        {
            if (!childCats.Select(c => c.Name).Contains(name))
                throw new ArgumentException($"Category {Name} doesn't contain a child category {name}");

            childCats.RemoveAll(c => c.Name == name);
        }

        public void Remove()
        {
            if (parent != null)
                parent.RemoveCategory(Name);
        }

        public Category<T> FindCat(string path)
        {
            var pathArgs = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            if (pathArgs.Length < 1)
                return this;

            var find = pathArgs.First();

            if (!childCats.Select(c => c.Name).Contains(find))
                return null;

            return childCats.Where(c => c.Name == find).Single().FindCat(string.Join("/", pathArgs.Skip(1)));
        }

        public bool HasCat(string name) => childCats.Find(c => c.Name == name) != null;

        public Category<T> GetCat(string name) => childCats.SingleOrDefault(c => c.Name == name);

        public void AddItem(T item, double weight) => picker.AddItem(item, weight);
        public bool RemoveItem(T item) => picker.RemoveItem(item);
        public void UpdateWeight(T item, double weight) => picker.UpdateWeight(item, weight);
        public void ClearItems() => picker.ClearItems();

        public T NextItem()
        {
            if (picker.HasItems())
                return picker.NextItem();

            var index = rand.Next(0, childCats.Count - 1);
            return childCats[index].NextItem();
        }

        public IEnumerable<T> NextItems(int count)
        {
            for (int i = 0; i < count; i++)
                yield return NextItem();
        }
    }
}
