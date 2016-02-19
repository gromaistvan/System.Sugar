namespace System.Sugar.Quality
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

    [TestClass]
    public class SuperstringTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod, Timeout( 100 )]
        public void TestLength()
        {
            var sugar = new Superstring( "a" );
            sugar *= 6;
            IsNotNull( sugar );
            AreEqual( 6, sugar.Length );
        }
    }
}
