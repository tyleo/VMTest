#ifndef VM_H
#define VM_H

#include <cstdint>

#include "Op.h"

struct VM final
{
    using RunDelegate = void(VM & self, uint8_t const * byteCode);

    static void (* FunctionAddresses[(int)Op::Size]) (VM & self, uint8_t const * byteCode);
    static void (* EmbeddedFunctionAddresses[(int)Op::Size]) (VM & self, uint8_t const * byteCode);

    static void const * LabelAddresses[(int)Op::Size];
    static void const * EmbeddedLabelAddresses[(int)Op::Size];
    static void const * EmbeddedAlignedLabelAddresses[(int)Op::Size];

    int32_t StackPointer;
    int32_t Stack[1000];
    int32_t ProgramCounter;
};

extern "C" __declspec(dllexport) void __cdecl VMStaticInit();

extern "C" __declspec(dllexport) void const * const * __cdecl VMGetFunctionAddresses();

extern "C" __declspec(dllexport) void const * const * __cdecl VMGetEmbeddedFunctionAddresses();

extern "C" __declspec(dllexport) void const * const * __cdecl VMGetLabelAddresses();

extern "C" __declspec(dllexport) void const * const * __cdecl VMGetEmbeddedLabelAddresses();

extern "C" __declspec(dllexport) void const * const * __cdecl VMGetEmbeddedAlignedLabelAddresses();

extern "C" __declspec(dllexport) void * __cdecl VMNew();

extern "C" __declspec(dllexport) void __cdecl VMDelete(void * pSelf);

extern "C" __declspec(dllexport) int32_t __cdecl VMGetLast(void const * pSelf);

extern "C" __declspec(dllexport) void __cdecl VMCalliRun(void * pSelf, uint8_t const * byteCode);

extern "C" __declspec(dllexport) void __cdecl VMCalliEmbeddedRun(void * pSelf, uint8_t const * byteCode);

extern "C" __declspec(dllexport) void __cdecl VMAddressOfLabelRun(void * pSelf, uint8_t const * byteCode);

extern "C" __declspec(dllexport) void __cdecl VMAddressOfLabelEmbeddedRun(void * pSelf, uint8_t const * byteCode);

extern "C" __declspec(dllexport) void __cdecl VMAddressOfLabelEmbeddedAlignedRun(void * pSelf, uint8_t const * byteCode);

extern "C" __declspec(dllexport) void __cdecl VMSwitchDispatchRun(void * pSelf, uint8_t const * byteCode);

#endif // VM_H
