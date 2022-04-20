using System.Numerics;
using Moralis.Hex.HexConvertors.Extensions;

namespace Moralis.Hex.HexConvertors
{
    public class HexBigIntegerBigEndianConvertor : IHexConvertor<BigInteger>
    {
        public string ConvertToHex(BigInteger newValue)
        {
            return newValue.ToHex(false);
        }

        public BigInteger ConvertFromHex(string hex)
        {
            return hex.HexToBigInteger(false);
        }
    }
}