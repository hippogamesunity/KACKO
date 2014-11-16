using System;
using System.Collections.Generic;

namespace Iteco.Autotests.Common.Utilities
{
    public class GenericComparer<T> : IComparer<T>
    {
        private readonly Func<T, T, int> _func;

        public GenericComparer(Func<T, T, int> func)
        {
            _func = func;
        }

        public int Compare(T x, T y)
        {
            return ReferenceEquals(x, y) ? 0 : _func(x, y);
        }
    }

    public class GenericEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, object> _func;

        public GenericEqualityComparer(Func<T, object> func)
        {
            _func = func;
        }

        public bool Equals(T x, T y)
        {
            return _func(x).Equals(_func(y));
        }

        public int GetHashCode(T i)
        {
            return _func(i).GetHashCode();
        }
    }  
}