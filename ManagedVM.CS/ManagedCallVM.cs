using ByteCode;

namespace ManagedVM.CS
{
    public sealed unsafe class ManagedCallVM
    {
        private delegate void Handler(ManagedCallVM self);
        private static readonly Handler[] _handlers;

        private byte * _byteCode;
        private readonly int[] _stack = new int[1024];
        private int _stackPointer;
        private int _programCounter;

        public int Last => _stack[_stackPointer];

        static ManagedCallVM()
        {
            _handlers = new Handler[(int)Op.Size];
            _handlers[(int)Op.NoOp] = NoOp;
            _handlers[(int)Op.Push] = Push;
            _handlers[(int)Op.Pop] = Pop;
            _handlers[(int)Op.Add] = Add;
            _handlers[(int)Op.Subtract] = Subtract;
            _handlers[(int)Op.Multiply] = Multiply;
            _handlers[(int)Op.Divide] = Divide;
            _handlers[(int)Op.BranchIfLess] = BranchIfLess;
            _handlers[(int)Op.BranchIfGreaterOrEqual] = BranchIfGreaterOrEqual;
            _handlers[(int)Op.Duplicate] = Duplicate;
            _handlers[(int)Op.Load] = Load;
            _handlers[(int)Op.Store] = Store;
            _handlers[(int)Op.End] = End;
        }

        public static Code Preprocess(Code byteCode) => byteCode;

        public void Run(byte* byteCode)
        {
            _byteCode = byteCode;
            _stackPointer = -1;
            _programCounter = 0;

            _handlers[_byteCode[_programCounter]](this);
        }

        private static void NoOp(ManagedCallVM self)
        {
            ++self._programCounter;
            _handlers[self._byteCode[self._programCounter]](self);
        }

        private static void Push(ManagedCallVM self)
        {
            var value = *((int*)(self._byteCode + self._programCounter + 1));
            ++self._stackPointer;
            self._stack[self._stackPointer] = value;
            self._programCounter += (1 + sizeof(int));
            _handlers[self._byteCode[self._programCounter]](self);
        }

        private static void Pop(ManagedCallVM self)
        {
            --self._stackPointer;
            ++self._programCounter;
            _handlers[self._byteCode[self._programCounter]](self);
        }

        private static void Add(ManagedCallVM self)
        {
            self._stack[self._stackPointer - 1] = self._stack[self._stackPointer - 1] + self._stack[self._stackPointer];
            --self._stackPointer;
            ++self._programCounter;
            _handlers[self._byteCode[self._programCounter]](self);
        }

        private static void Subtract(ManagedCallVM self)
        {
            self._stack[self._stackPointer - 1] = self._stack[self._stackPointer - 1] - self._stack[self._stackPointer];
            --self._stackPointer;
            ++self._programCounter;
            _handlers[self._byteCode[self._programCounter]](self);
        }

        private static void Multiply(ManagedCallVM self)
        {
            self._stack[self._stackPointer - 1] = self._stack[self._stackPointer - 1] * self._stack[self._stackPointer];
            --self._stackPointer;
            ++self._programCounter;
            _handlers[self._byteCode[self._programCounter]](self);
        }

        private static void Divide(ManagedCallVM self)
        {
            self._stack[self._stackPointer - 1] = self._stack[self._stackPointer - 1] / self._stack[self._stackPointer];
            --self._stackPointer;
            ++self._programCounter;
            _handlers[self._byteCode[self._programCounter]](self);
        }

        private static void BranchIfLess(ManagedCallVM self)
        {
            var greaterOrEqual = self._stack[self._stackPointer - 1] < self._stack[self._stackPointer];
            self._stackPointer -= 2;
            if (greaterOrEqual)
            {
                self._programCounter = *((int*)(self._byteCode + self._programCounter + 1));
            }
            else
            {
                self._programCounter += (1 + sizeof(int));
            }
            _handlers[self._byteCode[self._programCounter]](self);
        }

        private static void BranchIfGreaterOrEqual(ManagedCallVM self)
        {
            var greaterOrEqual = self._stack[self._stackPointer - 1] >= self._stack[self._stackPointer];
            self._stackPointer -= 2;
            if (greaterOrEqual)
            {
                self._programCounter = *((int*)(self._byteCode + self._programCounter + 1));
            }
            else
            {
                self._programCounter += (1 + sizeof(int));
            }
            _handlers[self._byteCode[self._programCounter]](self);
        }

        private static void Duplicate(ManagedCallVM self)
        {
            ++self._stackPointer;
            self._stack[self._stackPointer] = self._stack[self._stackPointer - 1];
            ++self._programCounter;
            _handlers[self._byteCode[self._programCounter]](self);
        }

        private static void Load(ManagedCallVM self)
        {
            var loadFrom = *((int*)(self._byteCode + self._programCounter + 1));
            ++self._stackPointer;
            self._stack[self._stackPointer] = self._stack[self._stackPointer - loadFrom];
            self._programCounter += (1 + sizeof(int));
            _handlers[self._byteCode[self._programCounter]](self);
        }

        private static void Store(ManagedCallVM self)
        {
            var storeTo = *((int*)(self._byteCode + self._programCounter + 1));
            self._stack[self._stackPointer - storeTo] = self._stack[self._stackPointer];
            --self._stackPointer;
            self._programCounter += (1 + sizeof(int));
            _handlers[self._byteCode[self._programCounter]](self);
        }

        private static void End(ManagedCallVM self)
        {
            ++self._programCounter;
        }
    }
}
