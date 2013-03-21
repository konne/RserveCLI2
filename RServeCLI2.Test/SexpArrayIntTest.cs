using System;
using Xunit;

namespace RserveCLI2.Tests
{

    /// <summary>
    /// Tests RserveCLI2.SexpArrayInt
    /// </summary>
    public class SexpArrayIntTest
    {

        [Fact]
        public void As2DArrayInt_SexpConstructedUsing2dArray_ReturnsSame2dArray()
        {

            // Arrange
            var values1 = new int[ 1 , 1 ] { { 2 } };
            var values2 = new int[ 2 , 1 ] { { 3 } , { 5 } };
            var values3 = new int[ 1 , 2 ] { { 7 , 4 } };
            var values4 = new int[ 3 , 4 ] { { 8 , 2 , 7 , 4 } , { 0 , -9 , 5 , -2 } , { 1 , -4 , -3 , -8 } };

            // Act
            Sexp sexp1 = Sexp.Make( values1 );
            Sexp sexp2 = Sexp.Make( values2 );
            Sexp sexp3 = Sexp.Make( values3 );
            Sexp sexp4 = Sexp.Make( values4 );

            // Assert
            Assert.Equal( values1 , sexp1.As2DArrayInt );
            Assert.Equal( values2 , sexp2.As2DArrayInt );
            Assert.Equal( values3 , sexp3.As2DArrayInt );
            Assert.Equal( values4 , sexp4.As2DArrayInt );

        }

        [Fact]
        public void As2DArrayInt_SexpConstructedUsingIEnumerableOfInt_ThrowsNotSupportedException()
        {

            // Arrange
            var values1 = new int[ 1 ] { 2 };
            var values2 = new int[ 2 ] { 3 , 5 };
            var values3 = new int[ 4 ] { 8 , 2 , 7 , 4 };

            // Act
            Sexp sexp1 = Sexp.Make( values1 );
            Sexp sexp2 = Sexp.Make( values2 );
            Sexp sexp3 = Sexp.Make( values3 );

            // Assert
            Assert.Throws<NotSupportedException>( () => sexp1.As2DArrayInt );
            Assert.Throws<NotSupportedException>( () => sexp2.As2DArrayInt );
            Assert.Throws<NotSupportedException>( () => sexp3.As2DArrayInt );
        }

        [Fact]
        public void As2DArrayInt_MatrixCreatedInR_Returns2DArrayWithProperValues()
        {
            using ( var service = new Rservice() )
            {
                // Arrange                
                service.RConnection.VoidEval( "test = matrix( as.integer( c( 1 , 2 , 3 , -4 , -5 , 6 ) ) , ncol = 3 , byrow = TRUE )" );
                var expected = new int[ 2 , 3 ] { { 1 , 2 , 3 } , { -4 , -5 , 6 } };

                // Act
                Sexp matrix = service.RConnection[ "test" ];

                // Assert
                Assert.IsType<SexpArrayInt>( matrix );
                Assert.Equal( expected , matrix.As2DArrayInt );
            }
        }


    }
}
