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
            var values5 = new double[ 2 , 1 ] { { 3.9 } , { double.NaN } };

            // Act
            Sexp sexp1 = Sexp.Make( values1 );
            Sexp sexp2 = Sexp.Make( values2 );
            Sexp sexp3 = Sexp.Make( values3 );
            Sexp sexp4 = Sexp.Make( values4 );
            Sexp sexp5 = Sexp.Make( values5 );

            // Assert
            Assert.Equal( values1 , sexp1.As2DArrayDouble );
            Assert.Equal( values2 , sexp2.As2DArrayDouble );
            Assert.Equal( values3 , sexp3.As2DArrayDouble );
            Assert.Equal( values4 , sexp4.As2DArrayDouble );
            Assert.Equal( values5 , sexp5.As2DArrayDouble );
        }

        [Fact]
        public void As2DArrayDouble_SexpConstructedUsingDoubleOrIEnumerableOfDouble_ThrowsNotSupportedException()
        {

            // Arrange

            // ReSharper disable RedundantExplicitArraySize
            var values1 = new double[ 1 ] { 2.3 };
            var values2 = new double[ 2 ] { 3.6 , 5.2 };
            var values3 = new double[ 4 ] { 8.3 , 2.9 , 7.1 , -4.4 };
            const double values4 = 4.4;
            // ReSharper restore RedundantExplicitArraySize

            // Act
            Sexp sexp1 = Sexp.Make( values1 );
            Sexp sexp2 = Sexp.Make( values2 );
            Sexp sexp3 = Sexp.Make( values3 );
            Sexp sexp4 = Sexp.Make( values4 );

            // Assert
            Assert.Throws<NotSupportedException>( () => sexp1.As2DArrayDouble );
            Assert.Throws<NotSupportedException>( () => sexp2.As2DArrayDouble );
            Assert.Throws<NotSupportedException>( () => sexp3.As2DArrayDouble );
            Assert.Throws<NotSupportedException>( () => sexp4.As2DArrayDouble );
        }

        [Fact]
        public void As2DArrayDouble_MatrixCreatedInR_Returns2DArrayWithProperValues()
        {
            using ( var service = new Rservice() )
            {
                // Arrange                
                service.RConnection.VoidEval( "test = matrix( c( 1.1 , 2.6 , 3.9 , -4.2 , -5.8 , NA ) , ncol = 3 , byrow = TRUE )" );
                var expected = new double[ 2 , 3 ] { { 1.1 , 2.6 , 3.9 } , { -4.2 , -5.8 , double.NaN } };

                // Act
                Sexp matrix = service.RConnection[ "test" ];

                // Assert
                Assert.IsType<SexpArrayDouble>( matrix );
                Assert.Equal( expected , matrix.As2DArrayDouble );
            }
        }

        [Fact]
        public void AsDoubles_SexpConstructedUsingConstructorOrMake_ReturnsSameSetOfDoubles()
        {

            // Arrange
            var values1 = new double[ 1 , 1 ] { { 2 } };
            var values2 = new double[ 3 , 4 ] { { 8 , 2 , 7 , 4 } , { 0 , double.NaN , 5 , -2 } , { 1 , -4 , -3 , -8 } };
            const double values3 = -5d;
            var values4 = new List<double> { 4 , 5 , 6 };

            // Act
            Sexp sexp1 = Sexp.Make( values1 );
            Sexp sexp2 = Sexp.Make( values2 );
            Sexp sexp3 = new SexpArrayDouble( values3 );
            Sexp sexp4 = new SexpArrayDouble( values4 );
            Sexp sexp5 = new SexpArrayDouble();

            // Assert
            Assert.Equal( new double[] { 2 } , sexp1.AsDoubles );
            Assert.Equal( new[] { 8 , 0 , 1 , 2 , double.NaN , -4 , 7 , 5 , -3 , 4 , -2 , -8 } , sexp2.AsDoubles );
            Assert.Equal( new double[] { -5 } , sexp3.AsDoubles );
            Assert.Equal( new double[] { 4 , 5 , 6 } , sexp4.AsDoubles );
            Assert.Equal( new double[] { } , sexp5.AsDoubles );
        }

        [Fact]
        public void AsDoubles_SexpConstructedFromR_ReturnsSameSetOfDoubles()
        {

            using ( var service = new Rservice() )
            {

                // Arrange & Act
                Sexp sexp1 = service.RConnection[ "numeric()" ];
                Sexp sexp2 = service.RConnection[ "c( 1.1 , 2.1 , 3.1 )" ];
                Sexp sexp3 = service.RConnection[ "matrix( c( 1.1 , NA , 3.1 , 4.1 , 5.1 , 6.1 ) , nrow = 2 )" ];

                // Assert
                Assert.Equal( new double[] { } , sexp1.AsDoubles );
                Assert.Equal( new[] { 1.1 , 2.1 , 3.1 } , sexp2.AsDoubles );
                Assert.Equal( new[] { 1.1 , double.NaN , 3.1 , 4.1 , 5.1 , 6.1 } , sexp3.AsDoubles );
            }

        }

        [Fact]
        public void Equals_ComparedToSameReferencedObject_ReturnsTrue()
        {
            // Arrange
            var value1 = new SexpArrayDouble();
            var value2 = new SexpArrayDouble( new[] { 2.3 , double.NaN , 4 } );
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
            var value2A = new SexpArrayDouble( new[] { 1.2 , double.NaN , -3.3 } );
            var value2B = new SexpArrayDouble( new[] { 1.2 , double.NaN , -3.3 } );
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
            var value2 = new SexpArrayDouble( new[] { 2.3 , double.NaN , 4 } );
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
            var value1 = new SexpArrayDouble( new[] { 2.3 , double.NaN , 4 } );
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
        public void Equals_ComparedToObjectOfDifferentTypeThatCanBeCoercedIntoArrayDouble_ReturnsTrue()
        {
            // Arrange
            var value1 = new SexpArrayDouble( new[] { 4d , -2d , -3d } );
            var value2 = new List<double> { 4 , -2 , -3 };
            var value3 = new int[] { 4 , -2 , -3 };
            var value4 = new SexpArrayInt( new int[] { 4 , -2 , -3 } );

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
            var value1 = new SexpArrayDouble();
            var value2 = new SexpArrayDouble( -7.7 );
            var value3 = new SexpArrayDouble( new[] { -1.2 , -6.4 , -3.3 } );
            var value4 = new SexpArrayDouble( new[] { 1.2 , double.NaN , -3.3 } );

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
                var value1A = service.RConnection[ "matrix( c( 1.1 , 2.6 , 3.9 , -4.2 , NA , 6.3 ) , ncol = 3 , byrow = TRUE )" ];
                var value1B = new SexpArrayDouble( new[] { 1.1 , -4.2 , 2.6 , double.NaN , 3.9 , 6.3 } );

                var value2A = service.RConnection[ "c( -4.4 , -2.2 , -1 )" ];
                var value2B = new SexpArrayDouble( new[] { -4.4 , -2.2 , -1 } );

                var value3A = service.RConnection[ "-.9" ];
                var value3B = new SexpArrayDouble( -.9 );

                var value4A = service.RConnection[ "numeric()" ];
                var value4B = new SexpArrayDouble();

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
                var value1A = service.RConnection[ "matrix( c( 1.1 , 2.6 , NA , -4.2 , -5.8 , 6.3 ) , ncol = 3 , byrow = TRUE )" ];
                var value1B = new SexpArrayDouble( new[] { 1.1 , -4.2 , 2.6 , -5.8 , double.NaN , 6.3 } );

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

        [Fact]
        public void IsNa_NaValueReadFromR_ReturnTrue()
        {
            using ( var rWrapper = new Rservice() )
            {
                // Arrange & Act
                Sexp naSexp = rWrapper.RConnection[ "as.numeric( NA )" ];

                // Assert
                Assert.IsType<SexpArrayDouble>( naSexp );
                Assert.True( naSexp.IsNa );
            }
        }

        [Fact]
        public void IsNa_NaValueInConstructor_ReturnTrue()
        {
            // Arrange
            var sexp1 = new SexpArrayDouble( double.NaN );
            var sexp2 = new SexpArrayDouble( new[] { double.NaN } );

            // Act & Assert
            Assert.True( sexp1.IsNa );
            Assert.True( sexp2.IsNa );
        }

        [Fact]
        public void IsNa_NotNa_ReturnFalse()
        {
            // Arrange
            var sexp1 = new SexpArrayDouble( 3.4 );
            var sexp2 = new SexpArrayDouble( new[] { -8.3 } );

            // Act & Assert
            Assert.False( sexp1.IsNa );
            Assert.False( sexp2.IsNa );
        }

        [Fact]
        public void IsNa_SexpContainsZeroOrMultipleValues_ThrowsNotSupportedException()
        {
            // Arrange
            var sexp1 = new SexpArrayDouble();
            var sexp2 = new SexpArrayDouble( new double[] { } );
            var sexp3 = new SexpArrayDouble( new[] { double.NaN , 4.4 } );
            var sexp4 = new SexpArrayDouble( new[] { 4.4 , -9.4 } );

            // Act & Assert
            Assert.Throws<NotSupportedException>( () => sexp1.IsNa );
            Assert.Throws<NotSupportedException>( () => sexp2.IsNa );
            Assert.Throws<NotSupportedException>( () => sexp3.IsNa );
            Assert.Throws<NotSupportedException>( () => sexp4.IsNa );
        }

        [Fact]
        public void Various_LinearAlgebraFunctionsOf2DArrayDouble_MeetsExpectation()
        {
            // note: this test was ported from RserveCLI.  Not in the typical Arrange/Act/Assert format

            using ( var service = new Rservice() )
            {
                // Same as for integers -- we'll divide by two to get floating point values that aren't integers
                var matA = new double[ , ] { { 14 , 9 , 3 } , { 2 , 11 , 15 } , { 0 , 12 , 17 } , { 5 , 2 , 3 } };
                var matB = new double[ , ] { { 12 , 25 } , { 9 , 10 } , { 8 , 5 } };
                var matC = new double[ , ] { { 273 , 455 } , { 243 , 235 } , { 244 , 205 } , { 102 , 160 } };
                var sexpA = Sexp.Make( matA );
                service.RConnection[ "a" ] = sexpA;
                service.RConnection[ "b" ] = Sexp.Make( matB );

                // Some simple tests with A
                for ( int i = 0 ; i <= 1 ; i++ )
                {
                    Assert.Equal( matA.GetLength( i ) , sexpA.GetLength( i ) );
                    Assert.Equal( matA.GetLength( i ) , service.RConnection[ "a" ].GetLength( i ) );
                }

                for ( var row = 0 ; row < matA.GetLength( 0 ) ; row++ )
                {
                    for ( var col = 0 ; col < matA.GetLength( 1 ) ; col++ )
                    {
                        Assert.Equal( matA[ row , col ] , sexpA[ row , col ].AsDouble );
                        Assert.Equal( matA[ row , col ] , service.RConnection[ "a" ][ row , col ].AsDouble );
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
                        Assert.Equal( matC[ row , col ] , matD[ row , col ].AsDouble );
                    }
                }
            }
        }

        /// <summary>
        /// Tests SexpArrayDouble.
        /// </summary>
        [Fact]
        public void Various_ArrayDoubleTest()
        {

            // note: this test was ported from RserveCLI.  Not in the typical Arrange/Act/Assert format
            const double tol = 1e-15;
            using ( var service = new Rservice() )
            {
                var testDoubles = new[] { -3.5 , 0.0 , 1.0 , 2.0 , 1.0E20 , double.NaN , double.NaN };
                var x1 = Sexp.Make( testDoubles );
                x1[ x1.Count - 1 ] = new SexpArrayDouble( SexpArrayDouble.Na );

                for ( int i = 0 ; i < x1.Count ; i++ )
                {
                    Assert.Equal( testDoubles[ i ] , x1[ i ].AsDouble );
                }

                service.RConnection.Eval( "x2 <- as.numeric(c(-3.5,0,1,2,1E20,NaN,NA))" );
                var x2 = service.RConnection[ "x2" ];

                Assert.Equal( x1.Count , x2.Count );

                for ( int i = 0 ; i < x1.Count ; i++ )
                {
                    if ( x1[ i ].IsNa )
                    {
                        Assert.True( x2[ i ].IsNa );
                    }
                    else if ( x1[ i ].IsNull )
                    {
                        Assert.True( x2[ i ].IsNull );
                    }
                    else if ( double.IsNaN( x1[ i ].AsDouble ) )
                    {
                        Assert.True( double.IsNaN( x2[ i ].AsDouble ) );
                    }
                    else
                    {
                        var res = Math.Abs( x1[ i ].AsDouble - x2[ i ].AsDouble ) < tol;
                        Assert.True( res );
                    }
                }

                service.RConnection[ "x1" ] = x1;
                var equals = service.RConnection[ "x1 == x2" ];

                Assert.Equal( x1.Count , equals.Count );

                for ( int i = 0 ; i < x1.Count ; i++ )
                {
                    if ( !double.IsNaN( x1[ i ].AsDouble ) )
                    {
                        Assert.True( equals[ i ].AsBool , equals.ToString() );
                    }
                }

                Assert.Equal( x1.IndexOf( new SexpArrayDouble( 1.0 ) ) , 2 );

                x1.AsList[ 0 ] = -5.5;
                Assert.Equal( x1[ 0 ].AsDouble , -5.5 );
            }
        }

        [Fact]
        public void Various_DoubleTest()
        {
            // note: this test was ported from RserveCLI.  Not in the typical Arrange/Act/Assert format
            var zero = new SexpArrayDouble( 0.0 );
            var one = new SexpArrayDouble( 1.0 );
            var na = new SexpArrayDouble( SexpArrayDouble.Na );

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
            Assert.True( zero.AsDouble == 0.0 );
            Assert.True( one.AsDouble == 1.0 );
            Assert.True( zero.AsInt == 0 );
            Assert.True( one.AsInt == 1 );
            Assert.True( double.IsNaN( na.AsDouble ) );

            foreach ( var a in new Sexp[] { zero , one , na } )
            {
                Assert.True( !a.Equals( new SexpNull() ) );
            }
        }


    }
}
