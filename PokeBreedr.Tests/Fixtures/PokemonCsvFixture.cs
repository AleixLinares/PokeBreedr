using PokeBreedr.Dto;

namespace PokeBreedr.Tests.Fixtures;

public class PokemonCsvFixture
{
    public Dictionary<string, PokemonCSVData> PokemonsInfo { get; }

    public PokemonCsvFixture()
    {
        PokemonsInfo = LoadPokemonData();
    }

    private Dictionary<string, PokemonCSVData> LoadPokemonData()
    {
        var path = Path.Combine(
            "..",
            "..",
            "..",
            "..",
            "wwwroot",
            "assets",
            "pokemmo_pokemon.csv"
        );

        var csv = File.ReadAllText(path);

        var pokemons = new Dictionary<string, PokemonCSVData>();

        var lines = csv.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        bool first = true;
        int i = 0;

        foreach (var line in lines)
        {
            if (first)
            {
                first = false;
                continue;
            }

            var parts = line.Split(',');

            if (parts.Length < 4)
                continue;

            var pokemon = new PokemonCSVData
            {
                Number = i++,
                Name = parts[0].Trim(),
                EggGroup1 = parts[1].Trim(),
                EggGroup2 = parts[2].Trim(),
                PokemonEgg = parts[3].Trim(),
                ImageBase64 = parts[4].Trim()
            };

            if (pokemon.EggGroup1 != "None")
                pokemons[pokemon.Name] = pokemon;
        }

        return pokemons;
    }
}