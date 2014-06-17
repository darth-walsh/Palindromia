using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Palindromia;

namespace PaliTest
{
	[TestClass]
	public class TrieTest
	{
		[TestMethod]
		public void TestEmpty() {
			var t = new Trie<string, char>(StringConcat.Instance);

			Assert.AreEqual(0, t.Count);
		}

		[TestMethod]
		public void TestAdd() {
			var t = new Trie<string, char>(StringConcat.Instance);

			Assert.IsTrue(t.Add("abc"));
			Assert.AreEqual(1, t.Count);

			Assert.IsFalse(t.Add("abc"));
			Assert.AreEqual(1, t.Count);

			Assert.IsTrue(t.Add("ab"));
			Assert.AreEqual(2, t.Count);

			Assert.IsTrue(t.Add("abcd"));
			Assert.AreEqual(3, t.Count);

			ICollection<string> cast = t;

			cast.Add("b");
			Assert.AreEqual(4, t.Count);
		}

		[TestMethod]
		public void TestRemove() {
			var t = new Trie<string, char>(StringConcat.Instance) { "a", "abc", "" };

			Assert.IsTrue(t.Remove("a"));
			Assert.AreEqual(2, t.Count);

			Assert.IsFalse(t.Remove("a"));
			Assert.AreEqual(2, t.Count);

			Assert.IsTrue(t.Remove("abc"));
			Assert.AreEqual(1, t.Count);

			Assert.IsTrue(t.Remove(""));
			Assert.AreEqual(0, t.Count);
		}

		[TestMethod]
		public void TestClear() {
			var t = new Trie<string, char>(StringConcat.Instance) { "a", "abc", "" };

			t.Clear();
			Assert.AreEqual(0, t.Count);

			t.Clear();
			Assert.AreEqual(0, t.Count);
		}

		[TestMethod]
		public void TestIncluded() {
			var t = new Trie<string, char>(StringConcat.Instance) { "a", "abc", "" };

			Assert.IsTrue(t.Contains(""));
			Assert.IsTrue(t.Contains("abc"));

			Assert.IsFalse(t.Contains("c"));
		}

		[TestMethod]
		public void TestReadonly() {
			var t = new Trie<string, char>(StringConcat.Instance) { "a", "abc", "" };

			Assert.IsFalse(t.IsReadOnly);
		}

		[TestMethod]
		public void TestEnumerate() {
			var t = new Trie<string, char>(StringConcat.Instance) { "a", "abc", "" };

			EnumerateEqual(new[] { "", "a", "abc" }, t);

			t.Add("ae");
			EnumerateEqual(new[] { "", "a", "ae", "abc" }, t);

			t.Remove("a");
			EnumerateEqual(new[] { "", "ae", "abc" }, t);
		}

		[TestMethod]
		public void TestList() {
			var t = new Trie<IList<int>, int>(ListConcat<int>.Instance) {
				new List<int> { 0, 2 },
				new List<int> { 0, 1 },
				new List<int> { },
			};

			Assert.AreEqual(3, t.Count);

			EnumerateEqual(
				new[] {
					new int[] { },
					new[] { 0, 1 },
					new[] { 0, 2 },
				},
				t,
				Enumerable.SequenceEqual);

			Assert.IsFalse(t.Add(new List<int> { 0, 1 }));
			Assert.IsTrue(t.Add(new List<int> { 1, 1 }));
			EnumerateEqual(
				new[] {
								new int[] { },
								new[] { 0, 1 },
								new[] { 0, 2 },
								new[] { 1, 1 },
							},
				t,
				Enumerable.SequenceEqual);

			Assert.IsTrue(t.Remove(new List<int> { 0, 1 }));
			Assert.IsFalse(t.Remove(new List<int> { 0, 1 }));
			Assert.IsFalse(t.Remove(new List<int> { 2, 0 }));
			EnumerateEqual(
				new[] {
								new int[] { },
								new[] { 0, 2 },
								new[] { 1, 1 },
							},
				t,
				Enumerable.SequenceEqual);
		}

		static void EnumerateEqual<T>(
				IEnumerable<T> expected,
				IEnumerable<T> actual, 
				Func<T, T, bool> comparer = null) {
			using (var eEn = expected.GetEnumerator())
			using (var aEn = actual.GetEnumerator()) {
				while (true) {
					var eMove = eEn.MoveNext();
					var aMove = aEn.MoveNext();

					Assert.AreEqual(eMove, aMove, "different lengths");
					if (!eMove)
						break;

					if (comparer == null)
						Assert.AreEqual(eEn.Current, aEn.Current);
					else
						Assert.IsTrue(comparer(eEn.Current, aEn.Current));
				}
			}
		}
	}
}
