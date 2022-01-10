using ByteCode;
using System;
using System.Runtime.CompilerServices;

namespace NativeVM.CS
{

    public sealed unsafe class NativeAddressOfLabelEmbeddedAlignedVM : NativeVMBase
    {
        public static Code Preprocess(Code byteCode) => Preprocessor<IntPtr>.Preprocess(byteCode.Bytes, (IntPtr*)Native.EmbeddedAlignedLabelAddresses, true);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Run(byte* byteCode) => Native.VMAddressOfLabelEmbeddedAlignedRun(_self, byteCode);
    }
}
