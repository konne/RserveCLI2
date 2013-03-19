// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SexpSymname.cs" company="Oliver M. Haynold">
//   Copyright (c) 2011, Oliver M. Haynold
// All rights reserved.
// </copyright>
// <summary>
// A Sexp for Symnames. Same as a string, but can't be NA.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RserveCLI2
{
    /// <summary>
    /// A Sexp for Symnames. Same as a string, but can't be NA.
    /// </summary>
    public class SexpSymname : Sexp
    {
        #region Constants and Fields

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SexpSymname"/> class.
        /// </summary>
        /// <param name="theValue">
        /// The value.
        /// </param>
        public SexpSymname( string theValue )
        {
            Value = theValue;
        }

        #endregion

        #region Properties

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
        /// Gets the value of the symname
        /// </summary>
        internal string Value { get; private set; }

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
            return Value;
        }

        #endregion
    }
}
