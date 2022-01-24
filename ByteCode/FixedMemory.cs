using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ByteCode
{
    public sealed unsafe class FixedMemory : IDisposable
    {
        private byte * _self;

        public ref byte this[int i] => ref _self[i];

        public byte * GetAddressOfByte(int i) => _self + i;

        public FixedMemory(IReadOnlyList<byte> original)
        {
            _self = (byte*)Marshal.AllocHGlobal(original.Count);

            for (int i = 0; i < original.Count; ++i)
            {
                _self[i] = original[i];
            }
        }

        ~FixedMemory() => Dispose();

        public void Dispose()
        {
            if (_self == null) return;
            Marshal.FreeHGlobal((IntPtr)_self);
            _self = null;
            GC.SuppressFinalize(this);
        }

        public byte * Take()
        {
            var result = _self;
            _self = null;
            return result;
        }
    }
}
