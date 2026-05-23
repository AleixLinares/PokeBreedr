namespace PokeBreedr.Dto
{
    public class PokemonCSVData
    {
        public int Number { get; set; }
        public string Name { get; set; } = "";
        public string EggGroup1 { get; set; } = "";
        public string EggGroup2 { get; set; } = "";
        public string ImageBase64 { get; set; } = "";
    }
}