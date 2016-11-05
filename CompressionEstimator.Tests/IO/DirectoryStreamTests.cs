using LibCompressionEstimator.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace CompressionEstimate.Tests.Packer
{
    [TestClass]
   public class DirectoryStreamTests
    {

        /*layout of TestDirectory:
file1 - 0000
file2 - AA
file3 - BBB
file4 - CCCC
file5 - E

total bytes:14       
*/
        [TestMethod]
        public void LengthTest()
        {
            //arrange
            DirectoryStream ds = new DirectoryStream("TestDirectory");
            //act
            var length = ds.Length;
            //assert
            Assert.AreEqual(14L, length);
        }

        [TestMethod]
        public void ReadByteTest()
        {
            //arrange
            DirectoryStream ds = new DirectoryStream("TestDirectory");
            //act
            byte b = (byte)ds.ReadByte();
            //assert
            Assert.AreEqual((byte)'0', b);
        }

        [TestMethod]
        public void ReadByteTestAtPosition()
        {
            //arrange
            DirectoryStream ds = new DirectoryStream("TestDirectory");
            ds.Position = 5;
            byte b = (byte)ds.ReadByte();
            //assert
            Assert.AreEqual((byte)'A', b);
        }

        [TestMethod]
        public void ReadTest()
        {
            //arrange
            DirectoryStream ds = new DirectoryStream("TestDirectory");
            byte[] data = new byte[14];
            //act
            ds.Read(data, 0, 14);
            //assert
            byte[] expected = Encoding.ASCII.GetBytes("0000AABBBCCCCE");
            CollectionAssert.AreEqual(expected, data);
        }        

        [TestMethod]
        public void SeekTestBegin()
        {
            //arrange
            DirectoryStream ds = new DirectoryStream("TestDirectory");
            //act
            ds.Seek(7, System.IO.SeekOrigin.Begin);
            byte b = (byte)ds.ReadByte();
            //assert
            Assert.AreEqual((byte)'B', b);
        }

        [TestMethod]
        public void SeekTestCurrent()
        {
            //arrange
            DirectoryStream ds = new DirectoryStream("TestDirectory");
            ds.Position = 4;
            //act
            ds.Seek(1, System.IO.SeekOrigin.Current);
            byte b = (byte)ds.ReadByte();
            //assert
            Assert.AreEqual((byte)'A', b);
        }

        [TestMethod]
        public void ReadByteTestUpdatesPosition()
        {
            //arrange
            DirectoryStream ds = new DirectoryStream("TestDirectory");
            //act
            ds.ReadByte();
            //assert
            Assert.AreEqual(1, ds.Position);
        }

        [TestMethod]
        public void ReadTestUpdatesPosition()
        {
            //arrange
            DirectoryStream ds = new DirectoryStream("TestDirectory");
            byte[] buffer = new byte[4];
            ds.Position = 4;
            //act
            ds.Read(buffer, 0, 4);
            //assert
            Assert.AreEqual(8, ds.Position);
        }

        [TestMethod]
        public void ReadTestDoesntGoBeyondFiles()
        {
            //arrange
            DirectoryStream ds = new DirectoryStream("TestDirectory");
            byte[] buffer = new byte[32];
            //act
            int read = ds.Read(buffer, 0, 32);
            //assert
            Assert.AreEqual(14, read);
        }

        [TestMethod]
        public void ReadTestLargeFileFromStreamHasTheSameContentAsFileOnDisk()
        {
            //arrange
            int size = 1024 * 1024 * 2;
            DirectoryStream ds = new DirectoryStream("TestDirLarge");
            byte[] buffer = new byte[size];
            byte[] expected = new byte[size];
            using (var fs = File.Open(@"TestDirLarge\file0.bin", FileMode.Open))
            {
                fs.Read(expected, 0, size);
            }
            //act
            ds.Read(buffer, 0, size);
            //assert
            CollectionAssert.AreEqual(expected, buffer);
        }

        [TestMethod]
        public void ReadTestWithOffset()
        {
            //arrange
            var ds = new DirectoryStream("TestDirSameSize");
            const int bytesToRead = 20;
            byte[] buffer = new byte[bytesToRead];
            byte[] expected = Encoding.ASCII.GetBytes("11222222222233333333");
            ds.Position = 8;
            //act
            int read = ds.Read(buffer, 0, bytesToRead);
            //assert
            Assert.AreEqual(bytesToRead, read);
            CollectionAssert.AreEqual(expected, buffer);
        }

        [TestMethod]
        public void ReadTestAtFileBoundary()
        {
            //arrange
            var ds = new DirectoryStream("TestDirSameSize");
            const int bytesToRead = 10;
            byte[] buffer = new byte[bytesToRead];
            byte[] expected = Encoding.ASCII.GetBytes("2222222222");
            ds.Seek(10, SeekOrigin.Begin);
            //act
            int read = ds.Read(buffer, 0, bytesToRead);
            //assert
            Assert.AreEqual(bytesToRead, read);
            CollectionAssert.AreEqual(expected, buffer);
        }
    }
}
