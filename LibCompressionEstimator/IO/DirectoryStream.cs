using System;
using System.IO;
using System.Linq;

namespace LibCompressionEstimator.IO
{
    /// <summary>
    /// Provides the equivalent of FileStream over a directory
    /// </summary>
    public class DirectoryStream : Stream
    {
        private DirectoryFilesManager manager;
        private long length;
        private long position;

        public DirectoryStream(string directoryName)
        {
            var di = new DirectoryInfo(directoryName);
            var files = di.GetFiles("*", SearchOption.AllDirectories);
            manager = new DirectoryFilesManager(files);
            length = manager.Length;
        }

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override long Length => length;

        public override int Read(byte[] buffer, int offset, int count)
        {
            var files = manager.GetFilesToRead(Position, count);
            int totalRead = 0;
            int currentOffset = 0;
            foreach (var f in files)
            {
                using (var fs = f.File.OpenRead())
                {
                    if (f.StartOffset != 0)
                        fs.Seek(f.StartOffset, SeekOrigin.Begin);
                    int read = fs.Read(buffer, currentOffset, f.BytesToRead);
                    currentOffset += read;
                    totalRead += read;
                }
            }
            Position += totalRead;
            return totalRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    Position = offset;
                    break;
                case SeekOrigin.Current:
                    Position += offset;
                    break;
                case SeekOrigin.End:
                    throw new NotImplementedException();
            }
            return Position;
        }

        public override int ReadByte()
        {
            var readInfos = manager.GetFilesToRead(Position, 1);
            var readInfo = readInfos.First();
            using (FileStream fs = readInfo.File.OpenRead())
            {
                fs.Position = readInfo.StartOffset;
                Position++;
                return fs.ReadByte();
            }
        }

        public override long Position
        {
            get { return position; }
            set { position = value; }
        }

        public override bool CanWrite => false;

        //////////////////////////
        //unused - as we can't write
        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
