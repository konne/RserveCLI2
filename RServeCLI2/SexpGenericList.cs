//-----------------------------------------------------------------------
// Original work Copyright (c) 2011, Oliver M. Haynold
// Modified work Copyright (c) 2013, Suraj Gupta
// All rights reserved.
//-----------------------------------------------------------------------

namespace RserveCLI2
{
    using System;
    using System.Collections;
    using System.Text;

    /// <summary>
    /// Functionality for list-like Sexps.
    /// </summary>
    public abstract class SexpGenericList : Sexp
    {

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether [as bool].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [as bool]; otherwise, <c>false</c>.
        /// </value>
        public override bool? AsBool
        {
            get
            {
                return AssertOne()[ 0 ].AsBool;
            }
        }

        /// <summary>
        /// Gets or sets as double.
        /// </summary>
        /// <value>
        /// As double.
        /// </value>
        public override double AsDouble
        {
            get
            {
                return AssertOne()[ 0 ].AsDouble;
            }
        }

        /// <summary>
        /// Gets or sets as int.
        /// </summary>
        /// <value>
        /// The value as an integer.
        /// </value>
        public override int AsInt
        {
            get
            {
                return AssertOne()[ 0 ].AsInt;
            }
        }

        /// <summary>
        /// Gets or sets as string.
        /// </summary>
        /// <value>
        /// As string.
        /// </value>
        public override string AsString
        {
            get
            {
                return AssertOne()[ 0 ].AsString;
            }
        }

        #endregion

        #region Public Methods

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
            Sexp o = Make( obj );
            if ( o.Count != Count )
            {
                return false;
            }

            for ( int i = 0 ; i < Count ; i++ )
            {
                if ( !this[ i ].Equals( o[ i ] ) )
                {
                    return false;
                }
            }

            return true;
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
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var a = new StringBuilder();
            a.Append( "[ " );
            foreach ( object b in ( IEnumerable )this )
            {
                a.Append( b.ToString() );
                a.Append( ", " );
            }

            a.Remove( a.Length - 2 , 2 );
            a.Append( " ]" );
            return a.ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Asserts that the list has exactly one element.
        /// </summary>
        /// <returns>
        /// The single sexp contained in the list
        /// </returns>
        private Sexp AssertOne()
        {
            if ( Count != 1 )
            {
                throw new Exception( "I can only compare the equality of lists for ones that have exactly one member." );
            }

            return this;
        }

        #endregion

    }
}
