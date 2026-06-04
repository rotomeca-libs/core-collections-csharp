namespace Rotomeca.Core.Collections.Interfaces
{
    /// <summary>
    /// Contrat d'instance des collections Rotomeca, inspiré de l'API
    /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array">Array JavaScript</see>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Cette interface expose une surface fonctionnelle équivalente à celle des tableaux JavaScript,
    /// incluant les mutateurs (<see cref="Push"/>, <see cref="Pop"/>, <see cref="Splice"/>…),
    /// les méthodes de recherche (<see cref="Find"/>, <see cref="IndexOf"/>…),
    /// les transformations (<see cref="Map{TResult}"/>, <see cref="Filter"/>, <see cref="Reduce{TResult}"/>…)
    /// et les copies immuables (<see cref="ToReversed"/>, <see cref="ToSorted"/>…).
    /// </para>
    /// <para>
    /// Les méthodes mutantes modifient la collection en place et retournent <c>this</c> pour permettre le chaînage.
    /// Les méthodes préfixées par <c>To</c> retournent toujours une nouvelle instance sans modifier l'original.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">Le type des éléments contenus dans la collection.</typeparam>
    public interface IRArray<T> : IEnumerable<T>
    {
        /// <summary>
        /// Obtient le nombre d'éléments dans la collection.
        /// </summary>
        /// <remarks>
        /// Équivalent de la propriété
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/length"><c>Array.length</c></see>
        /// en JavaScript.
        /// </remarks>
        int Length { get; }

        /// <summary>
        /// Obtient ou définit l'élément à l'index spécifié.
        /// </summary>
        /// <param name="index">Index de l'élément (à partir de 0).</param>
        /// <exception cref="IndexOutOfRangeException">
        /// Levée si <paramref name="index"/> est hors des bornes de la collection.
        /// </exception>
        T this[int index] { get; set; }

        /// <summary>
        /// Retourne l'élément à l'index spécifié, en supportant les indices négatifs.
        /// </summary>
        /// <remarks>
        /// Un index négatif compte depuis la fin du tableau : <c>At(-1)</c> retourne le dernier élément.
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/at"><c>Array.prototype.at()</c></see>.
        /// </remarks>
        /// <param name="index">
        /// Index de l'élément. Les valeurs négatives comptent depuis la fin (ex. : <c>-1</c> = dernier élément).
        /// </param>
        /// <returns>L'élément à l'index résolu.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Levée si l'index résolu est hors des bornes de la collection.
        /// </exception>
        T At(int index);

        /// <summary>
        /// Ajoute un ou plusieurs éléments en fin de collection.
        /// </summary>
        /// <remarks>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/push"><c>Array.prototype.push()</c></see>.
        /// </remarks>
        /// <param name="items">Éléments à ajouter.</param>
        /// <returns>La nouvelle taille de la collection après l'ajout.</returns>
        int Push(params T[] items);

        /// <summary>
        /// Supprime et retourne le dernier élément de la collection.
        /// </summary>
        /// <remarks>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/pop"><c>Array.prototype.pop()</c></see>.
        /// </remarks>
        /// <returns>
        /// Le dernier élément supprimé, ou <c>null</c> si la collection est vide.
        /// </returns>
        T? Pop();

        /// <summary>
        /// Ajoute un ou plusieurs éléments en début de collection.
        /// </summary>
        /// <remarks>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/unshift"><c>Array.prototype.unshift()</c></see>.
        /// </remarks>
        /// <param name="items">Éléments à insérer en tête.</param>
        /// <returns>La nouvelle taille de la collection après l'insertion.</returns>
        int Unshift(params T[] items);

        /// <summary>
        /// Supprime et retourne le premier élément de la collection.
        /// </summary>
        /// <remarks>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/shift"><c>Array.prototype.shift()</c></see>.
        /// </remarks>
        /// <returns>
        /// Le premier élément supprimé, ou <c>null</c> si la collection est vide.
        /// </returns>
        T? Shift();

        /// <summary>
        /// Modifie la collection en supprimant et/ou insérant des éléments à partir de <paramref name="start"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Si <paramref name="deleteCount"/> est <c>null</c>, tous les éléments depuis <paramref name="start"/>
        /// jusqu'à la fin sont supprimés. Si <paramref name="deleteCount"/> vaut <c>0</c>, aucun élément
        /// n'est supprimé et <paramref name="items"/> sont simplement insérés.
        /// </para>
        /// <para>Les indices négatifs sont supportés pour <paramref name="start"/>.</para>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/splice"><c>Array.prototype.splice()</c></see>.
        /// </remarks>
        /// <param name="start">Index de départ de la modification. Supporte les valeurs négatives.</param>
        /// <param name="deleteCount">
        /// Nombre d'éléments à supprimer à partir de <paramref name="start"/>.
        /// Si <c>null</c>, supprime jusqu'à la fin de la collection.
        /// </param>
        /// <param name="items">Éléments à insérer à la place des éléments supprimés.</param>
        /// <returns>Un nouveau tableau contenant les éléments supprimés.</returns>
        IRArray<T> Splice(int start, int? deleteCount = null, params T[] items);

        /// <summary>
        /// Remplace tous les éléments d'une plage par <paramref name="value"/> (mutation en place).
        /// </summary>
        /// <remarks>
        /// <para>Les indices négatifs sont supportés pour <paramref name="start"/> et <paramref name="end"/>.</para>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/fill"><c>Array.prototype.fill()</c></see>.
        /// </remarks>
        /// <param name="value">Valeur de remplacement.</param>
        /// <param name="start">Index de début (inclus). Défaut : <c>0</c>.</param>
        /// <param name="end">
        /// Index de fin (exclus). Défaut : fin du tableau (<c>null</c> = <see cref="Length"/>).
        /// </param>
        /// <returns>La collection modifiée.</returns>
        IRArray<T> Fill(T value, int start = 0, int? end = null);

        /// <summary>
        /// Copie une portion du tableau vers la position <paramref name="target"/> (mutation en place).
        /// </summary>
        /// <remarks>
        /// <para>Les indices négatifs sont supportés.</para>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/copyWithin"><c>Array.prototype.copyWithin()</c></see>.
        /// </remarks>
        /// <param name="target">Index de destination de la copie.</param>
        /// <param name="start">Index source de début (inclus). Défaut : <c>0</c>.</param>
        /// <param name="end">Index source de fin (exclus). Défaut : fin du tableau.</param>
        /// <returns>La collection modifiée.</returns>
        IRArray<T> CopyWithin(int target, int start = 0, int? end = null);

        /// <summary>
        /// Trie la collection en place selon <paramref name="compareFn"/>.
        /// </summary>
        /// <remarks>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/sort"><c>Array.prototype.sort()</c></see>.
        /// Voir aussi <see cref="ToSorted"/> pour une variante sans mutation.
        /// </remarks>
        /// <param name="compareFn">
        /// Fonction de comparaison. Si <c>null</c>, l'ordre naturel du type <typeparamref name="T"/> est utilisé.
        /// </param>
        /// <returns>La collection modifiée.</returns>
        IRArray<T> Sort(Comparison<T>? compareFn = null);

        /// <summary>
        /// Inverse l'ordre des éléments de la collection en place.
        /// </summary>
        /// <remarks>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/reverse"><c>Array.prototype.reverse()</c></see>.
        /// Voir aussi <see cref="ToReversed"/> pour une variante sans mutation.
        /// </remarks>
        /// <returns>La collection modifiée.</returns>
        IRArray<T> Reverse();

        /// <summary>
        /// Retourne l'index de la première occurrence de <paramref name="value"/>, ou <c>-1</c> si absent.
        /// </summary>
        /// <remarks>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/indexOf"><c>Array.prototype.indexOf()</c></see>.
        /// </remarks>
        /// <param name="value">Valeur recherchée.</param>
        /// <returns>Index de la première occurrence, ou <c>-1</c>.</returns>
        int IndexOf(T value);

        /// <summary>
        /// Retourne l'index de la dernière occurrence de <paramref name="value"/>, ou <c>-1</c> si absent.
        /// </summary>
        /// <remarks>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/lastIndexOf"><c>Array.prototype.lastIndexOf()</c></see>.
        /// </remarks>
        /// <param name="value">Valeur recherchée.</param>
        /// <returns>Index de la dernière occurrence, ou <c>-1</c>.</returns>
        int LastIndexOf(T value);

        /// <summary>
        /// Retourne le premier élément satisfaisant le prédicat <paramref name="fn"/>.
        /// </summary>
        /// <remarks>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/find"><c>Array.prototype.find()</c></see>.
        /// </remarks>
        /// <param name="fn">Prédicat de recherche.</param>
        /// <returns>Le premier élément correspondant, ou <c>null</c> si aucun n'est trouvé.</returns>
        T? Find(Func<T, bool> fn);

        /// <summary>
        /// Retourne l'index du premier élément satisfaisant le prédicat <paramref name="fn"/>.
        /// </summary>
        /// <remarks>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/findIndex"><c>Array.prototype.findIndex()</c></see>.
        /// </remarks>
        /// <param name="fn">Prédicat de recherche.</param>
        /// <returns>Index du premier élément correspondant, ou <c>-1</c>.</returns>
        int FindIndex(Func<T, bool> fn);

        /// <summary>
        /// Retourne le dernier élément satisfaisant le prédicat <paramref name="fn"/>.
        /// </summary>
        /// <remarks>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/findLast"><c>Array.prototype.findLast()</c></see>.
        /// </remarks>
        /// <param name="fn">Prédicat de recherche.</param>
        /// <returns>Le dernier élément correspondant, ou <c>null</c> si aucun n'est trouvé.</returns>
        T? FindLast(Func<T, bool> fn);

        /// <summary>
        /// Retourne l'index du dernier élément satisfaisant le prédicat <paramref name="fn"/>.
        /// </summary>
        /// <remarks>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/findLastIndex"><c>Array.prototype.findLastIndex()</c></see>.
        /// </remarks>
        /// <param name="fn">Prédicat de recherche.</param>
        /// <returns>Index du dernier élément correspondant, ou <c>-1</c>.</returns>
        int FindLastIndex(Func<T, bool> fn);

        /// <summary>
        /// Indique si la collection contient <paramref name="value"/>.
        /// </summary>
        /// <remarks>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/includes"><c>Array.prototype.includes()</c></see>.
        /// </remarks>
        /// <param name="value">Valeur recherchée.</param>
        /// <returns><c>true</c> si <paramref name="value"/> est présent ; sinon <c>false</c>.</returns>
        bool Includes(T value);

        /// <summary>
        /// Indique si tous les éléments satisfont le prédicat <paramref name="fn"/>.
        /// </summary>
        /// <remarks>
        /// Retourne <c>true</c> sur une collection vide (vacuous truth).
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/every"><c>Array.prototype.every()</c></see>.
        /// </remarks>
        /// <param name="fn">Prédicat à tester sur chaque élément.</param>
        /// <returns><c>true</c> si tous les éléments satisfont <paramref name="fn"/> ; sinon <c>false</c>.</returns>
        bool Every(Func<T, bool> fn);

        /// <summary>
        /// Indique si au moins un élément satisfait le prédicat <paramref name="fn"/>.
        /// </summary>
        /// <remarks>
        /// Retourne <c>false</c> sur une collection vide.
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/some"><c>Array.prototype.some()</c></see>.
        /// </remarks>
        /// <param name="fn">Prédicat à tester sur chaque élément.</param>
        /// <returns><c>true</c> si au moins un élément satisfait <paramref name="fn"/> ; sinon <c>false</c>.</returns>
        bool Some(Func<T, bool> fn);

        /// <summary>
        /// Retourne une nouvelle collection contenant uniquement les éléments satisfaisant <paramref name="fn"/>.
        /// </summary>
        /// <remarks>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/filter"><c>Array.prototype.filter()</c></see>.
        /// </remarks>
        /// <param name="fn">Prédicat de filtrage.</param>
        /// <returns>Nouvelle collection des éléments correspondants.</returns>
        IRArray<T> Filter(Func<T, bool> fn);

        /// <summary>
        /// Retourne une nouvelle collection en appliquant <paramref name="fn"/> à chaque élément.
        /// </summary>
        /// <remarks>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/map"><c>Array.prototype.map()</c></see>.
        /// </remarks>
        /// <typeparam name="TResult">Type des éléments produits par la transformation.</typeparam>
        /// <param name="fn">Fonction de transformation appliquée à chaque élément.</param>
        /// <returns>Nouvelle collection des éléments transformés.</returns>
        IRArray<TResult> Map<TResult>(Func<T, TResult> fn);

        /// <summary>
        /// Applique <paramref name="fn"/> à chaque élément et aplatit les résultats d'un niveau.
        /// </summary>
        /// <remarks>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/flatMap"><c>Array.prototype.flatMap()</c></see>.
        /// </remarks>
        /// <typeparam name="TResult">Type des éléments produits après aplatissement.</typeparam>
        /// <param name="fn">Fonction retournant un <see cref="IEnumerable{T}"/> pour chaque élément.</param>
        /// <returns>Nouvelle collection aplatie d'un niveau.</returns>
        IRArray<TResult> FlatMap<TResult>(Func<T, IEnumerable<TResult>> fn);

        /// <summary>
        /// Réduit la collection à une valeur unique en appliquant <paramref name="fn"/> de gauche à droite.
        /// </summary>
        /// <remarks>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/reduce"><c>Array.prototype.reduce()</c></see>.
        /// </remarks>
        /// <typeparam name="TResult">Type de la valeur accumulée.</typeparam>
        /// <param name="fn">
        /// Fonction accumulatrice. Reçoit l'accumulateur courant et l'élément courant, retourne le nouvel accumulateur.
        /// </param>
        /// <param name="initialValue">Valeur initiale de l'accumulateur.</param>
        /// <returns>La valeur finale de l'accumulateur.</returns>
        TResult Reduce<TResult>(Func<TResult, T, TResult> fn, TResult initialValue);

        /// <summary>
        /// Réduit la collection à une valeur unique en appliquant <paramref name="fn"/> de droite à gauche.
        /// </summary>
        /// <remarks>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/reduceRight"><c>Array.prototype.reduceRight()</c></see>.
        /// </remarks>
        /// <typeparam name="TResult">Type de la valeur accumulée.</typeparam>
        /// <param name="fn">
        /// Fonction accumulatrice. Reçoit l'accumulateur courant et l'élément courant, retourne le nouvel accumulateur.
        /// </param>
        /// <param name="initialValue">Valeur initiale de l'accumulateur.</param>
        /// <returns>La valeur finale de l'accumulateur.</returns>
        TResult ReduceRight<TResult>(Func<TResult, T, TResult> fn, TResult initialValue);

        /// <summary>
        /// Retourne une nouvelle collection contenant les éléments de la collection courante
        /// suivis des éléments de chaque <paramref name="others"/>.
        /// </summary>
        /// <remarks>
        /// N'altère pas la collection courante.
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/concat"><c>Array.prototype.concat()</c></see>.
        /// </remarks>
        /// <param name="others">Collections à concaténer à la suite.</param>
        /// <returns>Nouvelle collection résultant de la concaténation.</returns>
        IRArray<T> Concat(params IEnumerable<T>[] others);

        /// <summary>
        /// Retourne une copie superficielle d'une portion de la collection.
        /// </summary>
        /// <remarks>
        /// <para>Les indices négatifs sont supportés.</para>
        /// N'altère pas la collection courante.
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/slice"><c>Array.prototype.slice()</c></see>.
        /// </remarks>
        /// <param name="start">Index de début (inclus). Défaut : <c>0</c>.</param>
        /// <param name="end">
        /// Index de fin (exclus). Défaut : fin du tableau (<c>null</c> = <see cref="Length"/>).
        /// </param>
        /// <returns>Nouvelle collection contenant les éléments de la plage spécifiée.</returns>
        IRArray<T> Slice(int start = 0, int? end = null);

        /// <summary>
        /// Retourne une copie de la collection avec les éléments dans l'ordre inverse.
        /// </summary>
        /// <remarks>
        /// N'altère pas la collection courante, contrairement à <see cref="Reverse"/>.
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/toReversed"><c>Array.prototype.toReversed()</c></see>.
        /// </remarks>
        /// <returns>Nouvelle collection dans l'ordre inverse.</returns>
        IRArray<T> ToReversed();

        /// <summary>
        /// Retourne une copie triée de la collection.
        /// </summary>
        /// <remarks>
        /// N'altère pas la collection courante, contrairement à <see cref="Sort"/>.
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/toSorted"><c>Array.prototype.toSorted()</c></see>.
        /// </remarks>
        /// <param name="compareFn">
        /// Fonction de comparaison. Si <c>null</c>, l'ordre naturel du type <typeparamref name="T"/> est utilisé.
        /// </param>
        /// <returns>Nouvelle collection triée.</returns>
        IRArray<T> ToSorted(Comparison<T>? compareFn = null);

        /// <summary>
        /// Retourne une copie de la collection après suppression et/ou insertion d'éléments.
        /// </summary>
        /// <remarks>
        /// N'altère pas la collection courante, contrairement à <see cref="Splice"/>.
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/toSpliced"><c>Array.prototype.toSpliced()</c></see>.
        /// </remarks>
        /// <param name="start">Index de départ de la modification. Supporte les valeurs négatives.</param>
        /// <param name="deleteCount">
        /// Nombre d'éléments à supprimer. Si <c>null</c>, supprime jusqu'à la fin.
        /// </param>
        /// <param name="items">Éléments à insérer à la place des éléments supprimés.</param>
        /// <returns>Nouvelle collection résultant de l'opération.</returns>
        IRArray<T> ToSpliced(int start, int? deleteCount = null, params T[] items);

        /// <summary>
        /// Retourne une copie de la collection avec l'élément à <paramref name="index"/> remplacé par <paramref name="value"/>.
        /// </summary>
        /// <remarks>
        /// N'altère pas la collection courante.
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/with"><c>Array.prototype.with()</c></see>.
        /// </remarks>
        /// <param name="index">Index de l'élément à remplacer. Supporte les valeurs négatives.</param>
        /// <param name="value">Nouvelle valeur à placer à cet index.</param>
        /// <returns>Nouvelle collection avec la substitution appliquée.</returns>
        IRArray<T> With(int index, T value);

        /// <summary>
        /// Exécute <paramref name="fn"/> pour chaque élément de la collection.
        /// </summary>
        /// <remarks>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/forEach"><c>Array.prototype.forEach()</c></see>.
        /// </remarks>
        /// <param name="fn">Action à exécuter pour chaque élément.</param>
        void ForEach(Action<T> fn);

        /// <summary>
        /// Retourne un itérateur sur les paires <c>(index, valeur)</c> de la collection.
        /// </summary>
        /// <remarks>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/entries"><c>Array.prototype.entries()</c></see>.
        /// </remarks>
        /// <returns>Séquence de tuples <c>(Index, Value)</c>.</returns>
        IEnumerable<(int Index, T Value)> Entries();

        /// <summary>
        /// Retourne un itérateur sur les indices de la collection.
        /// </summary>
        /// <remarks>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/keys"><c>Array.prototype.keys()</c></see>.
        /// </remarks>
        /// <returns>Séquence des indices entiers de la collection.</returns>
        IEnumerable<int> Keys();

        /// <summary>
        /// Retourne un itérateur sur les valeurs de la collection.
        /// </summary>
        /// <remarks>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/values"><c>Array.prototype.values()</c></see>.
        /// </remarks>
        /// <returns>Séquence des valeurs de la collection.</returns>
        IEnumerable<T> Values();

        /// <summary>
        /// Concatène tous les éléments en une chaîne de caractères, séparés par <paramref name="separator"/>.
        /// </summary>
        /// <remarks>
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/join"><c>Array.prototype.join()</c></see>.
        /// </remarks>
        /// <param name="separator">Séparateur inséré entre chaque élément. Défaut : <c>","</c>.</param>
        /// <returns>Chaîne résultant de la concaténation.</returns>
        string Join(string separator = ",");

        /// <summary>
        /// Retourne un tableau natif <c>T[]</c> contenant tous les éléments de la collection.
        /// </summary>
        /// <returns>Tableau <c>T[]</c> des éléments.</returns>
        T[] ToArray();
    }

#if NET7_0_OR_GREATER
    /// <summary>
    /// Contrat des méthodes statiques de fabrique, inspiré de
    /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/from"><c>Array.from()</c></see>
    /// et
    /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/of"><c>Array.of()</c></see>
    /// en JavaScript.
    /// </summary>
    /// <remarks>
    /// Utilise le pattern CRTP (Curiously Recurring Template Pattern) via <typeparamref name="TSelf"/>
    /// afin de permettre au compilateur de résoudre les membres statiques abstraits à la compilation.
    /// <example>
    /// <code>
    /// public class RArray&lt;T&gt; : IRArray&lt;T&gt;, IRArrayFactory&lt;T, RArray&lt;T&gt;&gt;
    /// {
    ///     public static RArray&lt;T&gt; From(IEnumerable&lt;T&gt; source) => new([.. source]);
    ///     public static RArray&lt;T&gt; Of(params T[] items) => new(items);
    ///     public static async Task&lt;RArray&lt;T&gt;&gt; FromAsync(IAsyncEnumerable&lt;T&gt; source)
    ///     {
    ///         var result = new RArray&lt;T&gt;();
    ///         await foreach (var item in source) result.Push(item);
    ///         return result;
    ///     }
    /// }
    /// </code>
    /// </example>
    /// </remarks>
    /// <typeparam name="T">Le type des éléments.</typeparam>
    /// <typeparam name="TSelf">
    /// Le type concret qui implémente cette interface.
    /// Doit lui-même implémenter <see cref="IRArray{T}"/>.
    /// </typeparam>
    public interface IRArrayFactory<T, TSelf> where TSelf : IRArray<T>
    {
        /// <summary>
        /// Crée une instance à partir d'un <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="source">Séquence source.</param>
        /// <returns>Nouvelle instance de <typeparamref name="TSelf"/> contenant les éléments de <paramref name="source"/>.</returns>
        static abstract TSelf From(IEnumerable<T> source);

        /// <summary>
        /// Crée une instance à partir des éléments passés en argument.
        /// </summary>
        /// <param name="items">Éléments à inclure dans la collection.</param>
        /// <returns>Nouvelle instance de <typeparamref name="TSelf"/>.</returns>
        static abstract TSelf Of(params T[] items);

        /// <summary>
        /// Crée une instance à partir d'un <see cref="IAsyncEnumerable{T}"/> en consommant la séquence séquentiellement.
        /// </summary>
        /// <remarks>
        /// Les éléments sont collectés dans l'ordre d'énumération avant que la tâche ne se complète.
        /// Équivalent de
        /// <see href="https://developer.mozilla.org/fr/docs/Web/JavaScript/Reference/Global_Objects/Array/fromAsync"><c>Array.fromAsync()</c></see>
        /// (ES2024).
        /// </remarks>
        /// <param name="source">Séquence asynchrone source.</param>
        /// <returns>
        /// Tâche qui se complète avec une nouvelle instance de <typeparamref name="TSelf"/>
        /// contenant tous les éléments énumérés.
        /// </returns>
        static abstract Task<TSelf> FromAsync(IAsyncEnumerable<T> source);
    }
#endif
}