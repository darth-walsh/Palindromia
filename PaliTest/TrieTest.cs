using System;
using System.Collections.Generic;
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
	}
}
