namespace PokeBreedr.Utils
{
    public static class EnumExtensions
    {
        public static bool TryParseEnum<TEnum>(this string value, out TEnum result)
            where TEnum : struct, Enum
        {
            if (Enum.TryParse(value, true, out result))
                return true;

            if (int.TryParse(value, out int number) &&
                Enum.IsDefined(typeof(TEnum), number))
            {
                result = (TEnum)Enum.ToObject(typeof(TEnum), number);
                return true;
            }

            result = default;
            return false;
        }
    }
}