using System;
using System.Collections.Generic;
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
        public void As2DArrayInt_SexpConstructedUsingIntOrIEnumerableOfInt_ThrowsNotSupportedException()
        {

            // Arrange
            
            // ReSharper disable RedundantExplicitArraySize
            var values1 = new int[ 1 ] { 2 };
            var values2 = new int[ 2 ] { 3 , 5 };
            var values3 = new int[ 4 ] { 8 , 2 , 7 , 4 };
            const int values4 = 5;
            // ReSharper restore RedundantExplicitArraySize

            // Act
            Sexp sexp1 = Sexp.Make( values1 );
            Sexp sexp2 = Sexp.Make( values2 );
            Sexp sexp3 = Sexp.Make( values3 );
            Sexp sexp4 = Sexp.Make( values4 );

            // Assert
            Assert.Throws<NotSupportedException>( () => sexp1.As2DArrayInt );
            Assert.Throws<NotSupportedException>( () => sexp2.As2DArrayInt );
            Assert.Throws<NotSupportedException>( () => sexp3.As2DArrayInt );
            Assert.Throws<NotSupportedException>( () => sexp4.As2DArrayInt );
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

        [Fact]
        public void AsInts_SexpConstructedUsingConstructorOrMake_ReturnsSameSetOfInts()
        {

            // Arrange
            var values1 = new int[ 1 , 1 ] { { 2 } };
            var values2 = new int[ 3 , 4 ] { { 8 , 2 , 7 , 4 } , { 0 , -9 , 5 , -2 } , { 1 , -4 , -3 , -8 } };
            const int values3 = -5;
            var values4 = new List<int> { 4 , 5 , 6 };

            // Act
            Sexp sexp1 = Sexp.Make( values1 );
            Sexp sexp2 = Sexp.Make( values2 );
            Sexp sexp3 = new SexpArrayInt( values3 );
            Sexp sexp4 = new SexpArrayInt( values4 );
            Sexp sexp5 = new SexpArrayInt();

            // Assert
            Assert.Equal( new [] { 2 } , sexp1.AsInts );
            Assert.Equal( new [] { 8 , 0 , 1 , 2 , -9 , -4 , 7 , 5 , -3 , 4 , -2 , -8 } , sexp2.AsInts );
            Assert.Equal( new [] { -5 } , sexp3.AsInts );
            Assert.Equal( new [] { 4 , 5 , 6 } , sexp4.AsInts );
            Assert.Equal( new int[] { } , sexp5.AsInts );
        }

        [Fact]
        public void AsInts_SexpConstructedFromR_ReturnsSameSetOfInts()
        {

            using ( var service = new Rservice() )
            {
                
                // Arrange & Act
                Sexp sexp1 = service.RConnection[ "integer()" ];
                Sexp sexp2 = service.RConnection[ "as.integer( c( 1 , 2 , 3 ) )" ];
                Sexp sexp3 = service.RConnection[ "matrix( as.integer( c( 1 , 2 , 3 , 4 , 5 , 6 ) ) , nrow = 2 )" ];

                // Assert
                Assert.Equal( new int[] { } , sexp1.AsInts );
                Assert.Equal( new [] { 1 , 2 , 3 } , sexp2.AsInts );
                Assert.Equal( new [] { 1 , 2 , 3 , 4 , 5 , 6 } , sexp3.AsInts );                
            }
            
        }
        
        [Fact]
        public void Equals_ComparedToSameReferencedObject_ReturnsTrue()
        {
            // Arrange
            var value1 = new SexpArrayInt();
            var value2 = new SexpArrayInt( new[] { 2 , -5 , 4 } );
            var value3 = new SexpArrayInt( -6 );

            // Act & Assert
            Assert.True( value1.Equals( value1 ) );
            Assert.True( value1.Equals( ( object )value1 ) );
            Assert.True( value2.Equals( value2 ) );
            Assert.True( value2.Equals( ( object )value2 ) );
            Assert.True( value3.Equals( value3 ) );
            Assert.True( value3.Equals( ( object )value3 ) );
        }

        [Fact]
        public void Equals_ComparedToObjectWithSameValues_ReturnsTrue()
        {
            // Arrange
            var value1A = new SexpArrayInt();
            var value1B = new SexpArrayInt();
            var value2A = new SexpArrayInt( new[] { 1 , -6 , -3 } );
            var value2B = new SexpArrayInt( new[] { 1 , -6 , -3 } );
            var value3A = new SexpArrayInt( 1 );
            var value3B = new SexpArrayInt( 1 );
            var value4A = new SexpArrayInt( new[] { -8 } );
            var value4B = new SexpArrayInt( -8 );

            // Act & Assert
            Assert.True( value1A.Equals( value1B ) );
            Assert.True( value2A.Equals( value2B ) );
            Assert.True( value3A.Equals( value3B ) );
            Assert.True( value4A.Equals( value4B ) );
        }

        [Fact]
        public void Equals_ComparedToNull_ReturnsFalse()
        {
            // Arrange
            var value1 = new SexpArrayInt();
            var value2 = new SexpArrayInt( new[] { 2 , -5 , 4 } );
            var value3 = new SexpArrayInt( 6 );

            // Act & Assert
            // ReSharper disable RedundantCast
            Assert.False( value1.Equals( ( SexpArrayInt )null ) );
            Assert.False( value1.Equals( ( object )null ) );
            Assert.False( value2.Equals( ( SexpArrayInt )null ) );
            Assert.False( value2.Equals( ( object )null ) );
            Assert.False( value3.Equals( ( SexpArrayInt )null ) );
            Assert.False( value3.Equals( ( object )null ) );
            // ReSharper restore RedundantCast
        }

        [Fact]
        public void Equals_ComparedToObjectOfDifferentType_ReturnsFalse()
        {
            // Arrange
            var value1 = new SexpArrayInt( new[] { 2 , -5 , 4 } );
            var value2 = new SexpBool( true );
            var value3 = new SexpArrayDouble( new[] { 1.4 , 3.6 } );

            // Act & Assert

            // ReSharper disable RedundantCast
            Assert.False( value1.Equals( value2 ) );
            Assert.False( value1.Equals( ( object )value2 ) );
            Assert.False( value1.Equals( value3 ) );
            Assert.False( value1.Equals( ( object )value3 ) );
            // ReSharper restore RedundantCast
        }

        [Fact]
        public void Equals_ComparedToObjectWithDifferentValues_ReturnsFalse()
        {
            // Arrange
            var value1 = new SexpArrayInt();
            var value2 = new SexpArrayInt( -7 );
            var value3 = new SexpArrayInt( new[] { -1 , -6 , -3 } );
            var value4 = new SexpArrayInt( new[] { 1 , -6 , -3 } );

            // Act & Assert
            Assert.False( value1.Equals( value2 ) );
            Assert.False( value1.Equals( value3 ) );
            Assert.False( value1.Equals( value4 ) );
            Assert.False( value2.Equals( value1 ) );
            Assert.False( value2.Equals( value3 ) );
            Assert.False( value2.Equals( value4 ) );
            Assert.False( value3.Equals( value1 ) );
            Assert.False( value3.Equals( value2 ) );
            Assert.False( value3.Equals( value4 ) );
            Assert.False( value4.Equals( value1 ) );
            Assert.False( value4.Equals( value2 ) );
            Assert.False( value4.Equals( value3 ) );
        }

        [Fact]
        public void Equals_ComparedToObjectWithSameValuesReadFromR_ReturnsFalse()
        {
            using ( var service = new Rservice() )
            {
                // Arrange                
                var value1A = service.RConnection[ "matrix( as.integer( c( 1 , 2 , 3 , -4 , -5 , 6 ) ) , ncol = 3 , byrow = TRUE )" ];
                var value1B = new SexpArrayInt( new[] { 1 , -4 , 2 , -5 , 3 , 6 } );

                var value2A = service.RConnection[ "as.integer( c( -4 , -2 , -1 ) )" ];
                var value2B = new SexpArrayInt( new[] { -4 , -2 , -1 } );

                var value3A = service.RConnection[ "as.integer( -9 )" ];
                var value3B = new SexpArrayInt( -9 );

                var value4A = service.RConnection[ "integer()" ];
                var value4B = new SexpArrayInt();

                // Act & Assert
                Assert.True( value1A.Equals( value1B ) );
                Assert.True( value1B.Equals( value1A ) );
                Assert.True( value2A.Equals( value2B ) );
                Assert.True( value2B.Equals( value2A ) );
                Assert.True( value3A.Equals( value3B ) );
                Assert.True( value3B.Equals( value3A ) );
                Assert.True( value4A.Equals( value4B ) );
                Assert.True( value4B.Equals( value4A ) );
            }
        }

        [Fact]
        public void Equals_ComparedToObjectWithDifferentValuesReadFromR_ReturnsFalse()
        {
            using ( var service = new Rservice() )
            {
                // Arrange                
                var value1A = service.RConnection[ "matrix( as.integer( c( 1 , 2 , 3 , -4 , -5 , 6 ) ) , ncol = 3 , byrow = TRUE )" ];
                var value1B = new SexpArrayInt( new[] { 1 , -4 , 2 , -5 , 3 , 6 } );

                var value2A = service.RConnection[ "as.integer( c( -4 , -2 , -1 ) )" ];
                var value2B = new SexpArrayInt( new[] { -4 , -2 , -1 } );

                var value3A = service.RConnection[ "as.integer( -9 )" ];
                var value3B = new SexpArrayInt( -9 );

                var value4A = service.RConnection[ "integer()" ];
                var value4B = new SexpArrayInt();

                // Act & Assert
                Assert.False( value3B.Equals( value2A ) );
                Assert.False( value2A.Equals( value4A ) );
                Assert.False( value3B.Equals( value2B ) );
                Assert.False( value4A.Equals( value1B ) );
                Assert.False( value3B.Equals( value1A ) );
                Assert.False( value4A.Equals( value3A ) );
                Assert.False( value1A.Equals( value2B ) );
                Assert.False( value2A.Equals( value3A ) );
                Assert.False( value3A.Equals( value1A ) );
                Assert.False( value1B.Equals( value4B ) );

            }
        }

    }
}
