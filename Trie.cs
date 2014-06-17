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

		public bool Add(T item) {
			throw new NotImplementedException();
		}

		void ICollection<T>.Add(T item) {
			throw new NotImplementedException();
		}

		public bool Remove(T item) {
			throw new NotImplementedException();
		}

		public void Clear() {
			throw new NotImplementedException();
		}

		public int Count {
			get { return this.count; }
		}

		public bool Contains(T item) {
			throw new NotImplementedException();
		}

		public void CopyTo(T[] array, int arrayIndex) {
			throw new NotImplementedException();
		}

		public bool IsReadOnly {
			get { throw new NotImplementedException(); }
		}

		public IEnumerator<T> GetEnumerator() {
			throw new NotImplementedException();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			throw new NotImplementedException();
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

		class Node<U>
		{
			Dictionary<U, Node<U>> children = new Dictionary<U, Node<U>>();
			Node<U> parent;

			public Node()
				: this(null) {
			}

			Node(Node<U> parent) {
				this.parent = parent;
			}

			public Node<U> this[U u] {
				get {
					Node<U> child;
					if (!children.TryGetValue(u, out child)) {
						child = new Node<U>(this);
						children[u] = child;
					}
					return child;
				}
			}

			public IEnumerable<KeyValuePair<U, Node<U>>> Children {
				get {
					return this.children;
				}
			}

			public bool Included { get; set; } // Deleting children on Included=false not implemented
		}
	}
}
