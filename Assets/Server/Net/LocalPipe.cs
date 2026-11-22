using System;
using System.IO;
using System.Threading;

namespace NsoServer.Net
{
    // In-memory connection between the client and the embedded server: no socket, no port. Each side gets a Stream;
    // bytes written on one side are read, in order, on the other. Reading blocks until bytes arrive or the pipe closes.
    public static class LocalPipe
    {
        /// <summary>Create local pipe.</summary>
        /// <param name="clientSide">Client side.</param>
        /// <param name="serverSide">Server side.</param>
        public static void Create(out Stream clientSide, out Stream serverSide)
        {
            var toServer = new BytePipe();
            var toClient = new BytePipe();
            clientSide = new PipeStream(toClient, toServer);
            serverSide = new PipeStream(toServer, toClient);
        }
    }

    // One direction: a growing ring buffer guarded by its own lock.
    internal sealed class BytePipe
    {
        // buf: Buffer.
        private byte[] buf = new byte[64 * 1024];
        private int head, count;
        private bool closed;
        /// <summary>Write byte pipe.</summary>
        /// <param name="src">Source.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="n">Number of entries or iterations.</param>
        /// <remarks>Locals: i: Loop/collection index.</remarks>
        public void Write(byte[] src, int offset, int n)
        {
            lock (this)
            {
                if (closed)
                    throw new IOException("Connection closed");
                Ensure(count + n);
                for (int i = 0; i < n; i++)
                    buf[(head + count + i) % buf.Length] = src[offset + i];
                count += n;
                Monitor.PulseAll(this);
            }
        }

        // Returns 0 only when the pipe is closed and empty (end of stream).
        /// <summary>Read byte pipe.</summary>
        /// <param name="dst">Destination.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="n">Number of entries or iterations.</param>
        /// <remarks>Locals: k: Key or nested iteration index; i: Loop/collection index.</remarks>
        public int Read(byte[] dst, int offset, int n)
        {
            lock (this)
            {
                while (count == 0 && !closed)
                    Monitor.Wait(this);
                if (count == 0)
                    return 0;
                int k = System.Math.Min(n, count);
                for (int i = 0; i < k; i++)
                    dst[offset + i] = buf[(head + i) % buf.Length];
                head = (head + k) % buf.Length;
                count -= k;
                return k;
            }
        }

        /// <summary>Close byte pipe.</summary>
        public void Close()
        {
            lock (this)
            {
                closed = true;
                Monitor.PulseAll(this);
            }
        }

        /// <summary>Perform ensure.</summary>
        /// <param name="needed">Needed.</param>
        /// <remarks>Locals: i: Loop/collection index.</remarks>
        private void Ensure(int needed)
        {
            if (needed <= buf.Length)
                return;
            var bigger = new byte[System.Math.Max(buf.Length * 2, needed)];
            for (int i = 0; i < count; i++)
                bigger[i] = buf[(head + i) % buf.Length];
            buf = bigger;
            head = 0;
        }
    }

    // One end of the connection: reads from one pipe, writes to the other. Closing it closes both directions.
    internal sealed class PipeStream : Stream
    {
        private readonly BytePipe input, output;
        /// <summary>Initialize pipe stream state and its supplied dependencies.</summary>
        /// <param name="input">Input.</param>
        /// <param name="output">Output.</param>
        public PipeStream(BytePipe input, BytePipe output)
        {
            this.input = input;
            this.output = output;
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override long Length
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public override long Position
        {
            get
            {
                throw new NotSupportedException();
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>Read pipe stream.</summary>
        /// <param name="buffer">Buffer.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="count">Count.</param>
        public override int Read(byte[] buffer, int offset, int count)
        {
            return count == 0 ? 0 : input.Read(buffer, offset, count);
        }

        /// <summary>Write pipe stream.</summary>
        /// <param name="buffer">Buffer.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="count">Count.</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            output.Write(buffer, offset, count);
        }

        /// <summary>Retain the flush compatibility hook without performing work.</summary>
        public override void Flush()
        {
        }

        /// <summary>Compute seek.</summary>
        /// <param name="offset">Retained for call compatibility; not read by this implementation.</param>
        /// <param name="origin">Retained for call compatibility; not read by this implementation.</param>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        /// <summary>Set length.</summary>
        /// <param name="value">Retained for call compatibility; not read by this implementation.</param>
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        /// <summary>Release resources owned by this instance.</summary>
        /// <param name="disposing">Disposing.</param>
        protected override void Dispose(bool disposing)
        {
            input.Close();
            output.Close();
            base.Dispose(disposing);
        }
    }
}
