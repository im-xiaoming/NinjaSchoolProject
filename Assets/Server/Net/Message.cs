using System;
using System.IO;
using System.Text;

namespace NsoServer.Net
{
    // Wire message: one command byte plus a payload using java.io.DataInput/OutputStream encoding (big-endian, modified UTF-8).
    public sealed class Message
    {
        public sbyte Command;
        public readonly JavaWriter Writer;
        public readonly JavaReader Reader;
        /// <summary>Initialize message state and its supplied dependencies.</summary>
        /// <param name="command">Command.</param>
        public Message(sbyte command)
        {
            Command = command;
            Writer = new JavaWriter();
        }

        /// <summary>The received payload (null for messages built to send); read by PlayerLog.</summary>
        public readonly byte[] Data;
        /// <summary>Initialize message state and its supplied dependencies.</summary>
        /// <param name="command">Command.</param>
        /// <param name="data">Data.</param>
        public Message(sbyte command, byte[] data)
        {
            Command = command;
            Data = data;
            Reader = new JavaReader(data);
        }

        /// <summary>Return data.</summary>
        public byte[] GetData() => Writer.ToArray();
    }

    public sealed class JavaWriter
    {
        // ms: Duration/timestamp in milliseconds.
        private readonly MemoryStream ms = new MemoryStream();
        /// <summary>Compute to array.</summary>
        public byte[] ToArray() => ms.ToArray();
        /// <summary>Write byte.</summary>
        /// <param name="v">Value being inspected or transformed.</param>
        public void WriteByte(int v) => ms.WriteByte((byte)v);
        /// <summary>Write signed byte.</summary>
        /// <param name="v">Value being inspected or transformed.</param>
        public void WriteSByte(sbyte v) => ms.WriteByte((byte)v);
        /// <summary>Write boolean.</summary>
        /// <param name="v">Value being inspected or transformed.</param>
        public void WriteBoolean(bool v) => ms.WriteByte((byte)(v ? 1 : 0));
        /// <summary>Write short.</summary>
        /// <param name="v">Value being inspected or transformed.</param>
        public void WriteShort(int v)
        {
            ms.WriteByte((byte)(v >> 8));
            ms.WriteByte((byte)v);
        }

        /// <summary>Write int.</summary>
        /// <param name="v">Value being inspected or transformed.</param>
        public void WriteInt(int v)
        {
            ms.WriteByte((byte)(v >> 24));
            ms.WriteByte((byte)(v >> 16));
            ms.WriteByte((byte)(v >> 8));
            ms.WriteByte((byte)v);
        }

        /// <summary>Write long.</summary>
        /// <param name="v">Value being inspected or transformed.</param>
        public void WriteLong(long v)
        {
            WriteInt((int)(v >> 32));
            WriteInt((int)v);
        }

        /// <summary>Write java writer.</summary>
        /// <param name="b">Buffer or intermediate output value.</param>
        public void Write(byte[] b) => ms.Write(b, 0, b.Length);
        /// <summary>Write utf.</summary>
        /// <param name="s">Source text or serialized input.</param>
        /// <remarks>Locals: buf: Buffer; c: Client character being inspected or controlled.</remarks>
        public void WriteUTF(string s)
        {
            var buf = new MemoryStream();
            foreach (char c in s)
            {
                if (c >= 0x0001 && c <= 0x007F)
                    buf.WriteByte((byte)c);
                else if (c <= 0x07FF)
                {
                    buf.WriteByte((byte)(0xC0 | (c >> 6)));
                    buf.WriteByte((byte)(0x80 | (c & 0x3F)));
                }
                else
                {
                    buf.WriteByte((byte)(0xE0 | (c >> 12)));
                    buf.WriteByte((byte)(0x80 | ((c >> 6) & 0x3F)));
                    buf.WriteByte((byte)(0x80 | (c & 0x3F)));
                }
            }

            if (buf.Length > 65535)
                throw new IOException("UTF too long");
            WriteShort((int)buf.Length);
            buf.WriteTo(ms);
        }
    }

    public sealed class JavaReader
    {
        private readonly byte[] data;
        // pos: Position.
        private int pos;
        /// <summary>Initialize java reader state and its supplied dependencies.</summary>
        /// <param name="data">Data.</param>
        public JavaReader(byte[] data)
        {
            this.data = data;
        }

        public int Remaining
        {
            get
            {
                return data.Length - pos;
            }
        }

        /// <summary>Read bytes.</summary>
        /// <param name="count">Count.</param>
        public byte[] ReadBytes(int count)
        {
            if (count < 0 || pos + count > data.Length)
                throw new EndOfStreamException();
            var result = new byte[count];
            Buffer.BlockCopy(data, pos, result, 0, count);
            pos += count;
            return result;
        }

        /// <summary>Compute next.</summary>
        private int Next()
        {
            if (pos >= data.Length)
                throw new EndOfStreamException();
            return data[pos++];
        }

        /// <summary>Read signed byte.</summary>
        public sbyte ReadSByte() => (sbyte)Next();
        /// <summary>Read byte.</summary>
        public byte ReadByte() => (byte)Next();
        /// <summary>Read boolean.</summary>
        public bool ReadBoolean() => Next() != 0;
        /// <summary>Read short.</summary>
        public short ReadShort() => (short)((Next() << 8) | Next());
        /// <summary>Read int.</summary>
        public int ReadInt() => (Next() << 24) | (Next() << 16) | (Next() << 8) | Next();
        /// <summary>Read long.</summary>
        public long ReadLong() => ((long)ReadInt() << 32) | (uint)ReadInt();
        /// <summary>Read utf.</summary>
        /// <remarks>Locals: len: Length; sb: Text output builder; a: Entry selected from the source collection; b: Entry selected from the source collection; c: Entry selected from the source collection.</remarks>
        public string ReadUTF()
        {
            int len = (Next() << 8) | Next();
            var sb = new StringBuilder(len);
            int end = pos + len;
            if (end > data.Length)
                throw new EndOfStreamException();
            while (pos < end)
            {
                int a = data[pos++];
                if (a < 0x80)
                    sb.Append((char)a);
                else if ((a >> 5) == 0x6)
                    sb.Append((char)(((a & 0x1F) << 6) | (data[pos++] & 0x3F)));
                else
                {
                    int b = data[pos++], c = data[pos++];
                    sb.Append((char)(((a & 0x0F) << 12) | ((b & 0x3F) << 6) | (c & 0x3F)));
                }
            }

            return sb.ToString();
        }
    }
}
