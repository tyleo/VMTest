#ifndef EVVM_OP_H
#define EVVM_OP_H

#include <cstdint>

enum class EvvmOp : uint8_t
{
    AddI32_I32i_I32i_I32r,
    AddI32_I32i_I32r_I32r,
    AddI32_I32r_I32r_I32r,

    BranchIfGreaterOrEqualI32_I32i_I32i_I32i,
    BranchIfGreaterOrEqualI32_I32i_I32r_I32i,
    BranchIfGreaterOrEqualI32_I32r_I32r_I32i,

    BranchIfLessI32_I32i_I32i_I32i,
    BranchIfLessI32_I32i_I32r_I32i,
    BranchIfLessI32_I32r_I32r_I32i,

    CopyI32_I32i_I32r,
    CopyI32_I32r_I32r,

    DivideI32_I32i_I32i_I32r,
    DivideI32_I32i_I32r_I32r,
    DivideI32_I32r_I32r_I32r,

    MultiplyI32_I32i_I32i_I32r,
    MultiplyI32_I32i_I32r_I32r,
    MultiplyI32_I32r_I32r_I32r,

    NoOp,

    SubtractI32_I32i_I32i_I32r,
    SubtractI32_I32i_I32r_I32r,
    SubtractI32_I32r_I32r_I32r,

    End,

    Size,
};

#endif // EVVM_OP_H
