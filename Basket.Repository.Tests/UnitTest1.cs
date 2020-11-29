using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Basket.Repository.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //arrange
            int x = 3;
            int y = 5;
            //reguestmodel olustur

            //act
            double min = x + y;
            //servise git null donecek sana

            //assert
            //sonucu buraya yazacaksin
            Assert.AreEqual(3, min);
        }
    }
}
