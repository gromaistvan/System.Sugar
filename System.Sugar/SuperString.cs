namespace System
{
    using Collections;
    using Collections.Generic;
    using Diagnostics;
    using Diagnostics.CodeAnalysis;
    using Globalization;
    using JetBrains.Annotations;
    using Linq;
    using Security;
    using Text;
    using Text.RegularExpressions;
    using static FormattableString;
    using static StringComparison;

    /// <summary>
    ///     Fancy class for using smart value manipulation.
    /// </summary>
    /// <remarks>
    ///     Lots of syntactic sugar over strung usage.
    /// </remarks>
    /// <example>
    ///     <code>
    ///     Superstring str = "Some text";
    ///     str += " to edit.";
    ///     str[str.Length - 1] = '!';
    ///     </code>
    /// </example>
    [SuppressMessage( "Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "type",
        Target = "System.Superstring", Justification = "OK" )]
    [DebuggerDisplay( "{Internal.ToString()}" )]
    public class Superstring : IEquatable<Superstring>, IComparable<Superstring>, IComparable, ICloneable, IEnumerable<char>, IConvertible
    {
        #region Static fields

        [NotNull]
        static readonly char[] _whiteChars = { ' ', '\t', '\v', '\f', '\n', };

        [NotNull]
        static readonly char[] _extendedWhiteChars = { ' ', '\t', '\v', '\f', '\n', '\r', };

        #endregion

        #region ICloneable

        /// <summary>
        ///     Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        ///     A new object that is a copy of this instance.
        /// </returns>
        [Pure]
        public object Clone() => MemberwiseClone();

        #endregion

        #region IComparable

        /// <summary>
        ///     Compares the current instance with another object of the same type and returns an integer that indicates whether
        ///     the current instance precedes, follows,
        ///     or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <returns>
        ///     A value that indicates the relative order of the objects being compared. The return value has these meanings:
        ///     Value Meaning Less than zero. This instance precedes <paramref name="obj" /> in the sort order.
        ///     Zero This instance occurs in the same position in the sort order as <paramref name="obj" />.
        ///     Greater than zero This instance follows <paramref name="obj" /> in the sort order.
        /// </returns>
        /// <param name="obj">An object to compare with this instance. </param>
        /// <exception cref="ArgumentException"><paramref name="obj" /> is not the same type as this instance.</exception>
        [Pure]
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

        #region IComparable<Superstring>

        /// <summary>
        ///     Compares the current instance with another object of the same type and returns an integer that indicates whether
        ///     the current instance precedes, follows,
        ///     or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <returns>
        ///     A value that indicates the relative order of the objects being compared. The return value has these meanings:
        ///     Value Meaning Less than zero. This instance precedes <paramref name="other" /> in the sort order.
        ///     Zero This instance occurs in the same position in the sort order as <paramref name="other" />.
        ///     Greater than zero This instance follows <paramref name="other" /> in the sort order.
        /// </returns>
        /// <param name="other">An object to compare with this instance.</param>
        [Pure]
        public int CompareTo( Superstring other ) => Compare( this, other );

        #endregion

        #region IEnumerable<char>

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     An enumerator that can be used to iterate through the collection.
        /// </returns>
        [Pure]
        public IEnumerator<char> GetEnumerator() => Internal.ToString().GetEnumerator();

        #endregion

        #region IEnumerable

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        [Pure]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region IEquatable<Superstring>

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        ///     true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        [Pure]
        public bool Equals( Superstring other ) => Equals( this, other );

        #endregion

        #region Friendly static functions 

        /// <summary>
        ///     General equivalence.
        /// </summary>
        /// <param name="left">A value.</param>
        /// <param name="right">An other value.</param>
        /// <returns>Equivalent?</returns>
        [Pure]
        public static bool Equals( Superstring left, Superstring right ) =>
            ReferenceEquals( left, right )
            ||
            ( left?.Internal.Equals( right?.Internal ) ?? false );

        /// <summary>
        ///     General alphabetic compare.
        /// </summary>
        /// <param name="left">A value.</param>
        /// <param name="right">An other value.</param>
        /// <returns>
        ///     A value that indicates the relative order of the objects being compared. The return value has these meanings:
        ///     Value Meaning Less than zero <paramref name="left" /> precedes <paramref name="right" /> in the sort order.
        ///     Zero <paramref name="left" /> occurs in the same position in the sort order as <paramref name="right" />.
        ///     Greater than zero <paramref name="left" /> follows <paramref name="right" /> in the sort order.
        /// </returns>
        [Pure]
        public static int Compare( Superstring left, Superstring right ) =>
            string.Compare(
                left?.ToString( CultureInfo.InvariantCulture ),
                right?.ToString( CultureInfo.InvariantCulture ),
                OrdinalIgnoreCase );

        /// <summary>
        ///     Postfix concatenation.
        /// </summary>
        /// <param name="string">The value.</param>
        /// <param name="value">The value to append.</param>
        /// <returns>The <paramref name="string" /> instance.</returns>
        public static Superstring Add( Superstring @string, object value )
        {
            if( @string == null )
            {
                return null;
            }
            if( value == null )
            {
                return @string;
            }
            lock( @string.SyncRoot )
            {
                @string.Internal.Append( value );
            }
            return @string;
        }

        /// <summary>
        ///     Repate concatenation.
        /// </summary>
        /// <param name="string">The value.</param>
        /// <param name="count">Repeat count.</param>
        /// <returns>The <paramref name="string" /> instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count" /> is negative!</exception>
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
                lock( @string.SyncRoot )
                {
                    @string.Internal.Clear();
                }
                return @string;
            }
            if( count == 1 )
            {
                return @string;
            }
            lock( @string.SyncRoot )
            {
                var value = @string.Internal.ToString();
                for( var i = count - 1; i > 0; --i )
                {
                    @string.Internal.Append( value );
                }
            }
            return @string;
        }

        #endregion

        #region Operators

        /// <summary>
        ///     Equivalence.
        /// </summary>
        /// <param name="left">A value.</param>
        /// <param name="right">An other value.</param>
        /// <returns>Equivalent?</returns>
        public static bool operator ==( Superstring left, Superstring right ) => Equals( left, right );

        /// <summary>
        ///     Inequivalence.
        /// </summary>
        /// <param name="left">A value.</param>
        /// <param name="right">An other value.</param>
        /// <returns>Inequivalent?</returns>
        public static bool operator !=( Superstring left, Superstring right ) => ! Equals( left, right );

        /// <summary>
        ///     Less than.
        /// </summary>
        /// <param name="left">A value.</param>
        /// <param name="right">An other value.</param>
        public static bool operator <( Superstring left, Superstring right ) => Compare( left, right ) < 0;

        /// <summary>
        ///     Less than or equal.
        /// </summary>
        /// <param name="left">A value.</param>
        /// <param name="right">An other value.</param>
        public static bool operator <=( Superstring left, Superstring right ) => Compare( left, right ) <= 0;

        /// <summary>
        ///     Greather than.
        /// </summary>
        /// <param name="left">A value.</param>
        /// <param name="right">An other value.</param>
        public static bool operator >( Superstring left, Superstring right ) => Compare( left, right ) > 0;

        /// <summary>
        ///     Greather than or equal.
        /// </summary>
        /// <param name="left">A value.</param>
        /// <param name="right">An other value.</param>
        public static bool operator >=( Superstring left, Superstring right ) => Compare( left, right ) >= 0;

        /// <summary>
        ///     Postfix concatenation.
        /// </summary>
        /// <param name="left">The value.</param>
        /// <param name="right">The value to append.</param>
        /// <returns>The <paramref name="left" /> instance.</returns>
        public static Superstring operator +( Superstring left, object right ) => Add( left, right );

        /// <summary>
        ///     Repate concatenation.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="count">Repeat count.</param>
        /// <returns>The <paramref name="value" /> instance.</returns>
        public static Superstring operator *( Superstring value, int count ) => Multiply( value, count );

        /// <summary>
        ///     Used for the controlling expression in <c>if</c>, <c>do</c>, <c>while</c>, and <c>for</c> statements.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The string is not empty.</returns>
        public static bool operator true( Superstring value ) => value?.IsTrue ?? false;

        /// <summary>
        ///     Used for the controlling expression in <c>if</c>, <c>do</c>, <c>while</c>, and <c>for</c> statements.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The string is empty.</returns>
        public static bool operator false( Superstring value ) => ! ( value?.IsTrue ?? false );

        /// <summary>
        ///     Casting.
        /// </summary>
        [NotNull]
        public static implicit operator Superstring( string value ) => new Superstring( value );

        /// <summary>
        ///     Casting.
        /// </summary>
        [NotNull]
        public static implicit operator string( Superstring value )
            => value?.ToString( CultureInfo.InvariantCulture ) ?? "";

        /// <summary>
        ///     Casting.
        /// </summary>
        [NotNull]
        public static implicit operator SecureString( Superstring value )
            => value?.ToSecureString( CultureInfo.InvariantCulture ) ?? new SecureString();

        #endregion

        #region Fields

        /// <summary>
        ///     For thread safety.
        /// </summary>
        [NotNull, DebuggerBrowsable( DebuggerBrowsableState.Never )]
        protected object SyncRoot { get; } = new object();

        /// <summary>
        ///     Used for implementation logic.
        /// </summary>
        [NotNull, DebuggerBrowsable( DebuggerBrowsableState.Never )]
        protected StringBuilder Internal { get; } = new StringBuilder();

        #endregion

        #region Constructors

        /// <summary>
        ///     Empty constructor.
        /// </summary>
        public Superstring()
        {}

        /// <summary>
        ///     String constructor.
        /// </summary>
        /// <param name="value">Some text.</param>
        public Superstring( string value )
        {
            Internal = new StringBuilder( value ?? "" );
        }

        /// <summary>
        ///     Repeated value constructor.
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

        /// <summary>
        ///     Join constructor.
        /// </summary>
        /// <param name="values">Texts to join.</param>
        /// <param name="separator">Separator.</param>
        /// <exception cref="ArgumentNullException"><paramref name="values" /> is null.</exception>
        public Superstring( [NotNull] IEnumerable<string> values, string separator = " " )
        {
            if( values == null )
            {
                throw new ArgumentNullException( nameof( values ) );
            }

            foreach( var value in values )
            {
                if( Internal.Length > 0 )
                {
                    Internal.Append( separator );
                }
                Internal.Append( value );
            }
        }

        #endregion

        #region Object overrides

        /// <summary>
        ///     Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        ///     true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals( object obj ) => Equals( obj as Superstring );

        /// <summary>
        ///     Serves as the default hash function.
        /// </summary>
        /// <returns>
        ///     A hash code for the current object.
        /// </returns>
        public override int GetHashCode() => Internal.GetHashCode();

        /// <summary>
        ///     Returns a value that represents the current object.
        /// </summary>
        /// <returns>
        ///     A value that represents the current object.
        /// </returns>
        public override string ToString() => Internal.ToString();

        #endregion

        #region Properties and methods

        /// <summary>
        ///     The length of the value.
        /// </summary>
        public int Length => Internal.Length;

        /// <summary>
        ///     Word count.
        /// </summary>
        public int Words { get { throw new NotImplementedException(); } }

        /// <summary>
        ///     Empty strings are <c>false</c>.
        /// </summary>
        /// <returns>Not empty!</returns>
        public bool IsTrue => ! string.IsNullOrEmpty( Internal.ToString() );

        /// <summary>
        ///     Get character at given position.
        /// </summary>
        /// <param name="index">0-based start position.</param>
        /// <returns>Character at position.</returns>
        public char this[ int index ]
        {
            get
            {
                if( index < 0 )
                {
                    throw new ArgumentOutOfRangeException( nameof( index ) );
                }
                if( index >= Internal.Length )
                {
                    throw new ArgumentOutOfRangeException( nameof( index ) );
                }

                return Internal[ index ];
            }
            set
            {
                if( index < 0 )
                {
                    throw new ArgumentOutOfRangeException( nameof( index ) );
                }
                if( index >= Internal.Length )
                {
                    throw new ArgumentOutOfRangeException( nameof( index ) );
                }

                lock( Internal )
                {
                    Internal[ index ] = value;
                }
            }
        }

        /// <summary>
        ///     Slice substring.
        /// </summary>
        /// <param name="start">0-based start position (inclusive).</param>
        /// <param name="end">0-based end position (inclusive).</param>
        /// <returns>Substring.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Invalid position parameters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        [SuppressMessage( "Microsoft.Design", "CA1023:IndexersShouldNotBeMultidimensional", Justification = "OK" )]
        [NotNull]
        public string this[ int start, int end ]
        {
            get
            {
                if( start < 0 )
                {
                    throw new ArgumentOutOfRangeException( nameof( start ) );
                }
                if( end < 0 )
                {
                    throw new ArgumentOutOfRangeException( nameof( end ) );
                }
                if( start >= Internal.Length )
                {
                    throw new ArgumentOutOfRangeException( nameof( start ) );
                }
                if( end >= Internal.Length )
                {
                    throw new ArgumentOutOfRangeException( nameof( end ) );
                }
                if( end < start )
                {
                    throw new ArgumentOutOfRangeException( nameof( end ) );
                }

                return Internal.ToString().Substring( start, end - start + 1 );
            }
            set
            {
                if( value == null )
                {
                    throw new ArgumentNullException( nameof( value ) );
                }
                if( start < 0 )
                {
                    throw new ArgumentOutOfRangeException( nameof( start ) );
                }
                if( end < 0 )
                {
                    throw new ArgumentOutOfRangeException( nameof( end ) );
                }
                if( start >= Internal.Length )
                {
                    throw new ArgumentOutOfRangeException( nameof( start ) );
                }
                if( end >= Internal.Length )
                {
                    throw new ArgumentOutOfRangeException( nameof( end ) );
                }
                if( end < start )
                {
                    throw new ArgumentOutOfRangeException( nameof( end ) );
                }

                lock( Internal )
                {
                    Internal.Remove( start, end - start + 1 ).Insert( start, value );
                }
            }
        }

        /// <summary>
        ///     Convert to <see cref="SecureString" /> for using as password.
        /// </summary>
        /// <param name="provider">
        ///     An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific
        ///     formatting information.
        /// </param>
        /// <returns>Secure memory representation of value.</returns>
        [SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "provider",
            Justification = "OK" )]
        [NotNull]
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

        /// <summary>
        /// Split string to parts.
        /// </summary>
        /// <param name="chars">Separator characters.</param>
        /// <returns>String parts.</returns>
        public IEnumerable<string> Split( params char[] chars ) => Internal.ToString().Split( chars );

        /// <summary>
        /// Split string to parts by whitespaces.
        /// </summary>
        /// <returns>String parts.</returns>
        public IEnumerable<string> Split() => Internal.ToString().Split( _whiteChars );

        public Superstring Shuffle( params char[] chars )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Trim specified characters from the begining.
        /// </summary>
        /// <param name="chars">Characters to trim.</param>
        /// <returns>Trimmed string.</returns>
        public Superstring LeftTrim( [NotNull] params char[] chars )
        {
            if( chars == null ) throw new ArgumentNullException( nameof( chars ) );

            for( var c = Internal[ 0 ];
                 Array.BinarySearch( chars, c ) >= 0;
                 c = Internal[ 0 ] )
            {
                Internal.Remove( 0, 1 );
            }
            return this;
        }

        /// <summary>
        /// Trim whitespaces from the begining.
        /// </summary>
        /// <returns>Trimmed string.</returns>
        public Superstring LeftTrim() => LeftTrim( _extendedWhiteChars );

        /// <summary>
        /// Trim specified characters from the end.
        /// </summary>
        /// <param name="chars">Characters to trim.</param>
        /// <returns>Trimmed string.</returns>
        public Superstring RightTrim( params char[] chars )
        {
            if( chars == null )
            {
                throw new ArgumentNullException( nameof( chars ) );
            }

            var l = Internal.Length - 1;
            for( var c = Internal[ l ];
                 Array.BinarySearch( chars, c ) >= 0;
                 c = Internal[ l ] )
            {
                Internal.Remove( l--, 1 );
            }
            return this;
        }
        
        /// <summary>
        /// Trim whitespaces from the end.
        /// </summary>
        /// <returns>Trimmed string.</returns>
        public Superstring RightTrim() => RightTrim( _extendedWhiteChars );

        public Superstring LeftPad( int count, char @char = ' ' )
        {
            throw new NotImplementedException();
        }

        public Superstring RightPad( int count, char @char = ' ' )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check string begins with a given text.
        /// </summary>
        /// <param name="str">The prefix string.</param>
        /// <param name="caseSensitive">Check case-sensitive.</param>
        /// <returns>Prefix matches.</returns>
        public bool StartsWith( [NotNull] string str, bool caseSensitive = true )
        {
            if( str == null ) throw new ArgumentNullException( nameof( str ) );

            return
                ( str.Length <= Internal.Length )
                &&
                Internal.ToString( 0, str.Length ).Equals( str, caseSensitive ? Ordinal : OrdinalIgnoreCase );
        }

        /// <summary>
        /// Check string ends with a given text.
        /// </summary>
        /// <param name="str">The postfix string.</param>
        /// <param name="caseSensitive">Check case-sensitive.</param>
        /// <returns>postfix matches.</returns>
        public bool EndsWith( [NotNull] string str, bool caseSensitive = true )
        {
            if( str == null ) throw new ArgumentNullException( nameof( str ) );

            return
                ( str.Length <= Internal.Length )
                &&
                Internal.ToString( Internal.Length - str.Length, str.Length ).Equals( str, caseSensitive ? Ordinal : OrdinalIgnoreCase );
        }

        public bool Contains( [NotNull] string str, bool caseSeinsitive = true )
        {
            if( str == null )
            {
                throw new ArgumentNullException( nameof( str ) );
            }

            return
                ( str.Length <= Internal.Length )
                &&
                ( Internal.ToString().IndexOf( str, caseSeinsitive ? Ordinal : OrdinalIgnoreCase ) != -1 );
        }

        /// <summary>
        /// Uppercase the string.
        /// </summary>
        /// <returns>The string with uppercase letters.</returns>
        public Superstring ToUpper()
        {
            for( var i = 0; i < Internal.Length; i++ )
            {
                Internal[i] = char.ToUpperInvariant( Internal[i] );
            }
            return this;
        }

        /// <summary>
        /// Lowercase the string.
        /// </summary>
        /// <returns>The string with lowercase letters.</returns>
        public Superstring ToLower()
        {
            for( var i = 0; i < Internal.Length; i++ )
            {
                Internal[i] = char.ToLowerInvariant( Internal[i] );
            }
            return this;
        }

        public Superstring Wrap( int lineLength, string separator = null )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Replaces all occurrence in string.
        /// </summary>
        /// <param name="pattern">Pattern to search for.</param>
        /// <param name="replacement">The text to be substituted.</param>
        /// <returns>The new string.</returns>
        public Superstring Replace( string pattern, string replacement )
        {
            if( pattern == null )
            {
                return this;
            }
            Internal.Replace( pattern, replacement );
            return this;
        }

        /// <summary>
        /// Replaces all occurrence in string.
        /// </summary>
        /// <param name="replacements">Pattern - substitution pairs.</param>
        /// <returns>The new string.</returns>
        public Superstring Replace( IReadOnlyDictionary<string, string> replacements )
        {
            if( replacements == null )
            {
                return this;
            }
            lock( SyncRoot )
            {
                foreach( var pair in replacements.Where( pair => ! string.IsNullOrEmpty( pair.Key ) ) )
                {
                    Internal.Replace( pair.Key ?? "", pair.Value ?? "" );
                }
            }
            return this;
        }

        /// <summary>
        /// Replaces all occurrence in string.
        /// </summary>
        /// <param name="pattern">Pattern to search for.</param>
        /// <param name="replacement">The text to be substituted.</param>
        /// <returns>The new string.</returns>
        public Superstring Replace( [NotNull] Regex pattern, string replacement )
        {
            if( pattern == null ) throw new ArgumentNullException( nameof( pattern ) );
            
            lock( SyncRoot )
            {
                var input = Internal.ToString();
                var replaced = pattern.Replace( input, replacement ?? "" );
                Internal.Clear().Append( replaced );    
            }
            return this;
        }

        public Superstring Reverse()
        {
            throw new NotImplementedException();
            return this;
        }

        public Superstring Shuffle()
        {
            throw new NotImplementedException();
            return this;
        }

        #endregion

        #region IConvertible

        /// <summary>
        ///     Returns the <see cref="TypeCode" /> for this instance.
        /// </summary>
        /// <returns>
        ///     The enumerated constant that is the <see cref="TypeCode" /> of the class or value type that implements this
        ///     interface.
        /// </returns>
        public TypeCode GetTypeCode() => TypeCode.String;

        /// <summary>
        ///     Converts the value of this instance to an equivalent Boolean value using the specified culture-specific formatting
        ///     information.
        /// </summary>
        /// <returns>
        ///     A Boolean value equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific
        ///     formatting information.
        /// </param>
        [SuppressMessage( "ReSharper", "SimplifyConditionalTernaryExpression", Justification = "OK" )]
        public bool ToBoolean( IFormatProvider provider )
        {
            bool result;
            return bool.TryParse( Internal.ToString(), out result )
                ? result
                : default ( bool );
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent Unicode character using the specified culture-specific
        ///     formatting information.
        /// </summary>
        /// <returns>
        ///     A Unicode character equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific
        ///     formatting information.
        /// </param>
        public char ToChar( IFormatProvider provider )
        {
            char result;
            return char.TryParse( Internal.ToString(), out result )
                ? result
                : default ( char );
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent 8-bit signed integer using the specified culture-specific
        ///     formatting information.
        /// </summary>
        /// <returns>
        ///     An 8-bit signed integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific
        ///     formatting information.
        /// </param>
        [CLSCompliant( false )]
        public sbyte ToSByte( IFormatProvider provider )
        {
            sbyte result;
            return sbyte.TryParse( Internal.ToString(), NumberStyles.Any, provider, out result )
                ? result
                : default ( sbyte );
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent 8-bit unsigned integer using the specified culture-specific
        ///     formatting information.
        /// </summary>
        /// <returns>
        ///     An 8-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific
        ///     formatting information.
        /// </param>
        public byte ToByte( IFormatProvider provider )
        {
            byte result;
            return byte.TryParse( Internal.ToString(), NumberStyles.Any, provider, out result )
                ? result
                : default ( byte );
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent 16-bit signed integer using the specified culture-specific
        ///     formatting information.
        /// </summary>
        /// <returns>
        ///     An 16-bit signed integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific
        ///     formatting information.
        /// </param>
        public short ToInt16( IFormatProvider provider )
        {
            short result;
            return short.TryParse( Internal.ToString(), NumberStyles.Any, provider, out result )
                ? result
                : default ( short );
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent 16-bit unsigned integer using the specified culture-specific
        ///     formatting information.
        /// </summary>
        /// <returns>
        ///     An 16-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific
        ///     formatting information.
        /// </param>
        [CLSCompliant( false )]
        public ushort ToUInt16( IFormatProvider provider )
        {
            ushort result;
            return ushort.TryParse( Internal.ToString(), NumberStyles.Any, provider, out result )
                ? result
                : default ( ushort );
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent 32-bit signed integer using the specified culture-specific
        ///     formatting information.
        /// </summary>
        /// <returns>
        ///     An 32-bit signed integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific
        ///     formatting information.
        /// </param>
        public int ToInt32( IFormatProvider provider )
        {
            int result;
            return int.TryParse( Internal.ToString(), NumberStyles.Any, provider, out result )
                ? result
                : default ( int );
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent 32-bit unsigned integer using the specified culture-specific
        ///     formatting information.
        /// </summary>
        /// <returns>
        ///     An 32-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific
        ///     formatting information.
        /// </param>
        [CLSCompliant( false )]
        public uint ToUInt32( IFormatProvider provider )
        {
            uint result;
            return uint.TryParse( Internal.ToString(), NumberStyles.Any, provider, out result )
                ? result
                : default ( uint );
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent 64-bit signed integer using the specified culture-specific
        ///     formatting information.
        /// </summary>
        /// <returns>
        ///     An 64-bit signed integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific
        ///     formatting information.
        /// </param>
        public long ToInt64( IFormatProvider provider )
        {
            long result;
            return long.TryParse( Internal.ToString(), NumberStyles.Any, provider, out result )
                ? result
                : default ( long );
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent 64-bit unsigned integer using the specified culture-specific
        ///     formatting information.
        /// </summary>
        /// <returns>
        ///     An 64-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific
        ///     formatting information.
        /// </param>
        [CLSCompliant( false )]
        public ulong ToUInt64( IFormatProvider provider )
        {
            ulong result;
            return ulong.TryParse( Internal.ToString(), NumberStyles.Any, provider, out result )
                ? result
                : default ( ulong );
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent single-precision floating-point number using the specified
        ///     culture-specific formatting information.
        /// </summary>
        /// <returns>
        ///     A single-precision floating-point number equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific
        ///     formatting information.
        /// </param>
        public float ToSingle( IFormatProvider provider )
        {
            float result;
            return float.TryParse( Internal.ToString(), NumberStyles.Any, provider, out result )
                ? result
                : default ( float );
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent double-precision floating-point number using the specified
        ///     culture-specific formatting information.
        /// </summary>
        /// <returns>
        ///     A double-precision floating-point number equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific
        ///     formatting information.
        /// </param>
        public double ToDouble( IFormatProvider provider )
        {
            double result;
            return double.TryParse( Internal.ToString(), NumberStyles.Any, provider, out result )
                ? result
                : default ( double );
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent <see cref="decimal" /> number using the specified
        ///     culture-specific formatting information.
        /// </summary>
        /// <returns>
        ///     A <see cref="decimal" /> number equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific
        ///     formatting information.
        /// </param>
        public decimal ToDecimal( IFormatProvider provider )
        {
            decimal result;
            return decimal.TryParse( Internal.ToString(), NumberStyles.Any, provider, out result )
                ? result
                : default ( decimal );
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent <see cref="DateTime" /> using the specified culture-specific
        ///     formatting information.
        /// </summary>
        /// <returns>
        ///     A <see cref="DateTime" /> instance equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific
        ///     formatting information.
        /// </param>
        public DateTime ToDateTime( IFormatProvider provider )
        {
            DateTime result;
            return DateTime.TryParse( Internal.ToString(), provider, DateTimeStyles.None, out result )
                ? result
                : default ( DateTime );
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent <see cref="String" /> using the specified culture-specific
        ///     formatting information.
        /// </summary>
        /// <returns>
        ///     A <see cref="String" /> instance equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific
        ///     formatting information.
        /// </param>
        public string ToString( IFormatProvider provider ) => ToString();

        /// <summary>
        ///     Converts the value of this instance to an <see cref="object" /> of the specified <see cref="Type" /> that has an
        ///     equivalent value, using the specified
        ///     culture-specific formatting information.
        /// </summary>
        /// <returns>
        ///     An <see cref="object" /> instance of type <paramref name="conversionType" /> whose value is equivalent to the value
        ///     of this instance.
        /// </returns>
        /// <param name="conversionType">The <see cref="Type" /> to which the value of this instance is converted. </param>
        /// <param name="provider">
        ///     An <see cref="IFormatProvider" /> interface implementation that supplies culture-specific
        ///     formatting information.
        /// </param>
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
