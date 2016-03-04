namespace System
{
    using Collections;
    using Collections.Generic;
    using Diagnostics.CodeAnalysis;

    /// <summary>
    /// An alternate implementation of array.
    /// </summary>
    /// <typeparam name="TItem">Item type.</typeparam>
    [Serializable]
    public abstract class Array<TItem> : ICloneable, IStructuralComparable, IStructuralEquatable, IList<TItem>, IList
    {
        /// <summary>
        /// Gets value at <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> is not a valid index.</exception>
        protected abstract TItem GetValue( int index );

        /// <summary>
        /// Sets item at <paramref name="index"/> to <paramref name="value"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <param name="value">The new value.</param>
        /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> is not a valid index.</exception>
        protected abstract void SetValue( int index, TItem value );

        /// <summary>
        /// Get the length of the array.
        /// </summary>
        public abstract int Length { get; }

        /// <summary>
        /// Get the length of the array.
        /// </summary>
        public long LongLength => (long)Length;

        /// <summary>
        /// Get the dimensions of the array.
        /// </summary>
        /// <remarks>
        /// Always 1!
        /// </remarks>
        public int Rank => 1;

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone() => MemberwiseClone();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        int ICollection.Count => Length;

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
        /// <filterpriority>2</filterpriority>
        public bool IsSynchronized => false;

        void ICollection.CopyTo( Array array, int index )
        {
            throw new NotImplementedException();
        }

        object IList.this[ int index ]
        {
            get { return GetValue( index ); }
            set { SetValue( index, (TItem)value ); }
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

        int IList.Add( object value )
        {
            throw new NotSupportedException();
        }

        bool IList.Contains( object value )
        {
            throw new NotImplementedException();
        }

        void IList.Clear()
        {
            throw new NotImplementedException();
        }

        int IList.IndexOf( object value )
        {
            for( var i = 0; i < Length; i++ )
            {
                object item = GetValue( i );
                if( Equals( value, item ) )
                {
                    return i;
                }
            }
            return -1;
        }

        void IList.Insert( int index, object value )
        {
            throw new NotSupportedException();
        }

        void IList.Remove( object value )
        {
            throw new NotSupportedException();
        }

        void IList.RemoveAt( int index )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<TItem> GetEnumerator()
        {
            for( var i = 0; i < Length; ++i )
            {
                yield return GetValue( i );
            }
        }

        bool ICollection<TItem>.Contains( TItem item )
        {
            throw new NotImplementedException();
        }

        void ICollection<TItem>.CopyTo( TItem[] array, int arrayIndex )
        {
            throw new NotImplementedException();
        }

        bool ICollection<TItem>.Remove( TItem item )
        {
            throw new NotSupportedException();
        }

        int ICollection<TItem>.Count => Length;

        void ICollection<TItem>.Add( TItem item )
        {
            throw new NotSupportedException();
        }

        void ICollection<TItem>.Clear()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> is not a valid index.</exception>
        public TItem this[ int index ]
        {
            get { return GetValue( index ); }
            set { SetValue( index, value ); }
        }

        int IList<TItem>.IndexOf( TItem item )
        {
            for( var i = 0; i < Length; i++ )
            {
                var value = GetValue( i );
                if( Equals( value, item ) )
                {
                    return i;
                }
            }
            return -1;
        }

        void IList<TItem>.Insert( int index, TItem item )
        {
            throw new NotSupportedException();
        }

        void IList<TItem>.RemoveAt( int index )
        {
            throw new NotSupportedException();
        }

        [SuppressMessage( "ReSharper", "SuggestVarOrType_SimpleTypes" )]
        int IStructuralComparable.CompareTo( object other, IComparer comparer )
        {
            if( ( other == null ) || ( comparer == null ) )
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

        bool IStructuralEquatable.Equals( object other, IEqualityComparer comparer )
        {
            if( ( other == null ) || ( comparer == null ) )
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

        static int CombineHashCodes( int h1, int h2 ) => ( ( h1 << 5 ) + h1 ) ^ h2;

        int IStructuralEquatable.GetHashCode( IEqualityComparer comparer )
        {
            if( comparer == null )
                throw new ArgumentNullException( nameof( comparer ) );

            var ret = 0;
            for( var i = Length >= 8 ? Length - 8 : 0; i < Length; i++ )
            {
                ret = CombineHashCodes( ret, comparer.GetHashCode( GetValue( i ) ) );
            }
            return ret;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString() => $"{typeof( TItem ).FullName}[]";

        /// <summary>
        /// Create new array instance.
        /// </summary>
        /// <param name="length"></param>
        /// <returns>A new array instance.</returns>
        public static Array<TItem> CreateInstance( int length )
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
        /// Create new array instance.
        /// </summary>
        /// <returns>A new array instance.</returns>
        public static Array<TItem> CreateInstance() => new Array0<TItem>();

        /// <summary>
        /// Create new array instance.
        /// </summary>
        /// <returns>A new array instance.</returns>
        public static Array<TItem> CreateInstance( TItem arg0 ) => new Array1<TItem>
        {
            [ 0 ] = arg0
        };

        /// <summary>
        /// Create new array instance.
        /// </summary>
        /// <returns>A new array instance.</returns>
        public static Array<TItem> CreateInstance( TItem arg0, TItem arg1 ) => new Array2<TItem>
        {
            [ 0 ] = arg0,
            [ 1 ] = arg1
        };

        /// <summary>
        /// Create new array instance.
        /// </summary>
        /// <returns>A new array instance.</returns>
        public static Array<TItem> CreateInstance( TItem arg0, TItem arg1, TItem arg2 ) => new Array3<TItem>
        {
            [ 0 ] = arg0,
            [ 1 ] = arg1,
            [ 2 ] = arg2
        };
    }

    class Array0<TItem> : Array<TItem>
    {
        public override int Length => 0;

        protected override TItem GetValue( int index )
        {
            if( ( index < 0 ) || ( index >= Length ) ) throw new IndexOutOfRangeException();
            return default( TItem );
        }

        protected override void SetValue( int index, TItem value )
        {
            if( ( index < 0 ) || ( index >= Length ) ) throw new IndexOutOfRangeException();
        }
    }

    class Array1<TItem> : Array0<TItem>
    {
        protected TItem Pos0;

        public override int Length => 1;

        protected override TItem GetValue( int index ) =>
            index == 0
                ? Pos0
                : base.GetValue( index );

        protected override void SetValue( int index, TItem value )
        {
            if( index == 0 )
            {
                Pos0 = value;
            }
            else
            {
                base.SetValue( index, value );
            }
        }
    }

    class Array2<TItem> : Array1<TItem>
    {
        protected TItem Pos1;

        public override int Length => 2;

        protected override TItem GetValue( int index ) =>
            index == 0
                ? Pos1
                : base.GetValue( index );

        protected override void SetValue( int index, TItem value )
        {
            if( index == 1 )
            {
                Pos1 = value;
            }
            else
            {
                base.SetValue( index, value );
            }
        }
    }

    class Array3<TItem> : Array2<TItem>
    {
        protected TItem Pos2;

        public override int Length => 3;

        protected override TItem GetValue( int index ) =>
            index == 0
                ? Pos1
                : base.GetValue( index );

        protected override void SetValue( int index, TItem value )
        {
            if( index == 1 )
            {
                Pos1 = value;
            }
            else
            {
                base.SetValue( index, value );
            }
        }
    }
}
