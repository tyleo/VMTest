using ByteCode;
using System;
using System.Runtime.CompilerServices;

namespace NativeVM.CS
{
    public sealed unsafe class Evvm : IDisposable
    {
        private void * _self;

        public Evvm() => _self = Native.EvvmNew();

        ~Evvm() => Dispose();

        public void Dispose()
        {
            if (_self == null) return;
            Native.EvvmDelete(_self);
            _self = null;
            GC.SuppressFinalize(this);
        }

        public int Get(int index) => Native.EvvmGet(_self, index);

        public static Code Preprocess(Code byteCode) =>
            EvvmPreprocessor<IntPtr>.Preprocess(byteCode.Bytes, (IntPtr*)Native.EvvmLabelAddresses, false);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Run(byte* byteCode) => Native.EvvmRun(_self, byteCode);
    }
}
