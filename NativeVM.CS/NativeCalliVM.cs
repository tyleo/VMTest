using ByteCode;
using System.Runtime.CompilerServices;

namespace NativeVM.CS
{
    public sealed unsafe class NativeCalliVM : NativeVMBase
    {
        public static Code Preprocess(Code byteCode) => byteCode;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Run(byte* byteCode) => Native.VMCalliRun(_self, byteCode);
    }
}
