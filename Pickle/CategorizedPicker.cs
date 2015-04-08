using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pickle
{
    public sealed class CategorizedPicker<T>
    {
        Category<T> rootCat;

        Func<T, string> getTName;

        Random rand;

        public CategorizedPicker(Func<T, string> getTNameFunc)
        {
            rand = new Random();

            rootCat = new Category<T>("CategoryRoot", null, rand);

            getTName = getTNameFunc;
        }

        public void AddCategory(string name)
        {
            AddCategory(name, "");
        }
        public void AddCategory(string name, string path)
        {
            if (path.Trim() == "")
            {
                rootCat.AddCategory(name);
                return;
            }

            Category<T> cat = FindCat(path);

            if (cat == null)
                throw new ArgumentException(String.Format("\"{0}\" is not a valid category path", path));

            cat.AddCategory(name);
        }

        public void AddItem(T item, double prob)
        {
            AddItem("", item, prob);
        }
        public void AddItem(string path, T item, double prob)
        {
            if (path.Trim() == "")
            {
                rootCat.AddItem(item, prob);
                return;
            }

            FindCat(path).AddItem(item, prob);
        }

        public void RemoveItem(T item)
        {
            RemoveItem("", item);
        }
        public void RemoveItem(string path, T item)
        {
            if (path.Trim() == "")
            {
                rootCat.RemoveItem(item);
                return;
            }

            FindCat(path).RemoveItem(item);
        }

        public void UpdateProbability(T item, double prob)
        {
            UpdateProbability("", item, prob);
        }
        public void UpdateProbability(string path, T item, double prob)
        {
            if (path.Trim() == "")
            {
                rootCat.UpdateProbability(item, prob);
                return;
            }

            FindCat(path).UpdateProbability(item, prob);
        }

        public void RemoveCategory(string path)
        {
            FindCat(path).Remove();
        }

        Category<T> FindCat(string path)
        {
            return rootCat.FindCat(path);
        }

        T FindItem(string path)
        {
            string[] pathArgs = path.Split('/');
            return FindCat(String.Join("/", pathArgs.Take(pathArgs.Length - 1))).FindItem(pathArgs.Last(), getTName);
        }
    }
}
