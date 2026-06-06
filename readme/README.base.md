

## Installation

```bash
dotnet add package Rotomeca.Core.Collections
```

---

## Compatibilité

| Environnement | Support                                          |
| ------------- | ------------------------------------------------ |
| .NET Standard | 2.0, 2.1                                         |
| .NET          | 8.0, 9.0 (10.0 si SDK disponible)               |
| Source Link   | ✅ (navigation vers le code depuis le débogueur) |
| Symbols       | ✅ (`.snupkg`)                                   |
| Nullable      | ✅ (activé)                                      |

---

## Pourquoi `RArray<T>` ?

Les collections .NET (`List<T>`, `Array`, LINQ) ont des API fragmentées : les méthodes mutantes et les
projections n'ont pas de convention commune, le chaînage est impossible et l'API n'est pas unifiée entre types.

`RArray<T>` réunit tout en une seule surface calquée sur `Array.prototype` JavaScript :

```csharp
// ❌ C# classique — verbeux, pas de chaînage
var list = new List<int> { 3, 1, 4, 1, 5 };
list.Sort();
list.Reverse();
var result = list.Where(x => x > 2).Select(x => x * 10).ToList();

// ✅ RArray<T> — fluide, API JS familière
var result = new RArray<int>(3, 1, 4, 1, 5)
    .Sort()
    .Reverse()
    .Filter(x => x > 2)
    .Map(x => x * 10);
```

---

## Démarrage rapide

```csharp
using Rotomeca.Core.Collections;

// Construction
var arr = new RArray<int>(1, 2, 3);       // depuis des valeurs
var arr = new RArray<int>(myEnumerable);   // depuis une séquence
var arr = myList.ToRArray();               // extension sur IEnumerable<T>

// Accès — indices négatifs supportés comme en JS
int last  = arr.At(-1);   // → 3
int first = arr[0];       // → 1

// Mutateurs — retournent this pour le chaînage
arr.Push(4, 5);           // [1, 2, 3, 4, 5]
arr.Pop();                // → 5  |  arr = [1, 2, 3, 4]
arr.Unshift(0);           // [0, 1, 2, 3, 4]
arr.Shift();              // → 0  |  arr = [1, 2, 3, 4]

// Recherche
int idx   = arr.IndexOf(3);            // → 2
int idx   = arr.FindIndex(x => x > 2); // → 2
bool has  = arr.Includes(2);           // → true

// Transformations (retournent une nouvelle instance)
var doubled  = arr.Map(x => x * 2);
var evens    = arr.Filter(x => x % 2 == 0);
var sum      = arr.Reduce((acc, x) => acc + x, 0);
var flat     = arr.FlatMap(x => new[] { x, x * 10 });

// Copies immuables (n'altèrent pas l'original)
var reversed = arr.ToReversed();
var sorted   = arr.ToSorted((a, b) => b - a);
var sliced   = arr.Slice(1, 3);

// Itération
arr.ForEach(x => Console.WriteLine(x));
foreach (var (i, v) in arr.Entries()) { ... }

// Fabrique statique (NET7+)
var arr = RArray<int>.From(myEnumerable);
var arr = RArray<int>.Of(1, 2, 3);
var arr = await RArray<int>.FromAsync(myAsyncEnumerable);
```

---

## Référence de l'API publique

### Mutateurs — modifient en place

```csharp
.Push(params T[] items)       → int         // ajoute en fin, retourne la nouvelle taille
.Pop()                        → T?          // supprime et retourne le dernier élément
.Unshift(params T[] items)    → int         // insère en tête, retourne la nouvelle taille
.Shift()                      → T?          // supprime et retourne le premier élément
.Splice(start, deleteCount?, items) → IRArray<T>  // supprime / insère, retourne les supprimés
.Fill(value, start, end?)     → IRArray<T>  // remplit une plage
.CopyWithin(target, start, end?) → IRArray<T>
.Sort(compareFn?)             → IRArray<T>
.Reverse()                    → IRArray<T>
```

### Recherche

```csharp
.IndexOf(value)               → int
.LastIndexOf(value)           → int
.Find(fn)                     → T?
.FindIndex(fn)                → int
.FindLast(fn)                 → T?
.FindLastIndex(fn)            → int
.Includes(value)              → bool
.Every(fn)                    → bool
.Some(fn)                     → bool
```

### Transformations (nouvelles instances)

```csharp
.Filter(fn)                   → IRArray<T>
.Map<TResult>(fn)             → IRArray<TResult>
.FlatMap<TResult>(fn)         → IRArray<TResult>
.Reduce<TResult>(fn, seed)    → TResult
.ReduceRight<TResult>(fn, seed) → TResult
.Concat(params IEnumerable<T>[]) → IRArray<T>
.Slice(start, end?)           → IRArray<T>
```

### Copies immuables

```csharp
.ToReversed()                 → IRArray<T>
.ToSorted(compareFn?)         → IRArray<T>
.ToSpliced(start, deleteCount?, items) → IRArray<T>
.With(index, value)           → IRArray<T>
```

### Itération

```csharp
.ForEach(fn)
.Entries()                    → IEnumerable<(int Index, T Value)>
.Keys()                       → IEnumerable<int>
.Values()                     → IEnumerable<T>
.Join(separator = ",")        → string
.ToArray()                    → T[]
```

### Propriétés

```csharp
.Length                       // nombre d'éléments
this[int index]               // get/set avec indices négatifs
.At(int index)                // accès avec indices négatifs
```

### Extension

```csharp
myEnumerable.ToRArray<T>()    → RArray<T>
```

### Fabrique statique (NET7+)

```csharp
RArray<T>.From(IEnumerable<T>)
RArray<T>.Of(params T[])
RArray<T>.FromAsync(IAsyncEnumerable<T>)
```

---

## Interfaces

```
IRArray<T>                    // contrat instance complet
IRArrayFactory<T, TSelf>      // contrat fabrique statique (NET7+)
```

---

## Structure du dépôt

```
Rotomeca.Core.Collections/
├── src/
│   └── Rotomeca.Core.Collections/   # Librairie principale
│       ├── classes/
│       │   ├── RArray{T}.cs          # Implémentation concrète
│       │   └── extensions/
│       │       └── RArray.cs         # Extension ToRArray()
│       └── interfaces/
│           └── IRArray{T}.cs         # Contrats IRArray<T> et IRArrayFactory<T,TSelf>
└── tests/
    └── Rotomeca.Core.Collections.Tests/   # Suite de tests xUnit (121 tests)
```

---

## Ecosystème Rotomeca

> Voir la liste complète sur [rotomeca-libs](https://github.com/rotomeca-libs) et les packages C# sur [github](https://github.com/orgs/rotomeca-libs/repositories?q=csharp) ou sur [nuget](https://www.nuget.org/packages?q=Rotomeca&includeComputedFrameworks=true&prerel=true&sortby=relevance)

---

## Contribuer

```bash
git clone https://github.com/rotomeca-libs/core-collections-csharp.git
cd core-collections-csharp
dotnet restore
dotnet test
```

Les contributions sont les bienvenues via Pull Request sur la branche `dev`.

---

## Note sur l'utilisation de l'IA

L'intégralité du code de ce projet a d'abord été écrite à la main en essayant d'avoir le C# le plus propre possible. L'IA a ensuite été utilisée pour :

- **Proposer des axes d'amélioration et de refactorisation** si besoin, après relecture de ses modifications par mes soins
- **La documentation et les README** → j'ai toujours été une bille en documentation, je trouve celle de l'IA lisible et explicite ; elle a toujours été relue et validée par mes soins
- **Les tests unitaires** → tester, c'est facile, mais présenter des tests unitaires, c'est complexe (de mon point de vue) ; l'IA a dans un premier temps généré les tests, je les ai parcourus pour les comprendre et les corriger au besoin
- **La CI/CD** → vu que ce n'est pas mon domaine, mais ça permet d'apprendre beaucoup 👍

Sa principale contribution a donc été de m'accompagner sur les points qui me sont lacunaires.

---

## Licence

[ISC](LICENSE) © Rotomeca
