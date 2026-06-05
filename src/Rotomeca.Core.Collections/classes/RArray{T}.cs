using Rotomeca.Core.Collections.Interfaces;
namespace Rotomeca.Core.Collections
{
    public class RArray<T> : IRArray<T>
#if NET7_0_OR_GREATER
    , IRArrayFactory<T, RArray<T>>
#endif
    {
        private List<T> _values;

        public RArray()
        {
            _values = [];
        }

        public RArray(params T[] values)
        {
            _values = [.. values];
        }

        public RArray(IEnumerable<T> values)
        {
            _values = [.. values];
        }

        public T this[int index]
        {
            get => At(index); set => _set(index, value);
        }

        private void _set(int index, T value)
        {
            if (index >= 0) _values[index] = value;
            else
            {
                var realIndex = _values.Count + index;

                if (realIndex < 0) throw new IndexOutOfRangeException($"Index {index} est hors bornes pour un tableau de taille {_values.Count}.");
                _set(realIndex, value);
            }
        }

        public int Length => _values.Count;

        public T At(int index) => index >= 0 ? _AtPos((uint)index) : _AtNeg(index);

        private T _AtPos(uint index)
        {
            if (_values.Count <= index)  // ✅ (ton original avait > au lieu de <=)
                throw new IndexOutOfRangeException($"Index {index} est hors bornes pour un tableau de taille {_values.Count}.");
            return _values[(int)index];
        }

        private T _AtNeg(int index)
        {
            var realIndex = _values.Count + index;
            if (realIndex < 0)
                throw new IndexOutOfRangeException($"Index {index} est hors bornes pour un tableau de taille {_values.Count}.");
            return _AtPos((uint)realIndex);
        }
        public int Push(params T[] items)
        {
            _values.AddRange(items);
            return Length;
        }

        public T? Pop()
        {
            T? returnedValue;
            if (Length > 0)
            {
                returnedValue = At(-1);
                _values.RemoveAt(Length - 1);
            }
            else returnedValue = default;

            return returnedValue;
        }

        public int Unshift(params T[] items)
        {
            _values.InsertRange(0, items);
            return Length;
        }
        public T? Shift()
        {
            T? returnValue;
            if (Length > 0)
            {
                returnValue = _values[0];
                _values.RemoveAt(0);
            }
            else returnValue = default;

            return returnValue;
        }
        public IRArray<T> Splice(int start, int? deleteCount = null, params T[] items)
        {

        }
        public IRArray<T> Fill(T value, int start = 0, int? end = null);
        public IRArray<T> CopyWithin(int target, int start = 0, int? end = null);
        public IRArray<T> Sort(Comparison<T>? compareFn = null);
        public IRArray<T> Reverse();

        public int IndexOf(T value);
        public int LastIndexOf(T value);
        public T? Find(Func<T, bool> fn);
        public int FindIndex(Func<T, bool> fn);
        public T? FindLast(Func<T, bool> fn);
        public int FindLastIndex(Func<T, bool> fn);
        public bool Includes(T value);

        public bool Every(Func<T, bool> fn);
        public bool Some(Func<T, bool> fn);

        public IRArray<T> Filter(Func<T, bool> fn);
        public IRArray<TResult> Map<TResult>(Func<T, TResult> fn);
        public IRArray<TResult> FlatMap<TResult>(Func<T, IEnumerable<TResult>> fn);
        public TResult Reduce<TResult>(Func<TResult, T, TResult> fn, TResult initialValue);
        public TResult ReduceRight<TResult>(Func<TResult, T, TResult> fn, TResult initialValue);
        public IRArray<T> Concat(params IEnumerable<T>[] others);
        public IRArray<T> Slice(int start = 0, int? end = null);

        public IRArray<T> ToReversed();
        public IRArray<T> ToSorted(Comparison<T>? compareFn = null);
        public IRArray<T> ToSpliced(int start, int? deleteCount = null, params T[] items);
        public IRArray<T> With(int index, T value);

        public void ForEach(Action<T> fn);
        public IEnumerable<(int Index, T Value)> Entries();
        public IEnumerable<int> Keys();
        public IEnumerable<T> Values();

        public string Join(string separator = ",");
        public T[] ToArray();
    }
}