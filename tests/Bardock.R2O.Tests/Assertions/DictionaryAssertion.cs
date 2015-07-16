using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Execution;
using System.Collections.Generic;
using System.Linq;

namespace Bardock.R2O.Tests.Assertions
{
    public static class DictionaryAssertion
    {
        public static AndConstraint<GenericCollectionAssertions<Dictionary<TKey, TValue>>> DeepContain<TKey, TValue>(
            this GenericCollectionAssertions<Dictionary<TKey, TValue>> @this,
            IDictionary<TKey, TValue> expected, string because = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .UsingLineBreaks
                .ForCondition(@this.Subject.Any(x => IsEqual(x, expected)))
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected enumerable to contains Dictionary {0}{reason}.", expected);

            return new AndConstraint<GenericCollectionAssertions<Dictionary<TKey, TValue>>>(@this);
        }

        private static bool IsEqual<TKey, TValue>(IDictionary<TKey, TValue> a, IDictionary<TKey, TValue> b)
        {
            bool ret = true;
            foreach (var itemA in a)
            {
                if (!b.ContainsKey(itemA.Key))
                    return false;

                var valueA = a[itemA.Key];
                var valueB = b[itemA.Key];

                if (valueA == null)
                {
                    ret = ret && valueB == null;
                }
                else
                {
                    if (typeof(IDictionary<string, object>).IsAssignableFrom(valueA.GetType()))
                        ret = ret && IsEqual(valueA as IDictionary<string, object>, valueB as IDictionary<string, object>);
                    else if (typeof(IEnumerable<IDictionary<string, object>>).IsAssignableFrom(valueA.GetType()))
                        ret = ret && IsEqual(valueA as IEnumerable<IDictionary<string, object>>, valueB as IEnumerable<IDictionary<string, object>>);
                    else
                        ret = ret && valueA.Equals(valueB);
                }

                if (ret == false)
                    return false;
            }

            foreach (var itemB in b)
            {
                if (!a.ContainsKey(itemB.Key))
                    return false;
            }

            return ret;
        }

        private static bool IsEqual<TKey, TValue>(IEnumerable<IDictionary<TKey, TValue>> a, IEnumerable<IDictionary<TKey, TValue>> b)
        {
            if (a.Count() != b.Count())
                return false;

            bool ret = true;
            foreach (var itemA in a)
            {
                bool isContained = false;
                foreach (var itemB in b)
                {
                    isContained = isContained || IsEqual(itemA, itemB);
                    if (isContained)
                        continue;
                }

                ret = ret && isContained;

                if (ret == false)
                    return false;
            }
            return ret;
        }
    }
}