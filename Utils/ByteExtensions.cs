namespace PokeBreedr.Utils
{
    public static class ByteExtensions
    {
        public static bool InRange(this byte value, int min, int max)
        {
            return min <= value && value <= max;
        }
    }
}
