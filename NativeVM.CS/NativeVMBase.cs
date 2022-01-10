using System;

namespace NativeVM.CS
{
    public abstract unsafe class NativeVMBase : IDisposable
    {
        protected void * _self;

        public NativeVMBase() => _self = Native.VMNew();

        ~NativeVMBase() => Dispose();

        public void Dispose()
        {
            if (_self == null) return;
            Native.VMDelete(_self);
            _self = null;
            GC.SuppressFinalize(this);
        }

        public int Last => Native.VMGetLast(_self);
    }
}
