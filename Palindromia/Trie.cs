using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palindromia
{
	public class Trie<T, Tel> : ISet<T>
		where T : IEnumerable<Tel>
	{
		readonly IConcatable<T, Tel> concat;
		Node root;
		int count = 0;

		Trie(IConcatable<T, Tel> concat, Node root) {
			this.concat = concat;
			this.root = root;
		}

		public Trie(IConcatable<T, Tel> concat)
			: this(concat, new Node(concat))
		{
			this.concat = concat;
			this.root = new Node(concat);
		}

		public Trie(IConcatable<T, Tel> concat, IEnumerable<T> other) 
		  : this(concat)
		{
			foreach (var el in other)
				Add(el);
		}

		Node Find(IEnumerable<Tel> item) {
			return FindAll(item).Last();
		}

		IEnumerable<Node> FindAll(IEnumerable<Tel> item) {
			var search = root;
			yield return search;
			foreach (var el in item)
				yield return search = search[el];
		}

		IEnumerable<Node> GetIncludedNodes() {
			var search = new Queue<Node>(new[] { this.root });
			while (search.Any()) {
				var toSearch = search.Dequeue();
				if (toSearch.Included)
					yield return toSearch;
				foreach (var kvp in toSearch.Children)
					search.Enqueue(kvp.Value);
			}
		}

		public bool Add(T item) {
			var node = Find(item);
			if (node.Included)
				return false;

			node.Included = true;
			++this.count;
			return true;
		}

		void ICollection<T>.Add(T item) {
			Add(item);
		}

		public bool Remove(T item) {
			var node = Find(item);
			if (!node.Included)
				return false;

			node.Included = false;
			--this.count;
			return true;
		}

		public void Clear() {
			this.root = new Node(this.concat);
			this.count = 0;
		}

		public int Count {
			get { return this.count; }
		}

		public bool Contains(T item) {
			return Find(item).Included;
		}

		public IEnumerator<T> GetEnumerator() {
			foreach (var node in GetIncludedNodes())
				yield return node.Value;
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public void CopyTo(T[] array, int arrayIndex) {
			if (array == null)
				throw new ArgumentNullException("array");
			if (arrayIndex < 0)
				throw new ArgumentOutOfRangeException("arrayIndex", "" + arrayIndex);
			if(array.Length + arrayIndex < this.Count)
				throw new ArgumentException("array + arrayIndex");

			foreach (var t in this)
				array[arrayIndex++] = t;
		}

		public bool IsReadOnly {
			get { return false; }
		}

		// Could optimize for other is a Trie
		public bool IsProperSubsetOf(IEnumerable<T> other) {
			if (other == null)
				throw new ArgumentNullException("other");

			return new Trie<T, Tel>(this.concat, other).IsProperSupersetOf(this);
		}

		public bool IsProperSupersetOf(IEnumerable<T> other) {
			if (other == null)
				throw new ArgumentNullException("other");

			return this.Count > other.Count() && IsSupersetOf(other);
		}

		public bool IsSubsetOf(IEnumerable<T> other) {
			if (other == null)
				throw new ArgumentNullException("other");

			return new Trie<T, Tel>(this.concat, other).IsSupersetOf(this);
		}

		public bool IsSupersetOf(IEnumerable<T> other) {
			if (other == null)
				throw new ArgumentNullException("other");

			return other.All(Contains);
		}

		public bool Overlaps(IEnumerable<T> other) {
			if (other == null)
				throw new ArgumentNullException("other");

			return other.Any(Contains);
		}

		public bool SetEquals(IEnumerable<T> other) {
			if (other == null)
				throw new ArgumentNullException("other");

			return this.Count == other.Count() && IsSupersetOf(other);
		}

		public void ExceptWith(IEnumerable<T> other) {
			if (other == null)
				throw new ArgumentNullException("other");

			foreach (var t in other)
				Remove(t);
		}

		public void IntersectWith(IEnumerable<T> other) {
			if (other == null)
				throw new ArgumentNullException("other");

			var filter = new Trie<T, Tel>(this.concat, other);

			foreach (var node in GetIncludedNodes())
				if (!filter.Contains(node.Value))
					node.Included = false;
		}

		public void SymmetricExceptWith(IEnumerable<T> other) {
			if (other == null)
				throw new ArgumentNullException("other");

			foreach (var t in other) {
				var node = Find(t);
				node.Included = !node.Included;
			}
		}

		public void UnionWith(IEnumerable<T> other) {
			if (other == null)
				throw new ArgumentNullException("other");

			foreach (var t in other)
				Add(t);
		}

		// Elements that are the start of t
		// i.e. { "", "a", "ab" }.Subs("a") = { "", "a" }
		public IEnumerable<T> Subs(IEnumerable<Tel> item) {
			return FindAll(item).Where(n => n.Included).Select(n => n.Value);
		}

		// Elements that t is the start of 
		// i.e. { "", "a", "ab" }.Supers("a") = { "a", "ab" }
		public IEnumerable<T> Supers(T item) {
			return new Trie<T, Tel>(this.concat, Find(item));
		}

		class Node
		{
			IDictionary<Tel, Node> children = new SortedDictionary<Tel, Node>();
			readonly Node parent;
			readonly IConcatable<T, Tel> concat;
			public T Value { get; private set; }

			public Node(IConcatable<T, Tel> concat) {
				this.parent = null;
				this.concat = concat;

				this.Value = concat.Empty;
			}

			Node(Tel elem, IConcatable<T, Tel> concat, Node parent) {
				this.parent = parent;
				this.concat = concat;

				this.Value = concat.Concat(parent.Value, elem);
			}

			public Node this[Tel u] {
				get {
					Node child;
					if (!children.TryGetValue(u, out child)) {
						child = new Node(u, this.concat, this);
						children[u] = child;
					}
					return child;
				}
			}

			public IEnumerable<KeyValuePair<Tel, Node>> Children {
				get {
					return this.children;
				}
			}

			public bool Included { get; set; } // Pruning useless children on Included=false not implemented
		}
	}

	public class StringTrie : Trie<string, char>
	{
		public StringTrie()
			: base(StringConcat.Instance) {

		}
	}
}
