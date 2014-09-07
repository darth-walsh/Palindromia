using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Palindromia;

namespace PaliTest
{
	[TestFixture]
	public class PalindromeTest
	{
		[Test]
		public void TestExactSingle() {
			VerifyExact("stressed desserts");
		}

		[Test]
		public void TestExactDouble() {
			VerifyExact("too hot to hoot");
		}

		[Test]
		public void TestExactTriple() {
			VerifyExact("stressed no tips spit on desserts");
		}

		static void VerifyExact(string sentence) {
			var palindrome = sentence.Split(' ');
			var trie = new Trie<string, char>(StringConcat.Instance, palindrome);

			Assert.AreEqual(palindrome, Palindrome<string, char>.FindExact(palindrome.Take(palindrome.Length / 2), trie));
		}
	}
}
