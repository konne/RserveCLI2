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
    /// An array of (trivalue, i.e., including NA) booleans.
    /// </summary>
    /// <remarks>
    /// NA is represented as NULL
    /// </remarks>
    [Serializable]
    public class SexpArrayBool : SexpGenericList
    {

        #region Constants and Fields

        /// <summary>
        /// The representation of NA
        /// </summary>
        // ReSharper disable RedundantDefaultFieldInitializer
        private static readonly bool? NaValue = null;
        // ReSharper restore RedundantDefaultFieldInitializer

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of SexpArrayBool.
        /// </summary>
        public SexpArrayBool()
        {
            Value = new List<bool?>();
        }

        /// <summary>
        /// Initializes a new instance of SexpArrayBool with a nullable bool.
        /// </summary>
        public SexpArrayBool( bool? theValue )
        {
            Value = new List<bool?> { theValue };
        }

        /// <summary>
        /// Initializes a new instance of SexpArrayBool with an IEnumerable of nullable bool.
        /// </summary>
        public SexpArrayBool( IEnumerable<bool?> theValue )
        {
            Value = new List<bool?>();
            Value.AddRange( theValue );
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets as a nullable bool.
        /// </summary>
        public override bool? AsBool
        {
            get
            {
                if ( Value.Count == 1 )
                {
                    return Value[ 0 ];
                }
                throw new IndexOutOfRangeException( "Can only convert bool arrays of length 1 to bool." );
            }
        }

        /// <summary>
        /// Gets as an array of nullable bool.
        /// </summary>
        public override bool?[] AsBools
        {
            get
            {
                return Value.ToArray();
            }
        }

        /// <summary>
        /// Gets the bool value as byte that R can interpret.
        /// </summary>
        public byte AsByte
        {
            get
            {
                if ( Value.Count == 1 )
                {
                    if ( Value[ 0 ] == null )
                    {
                        return 2;
                    }
                    if ( Value[ 0 ] == true )
                    {
                        return 1;
                    }
                    return 0;
                }
                throw new NotSupportedException( "Can only convert bool arrays of length 1 to byte." );
            }
        }

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
        /// Determines if the value of this bool is NA
        /// </summary>
        public override bool IsNa
        {
            get
            {
                if ( Value.Count == 1 )
                {
                    return Value[ 0 ] == null;
                }
                throw new NotSupportedException( "Can only check NA for length 1 bool" );
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
        /// Gets the representation of NA
        /// </summary>
        public static bool? Na
        {
            get
            {
                return NaValue;
            }
        }

        /// <summary>
        /// Gets the values stored in the list
        /// </summary>
        internal List<bool?> Value { get; private set; }

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
                return new SexpArrayBool( Value[ index ] );
            }

            set
            {
                Value[ index ] = value.AsBool;
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
            Value.AddRange( item.AsBools );
        }

        /// <summary>
        /// Checks whether a value is NA.
        /// </summary>
        /// <param name="x">The value to be checked.</param>
        /// <returns>
        /// True if the value is NA, false otherwise.
        /// </returns>
        public static bool CheckNa( bool? x )
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
        /// Copies the elements of the ICollection to an Array, starting at a particular Array index.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from ICollection. The Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        public override void CopyTo( Sexp[] array , int arrayIndex )
        {
            for ( int i = 0 ; i < Value.Count ; i++ )
            {
                array[ arrayIndex + i ] = new SexpArrayBool( Value[ i ] );
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
            var objSexpArrayBool = obj as SexpArrayBool;
            if ( objSexpArrayBool != null )
            {
                return Equals( objSexpArrayBool );
            }

            // can obj be coersed into an array of nullable bool?
            try
            {
                return Equals( new SexpArrayBool( Make( obj ).AsBools ) );
            }
            catch ( NotSupportedException ) { }
            return false;
        }

        /// <summary>
        /// Determines whether the specified SexpArrayBool is equal to this instance.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>
        /// <c>true</c> if the specified SexpArrayBool is equal to this instance; otherwise, <c>false</c>.
        /// Does not check for attribute equality.
        /// </returns>
        public bool Equals( SexpArrayBool other )
        {
            if ( ReferenceEquals( null , other ) ) return false;
            if ( ReferenceEquals( this , other ) ) return true;
            return other.Value.SequenceEqual( Value );
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A IEnumerator that can be used to iterate through the collection.
        /// </returns>
        public override IEnumerator<Sexp> GetEnumerator()
        {
            return ( from a in Value select ( Sexp )( new SexpArrayBool( a ) ) ).GetEnumerator();
        }

        /// <summary>
        /// Gets hash code.
        /// </summary>
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
            return Value.IndexOf( item.AsBool );
        }

        /// <summary>
        /// Inserts an item to the IList at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index at which item should be inserted.
        /// </param>
        /// <param name="item">The object to insert into the IList.</param>
        public override void Insert( int index , Sexp item )
        {
            Value.Insert( index , item.AsBool );
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
            foreach ( bool? value in Value )
            {
                builder.Append( " " );
                builder.Append( CheckNa( value ) ? "NA" : value.ToString() );
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
