using System;
using Xunit;

namespace RserveCLI2.Tests
{

    /// <summary>
    /// Tests RserveCLI2.SexpArrayDouble
    /// </summary>
    public class SexpArrayDouble
    {

        [Fact]
        public void As2DArrayDouble_SexpConstructedUsing2dArray_ReturnsSame2dArray()
        {
            
            // Arrange
            var values1 = new double[ 1 , 1 ] { { 2.9 } };
            var values2 = new double[ 2 , 1 ] { { 3.9 } , { 5.6 } };
            var values3 = new double[ 1 , 2 ] { { 7.9 , 4.3 } };
            var values4 = new double[ 3 , 4 ] { { 8.8 , 2.1 , 7.5 , 4.3 } , { 0.1 , -9.8 , 5.1 , -2.7 } , { 1.1 , -4.6 , -4.5 , -8.7 } };
            
            // Act
            Sexp sexp1 = Sexp.Make( values1 );
            Sexp sexp2 = Sexp.Make( values2 );
            Sexp sexp3 = Sexp.Make( values3 );
            Sexp sexp4 = Sexp.Make( values4 );

            // Assert
            Assert.Equal( values1 , sexp1.As2DArrayDouble );
            Assert.Equal( values2 , sexp2.As2DArrayDouble );
            Assert.Equal( values3 , sexp3.As2DArrayDouble );
            Assert.Equal( values4 , sexp4.As2DArrayDouble );

        }

        [Fact]
        public void As2DArrayDouble_SexpConstructedUsingIEnumerableOfDouble_ThrowsNotSupportedException()
        {

            // Arrange
            var values1 = new double[ 1 ] { 2.3 };
            var values2 = new double[ 2 ] { 3.6 , 5.2 };
            var values3 = new double[ 4 ] { 8.3 , 2.9 , 7.1 , -4.4 };

            // Act
            Sexp sexp1 = Sexp.Make( values1 );
            Sexp sexp2 = Sexp.Make( values2 );
            Sexp sexp3 = Sexp.Make( values3 );

            // Assert
            Assert.Throws<NotSupportedException>( () => sexp1.As2DArrayDouble );
            Assert.Throws<NotSupportedException>( () => sexp2.As2DArrayDouble );
            Assert.Throws<NotSupportedException>( () => sexp3.As2DArrayDouble );
        }

    }
}
