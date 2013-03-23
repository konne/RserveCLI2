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
        public void Various_ArrayBool()
        {
            // note: this test was ported from RserveCLI.  Not in the typical Arrange/Act/Assert format
            using ( var service = new Rservice() )
            {
                var testvals = new[] { SexpBoolValue.True , SexpBoolValue.False , SexpBoolValue.Na };
                var x1 = new SexpArrayBool( testvals );

                Assert.Equal( testvals.Length , x1.Count );

                service.RConnection[ "x1" ] = x1;

                for ( int i = 0 ; i < x1.Count ; i++ )
                {
                    Assert.Equal( new SexpBool( testvals[ i ] ).AsBool , x1[ i ].AsBool );
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
                        Assert.True( equals[ i ].AsBool , equals.ToString() );
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
                var sexpTrue = new SexpBool( SexpBoolValue.True );
                var sexpFalse = new SexpBool( SexpBoolValue.False );
                var sexpNa = new SexpBool( SexpBoolValue.Na );

                // ReSharper disable EqualExpressionComparison
                Assert.True( sexpTrue.Equals( sexpTrue ) );
                Assert.True( sexpFalse.Equals( sexpFalse ) );
                Assert.True( !sexpNa.Equals( sexpNa ) );

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
