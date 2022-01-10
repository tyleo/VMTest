using ByteCode;
using System;
using System.Runtime.CompilerServices;

namespace NativeVM.CS
{

    public sealed unsafe class NativeAddressOfLabelEmbeddedVM : NativeVMBase
    {
        public static Code Preprocess(Code byteCode) => Preprocessor<IntPtr>.Preprocess(byteCode.Bytes, (IntPtr*)Native.EmbeddedLabelAddresses, false);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Run(byte* byteCode) => Native.VMAddressOfLabelEmbeddedRun(_self, byteCode);
    }
}
