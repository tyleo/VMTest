#ifndef EVVM_H
#define EVVM_H

#include <cstdint>

#include "EvvmOp.h"

struct Evvm final
{
    using RunDelegate = void(Evvm& self, uint8_t const* byteCode);

    static void const* LabelAddresses[(int)EvvmOp::Size];

    uint8_t Registers[1000];
};

extern "C" __declspec(dllexport) void __cdecl EvvmStaticInit();

extern "C" __declspec(dllexport) void const* const* __cdecl EvvmGetLabelAddresses();

extern "C" __declspec(dllexport) void* __cdecl EvvmNew();

extern "C" __declspec(dllexport) void __cdecl EvvmDelete(void* pSelf);

extern "C" __declspec(dllexport) int32_t __cdecl EvvmGet(void const* pSelf, int32_t index);

extern "C" __declspec(dllexport) void __cdecl EvvmRun(void* pSelf, uint8_t const* byteCode);

#endif // EVVM_H
