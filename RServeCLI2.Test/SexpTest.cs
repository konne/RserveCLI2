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


    }
}
