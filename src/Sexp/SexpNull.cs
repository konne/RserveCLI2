//-----------------------------------------------------------------------
// Original work Copyright (c) 2011, Oliver M. Haynold
// Modified work Copyright (c) 2013, Suraj Gupta
// All rights reserved.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace RserveCLI2
{
    
    /// <summary>
    /// The S NULL Value
    /// </summary>
    [Serializable]
    public class SexpNull : Sexp
    {

        #region Properties

        /// <summary>
        /// Gets the number of elements contained in the ICollection.
        /// </summary>
        public override int Count
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is NA.
        /// </summary>
        /// <value>
        /// true if this instance is NA; otherwise, false
        /// </value>
        public override bool IsNa
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is null.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is null; otherwise, <c>false</c>.
        /// </value>
        public override bool IsNull
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the ICollection is read-only.
        /// </summary>
        /// <returns>true if the ICollection is read-only; otherwise, false.
        /// </returns>
        public override bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the object.</param>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        public override Sexp this[ int index ]
        {
            get
            {
                throw new ArgumentOutOfRangeException();
            }

            set
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Removes all items from the ICollection.
        /// </summary>
        public override void Clear()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Determines whether the ICollection contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the ICollection.</param>
        /// <returns>
        /// true if item is found in the ICollection; otherwise, false.
        /// </returns>
        public override bool Contains( Sexp item )
        {
            return false;
        }

        /// <summary>
        /// Copies the elements of the ICollection to an Array, starting at a particular Array index.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from ICollection. The Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        public override void CopyTo( Sexp[] array , int arrayIndex )
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A IEnumerator that can be used to iterate through the collection.
        /// </returns>
        public override IEnumerator<Sexp> GetEnumerator()
        {
            return new List<Sexp>().GetEnumerator();
        }

        /// <summary>
        /// Converts the Sexp into the most appropriate native representation. 
        /// Use with caution--this is more a rapid prototyping than a production feature.
        /// </summary>
        /// <returns>
        /// A CLI native representation of the Sexp
        /// </returns>
        public override object ToNative()
        {
            return null;
        }

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>
        /// A string that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "NULL";
        }

        #endregion

    }
}
