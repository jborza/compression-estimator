﻿using LibCompressionEstimator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressionEstimator.Tests
{
    [TestClass]
    public class DirectoryEstimatorTests
    {
        [TestMethod]
        public void EstimateTestDiscoversCorrectAmountOfDirectories()
        {
            //arrange
            var de = new DirectoryEstimator(new NoOpLogger());
            //act
            var result = de.Estimate(".");
            //assert
            Assert.AreEqual(4, result.Count());
        }

        [TestMethod]
        public void OriginalSizeTest()
        {
            // arrange
            var de = new DirectoryEstimator(new NoOpLogger());
            //act
            var result = de.Estimate(".");
            //assert
            var dirLarge = result.Single(p => p.ShortName == "TestDirLarge");
            Assert.AreEqual(2616384, dirLarge.OriginalSize);
        }

        [TestMethod]
        public void SavedBytesTest()
        {
            //arrange
            var de = new DirectoryEstimator(new NoOpLogger());
            //act
            var result = de.Estimate(".");
            //assert
            var dir = result.Single(p => p.ShortName == "TestDirCompressible");
            Assert.AreEqual(820650, dir.BytesSavedByCompression);
        }

        [TestMethod]
        public void EstimateTestForNonCompressible()
        {
            //arrange
            var de = new DirectoryEstimator(new NoOpLogger());
            //act
            var result = de.Estimate(".");
            //assert
            var dirLarge = result.Single(p => p.ShortName == "TestDirLarge");
            Assert.IsFalse(dirLarge.ShouldBeCompressed(75)); //only binary files
            Assert.AreEqual(2616384, dirLarge.EstimatedSize);
        }

        [TestMethod]
        public void EstimateTestForCompressible()
        {
            //arrange
            var de = new DirectoryEstimator(new NoOpLogger());
            //act
            var result = de.Estimate(".");
            //assert
            var dirLarge = result.Single(p => p.ShortName == "TestDirCompressible");
            Assert.IsTrue(dirLarge.ShouldBeCompressed(75)); //only binary files
            Assert.AreEqual(198180, dirLarge.EstimatedSize);
        }

    }
}
