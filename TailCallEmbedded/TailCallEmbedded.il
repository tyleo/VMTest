.assembly extern System.Runtime
{
  .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A)
  .ver 5:0:0:0
}

.assembly TailCallEmbedded
{
    
}

.module TailCallEmbedded

.class public auto ansi sealed beforefieldinit TailCallEmbedded.VM extends [System.Runtime] System.Object
{
    .field private uint8 * _byteCode
    .field private initonly int32[] _stack
    .field private int32 _stackPointer
    .field private int32 _programCounter

    .method public hidebysig specialname instance int32 get_Last() cil managed
    {
        .maxstack 8
        ldarg.0
        ldfld int32[] TailCallEmbedded.VM::_stack
        ldarg.0
        ldfld int32 TailCallEmbedded.VM::_stackPointer
        ldelem.i4
        ret
    }

    .property instance int32 Last()
    {
        .get instance int32 TailCallEmbedded.VM::get_Last()
    }

    .method public hidebysig specialname rtspecialname instance void .ctor() cil managed
    {
        .maxstack 8
        ldarg.0
        ldc.i4 1000
        newarr [System.Runtime] System.Int32
        stfld int32[] TailCallEmbedded.VM::_stack
        ldarg.0
        call instance void [System.Runtime] System.Object::.ctor()
        ret
    }

    .method public hidebysig instance void Run(uint8 * byteCode) cil managed
    {
        .maxstack 2
        .locals init (method void * (class TailCallEmbedded.VM) V_0)
        ldarg.0
        ldarg.1
        stfld uint8 * TailCallEmbedded.VM::_byteCode
        ldarg.0
        ldc.i4.m1
        stfld int32 TailCallEmbedded.VM::_stackPointer
        ldarg.0
        ldc.i4.0
        stfld int32 TailCallEmbedded.VM::_programCounter
        ldarg.0
        ldfld uint8 * TailCallEmbedded.VM::_byteCode
        ldarg.0
        ldfld int32 TailCallEmbedded.VM::_programCounter
        add
        ldind.i
        stloc.0
        ldarg.0
        ldloc.0
        tail.
        calli void(class TailCallEmbedded.VM)
        ret
    }

    .method public hidebysig static void Add(class TailCallEmbedded.VM self) cil managed
    {
        .maxstack  5
        .locals init (method void * (class TailCallEmbedded.VM) V_0)
        ldarg.0
        ldfld int32[] TailCallEmbedded.VM::_stack
        ldarg.0
        ldfld int32 TailCallEmbedded.VM::_stackPointer
        ldc.i4.1
        sub
        ldarg.0
        ldfld int32[] TailCallEmbedded.VM::_stack
        ldarg.0
        ldfld int32 TailCallEmbedded.VM::_stackPointer
        ldc.i4.1
        sub
        ldelem.i4
        ldarg.0
        ldfld int32[] TailCallEmbedded.VM::_stack
        ldarg.0
        ldfld int32 TailCallEmbedded.VM::_stackPointer
        ldelem.i4
        add
        stelem.i4
        ldarg.0
        dup
        ldfld int32 TailCallEmbedded.VM::_stackPointer
        ldc.i4.1
        sub
        stfld int32 TailCallEmbedded.VM::_stackPointer
        ldarg.0
        dup
        ldfld int32 TailCallEmbedded.VM::_programCounter
        sizeof method void * (class TailCallEmbedded.VM)
        add
        stfld int32 TailCallEmbedded.VM::_programCounter
        ldarg.0
        ldfld uint8 * TailCallEmbedded.VM::_byteCode
        ldarg.0
        ldfld int32 TailCallEmbedded.VM::_programCounter
        add
        ldind.i
        stloc.0
        ldarg.0
        ldloc.0
        tail.
        calli void(class TailCallEmbedded.VM)
        ret
    }

    .method public hidebysig static void BranchIfLess(class TailCallEmbedded.VM self) cil managed
    {
        .maxstack  4
        .locals init (method void *(class TailCallEmbedded.VM) V_0)
        IL_0000:  ldarg.0
        IL_0001:  ldfld      int32[] TailCallEmbedded.VM::_stack
        IL_0006:  ldarg.0
        IL_0007:  ldfld      int32 TailCallEmbedded.VM::_stackPointer
        IL_000c:  ldc.i4.1
        IL_000d:  sub
        IL_000e:  ldelem.i4
        IL_000f:  ldarg.0
        IL_0010:  ldfld      int32[] TailCallEmbedded.VM::_stack
        IL_0015:  ldarg.0
        IL_0016:  ldfld      int32 TailCallEmbedded.VM::_stackPointer
        IL_001b:  ldelem.i4
        IL_001c:  clt
        IL_001e:  ldarg.0
        IL_001f:  dup
        IL_0020:  ldfld      int32 TailCallEmbedded.VM::_stackPointer
        IL_0025:  ldc.i4.2
        IL_0026:  sub
        IL_0027:  stfld      int32 TailCallEmbedded.VM::_stackPointer
        IL_002c:  brfalse.s  IL_004b
        IL_002e:  ldarg.0
        IL_002f:  ldarg.0
        IL_0030:  ldfld      uint8* TailCallEmbedded.VM::_byteCode
        IL_0035:  ldarg.0
        IL_0036:  ldfld      int32 TailCallEmbedded.VM::_programCounter
        IL_003b:  add
        IL_003c:  sizeof     method void *(class TailCallEmbedded.VM)
        IL_0042:  add
        IL_0043:  ldind.i4
        IL_0044:  stfld      int32 TailCallEmbedded.VM::_programCounter
        IL_0049:  br.s       IL_0060
        IL_004b:  ldarg.0
        IL_004c:  dup
        IL_004d:  ldfld      int32 TailCallEmbedded.VM::_programCounter
        IL_0052:  sizeof     method void *(class TailCallEmbedded.VM)
        IL_0058:  ldc.i4.4
        IL_0059:  add
        IL_005a:  add
        IL_005b:  stfld      int32 TailCallEmbedded.VM::_programCounter
        IL_0060:  ldarg.0
        IL_0061:  ldfld      uint8* TailCallEmbedded.VM::_byteCode
        IL_0066:  ldarg.0
        IL_0067:  ldfld      int32 TailCallEmbedded.VM::_programCounter
        IL_006c:  add
        IL_006d:  ldind.i
        IL_006e:  stloc.0
        IL_006f:  ldarg.0
        IL_0070:  ldloc.0
        tail.
        IL_0071:  calli      void(class TailCallEmbedded.VM)
        IL_0076:  ret
    }

    .method public hidebysig static void BranchIfGreaterOrEqual(class TailCallEmbedded.VM self) cil managed
    {
        .maxstack  4
        .locals init (method void *(class TailCallEmbedded.VM) V_0)
        IL_0000:  ldarg.0
        IL_0001:  ldfld      int32[] TailCallEmbedded.VM::_stack
        IL_0006:  ldarg.0
        IL_0007:  ldfld      int32 TailCallEmbedded.VM::_stackPointer
        IL_000c:  ldc.i4.1
        IL_000d:  sub
        IL_000e:  ldelem.i4
        IL_000f:  ldarg.0
        IL_0010:  ldfld      int32[] TailCallEmbedded.VM::_stack
        IL_0015:  ldarg.0
        IL_0016:  ldfld      int32 TailCallEmbedded.VM::_stackPointer
        IL_001b:  ldelem.i4
        IL_001c:  clt
        IL_001e:  ldc.i4.0
        IL_001f:  ceq
        IL_0021:  ldarg.0
        IL_0022:  dup
        IL_0023:  ldfld      int32 TailCallEmbedded.VM::_stackPointer
        IL_0028:  ldc.i4.2
        IL_0029:  sub
        IL_002a:  stfld      int32 TailCallEmbedded.VM::_stackPointer
        IL_002f:  brfalse.s  IL_004e
        IL_0031:  ldarg.0
        IL_0032:  ldarg.0
        IL_0033:  ldfld      uint8* TailCallEmbedded.VM::_byteCode
        IL_0038:  ldarg.0
        IL_0039:  ldfld      int32 TailCallEmbedded.VM::_programCounter
        IL_003e:  add
        IL_003f:  sizeof     method void *(class TailCallEmbedded.VM)
        IL_0045:  add
        IL_0046:  ldind.i4
        IL_0047:  stfld      int32 TailCallEmbedded.VM::_programCounter
        IL_004c:  br.s       IL_0063
        IL_004e:  ldarg.0
        IL_004f:  dup
        IL_0050:  ldfld      int32 TailCallEmbedded.VM::_programCounter
        IL_0055:  sizeof     method void *(class TailCallEmbedded.VM)
        IL_005b:  ldc.i4.4
        IL_005c:  add
        IL_005d:  add
        IL_005e:  stfld      int32 TailCallEmbedded.VM::_programCounter
        IL_0063:  ldarg.0
        IL_0064:  ldfld      uint8* TailCallEmbedded.VM::_byteCode
        IL_0069:  ldarg.0
        IL_006a:  ldfld      int32 TailCallEmbedded.VM::_programCounter
        IL_006f:  add
        IL_0070:  ldind.i
        IL_0071:  stloc.0
        IL_0072:  ldarg.0
        IL_0073:  ldloc.0
        tail.
        IL_0074:  calli      void(class TailCallEmbedded.VM)
        IL_0079:  ret
    }

    .method public hidebysig static void Divide(class TailCallEmbedded.VM self) cil managed
    {
        .maxstack  5
        .locals init (method void * (class TailCallEmbedded.VM) V_0)
        ldarg.0
        ldfld int32[] TailCallEmbedded.VM::_stack
        ldarg.0
        ldfld int32 TailCallEmbedded.VM::_stackPointer
        ldc.i4.1
        sub
        ldarg.0
        ldfld int32[] TailCallEmbedded.VM::_stack
        ldarg.0
        ldfld int32 TailCallEmbedded.VM::_stackPointer
        ldc.i4.1
        sub
        ldelem.i4
        ldarg.0
        ldfld int32[] TailCallEmbedded.VM::_stack
        ldarg.0
        ldfld int32 TailCallEmbedded.VM::_stackPointer
        ldelem.i4
        div
        stelem.i4
        ldarg.0
        dup
        ldfld int32 TailCallEmbedded.VM::_stackPointer
        ldc.i4.1
        sub
        stfld int32 TailCallEmbedded.VM::_stackPointer
        ldarg.0
        dup
        ldfld int32 TailCallEmbedded.VM::_programCounter
        sizeof method void * (class TailCallEmbedded.VM)
        add
        stfld int32 TailCallEmbedded.VM::_programCounter
        ldarg.0
        ldfld uint8 * TailCallEmbedded.VM::_byteCode
        ldarg.0
        ldfld int32 TailCallEmbedded.VM::_programCounter
        add
        ldind.i
        stloc.0
        ldarg.0
        ldloc.0
        tail.
        calli void(class TailCallEmbedded.VM)
        ret
    }

    .method public hidebysig static void Duplicate(class TailCallEmbedded.VM self) cil managed
    {
        .maxstack  5
        .locals init (method void *(class TailCallEmbedded.VM) V_0)
        IL_0000:  ldarg.0
        IL_0001:  dup
        IL_0002:  ldfld      int32 TailCallEmbedded.VM::_stackPointer
        IL_0007:  ldc.i4.1
        IL_0008:  add
        IL_0009:  stfld      int32 TailCallEmbedded.VM::_stackPointer
        IL_000e:  ldarg.0
        IL_000f:  ldfld      int32[] TailCallEmbedded.VM::_stack
        IL_0014:  ldarg.0
        IL_0015:  ldfld      int32 TailCallEmbedded.VM::_stackPointer
        IL_001a:  ldarg.0
        IL_001b:  ldfld      int32[] TailCallEmbedded.VM::_stack
        IL_0020:  ldarg.0
        IL_0021:  ldfld      int32 TailCallEmbedded.VM::_stackPointer
        IL_0026:  ldc.i4.1
        IL_0027:  sub
        IL_0028:  ldelem.i4
        IL_0029:  stelem.i4
        IL_002a:  ldarg.0
        IL_002b:  dup
        IL_002c:  ldfld      int32 TailCallEmbedded.VM::_programCounter
        IL_0031:  sizeof     method void *(class TailCallEmbedded.VM)
        IL_0037:  add
        IL_0038:  stfld      int32 TailCallEmbedded.VM::_programCounter
        IL_003d:  ldarg.0
        IL_003e:  ldfld      uint8* TailCallEmbedded.VM::_byteCode
        IL_0043:  ldarg.0
        IL_0044:  ldfld      int32 TailCallEmbedded.VM::_programCounter
        IL_0049:  add
        IL_004a:  ldind.i
        IL_004b:  stloc.0
        IL_004c:  ldarg.0
        IL_004d:  ldloc.0
        tail.
        IL_004e:  calli      void(class TailCallEmbedded.VM)
        IL_0053:  ret
    }

    .method public hidebysig static void End(class TailCallEmbedded.VM self) cil managed
    {
        .maxstack  8
        ldarg.0
        dup
        ldfld int32 TailCallEmbedded.VM::_programCounter
        sizeof method void * (class TailCallEmbedded.VM)
        add
        stfld int32 TailCallEmbedded.VM::_programCounter
        ret
    }

    .method public hidebysig static void Load(class TailCallEmbedded.VM self) cil managed
    {
        .maxstack  5
        .locals init (int32 V_0, method void *(class TailCallEmbedded.VM) V_1)
        IL_0000:  ldarg.0
        IL_0001:  ldfld      uint8* TailCallEmbedded.VM::_byteCode
        IL_0006:  ldarg.0
        IL_0007:  ldfld      int32 TailCallEmbedded.VM::_programCounter
        IL_000c:  add
        IL_000d:  sizeof     method void *(class TailCallEmbedded.VM)
        IL_0013:  add
        IL_0014:  ldind.i4
        IL_0015:  stloc.0
        IL_0016:  ldarg.0
        IL_0017:  dup
        IL_0018:  ldfld      int32 TailCallEmbedded.VM::_stackPointer
        IL_001d:  ldc.i4.1
        IL_001e:  add
        IL_001f:  stfld      int32 TailCallEmbedded.VM::_stackPointer
        IL_0024:  ldarg.0
        IL_0025:  ldfld      int32[] TailCallEmbedded.VM::_stack
        IL_002a:  ldarg.0
        IL_002b:  ldfld      int32 TailCallEmbedded.VM::_stackPointer
        IL_0030:  ldarg.0
        IL_0031:  ldfld      int32[] TailCallEmbedded.VM::_stack
        IL_0036:  ldarg.0
        IL_0037:  ldfld      int32 TailCallEmbedded.VM::_stackPointer
        IL_003c:  ldloc.0
        IL_003d:  sub
        IL_003e:  ldelem.i4
        IL_003f:  stelem.i4
        IL_0040:  ldarg.0
        IL_0041:  dup
        IL_0042:  ldfld      int32 TailCallEmbedded.VM::_programCounter
        IL_0047:  sizeof     method void *(class TailCallEmbedded.VM)
        IL_004d:  ldc.i4.4
        IL_004e:  add
        IL_004f:  add
        IL_0050:  stfld      int32 TailCallEmbedded.VM::_programCounter
        IL_0055:  ldarg.0
        IL_0056:  ldfld      uint8* TailCallEmbedded.VM::_byteCode
        IL_005b:  ldarg.0
        IL_005c:  ldfld      int32 TailCallEmbedded.VM::_programCounter
        IL_0061:  add
        IL_0062:  ldind.i
        IL_0063:  stloc.1
        IL_0064:  ldarg.0
        IL_0065:  ldloc.1
        tail.
        IL_0066:  calli      void(class TailCallEmbedded.VM)
        IL_006b:  ret
    }

    .method public hidebysig static void Multiply(class TailCallEmbedded.VM self) cil managed
    {
        .maxstack  5
        .locals init (method void * (class TailCallEmbedded.VM) V_0)
        ldarg.0
        ldfld int32[] TailCallEmbedded.VM::_stack
        ldarg.0
        ldfld int32 TailCallEmbedded.VM::_stackPointer
        ldc.i4.1
        sub
        ldarg.0
        ldfld int32[] TailCallEmbedded.VM::_stack
        ldarg.0
        ldfld int32 TailCallEmbedded.VM::_stackPointer
        ldc.i4.1
        sub
        ldelem.i4
        ldarg.0
        ldfld int32[] TailCallEmbedded.VM::_stack
        ldarg.0
        ldfld int32 TailCallEmbedded.VM::_stackPointer
        ldelem.i4
        mul
        stelem.i4
        ldarg.0
        dup
        ldfld int32 TailCallEmbedded.VM::_stackPointer
        ldc.i4.1
        sub
        stfld int32 TailCallEmbedded.VM::_stackPointer
        ldarg.0
        dup
        ldfld int32 TailCallEmbedded.VM::_programCounter
        sizeof method void * (class TailCallEmbedded.VM)
        add
        stfld int32 TailCallEmbedded.VM::_programCounter
        ldarg.0
        ldfld uint8 * TailCallEmbedded.VM::_byteCode
        ldarg.0
        ldfld int32 TailCallEmbedded.VM::_programCounter
        add
        ldind.i
        stloc.0
        ldarg.0
        ldloc.0
        tail.
        calli void(class TailCallEmbedded.VM)
        ret
    }

    .method public hidebysig static void NoOp(class TailCallEmbedded.VM self) cil managed
    {
        .maxstack  3
        .locals init (method void * (class TailCallEmbedded.VM) V_0)
        ldarg.0
        dup
        ldfld int32 TailCallEmbedded.VM::_programCounter
        sizeof method void * (class TailCallEmbedded.VM)
        add
        stfld int32 TailCallEmbedded.VM::_programCounter
        ldarg.0
        ldfld uint8 * TailCallEmbedded.VM::_byteCode
        ldarg.0
        ldfld int32 TailCallEmbedded.VM::_programCounter
        add
        ldind.i
        stloc.0
        ldarg.0
        ldloc.0
        tail.
        calli void(class TailCallEmbedded.VM)
        ret
    }

    .method public hidebysig static void Pop(class TailCallEmbedded.VM self) cil managed
    {
        .maxstack  3
        .locals init (method void * (class TailCallEmbedded.VM) V_0)
        ldarg.0
        dup
        ldfld int32 TailCallEmbedded.VM::_stackPointer
        ldc.i4.1
        sub
        stfld int32 TailCallEmbedded.VM::_stackPointer
        ldarg.0
        dup
        ldfld int32 TailCallEmbedded.VM::_programCounter
        sizeof method void * (class TailCallEmbedded.VM)
        add
        stfld int32 TailCallEmbedded.VM::_programCounter
        ldarg.0
        ldfld uint8 * TailCallEmbedded.VM::_byteCode
        ldarg.0
        ldfld int32 TailCallEmbedded.VM::_programCounter
        add
        ldind.i
        stloc.0
        ldarg.0
        ldloc.0
        tail.
        calli void(class TailCallEmbedded.VM)
        ret
    }

    .method public hidebysig static void Push(class TailCallEmbedded.VM self) cil managed
    {
        .maxstack  4
        .locals init (int32 V_0, method void * (class TailCallEmbedded.VM) V_1)
        ldarg.0
        ldfld uint8 * TailCallEmbedded.VM::_byteCode
        ldarg.0
        ldfld int32 TailCallEmbedded.VM::_programCounter
        add
        sizeof method void * (class TailCallEmbedded.VM)
        add
        ldind.i4
        stloc.0
        ldarg.0
        dup
        ldfld int32 TailCallEmbedded.VM::_stackPointer
        ldc.i4.1
        add
        stfld int32 TailCallEmbedded.VM::_stackPointer
        ldarg.0
        ldfld int32[] TailCallEmbedded.VM::_stack
        ldarg.0
        ldfld int32 TailCallEmbedded.VM::_stackPointer
        ldloc.0
        stelem.i4
        ldarg.0
        dup
        ldfld int32 TailCallEmbedded.VM::_programCounter
        sizeof method void * (class TailCallEmbedded.VM)
        ldc.i4.4
        add
        add
        stfld int32 TailCallEmbedded.VM::_programCounter
        ldarg.0
        ldfld uint8 * TailCallEmbedded.VM::_byteCode
        ldarg.0
        ldfld int32 TailCallEmbedded.VM::_programCounter
        add
        ldind.i
        stloc.1
        ldarg.0
        ldloc.1
        tail.
        calli void(class TailCallEmbedded.VM)
        ret
    }

    .method public hidebysig static void Store(class TailCallEmbedded.VM self) cil managed
    {
        .maxstack  4
        .locals init (int32 V_0, method void *(class TailCallEmbedded.VM) V_1)
        IL_0000:  ldarg.0
        IL_0001:  ldfld      uint8* TailCallEmbedded.VM::_byteCode
        IL_0006:  ldarg.0
        IL_0007:  ldfld      int32 TailCallEmbedded.VM::_programCounter
        IL_000c:  add
        IL_000d:  sizeof     method void *(class TailCallEmbedded.VM)
        IL_0013:  add
        IL_0014:  ldind.i4
        IL_0015:  stloc.0
        IL_0016:  ldarg.0
        IL_0017:  ldfld      int32[] TailCallEmbedded.VM::_stack
        IL_001c:  ldarg.0
        IL_001d:  ldfld      int32 TailCallEmbedded.VM::_stackPointer
        IL_0022:  ldloc.0
        IL_0023:  sub
        IL_0024:  ldarg.0
        IL_0025:  ldfld      int32[] TailCallEmbedded.VM::_stack
        IL_002a:  ldarg.0
        IL_002b:  ldfld      int32 TailCallEmbedded.VM::_stackPointer
        IL_0030:  ldelem.i4
        IL_0031:  stelem.i4
        IL_0032:  ldarg.0
        IL_0033:  dup
        IL_0034:  ldfld      int32 TailCallEmbedded.VM::_stackPointer
        IL_0039:  ldc.i4.1
        IL_003a:  sub
        IL_003b:  stfld      int32 TailCallEmbedded.VM::_stackPointer
        IL_0040:  ldarg.0
        IL_0041:  dup
        IL_0042:  ldfld      int32 TailCallEmbedded.VM::_programCounter
        IL_0047:  sizeof     method void *(class TailCallEmbedded.VM)
        IL_004d:  ldc.i4.4
        IL_004e:  add
        IL_004f:  add
        IL_0050:  stfld      int32 TailCallEmbedded.VM::_programCounter
        IL_0055:  ldarg.0
        IL_0056:  ldfld      uint8* TailCallEmbedded.VM::_byteCode
        IL_005b:  ldarg.0
        IL_005c:  ldfld      int32 TailCallEmbedded.VM::_programCounter
        IL_0061:  add
        IL_0062:  ldind.i
        IL_0063:  stloc.1
        IL_0064:  ldarg.0
        IL_0065:  ldloc.1
        tail.
        IL_0066:  calli      void(class TailCallEmbedded.VM)
        IL_006b:  ret
    }

    .method public hidebysig static void Subtract(class TailCallEmbedded.VM self) cil managed
    {
        .maxstack  5
        .locals init (method void * (class TailCallEmbedded.VM) V_0)
        ldarg.0
        ldfld int32[] TailCallEmbedded.VM::_stack
        ldarg.0
        ldfld int32 TailCallEmbedded.VM::_stackPointer
        ldc.i4.1
        sub
        ldarg.0
        ldfld int32[] TailCallEmbedded.VM::_stack
        ldarg.0
        ldfld int32 TailCallEmbedded.VM::_stackPointer
        ldc.i4.1
        sub
        ldelem.i4
        ldarg.0
        ldfld int32[] TailCallEmbedded.VM::_stack
        ldarg.0
        ldfld int32 TailCallEmbedded.VM::_stackPointer
        ldelem.i4
        sub
        stelem.i4
        ldarg.0
        dup
        ldfld int32 TailCallEmbedded.VM::_stackPointer
        ldc.i4.1
        sub
        stfld int32 TailCallEmbedded.VM::_stackPointer
        ldarg.0
        dup
        ldfld int32 TailCallEmbedded.VM::_programCounter
        sizeof method void * (class TailCallEmbedded.VM)
        add
        stfld int32 TailCallEmbedded.VM::_programCounter
        ldarg.0
        ldfld uint8 * TailCallEmbedded.VM::_byteCode
        ldarg.0
        ldfld int32 TailCallEmbedded.VM::_programCounter
        add
        ldind.i
        stloc.0
        ldarg.0
        ldloc.0
        tail.
        calli void(class TailCallEmbedded.VM)
        ret
    }
}
