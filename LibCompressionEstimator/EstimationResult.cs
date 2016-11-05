﻿using System;

namespace LibCompressionEstimator
{
    public class EstimationResult
    {
        public string Directory;
        public string ShortName;
        public long CurrentSize;
        public long EstimatedSize;
        public double CompressionRatio => CurrentSize == 0 ? Double.NaN : EstimatedSize / (double)CurrentSize * 100.0;
        public bool ShouldBeCompressed(double threshold)
        {
            return CompressionRatio <= threshold;
        }

        private float ByteToMB(long b)
        {
            return b / 1048576f;
        }

        public override string ToString()
        {
            return string.Format("{0} Ratio:{1:0.0}% {2:0}MB->{3:0}MB",
                ShortName,
                CompressionRatio,
                ByteToMB(CurrentSize),
                ByteToMB(EstimatedSize));
        }
    }
}