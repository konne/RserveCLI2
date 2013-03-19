// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SexpDate.cs" company="Suraj K. Gupta">
//   Copyright (c) 2013, Suraj K. Gupta
// All rights reserved.
// </copyright>
// <summary>
// A convenience representation for R date values. Note that Rserve never uses this type,
// but puts even a single number into a SexpArrayInt.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace RserveCli
{

    /// <summary>
    /// A convenience representation for R date values. Note that Rserve never uses this type,
    /// but puts even a single number into a SexpArrayInt.
    /// </summary>
    public class SexpDate : SexpInt
    {

        #region Constants and Fields

        /// <summary>
        /// The origin for R's conversion of dates to ints
        /// </summary>
        private static readonly DateTime Origin = new DateTime( 1970 , 1 , 1 );

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SexpInt"/> class.
        /// </summary>
        /// <param name="theValue">
        /// The value.
        /// </param>
        public SexpDate( DateTime theValue )
            : base( theValue.Date.Subtract( Origin ).Days )
        {
            Attributes[ "class" ] = new SexpString( "Date" );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SexpInt"/> class.
        /// </summary>
        /// <param name="theValue">
        /// The value.
        /// </param>
        public SexpDate( int theValue ) : base( theValue )
        {
            Attributes[ "class" ] = new SexpString( "Date" );
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets as Date.
        /// </summary>
        /// <value>
        /// The value as an integer.
        /// </value>
        public override DateTime AsDate
        {
            get
            {
                if ( IsNa )
                {
                    throw new ArithmeticException( "Cannot convert NA to DateTime." );
                }
                return Value;
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        internal new DateTime Value
        {
            get
            {
                return Origin.AddDays( base.Value );
            }
        }

        #endregion
        
        #region Public Methods

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
            return IsNa ? "NA" : Value.ToShortDateString();
        }

        #endregion

    }
}