# Rotomeca.Core.Collections — Librairie

Ce dossier contient le code source de la librairie `Rotomeca.Core.Collections`.

---

## Vue d'ensemble

`RArray<T>` est une collection C# inspirée de l'API `Array.prototype` JavaScript.
Elle regroupe mutateurs, transformations fonctionnelles et copies immuables dans une
interface unifiée et fluide, avec support des indices négatifs.

---

## Structure

```
Rotomeca.Core.Collections/
├── Rotomeca.Core.Collections.csproj
├── assets/
│   └── icon.png
├── classes/
│   ├── RArray{T}.cs          # Implémentation concrète de IRArray<T>
│   └── extensions/
│       └── RArray.cs         # Classe statique compagnon + extension ToRArray()
└── interfaces/
    └── IRArray{T}.cs         # IRArray<T>  +  IRArrayFactory<T,TSelf> (NET7+)
```

---

## Concepts clés

### Mutateurs vs copies immuables

Les méthodes mutantes modifient la collection en place et retournent `this` pour permettre le chaînage :

```csharp
arr.Push(4).Sort().Reverse(); // chaînage possible
```

Les méthodes préfixées par `To` retournent une nouvelle instance sans altérer l'original :

```csharp
var copy = arr.ToSorted(); // arr inchangé
```

### Indices négatifs

Toutes les méthodes qui acceptent un index supportent les valeurs négatives, comme en JavaScript :

```csharp
arr.At(-1)          // dernier élément
arr.Slice(-2)       // deux derniers éléments
arr.Splice(-1)      // supprime le dernier
arr.With(-1, 99)    // copie avec le dernier remplacé par 99
```

### Fabrique statique (NET7+)

Sur `net7.0` et supérieur, `RArray<T>` implémente `IRArrayFactory<T, RArray<T>>` via
le pattern CRTP et expose des membres statiques abstraits :

```csharp
var arr  = RArray<int>.From(myEnumerable);
var arr  = RArray<int>.Of(1, 2, 3);
var arr  = await RArray<int>.FromAsync(myAsyncEnumerable);
```

Sur `netstandard2.0` / `netstandard2.1`, utiliser les constructeurs ou l'extension `ToRArray()`.

---

## Frameworks ciblés

| Framework       | Notes                                       |
| --------------- | ------------------------------------------- |
| netstandard2.0  | Cible de compatibilité maximale             |
| netstandard2.1  | Nullable activé                             |
| net8.0          | LTS                                         |
| net9.0          | Current                                     |
| net10.0         | Ajouté automatiquement si SDK ≥ 10.0        |

---

## Configuration projet

| Option                      | Valeur          |
| --------------------------- | --------------- |
| `LangVersion`               | `latest`        |
| `Nullable`                  | `enable`        |
| `TreatWarningsAsErrors`     | `true`          |
| `GenerateDocumentationFile` | `true`          |
| `SymbolPackageFormat`       | `snupkg`        |
| Source Link                 | GitHub          |

---

## Résumé de l'API

### Mutateurs

| Méthode                          | Retour         | Équivalent JS            |
| -------------------------------- | -------------- | ------------------------ |
| `Push(params T[])`               | `int`          | `Array.push()`           |
| `Pop()`                          | `T?`           | `Array.pop()`            |
| `Unshift(params T[])`            | `int`          | `Array.unshift()`        |
| `Shift()`                        | `T?`           | `Array.shift()`          |
| `Splice(start, count?, items)` | `IRArray<T>`   | `Array.splice()`         |
| `Fill(value, start, end?)`       | `IRArray<T>`   | `Array.fill()`           |
| `CopyWithin(target, start, end?)`| `IRArray<T>`   | `Array.copyWithin()`     |
| `Sort(compareFn?)`               | `IRArray<T>`   | `Array.sort()`           |
| `Reverse()`                      | `IRArray<T>`   | `Array.reverse()`        |

### Recherche

| Méthode               | Retour  | Équivalent JS              |
| --------------------- | ------- | -------------------------- |
| `IndexOf(value)`      | `int`   | `Array.indexOf()`          |
| `LastIndexOf(value)`  | `int`   | `Array.lastIndexOf()`      |
| `Find(fn)`            | `T?`    | `Array.find()`             |
| `FindIndex(fn)`       | `int`   | `Array.findIndex()`        |
| `FindLast(fn)`        | `T?`    | `Array.findLast()`         |
| `FindLastIndex(fn)`   | `int`   | `Array.findLastIndex()`    |
| `Includes(value)`     | `bool`  | `Array.includes()`         |
| `Every(fn)`           | `bool`  | `Array.every()`            |
| `Some(fn)`            | `bool`  | `Array.some()`             |

### Transformations (nouvelles instances)

| Méthode                       | Retour             | Équivalent JS          |
| ----------------------------- | ------------------ | ---------------------- |
| `Filter(fn)`                  | `IRArray<T>`       | `Array.filter()`       |
| `Map<TResult>(fn)`            | `IRArray<TResult>` | `Array.map()`          |
| `FlatMap<TResult>(fn)`        | `IRArray<TResult>` | `Array.flatMap()`      |
| `Reduce<TResult>(fn, seed)`   | `TResult`          | `Array.reduce()`       |
| `ReduceRight<TResult>(fn, seed)` | `TResult`       | `Array.reduceRight()`  |
| `Concat(params IEnumerable[])` | `IRArray<T>`      | `Array.concat()`       |
| `Slice(start, end?)`          | `IRArray<T>`       | `Array.slice()`        |

### Copies immuables

| Méthode                           | Retour       | Équivalent JS            |
| --------------------------------- | ------------ | ------------------------ |
| `ToReversed()`                    | `IRArray<T>` | `Array.toReversed()`     |
| `ToSorted(compareFn?)`            | `IRArray<T>` | `Array.toSorted()`       |
| `ToSpliced(start, count?, items)` | `IRArray<T>` | `Array.toSpliced()`      |
| `With(index, value)`              | `IRArray<T>` | `Array.with()`           |

### Itération

| Méthode              | Retour                              | Équivalent JS          |
| -------------------- | ----------------------------------- | ---------------------- |
| `ForEach(fn)`        | `void`                              | `Array.forEach()`      |
| `Entries()`          | `IEnumerable<(int Index, T Value)>` | `Array.entries()`      |
| `Keys()`             | `IEnumerable<int>`                  | `Array.keys()`         |
| `Values()`           | `IEnumerable<T>`                    | `Array.values()`       |
| `Join(separator)`    | `string`                            | `Array.join()`         |
| `ToArray()`          | `T[]`                               | `Array.from(arr)`      |

---

## Dépendances

| Package                          | Version  | Rôle                     |
| -------------------------------- | -------- | ------------------------ |
| `Microsoft.SourceLink.GitHub`    | 8.0.0    | Navigation source (debug)|