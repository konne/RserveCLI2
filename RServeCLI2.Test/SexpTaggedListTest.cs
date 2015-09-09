using System.Collections.Generic;
using Xunit;
using Xunit.Extensions;

namespace RserveCLI2.Test
{
    public class SexpTaggedListTest
    {

        [Theory]
        [InlineData("foo", 1   , true)]
        [InlineData("bar", 2   , true)]
        [InlineData("baz", 3   , true)]
        [InlineData("???", null, false)]
        public void TryGetValue(string key, object value, bool exists)
        {
            var list = new SexpTaggedList
            {
                new KeyValuePair<string, Sexp>("foo", Sexp.Make(1)),
                new KeyValuePair<string, Sexp>("bar", Sexp.Make(2)),
                new KeyValuePair<string, Sexp>("baz", Sexp.Make(3)),
            };

            Sexp actual;
            var found = list.TryGetValue(key, out actual);
            Assert.Equal(exists, found);
            if (!exists)
                return;
            Assert.Equal(Sexp.Make(value), actual, EqualityComparer<Sexp>.Default);
        }
    }
}