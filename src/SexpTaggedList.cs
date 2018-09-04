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
    /// A tagged list.
    /// </summary>
    [Serializable]
    public class SexpTaggedList : Sexp , IEnumerable<KeyValuePair<string , Sexp>>
    {
        # region Constructors

        /// <summary>
        /// Initializes a new instance of SexpTaggedList.
        /// </summary>
        public SexpTaggedList()
        {
            Value = new List<KeyValuePair<string , Sexp>>();
        }

        /// <summary>
        /// Initializes a new instance of SexpTaggedList with an IEnumerable of KeyValuePair of string,Sexp
        /// </summary>
        public SexpTaggedList( IEnumerable<KeyValuePair<string , Sexp>> theValue )
        {
            Value = new List<KeyValuePair<string , Sexp>>();
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
        /// Gets an ICollection containing the keys of the IDictionary.
        /// </summary>
        /// <returns>
        /// An ICollection containing the keys of the object that implements IDictionary.
        /// </returns>
        public override ICollection<string> Keys
        {
            get
            {
                return ( from a in Value select a.Key ).ToArray();
            }
        }

        /// <summary>
        /// Gets an ICollection containing the values in the IDictionary.
        /// </summary>
        /// <returns>
        /// An ICollection containing the values in the object that implements IDictionary.
        /// </returns>
        public override ICollection<Sexp> Values
        {
            get
            {
                return ( from a in Value select a.Value ).ToArray();
            }
        }

        /// <summary>
        /// Gets the values stored in the tagged list
        /// </summary>
        internal List<KeyValuePair<string , Sexp>> Value { get; private set; }

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
                return Value[ index ].Value;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets or sets the element with a specific name
        /// </summary>
        /// <param name="key">The name of the element to be retrieved</param>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        public override Sexp this[ string key ]
        {
            get
            {
                int ndx = IndexOfKey( key );
                if ( ndx < 0 )
                {
                    throw new KeyNotFoundException();
                }

                return Value[ ndx ].Value;
            }

            set
            {
                int ndx = IndexOfKey( key );
                if ( ndx < 0 )
                {
                    Value.Add( new KeyValuePair<string , Sexp>( key , value ) );
                }
                else
                {
                    Value[ ndx ] = new KeyValuePair<string , Sexp>( key , value );
                }
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
            throw new NotSupportedException();
        }

        /// <summary>
        /// Adds an element with the provided key and value to the IDictionary.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        public override void Add( string key , Sexp value )
        {
            Value.Add( new KeyValuePair<string , Sexp>( key , value ) );
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
            return Value.Exists( x => x.Value == item );
        }

        /// <summary>
        /// Determines whether the ICollection contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the ICollection.</param>
        /// <returns>
        /// true if item is found in the ICollection; otherwise, false.
        /// </returns>
        public override bool Contains( KeyValuePair<string , Sexp> item )
        {
            return Value.Contains( item );
        }

        /// <summary>
        /// Determines whether the IDictionary contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the IDictionary.</param>
        /// <returns>
        /// true if the IDictionary contains an element with the key; otherwise, false.
        /// </returns>
        public override bool ContainsKey( string key )
        {
            return Value.Exists( x => x.Key == key );
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
                array[ arrayIndex + i ] = Value[ i ].Value;
            }
        }

        /// <summary>
        /// Copies the elements of the ICollection to an Array, starting at a particular Array index.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from ICollection. The Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins</param>
        public override void CopyTo( KeyValuePair<string , Sexp>[] array , int arrayIndex )
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
            return ( from a in Value select a.Value ).GetEnumerator();
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
            return Value.FindIndex( x => ( x.Value == item ) );
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
            throw new NotSupportedException();
        }

        /// <summary>
        /// Removes the element with the specified key from the IDictionary.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key"/> was not found in the original IDictionary.
        /// </returns>
        public override bool Remove( string key )
        {
            return Value.RemoveAll( x => x.Key == key ) > 0;
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the ICollection.
        /// </summary>
        /// <param name="item">The object to remove from the ICollection.</param>
        /// <returns>
        /// true if item was successfully removed from the ICollection; otherwise, false. This method also returns false if item is not found in the original ICollection.
        /// </returns>
        public override bool Remove( KeyValuePair<string , Sexp> item )
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

        /// <summary>
        /// Returns a string that represents this instance.
        /// </summary>
        /// <returns>
        /// A string that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var r = new StringBuilder();
            r.Append( "{ " );
            foreach ( var a in Value )
            {
                r.Append( a.Key );
                r.Append( ": " );
                r.Append( a.Value.ToString() );
                r.Append( ", " );
            }

            r.Remove( r.Length - 2 , 2 );
            r.Append( " }" );
            return r.ToString();
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.</param>
        /// <returns>
        /// true if the object that implements IDictionary contains an element with the specified key; otherwise, false.
        /// </returns>
        public override bool TryGetValue( string key , out Sexp value )
        {
            var ndx = IndexOfKey( key );
            if ( ndx < 0 )
            {
                value = null;
                return false;
            }
            value = Value[ ndx ].Value;
            return true;
        }

        private int IndexOfKey(string key)
        {
            return Value.FindIndex( e => e.Key == key );
        }

        #endregion

        #region Implemented Interfaces

        #region IEnumerable<KeyValuePair<string,Sexp>>

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A IEnumerator that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<KeyValuePair<string , Sexp>> IEnumerable<KeyValuePair<string , Sexp>>.GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        #endregion

        #endregion
    }
}
