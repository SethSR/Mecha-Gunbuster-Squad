using System;
using System.Collections.Generic;
using System.Linq;

namespace Functional {
	static public class FuncForEach {
		static public void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
			foreach (var item in source) {
				action(item);
			}
		}
	}
}