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
    /// A List that can contain Sexps of various types. This is the basis for data frames, too.
    /// </summary>
    [Serializable]
    public class SexpList : SexpGenericList
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of SexpList.
        /// </summary>
        public SexpList()
        {
            Value = new List<Sexp>();
        }

        /// <summary>
        /// Initializes a new instance SexpList with an IEnumerable of Sexp
        /// </summary>
        public SexpList( IEnumerable<Sexp> theValue )
        {
            Value = new List<Sexp>();
            Value.AddRange( theValue );
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of elements contained in the ICollection.
        /// </summary>
        public override int Count
        {
            get
            {
                return Value.Count;
            }
        }

        /// <summary>
        /// Determines if the ICollection is read-only.
        /// </summary>
        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets Value.
        /// </summary>
        internal List<Sexp> Value { get; private set; }

        #endregion

        #region Indexers

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to be retrieved</param>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        public override Sexp this[ int index ]
        {
            get
            {
                return Value[ index ];
            }

            set
            {
                Value[ index ] = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds an item to the ICollection.
        /// </summary>
        /// <param name="item">The object to add to the ICollection.</param>
        public override void Add( Sexp item )
        {
            Value.Add( item );
        }

        /// <summary>
        /// Removes all items from the ICollection.
        /// </summary>
        public override void Clear()
        {
            Value.Clear();
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
            return Value.Contains( item );
        }

        /// <summary>
        /// Copies the elements of the ICollection to an Array, starting at a particular Array index.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from ICollection. The Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        public override void CopyTo( Sexp[] array , int arrayIndex )
        {
            Value.CopyTo( array , arrayIndex );
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A IEnumerator that can be used to iterate through the collection.
        /// </returns>
        public override IEnumerator<Sexp> GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        /// <summary>
        /// Determines the index of a specific item in the IList.
        /// </summary>
        /// <param name="item">The object to locate in the IList.</param>
        /// <returns>
        /// The index of item if found in the list; otherwise, -1.
        /// </returns>
        public override int IndexOf( Sexp item )
        {
            return Value.IndexOf( item );
        }

        /// <summary>
        /// Inserts an item to the IList at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">
        /// The object to insert into the IList.
        /// </param>
        public override void Insert( int index , Sexp item )
        {
            Value.Insert( index , item );
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the ICollection.
        /// </summary>
        /// <param name="item">The object to remove from the ICollection.</param>
        /// <returns>
        /// true if item was successfully removed from the ICollection; otherwise, false. This method also returns false if item is not found in the original ICollection.
        /// </returns>
        public override bool Remove( Sexp item )
        {
            return Value.Remove( item );
        }

        /// <summary>
        /// Removes the IList item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public override void RemoveAt( int index )
        {
            Value.RemoveAt( index );
        }

        #endregion

    }
}
