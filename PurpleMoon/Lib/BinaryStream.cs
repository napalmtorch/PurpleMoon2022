using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurpleMoon.Lib
{
    public class BinaryStream
    {
        public byte[] Data { get; private set; }
        public int    Size { get; private set; }
        public int    Position { get; private set; }

        public BinaryStream(byte[] data, int size)
        {
            this.Data     = data;
            this.Size     = size;
            this.Position = 0;
        }

        public BinaryStream(string fname)
        {
            this.Data     = FileSystem.ReadBytes(fname);
            this.Size     = Data.Length;
            this.Position = 0;
        }

        public void Seek(int pos)
        {
            if (pos < 0 || pos >= Size) { return; }
            Position = pos;
        }

        public char ReadChar() { return (char)ReadByte(); }

        public byte ReadByte()
        {
            if (Position >= Size) { return 0; }
            byte v = Data[Position];
            Position++;
            return v;
        }

        public short ReadShort() { return (short)ReadUShort(); }

        public ushort ReadUShort()
        {
            byte[] vals = { ReadByte(), ReadByte() };
            return (ushort)((vals[1] << 8) | vals[0]);
        }

        public int ReadInt() { return (int)ReadUInt(); }

        public uint ReadUInt()
        {
            byte[] vals = { ReadByte(), ReadByte(), ReadByte(), ReadByte() };
            return (uint)((vals[3] << 24) | (vals[2] << 16) | (vals[1] << 8) | vals[0]);
        }
    }
}
