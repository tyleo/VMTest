using System.Collections.Generic;

namespace ByteCode
{
    public static unsafe class Preprocessor<T>
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
                    case Op.Push:
                        {
                            ParseInt32(byteCode, ref programCounter, ref result, addToResult);
                            AlignOp(shouldAlignOps, ref result, getCurrentAddress, align);
                        }
                        break;

                    case Op.BranchIfLess:
                        {
                            var writeAddress = result.Count;
                            var branchTarget = ParseInt32(byteCode, ref programCounter, ref result, addToResult);
                            if (!displacements.TryGetValue(branchTarget, out var writeAddresses))
                            {
                                writeAddresses = new List<int>();
                                displacements.Add(branchTarget, writeAddresses);
                            }
                            writeAddresses.Add(writeAddress);
                            AlignOp(shouldAlignOps, ref result, getCurrentAddress, align);
                        }
                        break;

                    case Op.BranchIfGreaterOrEqual:
                        {
                            var writeAddress = result.Count;
                            var branchTarget = ParseInt32(byteCode, ref programCounter, ref result, addToResult);
                            if (!displacements.TryGetValue(branchTarget, out var writeAddresses))
                            {
                                writeAddresses = new List<int>();
                                displacements.Add(branchTarget, writeAddresses);
                            }
                            writeAddresses.Add(writeAddress);
                            AlignOp(shouldAlignOps, ref result, getCurrentAddress, align);
                        }
                        break;

                    case Op.Load:
                        {
                            ParseInt32(byteCode, ref programCounter, ref result, addToResult);
                            AlignOp(shouldAlignOps, ref result, getCurrentAddress, align);
                        }
                        break;

                    case Op.Store:
                        {
                            ParseInt32(byteCode, ref programCounter, ref result, addToResult);
                            AlignOp(shouldAlignOps, ref result, getCurrentAddress, align);
                        }
                        break;

                    case Op.End:
                        {
                            running = false;
                        }
                        break;
                }
            }

            int addressToWrite = 0;
            programCounter = 0;

            ParseOpDelegate<int> incrementAddressToWrite = (ref int addressToWrite_, byte value, int index) => ++addressToWrite_;
            ParseOpDelegate<(List<byte> Result, int AddressToWriteTo)> writeAddressByte =
                (ref (List<byte> Result, int AddressToWriteTo) args, byte value, int index) => args.Result[args.AddressToWriteTo + index] = value;
            GetCurrentAddressDelegate<int> getAddressToWrite = (ref int addressToWrite_) => addressToWrite_;
            AlignDelegate<int> alignAddressToWrite = (ref int addressToWrite_) => ++addressToWrite_;

            for (; ; )
            {
                if (displacements.TryGetValue(programCounter, out var writeAddresses))
                {
                    foreach (var writeAddress in writeAddresses)
                    {
                        var args = (result, writeAddress);
                        var zero = 0;
                        ParseInt32((byte*)&addressToWrite, ref zero, ref args, writeAddressByte);
                    }
                }

                var instruction = ParseOp(handlers, byteCode, ref programCounter, ref addressToWrite, incrementAddressToWrite);
                switch (instruction)
                {
                    case Op.Push:
                        {
                            ParseInt32(byteCode, ref programCounter, ref addressToWrite, incrementAddressToWrite);
                            AlignOp(shouldAlignOps, ref addressToWrite, getAddressToWrite, alignAddressToWrite);
                        }
                        break;

                    case Op.BranchIfLess:
                        {
                            ParseInt32(byteCode, ref programCounter, ref addressToWrite, incrementAddressToWrite);
                            AlignOp(shouldAlignOps, ref addressToWrite, getAddressToWrite, alignAddressToWrite);
                        }
                        break;

                    case Op.BranchIfGreaterOrEqual:
                        {
                            ParseInt32(byteCode, ref programCounter, ref addressToWrite, incrementAddressToWrite);
                            AlignOp(shouldAlignOps, ref addressToWrite, getAddressToWrite, alignAddressToWrite);
                        }
                        break;

                    case Op.Load:
                        {
                            ParseInt32(byteCode, ref programCounter, ref addressToWrite, incrementAddressToWrite);
                            AlignOp(shouldAlignOps, ref addressToWrite, getAddressToWrite, alignAddressToWrite);
                        }
                        break;

                    case Op.Store:
                        {
                            ParseInt32(byteCode, ref programCounter, ref addressToWrite, incrementAddressToWrite);
                            AlignOp(shouldAlignOps, ref addressToWrite, getAddressToWrite, alignAddressToWrite);
                        }
                        break;

                    case Op.End:
                        return new Code(result.ToArray(), shouldAlignOps ? sizeof(T) : 1);
                }
            }
        }

        private static Op ParseOp<U>(T * handlers, byte * byteCode, ref int programCounter, ref U args, ParseOpDelegate<U> result)
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

            return (Op)value;
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
