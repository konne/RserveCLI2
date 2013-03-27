//-----------------------------------------------------------------------
// Copyright (c) 2013, Suraj Gupta
// All rights reserved.
//-----------------------------------------------------------------------

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
        public void RowNamesGet_MatrixCreatedInRWithRowNames_ReturnsMatrixRowNames()
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
        public void RowNamesGet_MatrixCreatedInRWithOnlyColNames_ReturnsNullRowNames()
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
        public void RowNamesGet_DataDoesNotHaveRowNames_ReturnsNull()
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
        public void RowNamesSet_DataDoesNotHaveRowNames_AddsRowNames()
        {
            using ( var service = new Rservice() )
            {

                // Arrange
                service.RConnection.VoidEval( "test1 = matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE )" );
                service.RConnection.VoidEval( "test2 = matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE , dimnames = list( NULL , c( 'ColA2' , 'ColB2' , 'ColC2' ) ) )" );
                Sexp test1 = service.RConnection[ "test1" ];
                Sexp test2 = service.RConnection[ "test2" ];

                // Act
                test1.RowNames = new[] { "RowA1" , "RowB1" };
                test2.RowNames = new[] { "RowA2" , "RowB2" };
                service.RConnection[ "test1" ] = test1;
                service.RConnection[ "test2" ] = test2;
                test1 = service.RConnection[ "test1" ];
                test2 = service.RConnection[ "test2" ];

                // Assert
                Assert.Equal( new[] { "RowA1" , "RowB1" } , test1.RowNames );
                Assert.Equal( new[] { "RowA1" , "RowB1" } , service.RConnection[ "rownames( test1 )" ].AsStrings );
                Assert.Null( test1.ColNames );                
                Assert.IsType<SexpNull>( service.RConnection[ "colnames( test1 )" ] );

                Assert.Equal( new[] { "RowA2" , "RowB2" } , test2.RowNames );
                Assert.Equal( new[] { "RowA2" , "RowB2" } , service.RConnection[ "rownames( test2 )" ].AsStrings );
                Assert.Equal( new[] { "ColA2" , "ColB2" , "ColC2" } , test2.ColNames );                
                Assert.Equal( new[] { "ColA2" , "ColB2" , "ColC2" } , service.RConnection[ "colnames( test2 )" ].AsStrings );
            }
        }

        [Fact]
        public void RowNamesSet_DataHasRowNames_OverwriteRowsNames()
        {
            using ( var service = new Rservice() )
            {

                // Arrange
                service.RConnection.VoidEval( "test1 = matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE , dimnames = list( c( 'RowA1' , 'RowB1' ) , NULL ) )" );
                service.RConnection.VoidEval( "test2 = matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE , dimnames = list( c( 'RowA2' , 'RowB2' ) , c( 'ColA2' , 'ColB2' , 'ColC2' ) ) )" );
                Sexp test1 = service.RConnection[ "test1" ];
                Sexp test2 = service.RConnection[ "test2" ];

                // Act
                test1.RowNames = new[] { "RowA3" , "RowB3" };
                test2.RowNames = new[] { "RowA4" , "RowB4" };
                service.RConnection[ "test1" ] = test1;
                service.RConnection[ "test2" ] = test2;
                test1 = service.RConnection[ "test1" ];
                test2 = service.RConnection[ "test2" ];

                // Assert
                Assert.Equal( new[] { "RowA3" , "RowB3" } , test1.RowNames );
                Assert.Equal( new[] { "RowA3" , "RowB3" } , service.RConnection[ "rownames( test1 )" ].AsStrings );
                Assert.Null( test1.ColNames );
                Assert.IsType<SexpNull>( service.RConnection[ "colnames( test1 )" ] );
                
                Assert.Equal( new[] { "RowA4" , "RowB4" } , test2.RowNames );
                Assert.Equal( new[] { "RowA4" , "RowB4" } , service.RConnection[ "rownames( test2 )" ].AsStrings );
                Assert.Equal( new[] { "ColA2" , "ColB2" , "ColC2" } , test2.ColNames );
                Assert.Equal( new[] { "ColA2" , "ColB2" , "ColC2" } , service.RConnection[ "colnames( test2 )" ].AsStrings );
            }
        }

        [Fact]
        public void RowNamesSet_SexpDoesNotHaveDimAttribute_ThrowsNotSupportedException()
        {
            using ( var service = new Rservice() )
            {

                // Arrange
                Sexp test1 = new SexpArrayDouble();
                Sexp test2 = service.RConnection[ "c( 1 , 2 , 3 )" ];

                // Act & Assert
                Assert.Throws<NotSupportedException>( () => test1.RowNames = new[] { "RowA" } );
                Assert.Throws<NotSupportedException>( () => test2.RowNames = new[] { "RowA" } );
            }
        }

        [Fact]
        public void RowNamesSet_LengthOfRowNamesDoesNotMatchNumberOfRows_ThrowsNotSupportedException()
        {
            using ( var service = new Rservice() )
            {

                // Arrange
                Sexp test1 = service.RConnection[ "matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE )" ];
                Sexp test2 = service.RConnection[ "matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE , dimnames = list( c( 'RowA2' , 'RowB2' ) , c( 'ColA2' , 'ColB2' , 'ColC2' ) ) )" ];

                // Act & Assert
                Assert.Throws<NotSupportedException>( () => test1.RowNames = new[] { "RowA" } );
                Assert.Throws<NotSupportedException>( () => test1.RowNames = new[] { "RowA" , "RowB" , "RowC" } );
                Assert.Throws<NotSupportedException>( () => test2.RowNames = new[] { "RowA" } );
                Assert.Throws<NotSupportedException>( () => test2.RowNames = new[] { "RowA" , "RowB" , "RowC" } );
            }
        }

        [Fact]
        public void RowNamesSet_SetToNull_ClearsRowNamesInSexp()
        {
            using ( var service = new Rservice() )
            {

                // Arrange
                service.RConnection.VoidEval( "test1 = matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE )" );
                service.RConnection.VoidEval( "test2 = matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE , dimnames = list( c( 'RowA2' , 'RowB2' ) , c( 'ColA2' , 'ColB2' , 'ColC2' ) ) )" );
                service.RConnection.VoidEval( "test3 = matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE , dimnames = list( c( 'RowA3' , 'RowB3' ) , NULL ) )" );
                service.RConnection.VoidEval( "test4 = matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE , dimnames = list( NULL , c( 'ColA4' , 'ColB4' , 'ColC4' ) ) )" );

                Sexp test1 = service.RConnection[ "test1" ];
                Sexp test2 = service.RConnection[ "test2" ];
                Sexp test3 = service.RConnection[ "test3" ];
                Sexp test4 = service.RConnection[ "test4" ];

                // Act
                test1.RowNames = null;
                test2.RowNames = null;
                test3.RowNames = null;
                test4.RowNames = null;

                // Assert
                Assert.Null( test1.RowNames );
                Assert.Null( test2.RowNames );
                Assert.Null( test3.RowNames );
                Assert.Null( test4.RowNames );

                Assert.Null( test1.ColNames );
                Assert.Equal( new[] { "ColA2" , "ColB2" , "ColC2" } , test2.ColNames );
                Assert.Null( test3.ColNames );
                Assert.Equal( new[] { "ColA4" , "ColB4" , "ColC4" } , test4.ColNames );
            }
        }

        [Fact]
        public void RowNamesSet_SetToNull_ClearsRowNamesInR()
        {
            using ( var service = new Rservice() )
            {

                // Arrange
                service.RConnection.VoidEval( "test1 = matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE )" );
                service.RConnection.VoidEval( "test2 = matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE , dimnames = list( c( 'RowA2' , 'RowB2' ) , c( 'ColA2' , 'ColB2' , 'ColC2' ) ) )" );
                service.RConnection.VoidEval( "test3 = matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE , dimnames = list( c( 'RowA3' , 'RowB3' ) , NULL ) )" );
                service.RConnection.VoidEval( "test4 = matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE , dimnames = list( NULL , c( 'ColA4' , 'ColB4' , 'ColC4' ) ) )" );

                Sexp test1 = service.RConnection[ "test1" ];
                Sexp test2 = service.RConnection[ "test2" ];
                Sexp test3 = service.RConnection[ "test3" ];
                Sexp test4 = service.RConnection[ "test4" ];

                // Act
                test1.RowNames = null;
                test2.RowNames = null;
                test3.RowNames = null;
                test4.RowNames = null;
                service.RConnection[ "test1" ] = test1;
                service.RConnection[ "test2" ] = test2;
                service.RConnection[ "test3" ] = test3;
                service.RConnection[ "test4" ] = test4;

                // Assert
                Assert.IsType<SexpNull>( service.RConnection[ "rownames(test1)" ] );
                Assert.IsType<SexpNull>( service.RConnection[ "rownames(test2)" ] );
                Assert.IsType<SexpNull>( service.RConnection[ "rownames(test3)" ] );
                Assert.IsType<SexpNull>( service.RConnection[ "rownames(test4)" ] );

                Assert.IsType<SexpNull>( service.RConnection[ "colnames(test1)" ] );
                Assert.Equal( new[] { "ColA2" , "ColB2" , "ColC2" } , service.RConnection[ "colnames(test2)" ].AsStrings );
                Assert.IsType<SexpNull>( service.RConnection[ "colnames(test3)" ] );
                Assert.Equal( new[] { "ColA4" , "ColB4" , "ColC4" } , service.RConnection[ "colnames(test4)" ].AsStrings );

            }
        }

        [Fact]
        public void ColNamesGet_MatrixCreatedInRWithColNames_ReturnsMatrixColNames()
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
        public void ColNamesGet_MatrixCreatedInRWithOnlyRowNames_ReturnsNullColNames()
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
        public void ColNamesGet_DataDoesNotHaveColNames_ReturnsNull()
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
        public void ColNamesSet_DataDoesNotHaveColNames_AddsColNames()
        {
            using ( var service = new Rservice() )
            {

                // Arrange
                service.RConnection.VoidEval( "test1 = matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE )" );
                service.RConnection.VoidEval( "test2 = matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE , dimnames = list( c( 'RowA2' , 'RowB2' ) , NULL ) )" );
                Sexp test1 = service.RConnection[ "test1" ];
                Sexp test2 = service.RConnection[ "test2" ];

                // Act
                test1.ColNames = new[] { "ColA1" , "ColB1" , "ColC1" };
                test2.ColNames = new[] { "ColA2" , "ColB2" , "ColC2" };
                service.RConnection[ "test1" ] = test1;
                service.RConnection[ "test2" ] = test2;
                test1 = service.RConnection[ "test1" ];
                test2 = service.RConnection[ "test2" ];

                // Assert
                Assert.Null( test1.RowNames );
                Assert.IsType<SexpNull>( service.RConnection[ "rownames( test1 )" ] );
                Assert.Equal( new[] { "ColA1" , "ColB1" , "ColC1" } , test1.ColNames );
                Assert.Equal( new[] { "ColA1" , "ColB1" , "ColC1" } , service.RConnection[ "colnames(test1)" ].AsStrings );

                Assert.Equal( new[] { "RowA2" , "RowB2" } , test2.RowNames );
                Assert.Equal( new[] { "RowA2" , "RowB2" } , service.RConnection[ "rownames(test2)" ].AsStrings );
                Assert.Equal( new[] { "ColA2" , "ColB2" , "ColC2" } , test2.ColNames );
                Assert.Equal( new[] { "ColA2" , "ColB2" , "ColC2" } , service.RConnection[ "colnames(test2)" ].AsStrings );

            }
        }

        [Fact]
        public void ColNamesSet_DataHasColNames_OverwriteColNames()
        {
            using ( var service = new Rservice() )
            {

                // Arrange
                service.RConnection.VoidEval( "test1 = matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE , dimnames = list( NULL , c( 'ColA1' , 'ColB1' , 'ColC1' ) ) )" );
                service.RConnection.VoidEval( "test2 = matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE , dimnames = list( c( 'RowA2' , 'RowB2' ) , c( 'ColA2' , 'ColB2' , 'ColC2' ) ) )" );
                Sexp test1 = service.RConnection[ "test1" ];
                Sexp test2 = service.RConnection[ "test2" ];

                // Act
                test1.ColNames = new[] { "ColA3" , "ColB3" , "ColC3" };
                test2.ColNames = new[] { "ColA4" , "ColB4" , "ColC4" };
                service.RConnection[ "test1" ] = test1;
                service.RConnection[ "test2" ] = test2;
                test1 = service.RConnection[ "test1" ];
                test2 = service.RConnection[ "test2" ];

                // Assert
                Assert.Null( test1.RowNames );
                Assert.IsType<SexpNull>( service.RConnection[ "rownames( test1 )" ] );
                Assert.Equal( new[] { "ColA3" , "ColB3" , "ColC3" } , test1.ColNames );
                Assert.Equal( new[] { "ColA3" , "ColB3" , "ColC3" } , service.RConnection[ "colnames( test1 )" ].AsStrings );

                Assert.Equal( new[] { "RowA2" , "RowB2" } , test2.RowNames );
                Assert.Equal( new[] { "RowA2" , "RowB2" } , service.RConnection[ "rownames( test2 )" ].AsStrings );
                Assert.Equal( new[] { "ColA4" , "ColB4" , "ColC4" } , test2.ColNames );
                Assert.Equal( new[] { "ColA4" , "ColB4" , "ColC4" } , service.RConnection[ "colnames( test2 )" ].AsStrings );
            }
        }

        [Fact]
        public void ColNamesSet_SexpDoesNotHaveDimAttribute_ThrowsNotSupportedException()
        {
            using ( var service = new Rservice() )
            {

                // Arrange
                Sexp test1 = new SexpArrayDouble();
                Sexp test2 = service.RConnection[ "c( 1 , 2 , 3 )" ];

                // Act & Assert
                Assert.Throws<NotSupportedException>( () => test1.ColNames = new[] { "RowA" } );
                Assert.Throws<NotSupportedException>( () => test2.ColNames = new[] { "RowA" } );
            }
        }

        [Fact]
        public void ColNamesSet_LengthOfColNamesDoesNotMatchNumberOfCols_ThrowsNotSupportedException()
        {
            using ( var service = new Rservice() )
            {

                // Arrange
                Sexp test1 = service.RConnection[ "matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE )" ];
                Sexp test2 = service.RConnection[ "matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE , dimnames = list( c( 'RowA2' , 'RowB2' ) , c( 'ColA2' , 'ColB2' , 'ColC2' ) ) )" ];

                // Act & Assert
                Assert.Throws<NotSupportedException>( () => test1.ColNames = new[] { "ColA" } );
                Assert.Throws<NotSupportedException>( () => test1.ColNames = new[] { "ColA" , "ColB" , "ColC" , "ColD" } );
                Assert.Throws<NotSupportedException>( () => test2.ColNames = new[] { "ColA" } );
                Assert.Throws<NotSupportedException>( () => test2.ColNames = new[] { "ColA" , "ColB" , "ColC" , "ColD" } );
            }
        }

        [Fact]
        public void ColNamesSet_SetToNull_ClearsColNamesInSexp()
        {
            using ( var service = new Rservice() )
            {

                // Arrange
                service.RConnection.VoidEval( "test1 = matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE )" );
                service.RConnection.VoidEval( "test2 = matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE , dimnames = list( c( 'RowA2' , 'RowB2' ) , c( 'ColA2' , 'ColB2' , 'ColC2' ) ) )" );
                service.RConnection.VoidEval( "test3 = matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE , dimnames = list( c( 'RowA3' , 'RowB3' ) , NULL ) )" );
                service.RConnection.VoidEval( "test4 = matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE , dimnames = list( NULL , c( 'ColA4' , 'ColB4' , 'ColC4' ) ) )" );

                Sexp test1 = service.RConnection[ "test1" ];
                Sexp test2 = service.RConnection[ "test2" ];
                Sexp test3 = service.RConnection[ "test3" ];
                Sexp test4 = service.RConnection[ "test4" ];

                // Act
                test1.ColNames = null;
                test2.ColNames = null;
                test3.ColNames = null;
                test4.ColNames = null;

                // Assert
                Assert.Null( test1.ColNames );
                Assert.Null( test2.ColNames );
                Assert.Null( test3.ColNames );
                Assert.Null( test4.ColNames );

                Assert.Null( test1.RowNames );
                Assert.Equal( new[] { "RowA2" , "RowB2" } , test2.RowNames );
                Assert.Equal( new[] { "RowA3" , "RowB3" } , test3.RowNames );
                Assert.Null( test4.RowNames );
                
            }
        }

        [Fact]
        public void ColNamesSet_SetToNull_ClearsColNamesInR()
        {
            using ( var service = new Rservice() )
            {

                // Arrange
                service.RConnection.VoidEval( "test1 = matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE )" );
                service.RConnection.VoidEval( "test2 = matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE , dimnames = list( c( 'RowA2' , 'RowB2' ) , c( 'ColA2' , 'ColB2' , 'ColC2' ) ) )" );
                service.RConnection.VoidEval( "test3 = matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE , dimnames = list( c( 'RowA3' , 'RowB3' ) , NULL ) )" );
                service.RConnection.VoidEval( "test4 = matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , byrow = TRUE , dimnames = list( NULL , c( 'ColA4' , 'ColB4' , 'ColC4' ) ) )" );

                Sexp test1 = service.RConnection[ "test1" ];
                Sexp test2 = service.RConnection[ "test2" ];
                Sexp test3 = service.RConnection[ "test3" ];
                Sexp test4 = service.RConnection[ "test4" ];

                // Act
                test1.ColNames = null;
                test2.ColNames = null;
                test3.ColNames = null;
                test4.ColNames = null;
                service.RConnection[ "test1" ] = test1;
                service.RConnection[ "test2" ] = test2;
                service.RConnection[ "test3" ] = test3;
                service.RConnection[ "test4" ] = test4;

                // Assert
                Assert.IsType<SexpNull>( service.RConnection[ "colnames(test1)" ] );
                Assert.IsType<SexpNull>( service.RConnection[ "colnames(test2)" ] );
                Assert.IsType<SexpNull>( service.RConnection[ "colnames(test3)" ] );
                Assert.IsType<SexpNull>( service.RConnection[ "colnames(test4)" ] );

                Assert.IsType<SexpNull>( service.RConnection[ "rownames(test1)" ] );
                Assert.Equal( new[] { "RowA2" , "RowB2" } , service.RConnection[ "rownames(test2)" ].AsStrings );
                Assert.Equal( new[] { "RowA3" , "RowB3" } , service.RConnection[ "rownames(test3)" ].AsStrings );
                Assert.IsType<SexpNull>( service.RConnection[ "rownames(test4)" ] );                
            }
        }

        [Fact]
        public void NamesGet_VectorHasNoNames_ReturnsNull()
        {
            using ( var service = new Rservice() )
            {

                // Arrange
                Sexp test1 = service.RConnection[ "c( 1 , 2 , 3 , 4 )" ];
                Sexp test2 = service.RConnection[ "numeric()" ];
                Sexp test3 = service.RConnection[ "c( TRUE , NA , FALSE )" ];
                Sexp test4 = service.RConnection[ "'abcde'" ];

                // Act & Assert
                Assert.Null( test1.Names );
                Assert.Null( test2.Names );
                Assert.Null( test3.Names );
                Assert.Null( test4.Names );               
            }
        }

        [Fact]
        public void NamesGet_Matrix_ReturnsNull()
        {
            using ( var service = new Rservice() )
            {

                // Arrange
                Sexp test1 = service.RConnection[ "matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 )" ];
                Sexp test2 = service.RConnection[ "matrix( c( 1 , 2 , 3 , 4 , 5 , 6 ) , nrow = 2 , dimnames = list( c( 'Row1' , 'Row2' ) , c( 'Col1' , 'Col2' , 'Col3' ) ) )" ];

                // Act & Assert
                Assert.Null( test1.Names );
                Assert.Null( test2.Names );
            }
        }

        [Fact]
        public void NamesGet_VectorHasNames_ReturnsNames()
        {
            using ( var service = new Rservice() )
            {

                // Arrange
                Sexp test1 = service.RConnection[ "c( A = 1 , B = 2 , C = 3 , D = 4 )" ];
                Sexp test2 = service.RConnection[ "c( Double1 = 1.1 , Double2 = 2.2 , Double3 = 3.3 , Double4 = 4.4 )" ];
                Sexp test3 = service.RConnection[ "c( First = TRUE , Second = NA , Third = FALSE )" ];
                Sexp test4 = service.RConnection[ "c( Alfred = 'Hitchcock' )" ];
                service.RConnection.VoidEval( "test5 = c( 1 , 2 ); names( test5 ) = c( 'Go' , 'Fast' )" );
                Sexp test5 = service.RConnection[ "test5" ];
                
                // Act & Assert
                Assert.Equal( new[] { "A" , "B" , "C" , "D" } , test1.Names );
                Assert.Equal( new[] { "Double1" , "Double2" , "Double3" , "Double4" } , test2.Names );
                Assert.Equal( new[] { "First" , "Second" , "Third" } , test3.Names );
                Assert.Equal( new[] { "Alfred" } , test4.Names );
                Assert.Equal( new[] { "Go" , "Fast" } , test5.Names );
            }
        }

        [Fact]
        public void NamesSet_SetToNull_ClearsNames()
        {
            using ( var service = new Rservice() )
            {

                // Arrange
                Sexp test1 = service.RConnection[ "c( A = 1 , B = 2 , C = 3 , D = 4 )" ];
                Sexp test2 = service.RConnection[ "c( 1.2 , 2.1 , 3.2 , 4.4 )" ];
                
                // Act
                test1.Names = null;
                test2.Names = null;
                service.RConnection[ "test1" ] = test1;
                service.RConnection[ "test2" ] = test2;

                // Act & Assert
                Assert.Null( test1.Names );
                Assert.Null( test2.Names );
                Assert.IsType<SexpNull>( service.RConnection[ "names( test1 )" ] );
                Assert.IsType<SexpNull>( service.RConnection[ "names( test2 )" ] );

            }
        }

        [Fact]
        public void NamesSet_AppliedToMatrix_ThrowsNotSupportedException()
        {
            using ( var service = new Rservice() )
            {
                // Arrange
                Sexp test1 = service.RConnection[ "matrix( 1 : 6 , nrow = 2 )" ];
                Sexp test2 = Sexp.Make( new double[ , ] { { 2 , 3 } , { 4 , 5 } } );

                // Act & Assert
                Assert.Throws<NotSupportedException>( () => test1.Names = new [] { "abcd" } );
                Assert.Throws<NotSupportedException>( () => test2.Names = new[] { "abcd" } );
            }
        }

        [Fact]
        public void NamesSet_NamesCountDoesNotMatchValuesCount_ThrowsNotSupportedException()
        {
            using ( var service = new Rservice() )
            {
                
                // Arrange
                Sexp test1 = service.RConnection[ "c( A = 1 , B = 2 , C = 3 , D = 4 )" ];
                Sexp test2 = service.RConnection[ "c( 1.1 , 2.2 , 3.3 , 4.4 )" ];
                Sexp test3 = service.RConnection[ "c( First = TRUE , Second = NA , Third = FALSE )" ];
                Sexp test4 = service.RConnection[ "'Hitchcock'" ];
                
                // Act & Assert
                Assert.Throws<NotSupportedException>( () => test1.Names = new string[] { } );
                Assert.Throws<NotSupportedException>( () => test1.Names = new [] { "one" , "two" , "three" } );
                Assert.Throws<NotSupportedException>( () => test1.Names = new [] { "one" , "two" , "three" , "four" , "five" } );

                Assert.Throws<NotSupportedException>( () => test2.Names = new string[] { } );
                Assert.Throws<NotSupportedException>( () => test2.Names = new[] { "one" , "two" , "three" } );
                Assert.Throws<NotSupportedException>( () => test2.Names = new[] { "one" , "two" , "three" , "four" , "five" } );
                
                Assert.Throws<NotSupportedException>( () => test3.Names = new[] { "one" } );
                Assert.Throws<NotSupportedException>( () => test3.Names = new[] { "one" , "two" } );
                Assert.Throws<NotSupportedException>( () => test3.Names = new[] { "one" , "two" , "three" , "four" } );

                Assert.Throws<NotSupportedException>( () => test4.Names = new string[] {  } );
                Assert.Throws<NotSupportedException>( () => test4.Names = new [] { "one" , "two" , "three" } );

            }
        }

        [Fact]
        public void NamesSet_VectorAlreadyContainsNames_NamesAreOverwritten()
        {
            using ( var service = new Rservice() )
            {
                // Arrange
                Sexp test1 = service.RConnection[ "c( A = 1 , B = 2 , C = 3 , D = 4 )" ];
                Sexp test2 = service.RConnection[ "c( Double1 = 1.1 , Double2 = 2.2 , Double3 = 3.3 , Double4 = 4.4 )" ];
                Sexp test3 = service.RConnection[ "c( First = TRUE , Second = NA , Third = FALSE )" ];
                Sexp test4 = service.RConnection[ "c( Alfred = 'Hitchcock' )" ];

                var newNames1 = new[] { "E" , "F" , "G" , "H" };
                var newNames2 = new[] { "D1" , "D2" , "D3" , "D4" };
                var newNames3 = new[] { "Fourth" , "Fifth" , "Sixth" };
                var newNames4 = new[] { "Al" };

                // Act
                test1.Names = newNames1;
                test2.Names = newNames2;
                test3.Names = newNames3;
                test4.Names = newNames4;
                service.RConnection[ "test1" ] = test1;
                service.RConnection[ "test2" ] = test2;
                service.RConnection[ "test3" ] = test3;
                service.RConnection[ "test4" ] = test4;
                
                // Assert
                Assert.Equal( newNames1 , test1.Names );
                Assert.Equal( newNames2 , test2.Names );
                Assert.Equal( newNames3 , test3.Names );
                Assert.Equal( newNames4 , test4.Names );

                Assert.Equal( newNames1 , service.RConnection[ "names( test1 )" ].AsStrings );
                Assert.Equal( newNames2 , service.RConnection[ "names( test2 )" ].AsStrings );
                Assert.Equal( newNames3 , service.RConnection[ "names( test3 )" ].AsStrings );
                Assert.Equal( newNames4 , service.RConnection[ "names( test4 )" ].AsStrings );
            }
        }

        [Fact]
        public void NamesSet_VectorDoesNotOriginallyContainNames_NamesAreAddedToVector()
        {
            using ( var service = new Rservice() )
            {
                // Arrange
                Sexp test1 = service.RConnection[ "c( 1 , 2 , 3 , 4 )" ];
                Sexp test2 = service.RConnection[ "c( 1.1 , 2.2 , 3.3 , 4.4 )" ];
                Sexp test3 = service.RConnection[ "c( TRUE , NA , FALSE )" ];
                Sexp test4 = service.RConnection[ "'Hitchcock'" ];

                var newNames1 = new[] { "E" , "F" , "G" , "H" };
                var newNames2 = new[] { "D1" , "D2" , "D3" , "D4" };
                var newNames3 = new[] { "Fourth" , "Fifth" , "Sixth" };
                var newNames4 = new[] { "Al" };

                // Act
                test1.Names = newNames1;
                test2.Names = newNames2;
                test3.Names = newNames3;
                test4.Names = newNames4;
                service.RConnection[ "test1" ] = test1;
                service.RConnection[ "test2" ] = test2;
                service.RConnection[ "test3" ] = test3;
                service.RConnection[ "test4" ] = test4;

                // Assert
                Assert.Equal( newNames1 , test1.Names );
                Assert.Equal( newNames2 , test2.Names );
                Assert.Equal( newNames3 , test3.Names );
                Assert.Equal( newNames4 , test4.Names );

                Assert.Equal( newNames1 , service.RConnection[ "names( test1 )" ].AsStrings );
                Assert.Equal( newNames2 , service.RConnection[ "names( test2 )" ].AsStrings );
                Assert.Equal( newNames3 , service.RConnection[ "names( test3 )" ].AsStrings );
                Assert.Equal( newNames4 , service.RConnection[ "names( test4 )" ].AsStrings );
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
            object objIEnumerableBool = new bool?[] { true , null , false };
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
            Assert.IsType<SexpArrayBool>( Sexp.Make( objBool ) );
            Assert.IsType<SexpArrayBool>( Sexp.Make( objIEnumerableBool ) );
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
            Assert.IsType<SexpArrayString>( Sexp.Make( objString ) );
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
