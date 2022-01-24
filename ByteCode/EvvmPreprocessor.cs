using System;
using System.Collections.Generic;

namespace ByteCode
{
    public static unsafe class EvvmPreprocessor<T>
        where T : unmanaged
    {
        private delegate void ParseOpDelegate<U>(ref U args, byte value, int index);
        private delegate int GetCurrentAddressDelegate<U>(ref U args);
        private delegate void AlignDelegate<U>(ref U args);

        public static Code Preprocess(byte * byteCode, T * handlers, bool shouldAlignOps = false)
        {
            var result = new List<byte>();
            ParseOpDelegate<List<byte>> addToResult = (ref List<byte> result_, byte value, int index) => result_.Add(value);
            GetCurrentAddressDelegate<List<byte>> getCurrentAddress = (ref List<byte> arg) => arg.Count;
            AlignDelegate<List<byte>> align = (ref List<byte> result_) => result_.Add(0);

            var displacements = new Dictionary<int, List<int>>();

            var programCounter = 0;
            for (var running = true; running;)
            {
                var instruction = ParseOp(handlers, byteCode, ref programCounter, ref result, addToResult);
                switch (instruction)
                {
                    case EvvmOp.AddI32_I32i_I32i_I32r:
                    case EvvmOp.AddI32_I32i_I32r_I32r:
                    case EvvmOp.AddI32_I32r_I32r_I32r:
                    case EvvmOp.DivideI32_I32i_I32i_I32r:
                    case EvvmOp.DivideI32_I32i_I32r_I32r:
                    case EvvmOp.DivideI32_I32r_I32r_I32r:
                    case EvvmOp.MultiplyI32_I32i_I32i_I32r:
                    case EvvmOp.MultiplyI32_I32i_I32r_I32r:
                    case EvvmOp.MultiplyI32_I32r_I32r_I32r:
                    case EvvmOp.SubtractI32_I32i_I32i_I32r:
                    case EvvmOp.SubtractI32_I32i_I32r_I32r:
                    case EvvmOp.SubtractI32_I32r_I32r_I32r:
                        {
                            ParseInt32(byteCode, ref programCounter, ref result, addToResult);
                            ParseInt32(byteCode, ref programCounter, ref result, addToResult);
                            ParseInt32(byteCode, ref programCounter, ref result, addToResult);
                            AlignOp(shouldAlignOps, ref result, getCurrentAddress, align);
                        }
                        break;

                    case EvvmOp.CopyI32_I32i_I32r:
                    case EvvmOp.CopyI32_I32r_I32r:
                        {
                            ParseInt32(byteCode, ref programCounter, ref result, addToResult);
                            ParseInt32(byteCode, ref programCounter, ref result, addToResult);
                            AlignOp(shouldAlignOps, ref result, getCurrentAddress, align);
                        }
                        break;

                    case EvvmOp.BranchIfGreaterOrEqualI32_I32i_I32i_I32i:
                    case EvvmOp.BranchIfGreaterOrEqualI32_I32i_I32r_I32i:
                    case EvvmOp.BranchIfGreaterOrEqualI32_I32r_I32r_I32i:
                    case EvvmOp.BranchIfLessI32_I32i_I32i_I32i:
                    case EvvmOp.BranchIfLessI32_I32i_I32r_I32i:
                    case EvvmOp.BranchIfLessI32_I32r_I32r_I32i:
                        {
                            ParseInt32(byteCode, ref programCounter, ref result, addToResult);
                            ParseInt32(byteCode, ref programCounter, ref result, addToResult);

                            var writeAddress = result.Count;
                            var branchTarget = ParseInstructionAddress(byteCode, ref programCounter, ref result, addToResult);
                            if (!displacements.TryGetValue(branchTarget, out var writeAddresses))
                            {
                                writeAddresses = new List<int>();
                                displacements.Add(branchTarget, writeAddresses);
                            }
                            writeAddresses.Add(writeAddress);
                            AlignOp(shouldAlignOps, ref result, getCurrentAddress, align);
                        }
                        break;

                    case EvvmOp.End:
                        {
                            running = false;
                        }
                        break;
                }
            }

            var fixedMemory = new FixedMemory(result);

            int addressIndexToWrite = 0;
            programCounter = 0;

            ParseOpDelegate<int> incrementAddressIndexToWrite =
                (ref int addressIndexToWrite, byte value, int index) => ++addressIndexToWrite;

            ParseOpDelegate<(FixedMemory Result, int AddressIndexToWriteTo)> writeAddressByte =
                (ref (FixedMemory Result, int AddressIndexToWriteTo) args, byte value, int index) => args.Result[args.AddressIndexToWriteTo + index] = value;

            GetCurrentAddressDelegate<int> getAddressToWrite = (ref int addressToWrite_) => addressToWrite_;
            AlignDelegate<int> alignAddressToWrite = (ref int addressToWrite_) => ++addressToWrite_;

            for (;;)
            {
                if (displacements.TryGetValue(programCounter, out var writeAddresses))
                {
                    foreach (var addressIndexToWriteTo in writeAddresses)
                    {
                        var addressToWrite = fixedMemory.GetAddressOfByte(addressIndexToWrite);
                        var zero = 0;
                        var args = (fixedMemory, addressIndexToWriteTo);
                        ParseInstructionAddress((byte*)&addressToWrite, ref zero, ref args, writeAddressByte, readIntPtrBytes: true);
                    }
                }

                var instruction = ParseOp(handlers, byteCode, ref programCounter, ref addressIndexToWrite, incrementAddressIndexToWrite);
                switch (instruction)
                {
                    case EvvmOp.AddI32_I32i_I32i_I32r:
                    case EvvmOp.AddI32_I32i_I32r_I32r:
                    case EvvmOp.AddI32_I32r_I32r_I32r:
                    case EvvmOp.DivideI32_I32i_I32i_I32r:
                    case EvvmOp.DivideI32_I32i_I32r_I32r:
                    case EvvmOp.DivideI32_I32r_I32r_I32r:
                    case EvvmOp.MultiplyI32_I32i_I32i_I32r:
                    case EvvmOp.MultiplyI32_I32i_I32r_I32r:
                    case EvvmOp.MultiplyI32_I32r_I32r_I32r:
                    case EvvmOp.SubtractI32_I32i_I32i_I32r:
                    case EvvmOp.SubtractI32_I32i_I32r_I32r:
                    case EvvmOp.SubtractI32_I32r_I32r_I32r:
                        {
                            ParseInt32(byteCode, ref programCounter, ref addressIndexToWrite, incrementAddressIndexToWrite);
                            ParseInt32(byteCode, ref programCounter, ref addressIndexToWrite, incrementAddressIndexToWrite);
                            ParseInt32(byteCode, ref programCounter, ref addressIndexToWrite, incrementAddressIndexToWrite);
                            AlignOp(shouldAlignOps, ref addressIndexToWrite, getAddressToWrite, alignAddressToWrite);
                        }
                        break;

                    case EvvmOp.CopyI32_I32i_I32r:
                    case EvvmOp.CopyI32_I32r_I32r:
                        {
                            ParseInt32(byteCode, ref programCounter, ref addressIndexToWrite, incrementAddressIndexToWrite);
                            ParseInt32(byteCode, ref programCounter, ref addressIndexToWrite, incrementAddressIndexToWrite);
                            AlignOp(shouldAlignOps, ref addressIndexToWrite, getAddressToWrite, alignAddressToWrite);
                        }
                        break;

                    case EvvmOp.BranchIfGreaterOrEqualI32_I32i_I32i_I32i:
                    case EvvmOp.BranchIfGreaterOrEqualI32_I32i_I32r_I32i:
                    case EvvmOp.BranchIfGreaterOrEqualI32_I32r_I32r_I32i:
                    case EvvmOp.BranchIfLessI32_I32i_I32i_I32i:
                    case EvvmOp.BranchIfLessI32_I32i_I32r_I32i:
                    case EvvmOp.BranchIfLessI32_I32r_I32r_I32i:
                        {
                            ParseInt32(byteCode, ref programCounter, ref addressIndexToWrite, incrementAddressIndexToWrite);
                            ParseInt32(byteCode, ref programCounter, ref addressIndexToWrite, incrementAddressIndexToWrite);
                            ParseInstructionAddress(byteCode, ref programCounter, ref addressIndexToWrite, incrementAddressIndexToWrite, readIntPtrBytes: true);
                            AlignOp(shouldAlignOps, ref addressIndexToWrite, getAddressToWrite, alignAddressToWrite);
                        }
                        break;

                    case EvvmOp.End:
                        return Code.FromAllocHGlobal(fixedMemory.Take());
                }
            }
        }

        private static EvvmOp ParseOp<U>(T * handlers, byte * byteCode, ref int programCounter, ref U args, ParseOpDelegate<U> result)
        {
            var value = byteCode[programCounter];
            var addr = handlers[value];
            // This is actually a pointer to a pointer to a function. We want to copy the
            // bytes of the pointer to the function so we index into the pointer to a
            // pointer to get them.
            var byteAddr = (byte *)&addr;
            for (var i = 0; i < sizeof(T); ++i)
            {
                result.Invoke(ref args, byteAddr[i], i);
            }
            ++programCounter;

            return (EvvmOp)value;
        }

        private static int ParseInt32<U>(byte* byteCode, ref int programCounter, ref U args, ParseOpDelegate<U> result)
        {
            int value = 0;
            var valueAddr = (byte*)&value;
            for (var i = 0; i < sizeof(int); ++i)
            {
                *(valueAddr + i) = byteCode[programCounter];
                result.Invoke(ref args, byteCode[programCounter], i);
                ++programCounter;
            }
            return value;
        }

        private static int ParseInstructionAddress<U>(
            byte* byteCode,
            ref int programCounter,
            ref U args,
            ParseOpDelegate<U> result,
            bool readIntPtrBytes = false
        )
        {
            int value = 0;
            var valueAddr = (byte*)&value;
            for (var i = 0; i < sizeof(int); ++i)
            {
                *(valueAddr + i) = byteCode[programCounter];
                result.Invoke(ref args, byteCode[programCounter], i);
                ++programCounter;
            }

            for (var i = 0; i < sizeof(IntPtr) - sizeof(int); ++i)
            {
                var byteToRead = readIntPtrBytes ? byteCode[programCounter + i] : (byte)0;
                result.Invoke(ref args, byteToRead, sizeof(int) + i);
            }

            return value;
        }

        private static void AlignOp<U>(bool shouldAlignOps, ref U args, GetCurrentAddressDelegate<U> getCurrentAddress, AlignDelegate<U> align)
        {
            if (!shouldAlignOps) return;
            while (getCurrentAddress(ref args) % sizeof(T) != 0)
            {
                align(ref args);
            }
        }
    }
}
