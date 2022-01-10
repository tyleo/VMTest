#include "VM.h"

void InitFunctionAddresses();
void InitEmbeddedFunctionAddresses();
void InitLabelAddresses();
void InitEmbeddedLabelAddresses();
void InitEmbeddedAlignedLabelAddresses();

void (* VM::FunctionAddresses[(int)Op::Size]) (VM & self, uint8_t const * byteCode) = { nullptr };

void (* VM::EmbeddedFunctionAddresses[(int)Op::Size]) (VM & self, uint8_t const * byteCode) = { nullptr };

void const * VM::LabelAddresses[(int)Op::Size] = { nullptr };

void const * VM::EmbeddedLabelAddresses[(int)Op::Size] = { nullptr };

void const * VM::EmbeddedAlignedLabelAddresses[(int)Op::Size] = { nullptr };

void VMStaticInit()
{
    InitFunctionAddresses();
    InitEmbeddedFunctionAddresses();
    InitLabelAddresses();
    InitEmbeddedLabelAddresses();
    InitEmbeddedAlignedLabelAddresses();
}

void const * const * VMGetFunctionAddresses()
{
    return reinterpret_cast<void const * const *>(VM::FunctionAddresses);
}

void const * const * VMGetEmbeddedFunctionAddresses()
{
    return reinterpret_cast<void const * const *>(VM::EmbeddedFunctionAddresses);
}

void const * const * VMGetLabelAddresses()
{
    return (void const* const*)VM::LabelAddresses;
}

void const * const * VMGetEmbeddedLabelAddresses()
{
    return (void const* const*)VM::EmbeddedLabelAddresses;
}

void const * const * VMGetEmbeddedAlignedLabelAddresses()
{
    return (void const* const*)VM::EmbeddedAlignedLabelAddresses;
}

void* VMNew()
{
    return new VM{};
}

void VMDelete(void * const pSelf)
{
    auto const pSelfTyped = static_cast<VM * const>(pSelf);
    delete pSelfTyped;
}

int VMGetLast(void const * const pSelf)
{
    auto& self = *static_cast<VM const * const>(pSelf);
    return self.Stack[self.StackPointer];
}

#pragma region Calli

void NoOp(VM& self, uint8_t const* byteCode)
{
    self.ProgramCounter += sizeof(uint8_t);
    VM::FunctionAddresses[byteCode[self.ProgramCounter]](self, byteCode);
}

void Push(VM& self, uint8_t const* byteCode)
{
    auto const value = *reinterpret_cast<int32_t const*>(byteCode + self.ProgramCounter + sizeof(uint8_t));
    ++self.StackPointer;
    self.Stack[self.StackPointer] = value;
    self.ProgramCounter += (sizeof(uint8_t) + sizeof(int32_t));
    VM::FunctionAddresses[byteCode[self.ProgramCounter]](self, byteCode);
}

void Pop(VM& self, uint8_t const* byteCode)
{
    --self.StackPointer;
    self.ProgramCounter += sizeof(uint8_t);
    VM::FunctionAddresses[byteCode[self.ProgramCounter]](self, byteCode);
}

void Add(VM& self, uint8_t const* byteCode)
{
    self.Stack[self.StackPointer - 1] = self.Stack[self.StackPointer - 1] + self.Stack[self.StackPointer];
    --self.StackPointer;
    self.ProgramCounter += sizeof(uint8_t);
    VM::FunctionAddresses[byteCode[self.ProgramCounter]](self, byteCode);
}

void Subtract(VM& self, uint8_t const* byteCode)
{
    self.Stack[self.StackPointer - 1] = self.Stack[self.StackPointer - 1] - self.Stack[self.StackPointer];
    --self.StackPointer;
    self.ProgramCounter += sizeof(uint8_t);
    VM::FunctionAddresses[byteCode[self.ProgramCounter]](self, byteCode);
}

void Multiply(VM& self, uint8_t const* byteCode)
{
    self.Stack[self.StackPointer - 1] = self.Stack[self.StackPointer - 1] * self.Stack[self.StackPointer];
    --self.StackPointer;
    self.ProgramCounter += sizeof(uint8_t);
    VM::FunctionAddresses[byteCode[self.ProgramCounter]](self, byteCode);
}

void Divide(VM& self, uint8_t const* byteCode)
{
    self.Stack[self.StackPointer - 1] = self.Stack[self.StackPointer - 1] / self.Stack[self.StackPointer];
    --self.StackPointer;
    self.ProgramCounter += sizeof(uint8_t);
    VM::FunctionAddresses[byteCode[self.ProgramCounter]](self, byteCode);
}

void BranchIfLess(VM& self, uint8_t const* byteCode)
{
    auto const less = self.Stack[self.StackPointer - 1] < self.Stack[self.StackPointer];
    self.StackPointer -= 2;
    if (less)
    {
        self.ProgramCounter = *reinterpret_cast<int32_t const *>(byteCode + self.ProgramCounter + sizeof(uint8_t));
    }
    else
    {
        self.ProgramCounter += (sizeof(uint8_t) + sizeof(int32_t));
    }
    VM::FunctionAddresses[byteCode[self.ProgramCounter]](self, byteCode);
}

void BranchIfGreaterOrEqual(VM& self, uint8_t const* byteCode)
{
    auto const greaterOrEqual = self.Stack[self.StackPointer - 1] >= self.Stack[self.StackPointer];
    self.StackPointer -= 2;
    if (greaterOrEqual)
    {
        self.ProgramCounter = *reinterpret_cast<int32_t const*>((byteCode + self.ProgramCounter + sizeof(uint8_t)));
    }
    else
    {
        self.ProgramCounter += (sizeof(uint8_t) + sizeof(int32_t));
    }
    VM::FunctionAddresses[byteCode[self.ProgramCounter]](self, byteCode);
}

void Duplicate(VM& self, uint8_t const* byteCode)
{
    ++self.StackPointer;
    self.Stack[self.StackPointer] = self.Stack[self.StackPointer - 1];
    self.ProgramCounter += sizeof(uint8_t);
    VM::FunctionAddresses[byteCode[self.ProgramCounter]](self, byteCode);
}

void Load(VM& self, uint8_t const* byteCode)
{
    auto const loadFrom = *reinterpret_cast<int32_t const*>(byteCode + self.ProgramCounter + sizeof(uint8_t));
    ++self.StackPointer;
    self.Stack[self.StackPointer] = self.Stack[self.StackPointer - loadFrom];
    self.ProgramCounter += (sizeof(uint8_t) + sizeof(int32_t));
    VM::FunctionAddresses[byteCode[self.ProgramCounter]](self, byteCode);
}

void Store(VM& self, uint8_t const* byteCode)
{
    auto const storeTo = *reinterpret_cast<int32_t const*>(byteCode + self.ProgramCounter + sizeof(uint8_t));
    self.Stack[self.StackPointer - storeTo] = self.Stack[self.StackPointer];
    --self.StackPointer;
    self.ProgramCounter += (sizeof(uint8_t) + sizeof(int32_t));
    VM::FunctionAddresses[byteCode[self.ProgramCounter]](self, byteCode);
}

void End(VM & self, uint8_t const* byteCode)
{
    self.ProgramCounter += sizeof(uint8_t);
}

void VMCalliRun(void* pSelf, uint8_t const* byteCode)
{
    auto& self = *static_cast<VM* const>(pSelf);
    self.ProgramCounter = 0;
    self.StackPointer = -1;

    VM::FunctionAddresses[byteCode[self.ProgramCounter]](self, byteCode);
}

void InitFunctionAddresses()
{
    VM::FunctionAddresses[(int)Op::NoOp] = NoOp;
    VM::FunctionAddresses[(int)Op::Push] = Push;
    VM::FunctionAddresses[(int)Op::Pop] = Pop;
    VM::FunctionAddresses[(int)Op::Add] = Add;
    VM::FunctionAddresses[(int)Op::Subtract] = Subtract;
    VM::FunctionAddresses[(int)Op::Multiply] = Multiply;
    VM::FunctionAddresses[(int)Op::Divide] = Divide;
    VM::FunctionAddresses[(int)Op::BranchIfLess] = BranchIfLess;
    VM::FunctionAddresses[(int)Op::BranchIfGreaterOrEqual] = BranchIfGreaterOrEqual;
    VM::FunctionAddresses[(int)Op::Duplicate] = Duplicate;
    VM::FunctionAddresses[(int)Op::Load] = Load;
    VM::FunctionAddresses[(int)Op::Store] = Store;
    VM::FunctionAddresses[(int)Op::End] = End;
}

#pragma endregion Calli

#pragma region CalliEmbedded

void NoOpEmbedded(VM& self, uint8_t const* byteCode)
{
    self.ProgramCounter += sizeof(void *);
    reinterpret_cast<VM::RunDelegate*>(*reinterpret_cast<void * const *>(byteCode + self.ProgramCounter))(self, byteCode);
}

void PushEmbedded(VM& self, uint8_t const* byteCode)
{
    auto const value = *reinterpret_cast<int32_t const*>(byteCode + self.ProgramCounter + sizeof(void *));
    ++self.StackPointer;
    self.Stack[self.StackPointer] = value;
    self.ProgramCounter += (sizeof(void *) + sizeof(int32_t));
    reinterpret_cast<VM::RunDelegate*>(*reinterpret_cast<void * const *>(byteCode + self.ProgramCounter))(self, byteCode);
}

void PopEmbedded(VM& self, uint8_t const* byteCode)
{
    --self.StackPointer;
    self.ProgramCounter += sizeof(void *);
    reinterpret_cast<VM::RunDelegate*>(*reinterpret_cast<void * const *>(byteCode + self.ProgramCounter))(self, byteCode);
}

void AddEmbedded(VM& self, uint8_t const* byteCode)
{
    self.Stack[self.StackPointer - 1] = self.Stack[self.StackPointer - 1] + self.Stack[self.StackPointer];
    --self.StackPointer;
    self.ProgramCounter += sizeof(void *);
    reinterpret_cast<VM::RunDelegate*>(*reinterpret_cast<void * const *>(byteCode + self.ProgramCounter))(self, byteCode);
}

void SubtractEmbedded(VM& self, uint8_t const* byteCode)
{
    self.Stack[self.StackPointer - 1] = self.Stack[self.StackPointer - 1] - self.Stack[self.StackPointer];
    --self.StackPointer;
    self.ProgramCounter += sizeof(void *);
    reinterpret_cast<VM::RunDelegate*>(*reinterpret_cast<void * const *>(byteCode + self.ProgramCounter))(self, byteCode);
}

void MultiplyEmbedded(VM& self, uint8_t const* byteCode)
{
    self.Stack[self.StackPointer - 1] = self.Stack[self.StackPointer - 1] * self.Stack[self.StackPointer];
    --self.StackPointer;
    self.ProgramCounter += sizeof(void *);
    reinterpret_cast<VM::RunDelegate*>(*reinterpret_cast<void * const *>(byteCode + self.ProgramCounter))(self, byteCode);
}

void DivideEmbedded(VM& self, uint8_t const* byteCode)
{
    self.Stack[self.StackPointer - 1] = self.Stack[self.StackPointer - 1] / self.Stack[self.StackPointer];
    --self.StackPointer;
    self.ProgramCounter += sizeof(void *);
    reinterpret_cast<VM::RunDelegate*>(*reinterpret_cast<void * const *>(byteCode + self.ProgramCounter))(self, byteCode);
}

void BranchIfLessEmbedded(VM& self, uint8_t const* byteCode)
{
    auto const less = self.Stack[self.StackPointer - 1] < self.Stack[self.StackPointer];
    self.StackPointer -= 2;
    if (less)
    {
        self.ProgramCounter = *reinterpret_cast<int32_t const*>(byteCode + self.ProgramCounter + sizeof(void *));
    }
    else
    {
        self.ProgramCounter += (sizeof(void *) + sizeof(int32_t));
    }
    reinterpret_cast<VM::RunDelegate*>(*reinterpret_cast<void * const *>(byteCode + self.ProgramCounter))(self, byteCode);
}

void BranchIfGreaterOrEqualEmbedded(VM& self, uint8_t const* byteCode)
{
    auto const greaterOrEqual = self.Stack[self.StackPointer - 1] >= self.Stack[self.StackPointer];
    self.StackPointer -= 2;
    if (greaterOrEqual)
    {
        self.ProgramCounter = *reinterpret_cast<int32_t const*>(byteCode + self.ProgramCounter + sizeof(void *));
    }
    else
    {
        self.ProgramCounter += (sizeof(void *) + sizeof(int32_t));
    }
    reinterpret_cast<VM::RunDelegate*>(*reinterpret_cast<void * const *>(byteCode + self.ProgramCounter))(self, byteCode);
}

void DuplicateEmbedded(VM& self, uint8_t const* byteCode)
{
    ++self.StackPointer;
    self.Stack[self.StackPointer] = self.Stack[self.StackPointer - 1];
    self.ProgramCounter += sizeof(void *);
    reinterpret_cast<VM::RunDelegate*>(*reinterpret_cast<void * const *>(byteCode + self.ProgramCounter))(self, byteCode);
}

void LoadEmbedded(VM& self, uint8_t const* byteCode)
{
    auto const loadFrom = *reinterpret_cast<int32_t const*>(byteCode + self.ProgramCounter + sizeof(void *));
    ++self.StackPointer;
    self.Stack[self.StackPointer] = self.Stack[self.StackPointer - loadFrom];
    self.ProgramCounter += (sizeof(void *) + sizeof(int32_t));
    reinterpret_cast<VM::RunDelegate*>(*reinterpret_cast<void * const *>(byteCode + self.ProgramCounter))(self, byteCode);
}

void StoreEmbedded(VM& self, uint8_t const* byteCode)
{
    auto const storeTo = *reinterpret_cast<int32_t const*>(byteCode + self.ProgramCounter + sizeof(void *));
    self.Stack[self.StackPointer - storeTo] = self.Stack[self.StackPointer];
    --self.StackPointer;
    self.ProgramCounter += (sizeof(void *) + sizeof(int32_t));
    reinterpret_cast<VM::RunDelegate*>(*reinterpret_cast<void * const *>(byteCode + self.ProgramCounter))(self, byteCode);
}

void EndEmbedded(VM& self, uint8_t const* byteCode)
{
    self.ProgramCounter += sizeof(void *);
}

void VMCalliEmbeddedRun(void* pSelf, uint8_t const* byteCode)
{
    auto& self = *static_cast<VM* const>(pSelf);
    self.ProgramCounter = 0;
    self.StackPointer = -1;

    reinterpret_cast<VM::RunDelegate*>(*reinterpret_cast<void * const *>(byteCode + self.ProgramCounter))(self, byteCode);
}

void InitEmbeddedFunctionAddresses()
{
    VM::EmbeddedFunctionAddresses[(int)Op::NoOp] = NoOpEmbedded;
    VM::EmbeddedFunctionAddresses[(int)Op::Push] = PushEmbedded;
    VM::EmbeddedFunctionAddresses[(int)Op::Pop] = PopEmbedded;
    VM::EmbeddedFunctionAddresses[(int)Op::Add] = AddEmbedded;
    VM::EmbeddedFunctionAddresses[(int)Op::Subtract] = SubtractEmbedded;
    VM::EmbeddedFunctionAddresses[(int)Op::Multiply] = MultiplyEmbedded;
    VM::EmbeddedFunctionAddresses[(int)Op::Divide] = DivideEmbedded;
    VM::EmbeddedFunctionAddresses[(int)Op::BranchIfLess] = BranchIfLessEmbedded;
    VM::EmbeddedFunctionAddresses[(int)Op::BranchIfGreaterOrEqual] = BranchIfGreaterOrEqualEmbedded;
    VM::EmbeddedFunctionAddresses[(int)Op::Duplicate] = DuplicateEmbedded;
    VM::EmbeddedFunctionAddresses[(int)Op::Load] = LoadEmbedded;
    VM::EmbeddedFunctionAddresses[(int)Op::Store] = StoreEmbedded;
    VM::EmbeddedFunctionAddresses[(int)Op::End] = EndEmbedded;
}

#pragma endregion CalliEmbedded

#pragma region AddressOfLabel

void VMAddressOfLabelRun(void * const pSelf, uint8_t const * byteCode)
{
    if (pSelf == nullptr)
    {
        VM::LabelAddresses[(int)Op::NoOp] = &&L_NoOp;
        VM::LabelAddresses[(int)Op::Push] = &&L_Push;
        VM::LabelAddresses[(int)Op::Pop] = &&L_Pop;
        VM::LabelAddresses[(int)Op::Add] = &&L_Add;
        VM::LabelAddresses[(int)Op::Subtract] = &&L_Subtract;
        VM::LabelAddresses[(int)Op::Multiply] = &&L_Multiply;
        VM::LabelAddresses[(int)Op::Divide] = &&L_Divide;
        VM::LabelAddresses[(int)Op::BranchIfLess] = &&L_BranchIfLess;
        VM::LabelAddresses[(int)Op::BranchIfGreaterOrEqual] = &&L_BranchIfGreaterOrEqual;
        VM::LabelAddresses[(int)Op::Duplicate] = &&L_Duplicate;
        VM::LabelAddresses[(int)Op::Load] = &&L_Load;
        VM::LabelAddresses[(int)Op::Store] = &&L_Store;
        VM::LabelAddresses[(int)Op::End] = &&L_End;
        return;
    }

    auto & self = *static_cast<VM * const>(pSelf);
    self.ProgramCounter = 0;
    self.StackPointer = -1;

    goto *VM::LabelAddresses[byteCode[self.ProgramCounter]];

L_NoOp:
    {
        self.ProgramCounter += sizeof(uint8_t);
    }
    goto *VM::LabelAddresses[byteCode[self.ProgramCounter]];

L_Push:
    {
        auto const value = *reinterpret_cast<int32_t const*>(byteCode + self.ProgramCounter + sizeof(uint8_t));
        ++self.StackPointer;
        self.Stack[self.StackPointer] = value;
        self.ProgramCounter += (sizeof(uint8_t) + sizeof(int32_t));
    }
    goto *VM::LabelAddresses[byteCode[self.ProgramCounter]];

L_Pop:
    {
        --self.StackPointer;
        self.ProgramCounter += sizeof(uint8_t);
    }
    goto *VM::LabelAddresses[byteCode[self.ProgramCounter]];

L_Add:
    {
        self.Stack[self.StackPointer - 1] = self.Stack[self.StackPointer - 1] + self.Stack[self.StackPointer];
        --self.StackPointer;
        self.ProgramCounter += sizeof(uint8_t);
    }
    goto *VM::LabelAddresses[byteCode[self.ProgramCounter]];

L_Subtract:
    {
        self.Stack[self.StackPointer - 1] = self.Stack[self.StackPointer - 1] - self.Stack[self.StackPointer];
        --self.StackPointer;
        self.ProgramCounter += sizeof(uint8_t);
    }
    goto *VM::LabelAddresses[byteCode[self.ProgramCounter]];

L_Multiply:
    {
        self.Stack[self.StackPointer - 1] = self.Stack[self.StackPointer - 1] * self.Stack[self.StackPointer];
        --self.StackPointer;
        self.ProgramCounter += sizeof(uint8_t);
    }
    goto *VM::LabelAddresses[byteCode[self.ProgramCounter]];

L_Divide:
    {
        self.Stack[self.StackPointer - 1] = self.Stack[self.StackPointer - 1] / self.Stack[self.StackPointer];
        --self.StackPointer;
        self.ProgramCounter += sizeof(uint8_t);
    }
    goto *VM::LabelAddresses[byteCode[self.ProgramCounter]];


L_BranchIfLess:
    {
        auto const less = self.Stack[self.StackPointer - 1] < self.Stack[self.StackPointer];
        self.StackPointer -= 2;
        if (less)
        {
            self.ProgramCounter = *reinterpret_cast<int32_t const*>(byteCode + self.ProgramCounter + sizeof(uint8_t));
        }
        else
        {
            self.ProgramCounter += (sizeof(uint8_t) + sizeof(int32_t));
        }
    }
    goto *VM::LabelAddresses[byteCode[self.ProgramCounter]];

L_BranchIfGreaterOrEqual:
    {
        auto const greaterOrEqual = self.Stack[self.StackPointer - 1] >= self.Stack[self.StackPointer];
        self.StackPointer -= 2;
        if (greaterOrEqual)
        {
            self.ProgramCounter = *reinterpret_cast<int32_t const*>(byteCode + self.ProgramCounter + sizeof(uint8_t));
        }
        else
        {
            self.ProgramCounter += (sizeof(uint8_t) + sizeof(int32_t));
        }
    }
    goto *VM::LabelAddresses[byteCode[self.ProgramCounter]];

L_Duplicate:
    {
        ++self.StackPointer;
        self.Stack[self.StackPointer] = self.Stack[self.StackPointer - 1];
        self.ProgramCounter += sizeof(uint8_t);
    }
    goto *VM::LabelAddresses[byteCode[self.ProgramCounter]];

L_Load:
    {
        auto const loadFrom = *reinterpret_cast<int32_t const*>(byteCode + self.ProgramCounter + sizeof(uint8_t));
        ++self.StackPointer;
        self.Stack[self.StackPointer] = self.Stack[self.StackPointer - loadFrom];
        self.ProgramCounter += (sizeof(uint8_t) + sizeof(int32_t));
    }
    goto *VM::LabelAddresses[byteCode[self.ProgramCounter]];

L_Store:
    {
        auto const storeTo = *reinterpret_cast<int32_t const*>(byteCode + self.ProgramCounter + sizeof(uint8_t));
        self.Stack[self.StackPointer - storeTo] = self.Stack[self.StackPointer];
        --self.StackPointer;
        self.ProgramCounter += (sizeof(uint8_t) + sizeof(int32_t));
    }
    goto *VM::LabelAddresses[byteCode[self.ProgramCounter]];

L_End:
    self.ProgramCounter += sizeof(uint8_t);
    return;
}

void InitLabelAddresses()
{
    VMAddressOfLabelRun(nullptr, nullptr);
}

#pragma endregion AddressOfLabel

#pragma region AddressOfLabelEmbedded

template <int Int32ToPtrOffset, typename T>
void VMAddressOfLabelEmbeddedImpl(void* pSelf, uint8_t const* byteCode)
{
    if (pSelf == nullptr)
    {
        auto labelAddresses = T::GetLabelAddresses();
        labelAddresses[(int)Op::NoOp] = && L_NoOp;
        labelAddresses[(int)Op::Push] = && L_Push;
        labelAddresses[(int)Op::Pop] = && L_Pop;
        labelAddresses[(int)Op::Add] = && L_Add;
        labelAddresses[(int)Op::Subtract] = && L_Subtract;
        labelAddresses[(int)Op::Multiply] = && L_Multiply;
        labelAddresses[(int)Op::Divide] = && L_Divide;
        labelAddresses[(int)Op::BranchIfLess] = && L_BranchIfLess;
        labelAddresses[(int)Op::BranchIfGreaterOrEqual] = && L_BranchIfGreaterOrEqual;
        labelAddresses[(int)Op::Duplicate] = && L_Duplicate;
        labelAddresses[(int)Op::Load] = && L_Load;
        labelAddresses[(int)Op::Store] = && L_Store;
        labelAddresses[(int)Op::End] = && L_End;
        return;
    }

    auto & self = *static_cast<VM* const>(pSelf);
    self.ProgramCounter = 0;
    self.StackPointer = -1;

    goto* *reinterpret_cast<void * const*>(byteCode + self.ProgramCounter);

L_NoOp:
    {
        self.ProgramCounter += sizeof(void *);
    }
    goto* *reinterpret_cast<void * const *>(byteCode + self.ProgramCounter);

L_Push:
    {
        auto const value = *reinterpret_cast<int32_t const*>(byteCode + self.ProgramCounter + sizeof(void *));
        ++self.StackPointer;
        self.Stack[self.StackPointer] = value;
        self.ProgramCounter += (sizeof(void *) + sizeof(int32_t) + Int32ToPtrOffset);
    }
    goto* *reinterpret_cast<void * const *>(byteCode + self.ProgramCounter);

L_Pop:
    {
        --self.StackPointer;
        self.ProgramCounter += sizeof(void *);
    }
    goto* *reinterpret_cast<void * const *>(byteCode + self.ProgramCounter);

L_Add:
    {
        self.Stack[self.StackPointer - 1] = self.Stack[self.StackPointer - 1] + self.Stack[self.StackPointer];
        --self.StackPointer;
        self.ProgramCounter += sizeof(void *);
    }
    goto* *reinterpret_cast<void * const *>(byteCode + self.ProgramCounter);

L_Subtract:
    {
        self.Stack[self.StackPointer - 1] = self.Stack[self.StackPointer - 1] - self.Stack[self.StackPointer];
        --self.StackPointer;
        self.ProgramCounter += sizeof(void *);
    }
    goto* *reinterpret_cast<void * const *>(byteCode + self.ProgramCounter);

L_Multiply:
    {
        self.Stack[self.StackPointer - 1] = self.Stack[self.StackPointer - 1] * self.Stack[self.StackPointer];
        --self.StackPointer;
        self.ProgramCounter += sizeof(void *);
    }
    goto* *reinterpret_cast<void * const *>(byteCode + self.ProgramCounter);

L_Divide:
    {
        self.Stack[self.StackPointer - 1] = self.Stack[self.StackPointer - 1] / self.Stack[self.StackPointer];
        --self.StackPointer;
        self.ProgramCounter += sizeof(void *);
    }
    goto* *reinterpret_cast<void * const *>(byteCode + self.ProgramCounter);

L_BranchIfLess:
    {
        auto const less = self.Stack[self.StackPointer - 1] < self.Stack[self.StackPointer];
        self.StackPointer -= 2;
        if (less)
        {
            self.ProgramCounter = *reinterpret_cast<int32_t const*>(byteCode + self.ProgramCounter + sizeof(void*));
        }
        else
        {
            self.ProgramCounter += (sizeof(void*) + sizeof(int32_t) + Int32ToPtrOffset);
        }
    }
    goto** (void const**)(byteCode + self.ProgramCounter);

L_BranchIfGreaterOrEqual:
    {
        auto const greaterOrEqual = self.Stack[self.StackPointer - 1] >= self.Stack[self.StackPointer];
        self.StackPointer -= 2;
        if (greaterOrEqual)
        {
            self.ProgramCounter = *reinterpret_cast<int32_t const*>(byteCode + self.ProgramCounter + sizeof(void *));
        }
        else
        {
            self.ProgramCounter += (sizeof(void*) + sizeof(int32_t) + Int32ToPtrOffset);
        }
    }
    goto* *reinterpret_cast<void * const *>(byteCode + self.ProgramCounter);

L_Duplicate:
    {
        ++self.StackPointer;
        self.Stack[self.StackPointer] = self.Stack[self.StackPointer - 1];
        self.ProgramCounter += sizeof(void *);
    }
    goto* *reinterpret_cast<void * const *>(byteCode + self.ProgramCounter);

L_Load:
    {
        auto const loadFrom = *reinterpret_cast<int32_t const*>(byteCode + self.ProgramCounter + sizeof(void *));
        ++self.StackPointer;
        self.Stack[self.StackPointer] = self.Stack[self.StackPointer - loadFrom];
        self.ProgramCounter += (sizeof(void*) + sizeof(int32_t) + Int32ToPtrOffset);
    }
    goto* *reinterpret_cast<void * const *>(byteCode + self.ProgramCounter);

L_Store:
    {
        auto const storeTo = *reinterpret_cast<int32_t const*>(byteCode + self.ProgramCounter + sizeof(void *));
        self.Stack[self.StackPointer - storeTo] = self.Stack[self.StackPointer];
        --self.StackPointer;
        self.ProgramCounter += (sizeof(void*) + sizeof(int32_t) + Int32ToPtrOffset);
    }
    goto* *reinterpret_cast<void * const *>(byteCode + self.ProgramCounter);

L_End:
    self.ProgramCounter += sizeof(void *);
    return;
}

struct GetAddressOfLabelEmbeddedConfig
{
    static void const** GetLabelAddresses()
    {
        return VM::EmbeddedLabelAddresses;
    }
};

struct GetAddressOfLabelAlignedEmbeddedConfig
{
    static void const** GetLabelAddresses()
    {
        return VM::EmbeddedAlignedLabelAddresses;
    }
};

void VMAddressOfLabelEmbeddedRun(void* pSelf, uint8_t const* byteCode)
{
    VMAddressOfLabelEmbeddedImpl<0, GetAddressOfLabelEmbeddedConfig>(pSelf, byteCode);
}

void VMAddressOfLabelEmbeddedAlignedRun(void* pSelf, uint8_t const* byteCode)
{
    VMAddressOfLabelEmbeddedImpl<((int32_t)(sizeof(void*) - sizeof(int32_t))) & 0x7FFFFF, GetAddressOfLabelAlignedEmbeddedConfig>(pSelf, byteCode);
}

void InitEmbeddedLabelAddresses()
{
    VMAddressOfLabelEmbeddedImpl<0, GetAddressOfLabelEmbeddedConfig>(nullptr, nullptr);
}

void InitEmbeddedAlignedLabelAddresses()
{
    VMAddressOfLabelEmbeddedImpl<((int32_t)(sizeof(void*) - sizeof(int32_t))) & 0x7FFFFF, GetAddressOfLabelAlignedEmbeddedConfig>(nullptr, nullptr);
}

#pragma endregion AddressOfLabelEmbedded

#pragma region SwitchDispatch

void VMSwitchDispatchRun(void * const pSelf, uint8_t const * const byteCode)
{
    auto & self = *static_cast<VM * const>(pSelf);
    self.StackPointer = -1;
    self.ProgramCounter = 0;

    for (;;)
    {
        auto const instruction = (Op)byteCode[self.ProgramCounter];
        switch (instruction)
        {
            case Op::NoOp:
            {
                self.ProgramCounter += sizeof(uint8_t);
            }
            break;

            case Op::Push:
            {
                auto const value = *reinterpret_cast<int32_t const * const>(byteCode + self.ProgramCounter + sizeof(uint8_t));
                ++self.StackPointer;
                self.Stack[self.StackPointer] = value;
                self.ProgramCounter += (sizeof(uint8_t) + sizeof(int32_t));
            }
            break;

            case Op::Pop:
            {
                --self.StackPointer;
                self.ProgramCounter += sizeof(uint8_t);
            }
            break;

            case Op::Add:
            {
                self.Stack[self.StackPointer - 1] = self.Stack[self.StackPointer - 1] + self.Stack[self.StackPointer];
                --self.StackPointer;
                self.ProgramCounter += sizeof(uint8_t);
            }
            break;

            case Op::Subtract:
            {
                self.Stack[self.StackPointer - 1] = self.Stack[self.StackPointer - 1] - self.Stack[self.StackPointer];
                --self.StackPointer;
                self.ProgramCounter += sizeof(uint8_t);
            }
            break;

            case Op::Multiply:
            {
                self.Stack[self.StackPointer - 1] = self.Stack[self.StackPointer - 1] * self.Stack[self.StackPointer];
                --self.StackPointer;
                self.ProgramCounter += sizeof(uint8_t);
            }
            break;

            case Op::Divide:
            {
                self.Stack[self.StackPointer - 1] = self.Stack[self.StackPointer - 1] / self.Stack[self.StackPointer];
                --self.StackPointer;
                self.ProgramCounter += sizeof(uint8_t);
            }
            break;

            case Op::BranchIfLess:
            {
                auto const less = self.Stack[self.StackPointer - 1] < self.Stack[self.StackPointer];
                self.StackPointer -= 2;
                if (less)
                {
                    self.ProgramCounter = *reinterpret_cast<int32_t const*>(byteCode + self.ProgramCounter + sizeof(uint8_t));
                }
                else
                {
                    self.ProgramCounter += (sizeof(uint8_t) + sizeof(int32_t));
                }
            }
            break;

            case Op::BranchIfGreaterOrEqual:
            {
                auto const greaterOrEqual = self.Stack[self.StackPointer - 1] >= self.Stack[self.StackPointer];
                self.StackPointer -= 2;
                if (greaterOrEqual)
                {
                    self.ProgramCounter = *reinterpret_cast<int32_t const*>(byteCode + self.ProgramCounter + sizeof(uint8_t));
                }
                else
                {
                    self.ProgramCounter += (sizeof(uint8_t) + sizeof(int32_t));
                }
            }
            break;

            case Op::Duplicate:
            {
                ++self.StackPointer;
                self.Stack[self.StackPointer] = self.Stack[self.StackPointer - 1];
                self.ProgramCounter += sizeof(uint8_t);
            }
            break;

            case Op::Load:
            {
                auto const loadFrom = *reinterpret_cast<int32_t const*>(byteCode + self.ProgramCounter + sizeof(uint8_t));
                ++self.StackPointer;
                self.Stack[self.StackPointer] = self.Stack[self.StackPointer - loadFrom];
                self.ProgramCounter += (sizeof(uint8_t) + sizeof(int32_t));
            }
            break;

            case Op::Store:
            {
                auto const storeTo = *reinterpret_cast<int32_t const*>(byteCode + self.ProgramCounter + sizeof(uint8_t));
                self.Stack[self.StackPointer - storeTo] = self.Stack[self.StackPointer];
                --self.StackPointer;
                self.ProgramCounter += (sizeof(uint8_t) + sizeof(int32_t));
            }
            break;

            case Op::End:
            case Op::Size:
                self.ProgramCounter += sizeof(uint8_t);
            return;
        }
    }
}

#pragma endregion SwitchDispatch
