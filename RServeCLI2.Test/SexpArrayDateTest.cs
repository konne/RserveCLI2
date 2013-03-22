using System;
using Xunit;

namespace RserveCLI2.Tests
{
    public class SexpArrayDateTest
    {

        [Fact]
        public void Constructor_AllOverloads_ProperlyStoresDatesAsInts()
        {

            // Arrange
            var dates2A = new DateTime( 1970 , 1 , 1 );
            var dates2B = new DateTime( 1970 , 1 , 2 );
            var dates2C = new DateTime( 1969 , 12 , 31 );
            var dates2D = new DateTime( 2012 , 10 , 12 );
            var dates2E = new DateTime( 1953 , 10 , 12 );

            var dates3A = new DateTime[] { };
            var dates3B = new[] { new DateTime( 1970 , 1 , 3 ) };
            var dates3C = new[] { new DateTime( 1969 , 12 , 31 ) , new DateTime( 1970 , 1 , 1 ) , new DateTime( 1970 , 1 , 2 ) , new DateTime( 1970 , 1 , 3 ) , new DateTime( 2012 , 10 , 12 ) , new DateTime( 1953 , 10 , 12 ) };

            var dates4A = new int[] { };
            var dates4B = new[] { 0 };
            var dates4C = new[] { -1 , 0 , 1 };

            // Act
            Sexp sexp1 = new SexpArrayDate();

            Sexp sexp2A = new SexpArrayDate( dates2A );
            Sexp sexp2B = new SexpArrayDate( dates2B );
            Sexp sexp2C = new SexpArrayDate( dates2C );
            Sexp sexp2D = new SexpArrayDate( dates2D );
            Sexp sexp2E = new SexpArrayDate( dates2E );

            Sexp sexp3A = new SexpArrayDate( dates3A );
            Sexp sexp3B = new SexpArrayDate( dates3B );
            Sexp sexp3C = new SexpArrayDate( dates3C );

            Sexp sexp4A = new SexpArrayDate( dates4A );
            Sexp sexp4B = new SexpArrayDate( dates4B );
            Sexp sexp4C = new SexpArrayDate( dates4C );

            // Assert
            Assert.Equal( new int[] { } , sexp1.AsInts );

            Assert.Equal( new[] { 0 } , sexp2A.AsInts );
            Assert.Equal( new[] { 1 } , sexp2B.AsInts );
            Assert.Equal( new[] { -1 } , sexp2C.AsInts );
            Assert.Equal( new[] { 15625 } , sexp2D.AsInts );
            Assert.Equal( new[] { -5925 } , sexp2E.AsInts );

            Assert.Equal( new int[] { } , sexp3A.AsInts );
            Assert.Equal( new[] { 2 } , sexp3B.AsInts );
            Assert.Equal( new[] { -1 , 0 , 1 , 2 , 15625 , -5925 } , sexp3C.AsInts );

            Assert.Equal( dates4A , sexp4A.AsInts );
            Assert.Equal( dates4B , sexp4B.AsInts );
            Assert.Equal( dates4C , sexp4C.AsInts );

        }
        
        [Fact]
        public void AsDates_SexpConstructedUsingConstructorOrMake_ReturnsSameSetOfDates()
        {

            // Arrange
            var dates1 = new DateTime( 1953 , 10 , 12 );
            var dates2 = new[] {  new DateTime( 1970 , 1 , 1 ) , new DateTime( 1970 , 1 , 2 ) , new DateTime( 1970 , 1 , 3 ) , new DateTime( 2012 , 10 , 12 ) , new DateTime( 1953 , 10 , 12 ) };
            var dates3 = new DateTime[] { };
            var dates4 = new[] { 0 , 1 , 2 , 15625 , -5925 };

            // Act
            Sexp sexp1A = Sexp.Make( dates1 );
            Sexp sexp1B = new SexpArrayDate( dates1 );

            Sexp sexp2A = Sexp.Make( dates2 );
            Sexp sexp2B = new SexpArrayDate( dates2 );

            Sexp sexp3A = Sexp.Make( dates3 );
            Sexp sexp3B = new SexpArrayDate( dates3 );

            Sexp sexp4 = new SexpArrayDate( dates4 );
            
            // Assert
            Assert.Equal( new [] { dates1 } , sexp1A.AsDates );
            Assert.Equal( new[] { dates1 } , sexp1B.AsDates );

            Assert.Equal( dates2 , sexp2A.AsDates );
            Assert.Equal( dates2 , sexp2B.AsDates );

            Assert.Equal( dates3 , sexp3A.AsDates );
            Assert.Equal( dates3 , sexp3B.AsDates );

            Assert.Equal( dates2 , sexp4.AsDates );
        }
        
        [Fact]
        public void AsDates_SexpConstructedFromR_ReturnsSameSetOfDates()
        {
            using ( var service = new Rservice() )
            {

                // Arrange & Act
                Sexp sexp1 = service.RConnection[ "as.Date( '2012-01-01' )" ];
                Sexp sexp2 = service.RConnection[ "as.Date( c( '2012-01-01' , '1970-01-01' , '1950-06-08' ) )" ];
                service.RConnection.VoidEval( "test = as.Date( c( '2012-01-01' , '1970-01-01' , '1950-06-08' ) )" );
                service.RConnection.VoidEval( "mode( test ) = 'integer'" );
                Sexp sexp3 = service.RConnection[ "test" ];

                // Assert
                Assert.Equal( new[] { new DateTime( 2012 , 01 , 01 ) } , sexp1.AsDates );
                Assert.Equal( new[] { new DateTime( 2012 , 01 , 01 ) , new DateTime( 1970 , 01 , 01 ) , new DateTime( 1950 , 06 , 08 ) } , sexp2.AsDates );
                Assert.Equal( new[] { new DateTime( 2012 , 01 , 01 ) , new DateTime( 1970 , 01 , 01 ) , new DateTime( 1950 , 06 , 08 ) } , sexp3.AsDates );
            }
        }        

    }
}
