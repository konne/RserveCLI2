//-----------------------------------------------------------------------
// Copyright (c) 2013, Suraj Gupta with contributions from Oliver M. Haynold
// All rights reserved.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Xunit;

namespace RserveCLI2.Test
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
            var values4 = new int[ 3 , 4 ] { { 8 , 2 , 7 , 4 } , { 0 , -9 , SexpArrayInt.Na , -2 } , { 1 , -4 , -3 , -8 } };

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
            var values3 = new int[ 4 ] { 8 , SexpArrayInt.Na , 7 , 4 };
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
                service.RConnection.VoidEval( "test = matrix( as.integer( c( 1 , NA , 3 , -4 , -5 , 6 ) ) , ncol = 3 , byrow = TRUE )" );
                var expected = new int[ 2 , 3 ] { { 1 , SexpArrayInt.Na , 3 } , { -4 , -5 , 6 } };

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
            var values2 = new int[ 3 , 4 ] { { 8 , 2 , 7 , SexpArrayInt.Na } , { 0 , -9 , 5 , -2 } , { 1 , -4 , -3 , -8 } };
            const int values3 = -5;
            var values4 = new List<int> { 4 , 5 , 6 };

            // Act
            Sexp sexp1 = Sexp.Make( values1 );
            Sexp sexp2 = Sexp.Make( values2 );
            Sexp sexp3 = new SexpArrayInt( values3 );
            Sexp sexp4 = new SexpArrayInt( values4 );
            Sexp sexp5 = new SexpArrayInt();

            // Assert
            Assert.Equal( new[] { 2 } , sexp1.AsInts );
            Assert.Equal( new[] { 8 , 0 , 1 , 2 , -9 , -4 , 7 , 5 , -3 , SexpArrayInt.Na , -2 , -8 } , sexp2.AsInts );
            Assert.Equal( new[] { -5 } , sexp3.AsInts );
            Assert.Equal( new[] { 4 , 5 , 6 } , sexp4.AsInts );
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
                Sexp sexp3 = service.RConnection[ "matrix( as.integer( c( 1 , 2 , 3 , 4 , 5 , NA ) ) , nrow = 2 )" ];

                // Assert
                Assert.Equal( new int[] { } , sexp1.AsInts );
                Assert.Equal( new[] { 1 , 2 , 3 } , sexp2.AsInts );
                Assert.Equal( new[] { 1 , 2 , 3 , 4 , 5 , SexpArrayInt.Na } , sexp3.AsInts );
            }

        }

        [Fact]
        public void Equals_ComparedToSameReferencedObject_ReturnsTrue()
        {
            // Arrange
            var value1 = new SexpArrayInt();
            var value2 = new SexpArrayInt( new[] { 2 , SexpArrayInt.Na , -4 } );
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
            var value2A = new SexpArrayInt( new[] { 1 , SexpArrayInt.Na , -3 } );
            var value2B = new SexpArrayInt( new[] { 1 , SexpArrayInt.Na , -3 } );
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
            var value2 = new SexpArrayInt( new[] { SexpArrayInt.Na , -5 , 4 } );
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
            var value1 = new SexpArrayInt( new[] { 2 , -5 , SexpArrayInt.Na } );
            var value2 = new SexpArrayBool( true );
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
        public void Equals_ComparedToObjectOfDifferentTypeThatCanBeCoercedIntoArrayInt_ReturnsTrue()
        {
            // Arrange
            var value1 = new SexpArrayInt( new[] { 2 , -5 , SexpArrayInt.Na } );
            var value2 = new double[] { 2.0d , -5.0d , Convert.ToDouble( SexpArrayInt.Na ) };
            var value3 = new List<int> { 2 , -5 , SexpArrayInt.Na };
            var value4 = new SexpArrayDouble( new List<double> { 2 , -5 , SexpArrayInt.Na } );

            // Act & Assert

            // ReSharper disable RedundantCast
            Assert.True( value1.Equals( value2 ) );
            Assert.True( value1.Equals( ( object )value2 ) );

            Assert.True( value1.Equals( value3 ) );
            Assert.True( value1.Equals( ( object )value3 ) );

            Assert.True( value1.Equals( value4 ) );
            Assert.True( value1.Equals( ( object )value4 ) );
            // ReSharper restore RedundantCast
        }

        [Fact]
        public void Equals_ComparedToObjectWithDifferentValues_ReturnsFalse()
        {
            // Arrange
            var value1 = new SexpArrayInt();
            var value2 = new SexpArrayInt( -7 );
            var value3 = new SexpArrayInt( new[] { -1 , -6 , -3 } );
            var value4 = new SexpArrayInt( new[] { SexpArrayInt.Na , -6 , -3 } );

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
                var value1A = service.RConnection[ "matrix( as.integer( c( 1 , 2 , 3 , -4 , -5 , NA ) ) , ncol = 3 , byrow = TRUE )" ];
                var value1B = new SexpArrayInt( new[] { 1 , -4 , 2 , -5 , 3 , SexpArrayInt.Na } );

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
                var value1A = service.RConnection[ "matrix( as.integer( c( NA , 2 , 3 , -4 , -5 , 6 ) ) , ncol = 3 , byrow = TRUE )" ];
                var value1B = new SexpArrayInt( new[] { SexpArrayInt.Na , -4 , 2 , -5 , 3 , 6 } );

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

        [Fact]
        public void IsNa_NaValueReadFromR_ReturnTrue()
        {
            using ( var rWrapper = new Rservice() )
            {
                // Arrange & Act
                Sexp naSexp = rWrapper.RConnection[ "as.integer( NA )" ];

                // Assert
                Assert.IsType<SexpArrayInt>( naSexp );
                Assert.True( naSexp.IsNa );
            }
        }

        [Fact]
        public void IsNa_NaValueInConstructor_ReturnTrue()
        {
            // Arrange
            var sexp1 = new SexpArrayInt( SexpArrayInt.Na );
            var sexp2 = new SexpArrayInt( new[] { SexpArrayInt.Na } );

            // Act & Assert
            Assert.True( sexp1.IsNa );
            Assert.True( sexp2.IsNa );
        }

        [Fact]
        public void IsNa_NotNa_ReturnFalse()
        {
            // Arrange
            var sexp1 = new SexpArrayInt( 3 );
            var sexp2 = new SexpArrayInt( new[] { -8 } );

            // Act & Assert
            Assert.False( sexp1.IsNa );
            Assert.False( sexp2.IsNa );
        }

        [Fact]
        public void IsNa_SexpContainsZeroOrMultipleValues_ThrowsNotSupportedException()
        {
            // Arrange
            var sexp1 = new SexpArrayInt();
            var sexp2 = new SexpArrayInt( new int[] { } );
            var sexp3 = new SexpArrayInt( new[] { SexpArrayInt.Na , 4 } );
            var sexp4 = new SexpArrayInt( new[] { 4 , -9 } );

            // Act & Assert
            Assert.Throws<NotSupportedException>( () => sexp1.IsNa );
            Assert.Throws<NotSupportedException>( () => sexp2.IsNa );
            Assert.Throws<NotSupportedException>( () => sexp3.IsNa );
            Assert.Throws<NotSupportedException>( () => sexp4.IsNa );
        }

        [Fact]
        public void Various_LinearAlgebraFunctionsOf2DArrayIntegers_MeetsExpectation()
        {
            // note: this test was ported from RserveCLI.  Not in the typical Arrange/Act/Assert format

            using ( var service = new Rservice() )
            {

                // Same as for integers -- we'll divide by two to get floating point values that aren't integers
                var matA = new[ , ] { { 14 , 9 , 3 } , { 2 , 11 , 15 } , { 0 , 12 , 17 } , { 5 , 2 , 3 } };
                var matB = new[ , ] { { 12 , 25 } , { 9 , 10 } , { 8 , 5 } };
                var matC = new[ , ] { { 273 , 455 } , { 243 , 235 } , { 244 , 205 } , { 102 , 160 } };
                var sexpA = Sexp.Make( matA );
                service.RConnection[ "a" ] = sexpA;
                service.RConnection[ "b" ] = Sexp.Make( matB );

                // Some simple tests with A
                for ( int i = 0 ; i <= 1 ; i++ )
                {
                    Assert.Equal( matA.GetLength( i ) , sexpA.GetLength( i ) );
                    Assert.Equal( matA.GetLength( i ) , service.RConnection[ "a" ].GetLength( i ) );
                }

                for ( int row = 0 ; row < matA.GetLength( 0 ) ; row++ )
                {
                    for ( int col = 0 ; col < matA.GetLength( 1 ) ; col++ )
                    {
                        Assert.Equal( matA[ row , col ] , sexpA[ row , col ].AsInt );
                        Assert.Equal( matA[ row , col ] , service.RConnection[ "a" ][ row , col ].AsInt );
                    }
                }

                var matD = service.RConnection[ "a %*% b" ];

                // check that C and D are equal
                for ( var i = 0 ; i <= 1 ; i++ )
                {
                    Assert.Equal( matC.GetLength( i ) , matD.GetLength( i ) );
                }

                for ( var row = 0 ; row < matC.GetLength( 0 ) ; row++ )
                {
                    for ( var col = 0 ; col < matD.GetLength( 1 ) ; col++ )
                    {
                        Assert.Equal( matC[ row , col ] , matD[ row , col ].AsInt );
                    }
                }
            }
        }

        [Fact]
        public void Various_LargeArray()
        {
            // note: this test was ported from RserveCLI.  Not in the typical Arrange/Act/Assert format

            using ( var service = new Rservice() )
            {
                service.RConnection.Eval( "x <- 1:1000000" );
                var x = service.RConnection[ "x" ];

                Assert.Equal( x.Count , 1000000 );

                for ( int i = 0 ; i < x.Count ; i++ )
                {
                    Assert.Equal( x[ i ].AsInt , i + 1 );
                }
            }
        }

        [Fact]
        public void Various_ArrayIntTests()
        {
            // note: this test was ported from RserveCLI.  Not in the typical Arrange/Act/Assert format

            using ( var service = new Rservice() )
            {

                var testInts = new[] { -3 , 0 , 1 , 2 , 524566 , 0 };
                var x1 = Sexp.Make( testInts );
                x1[ x1.Count - 1 ] = new SexpArrayInt( SexpArrayInt.Na );

                for ( int i = 0 ; i < x1.Count ; i++ )
                {
                    if ( !x1[ i ].IsNa )
                    {
                        Assert.Equal( testInts[ i ] , x1[ i ].AsInt );
                    }
                }

                service.RConnection.Eval( "x2 <- as.integer(c(-3,0,1,2,524566,NA))" );
                var x2 = service.RConnection[ "x2" ];

                Assert.Equal( x1.Count , x2.Count );

                for ( int i = 0 ; i < x1.Count ; i++ )
                {
                    if ( x1[ i ].IsNa )
                    {
                        Assert.True( x2[ i ].IsNa );
                    }
                    else
                    {
                        Assert.True( x1[ i ].AsDouble == x2[ i ].AsDouble );
                    }
                }

                service.RConnection[ "x1" ] = x1;
                var equals = service.RConnection[ "x1 == x2" ];

                Assert.Equal( x1.Count , equals.Count );

                for ( int i = 0 ; i < x1.Count ; i++ )
                {
                    if ( !x1[ i ].IsNa )
                    {
                        Assert.True( ( bool )equals[ i ].AsBool , equals.ToString() );
                    }
                }
                Assert.Equal( x1.IndexOf( new SexpArrayInt( 1 ) ) , 2 );
                x1.AsList[ 0 ] = -5;
                Assert.Equal( x1[ 0 ].AsInt , -5 );
            }
        }

        [Fact]
        public void Various_IntTests()
        {
            // note: this test was ported from RserveCLI.  Not in the typical Arrange/Act/Assert format

            var zero = new SexpArrayInt( 0 );
            var one = new SexpArrayInt( 1 );
            var na = new SexpArrayInt( SexpArrayInt.Na );

            // ReSharper disable EqualExpressionComparison
            Assert.True( zero.Equals( zero ) );
            Assert.True( one.Equals( one ) );
            Assert.True( na.Equals( na ) );

            // ReSharper restore EqualExpressionComparison
            Assert.True( !zero.Equals( one ) );
            Assert.True( !zero.Equals( na ) );
            Assert.True( !one.Equals( na ) );
            Assert.True( zero.Equals( 0 ) );
            Assert.True( one.Equals( 1 ) );
            Assert.True( na.IsNa );
            Assert.True( zero.AsInt == 0 );
            Assert.True( one.AsInt == 1 );

            foreach ( var a in new Sexp[] { zero , one , na } )
            {
                Assert.True( !a.Equals( new SexpNull() ) );
            }
        }

    }


}
