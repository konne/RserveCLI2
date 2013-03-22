using System;
using Xunit;

namespace RserveCLI2.Tests
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
        
    }
}
