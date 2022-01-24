using ByteCode;
using ManagedVM.CS;
using NativeVM.CS;
using LuaVM;
using System;
using System.Diagnostics;

namespace VMTest
{
    public sealed unsafe class Program
    {
        private delegate Code PreprocessDelegate(Code byteCode);
        private delegate int RunDelegate(byte* byteCode);
        private delegate int LuaDelegate();

        private static Code EvvmTest()
        {
            var factory = EvvmFactory
                .New()
                .BranchIfLessI32_I32i_I32i_I32i(0, 1, 1 + sizeof(int) + sizeof(int) + sizeof(int))
                .End();
            return Evvm.Preprocess(new Code(factory.ToArray(), 1));
        }

        public static void Main()
        {
            var fibonacciValue = 1000;
            var mixedOpByteCode = GenMixedOp(1000);
            var fibonacciByteCode = GenFibonacci(fibonacciValue);

            var evvmFibonacciByteCode = EvvmFiboacci(fibonacciValue);

            var stopWatch = new Stopwatch();

            var managedCallVM = new ManagedCallVM();
            int runManagedCallVM(byte* byteCode)
            {
                managedCallVM.Run(byteCode);
                return managedCallVM.Last;
            };

            var managedCalliVM = new ManagedCalliVM();
            int runManagedCalliVM(byte* byteCode)
            {
                managedCalliVM.Run(byteCode);
                return managedCalliVM.Last;
            };

            var managedCalliEmbeddedVM = new ManagedCalliEmbeddedVM();
            int runManagedCalliEmbeddedVM(byte* byteCode)
            {
                managedCalliEmbeddedVM.Run(byteCode);
                return managedCalliEmbeddedVM.Last;
            };

            var managedSwitchDispatchVM = new ManagedSwitchDispatchVM();
            int runManagedSwitchDispatchVM(byte* byteCode)
            {
                managedSwitchDispatchVM.Run(byteCode);
                return managedSwitchDispatchVM.Last;
            };

            var managedTailCallEmbeddedVM = new ManagedTailCallEmbeddedVM();
            int runManagedTailCallEmbeddedVM(byte* byteCode)
            {
                managedTailCallEmbeddedVM.Run(byteCode);
                return managedTailCallEmbeddedVM.Last;
            };

            var nativeAddressOfLabelVM = new NativeAddressOfLabelVM();
            int runNativeAddressOfLabelVM(byte* byteCode)
            {
                nativeAddressOfLabelVM.Run(byteCode);
                return nativeAddressOfLabelVM.Last;
            };

            var nativeAddressOfLabelEmbeddedVM = new NativeAddressOfLabelEmbeddedVM();
            int runNativeAddressOfLabelEmbeddedVM(byte* byteCode)
            {
                nativeAddressOfLabelEmbeddedVM.Run(byteCode);
                return nativeAddressOfLabelEmbeddedVM.Last;
            };

            var nativeAddressOfLabelEmbeddedAlignedVM = new NativeAddressOfLabelEmbeddedAlignedVM();
            int runNativeAddressOfLabelEmbeddedAlignedVM(byte* byteCode)
            {
                nativeAddressOfLabelEmbeddedAlignedVM.Run(byteCode);
                return nativeAddressOfLabelEmbeddedAlignedVM.Last;
            };

            var nativeCalliVM = new NativeCalliVM();
            int runNativeCalliVM(byte* byteCode)
            {
                nativeCalliVM.Run(byteCode);
                return nativeCalliVM.Last;
            };

            var nativeCalliEmbeddedVM = new NativeCalliEmbeddedVM();
            int runNativeCalliEmbeddedVM(byte* byteCode)
            {
                nativeCalliEmbeddedVM.Run(byteCode);
                return nativeCalliEmbeddedVM.Last;
            };

            var nativeSwitchDispatchVM = new NativeSwitchDispatchVM();
            int runNativeSwitchDispatchVM(byte* byteCode)
            {
                nativeSwitchDispatchVM.Run(byteCode);
                return nativeSwitchDispatchVM.Last;
            };

            var luaVM = new CLuaVM();
            luaVM.Prepare(fibonacciValue);
            int runLuaVM()
            {
                return luaVM.Fibonacci();
            };

            var evvm = new Evvm();
            int runEvvm(byte* byteCode)
            {
                evvm.Run(byteCode);
                return evvm.Get(8);
            }

            var times = 100000;

            //Console.WriteLine("MixedOp");
            //Run("ManagedCallVM", runManagedCallVM, ManagedCallVM.Preprocess(mixedOpByteCode), stopWatch, times);
            //Run("ManagedCalliVM", runManagedCalliVM, ManagedCalliVM.Preprocess(mixedOpByteCode), stopWatch, times);
            //Run("ManagedCalliEmbeddedVM", runManagedCalliEmbeddedVM, ManagedCalliEmbeddedVM.Preprocess(mixedOpByteCode), stopWatch, times);
            //var managedSwitchDispatchMixedOpTime = Run("ManagedSwitchDispatchVM", runManagedSwitchDispatchVM, ManagedSwitchDispatchVM.Preprocess(mixedOpByteCode), stopWatch, times);
            //var managedTailCallEmbeddedMixedOpTime = Run("ManagedTailCallEmbeddedVM", runManagedTailCallEmbeddedVM, ManagedTailCallEmbeddedVM.Preprocess(mixedOpByteCode), stopWatch, times);
            //var nativeAddressOfLabelMixedOpTime = Run("NativeAddressOfLabelVM", runNativeAddressOfLabelVM, NativeAddressOfLabelVM.Preprocess(mixedOpByteCode), stopWatch, times);
            //var nativeAddressOfLabelEmbeddedMixedOpTime = Run("NativeAddressOfLabelEmbeddedVM", runNativeAddressOfLabelEmbeddedVM, NativeAddressOfLabelEmbeddedVM.Preprocess(mixedOpByteCode), stopWatch, times);
            //var nativeAddressOfLabelEmbeddedAlignedMixedOpTime = Run("NativeAddressOfLabelEmbeddedAlignedVM", runNativeAddressOfLabelEmbeddedAlignedVM, NativeAddressOfLabelEmbeddedAlignedVM.Preprocess(mixedOpByteCode), stopWatch, times);
            //var nativeCalliMixedOpTime = Run("NativeCalliVM", runNativeCalliVM, NativeCalliVM.Preprocess(mixedOpByteCode), stopWatch, times);
            //var nativeCalliEmbeddedMixedOpTime = Run("NativeCalliEmbeddedVM", runNativeCalliEmbeddedVM, NativeCalliEmbeddedVM.Preprocess(mixedOpByteCode), stopWatch, times);
            //var nativeSwitchDispatchMixedOpTime = Run("NativeSwitchDispatchVM", runNativeSwitchDispatchVM, NativeSwitchDispatchVM.Preprocess(mixedOpByteCode), stopWatch, times);
            //Console.WriteLine();

            Console.WriteLine("Fibonacci");
            Run("ManagedCallVM", runManagedCallVM, ManagedCallVM.Preprocess(fibonacciByteCode), stopWatch, times);
            Run("ManagedCalliVM", runManagedCalliVM, ManagedCalliVM.Preprocess(fibonacciByteCode), stopWatch, times);
            Run("ManagedCalliEmbeddedVM", runManagedCalliEmbeddedVM, ManagedCalliEmbeddedVM.Preprocess(fibonacciByteCode), stopWatch, times);
            var managedSwitchDispatchFibonacciTime = Run("ManagedSwitchDispatchVM", runManagedSwitchDispatchVM, ManagedSwitchDispatchVM.Preprocess(fibonacciByteCode), stopWatch, times);
            var managedTailCallEmbeddedFibonacciTime = Run("ManagedTailCallEmbeddedVM", runManagedTailCallEmbeddedVM, ManagedTailCallEmbeddedVM.Preprocess(fibonacciByteCode), stopWatch, times);
            var nativeAddressOfLabelFibonacciTime = Run("NativeAddressOfLabelVM", runNativeAddressOfLabelVM, NativeAddressOfLabelVM.Preprocess(fibonacciByteCode), stopWatch, times);
            var nativeAddressOfLabelEmbeddedFibonacciTime = Run("NativeAddressOfLabelEmbeddedVM", runNativeAddressOfLabelEmbeddedVM, NativeAddressOfLabelEmbeddedVM.Preprocess(fibonacciByteCode), stopWatch, times);
            var nativeAddressOfLabelEmbeddedAlignedFibonacciTime = Run("NativeAddressOfLabelEmbeddedAlignedVM", runNativeAddressOfLabelEmbeddedAlignedVM, NativeAddressOfLabelEmbeddedAlignedVM.Preprocess(fibonacciByteCode), stopWatch, times);
            var nativeCalliFibonacciTime = Run("NativeCalliVM", runNativeCalliVM, NativeCalliVM.Preprocess(fibonacciByteCode), stopWatch, times);
            var nativeCalliEmbeddedFibonacciTime = Run("NativeCalliEmbeddedVM", runNativeCalliEmbeddedVM, NativeCalliEmbeddedVM.Preprocess(fibonacciByteCode), stopWatch, times);
            var nativeSwitchDispatchFibonacciTime = Run("NativeSwitchDispatchVM", runNativeSwitchDispatchVM, NativeSwitchDispatchVM.Preprocess(fibonacciByteCode), stopWatch, times);
            var luaTime = RunLua("LuaVM", runLuaVM, stopWatch, times);
            var evvmTime = Run("EVVM", runEvvm, evvmFibonacciByteCode, stopWatch, times);
            Console.WriteLine();

            // Console.WriteLine($"ManagedTailCallEmbeddedVM MixedOp is {(double)managedSwitchDispatchMixedOpTime / (double)managedTailCallEmbeddedMixedOpTime}x faster than ManagedSwitchDispatchVM");
            Console.WriteLine($"ManagedTailCallEmbeddedVM Fibonacci is {(double)managedSwitchDispatchFibonacciTime / (double)managedTailCallEmbeddedFibonacciTime}x faster than ManagedSwitchDispatchVM");
            Console.WriteLine();

            // Console.WriteLine($"NativeAddressOfLabelVM MixedOp is {(double)managedSwitchDispatchMixedOpTime / (double)nativeAddressOfLabelMixedOpTime}x faster than ManagedSwitchDispatchVM");
            Console.WriteLine($"NativeAddressOfLabelVM Fibonacci is {(double)managedSwitchDispatchFibonacciTime / (double)nativeAddressOfLabelFibonacciTime}x faster than ManagedSwitchDispatchVM");
            Console.WriteLine();

            // Console.WriteLine($"NativeAddressOfLabelEmbeddedVM MixedOp is {(double)managedSwitchDispatchMixedOpTime / (double)nativeAddressOfLabelEmbeddedMixedOpTime}x faster than ManagedSwitchDispatchVM");
            Console.WriteLine($"NativeAddressOfLabelEmbeddedVM Fibonacci is {(double)managedSwitchDispatchFibonacciTime / (double)nativeAddressOfLabelEmbeddedFibonacciTime}x faster than ManagedSwitchDispatchVM");
            Console.WriteLine();

            // Console.WriteLine($"NativeAddressOfLabelEmbeddedAlignedVM MixedOp is {(double)managedSwitchDispatchMixedOpTime / (double)nativeAddressOfLabelEmbeddedAlignedMixedOpTime}x faster than ManagedSwitchDispatchVM");
            Console.WriteLine($"NativeAddressOfLabelEmbeddedAlignedVM Fibonacci is {(double)managedSwitchDispatchFibonacciTime / (double)nativeAddressOfLabelEmbeddedAlignedFibonacciTime}x faster than ManagedSwitchDispatchVM");
            Console.WriteLine();
            // Console.WriteLine($"NativeCalliVM MixedOp is {(double)managedSwitchDispatchMixedOpTime / (double)nativeCalliMixedOpTime}x faster than ManagedSwitchDispatchVM");
            Console.WriteLine($"NativeCalliVM Fibonacci is {(double)managedSwitchDispatchFibonacciTime / (double)nativeCalliFibonacciTime}x faster than ManagedSwitchDispatchVM");
            Console.WriteLine();

            // Console.WriteLine($"NativeCalliEmbeddedVM MixedOp is {(double)managedSwitchDispatchMixedOpTime / (double)nativeCalliEmbeddedMixedOpTime}x faster than ManagedSwitchDispatchVM");
            Console.WriteLine($"NativeCalliEmbeddedVM Fibonacci is {(double)managedSwitchDispatchFibonacciTime / (double)nativeCalliEmbeddedFibonacciTime}x faster than ManagedSwitchDispatchVM");
            Console.WriteLine();

            // Console.WriteLine($"NativeSwitchDispatchVM MixedOp is {(double)managedSwitchDispatchMixedOpTime / (double)nativeSwitchDispatchMixedOpTime}x faster than ManagedSwitchDispatchVM");
            Console.WriteLine($"NativeSwitchDispatchVM Fibonacci is {(double)managedSwitchDispatchFibonacciTime / (double)nativeSwitchDispatchFibonacciTime}x faster than ManagedSwitchDispatchVM");
            Console.WriteLine();

            Console.WriteLine($"LuaVM Fibonacci is {(double)managedSwitchDispatchFibonacciTime / (double)luaTime}x faster than ManagedSwitchDispatchVM");
            Console.WriteLine();

            Console.WriteLine($"EVVM Fibonacci is {(double)managedSwitchDispatchFibonacciTime / (double)evvmTime}x faster than ManagedSwitchDispatchVM");
            Console.WriteLine();
        }

        private static Code GenMixedOp(int extraOps = 0)
        {
            var random = new Random((int)(DateTime.Now.Ticks & 0xFFFF));

            var factory = Factory
                .New()
                .Push(50)
                .Push(10)
                .Multiply()
                .Push(6)
                .Add()
                .Push(10)
                .Subtract()
                .Push(2)
                .Divide()
                .Push(500)
                .Add()
                .Push(-10)
                .Subtract()
                .Push(2)
                .Divide()
                .Push(60)
                .Multiply()
                .Push(500)
                .Add()
                .Push(-50)
                .Add();

            for (var i = 0; i < extraOps; ++i)
            {
                var num = random.Next(1, 10);
                var op = random.Next(0, 4);
                switch (op)
                {
                    case 0:
                        factory.Push(num).Add();
                        break;

                    case 1:
                        factory.Push(num).Subtract();
                        break;

                    case 2:
                        factory.Push(num / 4 + 1).Multiply();
                        break;

                    case 3:
                        factory.Push(num / 4 + 1).Divide();
                        break;

                }
            }

            return new Code(factory.End().ToArray());
        }

        private static Code GenFibonacci(int iterations)
        {
            var factory = Factory
                .New()                      // []
                                            // int iterations
                .Push(iterations)           // [iterations]
                                            // var current = 0;
                .Push(0)                    // [iterations, current]
                                            // var next = 1;
                .Push(1)                    // [iterations, current, next]
                                            // var i = 0;
                .Push(0)                    // [iterations, current, next, i]
                                            // i < iterations
                .Duplicate()                // [iterations, current, next, i, i]
                .Load(4);                   // [iterations, current, next, i, i, iterations]

            var addressOfTop = factory.CurrentAddress;

            factory
                // Replaced
                .BranchIfGreaterOrEqual(0);  // [iterations, current, next, i]

            var topTargetAddress = factory.CurrentAddress;

            factory
                                            // var lastCurrent = current;
                .Load(2)                    // [iterations, current, next, i, lastCurrent]
                                            // current = next;
                .Load(2)                    // [iterations, current, next, i, lastCurrent, next]
                .Duplicate()                // [iterations, current, next, i, lastCurrent, next, next]
                .Store(4)                   // [iterations, current, next, i, lastCurrent, next]
                                            // next = lastCurrent + next
                .Add()                      // [iterations, current, next, i, result]
                .Store(1)                   // [iterations, current, next, i]
                                            // ++i
                .Push(1)                    // [iterations, current, next, i, 1]
                .Add()                      // [iterations, current, next, i]
                                            // i < iterations
                .Duplicate()                // [iterations, current, next, i, i]
                .Load(4);                   // [iterations, current, next, i, i, iterations]

            var addressOfBottom = factory.CurrentAddress;

            factory
                // Replaced
                .BranchIfLess(0); // [iterations, current, next, i]

            var bottomTargetAddress = factory.CurrentAddress;

            factory
                // return current
                .Load(2)                    // [iterations, current, next, i, current]
                .End();

            factory.EmplaceInt(addressOfTop + 1, bottomTargetAddress);
            factory.EmplaceInt(addressOfBottom + 1, topTargetAddress);

            return new Code(factory.ToArray());
        }

        private static Code EvvmFiboacci(int iterations)
        {
            var factory = EvvmFactory.New()
                .CopyI32_I32i_I32r(iterations, 4)   // interations
                .CopyI32_I32i_I32r(0, 8)            // current
                .CopyI32_I32i_I32r(1, 12)           // next
                .CopyI32_I32i_I32r(0, 16);          // i

            var branchIfGreaterOrEqual = factory.CurrentAddress;
            factory
                .BranchIfGreaterOrEqualI32_I32r_I32r_I32i(16, 4, 0);

            var topTarget = factory.CurrentAddress;

            factory
                .CopyI32_I32r_I32r(8, 20)
                .CopyI32_I32r_I32r(12, 8)
                .AddI32_I32r_I32r_I32r(20, 8, 12)
                .AddI32_I32i_I32r_I32r(1, 16, 16);

            var branchIfLess = factory.CurrentAddress;

            factory
                .BranchIfLessI32_I32r_I32r_I32i(16, 4, topTarget);

            var bottomTarget = factory.CurrentAddress;

            factory
                .End();

            factory.EmplaceInt(branchIfGreaterOrEqual + 1 + sizeof(int) + sizeof(int), bottomTarget);
            factory.EmplaceInt(branchIfLess + 1 + sizeof(int) + sizeof(int), topTarget);

            return Evvm.Preprocess(new Code(factory.ToArray()));
        }

        private static long Run(string name, RunDelegate run, Code byteCode, Stopwatch stopwatch, int times)
        {
            stopwatch.Reset();

            Console.WriteLine($"{name}!");
            run(byteCode.Bytes);
            stopwatch.Start();
            for (var i = 0; i < times - 1; ++i)
            {
                run(byteCode.Bytes);
            }
            var result = run(byteCode.Bytes);
            var time = stopwatch.ElapsedTicks;
            stopwatch.Stop();

            Console.WriteLine($"{name} result: {result}, time: {time} ticks!");
            Console.WriteLine();

            return time;
        }

        private static long RunLua(string name, LuaDelegate run, Stopwatch stopwatch, int times)
        {
            stopwatch.Reset();

            Console.WriteLine($"{name}!");
            run();
            stopwatch.Start();
            for (var i = 0; i < times - 1; ++i)
            {
                run();
            }
            var result = run();
            var time = stopwatch.ElapsedTicks;
            stopwatch.Stop();

            Console.WriteLine($"{name} result: {result}, time: {time} ticks!");
            Console.WriteLine();

            return time;
        }
    }
}
