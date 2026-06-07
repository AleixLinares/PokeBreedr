namespace PokeBreedr.Utils
{
    public static class ByteExtensions
    {
        public static bool InRange(this byte value, int min, int max, out bool isIn)
        {
            isIn = min <= value && value <= max;
            return isIn;
        }
    }
}
