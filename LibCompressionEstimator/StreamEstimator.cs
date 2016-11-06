using System.IO;
using System.IO.Compression;

namespace LibCompressionEstimator
{
    internal class StreamEstimator
    {
        /// <summary>
        /// Estimates how large the directory would be after compression,  
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="sampleStreamSize">A maximum length of the directory sample stream</param>
        /// <param name="blockSize">Contiguous block size to read from the source stream</param>
        /// <returns></returns>
        public long EstimatePackedSize(Stream inputStream, int sampleStreamSize, int blockSize)
        {
            var config = PrepareConfiguration(inputStream, sampleStreamSize, blockSize);
            double estimatedCompressionRatio = EstimateCompressionRatio(inputStream, config);
            return (long)(estimatedCompressionRatio * config.InputLength);
        }

        private static double EstimateCompressionRatio(Stream inputStream, StreamEstimatorConfig config)
        {
            long totalReadBytes = 0;
            long compressedBytes;
            byte[] buffer = new byte[config.BlockSize];
            using (MemoryStream ms = new MemoryStream())
            {
                using (var compressedStream = CreateCompressedStream(ms))
                {
                    for (int i = 0; i < config.Blocks; i++)
                    {
                        var read = inputStream.Read(buffer, 0, config.BlockSize);
                        compressedStream.Write(buffer, 0, read);
                        totalReadBytes += read;
                        inputStream.Seek(config.SkipOffsetBetweenBlocks, SeekOrigin.Current);
                    }
                }
                compressedBytes = ms.Length;
            }
            double estimatedCompressionRatio = (double)compressedBytes / totalReadBytes;
            return estimatedCompressionRatio;
        }

        private static Stream CreateCompressedStream(Stream stream)
        {
            return new DeflateStream(stream, CompressionLevel.Fastest, true);
        }

        private StreamEstimatorConfig PrepareConfiguration(Stream inputStream, int maxReadSize, int blockSize)
        {
            long inputLength = inputStream.Length;
            long skippedSize = inputLength - maxReadSize;
            int blockCount = maxReadSize / blockSize;
            return new StreamEstimatorConfig()
            {
                InputLength = inputLength,
                Blocks = blockCount,
                BlockSize = blockSize,
                SkipOffsetBetweenBlocks = blockCount == 1 ? 0 : skippedSize / (blockCount - 1)
            };
        }

        private class StreamEstimatorConfig
        {
            public long InputLength;
            public int BlockSize;
            public int Blocks;
            public long SkipOffsetBetweenBlocks;
        }

    }
}
