//-----------------------------------------------------------------------
// Copyright (c) 2011, Oliver M. Haynold
// All rights reserved.
//-----------------------------------------------------------------------

using System;

namespace RserveCLI2
{

    /// <summary>
    /// A Sexp for Symnames. Same as a string, but can't be NA.
    /// </summary>
    [Serializable]
    public class SexpSymname : Sexp
    {

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the SexpSymname class with a string.
        /// </summary>
        public SexpSymname( string theValue )
        {
            Value = theValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets as string.
        /// </summary>
        public override string AsString
        {
            get
            {
                return Value;
            }
        }

        /// <summary>
        /// Gets as array of string.
        /// </summary>
        public override string[] AsStrings
        {
            get
            {
                return new [] { Value };
            }
        }

        /// <summary>
        /// Gets the value of the symname
        /// </summary>
        internal string Value { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts the Sexp into the most appropriate native representation. 
        /// Use with caution--this is more a rapid prototyping than a production feature.
        /// </summary>
        /// <returns>
        /// A CLI native representation of the Sexp
        /// </returns>
        public override object ToNative()
        {
            return Value;
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>
        /// A string that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Value;
        }

        #endregion
    }
}
