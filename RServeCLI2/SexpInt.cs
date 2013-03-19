// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SexpInt.cs" company="Oliver M. Haynold">
// Copyright (c) 2011, Oliver M. Haynold
// All rights reserved.
// </copyright>
// <summary>
// A convenience representation for R integer values. Rserve never uses this type,
// but puts even a single number into a SexpArrayInt.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RserveCli
{
    using System;

    /// <summary>
    /// A convenience representation for R integer values. Rserve never uses this type,
    /// but puts even a single number into a SexpArrayInt.
    /// </summary>
    public class SexpInt : Sexp
    {
        #region Constants and Fields

        /// <summary>
        /// The special values that marks an integer as NA.
        /// </summary>
        internal static readonly int NaValue = -2147483648;
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SexpInt"/> class.
        /// </summary>
        /// <param name="theValue">
        /// The value.
        /// </param>
        public SexpInt( int theValue )
        {
            Value = theValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a representation of the NA value.
        /// </summary>
        public static SexpInt Na
        {
            get
            {
                return new SexpInt( NaValue );
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
                return Value;
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
                if ( IsNa )
                {
                    throw new ArithmeticException( "Cannot convert NA to int." );
                }

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
                return Value == NaValue;
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        internal int Value { get; private set; }

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
        public static bool CheckNa( int x )
        {
            return x == NaValue;
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
        public static bool CheckNa( SexpInt x )
        {
            return x.Value == NaValue;
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
                if ( IsNa || ( ( Sexp )obj ).IsNa || ( ( Sexp )obj ).IsNull )
                {
                    return false;
                }

                return Value.Equals( ( ( Sexp )obj ).AsInt );
            }

            return Value.Equals( Convert.ChangeType( obj , typeof( int ) ) );
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
            return IsNa ? "NA" : Value.ToString();
        }

        #endregion
    }
}
