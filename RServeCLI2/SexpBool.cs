// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SexpBool.cs" company="Oliver M. Haynold">
//   Copyright (c) 2011, Oliver M. Haynold
// All rights reserved.
// </copyright>
// <summary>
// A convenience representation for R boolean values. Rserve never uses this type,
// but puts even a single number into a SexpArrayBool.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RserveCli
{
    using System;

    /// <summary>
    /// The values a bool values in a Sexp can take. Normal boolean logic and NA.
    /// </summary>
    public enum SexpBoolValue : byte
    {
        /// <summary>
        /// The boolean FALSE value
        /// </summary>
        False = 0 ,

        /// <summary>
        /// The boolean TRUE value
        /// </summary>
        True = 1 ,

        /// <summary>
        /// The NA value
        /// </summary>
        Na = 2
    }

    /// <summary>
    /// A convenience representation for R boolean values. Rserve never uses this type,
    /// but puts even a single number into a SexpArrayBool.
    /// </summary>
    public class SexpBool : Sexp
    {
        #region Constants and Fields
        /// <summary>
        /// The representation of NA
        /// </summary>
        private const SexpBoolValue NaValue = SexpBoolValue.Na;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SexpBool"/> class.
        /// </summary>
        /// <param name="theValue">
        /// The value.
        /// </param>
        public SexpBool( SexpBoolValue theValue )
        {
            if ( theValue > SexpBoolValue.Na )
            {
                throw new ArgumentOutOfRangeException( "theValue" );
            }

            Value = theValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SexpBool"/> class.
        /// </summary>
        /// <param name="theValue">
        /// if set to <c>true</c> [the value].
        /// </param>
        public SexpBool( bool theValue )
        {
            Value = theValue ? SexpBoolValue.True : SexpBoolValue.False;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the representation of NA
        /// </summary>
        public static SexpBoolValue Na
        {
            get
            {
                return NaValue;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [as bool].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [as bool]; otherwise, <c>false</c>.
        /// </value>
        public override bool AsBool
        {
            get
            {
                return Value == SexpBoolValue.True;
            }
        }

        /// <summary>
        /// Gets or sets as sexp bool.
        /// </summary>
        /// <value>
        /// As sexp bool.
        /// </value>
        public override SexpBoolValue AsSexpBool
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
                return Value == SexpBoolValue.Na;
            }
        }

        /// <summary>
        /// Gets the value of the Sexp
        /// </summary>
        internal SexpBoolValue Value { get; private set; }
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
        public static bool CheckNa( SexpBoolValue x )
        {
            return x == SexpBoolValue.Na;
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
        public static bool CheckNa( SexpBool x )
        {
            return x.Value == SexpBoolValue.Na;
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
            if ( Value == SexpBoolValue.Na )
            {
                return false;
            }

            if ( obj is SexpBoolValue )
            {
                return Value.Equals( ( SexpBoolValue )obj );
            }

            if ( obj is bool )
            {
                return ( Value == SexpBoolValue.True && ( ( bool )obj ) ) ||
                       ( Value == SexpBoolValue.False && !( ( bool )obj ) );
            }

            if ( obj is SexpBool )
            {
                return Equals( ( ( SexpBool )obj ).Value );
            }

            if ( obj is Sexp )
            {
                try
                {
                    var o = ( ( Sexp )obj ).AsSexpBool;
                    return Equals( o );
                }
                catch ( NotSupportedException )
                {
                    return false;
                }
            }

            return Equals( Make( obj ) );
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
            return AsBool;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            switch ( Value )
            {
                case SexpBoolValue.False:
                    return "False";
                case SexpBoolValue.True:
                    return "True";
                case SexpBoolValue.Na:
                    return "NA";
                default:
                    throw new NotSupportedException( "Not a valid SexpBoolValue." );
            }
        }

        #endregion
    }
}
