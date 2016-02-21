namespace System.Sugar.Quality
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

    [TestClass]
    public class SuperstringTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod, Timeout( TestTimeout.Infinite )]
        public void TestCasting()
        {
            Superstring str = "some text";
            IsNotNull( str );
        }

        [TestMethod, Timeout( TestTimeout.Infinite  )]
        public void TestEmpty()
        {
            Superstring valid = new Superstring( "none" ), empty = new Superstring();
            if( valid )
            { }
            else
            {
                Fail( "if (valid) -> false" );
            }
            if( empty )
            {
                Fail( "if( empty ) -> true" );
            }
        }

        [TestMethod, Timeout( TestTimeout.Infinite  )]
        public void TestLength()
        {
            const int count = 6;
            var str = new Superstring( "a" );
            str *= count;
            IsNotNull( str );
            AreEqual( count, str.Length );
        }
    }
}
