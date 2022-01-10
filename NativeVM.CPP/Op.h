#ifndef OP_H
#define OP_H

#include <cstdint>

enum class Op : uint8_t
{
    NoOp,
    Push,
    Pop,
    Add,
    Subtract,
    Multiply,
    Divide,
    BranchIfLess,
    BranchIfGreaterOrEqual,
    Duplicate,
    Load,
    Store,
    End,
    Size,
};

#endif // OP_H
