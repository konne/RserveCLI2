//-----------------------------------------------------------------------
// Original work Copyright (c) 2011, Oliver M. Haynold
// Modified work Copyright (c) 2013, Suraj Gupta
// All rights reserved.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RserveCLI2
{

    /// <summary>
    /// An array of Dates.  Time portion of a date is severed.
    /// </summary>
    [Serializable]
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
        /// Initializes a new instance of SexpArrayDate.
        /// </summary>
        public SexpArrayDate()
        {
            Attributes[ "class" ] = new SexpArrayString( "Date" );
        }

        /// <summary>
        /// Initializes a new instance of SexpArrayDate with a DateTime. 
        /// </summary>
        public SexpArrayDate( DateTime theValue )
            : base( DateToRInt( theValue ) )
        {
            Attributes[ "class" ] = new SexpArrayString( "Date" );
        }

        /// <summary>
        /// Initializes a new instance of SexpArrayDate with IEnumerable of DateTime
        /// </summary>
        public SexpArrayDate( IEnumerable<DateTime> theValue )
            : base( theValue.Select( DateToRInt ) )
        {
            Attributes[ "class" ] = new SexpArrayString( "Date" );
        }

        /// <summary>
        /// Initializes a new instance of the SexpArrayDate with dates in R integer format.
        /// </summary>
        /// <remarks>
        /// Should only be called from Qap1.DecodeSexp.  The class attribute will be added after this class is constructed.
        /// If its constructed now, then there will be an exception inserting duplicate key into Dictionary
        /// </remarks>
        internal SexpArrayDate( IEnumerable<int> theValue )
            : base( theValue )
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets as DateTime.
        /// </summary>
        public override DateTime AsDate
        {
            get
            {
                if ( Value.Count == 1 )
                {
                    return Value[ 0 ];
                }
                throw new NotSupportedException( "Can only convert length 1 Date." );
            }
        }

        /// <summary>
        /// Gets as array of Date.
        /// </summary>
        public override DateTime[] AsDates
        {
            get
            {
                return Value.ToArray();
            }
        }

        /// <summary>
        /// Gets the dates stored in the list
        /// </summary>
        internal new List<DateTime> Value
        {
            get
            {
                return ( base.Value.Select( RIntToDate ).ToList() );
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
                return new SexpArrayDate( Value[ index ] );
            }
            set
            {
                base[ index ] = new SexpArrayInt( DateToRInt( value.AsDate ) );
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds an item to the ICollection
        /// </summary>
        /// <param name="item">The object to add to the ICollection.</param>
        public override void Add( Sexp item )
        {
            Add( item.AsDates.Select( DateToRInt ) );
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
            return base.Contains( new SexpArrayInt( DateToRInt( item.AsDate ) ) );
        }

        /// <summary>
        /// Copies the elements of the ICollection to an Array, starting at a particular Array index.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from ICollection. The Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        public override void CopyTo( Sexp[] array , int arrayIndex )
        {
            for ( int i = 0 ; i < Value.Count ; i++ )
            {
                array[ arrayIndex + i ] = new SexpArrayDate( Value[ i ] );
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A IEnumerator that can be used to iterate through the collection.
        /// </returns>
        public override IEnumerator<Sexp> GetEnumerator()
        {
            return ( from a in Value select ( Sexp )( new SexpArrayDate( a ) ) ).GetEnumerator();
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
            return IndexOf( DateToRInt( item.AsDate ) );
        }

        /// <summary>
        /// Inserts an item to the IList at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert into the IList.</param>
        public override void Insert( int index , Sexp item )
        {
            base.Insert( index , new SexpArrayInt( DateToRInt( item.AsDate ) ) );
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
            return base.Remove( new SexpArrayInt( DateToRInt( item.AsDate ) ) );
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

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>
        /// A string that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach ( int value in base.Value )
            {
                builder.Append( " " );
                builder.Append( CheckNa( value ) ? "NA" : RIntToDate( value ).ToString("d") );
            }
            if ( builder.Length > 0 )
            {
                builder.Remove( 0 , 1 );
            }
            return builder.ToString();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Converts a DateTime into an R integer
        /// </summary>
        private static int DateToRInt( DateTime day )
        {
            return day.Subtract( Origin ).Days;
        }

        /// <summary>
        /// Converts a DateTime into an R integer
        /// </summary>
        private static DateTime RIntToDate( int rdate )
        {
            return Origin.AddDays( rdate );
        }

        #endregion

    }
}