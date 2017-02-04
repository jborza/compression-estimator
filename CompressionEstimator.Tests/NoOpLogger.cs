using LibCompressionEstimator;

namespace CompressionEstimator.Tests
{
    class NoOpLogger : ILogger
    {
        public void Log(string message)
        {
        }        
    }
}
