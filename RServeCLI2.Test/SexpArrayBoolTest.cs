//-----------------------------------------------------------------------
// Original work Copyright (c) 2011, Oliver M. Haynold
// Modified work Copyright (c) 2013, Suraj Gupta
// All rights reserved.
//-----------------------------------------------------------------------

using Xunit;

namespace RserveCLI2.Test
{
    public class SexpArrayBoolTest
    {
        [Fact]
        public void AsBools_ReadLogicalFromR_ReturnsArrayOfNullableBools()
        {            
            using ( var service = new Rservice() )
            {
                // Arrange / Act
                Sexp bool1 = service.RConnection[ "logical()" ];
                Sexp bool2 = service.RConnection[ "TRUE" ];
                Sexp bool3 = service.RConnection[ "FALSE" ];
                Sexp bool4 = service.RConnection[ "as.logical( NA )" ];
                Sexp bool5 = service.RConnection[ "c( TRUE , NA , FALSE  )" ];
                Sexp bool6 = service.RConnection[ "matrix( c( TRUE , NA , FALSE , FALSE , TRUE , NA  ) , nrow = 2 , byrow = TRUE )" ];
                
                // Assert
                Assert.Equal( new bool?[] { } , bool1.AsBools );
                Assert.Equal( new bool?[] { true } , bool2.AsBools );
                Assert.Equal( new bool?[] { false } , bool3.AsBools );
                Assert.Equal( new bool?[] { null } , bool4.AsBools );
                Assert.Equal( new bool?[] { true , null , false } , bool5.AsBools );
                Assert.Equal( new bool?[] { true , false , null , true , false , null } , bool6.AsBools );
            }
        }

        [Fact]
        public void AsBools_WriteSexpArrayBoolToRAndReadBack_ReturnsArrayOfNullableBools()
        {
            using ( var service = new Rservice() )
            {
                // Arrange
                Sexp bool1 = new SexpArrayBool();
                Sexp bool2 = new SexpArrayBool( true );
                Sexp bool3 = new SexpArrayBool( false );
                Sexp bool4 = new SexpArrayBool( ( bool? )null );
                Sexp bool5 = new SexpArrayBool( new bool?[] { true } );
                Sexp bool6 = new SexpArrayBool( new bool?[] { false } );
                Sexp bool7 = new SexpArrayBool( new bool?[] { null } );
                Sexp bool8 = new SexpArrayBool( new bool?[] { true , null , false } );
                Sexp bool9 = new SexpArrayBool( new bool?[] { true , false , null , true , false , null } );
                bool9.Attributes.Add( "dim" , Sexp.Make( new[] { 2 , 3 } ) );
                
                // Act
                service.RConnection[ "bool1" ] = bool1;
                service.RConnection[ "bool2" ] = bool2;
                service.RConnection[ "bool3" ] = bool3;
                service.RConnection[ "bool4" ] = bool4;
                service.RConnection[ "bool5" ] = bool5;
                service.RConnection[ "bool6" ] = bool6;
                service.RConnection[ "bool7" ] = bool7;
                service.RConnection[ "bool8" ] = bool8;
                service.RConnection[ "bool9" ] = bool9;
                bool1 = service.RConnection[ "bool1" ];
                bool2 = service.RConnection[ "bool2" ];
                bool3 = service.RConnection[ "bool3" ];
                bool4 = service.RConnection[ "bool4" ];
                bool5 = service.RConnection[ "bool5" ];
                bool6 = service.RConnection[ "bool6" ];
                bool7 = service.RConnection[ "bool7" ];
                bool8 = service.RConnection[ "bool8" ];
                bool9 = service.RConnection[ "bool9" ];
                
                // Assert                
                Assert.Equal( new bool?[] { } , bool1.AsBools );
                Assert.Equal( new bool?[] { true } , bool2.AsBools );
                Assert.Equal( new bool?[] { false } , bool3.AsBools );
                Assert.Equal( new bool?[] { null } , bool4.AsBools );
                Assert.Equal( new bool?[] { true } , bool5.AsBools );
                Assert.Equal( new bool?[] { false } , bool6.AsBools );
                Assert.Equal( new bool?[] { null } , bool7.AsBools );
                Assert.Equal( new bool?[] { true , null , false } , bool8.AsBools );
                Assert.Equal( new bool?[] { true , false , null , true , false , null } , bool9.AsBools );

            }
        }
        
        [Fact]
        public void Various_ArrayBool()
        {
            // note: this test was ported from RserveCLI.  Not in the typical Arrange/Act/Assert format
            using ( var service = new Rservice() )
            {
                var testvals = new bool?[] { true , false , null };
                var x1 = new SexpArrayBool( testvals );

                Assert.Equal( testvals.Length , x1.Count );

                service.RConnection[ "x1" ] = x1;

                for ( int i = 0 ; i < x1.Count ; i++ )
                {
                    Assert.Equal( new SexpArrayBool( testvals[ i ] ).AsBool , x1[ i ].AsBool );
                }

                service.RConnection.Eval( "x2 <- as.logical(c(TRUE,FALSE,NA))" );
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
                        Assert.Equal( x1[ i ].AsBool , x2[ i ].AsBool );
                    }
                }

                var equals = service.RConnection[ "x1 == x2" ];

                Assert.Equal( x1.Count , equals.Count );

                for ( int i = 0 ; i < x1.Count ; i++ )
                {
                    if ( !x1[ i ].IsNa )
                    {
                        Assert.True( ( bool )( equals[ i ].AsBool ) , equals.ToString() );
                    }
                }

                Assert.Equal( x1.IndexOf( Sexp.Make( false ) ) , 1 );

                x1.AsList[ 0 ] = false;
                Assert.Equal( x1[ 0 ].AsBool , false );
            }
        }

        [Fact]
        public void Various_Bool()
        {

            // note: this test was ported from RserveCLI.  Not in the typical Arrange/Act/Assert format
            using ( var service = new Rservice() )
            {
                var sexpTrue = new SexpArrayBool( true );
                var sexpFalse = new SexpArrayBool( false );
                var sexpNa = new SexpArrayBool( ( bool? )null );

                // ReSharper disable EqualExpressionComparison
                Assert.True( sexpTrue.Equals( sexpTrue ) );
                Assert.True( sexpFalse.Equals( sexpFalse ) );
                Assert.True( sexpNa.Equals( sexpNa ) );

                // ReSharper restore EqualExpressionComparison
                Assert.True( !sexpTrue.Equals( sexpFalse ) );
                Assert.True( !sexpTrue.Equals( sexpNa ) );
                Assert.True( sexpTrue.Equals( true ) );
                Assert.True( sexpFalse.Equals( false ) );
                Assert.True( !sexpNa.Equals( true ) );
                Assert.True( !sexpNa.Equals( false ) );
                foreach ( var a in new Sexp[] { sexpTrue , sexpFalse , sexpNa } )
                {
                    Assert.True( !a.Equals( new SexpNull() ) );
                }

                service.RConnection[ "x.bool" ] = Sexp.Make( true );
            }
        }

    }
}
