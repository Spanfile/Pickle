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
