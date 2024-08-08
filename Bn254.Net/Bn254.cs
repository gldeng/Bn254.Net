using System;
using System.Collections;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Bn254.Net
{
    public unsafe class Bn254 : Bn254Base
    {
        enum ReturnCodes
        {
            Success = 0,
            Failed = -1,
            InvalidInput = -2,
        }


        static readonly Lazy<bn254_add> bn254_add
            = LazyDelegate<bn254_add>(nameof(bn254_add));

        static readonly Lazy<bn254_scalar_mul> bn254_scalar_mul
            = LazyDelegate<bn254_scalar_mul>(nameof(bn254_scalar_mul));

        static readonly Lazy<bn254_pairing> bn254_pairing
            = LazyDelegate<bn254_pairing>(nameof(bn254_pairing));
        
        public static byte[] ModExp(byte[] @base, byte[] exp, byte[] modulus)
        {
            if (modulus.Length == 0)
            {
                return Array.Empty<byte>();
            }

            var modulusBig = new BigInteger(modulus, true, true);
            if (modulusBig.IsZero)
            {
                return Helpers.LeftPad(Array.Empty<byte>(), modulus.Length);
            }

            var baseBig = new BigInteger(@base, true, true);
            var expBig = new BigInteger(exp, true, true);
            var res = BigInteger.ModPow(baseBig, expBig, modulusBig);
            return Helpers.LeftPad(res.ToByteArray(true, true), modulus.Length);
        }

        public static (UInt256 x3, UInt256 y3) Add(UInt256 x1, UInt256 y1, UInt256 x2, UInt256 y2)
        {
            var buf = new byte[128];
            Array.Copy(x1.ToBigEndianBytes(), 0, buf, 0, 32);
            Array.Copy(y1.ToBigEndianBytes(), 0, buf, 32, 32);
            Array.Copy(x2.ToBigEndianBytes(), 0, buf, 64, 32);
            Array.Copy(y2.ToBigEndianBytes(), 0, buf, 96, 32);
            var res = 0;
            fixed (byte* bufPtr = buf)
            {
                res = bn254_add.Value(bufPtr, 128);
            }

            switch (res)
            {
                case (int)ReturnCodes.Success:
                    return (new UInt256(new Span<byte>(buf, 0, 32).ToArray()),
                        new UInt256(new Span<byte>(buf, 32, 32).ToArray()));
                case (int)ReturnCodes.Failed:
                    throw new ArgumentException("Invalid point");
                case (int)ReturnCodes.InvalidInput:
                    throw new ArgumentException("Invalid point");
                default:
                    throw new Exception("Unknown error");
            }
        }

        public static (UInt256 x, UInt256 y) Mul(UInt256 x1, UInt256 y1, UInt256 s)
        {
            var buf = new byte[96];
            Array.Copy(x1.ToBigEndianBytes(), 0, buf, 0, 32);
            Array.Copy(y1.ToBigEndianBytes(), 0, buf, 32, 32);
            Array.Copy(s.ToBigEndianBytes(), 0, buf, 64, 32);
            var res = 0;
            fixed (byte* bufPtr = buf)
            {
                res = bn254_scalar_mul.Value(bufPtr, 96);
            }

            switch (res)
            {
                case (int)ReturnCodes.Success:
                    return (new UInt256(new Span<byte>(buf, 0, 32).ToArray()),
                        new UInt256(new Span<byte>(buf, 32, 32).ToArray()));
                case (int)ReturnCodes.Failed:
                    throw new ArgumentException("Scalar multiplication failed");
                case (int)ReturnCodes.InvalidInput:
                    throw new ArgumentException("Invalid point");
                default:
                    throw new Exception("Unknown error");
            }
        }

        public static bool Pairing((UInt256, UInt256, UInt256, UInt256, UInt256, UInt256)[] input)
        {
            var buf = new byte[6 * input.Length * 32];

            foreach (var (index, value) in input.Select((value, index) => (index, value)))
            {
                var (x1, y1, x2, y2, x3, y3) = value;
                Array.Copy(x1.ToBigEndianBytes(), 0, buf, index * 192, 32);
                Array.Copy(y1.ToBigEndianBytes(), 0, buf, index * 192 + 32, 32);
                Array.Copy(x2.ToBigEndianBytes(), 0, buf, index * 192 + 64, 32);
                Array.Copy(y2.ToBigEndianBytes(), 0, buf, index * 192 + 96, 32);
                Array.Copy(x3.ToBigEndianBytes(), 0, buf, index * 192 + 128, 32);
                Array.Copy(y3.ToBigEndianBytes(), 0, buf, index * 192 + 160, 32);
            }

            var res = 0;
            fixed (byte* bufPtr = buf)
            {
                res = bn254_pairing.Value(bufPtr, (uint)(6 * input.Length * 32));
            }

            switch (res)
            {
                case (int)ReturnCodes.Success:

                {
                    var big = new BigInteger(new Span<byte>(buf, 0, 32).ToArray(), true, true);
                    return big == 1;
                }
                case (int)ReturnCodes.Failed:
                    return false;
                case (int)ReturnCodes.InvalidInput:
                    throw new ArgumentException("Invalid point");
                default:
                    throw new Exception("Unknown error");
            }
        }
    }
}