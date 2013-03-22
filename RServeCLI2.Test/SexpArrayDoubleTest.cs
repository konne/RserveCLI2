using System;
using Xunit;

namespace RserveCLI2.Tests
{

    /// <summary>
    /// Tests RserveCLI2.SexpArrayDouble
    /// </summary>
    public class SexpArrayDoubleTest
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
            
            // ReSharper disable RedundantExplicitArraySize
            var values1 = new double[ 1 ] { 2.3 };
            var values2 = new double[ 2 ] { 3.6 , 5.2 };
            var values3 = new double[ 4 ] { 8.3 , 2.9 , 7.1 , -4.4 };
            // ReSharper restore RedundantExplicitArraySize

            // Act
            Sexp sexp1 = Sexp.Make( values1 );
            Sexp sexp2 = Sexp.Make( values2 );
            Sexp sexp3 = Sexp.Make( values3 );

            // Assert
            Assert.Throws<NotSupportedException>( () => sexp1.As2DArrayDouble );
            Assert.Throws<NotSupportedException>( () => sexp2.As2DArrayDouble );
            Assert.Throws<NotSupportedException>( () => sexp3.As2DArrayDouble );
        }

        [Fact]
        public void As2DArrayDouble_MatrixCreatedInR_Returns2DArrayWithProperValues()
        {
            using ( var service = new Rservice() )
            {
                // Arrange                
                service.RConnection.VoidEval( "test = matrix( c( 1.1 , 2.6 , 3.9 , -4.2 , -5.8 , 6.3 ) , ncol = 3 , byrow = TRUE )" );
                var expected = new double[ 2 , 3 ] { { 1.1 , 2.6 , 3.9 } , { -4.2 , -5.8 , 6.3 } };

                // Act
                Sexp matrix = service.RConnection[ "test" ];

                // Assert
                Assert.IsType<SexpArrayDouble>( matrix );
                Assert.Equal( expected , matrix.As2DArrayDouble );
            }
        }

        [Fact]
        public void Equals_ComparedToSameReferencedObject_ReturnsTrue()
        {            
            // Arrange
            var value1 = new SexpArrayDouble();
            var value2 = new SexpArrayDouble( new[] { 2.3 , -5 , 4 } );
            var value3 = new SexpArrayDouble( -6.6 );

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
            var value1A = new SexpArrayDouble();
            var value1B = new SexpArrayDouble();
            var value2A = new SexpArrayDouble( new[] { 1.2 , -6.4 , -3.3 } );
            var value2B = new SexpArrayDouble( new[] { 1.2 , -6.4 , -3.3 } );
            var value3A = new SexpArrayDouble( 1.4 );
            var value3B = new SexpArrayDouble( 1.4 );
            var value4A = new SexpArrayDouble( new[] { -8.4 } );
            var value4B = new SexpArrayDouble( -8.4 );

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
            var value1 = new SexpArrayDouble();
            var value2 = new SexpArrayDouble( new[] { 2.3 , -5 , 4 } );
            var value3 = new SexpArrayDouble( 6.5 );

            // Act & Assert
            
            // ReSharper disable RedundantCast
            Assert.False( value1.Equals( ( SexpArrayDouble )null ) );
            Assert.False( value1.Equals( ( object )null ) );
            Assert.False( value2.Equals( ( SexpArrayDouble )null ) );            
            Assert.False( value2.Equals( ( object )null ) );
            Assert.False( value3.Equals( ( SexpArrayDouble )null ) );
            Assert.False( value3.Equals( ( object )null ) );
            // ReSharper restore RedundantCast

        }

        [Fact]
        public void Equals_ComparedToObjectOfDifferentType_ReturnsFalse()
        {
            // Arrange
            var value1 = new SexpArrayDouble( new[] { 2.3 , -5 , 4 } );
            var value2 = new SexpBool( true );
            var value3 = new SexpArrayInt( new[] { 1 , 3 } );

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
            var value1 = new SexpArrayDouble();
            var value2 = new SexpArrayDouble( -7.7 );
            var value3 = new SexpArrayDouble( new[] { -1.2 , -6.4 , -3.3 } );
            var value4 = new SexpArrayDouble( new[] { 1.2 , -6.4 , -3.3 } );

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
                var value1A = service.RConnection[ "matrix( c( 1.1 , 2.6 , 3.9 , -4.2 , -5.8 , 6.3 ) , ncol = 3 , byrow = TRUE )" ];
                var value1B = new SexpArrayDouble( new[] { 1.1 , -4.2 , 2.6 , -5.8 , 3.9 , 6.3 } );

                var value2A = service.RConnection[ "c( -4.4 , -2.2 , -1 )" ];
                var value2B = new SexpArrayDouble( new[] { -4.4 , -2.2 , -1 } );

                var value3A = service.RConnection[ "-.9" ];
                var value3B = new SexpArrayDouble( -.9 );

                var value4A = service.RConnection[ "numeric()" ];
                var value4B = new SexpArrayDouble( );
                
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
                var value1A = service.RConnection[ "matrix( c( 1.1 , 2.6 , 3.9 , -4.2 , -5.8 , 6.3 ) , ncol = 3 , byrow = TRUE )" ];
                var value1B = new SexpArrayDouble( new[] { 1.1 , -4.2 , 2.6 , -5.8 , 3.9 , 6.3 } );

                var value2A = service.RConnection[ "c( -4.4 , -2.2 , -1 )" ];
                var value2B = new SexpArrayDouble( new[] { -4.4 , -2.2 , -1 } );

                var value3A = service.RConnection[ "-.9" ];
                var value3B = new SexpArrayDouble( -.9 );

                var value4A = service.RConnection[ "numeric()" ];
                var value4B = new SexpArrayDouble();

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
