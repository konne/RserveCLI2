using System.Collections.Generic;
using Xunit;

namespace RserveCLI2.Test
{
    public static class SexpTaggedListTest
    {
        [Fact]
        public static void TryGetValue_KeyExistsInTaggedList_ReturnsTrue()
        {
            // Arrange
            var list = new SexpTaggedList
            {
                new KeyValuePair<string, Sexp>("foo", Sexp.Make(1)),
                new KeyValuePair<string, Sexp>("bar", Sexp.Make(2)),
                new KeyValuePair<string, Sexp>("baz", Sexp.Make(3)),
            };

            Sexp actual;

            // Act
            var found1 = list.TryGetValue("foo", out actual);
            var found2 = list.TryGetValue("bar", out actual);
            var found3 = list.TryGetValue("baz", out actual);
            
            // Assert
            Assert.True(found1);
            Assert.True(found2);
            Assert.True(found3);
        }

        [Fact]
        public static void TryGetValue_KeyExistsInTaggedList_AssignSexpToValueOutParameter()
        {
            // Arrange
            var list = new SexpTaggedList
            {
                new KeyValuePair<string, Sexp>("foo", Sexp.Make(1)),
                new KeyValuePair<string, Sexp>("bar", Sexp.Make(2)),
                new KeyValuePair<string, Sexp>("baz", Sexp.Make(3)),
            };

            Sexp actual1;
            Sexp actual2;
            Sexp actual3;

            var expected1 = Sexp.Make(1);
            var expected2 = Sexp.Make(2);
            var expected3 = Sexp.Make(3);
            
            // Act
            list.TryGetValue("foo", out actual1);
            list.TryGetValue("bar", out actual2);
            list.TryGetValue("baz", out actual3);

            // Assert
            Assert.Equal(expected1, actual1, EqualityComparer<Sexp>.Default);
            Assert.Equal(expected2, actual2, EqualityComparer<Sexp>.Default);
            Assert.Equal(expected3, actual3, EqualityComparer<Sexp>.Default);
        }

        [Fact]
        public static void TryGetValue_KeyDoesNotExistInTaggedList_ReturnsFalse()
        {
            // Arrange
            var list = new SexpTaggedList
            {
                new KeyValuePair<string, Sexp>("foo", Sexp.Make(1)),
                new KeyValuePair<string, Sexp>("bar", Sexp.Make(2)),
                new KeyValuePair<string, Sexp>("baz", Sexp.Make(3)),
            };

            Sexp actual;

            // Act
            var found1 = list.TryGetValue("Foo", out actual);
            var found2 = list.TryGetValue("???", out actual);
            var found3 = list.TryGetValue(string.Empty, out actual);

            // Assert
            Assert.False(found1);
            Assert.False(found2);
            Assert.False(found3);
        }

        [Fact]
        public static void TryGetValue_KeyDoesNotExistsInTaggedList_AssignNullToValueOutParameter()
        {
            // Arrange
            var list = new SexpTaggedList
            {
                new KeyValuePair<string, Sexp>("foo", Sexp.Make(1)),
                new KeyValuePair<string, Sexp>("bar", Sexp.Make(2)),
                new KeyValuePair<string, Sexp>("baz", Sexp.Make(3)),
            };

            Sexp actual1;
            Sexp actual2;
            Sexp actual3;

            // Act
            list.TryGetValue("Foo", out actual1);
            list.TryGetValue("???", out actual2);
            list.TryGetValue(string.Empty, out actual3);

            // Assert
            Assert.Null(actual1);
            Assert.Null(actual2);
            Assert.Null(actual3);
        }
    }
}