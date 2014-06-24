//-----------------------------------------------------------------------
// Copyright (c) 2011, Oliver M. Haynold
// All rights reserved.
//-----------------------------------------------------------------------

namespace RserveCLI2.Example
{
    using System;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// A set of simple examples for RserveCLI.
    /// </summary>
    public class MainClass
    {
        /// <summary>
        /// Executes the program
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main( string[] args )
        {
            using ( var s = new RConnection( new System.Net.IPAddress( new byte[] { 127 , 0 , 0 , 1 } ) ) )
            {
                // Generate some example data
                var x = Enumerable.Range( 1 , 20 ).ToArray();
                var y = ( from a in x select ( 0.5 * a * a ) + 2 ).ToArray();

                // Build an R data frame
                var d = Sexp.MakeDataFrame();
                d[ "x" ] = Sexp.Make( x );
                d[ "y" ] = Sexp.Make( y );
                s[ "d" ] = d;

                // Run a linear regression, obtain the summary, and print the result
                s.VoidEval( "linearModelSummary = summary(lm(y ~ x, d))" );
                var coefs = s[ "linearModelSummary$coefficients" ];
                var rSquared = s[ "linearModelSummary$r.squared" ].AsDouble;
                Console.WriteLine( "y = {0} x + {1}. R^2 = {2,4:F}%" , coefs[ 1 , 0 ] , coefs[ 0 , 0 ] , rSquared * 100 );

                // Now let's do some linear algebra
                var matA = new double[ , ] { { 14 , 9 , 3 } , { 2 , 11 , 15 } , { 0 , 12 , 17 } , { 5 , 2 , 3 } };
                var matB = new double[ , ] { { 12 , 25 } , { 9 , 10 } , { 8 , 5 } };
                s[ "a" ] = Sexp.Make( matA );
                s[ "b" ] = Sexp.Make( matB );
                Console.WriteLine( s[ "a %*% b" ].ToString() );
            }
            Console.WriteLine( "Done" );
        }

        // need to work this into a unit test - create file on server, read from server, then delete from server
        //// Make a chart and transfer it to the local machine
        //        s.VoidEval( "library(ggplot2)" );
        //        s.VoidEval( "pdf(\"outfile.pdf\")" );
        //        s.VoidEval( "print(qplot(x,y, data=d))" );
        //        s.VoidEval( "dev.off()" );

        //        using ( var f = File.Create( "Data Plot.pdf" ) )
        //        {
        //            s.ReadFile( "outfile.pdf" ).CopyTo( f );
        //        }

        //        s.RemoveFile( "outfile.pdf" );

    }
}
