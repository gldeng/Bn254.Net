using System;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Bn254.Net
{
    public class UInt256
    {
        private readonly BigInteger _inner = BigInteger.Zero;

        public static UInt256 FromHex(string hex)
        {
            return new UInt256(hex);
        }

        public static UInt256 FromBigEndianBytes(byte[] bytes)
        {
            return new UInt256(bytes);
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