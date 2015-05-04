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

        public string Path
        {
            get
            {
                if (parent == null)
                    return Name;

                return parent.Path + "/" + Name;
            }
        }

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

            Console.WriteLine(Path);
        }

        public Category<T> AddCategory(string name)
        {
            if (childCats.Select(c => c.Name).Contains(name))
                throw new ArgumentException($"Category {Name} already contains child category {name}");

            Category<T> cat = new Category<T>(name, this, rand);
            childCats.Add(cat);
            return cat;
        }

        public void RemoveCategory(string name)
        {
            if (!childCats.Select(c => c.Name).Contains(name))
                throw new ArgumentException($"Category {Name} doesn't contain a child category {name}");
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

        public bool HasCat(string name)
        {
            return childCats.Find(c => c.Name == name) != null;
        }

        public Category<T> GetCat(string name)
        {
            return childCats.SingleOrDefault(c => c.Name == name);
        }

        public void AddItem(T item, double prob)
        {
            picker.AddItem(item, prob);
        }

        public void RemoveItem(T item)
        {
            picker.RemoveItem(item);
        }

        public void UpdateProbability(T item, double prob)
        {
            picker.UpdateProbability(item, prob);
        }

        public void ClearItems()
        {
            picker.ClearItems();
        }

        public T NextItem()
        {
            if (picker.HasItems())
                return picker.NextItem();

            int index = rand.Next(0, childCats.Count - 1);
            return childCats[index].NextItem();
        }

        public IEnumerable<T> NextItems(int count)
        {
            for (int i = 0; i < count; i++)
                yield return NextItem();
        }
    }
}
