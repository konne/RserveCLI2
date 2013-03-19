//-----------------------------------------------------------------------
// <copyright file="Tests.cs" company="Oliver M. Haynold">
// Copyright (c) 2011, Oliver M. Haynold
// All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace RserveCLI2.Test
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    /// <summary>
    /// Collection of test fixtures for RServeCli
    /// </summary>
    [TestFixture]
    public class MainTests : IDisposable
    {
        /// <summary>
        /// Tolerance for comparing doubles
        /// </summary>
        private const double Tol = 1e-15;

        /// <summary>
        /// The connection to Rserve
        /// </summary>
        private RConnection sConn;

        /// <summary>
        /// Inits this instance.
        /// </summary>
        [SetUp]
        public void Init()
        {
            sConn = new RConnection(
                new System.Net.IPAddress( new byte[] { 192 , 168 , 37 , 10 } ) , user: "ruser" , password: "rpwd" );
        }

        /// <summary>
        /// Tests SexpInt.
        /// </summary>
        [Test]
        public void Int()
        {
            var zero = new SexpInt( 0 );
            var one = new SexpInt( 1 );
            var na = SexpInt.Na;

            // ReSharper disable EqualExpressionComparison
            Assert.That( zero.Equals( zero ) );
            Assert.That( one.Equals( one ) );
            Assert.That( !na.Equals( na ) );

            // ReSharper restore EqualExpressionComparison
            Assert.That( !zero.Equals( one ) );
            Assert.That( !zero.Equals( na ) );
            Assert.That( !one.Equals( na ) );
            Assert.That( zero.Equals( 0 ) );
            Assert.That( one.Equals( 1 ) );
            Assert.That( na.IsNa );
            Assert.That( zero.AsInt == 0 );
            Assert.That( one.AsInt == 1 );
#if(NUNIT_25)
#pragma warning disable 168
            Assert.Throws<ArithmeticException>(delegate { var x = na.AsInt; });
#pragma warning restore 168
#endif

            foreach ( var a in new Sexp[] { zero , one , na } )
            {
                Assert.That( !a.Equals( new SexpNull() ) );
            }
        }

        /// <summary>
        /// Tests SexpDouble.
        /// </summary>
        [Test]
        public void Double()
        {
            var zero = new SexpDouble( 0.0 );
            var one = new SexpDouble( 1.0 );
            var na = SexpDouble.Na;

            // ReSharper disable EqualExpressionComparison
            Assert.That( zero.Equals( zero ) );
            Assert.That( one.Equals( one ) );
            Assert.That( !na.Equals( na ) );

            // ReSharper restore EqualExpressionComparison
            Assert.That( !zero.Equals( one ) );
            Assert.That( !zero.Equals( na ) );
            Assert.That( !one.Equals( na ) );
            Assert.That( zero.Equals( 0 ) );
            Assert.That( one.Equals( 1 ) );
            Assert.That( na.IsNa );
            Assert.That( zero.AsDouble == 0.0 );
            Assert.That( one.AsDouble == 1.0 );
            Assert.That( zero.AsInt == 0 );
            Assert.That( one.AsInt == 1 );
            Assert.IsNaN( na.AsDouble );

            foreach ( var a in new Sexp[] { zero , one , na } )
            {
                Assert.That( !a.Equals( new SexpNull() ) );
            }
        }

        /// <summary>
        /// Tests SexpBool.
        /// </summary>
        [Test]
        public void Bool()
        {
            var sexpTrue = new SexpBool( SexpBoolValue.True );
            var sexpFalse = new SexpBool( SexpBoolValue.False );
            var sexpNa = new SexpBool( SexpBoolValue.Na );

            // ReSharper disable EqualExpressionComparison
            Assert.That( sexpTrue.Equals( sexpTrue ) );
            Assert.That( sexpFalse.Equals( sexpFalse ) );
            Assert.That( !sexpNa.Equals( sexpNa ) );

            // ReSharper restore EqualExpressionComparison
            Assert.That( !sexpTrue.Equals( sexpFalse ) );
            Assert.That( !sexpTrue.Equals( sexpNa ) );
            Assert.That( sexpTrue.Equals( true ) );
            Assert.That( sexpFalse.Equals( false ) );
            Assert.That( !sexpNa.Equals( true ) );
            Assert.That( !sexpNa.Equals( false ) );
            foreach ( var a in new Sexp[] { sexpTrue , sexpFalse , sexpNa } )
            {
                Assert.That( !a.Equals( new SexpNull() ) );
            }

            sConn[ "x.bool" ] = Sexp.Make( true );
        }

        /// <summary>
        /// Tests SexpArrayBool.
        /// </summary>
        [Test]
        public void ArrayBool()
        {
            var testvals = new[] { SexpBoolValue.True , SexpBoolValue.False , SexpBoolValue.Na };
            var x1 = new SexpArrayBool( testvals );

            Assert.AreEqual( testvals.Length , x1.Count );

            sConn[ "x1" ] = x1;

            for ( int i = 0 ; i < x1.Count ; i++ )
            {
                Assert.AreEqual( new SexpBool( testvals[ i ] ).AsBool , x1[ i ].AsBool );
            }

            sConn.Eval( "x2 <- as.logical(c(TRUE,FALSE,NA))" );
            var x2 = sConn[ "x2" ];

            Assert.AreEqual( x1.Count , x2.Count );

            for ( int i = 0 ; i < x1.Count ; i++ )
            {
                if ( x1[ i ].IsNa )
                {
                    Assert.IsTrue( x2[ i ].IsNa );
                }
                else
                {
                    Assert.AreEqual( x1[ i ].AsBool , x2[ i ].AsBool );
                }
            }

            var equals = sConn[ "x1 == x2" ];

            Assert.AreEqual( x1.Count , equals.Count );

            for ( int i = 0 ; i < x1.Count ; i++ )
            {
                if ( !x1[ i ].IsNa )
                {
                    Assert.That( equals[ i ].AsBool , equals.ToString() );
                }
            }

            Assert.AreEqual( x1.IndexOf( Sexp.Make( false ) ) , 1 );

            x1.AsList[ 0 ] = false;
            Assert.AreEqual( x1[ 0 ].AsBool , false );
        }

        /// <summary>
        /// Tests SexpArrayDouble.
        /// </summary>
        [Test]
        public void ArrayDouble()
        {
            var testDoubles = new[] { -3.5 , 0.0 , 1.0 , 2.0 , 1.0E20 , double.NaN , double.NaN };
            var x1 = Sexp.Make( testDoubles );
            x1[ x1.Count - 1 ] = SexpDouble.Na;

            for ( int i = 0 ; i < x1.Count ; i++ )
            {
                Assert.AreEqual( testDoubles[ i ] , x1[ i ].AsDouble );
            }

            sConn.Eval( "x2 <- as.numeric(c(-3.5,0,1,2,1E20,NaN,NA))" );
            var x2 = sConn[ "x2" ];

            Assert.AreEqual( x1.Count , x2.Count );

            for ( int i = 0 ; i < x1.Count ; i++ )
            {
                if ( x1[ i ].IsNa )
                {
                    Assert.IsTrue( x2[ i ].IsNa );
                }
                else if ( x1[ i ].IsNull )
                {
                    Assert.IsTrue( x2[ i ].IsNull );
                }
                else if ( double.IsNaN( x1[ i ].AsDouble ) )
                {
                    Assert.That( double.IsNaN( x2[ i ].AsDouble ) );
                }
                else
                {
                    var res = Math.Abs( x1[ i ].AsDouble - x2[ i ].AsDouble ) < Tol;
                    Assert.That( res );
                }
            }

            sConn[ "x1" ] = x1;
            var equals = sConn[ "x1 == x2" ];

            Assert.AreEqual( x1.Count , equals.Count );

            for ( int i = 0 ; i < x1.Count ; i++ )
            {
                if ( !double.IsNaN( x1[ i ].AsDouble ) )
                {
                    Assert.That( equals[ i ].AsBool , equals.ToString() );
                }
            }

            Assert.AreEqual( x1.IndexOf( new SexpDouble( 1.0 ) ) , 2 );

            x1.AsList[ 0 ] = -5.5;
            Assert.AreEqual( x1[ 0 ].AsDouble , -5.5 );
        }

        /// <summary>
        /// Tests SexpArrayInt.
        /// </summary>
        [Test]
        public void ArrayInt()
        {
            var testInts = new[] { -3 , 0 , 1 , 2 , 524566 , 0 };
            var x1 = Sexp.Make( testInts );
            x1[ x1.Count - 1 ] = SexpInt.Na;

            for ( int i = 0 ; i < x1.Count ; i++ )
            {
                if ( !x1[ i ].IsNa )
                {
                    Assert.AreEqual( testInts[ i ] , x1[ i ].AsInt );
                }
            }

            sConn.Eval( "x2 <- as.integer(c(-3,0,1,2,524566,NA))" );
            var x2 = sConn[ "x2" ];

            Assert.AreEqual( x1.Count , x2.Count );

            for ( int i = 0 ; i < x1.Count ; i++ )
            {
                if ( x1[ i ].IsNa )
                {
                    Assert.IsTrue( x2[ i ].IsNa );
                }
                else
                {
                    Assert.That( x1[ i ].AsDouble == x2[ i ].AsDouble );
                }
            }

            sConn[ "x1" ] = x1;
            var equals = sConn[ "x1 == x2" ];

            Assert.AreEqual( x1.Count , equals.Count );

            for ( int i = 0 ; i < x1.Count ; i++ )
            {
                if ( !x1[ i ].IsNa )
                {
                    Assert.That( equals[ i ].AsBool , equals.ToString() );
                }
            }

            Assert.AreEqual( x1.IndexOf( new SexpInt( 1 ) ) , 2 );

            x1.AsList[ 0 ] = -5;
            Assert.AreEqual( x1[ 0 ].AsInt , -5 );
        }

        /// <summary>
        /// Tests SexpArrayInt with a one million entries.
        /// </summary>
        [Test]
        public void ArrayIntLarge()
        {
            sConn.Eval( "x <- 1:1000000" );
            var x = sConn[ "x" ];

            Assert.AreEqual( x.Count , 1000000 );

            for ( int i = 0 ; i < x.Count ; i++ )
            {
                Assert.AreEqual( x[ i ].AsInt , i + 1 );
            }

        }

        /// <summary>
        /// Tests SexpList.
        /// </summary>
        [Test]
        public void List()
        {
            var mylist = new Dictionary<string , object> { { "One" , 1 } , { "Two" , 2.0 } , { "Three" , "three" } };
            var x1 = Sexp.Make( mylist );
            sConn[ "x1" ] = x1;

            Assert.AreEqual( x1.Count , mylist.Count );

            sConn.Eval( "x2 <- list(One=1,Two=2.0,Three=\"three\")" );
            var x2 = sConn[ "x2" ];

            Assert.AreEqual( x1.Count , x2.Count );

            for ( int i = 0 ; i < x1.Count ; i++ )
            {
                Assert.That( x1[ i ].Equals( x2[ i ] ) );
                Assert.AreEqual( x1.Names[ i ].AsString , x2.Names[ i ].AsString );
                Assert.That( x1[ x1.Names[ i ].AsString ].Equals( x2[ x2.Names[ i ].AsString ] ) );
            }
        }

        /// <summary>
        /// Tests the linear algebra functions of two-dimensional double arrays
        /// </summary>
        [Test]
        public void MatrixDouble()
        {
            // Same as for integers -- we'll divide by two to get floating point values that aren't integers
            var matA = new double[ , ] { { 14 , 9 , 3 } , { 2 , 11 , 15 } , { 0 , 12 , 17 } , { 5 , 2 , 3 } };
            var matB = new double[ , ] { { 12 , 25 } , { 9 , 10 } , { 8 , 5 } };
            var matC = new double[ , ] { { 273 , 455 } , { 243 , 235 } , { 244 , 205 } , { 102 , 160 } };
            var sexpA = Sexp.Make( matA );
            sConn[ "a" ] = sexpA;
            sConn[ "b" ] = Sexp.Make( matB );

            // Some simple tests with A
            for ( int i = 0 ; i <= 1 ; i++ )
            {
                Assert.AreEqual( matA.GetLength( i ) , sexpA.GetLength( i ) );
                Assert.AreEqual( matA.GetLength( i ) , sConn[ "a" ].GetLength( i ) );
            }

            for ( var row = 0 ; row < matA.GetLength( 0 ) ; row++ )
            {
                for ( var col = 0 ; col < matA.GetLength( 1 ) ; col++ )
                {
                    Assert.AreEqual( matA[ row , col ] , sexpA[ row , col ].AsDouble );
                    Assert.AreEqual( matA[ row , col ] , sConn[ "a" ][ row , col ].AsDouble );
                }
            }

            var matD = sConn[ "a %*% b" ];

            // check that C and D are equal
            for ( var i = 0 ; i <= 1 ; i++ )
            {
                Assert.AreEqual( matC.GetLength( i ) , matD.GetLength( i ) );
            }

            for ( var row = 0 ; row < matC.GetLength( 0 ) ; row++ )
            {
                for ( var col = 0 ; col < matD.GetLength( 1 ) ; col++ )
                {
                    Assert.AreEqual( matC[ row , col ] , matD[ row , col ].AsDouble );
                }
            }
        }

        /// <summary>
        /// Tests the linear algebra functions of two-dimensional integer arrays
        /// </summary>
        [Test]
        public void MatrixInt()
        {
            // Same as for integers -- we'll divide by two to get floating point values that aren't integers
            var matA = new[ , ] { { 14 , 9 , 3 } , { 2 , 11 , 15 } , { 0 , 12 , 17 } , { 5 , 2 , 3 } };
            var matB = new[ , ] { { 12 , 25 } , { 9 , 10 } , { 8 , 5 } };
            var matC = new[ , ] { { 273 , 455 } , { 243 , 235 } , { 244 , 205 } , { 102 , 160 } };
            var sexpA = Sexp.Make( matA );
            sConn[ "a" ] = sexpA;
            sConn[ "b" ] = Sexp.Make( matB );

            // Some simple tests with A
            for ( int i = 0 ; i <= 1 ; i++ )
            {
                Assert.AreEqual( matA.GetLength( i ) , sexpA.GetLength( i ) );
                Assert.AreEqual( matA.GetLength( i ) , sConn[ "a" ].GetLength( i ) );
            }

            for ( int row = 0 ; row < matA.GetLength( 0 ) ; row++ )
            {
                for ( int col = 0 ; col < matA.GetLength( 1 ) ; col++ )
                {
                    Assert.AreEqual( matA[ row , col ] , sexpA[ row , col ].AsInt );
                    Assert.AreEqual( matA[ row , col ] , sConn[ "a" ][ row , col ].AsInt );
                }
            }

            var matD = sConn[ "a %*% b" ];

            // check that C and D are equal
            for ( var i = 0 ; i <= 1 ; i++ )
            {
                Assert.AreEqual( matC.GetLength( i ) , matD.GetLength( i ) );
            }

            for ( var row = 0 ; row < matC.GetLength( 0 ) ; row++ )
            {
                for ( var col = 0 ; col < matD.GetLength( 1 ) ; col++ )
                {
                    Assert.AreEqual( matC[ row , col ] , matD[ row , col ].AsInt );
                }
            }
        }

        /// <summary>
        /// Tests transfer of a small file.
        /// </summary>
        [Test]
        public void TransferFileSmall()
        {
            TransferFile( 100 );
        }

        /// <summary>
        /// Tests transfer of a large file.
        /// </summary>
        [Test]
        public void TransferFileLarge()
        {
            TransferFile( 100000 );
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [TearDown]
        public void Dispose()
        {
            if ( sConn != null )
            {
                sConn.Dispose();
            }
        }

        /// <summary>
        /// Test file transfer to and from the R session.
        /// </summary>
        /// <param name="length">The length of the file to be transferred.</param>
        private void TransferFile( int length )
        {
            var data = new byte[ length ];
            var rnd = new Random( 2302 );
            rnd.NextBytes( data );
            using ( var os = new System.IO.MemoryStream( data ) )
            {
                sConn.WriteFile( "test.dat" , os );
            }

            using ( var ist = sConn.ReadFile( "test.dat" ) )
            {
                var checkstream = new System.IO.MemoryStream();
                ist.CopyTo( checkstream );
                var checkbytes = checkstream.ToArray();
                Assert.AreEqual( data , checkbytes );
            }

            sConn.RemoveFile( "test.dat" );
        }
    }
}
