using System;
using System.Runtime.InteropServices;

namespace ByteCode
{
    public sealed unsafe class Code : IDisposable
    {
        private readonly IntPtr _self;
        private byte * _selfAligned;

        public Code(byte[] original, int opAlignment = 1)
        {
            var maxOffset = opAlignment - 1;
            _self = Marshal.AllocHGlobal(original.Length + maxOffset);

            var address = _self.ToInt64();
            var offset = opAlignment - (address % opAlignment);
            if (offset == opAlignment) offset = 0;

            _selfAligned = ((byte *)_self) + offset;

            for (var i = 0; i < original.Length; ++i)
            {
                _selfAligned[i] = original[i];
            }
        }

        ~Code() => Dispose();

        public void Dispose()
        {
            if (_selfAligned == null) return;
            Marshal.FreeHGlobal(_self);
            _selfAligned = null;
            GC.SuppressFinalize(this);
        }

        public byte * Bytes => _selfAligned;
    }
}
