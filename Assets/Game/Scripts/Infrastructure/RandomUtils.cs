using System;
using System.Collections.Generic;

namespace Utils {

    public static class RandomUtils {

        private static readonly System.Random _random = new System.Random();

        public static int Next(int maxExclusive) {
            return _random.Next(maxExclusive);
        }

        public static T PickRandom<T>(this IList<T> array) {
            return array[Next(array.Count)];
        }

        public static T PickRandom<T>(this IEnumerable<T> enumerable) {
            T current = default(T);
            int count = 0;
            foreach (T element in enumerable) {
                count++;
                if (Next(count) == 0) {
                    current = element;
                }
            }
            if (count == 0) {
                throw new System.InvalidOperationException("Sequence was empty");
            }
            return current;
        }

        public static T PickWeightedRandom<T>(IEnumerable<T> enumerable, Func<T, int> weightGetter) {
            var totalWeight = 0;
            foreach (var item in enumerable) {
                totalWeight += weightGetter(item);
            }

            var randomNumber = Next(totalWeight);
            foreach (var item in enumerable) {
                var itemWeight = weightGetter(item);
                if (randomNumber < itemWeight) {
                    return item;
                }
                randomNumber -= itemWeight;
            }

            return default;
        }
    }
}