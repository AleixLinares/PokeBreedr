using PokeBreedr.Dto;

namespace PokeBreedr.Services
{
    public class PokemonInitialLoad
    {
        private Task? _loadTask;

        public bool IsLoaded { get; private set; }

        private readonly HttpClient _http;

        public Dictionary<string, PokemonCSVData> Pokemons { get; private set; } = new();

        public PokemonInitialLoad(HttpClient http)
        {
            _http = http;
        }

        public Task InitializeAsync()
        {
            _loadTask ??= LoadInternalAsync();
            return _loadTask;
        }

        private async Task LoadInternalAsync()
        {
            if (IsLoaded) return;

            var csv = await _http.GetStringAsync("assets/pokemmo_pokemon.csv");

            var lines = csv.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            int i = 0;

            foreach (var line in lines)
            {
                var parts = line.Split(',', StringSplitOptions.None);

                if (parts.Length < 4) continue;

                var pokemon = new PokemonCSVData
                {
                    Number = i++,
                    Name = parts[0].Trim(),
                    EggGroup1 = parts[1].Trim(),
                    EggGroup2 = parts[2].Trim(),
                    ImageBase64 = parts[3].Trim()
                };

                if (pokemon.EggGroup1 != "None") Pokemons[pokemon.Name] = pokemon;
            }

            IsLoaded = true;
        }

        // Return all keys in Pokemons
        public async Task<List<string>> GetAllPokemonsNames()
        {
            // Espera a que termine la carga inicial
            await InitializeAsync();

            return Pokemons.Keys.ToList();
        }

        // Returns the information of a specific pokemon
        public async Task<PokemonCSVData> GetPokemonInfo(string pokemonName)
        {
            await InitializeAsync();

            return Pokemons.TryGetValue(pokemonName, out var pokemon)
                ? pokemon
                : throw new KeyNotFoundException(
                    $"The pokemon with name '{pokemonName}' doesn't exist."
                );
        }
    }
}