using System;
using System.Numerics;

namespace Bn254.Net
{
    public class UInt256
    {
        private readonly BigInteger _inner = BigInteger.Zero;

        public static UInt256 FromDec(string dec)
        {
            return new UInt256(Helpers.LeftPad(BigInteger.Parse(dec).ToByteArray(true, true), 32));
        }

        public static UInt256 FromHex(string hex)
        {
            return new UInt256(hex);
        }

        public static UInt256 FromBigEndianBytes(byte[] bytes)
        {
            return new UInt256(bytes);
        }

        private UInt256(BigInteger inner)
        {
            _inner = inner;
        }

        public UInt256(string hex)
        {
            if (!Helpers.IsValidHex(hex))
                throw new ArgumentException("Invalid hex string", nameof(hex));
            var bigEndianBytes = Helpers.HexStringToByteArray(hex);

            _inner = new BigInteger(bigEndianBytes, true, true);
        }

        public UInt256(byte[] bigEndianBytes)
        {
            if (bigEndianBytes.Length != 32)
                throw new ArgumentException("Invalid byte length", nameof(bigEndianBytes));
            _inner = new BigInteger(bigEndianBytes, true, true);
        }


        #region Implicit Operators

        public static implicit operator UInt256(string a)
        {
            if (a[0] == '0' && (a[1] == 'x' || a[1] == 'X'))
            {
                return FromHex(a);
            }
            else
            {
                return FromDec(a);
            }
        }

        public static implicit operator string(UInt256 a)
        {
            return a._inner.ToString();
        }

        public static implicit operator int(UInt256 a)
        {
            return (int)a._inner;
        }

        public static implicit operator UInt256(int a)
        {
            return new UInt256(new BigInteger(a));
        }

        #endregion

        #region Operators

        public static UInt256 operator %(UInt256 a, UInt256 b)
        {
            return new UInt256(a._inner % b._inner);
        }

        public static UInt256 operator +(UInt256 a, UInt256 b)
        {
            return new UInt256(a._inner + b._inner);
        }

        public static UInt256 operator -(UInt256 a, UInt256 b)
        {
            return new UInt256(a._inner - b._inner);
        }

        public static UInt256 operator *(UInt256 a, UInt256 b)
        {
            return new UInt256(a._inner * b._inner);
        }

        public static bool operator ==(UInt256 a, UInt256 b)
        {
            return a._inner == b._inner;
        }

        public static bool operator !=(UInt256 a, UInt256 b)
        {
            return !(a == b);
        }

        #endregion


        public bool IsZero()
        {
            return _inner.IsZero;
        }


        public static UInt256 MulMod(UInt256 x, UInt256 y, UInt256 z)
        {
            return new UInt256(x._inner * y._inner % z._inner);
        }


        public byte[] ToBigEndianBytes()
        {
            var bytes = Helpers.LeftPad(_inner.ToByteArray(true, true), 32);
            return bytes;
        }

        public string ToHexString()
        {
            var bytes = ToBigEndianBytes();
            return "0x" + BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        public override string ToString()
        {
            return _inner.ToString();
        }

        public override int GetHashCode()
        {
            return _inner.GetHashCode();
        }


        public override bool Equals(object? obj)
        {
            if (obj is UInt256 uint256)
                return _inner.Equals(uint256._inner);
            return false;
        }
    }
}