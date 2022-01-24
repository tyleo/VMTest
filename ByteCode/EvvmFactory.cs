using System.Collections.Generic;

namespace ByteCode
{
    public sealed class EvvmFactory
    {
        private readonly List<byte> _byteCode = new List<byte>();
        private readonly bool _align;

        public int CurrentAddress => _byteCode.Count;

        private EvvmFactory(bool align) => _align = align;

        public static EvvmFactory New(bool align = false) => new EvvmFactory(align);

        public byte[] ToArray() => _byteCode.ToArray();

        public EvvmFactory AddI32_I32i_I32i_I32r(int lhs, int rhs, int output)
        {
            _byteCode.Add((byte)EvvmOp.AddI32_I32i_I32i_I32r);
            Int(lhs);
            Int(rhs);
            Int(output);
            return this;
        }

        public EvvmFactory AddI32_I32i_I32r_I32r(int lhs, int rhs, int output)
        {
            _byteCode.Add((byte)EvvmOp.AddI32_I32i_I32r_I32r);
            Int(lhs);
            Int(rhs);
            Int(output);
            return this;
        }

        public EvvmFactory AddI32_I32r_I32r_I32r(int lhs, int rhs, int output)
        {
            _byteCode.Add((byte)EvvmOp.AddI32_I32r_I32r_I32r);
            Int(lhs);
            Int(rhs);
            Int(output);
            return this;
        }

        public EvvmFactory BranchIfGreaterOrEqualI32_I32i_I32i_I32i(int lhs, int rhs, int address)
        {
            _byteCode.Add((byte)EvvmOp.BranchIfGreaterOrEqualI32_I32i_I32i_I32i);
            Int(lhs);
            Int(rhs);
            Int(address);
            return this;
        }

        public EvvmFactory BranchIfGreaterOrEqualI32_I32i_I32r_I32i(int lhs, int rhs, int address)
        {
            _byteCode.Add((byte)EvvmOp.BranchIfGreaterOrEqualI32_I32i_I32r_I32i);
            Int(lhs);
            Int(rhs);
            Int(address);
            return this;
        }

        public EvvmFactory BranchIfGreaterOrEqualI32_I32r_I32r_I32i(int lhs, int rhs, int address)
        {
            _byteCode.Add((byte)EvvmOp.BranchIfGreaterOrEqualI32_I32r_I32r_I32i);
            Int(lhs);
            Int(rhs);
            Int(address);
            return this;
        }

        public EvvmFactory BranchIfLessI32_I32i_I32i_I32i(int lhs, int rhs, int address)
        {
            _byteCode.Add((byte)EvvmOp.BranchIfLessI32_I32i_I32i_I32i);
            Int(lhs);
            Int(rhs);
            Int(address);
            return this;
        }

        public EvvmFactory BranchIfLessI32_I32i_I32r_I32i(int lhs, int rhs, int address)
        {
            _byteCode.Add((byte)EvvmOp.BranchIfLessI32_I32i_I32r_I32i);
            Int(lhs);
            Int(rhs);
            Int(address);
            return this;
        }

        public EvvmFactory BranchIfLessI32_I32r_I32r_I32i(int lhs, int rhs, int address)
        {
            _byteCode.Add((byte)EvvmOp.BranchIfLessI32_I32r_I32r_I32i);
            Int(lhs);
            Int(rhs);
            Int(address);
            return this;
        }

        public EvvmFactory CopyI32_I32i_I32r(int from, int to)
        {
            _byteCode.Add((byte)EvvmOp.CopyI32_I32i_I32r);
            Int(from);
            Int(to);
            return this;
        }

        public EvvmFactory CopyI32_I32r_I32r(int from, int to)
        {
            _byteCode.Add((byte)EvvmOp.CopyI32_I32r_I32r);
            Int(from);
            Int(to);
            return this;
        }

        public EvvmFactory DivideI32_I32i_I32i_I32r(int lhs, int rhs, int output)
        {
            _byteCode.Add((byte)EvvmOp.DivideI32_I32i_I32i_I32r);
            Int(lhs);
            Int(rhs);
            Int(output);
            return this;
        }

        public EvvmFactory DivideI32_I32i_I32r_I32r(int lhs, int rhs, int output)
        {
            _byteCode.Add((byte)EvvmOp.DivideI32_I32i_I32r_I32r);
            Int(lhs);
            Int(rhs);
            Int(output);
            return this;
        }

        public EvvmFactory DivideI32_I32r_I32r_I32r(int lhs, int rhs, int output)
        {
            _byteCode.Add((byte)EvvmOp.DivideI32_I32r_I32r_I32r);
            Int(lhs);
            Int(rhs);
            Int(output);
            return this;
        }

        public EvvmFactory MultiplyI32_I32i_I32i_I32r(int lhs, int rhs, int output)
        {
            _byteCode.Add((byte)EvvmOp.MultiplyI32_I32i_I32i_I32r);
            Int(lhs);
            Int(rhs);
            Int(output);
            return this;
        }

        public EvvmFactory MultiplyI32_I32i_I32r_I32r(int lhs, int rhs, int output)
        {
            _byteCode.Add((byte)EvvmOp.MultiplyI32_I32i_I32r_I32r);
            Int(lhs);
            Int(rhs);
            Int(output);
            return this;
        }

        public EvvmFactory MultiplyI32_I32r_I32r_I32r(int lhs, int rhs, int output)
        {
            _byteCode.Add((byte)EvvmOp.MultiplyI32_I32r_I32r_I32r);
            Int(lhs);
            Int(rhs);
            Int(output);
            return this;
        }

        public EvvmFactory NoOp()
        {
            _byteCode.Add((byte)EvvmOp.NoOp);
            return this;
        }

        public EvvmFactory SubtractI32_I32i_I32i_I32r(int lhs, int rhs, int output)
        {
            _byteCode.Add((byte)EvvmOp.SubtractI32_I32i_I32i_I32r);
            Int(lhs);
            Int(rhs);
            Int(output);
            return this;
        }

        public EvvmFactory SubtractI32_I32i_I32r_I32r(int lhs, int rhs, int output)
        {
            _byteCode.Add((byte)EvvmOp.SubtractI32_I32i_I32r_I32r);
            Int(lhs);
            Int(rhs);
            Int(output);
            return this;
        }

        public EvvmFactory SubtractI32_I32r_I32r_I32r(int lhs, int rhs, int output)
        {
            _byteCode.Add((byte)EvvmOp.SubtractI32_I32r_I32r_I32r);
            Int(lhs);
            Int(rhs);
            Int(output);
            return this;
        }

        public EvvmFactory End()
        {
            _byteCode.Add((byte)EvvmOp.End);
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
