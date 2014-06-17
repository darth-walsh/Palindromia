using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palindromia
{
	public interface IConcatable<T, Tel>
		where T : IEnumerable<Tel>
	{
		T Concat(T value, Tel item);

		T Empty { get; }
	}

	public class StringConcat : IConcatable<string, char>
	{
		StringConcat() { }

		public static readonly StringConcat Instance = new StringConcat();

		public string Concat(string value, char item) {
			return value + item;
		}

		public string Empty { get { return string.Empty; } }
	}

	public class ListConcat<T> : IConcatable<IList<T>, T>
	{
		ListConcat() { }

		public static readonly ListConcat<T> Instance = new ListConcat<T>();

		public IList<T> Concat(IList<T> value, T item) {
			var copy = new List<T>(value);
			copy.Add(item);
			return copy;
		}

		public IList<T> Empty { get { return new List<T>(); } }
	}
}
