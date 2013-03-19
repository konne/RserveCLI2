// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SexpArrayInt.cs" company="Oliver M. Haynold">
//   Copyright (c) 2011, Oliver M. Haynold
// All rights reserved.
// </copyright>
// <summary>
// An array of integers.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RserveCli
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// An array of integers.
    /// </summary>
    public class SexpArrayInt : SexpGenericList
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SexpArrayInt"/> class.
        /// </summary>
        public SexpArrayInt()
        {
            this.Value = new List<int>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SexpArrayInt"/> class.
        /// </summary>
        /// <param name="theValue">
        /// The value.
        /// </param>
        public SexpArrayInt(IEnumerable<int> theValue)
        {
            this.Value = new List<int>();
            this.Value.AddRange(theValue);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets as int.
        /// </summary>
        /// <value>
        /// The value as an integer.
        /// </value>
        public override int AsInt
        {
            get
            {
                if (this.Value.Count == 1)
                {
                    return this.Value[0];
                }

                throw new IndexOutOfRangeException("Can only convert numeric arrays of length 1 to double.");
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
                return this.Value.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is NA.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is NA; otherwise, <c>false</c>.
        /// </value>
        public override bool IsNa
        {
            get
            {
                if (this.Value.Count == 1)
                {
                    return SexpInt.CheckNa(this.Value[0]);
                }

                throw new IndexOutOfRangeException("Can only convert numeric arrays of length 1 to double.");
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
        /// Gets the integers stored in the list
        /// </summary>
        internal List<int> Value { get; private set; }
        #endregion

        #region Indexers

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element.</param>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        public override Sexp this[int index]
        {
            get
            {
                return new SexpInt(this.Value[index]);
            }

            set
            {
                this.Value[index] = value.IsNa ? SexpInt.NaValue : value.AsInt;
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
        public override void Add(Sexp item)
        {
            this.Value.Add(item.IsNa ? SexpInt.NaValue : item.AsInt);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </exception>
        public override void Clear()
        {
            this.Value.Clear();
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
        public override bool Contains(Sexp item)
        {
            return this.Value.Contains(item.IsNa ? SexpInt.NaValue : item.AsInt);
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
        public override void CopyTo(Sexp[] array, int arrayIndex)
        {
            for (int i = 0; i < this.Value.Count; i++)
            {
                array[arrayIndex + i] = new SexpInt(this.Value[i]);
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
            return (from a in this.Value select (Sexp)(new SexpInt(a))).GetEnumerator();
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
        public override int IndexOf(Sexp item)
        {
            return this.Value.IndexOf(item.IsNa ? SexpInt.NaValue : item.AsInt);
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
        public override void Insert(int index, Sexp item)
        {
            this.Value.Insert(index, item.IsNa ? SexpInt.NaValue : item.AsInt);
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
        public override bool Remove(Sexp item)
        {
            return this.Value.Remove(item.IsNa ? SexpInt.NaValue : item.AsInt);
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the item to remove.
        /// </param>
        public override void RemoveAt(int index)
        {
            this.Value.RemoveAt(index);
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
            return this.Value.ToArray();
        }

        #endregion
    }
}
