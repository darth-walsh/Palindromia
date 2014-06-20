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

			var expected = new[] { "", "ae", "abc" };
			int i = 0;
			foreach (string s in t)
				Assert.AreEqual(expected[i++], s);
			Assert.AreEqual(3, i);

			i = 0;
			var en = (t as System.Collections.IEnumerable).GetEnumerator();
			while (en.MoveNext())
				Assert.AreEqual(expected[i++], en.Current);
			Assert.AreEqual(3, i);
		}

		[TestMethod]
		public void TestCopyTo() {
			var t = new Trie<string, char>(StringConcat.Instance) { "a", "abc", "" };

			var a = new string[3];
			t.CopyTo(a, 0);
			EnumerateEqual(t, a);

			a = new string[4];
			a[0] = "first";
			t.CopyTo(a, 1);
			Assert.AreEqual("first", a[0]);
			EnumerateEqual(t, a.Skip(1));
		}

		[TestMethod]
		public void TestSuperSet() {
			var a = new Trie<string, char>(StringConcat.Instance) { "a" };
			var ab = new Trie<string, char>(StringConcat.Instance) { "a", "b" };

			Assert.IsTrue(ab.IsSupersetOf(a));
			Assert.IsFalse(a.IsSupersetOf(ab));
			Assert.IsTrue(a.IsSupersetOf(a));
		}

		[TestMethod]
		public void TestProperSuperSet() {
			var a = new Trie<string, char>(StringConcat.Instance) { "a" };
			var ab = new Trie<string, char>(StringConcat.Instance) { "a", "b" };

			Assert.IsTrue(ab.IsProperSupersetOf(a));
			Assert.IsFalse(a.IsProperSupersetOf(ab));
			Assert.IsFalse(a.IsProperSupersetOf(a));
		}

		[TestMethod]
		public void TestSubSet() {
			var a = new Trie<string, char>(StringConcat.Instance) { "a" };
			var ab = new Trie<string, char>(StringConcat.Instance) { "a", "b" };

			Assert.IsTrue(a.IsSubsetOf(ab));
			Assert.IsFalse(ab.IsSubsetOf(a));
			Assert.IsTrue(a.IsSubsetOf(a));
		}

		[TestMethod]
		public void TestProperSubSet() {
			var a = new Trie<string, char>(StringConcat.Instance) { "a" };
			var ab = new Trie<string, char>(StringConcat.Instance) { "a", "b" };

			Assert.IsTrue(a.IsProperSubsetOf(ab));
			Assert.IsFalse(ab.IsProperSubsetOf(a));
			Assert.IsFalse(a.IsProperSubsetOf(a));
		}

		[TestMethod]
		public void TestOverlaps() {
			var t = new Trie<string, char>(StringConcat.Instance) { "a", "abc", "" };

			Assert.IsTrue(t.Overlaps(new[] { "a" }));
			Assert.IsFalse(t.Overlaps(new[] { "b" }));
		}

		[TestMethod]
		public void TestSetEquals() {
			var t = new Trie<string, char>(StringConcat.Instance) { "a", "abc", "" };

			Assert.IsTrue(t.SetEquals(new[] { "a", "abc", "" }));
			Assert.IsFalse(t.SetEquals(new[] { "a", "" }));
			Assert.IsFalse(t.SetEquals(new[] { "a", "ab", "" }));
			Assert.IsFalse(t.SetEquals(new[] { "a", "abc", "", "c" }));
		}

		[TestMethod]
		public void TestExceptwith() {
			var t = new Trie<string, char>(StringConcat.Instance) { "a", "abc", "" };

			t.ExceptWith(new[] { "", "a", "bbb" });
			EnumerateEqual(new[] { "abc" }, t);
		}

		[TestMethod]
		public void TestIntersectWith() {
			var t = new Trie<string, char>(StringConcat.Instance) { "a", "abc", "" };

			t.IntersectWith(new[] { "", "a", "bbb" });
			EnumerateEqual(new[] { "", "a" }, t);
		}

		[TestMethod]
		public void TestSymmetricExceptWith() {
			var t = new Trie<string, char>(StringConcat.Instance) { "a", "abc", "" };

			t.SymmetricExceptWith(new[] { "", "a", "bbb" });
			EnumerateEqual(new[] { "abc", "bbb" }, t);
		}

		[TestMethod]
		public void TestUnionWith() {
			var t = new Trie<string, char>(StringConcat.Instance) { "a", "abc", "" };

			t.UnionWith(new[] { "", "a", "bbb" });
			EnumerateEqual(new[] { "", "a", "abc", "bbb" }, t);
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
