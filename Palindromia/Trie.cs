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
		int count = 0;
		Node<Tel> root = new Node<Tel>();
		readonly IConcatable<T, Tel> concat;

		public Trie(IConcatable<T, Tel> concat) {
			this.concat = concat;
		}

		Node<Tel> Find(T item) {
			var search = root;
			foreach (var el in item)
				search = search[el];
			return search;
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
			this.root = new Node<Tel>();
			this.count = 0;
		}

		public int Count {
			get { return this.count; }
		}

		public bool Contains(T item) {
			return Find(item).Included;
		}

		public IEnumerator<T> GetEnumerator() {
			var search = new Queue<Node<Tel>>(new [] { this.root });
			while (search.Any()) {
				var toSearch = search.Dequeue();
				if (toSearch.Included)
					yield return default(T); //TODO
				foreach (var kvp in toSearch.Children)
					search.Enqueue(kvp.Value);
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			throw new NotImplementedException();
		}

		public void CopyTo(T[] array, int arrayIndex) {
			throw new NotImplementedException();
		}

		public bool IsReadOnly {
			get { return false; }
		}

		public void ExceptWith(IEnumerable<T> other) {
			throw new NotImplementedException();
		}

		public void IntersectWith(IEnumerable<T> other) {
			throw new NotImplementedException();
		}

		public bool IsProperSubsetOf(IEnumerable<T> other) {
			throw new NotImplementedException();
		}

		public bool IsProperSupersetOf(IEnumerable<T> other) {
			throw new NotImplementedException();
		}

		public bool IsSubsetOf(IEnumerable<T> other) {
			throw new NotImplementedException();
		}

		public bool IsSupersetOf(IEnumerable<T> other) {
			throw new NotImplementedException();
		}

		public bool Overlaps(IEnumerable<T> other) {
			throw new NotImplementedException();
		}

		public bool SetEquals(IEnumerable<T> other) {
			throw new NotImplementedException();
		}

		public void SymmetricExceptWith(IEnumerable<T> other) {
			throw new NotImplementedException();
		}

		public void UnionWith(IEnumerable<T> other) {
			throw new NotImplementedException();
		}

		class Node<Tel>
		{
			Dictionary<Tel, Node<Tel>> children = new Dictionary<Tel, Node<Tel>>();
			Node<Tel> parent;

			public Node()
				: this(null) {
			}

			Node(Node<Tel> parent) {
				this.parent = parent;
			}

			public Node<Tel> this[Tel u] {
				get {
					Node<Tel> child;
					if (!children.TryGetValue(u, out child)) {
						child = new Node<Tel>(this);
						children[u] = child;
					}
					return child;
				}
			}

			public IEnumerable<KeyValuePair<Tel, Node<Tel>>> Children {
				get {
					return this.children;
				}
			}

			public bool Included { get; set; } // Deleting children on Included=false not implemented
		}
	}
}
