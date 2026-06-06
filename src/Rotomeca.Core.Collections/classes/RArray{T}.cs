using Rotomeca.Core.Collections.Internal;
using Rotomeca.Core.Collections.Interfaces;
using System.Collections;

namespace Rotomeca.Core.Collections
{
#if NET7_0_OR_GREATER
    /// <summary>
    /// Implémentation concrète de <see cref="IRArray{T}"/>, inspirée de l'API
    /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array">Array JavaScript</see>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Le backing store interne est une <see cref="List{T}"/> allouée à la construction.
    /// Les méthodes mutantes (<see cref="Push(T[])"/>, <see cref="Pop"/>, <see cref="Sort"/>, etc.)
    /// modifient cette liste en place. Les méthodes préfixées par <c>To</c>
    /// (<see cref="ToReversed"/>, <see cref="ToSorted"/>, etc.) retournent toujours une nouvelle instance.
    /// </para>
    /// <para>
    /// Cible les frameworks <c>netstandard2.0</c>, <c>netstandard2.1</c>, <c>net8.0</c>,
    /// <c>net9.0</c> et <c>net10.0</c>. Sur <c>NET7_0_OR_GREATER</c>, la classe implémente
    /// également <see cref="IRArrayFactory{T, TSelf}"/> et expose les membres statiques abstraits
    /// <see cref="From"/>, <see cref="Of"/> et <see cref="FromAsync"/>.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">Le type des éléments contenus dans la collection.</typeparam>
#else
    /// <summary>
    /// Implémentation concrète de <see cref="IRArray{T}"/>, inspirée de l'API
    /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array">Array JavaScript</see>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Le backing store interne est une <see cref="List{T}"/> allouée à la construction.
    /// Les méthodes mutantes (<see cref="Push(T[])"/>, <see cref="Pop"/>, <see cref="Sort"/>, etc.)
    /// modifient cette liste en place. Les méthodes préfixées par <c>To</c>
    /// (<see cref="ToReversed"/>, <see cref="ToSorted"/>, etc.) retournent toujours une nouvelle instance.
    /// </para>
    /// <para>
    /// Cible les frameworks <c>netstandard2.0</c>, <c>netstandard2.1</c>, <c>net8.0</c>,
    /// <c>net9.0</c> et <c>net10.0</c>. Sur <c>NET7_0_OR_GREATER</c>, la classe implémente
    /// également <c>"IRArrayFactory{T, TSelf}"</c> et expose les membres statiques abstraits
    /// <see cref="From"/>, <see cref="Of"/> et <see cref="FromAsync"/>.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">Le type des éléments contenus dans la collection.</typeparam>
#endif
    public class RArray<T> : IRArray<T>
#if NET7_0_OR_GREATER
    , IRArrayFactory<T, RArray<T>>
#endif
    {
        private List<T> _values;

        /// <summary>
        /// Initialise une nouvelle instance vide de <see cref="RArray{T}"/>.
        /// </summary>
        public RArray()
        {
            _values = [];
        }

        /// <summary>
        /// Initialise une nouvelle instance à partir d'un tableau d'éléments.
        /// </summary>
        /// <param name="values">Éléments initiaux de la collection.</param>
        public RArray(params T[] values)
        {
            _values = [.. values];
        }

        /// <summary>
        /// Initialise une nouvelle instance à partir d'une séquence d'éléments.
        /// </summary>
        /// <param name="values">Séquence source dont les éléments sont copiés.</param>
        public RArray(IEnumerable<T> values)
        {
            _values = [.. values];
        }

        /// <inheritdoc/>
        public T this[int index]
        {
            get => At(index); set => _set(index, value);
        }

        /// <summary>
        /// Définit l'élément à l'index spécifié, en supportant les indices négatifs.
        /// </summary>
        /// <param name="index">
        /// Index de destination. Les valeurs négatives comptent depuis la fin.
        /// </param>
        /// <param name="value">Valeur à affecter.</param>
        /// <exception cref="IndexOutOfRangeException">
        /// Levée si l'index résolu est hors des bornes de la collection.
        /// </exception>
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

        /// <inheritdoc/>
        public int Length => _values.Count;

        /// <inheritdoc/>
        public T At(int index) => index >= 0 ? _AtPos((uint)index) : _AtNeg(index);

        /// <summary>
        /// Accède à l'élément à un index positif, après vérification des bornes.
        /// </summary>
        /// <param name="index">Index positif converti en <see cref="uint"/>.</param>
        /// <returns>L'élément à l'index spécifié.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Levée si <paramref name="index"/> est supérieur ou égal à <see cref="Length"/>.
        /// </exception>
        private T _AtPos(uint index)
        {
            if (_values.Count <= index)
                throw new IndexOutOfRangeException($"Index {index} est hors bornes pour un tableau de taille {_values.Count}.");
            return _values[(int)index];
        }

        /// <summary>
        /// Accède à l'élément à un index négatif en le résolvant depuis la fin du tableau.
        /// </summary>
        /// <param name="index">Index négatif (ex. : <c>-1</c> = dernier élément).</param>
        /// <returns>L'élément à l'index résolu.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Levée si l'index résolu est inférieur à zéro.
        /// </exception>
        private T _AtNeg(int index)
        {
            var realIndex = _values.Count + index;
            if (realIndex < 0)
                throw new IndexOutOfRangeException($"Index {index} est hors bornes pour un tableau de taille {_values.Count}.");
            return _AtPos((uint)realIndex);
        }

        /// <inheritdoc/>
        public int Push(params T[] items) => Push((IEnumerable<T>)items);

        /// <summary>
        /// Ajoute tous les éléments de <paramref name="items"/> en fin de collection.
        /// </summary>
        /// <remarks>
        /// Surcharge acceptant directement un <see cref="IEnumerable{T}"/> pour éviter
        /// la récursion infinie que provoquerait l'appel depuis <see cref="Push(T[])"/>.
        /// </remarks>
        /// <param name="items">Séquence d'éléments à ajouter.</param>
        /// <returns>La nouvelle taille de la collection après l'ajout.</returns>
        public int Push(IEnumerable<T> items)
        {
            _values.AddRange(items);
            return Length;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public int Unshift(params T[] items)
        {
            _values.InsertRange(0, items);
            return Length;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        /// <remarks>
        /// Un buffer intermédiaire est utilisé pour garantir un comportement correct
        /// lorsque la plage source et la destination se chevauchent.
        /// </remarks>
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

            var buffer = _values.GetRange(actualStart, count);

            for (int i = 0; i < buffer.Count; i++)
                _values[actualTarget + i] = buffer[i];

            return this;
        }

        /// <inheritdoc/>
        public IRArray<T> Sort(Comparison<T>? compareFn = null)
        {
            if (compareFn is not null) _values.Sort(compareFn);
            else _values.Sort();

            return this;
        }

        /// <inheritdoc/>
        public IRArray<T> Reverse()
        {
            _values.Reverse();
            return this;
        }

        /// <inheritdoc/>
        public int IndexOf(T value) => _values.IndexOf(value);

        /// <inheritdoc/>
        public int LastIndexOf(T value) => _values.LastIndexOf(value);

        /// <inheritdoc/>
        public T? Find(Func<T, bool> fn) => _values.Find((x) => fn(x));

        /// <inheritdoc/>
        public int FindIndex(Func<T, bool> fn) => _values.FindIndex((x) => fn(x));

        /// <inheritdoc/>
        public T? FindLast(Func<T, bool> fn) => _values.FindLast(x => fn(x));

        /// <inheritdoc/>
        public int FindLastIndex(Func<T, bool> fn) => _values.FindLastIndex(x => fn(x));

        /// <inheritdoc/>
        public bool Includes(T value) => _values.Contains(value);

        /// <inheritdoc/>
        public bool Every(Func<T, bool> fn) => _values.All(x => fn(x));

        /// <inheritdoc/>
        public bool Some(Func<T, bool> fn) => _values.Any(x => fn(x));

        /// <inheritdoc/>
        public IRArray<T> Filter(Func<T, bool> fn) => _values.Clone().Where(fn).ToIRArray();

        /// <inheritdoc/>
        public IRArray<TResult> Map<TResult>(Func<T, TResult> fn) => _values.Clone().Select(fn).ToIRArray();

        /// <inheritdoc/>
        public IRArray<TResult> FlatMap<TResult>(Func<T, IEnumerable<TResult>> fn)
        {
            var result = new RArray<TResult>();

            foreach (var item in _values)
                result.Push(fn(item));

            return result;
        }

        /// <inheritdoc/>
        public TResult Reduce<TResult>(Func<TResult, T, TResult> fn, TResult initialValue)
        {
            var accumulator = initialValue;
            foreach (var item in _values)
                accumulator = fn(accumulator, item);
            return accumulator;
        }

        /// <inheritdoc/>
        public TResult ReduceRight<TResult>(Func<TResult, T, TResult> fn, TResult initialValue)
        {
            var accumulator = initialValue;
            for (int i = _values.Count - 1; i >= 0; i--)
                accumulator = fn(accumulator, _values[i]);
            return accumulator;
        }

        /// <inheritdoc/>
        public IRArray<T> Concat(params IEnumerable<T>[] others)
        {
            List<T> tmp = [.. _values];
            foreach (var other in others)
                tmp.AddRange(other);
            return new RArray<T>(tmp);
        }

        /// <inheritdoc/>
        public IRArray<T> Slice(int start = 0, int? end = null)
        {
            int length = _values.Count;

            int actualStart = start < 0 ? Math.Max(length + start, 0) : Math.Min(start, length);
            int actualEnd = end.HasValue
                ? (end.Value < 0 ? Math.Max(length + end.Value, 0) : Math.Min(end.Value, length))
                : length;

            return new RArray<T>(_values.GetRange(actualStart, Math.Max(actualEnd - actualStart, 0)));
        }

        /// <inheritdoc/>
        public IRArray<T> ToReversed()
        {
            List<T> tmp = [.. _values];
            tmp.Reverse();
            return new RArray<T>(tmp);
        }

        /// <inheritdoc/>
        public IRArray<T> ToSorted(Comparison<T>? compareFn = null)
        {
            List<T> tmp = [.. _values];
            if (compareFn is not null) tmp.Sort(compareFn);
            else tmp.Sort();
            return new RArray<T>(tmp);
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Clone la collection courante, applique <see cref="Splice"/> sur le clone
        /// (dont le résultat — les éléments supprimés — est ignoré), puis retourne le clone modifié.
        /// </remarks>
        public IRArray<T> ToSpliced(int start, int? deleteCount = null, params T[] items)
        {
            var clone = new RArray<T>(_values);
            clone.Splice(start, deleteCount, items);
            return clone;
        }

        /// <inheritdoc/>
        public IRArray<T> With(int index, T value)
        {
            int length = _values.Count;
            int actualIndex = index < 0 ? length + index : index;

            if (actualIndex < 0 || actualIndex >= length)
                throw new IndexOutOfRangeException($"Index {index} est hors bornes pour un tableau de taille {length}.");

            List<T> tmp = [.. _values];
            tmp[actualIndex] = value;
            return new RArray<T>(tmp);
        }

        /// <inheritdoc/>
        public void ForEach(Action<T> fn)
        {
            foreach (var item in _values)
                fn(item);
        }

        /// <inheritdoc/>
        public IEnumerable<(int Index, T Value)> Entries()
        {
            for (int i = 0; i < _values.Count; i++)
                yield return (i, _values[i]);
        }

        /// <inheritdoc/>
        public IEnumerable<int> Keys()
        {
            for (int i = 0; i < _values.Count; i++)
                yield return i;
        }

        /// <inheritdoc/>
        public IEnumerable<T> Values() => _values.Clone();

        /// <inheritdoc/>
        public string Join(string separator = ",") => string.Join(separator, _values);

        /// <inheritdoc/>
        public T[] ToArray() => [.. _values];

        /// <summary>
        /// Crée une instance de <see cref="RArray{T}"/> à partir d'une séquence.
        /// </summary>
        /// <param name="source">Séquence source.</param>
        /// <returns>Nouvelle instance contenant les éléments de <paramref name="source"/>.</returns>
        public static RArray<T> From(IEnumerable<T> source) => new(source);

        /// <summary>
        /// Crée une instance de <see cref="RArray{T}"/> à partir des éléments passés en argument.
        /// </summary>
        /// <param name="items">Éléments à inclure dans la collection.</param>
        /// <returns>Nouvelle instance contenant <paramref name="items"/>.</returns>
        public static RArray<T> Of(params T[] items) => new(items);

#if NETSTANDARD2_0
        /// <summary>
        /// Crée une instance de <see cref="RArray{T}"/> en attendant séquentiellement
        /// chaque tâche de <paramref name="source"/>.
        /// </summary>
        /// <remarks>
        /// Disponible sur <c>netstandard2.0</c> uniquement, où <c>IAsyncEnumerable&lt;T&gt;</c>
        /// n'est pas supporté. Sur les cibles supérieures, cette surcharge accepte
        /// directement un <c>IAsyncEnumerable&lt;T&gt;</c>.
        /// </remarks>
        /// <param name="source">Collection de tâches dont les résultats alimentent la collection.</param>
        /// <returns>
        /// Tâche qui se complète avec une nouvelle instance de <see cref="RArray{T}"/>
        /// contenant les résultats résolus dans l'ordre d'énumération.
        /// </returns>
        public static async Task<RArray<T>> FromAsync(IEnumerable<Task<T>> source)
        {
            var result = new RArray<T>();
            foreach (var task in source)
                result.Push(await task);
            return result;
        }
#else
        /// <summary>
        /// Crée une instance de <see cref="RArray{T}"/> en consommant séquentiellement
        /// la séquence asynchrone <paramref name="source"/>.
        /// </summary>
        /// <remarks>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/fromAsync"><c>Array.fromAsync()</c></see>
        /// (ES2024). Non disponible sur <c>netstandard2.0</c> ; sur cette cible, une surcharge
        /// acceptant <see cref="IEnumerable{T}"/> de <see cref="Task{T}"/> est exposée à la place.
        /// </remarks>
        /// <param name="source">Séquence asynchrone source.</param>
        /// <returns>
        /// Tâche qui se complète avec une nouvelle instance de <see cref="RArray{T}"/>
        /// contenant tous les éléments énumérés dans l'ordre.
        /// </returns>
        public static async Task<RArray<T>> FromAsync(IAsyncEnumerable<T> source)
        {
            var result = new RArray<T>();
            await foreach (var item in source)
                result.Push(item);
            return result;
        }
#endif

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

namespace Rotomeca.Core.Collections.Internal
{
    /// <summary>
    /// Extensions internes utilisées par <see cref="Rotomeca.Core.Collections.RArray{T}"/>.
    /// Ne pas exposer publiquement.
    /// </summary>
    internal static class _______StListExt
    {
        /// <summary>
        /// Convertit une séquence en <see cref="Rotomeca.Core.Collections.Interfaces.IRArray{T}"/>.
        /// </summary>
        /// <typeparam name="T">Le type des éléments.</typeparam>
        /// <param name="values">Séquence source.</param>
        /// <returns>Nouvelle instance de <see cref="Rotomeca.Core.Collections.RArray{T}"/>.</returns>
        public static IRArray<T> ToIRArray<T>(this IEnumerable<T> values) => new RArray<T>(values);

        /// <summary>
        /// Retourne une copie superficielle de la liste sous forme de <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">Le type des éléments.</typeparam>
        /// <param name="internal">Liste source à cloner.</param>
        /// <returns>Nouvelle séquence indépendante contenant les mêmes éléments.</returns>
        public static IEnumerable<T> Clone<T>(this List<T> @internal)
        {
            List<T> tmp = [.. @internal];
            return tmp;
        }
    }
}