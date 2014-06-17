using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Palindromia;

namespace PaliTest
{
	[TestClass]
	public class TrieTest
	{
		[TestMethod]
		public void TestEmpty() {
			var t = new Trie<string, char>();

			Assert.AreEqual(0, t.Count);
		}
	}
}
