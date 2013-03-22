using System;
using System.Collections.Generic;
using Xunit;

namespace RserveCLI2.Test
{

    /// <summary>
    /// Tests RserveCLI2.Sexp
    /// </summary>
    public class SexpTest
    {

        [Fact]
        public void RowNames_MatrixCreatedInRWithRowNames_ReturnsMatrixRowNames()
        {
            using ( var service = new Rservice() )
            {
                // Arrange                
                service.RConnection.VoidEval( "test1 = matrix( c( 1 , 2 , 3 , -4 , -5 , 6 ) , ncol = 3 , byrow = TRUE , dimnames = list( c( 'RowA' , 'RowB' ) , NULL ) )" );
                service.RConnection.VoidEval( "test2 = matrix( c( 1 , 2 , 3 , -4 , -5 , 6 ) , ncol = 3 , byrow = TRUE , dimnames = list( c( 'RowA' , 'RowB' ) , c( 'ColA' , 'ColB' , 'ColC' ) ) )" );

                // Act
                Sexp matrix1 = service.RConnection[ "test1" ];
                Sexp matrix2 = service.RConnection[ "test2" ];

                // Assert
                Assert.Equal( new[] { "RowA" , "RowB" } , matrix1.RowNames );
                Assert.Equal( new[] { "RowA" , "RowB" } , matrix2.RowNames );

            }
        }

        [Fact]
        public void RowNames_MatrixCreatedInRWithOnlyColNames_ReturnsNullRowNames()
        {
            using ( var service = new Rservice() )
            {
                // Arrange                
                service.RConnection.VoidEval( "test1 = matrix( c( 1 , 2 , 3 , -4 , -5 , 6 ) , ncol = 3 , byrow = TRUE , dimnames = list( NULL , c( 'ColA' , 'ColB' , 'ColC' ) ) )" );
                service.RConnection.VoidEval( "test2 = matrix( c( 1 , 2 , 3 , -4 , -5 , 6 ) , ncol = 3 , byrow = TRUE ); colnames( test2 ) = c( 'ColA' , 'ColB' , 'ColC' )" );

                // Act
                Sexp matrix1 = service.RConnection[ "test1" ];
                Sexp matrix2 = service.RConnection[ "test2" ];

                // Assert
                Assert.Null( matrix1.RowNames );
                Assert.Null( matrix2.RowNames );
            }
        }

        [Fact]
        public void RowNames_DataDoesNotHaveRowNames_ReturnsNull()
        {
            using ( var service = new Rservice() )
            {
                // Arrange                
                service.RConnection.VoidEval( "test1 = matrix( c( 1 , 2 , 3 , -4 , -5 , 6 ) )" );
                service.RConnection.VoidEval( "test2 = c( 1 , 2 , 3 , -4 , -5 , 6 )" );
                service.RConnection.VoidEval( "test3 = list( A = 1 )" );

                // Act
                Sexp data1 = service.RConnection[ "test1" ];
                Sexp data2 = service.RConnection[ "test2" ];
                Sexp data3 = service.RConnection[ "test3" ];

                // Assert
                Assert.Null( data1.RowNames );
                Assert.Null( data2.RowNames );
                Assert.Null( data3.RowNames );
            }
        }

        [Fact]
        public void ColNames_MatrixCreatedInRWithColNames_ReturnsMatrixColNames()
        {
            using ( var service = new Rservice() )
            {
                // Arrange                
                service.RConnection.VoidEval( "test1 = matrix( c( 1 , 2 , 3 , -4 , -5 , 6 ) , ncol = 3 , byrow = TRUE , dimnames = list( NULL , c( 'ColA' , 'ColB' , 'ColC' ) ) )" );
                service.RConnection.VoidEval( "test2 = matrix( c( 1 , 2 , 3 , -4 , -5 , 6 ) , ncol = 3 , byrow = TRUE , dimnames = list( c( 'RowA' , 'RowB' ) , c( 'ColA' , 'ColB' , 'ColC' ) ) )" );

                // Act
                Sexp matrix1 = service.RConnection[ "test1" ];
                Sexp matrix2 = service.RConnection[ "test2" ];

                // Assert
                Assert.Equal( new[] { "ColA" , "ColB" , "ColC" } , matrix1.ColNames );
                Assert.Equal( new[] { "ColA" , "ColB" , "ColC" } , matrix2.ColNames );

            }
        }

        [Fact]
        public void ColNames_MatrixCreatedInRWithOnlyRowNames_ReturnsNullColNames()
        {
            using ( var service = new Rservice() )
            {
                // Arrange                
                service.RConnection.VoidEval( "test1 = matrix( c( 1 , 2 , 3 , -4 , -5 , 6 ) , ncol = 3 , byrow = TRUE , dimnames = list( c( 'RowA' , 'RowB' ) , NULL ) )" );
                service.RConnection.VoidEval( "test2 = matrix( c( 1 , 2 , 3 , -4 , -5 , 6 ) , ncol = 3 , byrow = TRUE ); rownames( test2 ) = c( 'RowA' , 'RowB' )" );

                // Act
                Sexp matrix1 = service.RConnection[ "test1" ];
                Sexp matrix2 = service.RConnection[ "test2" ];

                // Assert
                Assert.Null( matrix1.ColNames );
                Assert.Null( matrix2.ColNames );
            }
        }

        [Fact]
        public void ColNames_DataDoesNotHaveColNames_ReturnsNull()
        {
            using ( var service = new Rservice() )
            {
                // Arrange                
                service.RConnection.VoidEval( "test1 = matrix( c( 1 , 2 , 3 , -4 , -5 , 6 ) )" );
                service.RConnection.VoidEval( "test2 = c( 1 , 2 , 3 , -4 , -5 , 6 )" );
                service.RConnection.VoidEval( "test3 = list( A = 1 )" );

                // Act
                Sexp data1 = service.RConnection[ "test1" ];
                Sexp data2 = service.RConnection[ "test2" ];
                Sexp data3 = service.RConnection[ "test3" ];

                // Assert
                Assert.Null( data1.ColNames );
                Assert.Null( data2.ColNames );
                Assert.Null( data3.ColNames );
            }
        }

        #region Make Methods

        [Fact]
        public void Make_WithObject_ReturnsExpectedSexpType()
        {

            // Arrange

            // ReSharper disable RedundantExplicitArrayCreation
            object objSexp = new SexpTaggedList();
            object objBool = true;
            object objDouble = 4.4d;
            object objIEnumerableDouble = new[] { -5.4d , 2.3d };
            object obj2DArrayDouble = new double[ 2 , 2 ] { { -5.4d , 2.3d } , { 5.4d , -2.3d } };
            object objDecimal = 3.5m;
            object objIEnumerableDecimal = new[] { -5.4m , 2.3m };
            object obj2DArrayDecimal = new decimal[ 2 , 2 ] { { -5.4m , 2.3m } , { 5.4m , -2.3m } };
            object objint = 6;
            object objIEnumerableInt = new[] { -3 , -9 };
            object obj2DArrayInt = new int[ 2 , 2 ] { { 2 , 5 } , { 8 , 2 } };
            object objDate = new DateTime( 2011 , 1 , 1 );
            object objIEnumerableDate = new[] { new DateTime( 1969 , 12 , 31 ) , new DateTime( 1970 , 1 , 1 ) , new DateTime( 1970 , 1 , 2 ) , new DateTime( 1970 , 1 , 3 ) , new DateTime( 2012 , 10 , 12 ) , new DateTime( 1953 , 10 , 12 ) };
            object objString = "abcde";
            object objIEnumerableString = new string[] { "abcde" , "def" };
            object objDictionary = new Dictionary<string , object> { { "Test" , new SexpArrayDate() } };
            // ReSharper restore RedundantExplicitArrayCreation

            // Act & Assert
            Assert.IsType<SexpTaggedList>( Sexp.Make( objSexp ) );
            Assert.IsType<SexpBool>( Sexp.Make( objBool ) );
            Assert.IsType<SexpArrayDouble>( Sexp.Make( objDouble ) );
            Assert.IsType<SexpArrayDouble>( Sexp.Make( objIEnumerableDouble ) );
            Assert.IsType<SexpArrayDouble>( Sexp.Make( obj2DArrayDouble ) );
            Assert.IsType<SexpArrayDouble>( Sexp.Make( objDecimal ) );
            Assert.IsType<SexpArrayDouble>( Sexp.Make( objIEnumerableDecimal ) );
            Assert.IsType<SexpArrayDouble>( Sexp.Make( obj2DArrayDecimal ) );
            Assert.IsType<SexpArrayInt>( Sexp.Make( objint ) );
            Assert.IsType<SexpArrayInt>( Sexp.Make( objIEnumerableInt ) );
            Assert.IsType<SexpArrayInt>( Sexp.Make( obj2DArrayInt ) );
            Assert.IsType<SexpArrayDate>( Sexp.Make( objDate ) );
            Assert.IsType<SexpArrayDate>( Sexp.Make( objIEnumerableDate ) );
            Assert.IsType<SexpString>( Sexp.Make( objString ) );
            Assert.IsType<SexpArrayString>( Sexp.Make( objIEnumerableString ) );
            Assert.IsType<SexpList>( Sexp.Make( objDictionary ) );
            
        }



        [Fact]
        public void Make_WithDate_CreatesSexpArrayDate()
        {

            // Arrange
            var dates = new DateTime( 2012 , 10 , 12 );

            // Act
            Sexp sexp = Sexp.Make( dates );

            // Assert
            Assert.IsType<SexpArrayDate>( sexp );
        }

        [Fact]
        public void Make_WithIEnumerableOfDate_CreatesSexpArrayDate()
        {

            // Arrange
            var dates = new[] { new DateTime( 1969 , 12 , 31 ) , new DateTime( 1970 , 1 , 1 ) , new DateTime( 1970 , 1 , 2 ) , new DateTime( 1970 , 1 , 3 ) , new DateTime( 2012 , 10 , 12 ) , new DateTime( 1953 , 10 , 12 ) };

            // Act
            Sexp sexp = Sexp.Make( dates );

            // Assert
            Assert.IsType<SexpArrayDate>( sexp );
        }
        
        [Fact]
        public void Make_WithDate_ProperlyStoresDateAsInt()
        {

            // Arrange
            var dates1A = new DateTime( 1970 , 1 , 1 );
            var dates1B = new DateTime( 1970 , 1 , 2 );
            var dates1C = new DateTime( 1969 , 12 , 31 );
            var dates1D = new DateTime( 2012 , 10 , 12 );
            var dates1E = new DateTime( 1953 , 10 , 12 );

            // Act
            Sexp sexp1A = Sexp.Make( dates1A );
            Sexp sexp1B = Sexp.Make( dates1B );
            Sexp sexp1C = Sexp.Make( dates1C );
            Sexp sexp1D = Sexp.Make( dates1D );
            Sexp sexp1E = Sexp.Make( dates1E );

            // Assert
            Assert.Equal( new[] { 0 } , sexp1A.AsInts );
            Assert.Equal( new[] { 1 } , sexp1B.AsInts );
            Assert.Equal( new[] { -1 } , sexp1C.AsInts );
            Assert.Equal( new[] { 15625 } , sexp1D.AsInts );
            Assert.Equal( new[] { -5925 } , sexp1E.AsInts );
        }

        [Fact]
        public void Make_WithIEnumerableOfDates_ProperlyStoresDatesAsInts()
        {

            // Arrange
            var dates1 = new DateTime[] { };
            var dates2 = new[] { new DateTime( 1970 , 1 , 3 ) };
            var dates3 = new[] { new DateTime( 1969 , 12 , 31 ) , new DateTime( 1970 , 1 , 1 ) , new DateTime( 1970 , 1 , 2 ) , new DateTime( 1970 , 1 , 3 ) , new DateTime( 2012 , 10 , 12 ) , new DateTime( 1953 , 10 , 12 ) };

            // Act        
            Sexp sexp1 = Sexp.Make( dates1 );
            Sexp sexp2 = Sexp.Make( dates2 );
            Sexp sexp3 = Sexp.Make( dates3 );

            // Assert
            Assert.Equal( new int[] { } , sexp1.AsInts );
            Assert.Equal( new[] { 2 } , sexp2.AsInts );
            Assert.Equal( new[] { -1 , 0 , 1 , 2 , 15625 , -5925 } , sexp3.AsInts );
        }

        [Fact]
        public void Make_2dArrayOfDecimal_CreatesSexpArrayDouble()
        {
            // Arrange
            var matrix = new decimal[ 2 , 3 ] { { 3.2m , -4.7m , 5.1m } , { -6.9m , 7.4m , -8.8m } };

            // Act

            // ReSharper disable RedundantArgumentDefaultValue
            Sexp matrix1Sexp = Sexp.Make( matrix );
            Sexp matrix2Sexp = Sexp.Make( matrix , new[] { "RowA" , "RowB" } , null );
            Sexp matrix3Sexp = Sexp.Make( matrix , null , new[] { "ColA" , "ColB" , "ColC" } );
            Sexp matrix4Sexp = Sexp.Make( matrix , new[] { "RowA" , "RowB" } , new[] { "ColA" , "ColB" , "ColC" } );
            // ReSharper restore RedundantArgumentDefaultValue

            // Assert
            Assert.IsType<SexpArrayDouble>( matrix1Sexp );
            Assert.IsType<SexpArrayDouble>( matrix2Sexp );
            Assert.IsType<SexpArrayDouble>( matrix3Sexp );
            Assert.IsType<SexpArrayDouble>( matrix4Sexp );
        }

        [Fact]
        public void Make_2dArrayOfDecimalAndDouble_ProducesTheSameSexpArrayDouble()
        {
            // Arrange
            var matrixDecimal = new decimal[ 2 , 3 ] { { 3.2m , -4.7m , 5.1m } , { -6.9m , 7.4m , -8.8m } };
            var matrixDouble = new double[ 2 , 3 ] { { 3.2d , -4.7d , 5.1d } , { -6.9d , 7.4d , -8.8d } };

            // Act

            // ReSharper disable RedundantArgumentDefaultValue
            Sexp matrix1DecimalSexp = Sexp.Make( matrixDecimal );
            Sexp matrix2DecimalSexp = Sexp.Make( matrixDecimal , new[] { "RowA" , "RowB" } , null );
            Sexp matrix3DecimalSexp = Sexp.Make( matrixDecimal , null , new[] { "ColA" , "ColB" , "ColC" } );
            Sexp matrix4DecimalSexp = Sexp.Make( matrixDecimal , new[] { "RowA" , "RowB" } , new[] { "ColA" , "ColB" , "ColC" } );

            Sexp matrix1DoubleSexp = Sexp.Make( matrixDouble );
            Sexp matrix2DoubleSexp = Sexp.Make( matrixDouble , new[] { "RowA" , "RowB" } , null );
            Sexp matrix3DoubleSexp = Sexp.Make( matrixDouble , null , new[] { "ColA" , "ColB" , "ColC" } );
            Sexp matrix4DoubleSexp = Sexp.Make( matrixDouble , new[] { "RowA" , "RowB" } , new[] { "ColA" , "ColB" , "ColC" } );
            // ReSharper restore RedundantArgumentDefaultValue

            // Assert
            Assert.True( matrix1DecimalSexp.Equals( matrix1DoubleSexp ) );
            Assert.True( matrix2DecimalSexp.Equals( matrix2DoubleSexp ) );
            Assert.True( matrix3DecimalSexp.Equals( matrix3DoubleSexp ) );
            Assert.True( matrix4DecimalSexp.Equals( matrix4DoubleSexp ) );
        }
        
        [Fact]
        public void Make_2dArrayOfDoubleWithRowAndColumnNames_ProperlyAssignsRowAndColumnNames()
        {
            using ( var service = new Rservice() )
            {
                // Arrange                

                // ReSharper disable RedundantArgumentDefaultValue
                var matrixValues = new double[ 2 , 3 ] { { 3.2d , -4.7d , 5.1d } , { -6.9d , 7.4d , -8.8d } };
                var matrix1 = Sexp.Make( matrixValues , null , null );
                var matrix2 = Sexp.Make( matrixValues , new[] { "RowA" , "RowB" } , null );
                var matrix3 = Sexp.Make( matrixValues , null , new[] { "ColA" , "ColB" , "ColC" } );
                var matrix4 = Sexp.Make( matrixValues , new[] { "RowA" , "RowB" } , new[] { "ColA" , "ColB" , "ColC" } );
                // ReSharper restore RedundantArgumentDefaultValue

                // Act
                service.RConnection[ "matrix1" ] = matrix1;
                service.RConnection[ "matrix2" ] = matrix2;
                service.RConnection[ "matrix3" ] = matrix3;
                service.RConnection[ "matrix4" ] = matrix4;
                Sexp matrix1FromR = service.RConnection[ "matrix1" ];
                Sexp matrix2FromR = service.RConnection[ "matrix2" ];
                Sexp matrix3FromR = service.RConnection[ "matrix3" ];
                Sexp matrix4FromR = service.RConnection[ "matrix4" ];

                // Assert
                Assert.Null( matrix1FromR.RowNames );
                Assert.Null( matrix1FromR.ColNames );

                Assert.Equal( new[] { "RowA" , "RowB" } , matrix2FromR.RowNames );
                Assert.Null( matrix2FromR.ColNames );

                Assert.Null( matrix3FromR.RowNames );
                Assert.Equal( new[] { "ColA" , "ColB" , "ColC" } , matrix3FromR.ColNames );

                Assert.Equal( new[] { "RowA" , "RowB" } , matrix4FromR.RowNames );
                Assert.Equal( new[] { "ColA" , "ColB" , "ColC" } , matrix4FromR.ColNames );


            }
        }

        [Fact]
        public void Make_2dArrayOfDecimalWithRowAndColumnNames_ProperlyAssignsRowAndColumnNames()
        {
            using ( var service = new Rservice() )
            {
                // Arrange                

                // ReSharper disable RedundantArgumentDefaultValue
                var matrixValues = new decimal[ 2 , 3 ] { { 3.2m , -4.7m , 5.1m } , { -6.9m , 7.4m , -8.8m } };
                var matrix1 = Sexp.Make( matrixValues , null , null );
                var matrix2 = Sexp.Make( matrixValues , new[] { "RowA" , "RowB" } , null );
                var matrix3 = Sexp.Make( matrixValues , null , new[] { "ColA" , "ColB" , "ColC" } );
                var matrix4 = Sexp.Make( matrixValues , new[] { "RowA" , "RowB" } , new[] { "ColA" , "ColB" , "ColC" } );
                // ReSharper restore RedundantArgumentDefaultValue

                // Act
                service.RConnection[ "matrix1" ] = matrix1;
                service.RConnection[ "matrix2" ] = matrix2;
                service.RConnection[ "matrix3" ] = matrix3;
                service.RConnection[ "matrix4" ] = matrix4;
                Sexp matrix1FromR = service.RConnection[ "matrix1" ];
                Sexp matrix2FromR = service.RConnection[ "matrix2" ];
                Sexp matrix3FromR = service.RConnection[ "matrix3" ];
                Sexp matrix4FromR = service.RConnection[ "matrix4" ];

                // Assert
                Assert.Null( matrix1FromR.RowNames );
                Assert.Null( matrix1FromR.ColNames );

                Assert.Equal( new[] { "RowA" , "RowB" } , matrix2FromR.RowNames );
                Assert.Null( matrix2FromR.ColNames );

                Assert.Null( matrix3FromR.RowNames );
                Assert.Equal( new[] { "ColA" , "ColB" , "ColC" } , matrix3FromR.ColNames );

                Assert.Equal( new[] { "RowA" , "RowB" } , matrix4FromR.RowNames );
                Assert.Equal( new[] { "ColA" , "ColB" , "ColC" } , matrix4FromR.ColNames );


            }
        }
        
        [Fact]
        public void Make_2dArrayOfIntWithRowAndColumnNames_ProperlyAssignsRowAndColumnNames()
        {
            using ( var service = new Rservice() )
            {
                // Arrange                

                // ReSharper disable RedundantArgumentDefaultValue
                var matrixValues = new int[ 2 , 3 ] { { 3 , -4 , 5 } , { -6 , 7 , -8 } };
                var matrix1 = Sexp.Make( matrixValues , null , null );
                var matrix2 = Sexp.Make( matrixValues , new[] { "RowA" , "RowB" } , null );
                var matrix3 = Sexp.Make( matrixValues , null , new[] { "ColA" , "ColB" , "ColC" } );
                var matrix4 = Sexp.Make( matrixValues , new[] { "RowA" , "RowB" } , new[] { "ColA" , "ColB" , "ColC" } );
                // ReSharper restore RedundantArgumentDefaultValue

                // Act
                service.RConnection[ "matrix1" ] = matrix1;
                service.RConnection[ "matrix2" ] = matrix2;
                service.RConnection[ "matrix3" ] = matrix3;
                service.RConnection[ "matrix4" ] = matrix4;
                Sexp matrix1FromR = service.RConnection[ "matrix1" ];
                Sexp matrix2FromR = service.RConnection[ "matrix2" ];
                Sexp matrix3FromR = service.RConnection[ "matrix3" ];
                Sexp matrix4FromR = service.RConnection[ "matrix4" ];

                // Assert
                Assert.Null( matrix1FromR.RowNames );
                Assert.Null( matrix1FromR.ColNames );

                Assert.Equal( new[] { "RowA" , "RowB" } , matrix2FromR.RowNames );
                Assert.Null( matrix2FromR.ColNames );

                Assert.Null( matrix3FromR.RowNames );
                Assert.Equal( new[] { "ColA" , "ColB" , "ColC" } , matrix3FromR.ColNames );

                Assert.Equal( new[] { "RowA" , "RowB" } , matrix4FromR.RowNames );
                Assert.Equal( new[] { "ColA" , "ColB" , "ColC" } , matrix4FromR.ColNames );


            }
        }
        
        [Fact]
        public void Make_2dArrayOfDoubleWithWrongSizedRowAndColumnNames_ThrowsNotSupportedException()
        {
            // Arrange
            var matrixValues = new double[ 2 , 3 ] { { 3.2d , -4.7d , 5.1d } , { -6.9d , 7.4d , -8.8d } };

            // Act & Assert

            // ReSharper disable RedundantArgumentDefaultValue
            Assert.Throws<NotSupportedException>( () => Sexp.Make( matrixValues , new[] { "RowA" , "RowB" , "RowC" } , null ) );
            Assert.Throws<NotSupportedException>( () => Sexp.Make( matrixValues , new[] { "RowA" } , null ) );
            Assert.Throws<NotSupportedException>( () => Sexp.Make( matrixValues , new string[] { } , null ) );
            Assert.Throws<NotSupportedException>( () => Sexp.Make( matrixValues , null , new[] { "ColA" , "ColC" , "ColD" , "ColG" } ) );
            Assert.Throws<NotSupportedException>( () => Sexp.Make( matrixValues , null , new[] { "ColA" } ) );
            Assert.Throws<NotSupportedException>( () => Sexp.Make( matrixValues , null , new string[] { } ) );
            Assert.Throws<NotSupportedException>( () => Sexp.Make( matrixValues , new[] { "RowA" } , new[] { "ColA" , "ColB" , "ColC" , "ColD" } ) );
            // ReSharper restore RedundantArgumentDefaultValue            
        }

        [Fact]
        public void Make_2dArrayOfDecimalWithWrongSizedRowAndColumnNames_ThrowsNotSupportedException()
        {
            // Arrange
            var matrixValues = new decimal[ 2 , 3 ] { { 3.2m , -4.7m , 5.1m } , { -6.9m , 7.4m , -8.8m } };

            // Act & Assert

            // ReSharper disable RedundantArgumentDefaultValue
            Assert.Throws<NotSupportedException>( () => Sexp.Make( matrixValues , new[] { "RowA" , "RowB" , "RowC" } , null ) );
            Assert.Throws<NotSupportedException>( () => Sexp.Make( matrixValues , new[] { "RowA" } , null ) );
            Assert.Throws<NotSupportedException>( () => Sexp.Make( matrixValues , new string[] { } , null ) );
            Assert.Throws<NotSupportedException>( () => Sexp.Make( matrixValues , null , new[] { "ColA" , "ColC" , "ColD" , "ColG" } ) );
            Assert.Throws<NotSupportedException>( () => Sexp.Make( matrixValues , null , new[] { "ColA" } ) );
            Assert.Throws<NotSupportedException>( () => Sexp.Make( matrixValues , null , new string[] { } ) );
            Assert.Throws<NotSupportedException>( () => Sexp.Make( matrixValues , new[] { "RowA" } , new[] { "ColA" , "ColB" , "ColC" , "ColD" } ) );
            // ReSharper restore RedundantArgumentDefaultValue            
        }

        [Fact]
        public void Make_2dArrayOfIntWithWrongSizedRowAndColumnNames_ThrowsNotSupportedException()
        {
            // Arrange
            var matrixValues = new int[ 2 , 3 ] { { 3 , -4 , 5 } , { -6 , 7 , -8 } };

            // Act & Assert

            // ReSharper disable RedundantArgumentDefaultValue
            Assert.Throws<NotSupportedException>( () => Sexp.Make( matrixValues , new[] { "RowA" , "RowB" , "RowC" } , null ) );
            Assert.Throws<NotSupportedException>( () => Sexp.Make( matrixValues , new[] { "RowA" } , null ) );
            Assert.Throws<NotSupportedException>( () => Sexp.Make( matrixValues , new string[] { } , null ) );
            Assert.Throws<NotSupportedException>( () => Sexp.Make( matrixValues , null , new[] { "ColA" , "ColC" , "ColD" , "ColG" } ) );
            Assert.Throws<NotSupportedException>( () => Sexp.Make( matrixValues , null , new[] { "ColA" } ) );
            Assert.Throws<NotSupportedException>( () => Sexp.Make( matrixValues , null , new string[] { } ) );
            Assert.Throws<NotSupportedException>( () => Sexp.Make( matrixValues , new[] { "RowA" } , new[] { "ColA" , "ColB" , "ColC" , "ColD" } ) );
            // ReSharper restore RedundantArgumentDefaultValue            
        }

        # endregion

    }
}
