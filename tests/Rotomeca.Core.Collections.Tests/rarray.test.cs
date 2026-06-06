using Rotomeca.Core.Collections;
using Xunit;

namespace Rotomeca.Core.Collections.Tests
{
    // ═══════════════════════════════════════════════════════════════
    // Constructeurs
    // ═══════════════════════════════════════════════════════════════
    public class RArray_Constructor_Tests
    {
        [Fact]
        public void Default_ctor_creates_empty_collection()
        {
            var arr = new RArray<int>();
            Assert.Equal(0, arr.Length);
        }

        [Fact]
        public void Params_ctor_stores_elements_in_order()
        {
            var arr = new RArray<int>(1, 2, 3);

            Assert.Equal(3, arr.Length);
            Assert.Equal(1, arr[0]);
            Assert.Equal(2, arr[1]);
            Assert.Equal(3, arr[2]);
        }

        [Fact]
        public void IEnumerable_ctor_stores_elements_in_order()
        {
            var source = new List<string> { "a", "b", "c" };
            var arr = new RArray<string>(source);

            Assert.Equal(3, arr.Length);
            Assert.Equal("a", arr[0]);
            Assert.Equal("b", arr[1]);
            Assert.Equal("c", arr[2]);
        }

        [Fact]
        public void IEnumerable_ctor_is_independent_of_source()
        {
            var source = new List<int> { 1, 2, 3 };
            var arr = new RArray<int>(source);
            source.Add(4);

            Assert.Equal(3, arr.Length);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // At / Indexeur
    // ═══════════════════════════════════════════════════════════════
    public class RArray_At_Tests
    {
        [Theory]
        [InlineData(0, 10)]
        [InlineData(1, 20)]
        [InlineData(2, 30)]
        public void At_positive_returns_correct_element(int index, int expected)
        {
            var arr = new RArray<int>(10, 20, 30);
            Assert.Equal(expected, arr.At(index));
        }

        [Theory]
        [InlineData(-1, 30)]
        [InlineData(-2, 20)]
        [InlineData(-3, 10)]
        public void At_negative_counts_from_end(int index, int expected)
        {
            var arr = new RArray<int>(10, 20, 30);
            Assert.Equal(expected, arr.At(index));
        }

        [Theory]
        [InlineData(5)]
        [InlineData(-5)]
        public void At_out_of_range_throws(int index)
        {
            var arr = new RArray<int>(1, 2, 3);
            Assert.Throws<IndexOutOfRangeException>(() => arr.At(index));
        }

        [Fact]
        public void Indexer_get_delegates_to_At_positive()
        {
            var arr = new RArray<int>(10, 20, 30);
            Assert.Equal(arr.At(1), arr[1]);
        }

        [Fact]
        public void Indexer_get_delegates_to_At_negative()
        {
            var arr = new RArray<int>(10, 20, 30);
            Assert.Equal(arr.At(-1), arr[-1]);
        }

        [Fact]
        public void Indexer_set_positive_index_replaces_element()
        {
            var arr = new RArray<int>(1, 2, 3);
            arr[1] = 99;
            Assert.Equal(99, arr[1]);
        }

        [Fact]
        public void Indexer_set_negative_index_replaces_element()
        {
            var arr = new RArray<int>(1, 2, 3);
            arr[-1] = 99;
            Assert.Equal(99, arr[2]);
        }

        [Fact]
        public void Indexer_set_out_of_range_throws()
        {
            var arr = new RArray<int>(1, 2, 3);
            Assert.Throws<IndexOutOfRangeException>(() => arr[-10] = 0);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // Push / Pop
    // ═══════════════════════════════════════════════════════════════
    public class RArray_PushPop_Tests
    {
        [Fact]
        public void Push_single_item_returns_new_length()
        {
            var arr = new RArray<int>();
            var newLen = arr.Push(42);

            Assert.Equal(1, newLen);
            Assert.Equal(42, arr[0]);
        }

        [Fact]
        public void Push_multiple_items_appends_in_order_and_returns_length()
        {
            var arr = new RArray<int>(1);
            var newLen = arr.Push(2, 3, 4);

            Assert.Equal(4, newLen);
            Assert.Equal(new[] { 1, 2, 3, 4 }, arr.ToArray());
        }

        [Fact]
        public void Push_IEnumerable_overload_appends_all()
        {
            var arr = new RArray<int>(1);
            arr.Push(new List<int> { 2, 3 });

            Assert.Equal(3, arr.Length);
            Assert.Equal(new[] { 1, 2, 3 }, arr.ToArray());
        }

        [Fact]
        public void Pop_returns_last_element_and_reduces_length()
        {
            var arr = new RArray<int>(1, 2, 3);
            var popped = arr.Pop();

            Assert.Equal(3, popped);
            Assert.Equal(2, arr.Length);
            Assert.Equal(new[] { 1, 2 }, arr.ToArray());
        }

        [Fact]
        public void Pop_on_empty_returns_default()
        {
            var arr = new RArray<int>();
            Assert.Equal(default, arr.Pop());
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // Unshift / Shift
    // ═══════════════════════════════════════════════════════════════
    public class RArray_UnshiftShift_Tests
    {
        [Fact]
        public void Unshift_inserts_items_at_beginning_in_order()
        {
            var arr = new RArray<int>(3, 4);
            var newLen = arr.Unshift(1, 2);

            Assert.Equal(4, newLen);
            Assert.Equal(new[] { 1, 2, 3, 4 }, arr.ToArray());
        }

        [Fact]
        public void Shift_removes_and_returns_first_element()
        {
            var arr = new RArray<int>(10, 20, 30);
            var shifted = arr.Shift();

            Assert.Equal(10, shifted);
            Assert.Equal(2, arr.Length);
            Assert.Equal(20, arr[0]);
        }

        [Fact]
        public void Shift_on_empty_returns_default()
        {
            var arr = new RArray<string>();
            Assert.Null(arr.Shift());
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // Splice
    // ═══════════════════════════════════════════════════════════════
    public class RArray_Splice_Tests
    {
        [Fact]
        public void Splice_delete_only_removes_elements_and_returns_them()
        {
            var arr = new RArray<int>(1, 2, 3, 4, 5);
            var removed = arr.Splice(1, 2);

            Assert.Equal(new[] { 2, 3 }, removed.ToArray());
            Assert.Equal(new[] { 1, 4, 5 }, arr.ToArray());
        }

        [Fact]
        public void Splice_delete_count_zero_inserts_without_removing()
        {
            var arr = new RArray<int>(1, 2, 4, 5);
            var removed = arr.Splice(2, 0, 3);

            Assert.Empty(removed.ToArray());
            Assert.Equal(new[] { 1, 2, 3, 4, 5 }, arr.ToArray());
        }

        [Fact]
        public void Splice_delete_and_insert_replaces_range()
        {
            var arr = new RArray<int>(1, 2, 3, 4, 5);
            arr.Splice(1, 2, 10, 20);

            Assert.Equal(new[] { 1, 10, 20, 4, 5 }, arr.ToArray());
        }

        [Fact]
        public void Splice_null_deleteCount_removes_from_start_to_end()
        {
            var arr = new RArray<int>(1, 2, 3, 4, 5);
            var removed = arr.Splice(2);

            Assert.Equal(new[] { 3, 4, 5 }, removed.ToArray());
            Assert.Equal(new[] { 1, 2 }, arr.ToArray());
        }

        [Fact]
        public void Splice_negative_start_counts_from_end()
        {
            var arr = new RArray<int>(1, 2, 3, 4, 5);
            arr.Splice(-2, 1);

            Assert.Equal(new[] { 1, 2, 3, 5 }, arr.ToArray());
        }

        [Fact]
        public void Splice_start_beyond_length_appends_items()
        {
            var arr = new RArray<int>(1, 2, 3);
            arr.Splice(100, 0, 4, 5);

            Assert.Equal(new[] { 1, 2, 3, 4, 5 }, arr.ToArray());
        }

        [Fact]
        public void Splice_on_empty_returns_empty_removed()
        {
            var arr = new RArray<int>();
            var removed = arr.Splice(0, 5);

            Assert.Empty(removed.ToArray());
            Assert.Empty(arr.ToArray());
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // Fill
    // ═══════════════════════════════════════════════════════════════
    public class RArray_Fill_Tests
    {
        [Fact]
        public void Fill_entire_array_when_no_range_given()
        {
            var arr = new RArray<int>(1, 2, 3);
            arr.Fill(0);

            Assert.Equal(new[] { 0, 0, 0 }, arr.ToArray());
        }

        [Fact]
        public void Fill_partial_range_positive_indices()
        {
            var arr = new RArray<int>(1, 2, 3, 4, 5);
            arr.Fill(0, 1, 3);

            Assert.Equal(new[] { 1, 0, 0, 4, 5 }, arr.ToArray());
        }

        [Fact]
        public void Fill_with_negative_start()
        {
            var arr = new RArray<int>(1, 2, 3, 4, 5);
            arr.Fill(0, -3);

            Assert.Equal(new[] { 1, 2, 0, 0, 0 }, arr.ToArray());
        }

        [Fact]
        public void Fill_with_negative_start_and_end()
        {
            var arr = new RArray<int>(1, 2, 3, 4, 5);
            arr.Fill(0, -3, -1);

            Assert.Equal(new[] { 1, 2, 0, 0, 5 }, arr.ToArray());
        }

        [Fact]
        public void Fill_returns_this_for_chaining()
        {
            var arr = new RArray<int>(1, 2, 3);
            var result = arr.Fill(0);

            Assert.Same(arr, result);
        }

        [Fact]
        public void Fill_does_not_grow_collection()
        {
            var arr = new RArray<int>(1, 2, 3);
            arr.Fill(99, 0, 100);

            Assert.Equal(3, arr.Length);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // CopyWithin
    // ═══════════════════════════════════════════════════════════════
    public class RArray_CopyWithin_Tests
    {
        [Fact]
        public void CopyWithin_copies_tail_to_start()
        {
            // JS: [1,2,3,4,5].copyWithin(0, 3) => [4,5,3,4,5]
            var arr = new RArray<int>(1, 2, 3, 4, 5);
            arr.CopyWithin(0, 3);

            Assert.Equal(new[] { 4, 5, 3, 4, 5 }, arr.ToArray());
        }

        [Fact]
        public void CopyWithin_with_explicit_end()
        {
            // JS: [1,2,3,4,5].copyWithin(1, 3, 5) => [1,4,5,4,5]
            var arr = new RArray<int>(1, 2, 3, 4, 5);
            arr.CopyWithin(1, 3, 5);

            Assert.Equal(new[] { 1, 4, 5, 4, 5 }, arr.ToArray());
        }

        [Fact]
        public void CopyWithin_negative_target_counts_from_end()
        {
            // JS: [1,2,3,4,5].copyWithin(-2) => [1,2,3,1,2]
            var arr = new RArray<int>(1, 2, 3, 4, 5);
            arr.CopyWithin(-2);

            Assert.Equal(new[] { 1, 2, 3, 1, 2 }, arr.ToArray());
        }

        [Fact]
        public void CopyWithin_does_not_change_length()
        {
            var arr = new RArray<int>(1, 2, 3, 4, 5);
            arr.CopyWithin(0, 3);

            Assert.Equal(5, arr.Length);
        }

        [Fact]
        public void CopyWithin_returns_this_for_chaining()
        {
            var arr = new RArray<int>(1, 2, 3);
            var result = arr.CopyWithin(0, 1);

            Assert.Same(arr, result);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // Sort / Reverse
    // ═══════════════════════════════════════════════════════════════
    public class RArray_SortReverse_Tests
    {
        [Fact]
        public void Sort_default_uses_natural_order()
        {
            var arr = new RArray<int>(3, 1, 4, 1, 5, 9, 2);
            arr.Sort();

            Assert.Equal(new[] { 1, 1, 2, 3, 4, 5, 9 }, arr.ToArray());
        }

        [Fact]
        public void Sort_custom_compareFn_descending()
        {
            var arr = new RArray<int>(3, 1, 4, 1, 5);
            arr.Sort((a, b) => b - a);

            Assert.Equal(new[] { 5, 4, 3, 1, 1 }, arr.ToArray());
        }

        [Fact]
        public void Sort_mutates_in_place_and_returns_this()
        {
            var arr = new RArray<int>(3, 1, 2);
            var result = arr.Sort();

            Assert.Same(arr, result);
            Assert.Equal(new[] { 1, 2, 3 }, arr.ToArray());
        }

        [Fact]
        public void Reverse_reverses_in_place()
        {
            var arr = new RArray<int>(1, 2, 3, 4, 5);
            arr.Reverse();

            Assert.Equal(new[] { 5, 4, 3, 2, 1 }, arr.ToArray());
        }

        [Fact]
        public void Reverse_on_single_element_is_no_op()
        {
            var arr = new RArray<int>(42);
            arr.Reverse();

            Assert.Equal(new[] { 42 }, arr.ToArray());
        }

        [Fact]
        public void Reverse_returns_this_for_chaining()
        {
            var arr = new RArray<int>(1, 2, 3);
            var result = arr.Reverse();

            Assert.Same(arr, result);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // IndexOf / LastIndexOf / Includes
    // ═══════════════════════════════════════════════════════════════
    public class RArray_Search_Tests
    {
        [Fact]
        public void IndexOf_returns_first_occurrence()
        {
            var arr = new RArray<int>(1, 2, 3, 2, 1);
            Assert.Equal(1, arr.IndexOf(2));
        }

        [Fact]
        public void IndexOf_returns_minus_one_when_absent()
        {
            var arr = new RArray<int>(1, 2, 3);
            Assert.Equal(-1, arr.IndexOf(99));
        }

        [Fact]
        public void LastIndexOf_returns_last_occurrence()
        {
            var arr = new RArray<int>(1, 2, 3, 2, 1);
            Assert.Equal(3, arr.LastIndexOf(2));
        }

        [Fact]
        public void LastIndexOf_returns_minus_one_when_absent()
        {
            var arr = new RArray<int>(1, 2, 3);
            Assert.Equal(-1, arr.LastIndexOf(99));
        }

        [Fact]
        public void Includes_returns_true_when_value_is_present()
        {
            var arr = new RArray<string>("a", "b", "c");
            Assert.True(arr.Includes("b"));
        }

        [Fact]
        public void Includes_returns_false_when_value_is_absent()
        {
            var arr = new RArray<string>("a", "b", "c");
            Assert.False(arr.Includes("z"));
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // Find / FindIndex / FindLast / FindLastIndex
    // ═══════════════════════════════════════════════════════════════
    public class RArray_Find_Tests
    {
        [Fact]
        public void Find_returns_first_matching_element()
        {
            var arr = new RArray<int>(1, 2, 3, 4);
            Assert.Equal(2, arr.Find(x => x % 2 == 0));
        }

        [Fact]
        public void Find_returns_null_when_not_found_for_reference_type()
        {
            var arr = new RArray<string>("a", "b", "c");
            Assert.Null(arr.Find(x => x == "z"));
        }

        [Fact]
        public void FindIndex_returns_index_of_first_match()
        {
            var arr = new RArray<int>(1, 2, 3, 4);
            Assert.Equal(1, arr.FindIndex(x => x % 2 == 0));
        }

        [Fact]
        public void FindIndex_returns_minus_one_when_not_found()
        {
            var arr = new RArray<int>(1, 3, 5);
            Assert.Equal(-1, arr.FindIndex(x => x % 2 == 0));
        }

        [Fact]
        public void FindLast_returns_last_matching_element()
        {
            var arr = new RArray<int>(1, 2, 3, 4);
            Assert.Equal(4, arr.FindLast(x => x % 2 == 0));
        }

        [Fact]
        public void FindLast_returns_null_when_not_found_for_reference_type()
        {
            var arr = new RArray<string>("a", "b", "c");
            Assert.Null(arr.FindLast(x => x == "z"));
        }

        [Fact]
        public void FindLastIndex_returns_index_of_last_match()
        {
            var arr = new RArray<int>(1, 2, 3, 4);
            Assert.Equal(3, arr.FindLastIndex(x => x % 2 == 0));
        }

        [Fact]
        public void FindLastIndex_returns_minus_one_when_not_found()
        {
            var arr = new RArray<int>(1, 3, 5);
            Assert.Equal(-1, arr.FindLastIndex(x => x % 2 == 0));
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // Every / Some
    // ═══════════════════════════════════════════════════════════════
    public class RArray_Every_Some_Tests
    {
        [Fact]
        public void Every_returns_true_when_all_elements_match()
        {
            var arr = new RArray<int>(2, 4, 6);
            Assert.True(arr.Every(x => x % 2 == 0));
        }

        [Fact]
        public void Every_returns_false_when_any_element_does_not_match()
        {
            var arr = new RArray<int>(2, 3, 6);
            Assert.False(arr.Every(x => x % 2 == 0));
        }

        [Fact]
        public void Every_returns_true_on_empty_collection_vacuous_truth()
        {
            var arr = new RArray<int>();
            Assert.True(arr.Every(x => x % 2 == 0));
        }

        [Fact]
        public void Some_returns_true_when_at_least_one_matches()
        {
            var arr = new RArray<int>(1, 3, 4);
            Assert.True(arr.Some(x => x % 2 == 0));
        }

        [Fact]
        public void Some_returns_false_when_no_element_matches()
        {
            var arr = new RArray<int>(1, 3, 5);
            Assert.False(arr.Some(x => x % 2 == 0));
        }

        [Fact]
        public void Some_returns_false_on_empty_collection()
        {
            var arr = new RArray<int>();
            Assert.False(arr.Some(x => x > 0));
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // Filter / Map / FlatMap
    // ═══════════════════════════════════════════════════════════════
    public class RArray_Transform_Tests
    {
        [Fact]
        public void Filter_returns_new_collection_with_only_matching_elements()
        {
            var arr = new RArray<int>(1, 2, 3, 4, 5);
            var result = arr.Filter(x => x % 2 == 0);

            Assert.Equal(new[] { 2, 4 }, result.ToArray());
        }

        [Fact]
        public void Filter_does_not_mutate_original()
        {
            var arr = new RArray<int>(1, 2, 3, 4, 5);
            arr.Filter(x => x % 2 == 0);

            Assert.Equal(5, arr.Length);
        }

        [Fact]
        public void Filter_returns_empty_when_no_element_matches()
        {
            var arr = new RArray<int>(1, 3, 5);
            var result = arr.Filter(x => x % 2 == 0);

            Assert.Empty(result.ToArray());
        }

        [Fact]
        public void Map_applies_transformation_to_each_element()
        {
            var arr = new RArray<int>(1, 2, 3);
            var result = arr.Map(x => x * 2);

            Assert.Equal(new[] { 2, 4, 6 }, result.ToArray());
        }

        [Fact]
        public void Map_can_project_to_different_type()
        {
            var arr = new RArray<int>(1, 2, 3);
            var result = arr.Map(x => x.ToString());

            Assert.Equal(new[] { "1", "2", "3" }, result.ToArray());
        }

        [Fact]
        public void Map_does_not_mutate_original()
        {
            var arr = new RArray<int>(1, 2, 3);
            arr.Map(x => x * 100);

            Assert.Equal(new[] { 1, 2, 3 }, arr.ToArray());
        }

        [Fact]
        public void FlatMap_flattens_one_level_and_interleaves_results()
        {
            var arr = new RArray<int>(1, 2, 3);
            var result = arr.FlatMap(x => new[] { x, x * 10 });

            Assert.Equal(new[] { 1, 10, 2, 20, 3, 30 }, result.ToArray());
        }

        [Fact]
        public void FlatMap_with_empty_inner_sequences_returns_empty()
        {
            var arr = new RArray<int>(1, 2, 3);
            var result = arr.FlatMap<int>(_ => Array.Empty<int>());

            Assert.Empty(result.ToArray());
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // Reduce / ReduceRight
    // ═══════════════════════════════════════════════════════════════
    public class RArray_Reduce_Tests
    {
        [Fact]
        public void Reduce_accumulates_left_to_right()
        {
            var arr = new RArray<int>(1, 2, 3, 4, 5);
            var sum = arr.Reduce((acc, x) => acc + x, 0);

            Assert.Equal(15, sum);
        }

        [Fact]
        public void Reduce_builds_string_in_left_to_right_order()
        {
            var arr = new RArray<string>("a", "b", "c");
            var result = arr.Reduce((acc, x) => acc + x, "");

            Assert.Equal("abc", result);
        }

        [Fact]
        public void Reduce_with_initial_value_is_used_first()
        {
            var arr = new RArray<int>(1, 2, 3);
            var result = arr.Reduce((acc, x) => acc + x, 10);

            Assert.Equal(16, result);
        }

        [Fact]
        public void ReduceRight_accumulates_right_to_left()
        {
            var arr = new RArray<string>("a", "b", "c");
            var result = arr.ReduceRight((acc, x) => acc + x, "");

            Assert.Equal("cba", result);
        }

        [Fact]
        public void ReduceRight_arithmetic_is_right_associative()
        {
            // acc starts at 0
            // i=4: fn(0, 5) = 5-0 = 5
            // i=3: fn(5, 4) = 4-5 = -1
            // i=2: fn(-1, 3) = 3-(-1) = 4
            // i=1: fn(4, 2) = 2-4 = -2
            // i=0: fn(-2, 1) = 1-(-2) = 3
            var arr = new RArray<int>(1, 2, 3, 4, 5);
            var result = arr.ReduceRight((acc, x) => x - acc, 0);

            Assert.Equal(3, result);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // Concat / Slice
    // ═══════════════════════════════════════════════════════════════
    public class RArray_Concat_Slice_Tests
    {
        [Fact]
        public void Concat_single_sequence_appends_all_elements()
        {
            var arr = new RArray<int>(1, 2, 3);
            var result = arr.Concat(new[] { 4, 5, 6 });

            Assert.Equal(new[] { 1, 2, 3, 4, 5, 6 }, result.ToArray());
        }

        [Fact]
        public void Concat_multiple_sequences_in_order()
        {
            var arr = new RArray<int>(1);
            var result = arr.Concat(new[] { 2, 3 }, new[] { 4, 5 });

            Assert.Equal(new[] { 1, 2, 3, 4, 5 }, result.ToArray());
        }

        [Fact]
        public void Concat_does_not_mutate_original()
        {
            var arr = new RArray<int>(1, 2, 3);
            arr.Concat(new[] { 4, 5 });

            Assert.Equal(3, arr.Length);
        }

        [Fact]
        public void Concat_with_empty_sequence_returns_same_content()
        {
            var arr = new RArray<int>(1, 2, 3);
            var result = arr.Concat(Array.Empty<int>());

            Assert.Equal(new[] { 1, 2, 3 }, result.ToArray());
        }

        [Fact]
        public void Slice_no_args_returns_full_shallow_copy()
        {
            var arr = new RArray<int>(1, 2, 3, 4, 5);
            var result = arr.Slice();

            Assert.Equal(arr.ToArray(), result.ToArray());
            Assert.NotSame(arr, result);
        }

        [Fact]
        public void Slice_with_start_trims_head()
        {
            var arr = new RArray<int>(1, 2, 3, 4, 5);
            var result = arr.Slice(2);

            Assert.Equal(new[] { 3, 4, 5 }, result.ToArray());
        }

        [Fact]
        public void Slice_with_start_and_end_extracts_range()
        {
            var arr = new RArray<int>(1, 2, 3, 4, 5);
            var result = arr.Slice(1, 3);

            Assert.Equal(new[] { 2, 3 }, result.ToArray());
        }

        [Fact]
        public void Slice_negative_start_counts_from_end()
        {
            var arr = new RArray<int>(1, 2, 3, 4, 5);
            var result = arr.Slice(-3);

            Assert.Equal(new[] { 3, 4, 5 }, result.ToArray());
        }

        [Fact]
        public void Slice_negative_end_counts_from_end()
        {
            var arr = new RArray<int>(1, 2, 3, 4, 5);
            var result = arr.Slice(0, -2);

            Assert.Equal(new[] { 1, 2, 3 }, result.ToArray());
        }

        [Fact]
        public void Slice_does_not_mutate_original()
        {
            var arr = new RArray<int>(1, 2, 3, 4, 5);
            arr.Slice(1, 3);

            Assert.Equal(5, arr.Length);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // Copies immuables : ToReversed / ToSorted / ToSpliced / With
    // ═══════════════════════════════════════════════════════════════
    public class RArray_ImmutableCopy_Tests
    {
        [Fact]
        public void ToReversed_returns_reversed_copy()
        {
            var arr = new RArray<int>(1, 2, 3);
            var result = arr.ToReversed();

            Assert.Equal(new[] { 3, 2, 1 }, result.ToArray());
        }

        [Fact]
        public void ToReversed_does_not_mutate_original()
        {
            var arr = new RArray<int>(1, 2, 3);
            arr.ToReversed();

            Assert.Equal(new[] { 1, 2, 3 }, arr.ToArray());
        }

        [Fact]
        public void ToSorted_returns_sorted_copy_with_default_order()
        {
            var arr = new RArray<int>(3, 1, 2);
            var result = arr.ToSorted();

            Assert.Equal(new[] { 1, 2, 3 }, result.ToArray());
        }

        [Fact]
        public void ToSorted_does_not_mutate_original()
        {
            var arr = new RArray<int>(3, 1, 2);
            arr.ToSorted();

            Assert.Equal(new[] { 3, 1, 2 }, arr.ToArray());
        }

        [Fact]
        public void ToSorted_with_custom_compareFn()
        {
            var arr = new RArray<int>(1, 3, 2);
            var result = arr.ToSorted((a, b) => b - a);

            Assert.Equal(new[] { 3, 2, 1 }, result.ToArray());
        }

        [Fact]
        public void ToSpliced_returns_modified_copy()
        {
            var arr = new RArray<int>(1, 2, 3, 4, 5);
            var result = arr.ToSpliced(1, 2, 10, 20);

            Assert.Equal(new[] { 1, 10, 20, 4, 5 }, result.ToArray());
        }

        [Fact]
        public void ToSpliced_does_not_mutate_original()
        {
            var arr = new RArray<int>(1, 2, 3, 4, 5);
            arr.ToSpliced(1, 2, 10);

            Assert.Equal(new[] { 1, 2, 3, 4, 5 }, arr.ToArray());
        }

        [Fact]
        public void With_returns_copy_with_substituted_element_positive_index()
        {
            var arr = new RArray<int>(1, 2, 3);
            var result = arr.With(1, 99);

            Assert.Equal(new[] { 1, 99, 3 }, result.ToArray());
        }

        [Fact]
        public void With_supports_negative_index()
        {
            var arr = new RArray<int>(1, 2, 3);
            var result = arr.With(-1, 99);

            Assert.Equal(new[] { 1, 2, 99 }, result.ToArray());
        }

        [Fact]
        public void With_does_not_mutate_original()
        {
            var arr = new RArray<int>(1, 2, 3);
            arr.With(0, 99);

            Assert.Equal(new[] { 1, 2, 3 }, arr.ToArray());
        }

        [Theory]
        [InlineData(10)]
        [InlineData(-10)]
        public void With_out_of_range_throws(int index)
        {
            var arr = new RArray<int>(1, 2, 3);
            Assert.Throws<IndexOutOfRangeException>(() => arr.With(index, 99));
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // ForEach / Entries / Keys / Values / Join / ToArray / GetEnumerator
    // ═══════════════════════════════════════════════════════════════
    public class RArray_Iteration_Tests
    {
        [Fact]
        public void ForEach_visits_all_elements_in_order()
        {
            var arr = new RArray<int>(1, 2, 3);
            var visited = new List<int>();
            arr.ForEach(x => visited.Add(x));

            Assert.Equal(new[] { 1, 2, 3 }, visited);
        }

        [Fact]
        public void Entries_yields_index_value_pairs_in_order()
        {
            var arr = new RArray<string>("a", "b", "c");
            var entries = arr.Entries().ToList();

            Assert.Equal(3, entries.Count);
            Assert.Equal((0, "a"), entries[0]);
            Assert.Equal((1, "b"), entries[1]);
            Assert.Equal((2, "c"), entries[2]);
        }

        [Fact]
        public void Keys_yields_sequential_indices()
        {
            var arr = new RArray<string>("x", "y", "z");
            Assert.Equal(new[] { 0, 1, 2 }, arr.Keys().ToArray());
        }

        [Fact]
        public void Values_yields_all_values_in_order()
        {
            var arr = new RArray<int>(10, 20, 30);
            Assert.Equal(new[] { 10, 20, 30 }, arr.Values().ToArray());
        }

        [Fact]
        public void Values_returns_independent_copy()
        {
            var arr = new RArray<int>(1, 2, 3);
            var values = arr.Values();
            arr.Push(4);

            Assert.Equal(3, values.Count());
        }

        [Fact]
        public void Join_uses_comma_as_default_separator()
        {
            var arr = new RArray<int>(1, 2, 3);
            Assert.Equal("1,2,3", arr.Join());
        }

        [Fact]
        public void Join_uses_custom_separator()
        {
            var arr = new RArray<string>("a", "b", "c");
            Assert.Equal("a - b - c", arr.Join(" - "));
        }

        [Fact]
        public void Join_on_empty_array_returns_empty_string()
        {
            var arr = new RArray<int>();
            Assert.Equal(string.Empty, arr.Join());
        }

        [Fact]
        public void ToArray_returns_native_array_with_all_elements()
        {
            var arr = new RArray<int>(1, 2, 3);
            var native = arr.ToArray();

            Assert.IsType<int[]>(native);
            Assert.Equal(new[] { 1, 2, 3 }, native);
        }

        [Fact]
        public void GetEnumerator_supports_foreach_loop()
        {
            var arr = new RArray<int>(1, 2, 3);
            var collected = new List<int>();
            foreach (var item in arr)
                collected.Add(item);

            Assert.Equal(new[] { 1, 2, 3 }, collected);
        }

        [Fact]
        public void GetEnumerator_nongeneric_is_consistent_with_generic()
        {
            var arr = new RArray<int>(1, 2, 3);
            var collected = new List<object?>();
            var enumerator = ((System.Collections.IEnumerable)arr).GetEnumerator();
            while (enumerator.MoveNext())
                collected.Add(enumerator.Current);

            Assert.Equal(new object[] { 1, 2, 3 }, collected);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // Méthodes statiques : From / Of / FromAsync
    // ═══════════════════════════════════════════════════════════════
    public class RArray_Static_Tests
    {
        [Fact]
        public void From_creates_instance_from_enumerable()
        {
            var source = new List<int> { 1, 2, 3 };
            var arr = RArray<int>.From(source);

            Assert.IsType<RArray<int>>(arr);
            Assert.Equal(new[] { 1, 2, 3 }, arr.ToArray());
        }

        [Fact]
        public void Of_creates_instance_from_params()
        {
            var arr = RArray<string>.Of("a", "b", "c");

            Assert.IsType<RArray<string>>(arr);
            Assert.Equal(new[] { "a", "b", "c" }, arr.ToArray());
        }

        [Fact]
        public void Of_with_no_args_creates_empty_instance()
        {
            var arr = RArray<int>.Of();
            Assert.Equal(0, arr.Length);
        }

#if !NETSTANDARD2_0
        [Fact]
        public async Task FromAsync_collects_elements_from_async_sequence_in_order()
        {
            async IAsyncEnumerable<int> AsyncSeq()
            {
                yield return 1;
                yield return 2;
                yield return 3;
            }

            var arr = await RArray<int>.FromAsync(AsyncSeq());

            Assert.Equal(new[] { 1, 2, 3 }, arr.ToArray());
        }

        [Fact]
        public async Task FromAsync_on_empty_sequence_creates_empty_instance()
        {
            async IAsyncEnumerable<int> Empty() { yield break; }

            var arr = await RArray<int>.FromAsync(Empty());

            Assert.Equal(0, arr.Length);
        }
#else
        [Fact]
        public async Task FromAsync_collects_results_from_task_enumerable()
        {
            var tasks = new[] { Task.FromResult(1), Task.FromResult(2), Task.FromResult(3) };
            var arr = await RArray<int>.FromAsync(tasks);

            Assert.Equal(new[] { 1, 2, 3 }, arr.ToArray());
        }
#endif
    }

    // ═══════════════════════════════════════════════════════════════
    // Extension ToRArray (classe compagnon statique RArray)
    // ═══════════════════════════════════════════════════════════════
    public class RArray_Extension_Tests
    {
        [Fact]
        public void ToRArray_converts_array_to_RArray()
        {
            var result = new[] { 1, 2, 3 }.ToRArray();

            Assert.IsType<RArray<int>>(result);
            Assert.Equal(new[] { 1, 2, 3 }, result.ToArray());
        }

        [Fact]
        public void ToRArray_converts_list_to_RArray()
        {
            var result = new List<string> { "x", "y" }.ToRArray();

            Assert.IsType<RArray<string>>(result);
            Assert.Equal(new[] { "x", "y" }, result.ToArray());
        }

        [Fact]
        public void ToRArray_on_empty_sequence_creates_empty_RArray()
        {
            var result = Enumerable.Empty<int>().ToRArray();

            Assert.Equal(0, result.Length);
        }

        [Fact]
        public void ToRArray_result_is_independent_of_source()
        {
            var source = new List<int> { 1, 2, 3 };
            var result = source.ToRArray();
            source.Add(4);

            Assert.Equal(3, result.Length);
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // Chaînage des méthodes mutantes
    // ═══════════════════════════════════════════════════════════════
    public class RArray_Chaining_Tests
    {
        [Fact]
        public void Sort_then_Reverse_produces_descending_order()
        {
            var arr = new RArray<int>(3, 1, 4, 1, 5);
            arr.Sort().Reverse();

            Assert.Equal(new[] { 5, 4, 3, 1, 1 }, arr.ToArray());
        }

        [Fact]
        public void Fill_then_Reverse_consistent()
        {
            var arr = new RArray<int>(1, 2, 3, 4, 5);
            arr.Fill(0, 2, 4).Reverse();

            // Fill(0, 2, 4) => [1, 2, 0, 0, 5], Reverse => [5, 0, 0, 2, 1]
            Assert.Equal(new[] { 5, 0, 0, 2, 1 }, arr.ToArray());
        }
    }
}