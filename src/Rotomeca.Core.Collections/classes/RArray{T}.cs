using System.Collections.Immutable;
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
            int length = _values.Count;

            int actualStart = start < 0
                ? Math.Max(length + start, 0)
                : Math.Min(start, length);

            int actualDeleteCount = deleteCount.HasValue
                ? Math.Min(Math.Max(deleteCount.Value, 0), length - actualStart)
                : length - actualStart;

            var removed = _values.GetRange(actualStart, actualDeleteCount);
            _values.RemoveRange(actualStart, actualDeleteCount);

            if (items.Length > 0)
                _values.InsertRange(actualStart, items);

            return new RArray<T>(removed);
        }

        public IRArray<T> Fill(T value, int start = 0, int? end = null)
        {
            int length = _values.Count;

            int actualStart = start < 0 ? Math.Max(length + start, 0) : Math.Min(start, length);
            int actualEnd = end.HasValue
                ? (end.Value < 0 ? Math.Max(length + end.Value, 0) : Math.Min(end.Value, length))
                : length;

            for (int i = actualStart; i < actualEnd; i++)
                _values[i] = value;

            return this;
        }
        public IRArray<T> CopyWithin(int target, int start = 0, int? end = null)
        {
            int length = _values.Count;

            int actualTarget = target < 0 ? Math.Max(length + target, 0) : Math.Min(target, length);
            int actualStart = start < 0 ? Math.Max(length + start, 0) : Math.Min(start, length);
            int actualEnd = end.HasValue
                ? (end.Value < 0 ? Math.Max(length + end.Value, 0) : Math.Min(end.Value, length))
                : length;

            int count = Math.Min(actualEnd - actualStart, length - actualTarget);
            if (count <= 0) return this;

            // Buffer intermédiaire pour gérer correctement les plages qui se chevauchent
            var buffer = _values.GetRange(actualStart, count);

            for (int i = 0; i < buffer.Count; i++)
                _values[actualTarget + i] = buffer[i];

            return this;
        }
        public IRArray<T> Sort(Comparison<T>? compareFn = null)
        {
            List<T> values = [.. _values];

            if (compareFn is not null) values.Sort(compareFn);
            else values.Sort();

            return new RArray<T>(values);
        }

        public IRArray<T> Reverse()
        {
            List<T> values = [.. _values];

            values.Reverse();
            return new RArray<T>(values);
        }

        public int IndexOf(T value) => _values.IndexOf(value);
        public int LastIndexOf(T value) => _values.LastIndexOf(value);
        public T? Find(Func<T, bool> fn) => _values.Find((x) => fn(x));
        public int FindIndex(Func<T, bool> fn) => _values.FindIndex((x) => fn(x));
        public T? FindLast(Func<T, bool> fn) => _values.FindLast(x => fn(x));
        public int FindLastIndex(Func<T, bool> fn) => _values.FindLastIndex(x => fn(x));
        public bool Includes(T value) => _values.Contains(value);

        public bool Every(Func<T, bool> fn) => _values.All(x => fn(x));
        public bool Some(Func<T, bool> fn) => _values.Any(x => fn(x));

        public IRArray<T> Filter(Func<T, bool> fn)
        {
            List<T> value = [.. _values];

            return new RArray<T>(value.Where(x => fn(x)));
        }
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