namespace System.Sugar.Quality
{
    using Diagnostics.CodeAnalysis;
    using Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

    [TestClass]
    public class SuperstringTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod, Timeout( TestTimeout.Infinite )]
        [SuppressMessage( "ReSharper", "RedundantArgumentNameForLiteralExpression" )]
        public void TestConstrunctors()
        {
            var one = new Superstring( "aaaaa" );
            var trwo = new Superstring( Enumerable.Repeat( "a", 5 ), separator: "" );
            var three = new Superstring( "a", 5 );

            AreEqual( one, trwo );
            AreEqual( trwo, three );
        }

        [TestMethod, Timeout( TestTimeout.Infinite )]
        public void TestCasting()
        {
            const string text = "some text";
            Superstring str = text;
            IsNotNull( str );
            AreEqual( text, str.ToString( null ) );
        }

        [TestMethod, Timeout( TestTimeout.Infinite )]
        [SuppressMessage( "ReSharper", "ConvertIfToOrExpression" )]
        public void TestHasContent()
        {
            var content = new Superstring( "none" );
            var hasContent = false;
            if( content )
            {
                hasContent = true;
            }
            IsTrue( hasContent, "'if( content )' turned out to be false!" );
        }

        [TestMethod, Timeout( TestTimeout.Infinite )]
        [SuppressMessage( "ReSharper", "ConvertIfToOrExpression" )]
        public void TestEmpty()
        {
            var empty = new Superstring();
            var hasContent = false;
            if( empty )
            {
                hasContent = true;
            }
            IsFalse( hasContent, "'if( empty )' turned out to be true!" );
        }

        [TestMethod, Timeout( TestTimeout.Infinite )]
        [SuppressMessage( "ReSharper", "ConvertIfToOrExpression" )]
        [SuppressMessage( "ReSharper", "EqualExpressionComparison" )]
        public void TestAlphabeticalOrder()
        {
            Superstring apple = "apple", zen = "zen";
            IsTrue( zen <= zen, "Invalid alphabetic order (zen <= zen)!" );
            IsFalse( zen < zen, "Invalid alphabetic order (zen < zen)!" );
            IsTrue( apple <= zen, "Invalid alphabetic order (apple <= zen)!" );
            IsTrue( apple < zen, "Invalid alphabetic order (apple < zen)!" );
            IsFalse( apple > zen, "Invalid alphabetic order (apple > zen)!" );
        }

        [TestMethod, ExpectedException( typeof( ArgumentOutOfRangeException ) )]
        public void TestInvalidMultiple()
        {
            var str = new Superstring( "a" );
            str = str * -3;
            TestContext?.WriteLine( str );
        }

        [TestMethod, Timeout( TestTimeout.Infinite )]
        public void TestLength()
        {
            const int count = 6;
            var str = new Superstring( "a" );
            str *= count;
            IsNotNull( str );
            AreEqual( count, str.Length );
        }

        [TestMethod, Timeout( TestTimeout.Infinite )]
        public void TestSecure()
        {
            Superstring str = "Password";
            using( var secure = str.ToSecureString( null ) )
            {
                IsNotNull( secure );
                AreEqual( str.Length, secure.Length );
                IsTrue( secure.IsReadOnly(), "Not read only password!" );
            }
        }

        [TestMethod, Timeout( TestTimeout.Infinite )]
        public void TestSlice()
        {
            Superstring str = "s_me tet";
            AreEqual( '_', str[ 1 ] );
            AreEqual( "me", str[ 2, 3 ] );
            str[ 1 ] = 'o';
            str[ 5, 7 ] = "text";
            AreEqual( "some text", (string)str );
        }

        [TestMethod, Timeout( TestTimeout.Infinite )]
        public void TestConvertible()
        {
            Superstring str = "0";
            AreEqual( TypeCode.String, str.GetTypeCode() );
            AreEqual( 0, str.ToInt32( null ) );
            AreEqual( 0L, str.ToInt64( null ) );
            AreEqual( 0f, str.ToSingle( null ) );
            AreEqual( 0d, str.ToDouble( null ) );
            AreEqual( 0m, str.ToDecimal( null ) );
            AreEqual( false, str.ToBoolean( null ) );
            AreEqual( '0', str.ToChar( null ) );
        }
    }
}
