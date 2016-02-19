namespace System
{
    using Collections;
    using Collections.Generic;
    using Diagnostics;
    using Diagnostics.CodeAnalysis;
    using Globalization;
    using JetBrains.Annotations;
    using Security;
    using Text;
    using static FormattableString;

    /// <summary>
    /// Fancy class for using smart value manipulation.
    /// </summary>
    /// <remarks>
    /// Lots of syntactic sugar over strung usage.
    /// </remarks>
    /// <example>
    ///     <code>
    /// 
    /// 
    ///     </code>
    /// </example>
    [SuppressMessage( "Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "type", Target = "System.Superstring", Justification = "OK" )]
    [DebuggerDisplay( "{Internal.ToString()}" )]
    public class Superstring : IEquatable<Superstring>, IComparable<Superstring>, IComparable, ICloneable, IEnumerable<char>, IConvertible
    {
        #region Friendly static functions 

        /// <summary>
        /// General equivalence.
        /// </summary>
        /// <param name="left">A value.</param>
        /// <param name="right">An other value.</param>
        /// <returns>Equivalent?</returns>
        public static bool Equals( Superstring left, Superstring right ) =>
            ReferenceEquals( left, right )
            ||
            (
                ( left != null )
                &&
                ( right != null )
                &&
                left.Internal.Equals( right.Internal )
            );

        /// <summary>
        /// General alphabetic compare.
        /// </summary>
        /// <param name="left">A value.</param>
        /// <param name="right">An other value.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// Value Meaning Less than zero <paramref name="left"/> precedes <paramref name="right"/> in the sort order.
        /// Zero <paramref name="left"/> occurs in the same position in the sort order as <paramref name="right"/>.
        /// Greater than zero <paramref name="left"/> follows <paramref name="right"/> in the sort order. 
        /// </returns>
        public static int Compare( Superstring left, Superstring right ) => string.Compare(
            left?.ToString( CultureInfo.InvariantCulture ),
            right?.ToString( CultureInfo.InvariantCulture ),
            StringComparison.OrdinalIgnoreCase );

        /// <summary>
        /// Postfix concatenation.
        /// </summary>
        /// <param name="string">The value.</param>
        /// <param name="value">The value to append.</param>
        /// <returns>The <paramref name="string"/> instance.</returns>
        public static Superstring Add( Superstring @string, object value )
        {
            if( @string == null )
            {
                return null;
            }
            if( value != null )
            {
                @string.Internal.Append( value );
            }
            return @string;
        }

        /// <summary>
        /// Repate concatenation.
        /// </summary>
        /// <param name="string">The value.</param>
        /// <param name="count">Repeat count.</param>
        /// <returns>The <paramref name="string"/> instance.</returns>
        [SuppressMessage( "ReSharper", "ConvertIfStatementToSwitchStatement", Justification = "OK" )]
        public static Superstring Multiply( Superstring @string, int count )
        {
            if( count < 0 ) throw new ArgumentOutOfRangeException( nameof( count ), "count must be greater or equal than 0" );

            if( @string == null )
            {
                return null;
            }
            if( count == 0 )
            {
                @string.Internal.Clear();
                return @string;
            }
            if( count == 1 )
            {
                return @string;
            }
            var value = @string.Internal.ToString();
            for( var i = count - 1; i > 0; --i )
            {
                @string.Internal.Append( value );
            }
            return @string;
        }

        #endregion

        #region Operators

        /// <summary>
        /// Equivalence.
        /// </summary>
        /// <param name="left">A value.</param>
        /// <param name="right">An other value.</param>
        /// <returns>Equivalent?</returns>
        public static bool operator ==( Superstring left, Superstring right ) => Equals( left, right );

        /// <summary>
        /// Inequivalence.
        /// </summary>
        /// <param name="left">A value.</param>
        /// <param name="right">An other value.</param>
        /// <returns>Inequivalent?</returns>
        public static bool operator !=( Superstring left, Superstring right ) => ! Equals( left, right );

        /// <summary>
        /// Less than.
        /// </summary>
        /// <param name="left">A value.</param>
        /// <param name="right">An other value.</param>
        public static bool operator <( Superstring left, Superstring right ) => Compare( left, right ) < 0;

        /// <summary>
        /// Less than or equal.
        /// </summary>
        /// <param name="left">A value.</param>
        /// <param name="right">An other value.</param>
        public static bool operator <=( Superstring left, Superstring right ) => Compare( left, right ) <= 0;

        /// <summary>
        /// Greather than.
        /// </summary>
        /// <param name="left">A value.</param>
        /// <param name="right">An other value.</param>
        public static bool operator >( Superstring left, Superstring right ) => Compare( left, right ) > 0;

        /// <summary>
        /// Greather than or equal.
        /// </summary>
        /// <param name="left">A value.</param>
        /// <param name="right">An other value.</param>
        public static bool operator >=( Superstring left, Superstring right ) => Compare( left, right ) >= 0;

        /// <summary>
        /// Postfix concatenation.
        /// </summary>
        /// <param name="left">The value.</param>
        /// <param name="right">The value to append.</param>
        /// <returns>The <paramref name="left"/> instance.</returns>
        public static Superstring operator +( Superstring left, object right ) => Add( left, right );

        /// <summary>
        /// Repate concatenation.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="count">Repeat count.</param>
        /// <returns>The <paramref name="value"/> instance.</returns>
        public static Superstring operator *( Superstring value, int count ) => Multiply( value, count );

        public static bool operator true( Superstring value ) => value?.IsTrue ?? false;

        public static bool operator false( Superstring value ) => !( value?.IsTrue ?? false );

        /// <summary>
        /// Casting.
        /// </summary>
        public static implicit operator Superstring( string value ) => new Superstring( value );

        /// <summary>
        /// Casting.
        /// </summary>
        public static implicit operator string( Superstring value ) => value?.ToString( CultureInfo.InvariantCulture );

        /// <summary>
        /// Casting.
        /// </summary>
        public static implicit operator SecureString( Superstring value ) => value?.ToSecureString( CultureInfo.InvariantCulture );

        #endregion

        #region Fields

        /// <summary>
        /// For thread safety.
        /// </summary>
        [NotNull, DebuggerBrowsable( DebuggerBrowsableState.Never )]
        protected object SyncRoot { get; } = new object();

        /// <summary>
        /// Used for implementation logic.
        /// </summary>
        [NotNull, DebuggerBrowsable( DebuggerBrowsableState.Never )]
        protected StringBuilder Internal { get; } = new StringBuilder();

        #endregion

        #region Constructors

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Superstring()
        { }

        /// <summary>
        /// String constructor.
        /// </summary>
        /// <param name="value">Some text.</param>
        public Superstring( string value )
        {
            Internal = new StringBuilder( value ?? "" );
        }

        /// <summary>
        /// Repeated value constructor.
        /// </summary>
        /// <param name="value">Some text.</param>
        /// <param name="count">Repeat count.</param>
        public Superstring( string value, int count )
        {
            for( var i = 0; i < count - 1; i++ )
            {
                Internal.Append( value );
            }
        }

        #endregion

        #region Object overrides

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals( object obj ) => Equals( obj as Superstring );

        /// <summary>
        /// Serves as the default hash function. 
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode() => Internal.GetHashCode();

        /// <summary>
        /// Returns a value that represents the current object.
        /// </summary>
        /// <returns>
        /// A value that represents the current object.
        /// </returns>
        public override string ToString() => Internal.ToString();

        #endregion

        #region Properties and methods

        /// <summary>
        /// The length of the value.
        /// </summary>
        public int Length => Internal.Length;

        /// <summary>
        /// Empty strings are <c>false</c>.
        /// </summary>
        /// <returns>Not empty!</returns>
        public bool IsTrue => ! string.IsNullOrEmpty( Internal.ToString() );

        /// <summary>
        /// Convert to <see cref="SecureString"/> for using as password.
        /// </summary>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>Secure memory representation of value.</returns>
        [SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "provider", Justification = "OK" )]
        public SecureString ToSecureString( [UsedImplicitly] IFormatProvider provider )
        {
            var secure = new SecureString();
            try
            {
                foreach( var c in this )
                {
                    secure.AppendChar( c );
                }
                secure.MakeReadOnly();
                return secure;
            }
            catch
            {
                secure.Dispose();
                throw;
            }
        }

        #endregion

        #region IEquatable<Superstring>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals( Superstring other ) => Equals( this, other );

        #endregion

        #region IComparable<Superstring>

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows,
        /// or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// Value Meaning Less than zero. This instance precedes <paramref name="other"/> in the sort order.
        /// Zero This instance occurs in the same position in the sort order as <paramref name="other"/>.
        /// Greater than zero This instance follows <paramref name="other"/> in the sort order. 
        /// </returns>
        /// <param name="other">An object to compare with this instance.</param>
        public int CompareTo( Superstring other ) => Compare( this, other );

        #endregion

        #region IComparable

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows,
        /// or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// Value Meaning Less than zero. This instance precedes <paramref name="obj"/> in the sort order.
        /// Zero This instance occurs in the same position in the sort order as <paramref name="obj"/>.
        /// Greater than zero This instance follows <paramref name="obj"/> in the sort order. 
        /// </returns>
        /// <param name="obj">An object to compare with this instance. </param>
        /// <exception cref="ArgumentException"><paramref name="obj"/> is not the same type as this instance.</exception>
        public virtual int CompareTo( object obj )
        {
            var other = obj as Superstring;
            if( obj == null )
            {
                throw new ArgumentException( Invariant( $"obj is not {nameof( Superstring )}" ), nameof( obj ) );
            }
            return CompareTo( other );
        }

        #endregion

        #region ICloneable

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone() => MemberwiseClone();

        #endregion

        #region IEnumerable<char>

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<char> GetEnumerator() => Internal.ToString().GetEnumerator();

        #endregion

        #region IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region IConvertible

        /// <summary>
        /// Returns the <see cref="TypeCode"/> for this instance.
        /// </summary>
        /// <returns>
        /// The enumerated constant that is the <see cref="TypeCode"/> of the class or value type that implements this interface.
        /// </returns>
        public TypeCode GetTypeCode() => TypeCode.String;

        /// <summary>
        /// Converts the value of this instance to an equivalent Boolean value using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// A Boolean value equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        [SuppressMessage( "ReSharper", "SimplifyConditionalTernaryExpression", Justification = "OK" )]
        public bool ToBoolean( IFormatProvider provider )
        {
            bool result;
            return bool.TryParse( Internal.ToString(), out result )
                ? result
                : default( bool );
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent Unicode character using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// A Unicode character equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        public char ToChar( IFormatProvider provider )
        {
            char result;
            return char.TryParse( Internal.ToString(), out result )
                ? result
                : default( char );
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 8-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An 8-bit signed integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        [CLSCompliant( false )]
        public sbyte ToSByte( IFormatProvider provider )
        {
            sbyte result;
            return sbyte.TryParse( Internal.ToString(), NumberStyles.Any, provider, out result )
                ? result
                : default( sbyte );
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 8-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An 8-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        public byte ToByte( IFormatProvider provider )
        {
            byte result;
            return byte.TryParse( Internal.ToString(), NumberStyles.Any, provider, out result )
                ? result
                : default( byte );
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 16-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An 16-bit signed integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        public short ToInt16( IFormatProvider provider )
        {
            short result;
            return short.TryParse( Internal.ToString(), NumberStyles.Any, provider, out result )
                ? result
                : default( short );
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 16-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An 16-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        [CLSCompliant( false )]
        public ushort ToUInt16( IFormatProvider provider )
        {
            ushort result;
            return ushort.TryParse( Internal.ToString(), NumberStyles.Any, provider, out result )
                ? result
                : default( ushort );
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 32-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An 32-bit signed integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        public int ToInt32( IFormatProvider provider )
        {
            int result;
            return int.TryParse( Internal.ToString(), NumberStyles.Any, provider, out result )
                ? result
                : default( int );
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 32-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An 32-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        [CLSCompliant( false )]
        public uint ToUInt32( IFormatProvider provider )
        {
            uint result;
            return uint.TryParse( Internal.ToString(), NumberStyles.Any, provider, out result )
                ? result
                : default( uint );
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 64-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An 64-bit signed integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        public long ToInt64( IFormatProvider provider )
        {
            long result;
            return long.TryParse( Internal.ToString(), NumberStyles.Any, provider, out result )
                ? result
                : default( long );
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 64-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An 64-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        [CLSCompliant( false )]
        public ulong ToUInt64( IFormatProvider provider )
        {
            ulong result;
            return ulong.TryParse( Internal.ToString(), NumberStyles.Any, provider, out result )
                ? result
                : default( ulong );
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent single-precision floating-point number using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// A single-precision floating-point number equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        public float ToSingle( IFormatProvider provider )
        {
            float result;
            return float.TryParse( Internal.ToString(), NumberStyles.Any, provider, out result )
                ? result
                : default( float );
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent double-precision floating-point number using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// A double-precision floating-point number equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        public double ToDouble( IFormatProvider provider )
        {
            double result;
            return double.TryParse( Internal.ToString(), NumberStyles.Any, provider, out result )
                ? result
                : default( double );
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent <see cref="decimal"/> number using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// A <see cref="decimal"/> number equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        public decimal ToDecimal( IFormatProvider provider )
        {
            decimal result;
            return decimal.TryParse( Internal.ToString(), NumberStyles.Any, provider, out result )
                ? result
                : default( decimal );
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent <see cref="DateTime"/> using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// A <see cref="DateTime"/> instance equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        public DateTime ToDateTime( IFormatProvider provider )
        {
            DateTime result;
            return DateTime.TryParse( Internal.ToString(), provider, DateTimeStyles.None, out result )
                ? result
                : default( DateTime );
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent <see cref="String"/> using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// A <see cref="String"/> instance equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information. </param>
        public string ToString( IFormatProvider provider ) => ToString();

        /// <summary>
        /// Converts the value of this instance to an <see cref="object"/> of the specified <see cref="Type"/> that has an equivalent value, using the specified
        /// culture-specific formatting information.
        /// </summary>
        /// <returns>
        /// An <see cref="object"/> instance of type <paramref name="conversionType"/> whose value is equivalent to the value of this instance.
        /// </returns>
        /// <param name="conversionType">The <see cref="Type"/> to which the value of this instance is converted. </param>
        /// <param name="provider">An <see cref="IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        public object ToType( Type conversionType, IFormatProvider provider )
        {
            if( conversionType == null )
            {
                return null;
            }
            if( conversionType == typeof( Nullable<> ) )
            {
                conversionType = Nullable.GetUnderlyingType( conversionType );
            }
            if( conversionType == typeof( byte ) )
            {
                return ToByte( provider );
            }
            if( conversionType == typeof( short ) )
            {
                return ToInt16( provider );
            }
            if( conversionType == typeof( int ) )
            {
                return ToInt32( provider );
            }
            if( conversionType == typeof( long ) )
            {
                return ToInt64( provider );
            }
            if( conversionType == typeof( sbyte ) )
            {
                return ToSByte( provider );
            }
            if( conversionType == typeof( ushort ) )
            {
                return ToUInt16( provider );
            }
            if( conversionType == typeof( uint ) )
            {
                return ToUInt32( provider );
            }
            if( conversionType == typeof( ulong ) )
            {
                return ToUInt64( provider );
            }
            if( conversionType == typeof( float ) )
            {
                return ToSingle( provider );
            }
            if( conversionType == typeof( double ) )
            {
                return ToDouble( provider );
            }
            if( conversionType == typeof( decimal ) )
            {
                return ToDecimal( provider );
            }
            if( conversionType == typeof( bool ) )
            {
                return ToBoolean( provider );
            }
            if( conversionType == typeof( DateTime ) )
            {
                return ToDateTime( provider );
            }
            if( conversionType == typeof( char ) )
            {
                return ToChar( provider );
            }
            if( conversionType == typeof( string ) )
            {
                return ToString( provider );
            }
            if( conversionType == typeof( SecureString ) )
            {
                return ToSecureString( provider );
            }
            throw new FormatException(); // TODO Specific exception.
        }

        #endregion
    }
}
