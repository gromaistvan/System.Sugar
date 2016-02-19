namespace System.Sugar.Quality
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

    [TestClass]
    public class SuperstringTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod, Timeout(100)]
        public void TestCasting()
        {
            Superstring str = "some text";
            IsNotNull( str );
        }

        [TestMethod, Timeout( 100 )]
        public void TestEmpty()
        {
            Superstring valid = new Superstring( "none" ), empty = new Superstring();
            if (valid)
            { }
            else
            {
                Fail( "if (valid) -> false" );
            }
            if (empty)
            {
                Fail( "if( empty ) -> true" );
            }
        }

        [TestMethod, Timeout( 100 )]
        public void TestLength()
        {
            var str = new Superstring( "a" );
            str *= 6;
            IsNotNull( str );
            AreEqual( 6, str.Length );
        }
    }
}
