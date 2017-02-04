using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Management;
using LibCompressionEstimator;

namespace CompressionEstimator.Tests
{
    [TestClass]
    public class DirectoryEstimatorTestsForCompression
    {
        const string BaseDirectory = "NtfsCompressionDir";
        const string Compressed = "Compressed";
        const string NotCompressed = "NotCompressed";

        [TestInitialize]
        public void Initialize()
        {
            Directory.CreateDirectory(BaseDirectory);
            Directory.CreateDirectory(Path.Combine(BaseDirectory,Compressed));
            Directory.CreateDirectory(Path.Combine(BaseDirectory, NotCompressed));
            SetCompressedAttribute(Path.Combine(BaseDirectory, Compressed));
        }

        [TestCleanup]
        public void Cleanup()
        {
            Directory.Delete(BaseDirectory, recursive: true);
        }

        [TestMethod]
        public void NtfsCompressedIsReported()
        {
            //arrange
            var de = new DirectoryEstimator(new NoOpLogger());
            //act
            var result = de.Estimate(BaseDirectory);
            //assert
            Assert.AreEqual(2, result.Count());
            var compressed = result.Single(p => p.ShortName == Compressed);
            Assert.IsTrue(compressed.NtfsCompressed);
        }

        [TestMethod]
        public void NtfsCompressedIsNotReportedForNotCompressedFolder()
        {
            //arrange
            var de = new DirectoryEstimator(new NoOpLogger());
            //act
            var result = de.Estimate(BaseDirectory);
            //assert
            Assert.AreEqual(2, result.Count());
            var compressed = result.Single(p => p.ShortName == NotCompressed);
            Assert.IsFalse(compressed.NtfsCompressed);
        }


        [TestMethod]
        public void NtfsCompressedFolderIsSkipped()
        {
            //arrange
            var de = new DirectoryEstimator(new NoOpLogger(),skipCompressedDirectories: true);
            //act
            //act
            var result = de.Estimate(BaseDirectory);
            //assert
            Assert.AreEqual(1, result.Count());
            Assert.IsFalse(result.Any(p => p.ShortName == Compressed));
        }

        private static void SetCompressedAttribute(string directory)
        {
            DirectoryInfo di = new DirectoryInfo(directory);
            string nameWithForwardSlashes = di.FullName.Replace('\\', '/');
            var objPath = $"Win32_Directory.Name='{nameWithForwardSlashes}'";
            using (ManagementObject dir = new ManagementObject(objPath))
            {
                ManagementBaseObject outParams = dir.InvokeMethod("Compress", null, null);
                uint ret = (uint)(outParams.Properties["ReturnValue"].Value);
            }
        }
    }
}