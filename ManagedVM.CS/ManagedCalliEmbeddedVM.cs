using ByteCode;
using System;

namespace ManagedVM.CS
{
    public sealed unsafe class ManagedCalliEmbeddedVM
    {
        private static readonly IntPtr[] _handlers;

        private byte* _byteCode;
        private readonly int[] _stack = new int[1000];
        private int _stackPointer;
        private int _programCounter;

        public int Last => _stack[_stackPointer];

        static ManagedCalliEmbeddedVM()
        {
            _handlers = new IntPtr[(int)Op.Size];
            _handlers[(int)Op.NoOp] = (IntPtr)(delegate*<ManagedCalliEmbeddedVM, void>)&NoOp;
            _handlers[(int)Op.Push] = (IntPtr)(delegate*<ManagedCalliEmbeddedVM, void>)&Push;
            _handlers[(int)Op.Pop] = (IntPtr)(delegate*<ManagedCalliEmbeddedVM, void>)&Pop;
            _handlers[(int)Op.Add] = (IntPtr)(delegate*<ManagedCalliEmbeddedVM, void>)&Add;
            _handlers[(int)Op.Subtract] = (IntPtr)(delegate*<ManagedCalliEmbeddedVM, void>)&Subtract;
            _handlers[(int)Op.Multiply] = (IntPtr)(delegate*<ManagedCalliEmbeddedVM, void>)&Multiply;
            _handlers[(int)Op.Divide] = (IntPtr)(delegate*<ManagedCalliEmbeddedVM, void>)&Divide;
            _handlers[(int)Op.BranchIfLess] = (IntPtr)(delegate*<ManagedCalliEmbeddedVM, void>)&ManagedCalliEmbeddedVM.BranchIfLess;
            _handlers[(int)Op.BranchIfGreaterOrEqual] = (IntPtr)(delegate*<ManagedCalliEmbeddedVM, void>)&ManagedCalliEmbeddedVM.BranchIfGreaterOrEqual;
            _handlers[(int)Op.Duplicate] = (IntPtr)(delegate*<ManagedCalliEmbeddedVM, void>)&ManagedCalliEmbeddedVM.Duplicate;
            _handlers[(int)Op.Load] = (IntPtr)(delegate*<ManagedCalliEmbeddedVM, void>)&ManagedCalliEmbeddedVM.Load;
            _handlers[(int)Op.Store] = (IntPtr)(delegate*<ManagedCalliEmbeddedVM, void>)&ManagedCalliEmbeddedVM.Store;
            _handlers[(int)Op.End] = (IntPtr)(delegate*<ManagedCalliEmbeddedVM, void>)&End;
        }

        public static Code Preprocess(Code byteCode)
        {
            fixed (IntPtr* pHandlers = _handlers)
            {
                return Preprocessor<IntPtr>.Preprocess(byteCode.Bytes, pHandlers);
            }
        }

        public void Run(byte* byteCode)
        {
            _byteCode = byteCode;
            _stackPointer = -1;
            _programCounter = 0;

            (*((delegate*<ManagedCalliEmbeddedVM, void>*)(_byteCode + _programCounter)))(this);
        }

        private static void NoOp(ManagedCalliEmbeddedVM self)
        {
            self._programCounter += sizeof(delegate*<ManagedCalliEmbeddedVM, void>);
            (*((delegate*<ManagedCalliEmbeddedVM, void>*)(self._byteCode + self._programCounter)))(self);
        }

        private static void Push(ManagedCalliEmbeddedVM self)
        {
            var value = *((int*)(self._byteCode + self._programCounter + sizeof(delegate*<ManagedCalliEmbeddedVM, void>)));
            ++self._stackPointer;
            self._stack[self._stackPointer] = value;
            self._programCounter += (sizeof(delegate*<ManagedCalliEmbeddedVM, void>) + sizeof(int));
            (*((delegate*<ManagedCalliEmbeddedVM, void>*)(self._byteCode + self._programCounter)))(self);
        }

        private static void Pop(ManagedCalliEmbeddedVM self)
        {
            --self._stackPointer;
            self._programCounter += sizeof(delegate*<ManagedCalliEmbeddedVM, void>);
            (*((delegate*<ManagedCalliEmbeddedVM, void>*)(self._byteCode + self._programCounter)))(self);
        }

        private static void Add(ManagedCalliEmbeddedVM self)
        {
            self._stack[self._stackPointer - 1] = self._stack[self._stackPointer - 1] + self._stack[self._stackPointer];
            --self._stackPointer;
            self._programCounter += sizeof(delegate*<ManagedCalliEmbeddedVM, void>);
            (*((delegate*<ManagedCalliEmbeddedVM, void>*)(self._byteCode + self._programCounter)))(self);
        }

        private static void Subtract(ManagedCalliEmbeddedVM self)
        {
            self._stack[self._stackPointer - 1] = self._stack[self._stackPointer - 1] - self._stack[self._stackPointer];
            --self._stackPointer;
            self._programCounter += sizeof(delegate*<ManagedCalliEmbeddedVM, void>);
            (*((delegate*<ManagedCalliEmbeddedVM, void>*)(self._byteCode + self._programCounter)))(self);
        }

        private static void Multiply(ManagedCalliEmbeddedVM self)
        {
            self._stack[self._stackPointer - 1] = self._stack[self._stackPointer - 1] * self._stack[self._stackPointer];
            --self._stackPointer;
            self._programCounter += sizeof(delegate*<ManagedCalliEmbeddedVM, void>);
            (*((delegate*<ManagedCalliEmbeddedVM, void>*)(self._byteCode + self._programCounter)))(self);
        }

        private static void Divide(ManagedCalliEmbeddedVM self)
        {
            self._stack[self._stackPointer - 1] = self._stack[self._stackPointer - 1] / self._stack[self._stackPointer];
            --self._stackPointer;
            self._programCounter += sizeof(delegate*<ManagedCalliEmbeddedVM, void>);
            (*((delegate*<ManagedCalliEmbeddedVM, void>*)(self._byteCode + self._programCounter)))(self);
        }

        private static void BranchIfLess(ManagedCalliEmbeddedVM self)
        {
            var greaterOrEqual = self._stack[self._stackPointer - 1] < self._stack[self._stackPointer];
            self._stackPointer -= 2;
            if (greaterOrEqual)
            {
                self._programCounter = *((int*)(self._byteCode + self._programCounter + sizeof(delegate*<ManagedCalliEmbeddedVM, void>)));
            }
            else
            {
                self._programCounter += (sizeof(delegate*<ManagedCalliEmbeddedVM, void>) + sizeof(int));
            }
            (*((delegate*<ManagedCalliEmbeddedVM, void>*)(self._byteCode + self._programCounter)))(self);
        }

        private static void BranchIfGreaterOrEqual(ManagedCalliEmbeddedVM self)
        {
            var greaterOrEqual = self._stack[self._stackPointer - 1] >= self._stack[self._stackPointer];
            self._stackPointer -= 2;
            if (greaterOrEqual)
            {
                self._programCounter = *((int*)(self._byteCode + self._programCounter + sizeof(delegate*<ManagedCalliEmbeddedVM, void>)));
            }
            else
            {
                self._programCounter += (sizeof(delegate*<ManagedCalliEmbeddedVM, void>) + sizeof(int));
            }
            (*((delegate*<ManagedCalliEmbeddedVM, void>*)(self._byteCode + self._programCounter)))(self);
        }

        private static void Duplicate(ManagedCalliEmbeddedVM self)
        {
            ++self._stackPointer;
            self._stack[self._stackPointer] = self._stack[self._stackPointer - 1];
            self._programCounter += sizeof(delegate*<ManagedCalliEmbeddedVM, void>);
            (*((delegate*<ManagedCalliEmbeddedVM, void>*)(self._byteCode + self._programCounter)))(self);
        }

        private static void Load(ManagedCalliEmbeddedVM self)
        {
            var loadFrom = *((int*)(self._byteCode + self._programCounter + sizeof(delegate*<ManagedCalliEmbeddedVM, void>)));
            ++self._stackPointer;
            self._stack[self._stackPointer] = self._stack[self._stackPointer - loadFrom];
            self._programCounter += (sizeof(delegate*<ManagedCalliEmbeddedVM, void>) + sizeof(int));
            (*((delegate*<ManagedCalliEmbeddedVM, void>*)(self._byteCode + self._programCounter)))(self);
        }

        private static void Store(ManagedCalliEmbeddedVM self)
        {
            var storeTo = *((int*)(self._byteCode + self._programCounter + sizeof(delegate*<ManagedCalliEmbeddedVM, void>)));
            self._stack[self._stackPointer - storeTo] = self._stack[self._stackPointer];
            --self._stackPointer;
            self._programCounter += (sizeof(delegate*<ManagedCalliEmbeddedVM, void>) + sizeof(int));
            (*((delegate*<ManagedCalliEmbeddedVM, void>*)(self._byteCode + self._programCounter)))(self);
        }

        private static void End(ManagedCalliEmbeddedVM self)
        {
            self._programCounter += sizeof(delegate*<ManagedCalliEmbeddedVM, void>);
        }
    }
}
