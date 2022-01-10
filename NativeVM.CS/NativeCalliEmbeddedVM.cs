using ByteCode;
using System;
using System.Runtime.CompilerServices;

namespace NativeVM.CS
{
    public sealed unsafe class NativeCalliEmbeddedVM : NativeVMBase
    {
        public static Code Preprocess(Code byteCode) => Preprocessor<IntPtr>.Preprocess(byteCode.Bytes, (IntPtr*)Native.EmbeddedFunctionAddresses);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Run(byte* byteCode) => Native.VMCalliEmbeddedRun(_self, byteCode);
    }
}
