//-----------------------------------------------------------------------
// Copyright (c) 2011, Oliver M. Haynold
// All rights reserved.
//-----------------------------------------------------------------------

using System;

namespace RserveCLI2
{

    /// <summary>
    /// A container for Sexps that we don't know how to interpret. This way you can still pass such a Sexp back to Rserve, even though its meaning
    /// remains opaque.
    /// </summary>
    [Serializable]
    public class SexpQap1Raw : Sexp
    {

        # region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SexpQap1Raw"/> class.
        /// </summary>
        /// <param name="type">
        /// The type of the Sexp as represented by Rserve.
        /// </param>
        /// <param name="data">
        /// The data of the Sexp. Its meaning is opaque.
        /// </param>
        public SexpQap1Raw( byte type , byte[] data )
        {
            Data = data;
            Type = type;
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets the data of the Sexp as sent by Rserve
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// Gets the XT_... type of the Sexp
        /// </summary>
        public byte Type { get; private set; }

        #endregion

        #region Public Methods
        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>
        /// A string that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format( "[SexpQap1Raw (" + Type + ")]" );
        }

        #endregion
    }
}
