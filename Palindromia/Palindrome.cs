using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palindromia
{
	public class Palindrome<T, Tel>
		where T : IEnumerable<Tel> 
	{
		public static IEnumerable<T> FindExact(IEnumerable<T> seed, Trie<T, Tel> language){
			var backwards = seed.SelectMany(el => el).ToList();
			backwards.Reverse();

			IEnumerable<T> palindrome;
			if (new Palindrome<T, Tel>(backwards, language).TryFindAfter(0, out palindrome)) {
				return seed.Concat(palindrome);
			} else
				throw new ArgumentException("seed");
		}

		List<Tel> backwards;
		Trie<T, Tel> language;

		Palindrome(List<Tel> backwards, Trie<T, Tel> language)
		{
			this.backwards = backwards;
			this.language = language;
		}

		bool TryFindAfter(int index, out IEnumerable<T> found) {
			if (index == backwards.Count) {
				found = Enumerable.Empty<T>();
				return true;
			}

			// probably should priority queue search in the future
			foreach (var t in language.Subs(backwards.Skip(index)).OrderByDescending(t => t.Count())) {
				var nextIndex = index + t.Count();
				IEnumerable<T> nextFound;
				if (TryFindAfter(nextIndex, out nextFound)) {
					found = new[] { t }.Concat(nextFound);
					return true;
				}
			}

			found = null;
			return false;
		}
	}
}