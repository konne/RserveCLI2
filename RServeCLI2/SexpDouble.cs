// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SexpDouble.cs" company="Oliver M. Haynold">
//   Copyright (c) 2011, Oliver M. Haynold
// All rights reserved.
// </copyright>
// <summary>
// A convenience representation for R double values. Rserve never uses this type,
// but puts even a single number into a SexpArrayDouble.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RserveCli
{
    using System;
    using System.Linq;

    /// <summary>
    /// A convenience representation for R double values. Rserve never uses this type,
    /// but puts even a single number into a SexpArrayDouble.
    /// </summary>
    public class SexpDouble : Sexp
    {
        #region Constants and Fields

        /// <summary>
        /// This is how NA gets represented. This is NaN, but not every NaN is like  The bit pattern might vary between platforms.
        /// </summary>
        private static readonly byte[] Nabytes = BitConverter.GetBytes( BitConverter.ToDouble( new byte[] { 162 , 7 , 0 , 0 , 0 , 0 , 240 , 127 } , 0 ) );

        /// <summary>
        /// The double that means NA.
        /// </summary>
        private static readonly double NaValue = BitConverter.ToDouble( Nabytes , 0 );

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SexpDouble"/> class.
        /// </summary>
        /// <param name="theValue">
        /// The value.
        /// </param>
        public SexpDouble( double theValue )
        {
            Value = theValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the NA value of double.
        /// </summary>
        public static SexpDouble Na
        {
            get
            {
                return new SexpDouble( NaValue );
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
                if ( Math.Floor( Value ) != Value )
                {
                    throw new ArgumentException(
                        "Can only force integer values to integer type. Try to avoid using " );
                }

                return ( int )Value;
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
                return BitConverter.GetBytes( Value ).SequenceEqual( Nabytes );
            }
        }

        /// <summary>
        /// Gets the Sexp's value or a special value for NA
        /// </summary>
        internal double Value { get; private set; }
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
        public static bool CheckNa( double x )
        {
            return BitConverter.GetBytes( x ).SequenceEqual( Nabytes );
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
        public static bool CheckNa( SexpDouble x )
        {
            return BitConverter.GetBytes( x.Value ).SequenceEqual( Nabytes );
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

                return Value.Equals( ( ( Sexp )obj ).AsDouble );
            }

            return Value.Equals( Convert.ChangeType( obj , typeof( double ) ) );
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
