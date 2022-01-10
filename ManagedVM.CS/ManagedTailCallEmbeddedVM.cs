using ByteCode;
using System.Runtime.CompilerServices;
using System;

namespace ManagedVM.CS
{
    public unsafe class ManagedTailCallEmbeddedVM
    {
        private static readonly IntPtr[] _handlers;

        private readonly TailCallEmbedded.VM _impl = new();

        static ManagedTailCallEmbeddedVM()
        {
            _handlers = new IntPtr[(int)Op.Size];
            _handlers[(int)Op.NoOp] = (IntPtr)(delegate*<TailCallEmbedded.VM, void>)&TailCallEmbedded.VM.NoOp;
            _handlers[(int)Op.Push] = (IntPtr)(delegate*<TailCallEmbedded.VM, void>)&TailCallEmbedded.VM.Push;
            _handlers[(int)Op.Pop] = (IntPtr)(delegate*<TailCallEmbedded.VM, void>)&TailCallEmbedded.VM.Pop;
            _handlers[(int)Op.Add] = (IntPtr)(delegate*<TailCallEmbedded.VM, void>)&TailCallEmbedded.VM.Add;
            _handlers[(int)Op.Subtract] = (IntPtr)(delegate*<TailCallEmbedded.VM, void>)&TailCallEmbedded.VM.Subtract;
            _handlers[(int)Op.Multiply] = (IntPtr)(delegate*<TailCallEmbedded.VM, void>)&TailCallEmbedded.VM.Multiply;
            _handlers[(int)Op.Divide] = (IntPtr)(delegate*<TailCallEmbedded.VM, void>)&TailCallEmbedded.VM.Divide;
            _handlers[(int)Op.BranchIfLess] = (IntPtr)(delegate*<TailCallEmbedded.VM, void>)&TailCallEmbedded.VM.BranchIfLess;
            _handlers[(int)Op.BranchIfGreaterOrEqual] = (IntPtr)(delegate*<TailCallEmbedded.VM, void>)&TailCallEmbedded.VM.BranchIfGreaterOrEqual;
            _handlers[(int)Op.Duplicate] = (IntPtr)(delegate*<TailCallEmbedded.VM, void>)&TailCallEmbedded.VM.Duplicate;
            _handlers[(int)Op.Load] = (IntPtr)(delegate*<TailCallEmbedded.VM, void>)&TailCallEmbedded.VM.Load;
            _handlers[(int)Op.Store] = (IntPtr)(delegate*<TailCallEmbedded.VM, void>)&TailCallEmbedded.VM.Store;
            _handlers[(int)Op.End] = (IntPtr)(delegate*<TailCallEmbedded.VM, void>)&TailCallEmbedded.VM.End;
        }

        public int Last => _impl.Last;

        public static Code Preprocess(Code byteCode)
        {
            fixed (IntPtr * pHandlers = _handlers)
            {
                return Preprocessor<IntPtr>.Preprocess(byteCode.Bytes, pHandlers);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Run(byte * byteCode) => _impl.Run(byteCode);
    }
}
