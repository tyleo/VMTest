using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace NativeVM.CS
{
    /// <summary>
    /// Wrap calls to native functions. NativeLibrary.TryLoad() is much faster than P/Invoke which
    /// is why the native methods are wrapped this way.
    /// </summary>
    public static unsafe class Native
    {
        private static readonly delegate*<void> _evvmStaticInit;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EvvmStaticInit() => _evvmStaticInit();

        private static readonly delegate*<void**> _evvmGetLabelAddresses;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void** EvvmGetLabelAddresses() => _evvmGetLabelAddresses();

        private static readonly delegate*<void*> _evvmNew;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* EvvmNew() => _evvmNew();

        private static readonly delegate*<void*, void> _evvmDelete;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EvvmDelete(void* self) => _evvmDelete(self);

        private static readonly delegate*<void*, int, int> _evvmGet;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int EvvmGet(void* pSelf, int index) => _evvmGet(pSelf, index);

        private static readonly delegate*<void*, byte*, void> _evvmRun;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EvvmRun(void* pSelf, byte* byteCode) => _evvmRun(pSelf, byteCode);

        public static readonly void** EvvmLabelAddresses;




        private static readonly delegate*<void> _vmStaticInit;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void VMStaticInit() => _vmStaticInit();

        private static readonly delegate*<void**> _vmGetFunctionAddresses;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void** VMGetFunctionAddresses() => _vmGetFunctionAddresses();

        private static readonly delegate*<void**> _vmGetEmbeddedFunctionAddresses;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void** VMGetEmbeddedFunctionAddresses() => _vmGetEmbeddedFunctionAddresses();

        private static readonly delegate*<void**> _vmGetLabelAddresses;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void** VMGetLabelAddresses() => _vmGetLabelAddresses();

        private static readonly delegate*<void**> _vmGetEmbeddedLabelAddresses;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void** VMGetEmbeddedLabelAddresses() => _vmGetEmbeddedLabelAddresses();

        private static readonly delegate*<void**> _vmGetEmbeddedAlignedLabelAddresses;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void** VMGetEmbeddedAlignedLabelAddresses() => _vmGetEmbeddedAlignedLabelAddresses();

        private static readonly delegate*<void*> _vmNew;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* VMNew() => _vmNew();

        private static readonly delegate*<void*, void> _vmDelete;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void VMDelete(void* self) => _vmDelete(self);

        private static readonly delegate*<void*, int> _vmGetLast;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int VMGetLast(void* self) => _vmGetLast(self);

        private static readonly delegate*<void*, byte*, void> _vmCalliRun;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void VMCalliRun(void* pSelf, byte* byteCode) => _vmCalliRun(pSelf, byteCode);

        private static readonly delegate*<void*, byte*, void> _vmCalliEmbeddedRun;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void VMCalliEmbeddedRun(void* pSelf, byte* byteCode) => _vmCalliEmbeddedRun(pSelf, byteCode);

        private static readonly delegate*<void*, byte*, void> _vmAddressOfLabelRun;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void VMAddressOfLabelRun(void* pSelf, byte* byteCode) => _vmAddressOfLabelRun(pSelf, byteCode);

        private static readonly delegate*<void*, byte*, void> _vmAddressOfLabelEmbeddedRun;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void VMAddressOfLabelEmbeddedRun(void* pSelf, byte* byteCode) => _vmAddressOfLabelEmbeddedRun(pSelf, byteCode);

        private static readonly delegate*<void*, byte*, void> _vmAddressOfLabelEmbeddedAlignedRun;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void VMAddressOfLabelEmbeddedAlignedRun(void* pSelf, byte* byteCode) => _vmAddressOfLabelEmbeddedAlignedRun(pSelf, byteCode);

        private static readonly delegate*<void*, byte*, void> _vmSwitchDispatchRun;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void VMSwitchDispatchRun(void* pSelf, byte* byteCode) => _vmSwitchDispatchRun(pSelf, byteCode);

        public static readonly void** FunctionAddresses;
        public static readonly void** EmbeddedFunctionAddresses;
        public static readonly void** LabelAddresses;
        public static readonly void** EmbeddedLabelAddresses;
        public static readonly void** EmbeddedAlignedLabelAddresses;

        private static IntPtr GetExport(IntPtr dll, string name)
        {
            if (!NativeLibrary.TryGetExport(dll, name, out var result))
            {
                throw new Exception($"{name} could not be found in NativeVM.CPP.dll.");
            }
            return result;
        }

        static Native()
        {
            if (!NativeLibrary.TryLoad("NativeVM.CPP.dll", out var dll)) throw new Exception("NativeVM.CPP.dll not found.");

            _evvmStaticInit = (delegate*<void>)GetExport(dll, "EvvmStaticInit");
            _evvmGetLabelAddresses = (delegate*<void**>)GetExport(dll, "EvvmGetLabelAddresses");
            _evvmNew = (delegate*<void*>)GetExport(dll, "EvvmNew");
            _evvmDelete = (delegate*<void*, void>)GetExport(dll, "EvvmDelete");
            _evvmGet = (delegate*<void*, int, int>)GetExport(dll, "EvvmGet");
            _evvmRun = (delegate*<void*, byte*, void>)GetExport(dll, "EvvmRun");

            EvvmStaticInit();
            EvvmLabelAddresses = EvvmGetLabelAddresses();

            _vmStaticInit = (delegate*<void>)GetExport(dll, "VMStaticInit");
            _vmGetFunctionAddresses = (delegate*<void**>)GetExport(dll, "VMGetFunctionAddresses");
            _vmGetEmbeddedFunctionAddresses = (delegate*<void**>)GetExport(dll, "VMGetEmbeddedFunctionAddresses");
            _vmGetLabelAddresses = (delegate*<void**>)GetExport(dll, "VMGetLabelAddresses");
            _vmGetEmbeddedLabelAddresses = (delegate*<void**>)GetExport(dll, "VMGetEmbeddedLabelAddresses");
            _vmGetEmbeddedAlignedLabelAddresses = (delegate*<void**>)GetExport(dll, "VMGetEmbeddedAlignedLabelAddresses");
            _vmNew = (delegate*<void*>)GetExport(dll, "VMNew");
            _vmDelete = (delegate*<void*, void>)GetExport(dll, "VMDelete");
            _vmGetLast = (delegate*<void*, int>)GetExport(dll, "VMGetLast");
            _vmCalliRun = (delegate*<void*, byte*, void>)GetExport(dll, "VMCalliRun");
            _vmCalliEmbeddedRun = (delegate*<void*, byte*, void>)GetExport(dll, "VMCalliEmbeddedRun");
            _vmAddressOfLabelRun = (delegate*<void*, byte*, void>)GetExport(dll, "VMAddressOfLabelRun");
            _vmAddressOfLabelEmbeddedRun = (delegate*<void*, byte*, void>)GetExport(dll, "VMAddressOfLabelEmbeddedRun");
            _vmAddressOfLabelEmbeddedAlignedRun = (delegate*<void*, byte*, void>)GetExport(dll, "VMAddressOfLabelEmbeddedAlignedRun");
            _vmSwitchDispatchRun = (delegate*<void*, byte*, void>)GetExport(dll, "VMSwitchDispatchRun");

            VMStaticInit();

            FunctionAddresses = VMGetFunctionAddresses();
            EmbeddedFunctionAddresses = VMGetEmbeddedFunctionAddresses();
            LabelAddresses = VMGetLabelAddresses();
            EmbeddedLabelAddresses = VMGetEmbeddedLabelAddresses();
            EmbeddedAlignedLabelAddresses = VMGetEmbeddedAlignedLabelAddresses();
        }
    }
}
