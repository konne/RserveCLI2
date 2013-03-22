// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SexpArrayBool.cs" company="Oliver M. Haynold">
//   Copyright (c) 2011, Oliver M. Haynold
// All rights reserved.
// </copyright>
// <summary>
// An array of (trivalue, i.e., including NA) booleans.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RserveCLI2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An array of (trivalue, i.e., including NA) booleans.
    /// </summary>
    public class SexpArrayBool : SexpGenericList
    {

        #region Constants and Fields

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of SexpArrayBool.
        /// </summary>
        public SexpArrayBool()
        {
            Value = new List<SexpBoolValue>();
        }

        /// <summary>
        /// Initializes a new instance of SexpArrayBool with a bool.
        /// </summary>
        public SexpArrayBool( bool theValue )
        {
            Value = new List<SexpBoolValue> { theValue ? SexpBoolValue.True : SexpBoolValue.False };
        }

        /// <summary>
        /// Initializes a new instance of SexpArrayBool with an IEnumerable of SexpBoolValue.
        /// </summary>
        public SexpArrayBool( IEnumerable<SexpBoolValue> theValue )
        {
            Value = new List<SexpBoolValue>();
            Value.AddRange( theValue );
        }

        #endregion

        #region Properties

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
                if ( Value.Count == 1 )
                {
                    return new SexpBool( Value[ 0 ] ).AsBool;
                }

                throw new IndexOutOfRangeException( "Can only convert numeric arrays of length 1 to double." );
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public override int Count
        {
            get
            {
                return Value.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the values stored in the list
        /// </summary>
        internal List<SexpBoolValue> Value { get; private set; }
        #endregion

        #region Indexers

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the element.</param>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        public override Sexp this[ int index ]
        {
            get
            {
                return new SexpBool( Value[ index ] );
            }

            set
            {
                Value[ index ] = value.AsSexpBool;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">
        /// The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </param>
        public override void Add( Sexp item )
        {
            Value.Add( item.AsSexpBool );
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </exception>
        public override void Clear()
        {
            Value.Clear();
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">
        /// The array.
        /// </param>
        /// <param name="arrayIndex">
        /// Index of the array.
        /// </param>
        public override void CopyTo( Sexp[] array , int arrayIndex )
        {
            for ( int i = 0 ; i < Value.Count ; i++ )
            {
                array[ arrayIndex + i ] = new SexpBool( Value[ i ] );
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public override IEnumerator<Sexp> GetEnumerator()
        {
            return ( from a in Value select ( Sexp )( new SexpBool( a ) ) ).GetEnumerator();
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </param>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        public override int IndexOf( Sexp item )
        {
            return Value.IndexOf( item.IsNa ? SexpBool.Na : item.AsSexpBool );
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index at which <paramref name="item"/> should be inserted.
        /// </param>
        /// <param name="item">
        /// The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </param>
        public override void Insert( int index , Sexp item )
        {
            Value.Insert( index , item.AsSexpBool );
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the item to remove.
        /// </param>
        public override void RemoveAt( int index )
        {
            Value.RemoveAt( index );
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
            return Value.ToArray();
        }

        #endregion
    }
}
