using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HyperMarket.Queries.Tests
{
    public static class AssertionExtensions
    {
        public static void AssertContains<T>(this IEnumerable<T> colleciton, Func<T, bool> predicate)
        {
            var contains = colleciton.Any(predicate);
            Assert.IsTrue(contains);
        }

        public static void AssertNotContains<T>(this IEnumerable<T> colleciton, Func<T, bool> predicate)
        {
            var contains = colleciton.Any(predicate);
            Assert.IsFalse(contains);
        }
    }
}
