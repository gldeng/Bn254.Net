namespace Bn254.Net
{
    public static class Extensions
    {
        public static UInt256 ToUInt256(this byte[] bytes)
        {
            return new UInt256(bytes);
        }

        public static UInt256 ToUInt256(this string hex)
        {
            return new UInt256(hex);
        }

        public static UInt256 DecToUInt256(this string dec)
        {
            return UInt256.FromDec(dec);
        }
    }
}