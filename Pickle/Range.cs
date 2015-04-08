﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pickle
{
    internal class Range<T>
    {
        public T Item
        {
            get { return item; }
        }

        public double LowBound
        {
            get { return lowBound; }
            set { lowBound = value; }
        }

        public double HighBound
        {
            get { return highBound; }
            set { highBound = value; }
        }

        public double Size
        {
            get { return highBound - lowBound; }
        }

        T item;
        double lowBound;
        double highBound;

        public Range(T item, double lowBound, double highBound)
        {
            this.item = item;
            this.lowBound = lowBound;
            this.highBound = highBound;
        }

        public bool Contains(double value)
        {
            return value >= lowBound && value < highBound;
        }

        public void Move(double amount)
        {
            lowBound += amount;
            highBound += amount;
        }
    }
}
