using System;

namespace Bn254.Net
{
    public class Bn254Base
    {
        internal const string LIB = "bn254";

        public static string LibPath => _libPath.Value;
        static readonly Lazy<string> _libPath = new Lazy<string>(() => LibPathResolver.Resolve(LIB));
        static readonly Lazy<IntPtr> _libPtr = new Lazy<IntPtr>(() => LoadLibNative.LoadLib(_libPath.Value));

        public static Lazy<TDelegate> LazyDelegate<TDelegate>(string symbol)
        {
            return new Lazy<TDelegate>(
                () => { return LoadLibNative.GetDelegate<TDelegate>(_libPtr.Value, symbol); },
                isThreadSafe: false);
        }

        private static Lazy<TDelegate> LazyDelegate<TDelegate>(string symbol,
            Func<IntPtr, IntPtr> pointerDereferenceFunc)
        {
            return new Lazy<TDelegate>(
                () => { return LoadLibNative.GetDelegate<TDelegate>(_libPtr.Value, symbol, pointerDereferenceFunc); },
                isThreadSafe: false);
        }
    }
}