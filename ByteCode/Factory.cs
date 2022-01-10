using System.Collections.Generic;

namespace ByteCode
{
    public sealed class Factory
    {
        private readonly List<byte> _byteCode = new List<byte>();
        private readonly bool _align;

        public int CurrentAddress => _byteCode.Count;

        private Factory(bool align) => _align = align;

        public static Factory New(bool align = false) => new Factory(align);

        public byte[] ToArray() => _byteCode.ToArray();

        public Factory NoOp()
        {
            _byteCode.Add((byte)Op.NoOp);
            return this;
        }

        public Factory Push(int value)
        {
            if (_align)
            {
                while (_byteCode.Count % sizeof(int) != sizeof(int) - 1) NoOp();
            }

            _byteCode.Add((byte)Op.Push);
            Int(value);
            return this;
        }

        public Factory Pop()
        {
            _byteCode.Add((byte)Op.Pop);
            return this;
        }

        public Factory Add()
        {
            _byteCode.Add((byte)Op.Add);
            return this;
        }

        public Factory Subtract()
        {
            _byteCode.Add((byte)Op.Subtract);
            return this;
        }

        public Factory Multiply()
        {
            _byteCode.Add((byte)Op.Multiply);
            return this;
        }

        public Factory Divide()
        {
            _byteCode.Add((byte)Op.Divide);
            return this;
        }


        public Factory End()
        {
            _byteCode.Add((byte)Op.End);
            return this;
        }

        public Factory BranchIfLess(int address)
        {
            _byteCode.Add((byte)Op.BranchIfLess);
            Int(address);
            return this;
        }

        public Factory BranchIfGreaterOrEqual(int address)
        {
            _byteCode.Add((byte)Op.BranchIfGreaterOrEqual);
            Int(address);
            return this;
        }

        public Factory Duplicate()
        {
            _byteCode.Add((byte)Op.Duplicate);
            return this;
        }

        public Factory Load(int index)
        {
            _byteCode.Add((byte)Op.Load);
            Int(index + 1);
            return this;
        }

        public Factory Store(int index)
        {
            _byteCode.Add((byte)Op.Store);
            Int(index + 1);
            return this;
        }

        public unsafe void EmplaceInt(int address, int value)
        {
            var pValueInt = &value;
            var pValueBytes = (byte*)pValueInt;
            _byteCode[address + 0] = pValueBytes[0];
            _byteCode[address + 1] = pValueBytes[1];
            _byteCode[address + 2] = pValueBytes[2];
            _byteCode[address + 3] = pValueBytes[3];
        }

        private unsafe void Int(int value)
        {
            var pValueInt = &value;
            var pValueBytes = (byte*)pValueInt;
            _byteCode.Add(pValueBytes[0]);
            _byteCode.Add(pValueBytes[1]);
            _byteCode.Add(pValueBytes[2]);
            _byteCode.Add(pValueBytes[3]);
        }
    }
}
