// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SexpArrayDouble.cs" company="Oliver M. Haynold">
//   Copyright (c) 2011, Oliver M. Haynold
// All rights reserved.
// </copyright>
// <summary>
// An array of double-precision floating-point values
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace RserveCLI2
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An array of double-precision floating-point values
    /// </summary>
    public class SexpArrayDouble : SexpGenericList
    {
        #region Constants and Fields

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SexpArrayDouble"/> class.
        /// </summary>
        public SexpArrayDouble()
        {
            Value = new List<double>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SexpArrayDouble"/> class.
        /// </summary>
        /// <param name="theValue">
        /// The value.
        /// </param>
        public SexpArrayDouble( IEnumerable<double> theValue )
        {
            Value = new List<double>();
            Value.AddRange( theValue );
        }

        #endregion

        #region Properties

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
        /// Gets the doubles stored in the list
        /// </summary>
        internal List<double> Value { get; private set; }
        
        #endregion

        #region Indexers

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element</param>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        public override Sexp this[ int index ]
        {
            get
            {
                return new SexpDouble( Value[ index ] );
            }

            set
            {
                Value[ index ] = value.AsDouble;
            }
        }

        /// <summary>
        /// Gets the values as a 2-dimensional array
        /// </summary>
        /// <remarks>
        /// This method will only work if the Sexp was originally constructed using a 2-dimensional array.
        /// </remarks>
        public override double[ , ] As2DArrayDouble
        {
            get
            {
                if ( !Attributes.ContainsKey( "dim" ) )
                {
                    throw new NotSupportedException( "Sexp does not have the dim attribute." );
                }
                if ( Rank == 2 )
                {
                    // if GetLength fails it means the user screwed around with the dim attribute
                    int rows = GetLength( 0 );
                    int cols = GetLength( 1 );
                    var result = new double[ rows , cols ];
                    for ( int row = 0 ; row < rows ; row++ )
                    {
                        for ( int col = 0 ; col < cols ; col++ )
                        {
                            result[ row , col ] = Value[ ( col * rows ) + row ];
                        }
                    }
                    return result;
                }
                throw new NotSupportedException( "Sexp does not have 2 dimension." );
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
            Value.Add( item.AsInt );
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
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </param>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        public override bool Contains( Sexp item )
        {
            return Value.Contains( item.AsInt );
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
                array[ arrayIndex + i ] = new SexpDouble( Value[ i ] );
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
            return ( from a in Value select ( Sexp )( new SexpDouble( a ) ) ).GetEnumerator();
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
            return item != null ? Value.IndexOf( item.IsNa ? SexpDouble.Na.AsDouble : item.AsDouble ) : -1;
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
            Value.Insert( index , item.AsInt );
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">
        /// The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </param>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </exception>
        public override bool Remove( Sexp item )
        {
            return Value.Remove( item.AsInt );
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
