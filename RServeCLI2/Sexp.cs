// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Sexp.cs" company="Oliver M. Haynold">
// Copyright (c) 2011, Oliver M. Haynold
// All rights reserved.
// </copyright>
// <summary>
// Implements Sexpressions
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RserveCLI2
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A local representation for an S-Expression (a.k.a., Sexp, Rexp, R-expression).
    /// </summary>
    public abstract class Sexp : IList<Sexp> , IDictionary<string , Sexp> , IList<object> , IDictionary<string , object>
    {

        #region Constants and Fields

        /// <summary>
        /// The Sexp attributes, if any
        /// </summary>
        private Dictionary<string , Sexp> _attributes;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether [as bool].
        /// </summary>
        /// <value>
        /// <c>true</c> if [as bool]; otherwise, <c>false</c>.
        /// </value>
        public virtual bool AsBool
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Syntactic sugar for explicit conversion to bool
        /// </summary>
        /// <param name="s">The Sexp</param>
        /// <returns>The converted value</returns>
        public static explicit operator bool( Sexp s )
        {
            return s.AsBool;
        }

        /// <summary>
        /// Gets as dictionary of objects.
        /// </summary>
        public IDictionary<string , object> AsDictionary
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Gets as double.
        /// </summary>
        /// <value>
        /// As double.
        /// </value>
        public virtual double AsDouble
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Syntactic sugar for explicit conversion to double
        /// </summary>
        /// <param name="s">The Sexp</param>
        /// <returns>The converted value</returns>
        public static explicit operator double( Sexp s )
        {
            return s.AsDouble;
        }

        /// <summary>
        /// Gets as array of double.
        /// </summary>
        public virtual double[] AsDoubles
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Syntactic sugar for explicit conversion to double
        /// </summary>
        /// <param name="s">The Sexp</param>
        /// <returns>The converted value</returns>
        public static explicit operator double[]( Sexp s )
        {
            return s.AsDoubles;
        }

        /// <summary>
        /// Gets as int.
        /// </summary>
        /// <value>
        /// The value as an integer.
        /// </value>
        public virtual int AsInt
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Syntactic sugar for explicit conversion to int
        /// </summary>
        /// <param name="s">The Sexp</param>
        /// <returns>The converted value</returns>
        public static explicit operator int( Sexp s )
        {
            return s.AsInt;
        }

        /// <summary>
        /// Gets as double.
        /// </summary>
        /// <value>
        /// As double.
        /// </value>
        public virtual double[ , ] As2DArrayDouble
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Syntactic sugar for explicit conversion to double
        /// </summary>
        /// <param name="s">The Sexp</param>
        /// <returns>The converted value</returns>
        public static explicit operator double[ , ]( Sexp s )
        {
            return s.As2DArrayDouble;
        }

        /// <summary>
        /// Gets as int.
        /// </summary>
        /// <value>
        /// The value as an integer.
        /// </value>
        public virtual int[] AsInts
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Syntactic sugar for explicit conversion to int
        /// </summary>
        /// <param name="s">The Sexp</param>
        /// <returns>The converted value</returns>
        public static explicit operator int[]( Sexp s )
        {
            return s.AsInts;
        }
        
        /// <summary>
        /// Gets as int.
        /// </summary>
        /// <value>
        /// The value as an integer.
        /// </value>
        public virtual int[ , ] As2DArrayInt
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Syntactic sugar for explicit conversion to int
        /// </summary>
        /// <param name="s">The Sexp</param>
        /// <returns>The converted value</returns>
        public static explicit operator int[ , ]( Sexp s )
        {
            return s.As2DArrayInt;
        }

        /// <summary>
        /// Gets as date.
        /// </summary>
        /// <value>
        /// The value as an integer.
        /// </value>
        public virtual DateTime AsDate
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Syntactic sugar for explicit conversion to int
        /// </summary>
        /// <param name="s">The Sexp</param>
        /// <returns>The converted value</returns>
        public static explicit operator DateTime( Sexp s )
        {
            return s.AsDate;
        }

        /// <summary>
        /// Gets as date.
        /// </summary>
        /// <value>
        /// The value as an integer.
        /// </value>
        public virtual DateTime[] AsDates
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Syntactic sugar for explicit conversion to int
        /// </summary>
        /// <param name="s">The Sexp</param>
        /// <returns>The converted value</returns>
        public static explicit operator DateTime[]( Sexp s )
        {
            return s.AsDates;
        }

        /// <summary>
        /// Gets as list of objects.
        /// </summary>
        public IList<object> AsList
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Gets as sexp bool.
        /// </summary>
        /// <value>
        /// As sexp bool.
        /// </value>
        public virtual SexpBoolValue AsSexpBool
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Syntactic sugar for explicit conversion to SexpBoolValue
        /// </summary>
        /// <param name="s">The Sexp</param>
        /// <returns>The converted value</returns>
        public static explicit operator SexpBoolValue( Sexp s )
        {
            return s.AsSexpBool;
        }

        /// <summary>
        /// Gets as dictionary of Sexps.
        /// </summary>
        public IDictionary<string , Sexp> AsSexpDictionary
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Gets as string.
        /// </summary>
        /// <value>
        /// As string.
        /// </value>
        public virtual string AsString
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Syntactic sugar for explicit conversion to string
        /// </summary>
        /// <param name="s">The Sexp</param>
        /// <returns>The converted value</returns>
        public static explicit operator string( Sexp s )
        {
            return s.AsString;
        }

        /// <summary>
        /// Gets as Strings.
        /// </summary>
        public virtual string[] AsStrings
        {
            get
            {
                return this.Select<Sexp , string>( a => a.AsString ).ToArray();
            }
        }

        /// <summary>
        /// Syntactic sugar for explicit conversion to string[]
        /// </summary>
        /// <param name="s">The Sexp</param>
        /// <returns>The converted value</returns>
        public static explicit operator string[]( Sexp s )
        {
            return s.AsStrings;
        }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        public Dictionary<string , Sexp> Attributes
        {
            get
            {
                return _attributes ?? ( _attributes = new Dictionary<string , Sexp>() );
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public virtual int Count
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is NA.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is NA; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsNa
        {
            get
            {
                if ( Count == 1 )
                {
                    return this[ 0 ].IsNa;
                }

                throw new IndexOutOfRangeException( "Only single values can be tested for NA." );
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is null.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is null; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsNull
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        public virtual bool IsReadOnly
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        public virtual ICollection<string> Keys
        {
            get
            {
                return Names.AsStrings;
            }
        }

        /// <summary>
        /// Gets the names.
        /// </summary>
        public virtual Sexp Names
        {
            get
            {
                return Attributes[ "names" ];
            }
        }

        /// <summary>
        /// Gets the rank. E.g., a plain list has a rank of 1 and a matrix has a rank of 2.
        /// </summary>
        public virtual int Rank
        {
            get
            {
                return Attributes[ "dim" ].Count;
            }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        public virtual ICollection<Sexp> Values
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.ICollection`1"/> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        ICollection<object> IDictionary<string , object>.Values
        {
            get
            {
                return ( ICollection<object> )Values;
            }
        }

        /// <summary>
        /// Gets rownames of this matrix.
        /// </summary>
        /// <remarks>
        /// If none, returns null.  This matches R's behavior.  
        /// For example, both rownames( matrix( 1 ) ) and rownames( c( 1 , 2 , 3 ) ) return NULL in R.
        /// </remarks>
        public string[] RowNames
        {
            get
            {
                if ( Attributes.ContainsKey( "dimnames" ) )
                {
                    string[] rowNames = Attributes[ "dimnames" ].Values.ToList()[ 0 ].AsStrings;
                    return rowNames.Length == 0 ? null : rowNames;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets colnames of this matrix.
        /// </summary>
        /// <remarks>
        /// If none, returns null.  This matches R's behavior.  
        /// For example, both colnames( matrix( 1 ) ) and colnames( c( 1 , 2 , 3 ) ) return NULL in R.
        /// </remarks>
        public string[] ColNames
        {
            get
            {
                if ( Attributes.ContainsKey( "dimnames" ) )
                {
                    string[] colNames = Attributes[ "dimnames" ].Values.ToList()[ 1 ].AsStrings;
                    return colNames.Length == 0 ? null : colNames;
                }
                return null;
            }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="row">The zero-based row index.</param>
        /// <param name="col">The zero-based column index.</param>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        public virtual Sexp this[ int row , int col ]
        {
            get
            {
                if ( Rank != 2 )
                {
                    throw new ArithmeticException( "Only Sexps of Rank 2 can be accessed as arrays." );
                }

                return this[ ( col * GetLength( 0 ) ) + row ];
            }

            set
            {
                if ( Rank != 2 )
                {
                    throw new ArithmeticException( "Only objects of rank 2 can be accessed as matrices." );
                }

                this[ ( col * GetLength( 0 ) ) + row ] = value;
            }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index.</param>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        public virtual Sexp this[ int index ]
        {
            get
            {
                throw new NotSupportedException();
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="key">The name of the value.</param>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        public virtual Sexp this[ string key ]
        {
            get
            {
                var index = Array.IndexOf( Names.AsStrings , key );
                if ( index < 0 )
                {
                    throw new KeyNotFoundException( "Could not find key '" + key + "' in names." );
                }

                return this[ index ];
            }

            set
            {
                var index = Array.IndexOf( Names.AsStrings , key );
                if ( index < 0 )
                {
                    Add( key , value );
                }
                else
                {
                    this[ index ] = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element.</param>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        object IList<object>.this[ int index ]
        {
            get
            {
                return this[ index ];
            }

            set
            {
                this[ index ] = Make( value );
            }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="key">The name of the element.</param>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        object IDictionary<string , object>.this[ string key ]
        {
            get
            {
                return this[ key ];
            }

            set
            {
                this[ key ] = Make( value );
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Makes a Sexp from an object.
        /// </summary>
        /// <param name="x">The object to convert into an Sexp.</param>
        /// <returns>
        /// The Sexp made.
        /// </returns>
        public static Sexp Make( object x )
        {        
            if ( x is Sexp                         ) { return       ( Sexp )x;                           }
            if ( x is bool                         ) { return Make( ( bool )x                         ); }
            if ( x is double                       ) { return Make( ( double )x                       ); }
            if ( x is IEnumerable<double>          ) { return Make( ( IEnumerable<double> )x          ); }
            if ( x is double[ , ]                  ) { return Make( ( double[ , ] )x                  ); }
            if ( x is decimal                      ) { return Make( ( decimal )x                      ); }
            if ( x is IEnumerable<decimal>         ) { return Make( ( IEnumerable<decimal> )x         ); }
            if ( x is decimal[ , ]                 ) { return Make( ( decimal[ , ] )x                 ); }
            if ( x is int                          ) { return Make( ( int )x                          ); }
            if ( x is int[ , ]                     ) { return Make( ( int[ , ] )x                     ); }
            if ( x is IEnumerable<int>             ) { return Make( ( IEnumerable<int> )x             ); }
            if ( x is DateTime                     ) { return Make( ( DateTime )x                     ); }
            if ( x is IEnumerable<DateTime>        ) { return Make( ( IEnumerable<DateTime> )x        ); }
            if ( x is string                       ) { return Make( ( string )x                       ); }
            if ( x is IEnumerable<string>          ) { return Make( ( IEnumerable<string> )x          ); }
            if ( x is IDictionary<string , object> ) { return Make( ( IDictionary<string , object> )x ); }
            throw new ArgumentException( string.Format( "I don't have an automatic conversion rule for type {0} to Sexp." , x.GetType().Name ) );
        }

        /// <summary>
        /// Makes a SexpBool from a bool.
        /// </summary>
        /// <param name="x">The bool to convert into an SexpBool.</param>
        public static Sexp Make( bool x )
        {
            return new SexpBool( x );
        }

        /// <summary>
        /// Makes a SexpArrayInt from an int.
        /// </summary>
        /// <param name="x">The int to convert into an SexpArrayInt.</param>
        public static Sexp Make( int x )
        {
            return new SexpArrayInt( x );
        }

        /// <summary>
        /// Makes a SexpArrayInt from an IEnumerable of int.
        /// </summary>
        /// <param name="xs">The IEnumerable of int to convert into an SexpArrayInt.</param>
        public static Sexp Make( IEnumerable<int> xs )
        {
            return new SexpArrayInt( xs );
        }

        /// <summary>
        /// Makes a SexpArrayDate from a DateTime.
        /// </summary>
        /// <param name="x">The DateTime to convert into an SexpArrayDate.
        /// </param>
        public static Sexp Make( DateTime x )
        {
            return new SexpArrayDate( x );
        }

        /// <summary>
        /// Makes a SexpArrayDate from an IEnumerable of DateTime
        /// </summary>
        /// <param name="xs">The IEnumerable of DateTime to convert into an SexpArrayDate.</param>
        public static Sexp Make( IEnumerable<DateTime> xs )
        {
            return new SexpArrayDate( xs );
        }

        /// <summary>
        /// Makes a SexpArrayDouble from a decimal.
        /// </summary>
        /// <param name="x">The decimal to convert into an SexpArrayDouble.</param>
        public static Sexp Make( decimal x )
        {
            return Make( Convert.ToDouble( x ) );
        }

        /// <summary>
        /// Makes a SexpArrayDouble from an IEnumerable of decimal.
        /// </summary>
        /// <param name="xs">The IEnumerable of decimal to convert into an SexpArrayDouble.</param>
        public static Sexp Make( IEnumerable<decimal> xs )
        {
            return Make( xs.Select( Convert.ToDouble ) );
        }

        /// <summary>
        /// Makes a SexpArrayDouble Sexp from a matrix of decimal.
        /// </summary>
        /// <param name="xs">The matrix of decimal.</param>
        /// <param name="rowNames">Matrix row names</param>
        /// <param name="colNames">Matrix column names</param>
        public static Sexp Make( decimal[ , ] xs , IEnumerable<string> rowNames = null , IEnumerable<string> colNames = null )
        {
            var xsDouble = new double[ xs.GetLength( 0 ) , xs.GetLength( 1 ) ];
            var rows = xs.GetLength( 0 );
            var cols = xs.GetLength( 1 );
            for ( int row = 0 ; row < rows ; row++ )
            {
                for ( int col = 0 ; col < cols ; col++ )
                {
                    xsDouble[ row , col ] = Convert.ToDouble( xs[ row , col ] );
                }
            }
            return Make( xsDouble , rowNames , colNames );
        }

        /// <summary>
        /// Makes a SexpArrayDouble from a double.
        /// </summary>
        /// <param name="x">The double to convert into an SexpArrayDouble.</param>
        public static Sexp Make( double x )
        {
            return new SexpArrayDouble( x );
        }

        /// <summary>
        /// Makes a SexpArrayDouble from an IEnumerable of double.
        /// </summary>
        /// <param name="xs">The IEnumerable of double to convert into an SexpArrayDouble.</param>
        public static Sexp Make( IEnumerable<double> xs )
        {
            return new SexpArrayDouble( xs );
        }

        /// <summary>
        /// Makes a SexpArrayDouble from a matrix of double.
        /// </summary>
        /// <param name="xs">The matrix of double.</param>
        /// <param name="rowNames">Matrix row names</param>
        /// <param name="colNames">Matrix column names</param>
        public static Sexp Make( double[ , ] xs , IEnumerable<string> rowNames = null , IEnumerable<string> colNames = null )
        {
            var rows = xs.GetLength( 0 );
            var cols = xs.GetLength( 1 );
            var fortranXs = new double[ rows * cols ];
            for ( int row = 0 ; row < rows ; row++ )
            {
                for ( int col = 0 ; col < cols ; col++ )
                {
                    fortranXs[ ( col * rows ) + row ] = xs[ row , col ];
                }
            }
            var res = new SexpArrayDouble( fortranXs );
            res.Attributes.Add( "dim" , Make( new[] { rows , cols } ) );
            AddDimNamesAttribute( res , rows , cols , rowNames , colNames );
            return res;
        }

        /// <summary>
        /// Makes a SexpArrayInt from a matrix of int.
        /// </summary>
        /// <param name="xs">The matrix of int.</param>
        /// <param name="rowNames">Matrix row names</param>
        /// <param name="colNames">Matrix column names</param>
        public static Sexp Make( int[ , ] xs , IEnumerable<string> rowNames = null , IEnumerable<string> colNames = null )
        {
            var rows = xs.GetLength( 0 );
            var cols = xs.GetLength( 1 );
            var fortranXs = new int[ rows * cols ];
            for ( int row = 0 ; row < rows ; row++ )
            {
                for ( int col = 0 ; col < cols ; col++ )
                {
                    fortranXs[ ( col * rows ) + row ] = xs[ row , col ];
                }
            }

            var res = new SexpArrayInt( fortranXs );
            res.Attributes.Add( "dim" , Make( new[] { rows , cols } ) );
            AddDimNamesAttribute( res , rows , cols , rowNames , colNames );
            return res;
        }

        /// <summary>
        /// Makes a SexpString from a string.
        /// </summary>
        /// <param name="x">The string to convert into an SexpString.</param>
        public static Sexp Make( string x )
        {
            return new SexpString( x );
        }

        /// <summary>
        /// Makes a SexpArrayString from an IEnumerable of string.
        /// </summary>
        /// <param name="xs">The IEnumerable of string to convert into an SexpArrayString.</param>
        public static Sexp Make( IEnumerable<string> xs )
        {
            return new SexpArrayString( xs );
        }

        /// <summary>
        /// Makes a SexpList from a Dictionary.
        /// </summary>
        /// <param name="xs">The Dictionary to convert into an SexpList.</param>
        public static Sexp Make( IDictionary<string , object> xs )
        {
            var res = new SexpList();
            foreach ( var a in xs )
            {
                res.Add( a );
            }
            return res;
        }

        /// <summary>
        /// Makes a data frame.
        /// </summary>
        /// <param name="columns">
        /// The columns.
        /// </param>
        /// <param name="rowNames">
        /// The row names.
        /// </param>
        /// <returns>
        /// Sexp of data frame
        /// </returns>
        public static SexpList MakeDataFrame(
            IEnumerable<KeyValuePair<string , object>> columns = null , IEnumerable<string> rowNames = null )
        {
            var res = new SexpList();
            res.Attributes[ "class" ] = new SexpString( "data.frame" );
            res.Attributes[ "names" ] = new SexpArrayString();
            if ( columns != null )
            {
                int? rows = null;
                foreach ( var col in columns )
                {
                    // make the column
                    res.Attributes[ "names" ].Add( new SexpString( col.Key ) );
                    Sexp column = Make( col.Value );

                    // must be an SexpArray type otherwise it's not a data.frame.  Technically it could be an SexpBool, SexpInt, etc. but too cumbersome to check all of those types
                    if ( !column.GetType().ToString().Contains( "SexpArray" ) )
                    {
                        throw new NotSupportedException( "Can only build data.frame with SexpArray types" );
                    }

                    // each column must have the same number of rows.  
                    // In R you can can do something like data.frame( A = c( 1 , 2 , 3 ) , B = "Test" ) and B will be replicated.
                    // but this library does not support that convenience feature
                    if ( rows == null )
                    {
                        rows = column.Count;
                    }
                    else if ( column.Count != rows )
                    {
                        throw new NotSupportedException( string.Format( "arguments imply differing number of rows: {0}, {1}" , rows , column.Count ) );
                    }

                    res.Add( column );
                }

                if ( rowNames == null )
                {
                    // without this, data.frames will look like they have 0 observations
                    // ReSharper disable PossibleInvalidOperationException
                    res.Attributes[ "row.names" ] = new SexpArrayInt( new List<int> { SexpInt.Na.Value , -1 * ( int )rows } );
                    // ReSharper restore PossibleInvalidOperationException
                }
                else if ( rowNames.Count() != rows )
                {
                    throw new NotSupportedException( "invalid 'row.names' length" );
                }
                else
                {
                    res.Attributes[ "row.names" ] = new SexpArrayString( rowNames );
                }
            }

            return res;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="System.Object"/> to compare with this instance.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals( object obj )
        {
            throw new NotSupportedException( "Don't have an equality override for type" + GetType() );
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Gets the length of an array-like object with respect to a given dimension.
        /// </summary>
        /// <param name="dim">
        /// The zero-based index of the dimension.
        /// </param>
        /// <returns>
        /// Length of the object in the dimension requested.
        /// </returns>
        public virtual int GetLength( int dim )
        {
            return Attributes[ "dim" ][ dim ].AsInt;
        }

        /// <summary>
        /// Converts the Sexp into the most appropriate native representation. Use with caution--this is more a rapid prototyping than
        /// a production feature.
        /// </summary>
        /// <returns>
        /// A CLI native representation of the Sexp
        /// </returns>
        public virtual object ToNative()
        {
            throw new NotSupportedException();
        }

        #endregion

        #region Implemented Interfaces

        #region ICollection<KeyValuePair<string,object>>

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">
        /// The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </param>
        public void Add( KeyValuePair<string , object> item )
        {
            Add( new KeyValuePair<string , Sexp>( item.Key , Make( item.Value ) ) );
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </param>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        public bool Contains( KeyValuePair<string , object> item )
        {
            return Contains( new KeyValuePair<string , Sexp>( item.Key , Make( item.Value ) ) );
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">
        /// The array.
        /// </param>
        /// <param name="arrayIndex">
        /// Index of the array.
        /// </param>
        public void CopyTo( KeyValuePair<string , object>[] array , int arrayIndex )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">
        /// The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </param>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </exception>
        public bool Remove( KeyValuePair<string , object> item )
        {
            return Remove( new KeyValuePair<string , Sexp>( item.Key , Make( item.Value ) ) );
        }

        #endregion

        #region ICollection<KeyValuePair<string,Sexp>>

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">
        /// The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </param>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </exception>
        public virtual void Add( KeyValuePair<string , Sexp> item )
        {
            Add( item.Key , item.Value );
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </param>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        public virtual bool Contains( KeyValuePair<string , Sexp> item )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">
        /// The array.
        /// </param>
        /// <param name="arrayIndex">
        /// Index of the array.
        /// </param>
        public virtual void CopyTo( KeyValuePair<string , Sexp>[] array , int arrayIndex )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">
        /// The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </param>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </exception>
        public virtual bool Remove( KeyValuePair<string , Sexp> item )
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICollection<object>

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">
        /// The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </param>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </exception>
        public void Add( object item )
        {
            Add( Make( item ) );
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </param>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        public bool Contains( object item )
        {
            return Contains( Make( item ) );
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">
        /// The array.
        /// </param>
        /// <param name="arrayIndex">
        /// Index of the array.
        /// </param>
        public void CopyTo( object[] array , int arrayIndex )
        {
            CopyTo( array , arrayIndex );
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">
        /// The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </param>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </exception>
        public bool Remove( object item )
        {
            return Remove( Make( item ) );
        }

        #endregion

        #region ICollection<Sexp>

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">
        /// The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </param>
        public virtual void Add( Sexp item )
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </exception>
        public virtual void Clear()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </param>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        public virtual bool Contains( Sexp item )
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">
        /// The array.
        /// </param>
        /// <param name="arrayIndex">
        /// Index of the array.
        /// </param>
        public virtual void CopyTo( Sexp[] array , int arrayIndex )
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">
        /// The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </param>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </exception>
        public virtual bool Remove( Sexp item )
        {
            throw new NotSupportedException();
        }

        #endregion

        #region IDictionary<string,object>

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <param name="key">
        /// The object to use as the key of the element to add.
        /// </param>
        /// <param name="value">
        /// The object to use as the value of the element to add.
        /// </param>
        public void Add( string key , object value )
        {
            Add( key , Make( value ) );
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">
        /// The key whose value to get.
        /// </param>
        /// <param name="value">
        /// When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        /// true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key; otherwise, false.
        /// </returns>
        public bool TryGetValue( string key , out object value )
        {
            return TryGetValue( key , out value );
        }

        #endregion

        #region IDictionary<string,Sexp>

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <param name="key">
        /// The object to use as the key of the element to add.
        /// </param>
        /// <param name="value">
        /// The object to use as the value of the element to add.
        /// </param>
        public virtual void Add( string key , Sexp value )
        {
            if ( Count == 0 && !Attributes.ContainsKey( "names" ) )
            {
                Attributes[ "names" ] = new SexpArrayString();
            }

            Add( value );
            Names.Add( new SexpString( key ) );
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key.
        /// </summary>
        /// <param name="key">
        /// The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </param>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the key; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="key"/> is null.
        /// </exception>
        public virtual bool ContainsKey( string key )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </summary>
        /// <param name="key">
        /// The key of the element to remove.
        /// </param>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key"/> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"/>.
        /// </returns>
        public virtual bool Remove( string key )
        {
            if ( Count == 0 )
            {
                return false;
            }

            var index = Array.IndexOf( Names.AsStrings , key );
            if ( index < 0 )
            {
                return false;
            }

            RemoveAt( index );
            Names.RemoveAt( index );
            return true;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">
        /// The key whose value to get.
        /// </param>
        /// <param name="value">
        /// When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        /// true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"/> contains an element with the specified key; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="key"/> is null.
        /// </exception>
        public virtual bool TryGetValue( string key , out Sexp value )
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IEnumerable<KeyValuePair<string,object>>

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<KeyValuePair<string , object>> IEnumerable<KeyValuePair<string , object>>.GetEnumerator()
        {
            return
                ( IEnumerator<KeyValuePair<string , object>> )
                ( ( IEnumerable<KeyValuePair<string , Sexp>> )this ).GetEnumerator();
        }

        #endregion

        #region IEnumerable<KeyValuePair<string,Sexp>>

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<KeyValuePair<string , Sexp>> IEnumerable<KeyValuePair<string , Sexp>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable<object>

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<object> IEnumerable<object>.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IEnumerable<Sexp>

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public virtual IEnumerator<Sexp> GetEnumerator()
        {
            throw new NotSupportedException();
        }

        #endregion

        #region IList<object>

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </param>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf( object item )
        {
            return IndexOf( Make( item ) );
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index at which <paramref name="item"/> should be inserted.
        /// </param>
        /// <param name="item">
        /// The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </param>
        public void Insert( int index , object item )
        {
            Insert( index , Make( item ) );
        }

        #endregion

        #region IList<Sexp>

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </param>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        public virtual int IndexOf( Sexp item )
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index at which <paramref name="item"/> should be inserted.
        /// </param>
        /// <param name="item">
        /// The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </param>
        public virtual void Insert( int index , Sexp item )
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the item to remove.
        /// </param>
        public virtual void RemoveAt( int index )
        {
            throw new NotSupportedException();
        }

        #endregion

        #endregion

        #region Private Members

        /// <summary>
        /// Adds dimnames attribute to an Sexp
        /// </summary>
        private static void AddDimNamesAttribute( Sexp data , int rows , int cols , IEnumerable<string> rowNames = null , IEnumerable<string> colNames = null )
        {
            if ( ( rowNames != null ) || ( colNames != null ) )
            {
                var dimnames = new SexpList();
                if ( rowNames == null )
                {
                    dimnames.Add( new SexpNull() );
                }
                else
                {
                    if ( rows != rowNames.Count() )
                    {
                        throw new NotSupportedException( "length of 'dimnames' [1] not equal to array extent" );
                    }
                    dimnames.Add( Make( rowNames ) );
                }

                if ( colNames == null )
                {
                    dimnames.Add( new SexpNull() );
                }
                else
                {
                    if ( cols != colNames.Count() )
                    {
                        throw new NotSupportedException( "length of 'dimnames' [2] not equal to array extent" );
                    }
                    dimnames.Add( Make( colNames ) );
                }
                data.Attributes.Add( "dimnames" , dimnames );
            }
        }

        #endregion

    }
}