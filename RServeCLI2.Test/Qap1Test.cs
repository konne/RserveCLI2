//-----------------------------------------------------------------------
// Copyright (c) 2013, Suraj Gupta
// All rights reserved.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace RserveCLI2.Test
{
    
    public class Qap1Test
    {

        /// <remarks>
        /// Wrote this test because it wasn't clear how the protocol supports more than 255 bools in an SexpArrayBool.
        /// The protocol specifies: (1) int n (n) byte,byte,..
        /// So that's only one byte for the length of the SexpArrayBool.
        /// </remarks>
        [Fact]
        public void Command_AssignAndEvalLargeSexpArrayBool_ProperlyEncodesAndDecodesArrayOfBoolean()
        {
            using ( var service = new Rservice() )
            {
                // Arrange
                var random = new Random();

                const int count = 254975;
                var sexpBool1ToEncode = new SexpArrayBool( Enumerable.Range( 0 , count ).Select( x => { var y = random.Next( 1 , 4 ); return ( y == 1 ? true : y == 2 ? false : ( bool? )null ); } ) );
                var sexpBool2ToEncode = new SexpArrayBool( Enumerable.Range( 0 , count + 1 ).Select( x => { var y = random.Next( 1 , 4 ); return ( y == 1 ? true : y == 2 ? false : ( bool? )null ); } ) );
                var sexpBool3ToEncode = new SexpArrayBool( Enumerable.Range( 0 , count + 3 ).Select( x => { var y = random.Next( 1 , 4 ); return ( y == 1 ? true : y == 2 ? false : ( bool? )null ); } ) );
                var sexpBool4ToEncode = new SexpArrayBool( Enumerable.Range( 0 , count + 4 ).Select( x => { var y = random.Next( 1 , 4 ); return ( y == 1 ? true : y == 2 ? false : ( bool? )null ); } ) );

                // Act
                service.RConnection[ "bools1" ] = sexpBool1ToEncode;
                service.RConnection[ "bools2" ] = sexpBool2ToEncode;
                service.RConnection[ "bools3" ] = sexpBool3ToEncode;
                service.RConnection[ "bools4" ] = sexpBool4ToEncode;

                Sexp sexpBool1ToDecoded = service.RConnection[ "bools1" ];
                Sexp sexpBool2ToDecoded = service.RConnection[ "bools2" ];
                Sexp sexpBool3ToDecoded = service.RConnection[ "bools3" ];
                Sexp sexpBool4ToDecoded = service.RConnection[ "bools4" ];
                
                // Assert
                Assert.IsType<SexpArrayBool>( sexpBool1ToDecoded );
                Assert.IsType<SexpArrayBool>( sexpBool2ToDecoded );
                Assert.IsType<SexpArrayBool>( sexpBool3ToDecoded );
                Assert.IsType<SexpArrayBool>( sexpBool4ToDecoded );

                Assert.True( sexpBool1ToEncode.Values.SequenceEqual( sexpBool1ToDecoded.Values ) );
                Assert.True( sexpBool2ToEncode.Values.SequenceEqual( sexpBool2ToDecoded.Values ) );
                Assert.True( sexpBool3ToEncode.Values.SequenceEqual( sexpBool3ToDecoded.Values ) );
                Assert.True( sexpBool4ToEncode.Values.SequenceEqual( sexpBool4ToDecoded.Values ) );                
            }
        }

        /// <summary>
        /// 500,000 rows creates a situation where all columns, taken together, represent a large dataset
        /// 5,000,000 rows creates a situation where the individual columns themselves are large datasets
        /// </summary>
        [Theory]
        [InlineData( 500000 )]
        [InlineData( 5000000 )]
        public void Command_AssignLargeDataFrame_ProperlyEncodesAndSendsSexpToServer( int rowsToGenerate )
        {

            // Arrange
            var random = new Random();

            DateTime today = DateTime.Today;
            IEnumerable<DateTime> dates = Enumerable.Range( 0 , rowsToGenerate ).Select( i => today.AddHours( -i ).Date );
            var date1 = new DateTime( 2050 , 3 , 4 );
            var date2 = new DateTime( 2060 , 12 , 31 );
            var date3 = new DateTime( 2070 , 04 , 14 );
            List<DateTime> allDates = dates.Concat( new[] { date1 } ).Concat( dates ).Concat( new[] { date2 } ).Concat( dates ).Concat( new[] { date3 } ).ToList();

            List<double> doubles = Enumerable.Range( 0 , rowsToGenerate ).Select( i => random.NextDouble() ).ToList();
            const double double1 = -31.123d;
            const double double2 = -9.35d;
            const double double3 = 83.559d;
            const double double4 = -54.332d;
            List<double> allDoubles = doubles.Concat( new[] { double2 } ).Concat( doubles ).Concat( new[] { double3 } ).Concat( doubles ).Concat( new[] { double4 } ).ToList();
            allDoubles[ 0 ] = double1;

            const int integer1 = -99;
            const int integer2 = -100;
            const int integer3 = -541;
            List<int> allIntegers = Enumerable.Range( 0 , rowsToGenerate ).Concat( new[] { integer1 } ).Concat( Enumerable.Range( rowsToGenerate + 1 , rowsToGenerate ).Concat( new[] { integer2 } ) ).Concat( Enumerable.Range( ( rowsToGenerate * 2 ) + 1 , rowsToGenerate ) ).Concat( new[] { integer3 } ).ToList();

            var dateColumn = new KeyValuePair<string , object>( "Date" , allDates );
            var integerColumn = new KeyValuePair<string , object>( "Integers" , allIntegers );
            var doubleColumn = new KeyValuePair<string , object>( "Doubles" , allDoubles );
            Sexp toAssign1 = Sexp.MakeDataFrame( new[] { dateColumn , integerColumn , doubleColumn } );
            Sexp toAssign2 = Sexp.MakeDataFrame( new[] { dateColumn , integerColumn , doubleColumn } , Enumerable.Range( 2 , ( rowsToGenerate * 3 ) + 3 ).Select( x => x.ToString() ) );

            using ( var service = new Rservice( false , 524288 ) )
            {
                // Act
                service.RConnection[ "assigns1" ] = toAssign1;
                service.RConnection[ "assigns2" ] = toAssign2;
                
                // Assert
                for ( int x = 1 ; x <= 2 ; x++ )
                {
                    Assert.Equal( today , service.RConnection[ string.Format( "assigns{0}$Date[ 1 ]" , x ) ].AsDate );
                    Assert.Equal( date1 , service.RConnection[ string.Format( "assigns{0}$Date[ {1} ]" , x , rowsToGenerate + 1 ) ].AsDate );
                    Assert.Equal( date2 , service.RConnection[ string.Format( "assigns{0}$Date[ {1} ]" , x , ( rowsToGenerate * 2 ) + 2 ) ].AsDate );
                    Assert.Equal( date3 , service.RConnection[ string.Format( "assigns{0}$Date[ {1} ]" , x , ( rowsToGenerate * 3 ) + 3 ) ].AsDate );

                    Assert.Equal( double1 , service.RConnection[ string.Format( "assigns{0}$Doubles[ 1 ]" , x ) ].AsDouble );
                    Assert.Equal( double2 , service.RConnection[ string.Format( "assigns{0}$Doubles[ {1} ]" , x , rowsToGenerate + 1 ) ].AsDouble );
                    Assert.Equal( double3 , service.RConnection[ string.Format( "assigns{0}$Doubles[ {1} ]" , x , ( rowsToGenerate * 2 ) + 2 ) ].AsDouble );
                    Assert.Equal( double4 , service.RConnection[ string.Format( "assigns{0}$Doubles[ {1} ]" , x , ( rowsToGenerate * 3 ) + 3 ) ].AsDouble );

                    Assert.Equal( 0 , service.RConnection[ string.Format( "assigns{0}$Integers[ 1 ]" , x ) ].AsInt );
                    Assert.Equal( integer1 , service.RConnection[ string.Format( "assigns{0}$Integers[ {1} ]" , x , rowsToGenerate + 1 ) ].AsInt );
                    Assert.Equal( integer2 , service.RConnection[ string.Format( "assigns{0}$Integers[ {1} ]" , x , ( rowsToGenerate * 2 ) + 2 ) ].AsInt );
                    Assert.Equal( integer3 , service.RConnection[ string.Format( "assigns{0}$Integers[ {1} ]" , x , ( rowsToGenerate * 3 ) + 3 ) ].AsInt );
                }

                Assert.Equal( "2" , service.RConnection[ "rownames( assigns2 )[ 1 ]" ].AsString );
                Assert.Equal( ( rowsToGenerate + 2 ).ToString() , service.RConnection[ string.Format( "rownames( assigns2 )[ {0} ]" , rowsToGenerate + 1 ) ].AsString );
                Assert.Equal( ( ( rowsToGenerate * 2 ) + 3 ).ToString() , service.RConnection[ string.Format( "rownames( assigns2 )[ {0} ]" , ( rowsToGenerate * 2 ) + 2 ) ].AsString );
                Assert.Equal( ( ( rowsToGenerate * 3 ) + 4 ).ToString() , service.RConnection[ string.Format( "rownames( assigns2 )[ {0} ]" , ( rowsToGenerate * 3 ) + 3 ) ].AsString );
            }
        }
    }


}