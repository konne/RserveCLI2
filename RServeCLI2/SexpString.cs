//-----------------------------------------------------------------------
// Original work Copyright (c) 2011, Oliver M. Haynold
// Modified work Copyright (c) 2013, Suraj Gupta
// All rights reserved.
//-----------------------------------------------------------------------

namespace RserveCLI2
{
    using System;

    /// <summary>
    /// A convenience representation for R string values. Rserve never uses this type,
    /// but puts even a single number into a SexpArrayString.
    /// </summary>
    public class SexpString : Sexp
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SexpString"/> class.
        /// </summary>
        /// <param name="theValue">
        /// The value.
        /// </param>
        public SexpString( string theValue )
        {
            Value = theValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the NA representation of strings.
        /// </summary>
        public static SexpString Na
        {
            get
            {
                return new SexpString( null );
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
                return Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is NA.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is NA; otherwise, <c>false</c>.
        /// </value>
        public override bool IsNa
        {
            get
            {
                return Value == null;
            }
        }

        /// <summary>
        /// Gets the value of the Sexp
        /// </summary>
        internal string Value { get; private set; }
        #endregion

        #region Public Methods

        /// <summary>
        /// Checks whether a value is NA.
        /// </summary>
        /// <param name="x">
        /// The value to be checked.
        /// </param>
        /// <returns>
        /// True if the value is NA, false otherwise.
        /// </returns>
        public static bool CheckNa( string x )
        {
            return x == null;
        }

        /// <summary>
        /// Checks whether a value is NA.
        /// </summary>
        /// <param name="x">
        /// The value to be checked.
        /// </param>
        /// <returns>
        /// True if the value is NA, false otherwise.
        /// </returns>
        public static bool CheckNa( SexpString x )
        {
            return x.Value == null;
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
            if ( obj is Sexp )
            {
                return Value.Equals( ( ( Sexp )obj ).AsString );
            }

            return Value.Equals( Convert.ChangeType( obj , typeof( string ) ) );
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Converts the Sexp into the most appropriate native representation. Use with caution--this is more a rapid prototyping than
        /// a production feature.
        /// </summary>
        /// <returns>
        /// A CLI native representation of the Sexp
        /// </returns>
        public override object ToNative()
        {
            return Value;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return IsNa ? "NA" : Value;
        }

        #endregion
    }
}
