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
	}

	public class StringConcat : IConcatable<string, char>
	{
		StringConcat() { }

		public static readonly StringConcat Instance = new StringConcat();

		public string Concat(string value, char item) {
			return value + item;
		}
	}
}
