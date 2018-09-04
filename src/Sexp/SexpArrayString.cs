//-----------------------------------------------------------------------
// Original work Copyright (c) 2011, Oliver M. Haynold
// Modified work Copyright (c) 2013, Suraj Gupta
// All rights reserved.
//-----------------------------------------------------------------------

using System.Text;

namespace RserveCLI2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An array of strings
    /// </summary>
    [Serializable]
    public class SexpArrayString : SexpGenericList
    {
        
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the SexpArryString.
        /// </summary>
        public SexpArrayString()
        {
            Value = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of SexpArryString with a string.
        /// </summary>
        public SexpArrayString( string theValue )
        {
            Value = new List<string> { theValue };
        }

        /// <summary>
        /// Initializes a new instance of SexpArrayString with an IEnumerable of string.
        /// </summary>
        public SexpArrayString( IEnumerable<string> theValue )
        {
            Value = new List<string>();
            Value.AddRange( theValue );
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
                if ( Value.Count == 1 )
                {
                    return Value[ 0 ];
                }
                throw new IndexOutOfRangeException( "Can only convert numeric arrays of length 1 to double." );
            }
        }

        /// <summary>
        /// Gets as array of string.
        /// </summary>
        public override string[] AsStrings
        {
            get
            {
                return Value.ToArray();
            }
        }
        
        /// <summary>
        /// Gets the number of elements contained in the ICollection.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the ICollection.
        /// </returns>
        public override int Count
        {
            get
            {
                return Value.Count;
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
                if ( Value.Count == 1 )
                {
                    return Value[ 0 ] == null;
                }
                throw new IndexOutOfRangeException( "Can only convert numeric arrays of length 1 to double." );
            }
        }

        /// <summary>
        /// Gets a value indicating whether the ICollection is read-only.
        /// </summary>
        /// <returns>
        /// true if the ICollection is read-only; otherwise, false.
        /// </returns>
        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the NA representation of strings.
        /// </summary>
        public static string Na
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the strings stored in the list
        /// </summary>
        internal List<string> Value { get; private set; }

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
                return new SexpArrayString( Value[ index ] );
            }

            set
            {
                Value[ index ] = value.AsString;
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
            Value.AddRange( item.AsStrings );
        }

        /// <summary>
        /// Checks whether a value is NA.
        /// </summary>
        /// <param name="x">The value to be checked.</param>
        /// <returns>
        /// True if the value is NA, false otherwise.
        /// </returns>
        public static bool CheckNa( string x )
        {
            return x == null;
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
            return Value.Contains( item.AsString );
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
                array[ arrayIndex + i ] = new SexpArrayString( Value[ i ] );
            }
        }

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns>
        /// true if the specified object is equal to this instance; otherwise, false.
        /// Does not check for attribute equality.
        /// </returns>
        public override bool Equals( object obj )
        {
            if ( obj == null )
            {
                return false;
            }
            var objSexpArrayString = obj as SexpArrayString;
            if ( objSexpArrayString != null )
            {
                return Equals( objSexpArrayString );
            }

            // can obj be coersed into an array of int?
            try
            {
                return Equals( new SexpArrayString( Make( obj ).AsStrings ) );
            }
            catch ( NotSupportedException ) { }
            return false;
        }

        /// <summary>
        /// Determines whether the specified SexpArrayInt is equal to this instance.
        /// </summary>
        /// <returns>
        /// true if the specified SexpArrayString is equal to this instance; otherwise, false.
        /// Does not check for attribute equality.
        /// </returns>
        public bool Equals( SexpArrayString other )
        {
            if ( ReferenceEquals( null , other ) ) return false;
            if ( ReferenceEquals( this , other ) ) return true;
            return other.Value.SequenceEqual( Value );
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An IEnumerator that can be used to iterate through the collection.
        /// </returns>
        public override IEnumerator<Sexp> GetEnumerator()
        {
            return ( from a in Value select ( Sexp )( new SexpArrayString( a ) ) ).GetEnumerator();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ( base.GetHashCode() * 397 ) ^ ( Value != null ? Value.GetHashCode() : 0 );
            }
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
            return Value.IndexOf( item.AsString );
        }

        /// <summary>
        /// Inserts an item to the IList at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert into the IList.</param>
        public override void Insert( int index , Sexp item )
        {
            Value.Insert( index , item.AsString );
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
            return Value.Remove( item.AsString );
        }

        /// <summary>
        /// Removes the IList item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public override void RemoveAt( int index )
        {
            Value.RemoveAt( index );
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
            foreach ( string value in Value )
            {
                builder.Append( " " );
                builder.Append( CheckNa( value ) ? "NA" : value );
            }
            if ( builder.Length > 0 )
            {
                builder.Remove( 0 , 1 );
            }
            return builder.ToString();
        }

        #endregion

    }
}