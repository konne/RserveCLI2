// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SexpArrayDate.cs" company="Suraj K. Gupta">
//   Copyright (c) 2013, Suraj K. Gupta
// All rights reserved.
// </copyright>
// <summary>
// An array of dates.  Time portion of a date is severed.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace RserveCLI2
{

    /// <summary>
    /// An array of Dates.  Time portion of a date is severed.
    /// </summary>
    public class SexpArrayDate : SexpArrayInt
    {

        #region Constants and Fields

        /// <summary>
        /// The origin for R's conversion of dates to ints
        /// </summary>
        private static readonly DateTime Origin = new DateTime( 1970 , 1 , 1 );

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SexpArrayDate"/> class.
        /// </summary>
        public SexpArrayDate()
        {
            Attributes[ "class" ] = new SexpString( "Date" );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SexpArrayDate"/> class.
        /// </summary>
        /// <param name="theValue">
        /// The value.
        /// </param>
        public SexpArrayDate( IEnumerable<DateTime> theValue )
            : base( theValue.Select( x => x.Date ).Select( y => y.Subtract( Origin ).Days ) )
        {
            Attributes[ "class" ] = new SexpString( "Date" );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SexpArrayDate"/> class.
        /// </summary>
        /// <param name="theValue">
        /// The value.
        /// </param>
        public SexpArrayDate( IEnumerable<int> theValue )
            : base( theValue )
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
                if ( Value.Count == 1 )
                {
                    return Value[ 0 ];
                }

                throw new IndexOutOfRangeException( "Can only convert numeric arrays of length 1 to double." );
            }
        }

        /// <summary>
        /// Gets the integers stored in the list
        /// </summary>
        internal new List<DateTime> Value
        {
            get
            {
                return ( base.Value.Select( x => Origin.AddDays( x ) ).ToList() );
            }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element.</param>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        public new Sexp this[ int index ]
        {
            get
            {
                return new SexpDate( Value[ index ] );
            }
            set
            {
                base[ index ] = value.IsNa ? new SexpDate( SexpInt.NaValue ) : new SexpDate( value.AsDate );
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
            base.Add( item.IsNa ? item : new SexpDate( item.AsDate ) );
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
            return base.Contains( item.IsNa ? item : new SexpDate( item.AsDate ) );
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
                array[ arrayIndex + i ] = new SexpDate( Value[ i ] );
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
            return ( from a in Value select ( Sexp )( new SexpDate( a ) ) ).GetEnumerator();
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
            return base.IndexOf( item.IsNa ? item : new SexpDate( item.AsDate ) );
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
            base.Insert( index , item.IsNa ? item : new SexpDate( item.AsDate ) );
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
            return base.Remove( item.IsNa ? item : new SexpDate( item.AsDate ) );
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