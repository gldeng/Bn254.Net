using System;
using System.Runtime.InteropServices;

namespace Bn254.Net
{
    public unsafe delegate int bn254_add(byte* buf, uint max_len);

    public unsafe delegate int bn254_scalar_mul(byte* buf, uint max_len);

    public unsafe delegate int bn254_pairing(byte* buf, uint max_len);

    /// <summary>
    /// Type for error and illegal callback functions,
    /// </summary>
    /// <param name="message">message: error message.</param>
    /// <param name="data">data: callback marker, it is set by user together with callback.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void ErrorCallbackDelegate(string message, void* data);
}