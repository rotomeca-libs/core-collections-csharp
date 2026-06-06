# Rotomeca.Core.Collections.Tests — Suite de tests

Ce dossier contient la suite de tests unitaires pour `Rotomeca.Core.Collections`.

---

## Stack

| Outil                          | Version  | Rôle                                   |
| ------------------------------ | -------- | -------------------------------------- |
| xUnit                          | 2.9.3    | Framework de tests                     |
| xunit.runner.visualstudio      | 3.1.4    | Intégration VS / `dotnet test`         |
| coverlet.collector             | 6.0.4    | Couverture de code                     |
| Microsoft.NET.Test.Sdk         | 17.14.1  | Infrastructure de test .NET            |
| Framework cible                | net10.0  | —                                      |

---

## Lancer les tests

```bash
# Depuis la racine du dépôt
dotnet test

# Avec couverture de code
dotnet test --collect:"XPlat Code Coverage"

# Filtrer par classe de test
dotnet test --filter "FullyQualifiedName~RArray_Transform_Tests"
```

---

## Organisation des tests

Les tests sont regroupés par fonctionnalité dans `rarray.test.cs`.
Chaque groupe correspond à une classe de test dédiée.

| Classe de test                    | Fonctionnalité couverte                                      |
| --------------------------------- | ------------------------------------------------------------ |
| `RArray_Constructor_Tests`        | Constructeurs (vide, `params`, `IEnumerable`, indépendance)  |
| `RArray_At_Tests`                 | `At()`, indexeur `[]`, indices négatifs, hors bornes         |
| `RArray_PushPop_Tests`            | `Push()`, `Pop()`, comportement sur collection vide          |
| `RArray_UnshiftShift_Tests`       | `Unshift()`, `Shift()`, comportement sur collection vide     |
| `RArray_Splice_Tests`             | `Splice()` — suppression, insertion, indices négatifs        |
| `RArray_Fill_Tests`               | `Fill()` — plage complète, partielle, indices négatifs       |
| `RArray_CopyWithin_Tests`         | `CopyWithin()` — copie interne, indices négatifs             |
| `RArray_SortReverse_Tests`        | `Sort()`, `Reverse()` — mutation en place                    |
| `RArray_Search_Tests`             | `IndexOf()`, `LastIndexOf()`, `Includes()`                   |
| `RArray_Find_Tests`               | `Find()`, `FindIndex()`, `FindLast()`, `FindLastIndex()`     |
| `RArray_Every_Some_Tests`         | `Every()`, `Some()` — vacuous truth, courts-circuits         |
| `RArray_Transform_Tests`          | `Filter()`, `Map()`, `FlatMap()`                             |
| `RArray_Reduce_Tests`             | `Reduce()`, `ReduceRight()`                                  |
| `RArray_Concat_Slice_Tests`       | `Concat()`, `Slice()` — non-mutation de l'original           |
| `RArray_ImmutableCopy_Tests`      | `ToReversed()`, `ToSorted()`, `ToSpliced()`, `With()`        |
| `RArray_Iteration_Tests`          | `ForEach()`, `Entries()`, `Keys()`, `Values()`, `Join()`     |
| `RArray_Static_Tests`             | `RArray<T>.From()`, `RArray<T>.Of()`, `RArray<T>.FromAsync()`|
| `RArray_Extension_Tests`          | Extension `ToRArray()`                                       |
| `RArray_Chaining_Tests`           | Chaînage de mutateurs                                        |

**Total : 19 classes — 121 tests (`[Fact]` + `[Theory]`)**

---

## Conventions

- Nommage des méthodes : `{Méthode}_{Contexte}_{ComportementAttendu}`
  - ex. : `At_negative_counts_from_end`, `Pop_on_empty_returns_null`
- Un test = un comportement atomique
- `[Theory]` + `[InlineData]` pour couvrir plusieurs valeurs sans répétition
- Aucune dépendance externe autre que xUnit et la librairie testée

---

## Exemple de test

```csharp
// [Theory] : plusieurs cas en un seul test
[Theory]
[InlineData(-1, 30)]
[InlineData(-2, 20)]
[InlineData(-3, 10)]
public void At_negative_counts_from_end(int index, int expected)
{
    var arr = new RArray<int>(10, 20, 30);
    Assert.Equal(expected, arr.At(index));
}

// [Fact] : comportement unitaire précis
[Fact]
public void Pop_on_empty_collection_returns_null()
{
    var arr = new RArray<int>();
    Assert.Null(arr.Pop());
}
```

---

## Couverture

La suite couvre l'intégralité de la surface publique de `RArray<T>` :

- tous les constructeurs
- tous les mutateurs, y compris les cas limites (collection vide, indices négatifs, hors bornes)
- toutes les méthodes de recherche et de transformation
- toutes les copies immuables
- les méthodes d'itération et de conversion
- les membres statiques de fabrique (NET7+)
- le chaînage de mutateurs