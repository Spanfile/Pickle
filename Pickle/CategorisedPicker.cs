using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pickle
{
    public sealed class CategorisedPicker<T>
    {
        Category<T> rootCat;

        Func<T, string> getTName;

        Random rand;

        /// <summary>
        /// Creates a new CategorisedPicker, allowing you to define categories for different items. Works like a normal Picker.
        /// </summary>
        /// <param name="getTNameFunc">A method that takes in your item and has to return a unique name for that item.</param>
        public CategorisedPicker(Func<T, string> getTNameFunc)
        {
            rand = new Random();

            rootCat = new Category<T>("CategoryRoot", null, rand);

            getTName = getTNameFunc;
        }

        /// <summary>
        /// Adds a category to the picker.
        /// </summary>
        /// <param name="name">The unique name of the category.</param>
        public void AddCategory(string name)
        {
            AddCategory(name, "");
        }
        /// <summary>
        /// Adds a category to another category in the picker.
        /// </summary>
        /// <param name="name">The unique name of the category.</param>
        /// <param name="path">The path for the category.</param>
        /// <exception cref="System.ArgumentException">Thrown when a category with given name already exists or path is invalid.</exception>
        public void AddCategory(string name, string path) // TODO: should this work just with one arg, the full path of the category you want to add?
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

        /// <summary>
        /// Removes a category from the picker.
        /// </summary>
        /// <param name="path">The path for the category.</param>
        /// <exception cref="System.ArgumentException">Thrown when the given path is invalid.</exception>
        public void RemoveCategory(string path)
        {
            FindCat(path).Remove();
        }

        /// <summary>
        /// Adds an item to the picker.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="prob">The probability for the item to be returned.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when prob is less than 1 or higher than 100.</exception>
        /// <exception cref="System.ArgumentException">Thrown when item has already been added.</exception>
        public void AddItem(T item, double prob)
        {
            AddItem("", item, prob);
        }
        /// <summary>
        /// Adds an item to a category in the picker.
        /// </summary>
        /// <param name="path">The path for the category where the item should be added.</param>
        /// <param name="item">The item to add.</param>
        /// <param name="prob">The probability for the item to be returned.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when prob is less than 1 or higher than 100.</exception>
        /// <exception cref="System.ArgumentException">Thrown when item has already been added or when path is invalid.</exception>
        public void AddItem(string path, T item, double prob)
        {
            if (path.Trim() == "")
            {
                rootCat.AddItem(item, prob);
                return;
            }

            FindCat(path).AddItem(item, prob);
        }

        /// <summary>
        /// Removes an item from the picker.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <exception cref="System.ArgumentException">Thrown when item doesn't exist in the picker.</exception>
        public void RemoveItem(T item)
        {
            RemoveItem("", item);
        }
        /// <summary>
        /// Removes an item from a category in the picker.
        /// </summary>
        /// <param name="path">The path of the category.</param>
        /// <param name="item">The item to remove.</param>
        /// <exception cref="System.ArgumentException">Thrown when item doesn't exist in the picker or when path is invalid.</exception>
        public void RemoveItem(string path, T item)
        {
            if (path.Trim() == "")
            {
                rootCat.RemoveItem(item);
                return;
            }

            FindCat(path).RemoveItem(item);
        }

        /// <summary>
        /// Updates the probability of an existing item in the picker.
        /// </summary>
        /// <param name="item">The item of which's probability to update.</param>
        /// <param name="prob">The new probability of the item.</param>
        /// <exception cref="System.ArgumentException">Thrown when the given item isn't in the picker.</exception>
        public void UpdateProbability(T item, double prob)
        {
            UpdateProbability("", item, prob);
        }
        /// <summary>
        /// Updates the probability of an existing item in a category in the picker.
        /// </summary>
        /// <param name="path">The path of the category.</param>
        /// <param name="item">The item of which's probability to update.</param>
        /// <param name="prob">The new probability of the item.</param>
        /// <exception cref="System.ArgumentException">Thrown when the given item isn't in the picker or when path is invalid.</exception>
        public void UpdateProbability(string path, T item, double prob)
        {
            if (path.Trim() == "")
            {
                rootCat.UpdateProbability(item, prob);
                return;
            }

            FindCat(path).UpdateProbability(item, prob);
        }

        /// <summary>
        /// Returns a random item from the picker, based on the probabilities of each item.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the sum of item probabilities isn't 100.</exception>
        public T NextItem()
        {
            return rootCat.NextItem();
        }
        /// <summary>
        /// Returns a random item from a category in the picker, based on the probabilities of each item.
        /// </summary>
        /// <param name="path">The path of the category.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the sum of item probabilities isn't 100.</exception>
        /// <exception cref="System.ArgumentInvalidException">Thrown when path is invalid.</exception>
        public T NextItem(string path)
        {
            if (path.Trim() == "")
                return rootCat.NextItem();

            return FindCat(path).NextItem();
        }

        /// <summary>
        /// Returns random items from the picker, based on the probabilities of each item.
        /// </summary>
        /// <param name="count">How many items to return.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when count is less than one.</exception>
        public IEnumerable<T> NextItems(int count)
        {
            return rootCat.NextItems(count);
        }
        /// <summary>
        /// Returns random items from a category in the picker, based on the probabilities of each item.
        /// </summary>
        /// <param name="path">The path of the category.</param>
        /// <param name="count">How many items to return.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when count is less than one.</exception>
        /// <exception cref="System.ArgumentInvalidException">Thrown when path is invalid.</exception>
        public IEnumerable<T> NextItems(string path, int count)
        {
            return FindCat(path).NextItems(count);
        }

        Category<T> FindCat(string path)
        {
            return rootCat.FindCat(path);
        }
    }
}
