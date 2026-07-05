namespace PokeBreedr.Dto
{
    public class TransferPokemonDto
    {
        public PokemonInfoDto Pokemon { get; set; } = default!;

        public Guid Parent1 { get; set; }

        public Guid Parent2 { get; set; }

    }
}
