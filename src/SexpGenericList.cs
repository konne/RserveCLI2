//-----------------------------------------------------------------------
// Original work Copyright (c) 2011, Oliver M. Haynold
// Modified work Copyright (c) 2013, Suraj Gupta
// All rights reserved.
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.Text;

namespace RserveCLI2
{
    /// <summary>
    /// Functionality for list-like Sexps.
    /// </summary>
    [Serializable]
    public abstract class SexpGenericList : Sexp
    {

        #region Public Methods

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified object is equal to this instance; otherwise, <c>false</c>.
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
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>
        /// A string that represents this instance.
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

    }
}