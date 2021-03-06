﻿using LibCompressionEstimator.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LibCompressionEstimator
{
    public class DirectoryEstimator
    {
        /// <summary>
        /// Suggest that a directory should be compressed when its compression ratio is below or equal 
        /// to the threshold
        /// </summary>
        public double Threshold = 0.75;
        public int MAX_READ_SIZE = 1024 * 1024 * 50;
        public int BLOCK_SIZE = 1024 * 512;
        private readonly bool skipCompressedDirectories;
        private readonly bool parallelRun;
        private readonly ILogger logger;

        public DirectoryEstimator(ILogger logger, bool skipCompressedDirectories = false, bool parallelRun = false)
        {
            this.logger = logger;
            this.skipCompressedDirectories = skipCompressedDirectories;
            this.parallelRun = parallelRun;
        }

        public IEnumerable<EstimationResult> Estimate(string directory)
        {
            var di = new DirectoryInfo(directory);
            var enumerator = GetDirectoryEnumerator();
            var directories = enumerator.EnumerateDirectories(di).ToArray();
            var runner = GetEstimatorRunner();
            return runner.Estimate(directories, EstimateDirectory);
        }

        private IEstimatorRunner GetEstimatorRunner()
        {
            if (parallelRun)
                return new ParallelEstimatorRunner();
            else
                return new SimpleEstimatorRunner();
        }

        private IDirectoryEnumerator GetDirectoryEnumerator()
        {
            if (skipCompressedDirectories)
                return new SkipCompressedDirectoryEnumerator();
            else
                return new BasicDirectoryEnumerator();
        }

        private EstimationResult EstimateDirectory(DirectoryInfo arg)
        {
            try
            {
                logger.Log("Starting: " + arg.Name);
                DirectoryStream ds = new DirectoryStream(arg.FullName);
                StreamEstimator se = new StreamEstimator();
                var packedSize = se.EstimatePackedSize(ds, MAX_READ_SIZE, BLOCK_SIZE);
                var result = new EstimationResult()
                {
                    Directory = arg.FullName,
                    ShortName = arg.Name,
                    OriginalSize = ds.Length,
                    EstimatedSize = packedSize,
                    NtfsCompressed = arg.IsCompressed()
                };
                logger.Log("Finished: " + arg.Name + " - ratio: " + result.CompressionRatio);
                return result;
            }
            catch (Exception e)
            {
                return new EstimationResult();
            }
        }
    }
}
