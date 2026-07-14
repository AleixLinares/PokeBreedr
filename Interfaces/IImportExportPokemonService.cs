using PokeBreedr.Dto;

namespace PokeBreedr.Interfaces
{
    public interface IImportExportPokemonService
    {
        List<PokemonInfoDto> ImportPokemons(List<PokemonInfoDto> pokemons, Dictionary<string, PokemonCSVData> pokemonsInfo, string csv, out string results);

        string ExportPokemons(List<PokemonInfoDto> pokemons);
    }
}
