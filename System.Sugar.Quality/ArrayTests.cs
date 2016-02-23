namespace System.Sugar.Quality
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static Arrays;
    using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

    [TestClass]
    public class ArrayTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestConstrunctors()
        {
            var one = CreateInstance<int>( 3 );
            var two = CreateInstance<string>();
            var three = InitializeInstance( 1, 2, 3 );
            var four = InitializeInstance( "a" );

            IsNotNull( one );
            IsNotNull( two );
            IsNotNull( three );
            IsNotNull( four );
        }
    }
}
