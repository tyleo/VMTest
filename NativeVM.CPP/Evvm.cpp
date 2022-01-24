#include "Evvm.h"

#define I32I(instructionPointer, index) \
    *reinterpret_cast<int32_t const *>(instructionPointer + sizeof(void *) + index * sizeof(int32_t))

#define BRANCH_TARGET(instructionPointer) \
    *reinterpret_cast<uint8_t const * const*>(instructionPointer + sizeof(void *) + sizeof(int32_t) + sizeof(int32_t))

#define I32R(instructionPointer, registerPointer, index) \
    reinterpret_cast<int32_t *>( \
        registerPointer + \
        *reinterpret_cast<int32_t const *>(instructionPointer + sizeof(void *) + index * sizeof(int32_t)) \
    )

void InitEvvmLabelAddresses();

void const* Evvm::LabelAddresses[(int)EvvmOp::Size] = { nullptr };

void EvvmStaticInit()
{
    InitEvvmLabelAddresses();
}

void const* const* EvvmGetLabelAddresses()
{
    return reinterpret_cast<void const* const*>(Evvm::LabelAddresses);
}

void* EvvmNew()
{
    return new Evvm{};
}

void EvvmDelete(void* const pSelf)
{
    auto const pSelfTyped = static_cast<Evvm* const>(pSelf);
    delete pSelfTyped;
}

int EvvmGet(void const* const pSelf, int32_t index)
{
    auto& self = *static_cast<Evvm const* const>(pSelf);
    return *reinterpret_cast<int const *>(self.Registers + index);
}

void EvvmRun(void* const pSelf, uint8_t const* byteCode)
{
    if (pSelf == nullptr)
    {
        Evvm::LabelAddresses[(int)EvvmOp::AddI32_I32i_I32i_I32r] = && L_AddI32_I32i_I32i_I32r;
        Evvm::LabelAddresses[(int)EvvmOp::AddI32_I32i_I32r_I32r] = && L_AddI32_I32i_I32r_I32r;
        Evvm::LabelAddresses[(int)EvvmOp::AddI32_I32r_I32r_I32r] = && L_AddI32_I32r_I32r_I32r;

        Evvm::LabelAddresses[(int)EvvmOp::BranchIfGreaterOrEqualI32_I32i_I32i_I32i] = && L_BranchIfGreaterOrEqualI32_I32i_I32i_I32i;
        Evvm::LabelAddresses[(int)EvvmOp::BranchIfGreaterOrEqualI32_I32i_I32r_I32i] = && L_BranchIfGreaterOrEqualI32_I32i_I32r_I32i;
        Evvm::LabelAddresses[(int)EvvmOp::BranchIfGreaterOrEqualI32_I32r_I32r_I32i] = && L_BranchIfGreaterOrEqualI32_I32r_I32r_I32i;

        Evvm::LabelAddresses[(int)EvvmOp::BranchIfLessI32_I32i_I32i_I32i] = && L_BranchIfLessI32_I32i_I32i_I32i;
        Evvm::LabelAddresses[(int)EvvmOp::BranchIfLessI32_I32i_I32r_I32i] = && L_BranchIfLessI32_I32i_I32r_I32i;
        Evvm::LabelAddresses[(int)EvvmOp::BranchIfLessI32_I32r_I32r_I32i] = && L_BranchIfLessI32_I32r_I32r_I32i;

        Evvm::LabelAddresses[(int)EvvmOp::CopyI32_I32i_I32r] = && L_CopyI32_I32i_I32r;
        Evvm::LabelAddresses[(int)EvvmOp::CopyI32_I32r_I32r] = && L_CopyI32_I32r_I32r;

        Evvm::LabelAddresses[(int)EvvmOp::DivideI32_I32i_I32i_I32r] = && L_DivideI32_I32i_I32i_I32r;
        Evvm::LabelAddresses[(int)EvvmOp::DivideI32_I32i_I32r_I32r] = && L_DivideI32_I32i_I32r_I32r;
        Evvm::LabelAddresses[(int)EvvmOp::DivideI32_I32r_I32r_I32r] = && L_DivideI32_I32r_I32r_I32r;

        Evvm::LabelAddresses[(int)EvvmOp::MultiplyI32_I32i_I32i_I32r] = && L_MultiplyI32_I32i_I32i_I32r;
        Evvm::LabelAddresses[(int)EvvmOp::MultiplyI32_I32i_I32r_I32r] = && L_MultiplyI32_I32i_I32r_I32r;
        Evvm::LabelAddresses[(int)EvvmOp::MultiplyI32_I32r_I32r_I32r] = && L_MultiplyI32_I32r_I32r_I32r;

        Evvm::LabelAddresses[(int)EvvmOp::NoOp] = && L_NoOp;

        Evvm::LabelAddresses[(int)EvvmOp::SubtractI32_I32i_I32i_I32r] = && L_SubtractI32_I32i_I32i_I32r;
        Evvm::LabelAddresses[(int)EvvmOp::SubtractI32_I32i_I32r_I32r] = && L_SubtractI32_I32i_I32r_I32r;
        Evvm::LabelAddresses[(int)EvvmOp::SubtractI32_I32r_I32r_I32r] = && L_SubtractI32_I32r_I32r_I32r;

        Evvm::LabelAddresses[(int)EvvmOp::End] = && L_End;

        return;
    }

    auto& self = *static_cast<Evvm* const>(pSelf);

    auto instructionPointer = byteCode;
    auto registerPointer = &self.Registers[0];

    goto** reinterpret_cast<void* const*>(instructionPointer);

L_AddI32_I32i_I32i_I32r:
    {
        *I32R(instructionPointer, registerPointer, 2) = I32I(instructionPointer, 0) + I32I(instructionPointer, 1);
        instructionPointer += sizeof(void *) + sizeof(int32_t) + sizeof(int32_t) + sizeof(int32_t);
    }
    goto** reinterpret_cast<void* const*>(instructionPointer);

L_AddI32_I32i_I32r_I32r:
    {
        *I32R(instructionPointer, registerPointer, 2) = I32I(instructionPointer, 0) + *I32R(instructionPointer, registerPointer, 1);
        instructionPointer += sizeof(void *) + sizeof(int32_t) + sizeof(int32_t) + sizeof(int32_t);
    }
    goto** reinterpret_cast<void* const*>(instructionPointer);

L_AddI32_I32r_I32r_I32r:
    {
        *I32R(instructionPointer, registerPointer, 2) = *I32R(instructionPointer, registerPointer, 0) + *I32R(instructionPointer, registerPointer, 1);
        instructionPointer += sizeof(void *) + sizeof(int32_t) + sizeof(int32_t) + sizeof(int32_t);
    }
    goto** reinterpret_cast<void* const*>(instructionPointer);


L_BranchIfGreaterOrEqualI32_I32i_I32i_I32i:
    {
        if (I32I(instructionPointer, 0) >= I32I(instructionPointer, 1))
        {
            instructionPointer = BRANCH_TARGET(instructionPointer);
        }
        else
        {
            instructionPointer += sizeof(void *) + sizeof(int32_t) + sizeof(int32_t) + sizeof(uint8_t *);
        }
    }
    goto** reinterpret_cast<void* const*>(instructionPointer);

L_BranchIfGreaterOrEqualI32_I32i_I32r_I32i:
    {
        if (I32I(instructionPointer, 0) >= *I32R(instructionPointer, registerPointer, 1))
        {
            instructionPointer = BRANCH_TARGET(instructionPointer);
        }
        else
        {
            instructionPointer += sizeof(void*) + sizeof(int32_t) + sizeof(int32_t) + sizeof(uint8_t*);
        }
    }
    goto** reinterpret_cast<void* const*>(instructionPointer);

L_BranchIfGreaterOrEqualI32_I32r_I32r_I32i:
    {
        if (*I32R(instructionPointer, registerPointer, 0) >= *I32R(instructionPointer, registerPointer, 1))
        {
            instructionPointer = BRANCH_TARGET(instructionPointer);
        }
        else
        {
            instructionPointer += sizeof(void*) + sizeof(int32_t) + sizeof(int32_t) + sizeof(uint8_t*);
        }
    }
    goto** reinterpret_cast<void* const*>(instructionPointer);


L_BranchIfLessI32_I32i_I32i_I32i:
    {
        if (I32I(instructionPointer, 0) < I32I(instructionPointer, 1))
        {
            instructionPointer = BRANCH_TARGET(instructionPointer);
        }
        else
        {
            instructionPointer += sizeof(void*) + sizeof(int32_t) + sizeof(int32_t) + sizeof(uint8_t*);
        }
    }
    goto** reinterpret_cast<void* const*>(instructionPointer);

L_BranchIfLessI32_I32i_I32r_I32i:
    {
        if (I32I(instructionPointer, 0) < *I32R(instructionPointer, registerPointer, 1))
        {
            instructionPointer = BRANCH_TARGET(instructionPointer);
        }
        else
        {
            instructionPointer += sizeof(void*) + sizeof(int32_t) + sizeof(int32_t) + sizeof(uint8_t*);
        }
    }
    goto** reinterpret_cast<void* const*>(instructionPointer);

L_BranchIfLessI32_I32r_I32r_I32i:
    {
        if (*I32R(instructionPointer, registerPointer, 0) < *I32R(instructionPointer, registerPointer, 1))
        {
            instructionPointer = BRANCH_TARGET(instructionPointer);
        }
        else
        {
            instructionPointer += sizeof(void*) + sizeof(int32_t) + sizeof(int32_t) + sizeof(uint8_t*);
        }
    }
    goto** reinterpret_cast<void* const*>(instructionPointer);


L_CopyI32_I32i_I32r:
    {
        *I32R(instructionPointer, registerPointer, 1) = I32I(instructionPointer, 0);
        instructionPointer += sizeof(void *) + sizeof(int32_t) + sizeof(int32_t);
    }
    goto** reinterpret_cast<void* const*>(instructionPointer);

L_CopyI32_I32r_I32r:
    {
        *I32R(instructionPointer, registerPointer, 1) = *I32R(instructionPointer, registerPointer, 0);
        instructionPointer += sizeof(void *) + sizeof(int32_t) + sizeof(int32_t);
    }
    goto** reinterpret_cast<void* const*>(instructionPointer);


L_DivideI32_I32i_I32i_I32r:
    {
        *I32R(instructionPointer, registerPointer, 2) = I32I(instructionPointer, 0) / I32I(instructionPointer, 1);
        instructionPointer += sizeof(void *) + sizeof(int32_t) + sizeof(int32_t) + sizeof(int32_t);
    }
    goto** reinterpret_cast<void* const*>(instructionPointer);

L_DivideI32_I32i_I32r_I32r:
    {
        *I32R(instructionPointer, registerPointer, 2) = I32I(instructionPointer, 0) / *I32R(instructionPointer, registerPointer, 1);
        instructionPointer += sizeof(void *) + sizeof(int32_t) + sizeof(int32_t) + sizeof(int32_t);
    }
    goto** reinterpret_cast<void* const*>(instructionPointer);

L_DivideI32_I32r_I32r_I32r:
    {
        *I32R(instructionPointer, registerPointer, 2) = *I32R(instructionPointer, registerPointer, 0) / *I32R(instructionPointer, registerPointer, 1);
        instructionPointer += sizeof(void *) + sizeof(int32_t) + sizeof(int32_t) + sizeof(int32_t);
    }
    goto** reinterpret_cast<void* const*>(instructionPointer);


L_MultiplyI32_I32i_I32i_I32r:
    {
        *I32R(instructionPointer, registerPointer, 2) = I32I(instructionPointer, 0) * I32I(instructionPointer, 1);
        instructionPointer += sizeof(void *) + sizeof(int32_t) + sizeof(int32_t) + sizeof(int32_t);
    }
    goto** reinterpret_cast<void* const*>(instructionPointer);

L_MultiplyI32_I32i_I32r_I32r:
    {
        *I32R(instructionPointer, registerPointer, 2) = I32I(instructionPointer, 0) * *I32R(instructionPointer, registerPointer, 1);
        instructionPointer += sizeof(void *) + sizeof(int32_t) + sizeof(int32_t) + sizeof(int32_t);
    }
    goto** reinterpret_cast<void* const*>(instructionPointer);

L_MultiplyI32_I32r_I32r_I32r:
    {
        *I32R(instructionPointer, registerPointer, 2) = *I32R(instructionPointer, registerPointer, 0) * *I32R(instructionPointer, registerPointer, 1);
        instructionPointer += sizeof(void *) + sizeof(int32_t) + sizeof(int32_t) + sizeof(int32_t);
    }
    goto** reinterpret_cast<void* const*>(instructionPointer);


L_NoOp:
    {
        instructionPointer += sizeof(void *);
    }
    goto** reinterpret_cast<void* const*>(instructionPointer);


L_SubtractI32_I32i_I32i_I32r:
    {
        *I32R(instructionPointer, registerPointer, 2) = I32I(instructionPointer, 0) - I32I(instructionPointer, 1);
        instructionPointer += sizeof(void *) + sizeof(int32_t) + sizeof(int32_t) + sizeof(int32_t);
    }
    goto** reinterpret_cast<void* const*>(instructionPointer);

L_SubtractI32_I32i_I32r_I32r:
    {
        *I32R(instructionPointer, registerPointer, 2) = I32I(instructionPointer, 0) - *I32R(instructionPointer, registerPointer, 1);
        instructionPointer += sizeof(void *) + sizeof(int32_t) + sizeof(int32_t) + sizeof(int32_t);
    }
    goto** reinterpret_cast<void* const*>(instructionPointer);

L_SubtractI32_I32r_I32r_I32r:
    {

        *I32R(instructionPointer, registerPointer, 2) = *I32R(instructionPointer, registerPointer, 0) - *I32R(instructionPointer, registerPointer, 1);
        instructionPointer += sizeof(void *) + sizeof(int32_t) + sizeof(int32_t) + sizeof(int32_t);
    }
    goto** reinterpret_cast<void* const*>(instructionPointer);


L_End:
    {
    }
    return;
}

void InitEvvmLabelAddresses()
{
    EvvmRun(nullptr, nullptr);
}
