using LibCompressionEstimator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace CompressionEstimator.Tests
{
    [TestClass]
    public class StreamEstimatorTests
    {
        [TestMethod]
        public void EstimatePackedSizeTest()
        {
            //Arrange
            var se = new StreamEstimator();
            var stream = CreateSampleCompressibleStream(1024 * 1024);
            //Act
            var result = se.EstimatePackedSize(stream, 1024 * 100, 1024);
            //Assert
            var expectedLength = 4741; //validated once manually
            Assert.AreEqual(expectedLength, result);
        }

        private MemoryStream CreateSampleCompressibleStream(int bytes)
        {
            var byteArray = new byte[bytes];
            return new MemoryStream(byteArray, writable: false);
        }
    }
}
