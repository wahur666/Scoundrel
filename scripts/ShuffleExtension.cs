using System;
using System.Collections.Generic;
using System.Linq;

namespace Scoundrel.scripts;

public static class ShuffleExtension {
	private static readonly Random Rng = new();

	public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source) {
		return source.OrderBy(_ => Rng.Next());
	}

	public static IEnumerable<T> Shuffle2<T>(this IEnumerable<T> source) {
		var shuffledList = source.ToList();
		int n = shuffledList.Count;
		while (n > 1) {
			n--;
			int k = Rng.Next(n + 1);
			(shuffledList[k], shuffledList[n]) = (shuffledList[n], shuffledList[k]);
		}

		return shuffledList;
	}
}
