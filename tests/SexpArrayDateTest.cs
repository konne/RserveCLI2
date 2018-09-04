//-----------------------------------------------------------------------
// Copyright (c) 2013, Suraj Gupta
// All rights reserved.
//-----------------------------------------------------------------------

using System;
using Xunit;

namespace RserveCLI2.Test
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
        public void AsDate_ArrayContainsOneDate_ReturnsTheDate()
        {

            // Arrange
            var day = new DateTime( 2012 , 02 , 23 );
            Sexp sexp1 = new SexpArrayDate( day );
            Sexp sexp2 = new SexpArrayDate( new [] { day } );

            // Act & Assert
            Assert.Equal( day , sexp1.AsDate );
            Assert.Equal( day , sexp2.AsDate );
        }

        [Fact]
        public void AsDate_ArrayContainsNoDatesOrMoreThanOneDate_ThrowsNotSupportedExeption()
        {
            // Arrange
            var day1 = new DateTime( 2012 , 02 , 23 );
            var day2 = new DateTime( 2012 , 04 , 23 );
            Sexp sexp1 = new SexpArrayDate( );
            Sexp sexp2 = new SexpArrayDate( new DateTime[] { } );
            Sexp sexp3 = new SexpArrayDate( new [] { day1 , day2 } );

            // Act & Assert
            Assert.Throws<NotSupportedException>( () => sexp1.AsDate );
            Assert.Throws<NotSupportedException>( () => sexp2.AsDate );
            Assert.Throws<NotSupportedException>( () => sexp3.AsDate );
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

        [Fact]
        public void Indexer_Get_ReturnsSexpWithExpectedDate()
        {            
            // Arrange            
            var sexp1 = new SexpArrayDate( new DateTime( 2004 , 01 , 23 ) );
            var sexp2 = new SexpArrayDate( new [] { new DateTime( 2004 , 01 , 23 ) , new DateTime( 2005 , 01 , 23 ) , new DateTime( 2007 , 03 , 12 ) } );

            // Act & Assert
            Assert.Equal( new DateTime( 2004 , 01 , 23 ) , sexp1[ 0 ].AsDate );
            Assert.Equal( new DateTime( 2004 , 01 , 23 ) , sexp2[ 0 ].AsDate );
            Assert.Equal( new DateTime( 2005 , 01 , 23 ) , sexp2[ 1 ].AsDate );
            Assert.Equal( new DateTime( 2007 , 03 , 12 ) , sexp2[ 2 ].AsDate );            
        }

        [Fact]
        public void Indexer_GetIndexOutOfBounds_ThrowsArgumentOutOfRangeException()
        {
            // Arrange            
            var sexp1 = new SexpArrayDate( new DateTime( 2004 , 01 , 23 ) );
            var sexp2 = new SexpArrayDate( new[] { new DateTime( 2004 , 01 , 23 ) , new DateTime( 2005 , 01 , 23 ) , new DateTime( 2007 , 03 , 12 ) } );

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>( () => sexp1[ 2 ] );
            Assert.Throws<ArgumentOutOfRangeException>( () => sexp2[ 4 ] );
        }

        [Fact]
        public void Indexer_Set_ReplacesExistingDate()
        {
            // Arrange            
            var sexp1 = new SexpArrayDate( new DateTime( 2004 , 01 , 23 ) );
            var sexp2A = new SexpArrayDate( new[] { new DateTime( 2004 , 01 , 23 ) , new DateTime( 2005 , 01 , 23 ) , new DateTime( 2007 , 03 , 12 ) } );
            var sexp2B = new SexpArrayDate( new[] { new DateTime( 2004 , 01 , 23 ) , new DateTime( 2005 , 01 , 23 ) , new DateTime( 2007 , 03 , 12 ) } );

            // Act
            sexp1[ 0 ] = new SexpArrayDate( new DateTime( 2005 , 02 , 03 ) );
            sexp2A[ 1 ] = new SexpArrayDate( new DateTime( 2005 , 02 , 03 ) );
            sexp2B[ 2 ] = new SexpArrayDate( new DateTime( 2007 , 02 , 03 ) );

            // Assert
            Assert.Equal( new DateTime( 2005 , 02 , 03 ) , sexp1.AsDate );
            Assert.Equal( new[] { new DateTime( 2004 , 01 , 23 ) , new DateTime( 2005 , 02 , 03 ) , new DateTime( 2007 , 03 , 12 ) } , sexp2A.AsDates );
            Assert.Equal( new[] { new DateTime( 2004 , 01 , 23 ) , new DateTime( 2005 , 01 , 23 ) , new DateTime( 2007 , 02 , 03 ) } , sexp2B.AsDates );            
        }

        [Fact]
        public void Indexer_SetWithNonSexpArrayDate_ThrowsNotSupportedException()
        {
            // Arrange            
            var sexp1 = new SexpArrayDate( new DateTime( 2004 , 01 , 23 ) );
            var sexp2 = new SexpArrayDate( new[] { new DateTime( 2004 , 01 , 23 ) , new DateTime( 2005 , 01 , 23 ) , new DateTime( 2007 , 03 , 12 ) } );

            // Act & Assert
            Assert.Throws<NotSupportedException>( () => sexp1[ 0 ] = new SexpArrayBool( false ) );
            Assert.Throws<NotSupportedException>( () => sexp2[ 0 ] = new SexpArrayString( "asdf" ) );            
        }

        [Fact]
        public void Indexer_SetIndexOutOfRange_ThrowsArgumentOutOfRangeException()
        {
            // Arrange            
            var sexp1 = new SexpArrayDate( new DateTime( 2004 , 01 , 23 ) );
            var sexp2 = new SexpArrayDate( new[] { new DateTime( 2004 , 01 , 23 ) , new DateTime( 2005 , 01 , 23 ) , new DateTime( 2007 , 03 , 12 ) } );

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>( () => sexp1[ 4 ] = new SexpArrayDate( new DateTime( 2005 , 02 , 03 ) ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => sexp2[ 5 ] = new SexpArrayDate( new DateTime( 2005 , 02 , 03 ) ) );
            
        }

        //[Fact]
        //public void Add_AddsDateToList()
        //{            
            
        //    // Arrange
        //    var sexp1 = new SexpArrayDate(  );
        //    var sexp2 = new SexpArrayDate( new DateTime( 2004 , 01 , 23 ) );
        //    var sexp3 = new SexpArrayDate( new[] { new DateTime( 2004 , 01 , 23 ) , new DateTime( 2005 , 01 , 23 ) , new DateTime( 2007 , 03 , 12 ) } );
            
        //    // Act
        //    sexp1.Add( new SexpArrayDate() );
        //    sexp1.Add( new SexpArrayDate( new DateTime( 2009 , 01 , 23 ) ) );
        //    sexp1.Add( new SexpArrayDate( new []{ new DateTime( 2004 , 01 , 23 ) , new DateTime( 2005 , 01 , 23 ) , new DateTime( 2007 , 03 , 12 ) } );

        //    sexp2.Add( new SexpArrayDate() );
        //    sexp2.Add( new SexpArrayDate( new DateTime( 2009 , 01 , 23 ) ) );
        //    sexp2.Add( new SexpArrayDate( new []{ new DateTime( 2004 , 01 , 23 ) , new DateTime( 2005 , 01 , 23 ) , new DateTime( 2007 , 03 , 12 ) } );

        //    sexp3.Add( new SexpArrayDate() );
        //    sexp3.Add( new SexpArrayDate( new DateTime( 2009 , 01 , 23 ) ) );
        //    sexp3.Add( new SexpArrayDate( new []{ new DateTime( 2004 , 01 , 23 ) , new DateTime( 2005 , 01 , 23 ) , new DateTime( 2007 , 03 , 12 ) } );

        //    // Assert
        //    Assert.Equal( new DateTime[]{} , sexp1 )
        //}



    }
}
