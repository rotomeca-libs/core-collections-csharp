namespace Rotomeca.Core.Collections
{
    /// <summary>
    /// Classe statique compagnon de <see cref="RArray{T}"/>, exposant des méthodes utilitaires
    /// et des extensions pour interagir avec les collections Rotomeca.
    /// </summary>
    public static class RArray
    {
        /// <summary>
        /// Convertit une séquence en <see cref="RArray{T}"/>.
        /// </summary>
        /// <typeparam name="T">Le type des éléments.</typeparam>
        /// <param name="values">Séquence source à convertir.</param>
        /// <returns>Nouvelle instance de <see cref="RArray{T}"/> contenant les éléments de <paramref name="values"/>.</returns>
        public static RArray<T> ToRArray<T>(this IEnumerable<T> values) => new(values);
    }
}

