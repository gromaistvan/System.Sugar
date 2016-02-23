namespace System
{
    using Collections;
    using Collections.Generic;
    using Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;
    using Linq;
    using Security;

    /// <summary>
    /// An alternate implementation of array.
    /// </summary>
    /// <typeparam name="TItem">Item type.</typeparam>
    [SuppressMessage( "Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "OK" )]
    [Serializable]
    public abstract class Array<TItem> : ICloneable, IStructuralComparable, IStructuralEquatable, IList<TItem>, IList
    {
        /// <summary>
        /// Gets the length of array.
        /// </summary>
        /// <returns>The length.</returns>
        [Pure]
        protected abstract int GetLength();

        /// <summary>
        /// Copy array.
        /// </summary>
        /// <param name="array">Destinsation array.</param>
        /// <param name="index">Start index in destination array.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
        protected virtual void Copy( Array array, int index )
        {
            if( array == null ) throw new ArgumentNullException( nameof( array ) );
            
            for( var i = GetLowerBound( 0 ); i < GetUpperBound( 0 ); i++ )
            {
                array.SetValue( GetValue( i ), index++ );
            }
        }

        /// <summary>
        /// Clear all items.
        /// </summary>
        protected virtual void Reset()
        {
            for( var i = GetLowerBound( 0 ); i < GetUpperBound( 0 ); i++ )
            {
                SetValue( default( TItem ), i );
            }
        }

        /// <summary>
        /// Tests array for contaning a given item.
        /// </summary>
        /// <param name="item">Item to find.</param>
        /// <returns>Item found or not.</returns>
        [Pure]
        protected virtual bool ContainsItem( object item ) => this.Any( _ => Equals( _, item ) );

        /// <summary>
        /// Gets the first occurrence of a given item.
        /// </summary>
        /// <param name="item">Item to find.</param>
        /// <returns>The index of first occurrence, -1 otherwise.</returns>
        [Pure]
        protected virtual int FirstIndexOf( object item )
        {
            for( var i = GetLowerBound( 0 ); i <= GetUpperBound( 0 ); i++ )
            {
                var value = GetValue( i );
                if( Equals( value, item ) )
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Gets the value at the specified position in the one-dimensional array.
        /// The index is specified as a 32-bit integer.
        /// </summary>
        /// <param name="index">A 32-bit integer that represents the position of the array element to get.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index.</exception>
        [Pure]
        public abstract TItem GetValue( int index );

        /// <summary>
        /// Sets a value to the element at the specified position in the one-dimensional array.
        /// The index is specified as a 32-bit integer.
        /// </summary>
        /// <param name="value">The new value for the specified element.</param>
        /// <param name="index">A 32-bit integer that represents the position of the array element to set.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index.</exception>
        public abstract void SetValue( TItem value, int index );

        /// <summary>
        /// Gets the value at the specified position in the one-dimensional array.
        /// The index is specified as a 64-bit integer.
        /// </summary>
        /// <param name="index">A 64-bit integer that represents the position of the array element to get.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index.</exception>
        [Pure]
        public TItem GetValue( long index ) => GetValue( (int)index );

        /// <summary>
        /// Sets a value to the element at the specified position in the one-dimensional array.
        /// The index is specified as a 64-bit integer.
        /// </summary>
        /// <param name="value">The new value for the specified element.</param>
        /// <param name="index">A 64-bit integer that represents the position of the array element to set.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index.</exception>
        public void SetValue( TItem value, long index ) => SetValue( value, (int)index );

        /// <summary>
        /// Gets the index of the last element of the specified dimension in the array.
        /// </summary>
        /// <param name="dimension">A zero-based dimension of the array whose upper bound needs to be determined.</param>
        /// <returns>The index of the last element of the specified dimension in the array, or -1 if the specified dimension is empty.</returns>
        [Pure]
        public int GetUpperBound( int dimension ) => dimension == 0 ? GetLength() - 1 : -1;

        /// <summary>
        /// Gets the index of the first element of the specified dimension in the array.
        /// </summary>
        /// <param name="dimension">A zero-based dimension of the array whose upper bound needs to be determined.</param>
        /// <returns>The index of the last element of the specified dimension in the array, or -1 if the specified dimension is empty.</returns>
        [Pure]
        public int GetLowerBound( int dimension ) => dimension == 0 ? 0 : -1;

        /// <summary>
        /// Gets a 32-bit integer that represents the number of elements in the specified dimension of the array.
        /// </summary>
        /// <param name="dimension">A zero-based dimension of the array whose length needs to be determined.</param>
        /// <returns>A 32-bit integer that represents the number of elements in the specified dimension.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="dimension"/> is less than zero or
        /// <paramref name="dimension"/> is equal to or greater than <see cref="Rank"/>
        /// </exception>
        [SuppressMessage( "Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes", Justification = "OK" )]
        [SuppressMessage( "ReSharper", "UnthrowableException", Justification = "OK" )]
        [Pure]
        public int GetLength( int dimension )
        {
            if(  dimension != 0 ) throw new IndexOutOfRangeException();

            return GetLength();
        }

        /// <summary>
        /// Gets a 64-bit integer that represents the number of elements in the specified dimension of the array.
        /// </summary>
        /// <param name="dimension">A zero-based dimension of the array whose length needs to be determined.</param>
        /// <returns>A 64-bit integer that represents the number of elements in the specified dimension.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="dimension"/> is less than zero or
        /// <paramref name="dimension"/> is equal to or greater than <see cref="Rank"/>
        /// </exception>
        [SuppressMessage( "Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes", Justification = "OK" )]
        [SuppressMessage( "Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "long", Justification = "OK" )]
        [SuppressMessage( "ReSharper", "UnthrowableException", Justification = "OK" )]
        [Pure]
        public long GetLongLength( int dimension )
        {
            if(  dimension != 0 ) throw new IndexOutOfRangeException();

            return GetLength();
        }

        /// <summary>
        /// Gets the rank (number of dimensions) of the array.
        /// For example, a one-dimensional array returns 1, a two-dimensional array returns 2, and so on.
        /// </summary>
        public int Rank => 1;

        /// <summary>
        /// Gets the total number of elements in all the dimensions of the array.
        /// </summary>
        [SuppressMessage( "Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "OK"  )]
        public int Length => GetLength( Rank - 1 );

        /// <summary>
        /// Gets a 64-bit integer that represents the total number of elements in all the dimensions of the array.
        /// </summary>
        [SuppressMessage( "Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "OK"  )]
        [SuppressMessage( "Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "long", Justification = "OK"  )]
        public long LongLength => GetLongLength( Rank - 1 );

        #region Object

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        [Pure]
        public override string ToString() => FormattableString.Invariant( $"{typeof( TItem ).FullName}[]" ) ?? "";

        #endregion

        #region IEnumerable

        [Pure]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region ICollection

        int ICollection.Count => GetLength();

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="ICollection"/>.
        /// </summary>
        /// <returns>
        /// An object that can be used to synchronize access to the <see cref="ICollection"/>.
        /// </returns>
        public object SyncRoot => this;

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="ICollection"/> is synchronized (thread safe).
        /// </summary>
        /// <returns>
        /// true if access to the <see cref="ICollection"/> is synchronized (thread safe); otherwise, false.
        /// </returns>
        public bool IsSynchronized => false;

        void ICollection.CopyTo( Array array, int index ) => Copy( array, index );

        #endregion

        #region IList

        object IList.this[ int index ]
        {
            [Pure]
            get { return GetValue( index ); }
            set { SetValue( (TItem)value, index ); }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="IList"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="IList"/> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly => false;

        /// <summary>
        /// Gets a value indicating whether the <see cref="IList"/> has a fixed size.
        /// </summary>
        /// <returns>
        /// true if the <see cref="IList"/> has a fixed size; otherwise, false.
        /// </returns>
        public bool IsFixedSize => true;

        [Pure]
        bool IList.Contains( object value ) => ContainsItem( value );

        void IList.Clear() => Reset();

        [Pure]
        int IList.IndexOf( object value ) => FirstIndexOf( value );
        
        void IList.Insert( int index, object value )
        {
            throw new NotSupportedException();
        }

        int IList.Add( object value )
        {
            throw new NotSupportedException();
        }

        void IList.Remove( object value )
        {
            throw new NotSupportedException();
        }

        void IList.RemoveAt( int index )
        {
            throw new NotSupportedException();
        }

        #endregion

        #region IEnumerator<TItem>

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        [Pure]
        public IEnumerator<TItem> GetEnumerator()
        {
            for( var i = 0; i < Length; ++i )
            {
                yield return GetValue( i );
            }
        }

        #endregion

        #region ICollection<TItem>

        int ICollection<TItem>.Count => GetLength();

        [Pure]
        bool ICollection<TItem>.Contains( TItem item ) => ContainsItem( item );

        void ICollection<TItem>.Clear() => Reset();

        void ICollection<TItem>.CopyTo( TItem[] array /* TODO Violates rule! */, int arrayIndex ) => Copy( array, arrayIndex );

        bool ICollection<TItem>.Remove( TItem item )
        {
            throw new NotSupportedException();
        }

        void ICollection<TItem>.Add( TItem item )
        {
            throw new NotSupportedException();
        }

        #endregion

        #region IList<TItem>

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> is not a valid index.</exception>
        [SuppressMessage( "Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes", Justification = "OK"  )]
        [SuppressMessage( "Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Justification = "OK" )]
        [SuppressMessage( "ReSharper", "UnthrowableException", Justification = "OK" )]
        public TItem this[ int index ]
        {
            [Pure]
            get
            {
                if( ( index < 0 ) || ( index >= Length ) ) throw new IndexOutOfRangeException();

                return GetValue( index );
            }
            set
            {
                if( ( index < 0 ) || ( index >= Length ) ) throw new IndexOutOfRangeException();

                SetValue( value, index );
            }
        }

        [Pure]
        int IList<TItem>.IndexOf( TItem item ) => FirstIndexOf( item );

        void IList<TItem>.Insert( int index, TItem item )
        {
            throw new NotSupportedException();
        }

        void IList<TItem>.RemoveAt( int index )
        {
            throw new NotSupportedException();
        }

        #endregion

        #region ICloneable

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        [Pure]
        [SecuritySafeCritical]
        public object Clone() => MemberwiseClone();

        #endregion

        #region IStructuralEquatable

        [Pure]
        bool IStructuralEquatable.Equals( object other, IEqualityComparer comparer )
        {
            if( comparer == null ) throw new ArgumentNullException( nameof( comparer ) );

            if( other == null )
            {
                return false;
            }
            if( ReferenceEquals( this, other ) )
            {
                return true;
            }
            var array = other as Array<TItem>;
            if( ( array == null ) || ( array.Length != Length ) )
            {
                return false;
            }
            for( var i = 0; i < array.Length; i++ )
            {
                object left = GetValue( i );
                object right = array.GetValue( i );
                if( !comparer.Equals( left, right ) )
                {
                    return false;
                }
            }
            return true;
        }

        [Pure]
        int IStructuralEquatable.GetHashCode( IEqualityComparer comparer )
        {
            if( comparer == null ) throw new ArgumentNullException( nameof( comparer ) );

            var length = GetLength();
            var hash = 0;
            for( var i = length >= 8 ? length - 8 : 0; i < length; i++ )
            {
                var itemHash = comparer.GetHashCode( GetValue( i ) );
                hash = ( ( hash << 5 ) + hash ) ^ itemHash;
            }
            return hash;
        }

        #endregion

        #region IStructuralComparable 

        [SuppressMessage( "ReSharper", "SuggestVarOrType_SimpleTypes", Justification = "OK" )]
        [Pure]
        int IStructuralComparable.CompareTo( object other, IComparer comparer )
        {
            if( comparer == null ) throw new ArgumentNullException( nameof( comparer ) );

            if( other == null )
            {
                return 1;
            }
            var array = other as Array;
            if( ( array == null ) || ( Length != array.Length ) )
            {
                throw new ArgumentException( "Invalid type!", nameof( other ) );
            }
            var c = 0;
            for( var i = 0; ( i < array.Length ) && ( c == 0 ); ++i )
            {
                object left = GetValue( i );
                object right = array.GetValue( i );
                c = comparer.Compare( left, right );
            }
            return c;
        }

        #endregion
    }

    sealed class Array0<TItem> : Array<TItem>
    {
        protected override int GetLength() => 0;

        public override TItem GetValue( int index )
        {
            throw new ArgumentOutOfRangeException( nameof( index ) );
        }

        public override void SetValue( TItem value, int index )
        {
            throw new ArgumentOutOfRangeException( nameof( index ) );
        }
    }

    sealed class Array1<TItem> : Array<TItem>
    {
        TItem _0;

        protected override int GetLength() => 1;

        public override TItem GetValue( int index )
        {
            switch( index )
            {
                case 0:
                    return _0;
                default:
                    throw new ArgumentOutOfRangeException( nameof( index ) );
            }
        }

        public override void SetValue( TItem value, int index )
        {
            switch( index )
            {
                case 0:
                    _0 = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException( nameof( index ) );
            }
        }
    }

    sealed class Array2<TItem> : Array<TItem>
    {
        TItem _0, _1;

        protected override int GetLength() => 2;

        public override TItem GetValue( int index )
        {
            switch( index )
            {
                case 0:
                    return _0;
                case 1:
                    return _1;
                default:
                    throw new ArgumentOutOfRangeException( nameof( index ) );
            }
        }

        public override void SetValue( TItem value, int index )
        {
            switch( index )
            {
                case 0:
                    _0 = value;
                    break;
                case 1:
                    _1 = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException( nameof( index ) );
            }
        }
    }

    sealed class Array3<TItem> : Array<TItem>
    {
        TItem _0, _1, _2;

        protected override int GetLength() => 3;

        public override TItem GetValue( int index )
        {
            switch( index )
            {
                case 0:
                    return _0;
                case 1:
                    return _1;
                case 2:
                    return _2;
                default:
                    throw new ArgumentOutOfRangeException( nameof( index ) );
            }
        }

        public override void SetValue( TItem value, int index )
        {
            switch( index )
            {
                case 0:
                    _0 = value;
                    break;
                case 1:
                    _1 = value;
                    break;
                case 2:
                    _2 = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException( nameof( index ) );
            }
        }
    }

    /// <summary>
    /// Create new instances.
    /// </summary>
    public static class Arrays
    {
        /// <summary>
        /// Create new empty array instance.
        /// </summary>
        /// <returns>A new array instance.</returns>
        [NotNull]
        public static Array<TItem> CreateInstance<TItem>() => new Array0<TItem>();

        /// <summary>
        /// Create new array instance.
        /// </summary>
        /// <param name="length"></param>
        /// <returns>A new array instance.</returns>
        [NotNull]
        public static Array<TItem> CreateInstance<TItem>( int length )
        {
            switch( length )
            {
                case 0:
                    return new Array0<TItem>();
                case 1:
                    return new Array1<TItem>();
                case 2:
                    return new Array2<TItem>();
                case 3:
                    return new Array3<TItem>();
                default:
                    throw new OverflowException();
            }
        }

        /// <summary>
        /// Create and initialize new array instance.
        /// </summary>
        /// <returns>A new array instance.</returns>
        [NotNull]
        public static Array<TItem> InitializeInstance<TItem>( TItem arg0 ) => new Array1<TItem>
        {
            [ 0 ] = arg0
        };

        /// <summary>
        /// Create and initialize new array instance.
        /// </summary>
        /// <returns>A new array instance.</returns>
        [NotNull]
        public static Array<TItem> InitializeInstance<TItem>( TItem arg0, TItem arg1 ) => new Array2<TItem>
        {
            [ 0 ] = arg0,
            [ 1 ] = arg1
        };

        /// <summary>
        /// Create and initialize new array instance.
        /// </summary>
        /// <returns>A new array instance.</returns>
        [NotNull]
        public static Array<TItem> InitializeInstance<TItem>( TItem arg0, TItem arg1, TItem arg2 ) => new Array3<TItem>
        {
            [ 0 ] = arg0,
            [ 1 ] = arg1,
            [ 2 ] = arg2
        };
    }
}
