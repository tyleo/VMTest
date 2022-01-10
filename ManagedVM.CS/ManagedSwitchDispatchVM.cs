using ByteCode;

namespace ManagedVM.CS
{
    public sealed unsafe class ManagedSwitchDispatchVM
    {
        private byte * _byteCode;
        private readonly int[] _stack = new int[1024];
        private int _stackPointer;
        private int _programCounter;

        public int Last => _stack[_stackPointer];

        public static Code Preprocess(Code byteCode) => byteCode;

        public void Run(byte * byteCode)
        {
            _byteCode = byteCode;
            _stackPointer = -1;
            _programCounter = 0;

            for (;;)
            {
                var instruction = (Op)_byteCode[_programCounter];
                switch (instruction)
                {
                    case Op.NoOp:
                        ++_programCounter;
                        break;

                    case Op.Push:
                        var value = *((int*)(_byteCode + _programCounter + 1));
                        ++_stackPointer;
                        _stack[_stackPointer] = value;
                        _programCounter += (1 + sizeof(int));
                        break;

                    case Op.Pop:
                        --_stackPointer;
                        ++_programCounter;
                        break;

                    case Op.Add:
                        _stack[_stackPointer - 1] = _stack[_stackPointer - 1] + _stack[_stackPointer];
                        --_stackPointer;
                        ++_programCounter;
                        break;

                    case Op.Subtract:
                        _stack[_stackPointer - 1] = _stack[_stackPointer - 1] - _stack[_stackPointer];
                        --_stackPointer;
                        ++_programCounter;
                        break;

                    case Op.Multiply:
                        _stack[_stackPointer - 1] = _stack[_stackPointer - 1] * _stack[_stackPointer];
                        --_stackPointer;
                        ++_programCounter;
                        break;

                    case Op.Divide:
                        _stack[_stackPointer - 1] = _stack[_stackPointer - 1] / _stack[_stackPointer];
                        --_stackPointer;
                        ++_programCounter;
                        break;

                    case Op.BranchIfLess:
                        var less = _stack[_stackPointer - 1] < _stack[_stackPointer];
                        _stackPointer -= 2;
                        if (less)
                        {
                            _programCounter = *((int*)(_byteCode + _programCounter + 1));
                        }
                        else
                        {
                            _programCounter += (1 + sizeof(int));
                        }
                        break;

                    case Op.BranchIfGreaterOrEqual:
                        var greaterOrEqual = _stack[_stackPointer - 1] >= _stack[_stackPointer];
                        _stackPointer -= 2;
                        if (greaterOrEqual)
                        {
                            _programCounter = *((int *)(_byteCode + _programCounter + 1));
                        }
                        else
                        {
                            _programCounter += (1 + sizeof(int));
                        }
                        break;

                    case Op.Duplicate:
                        ++_stackPointer;
                        _stack[_stackPointer] = _stack[_stackPointer - 1];
                        ++_programCounter;
                        break;

                    case Op.Load:
                        var loadFrom = *((int*)(_byteCode + _programCounter + 1));
                        ++_stackPointer;
                        _stack[_stackPointer] = _stack[_stackPointer - loadFrom];
                        _programCounter += (1 + sizeof(int));
                        break;

                    case Op.Store:
                        var storeTo = *((int*)(_byteCode + _programCounter + 1));
                        _stack[_stackPointer - storeTo] = _stack[_stackPointer];
                        --_stackPointer;
                        _programCounter += (1 + sizeof(int));
                        break;

                    case Op.End:
                        ++_programCounter;
                        return;
                }
            }
        }
    }
}
