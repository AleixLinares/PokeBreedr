using PokeBreedr.Dto;

public interface IPokemonInitialLoad
{
    Task InitializeAsync();

    Task<List<string>> GetAllPokemonsNames();

    Task<PokemonCSVData> GetPokemonInfo(string pokemonName);

    Task<Dictionary<string, PokemonCSVData>> GetAllPokemonsInfo();
}
