using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PokeBreedr.Interfaces;
using PokeBreedr.Models;
using System.Text.Json;

namespace PokeBreedr.Services
{
    public class PokemonLocalStorageService : IPokemonPersistanceInterface
    {
        private readonly IJSRuntime JSRuntime;
        private IJSObjectReference? localStorageModule;

        private const string STORAGE_KEY = "pokemons";

        public PokemonLocalStorageService(IJSRuntime jSRuntime)
        {
            JSRuntime = jSRuntime;
        }

        private async Task<IJSObjectReference> GetModule()
        {
            localStorageModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>(
                "import",
                "./js/localStorage.js"
            );

            return localStorageModule;
        }

        /// <summary>
        /// Save the pokemon provided.
        /// </summary>
        /// <param name="pokemonInfo"></param>
        /// <returns></returns>
        public async Task Save(PokemonInfo pokemonInfo)
        {
            var pokemons = await GetAll();

            var exists = pokemons
                .FirstOrDefault(p => p.Guid == pokemonInfo.Guid);

            if (exists != null)
            {
                pokemons.Remove(exists);
            }

            pokemons.Add(pokemonInfo);

            await SaveAll(pokemons);
        }

        /// <summary>
        /// Delete the pokemon with pokemonInfoID provided.
        /// </summary>
        /// <param name="pokemonInfoId"></param>
        /// <returns></returns>
        public async Task Delete(Guid pokemonInfoId)
        {
            var pokemons = await GetAll();

            pokemons.RemoveAll(p => p.Guid == pokemonInfoId);

            await SaveAll(pokemons);
        }

        /// <summary>
        /// Obtain the pokemon with the pokemonInfoID provided.
        /// </summary>
        /// <param name="pokemonInfoID"></param>
        public async Task<PokemonInfo?> Obtain(Guid pokemonInfoID)
        {
            var pokemons = await GetAll();

            return pokemons
                .FirstOrDefault(p => p.Guid == pokemonInfoID);
        }

        /// <summary>
        /// Get all Pokemons. The list returned is never null.
        /// </summary>
        public async Task<List<PokemonInfo>> GetAll()
        {

            var module = await GetModule();

            var json = await module.InvokeAsync<string>(
                "getItem",
                STORAGE_KEY
            );

            return string.IsNullOrEmpty(json)
                ? new List<PokemonInfo>()
                : JsonSerializer.Deserialize<List<PokemonInfo>>(json)!;
        }

        private async Task SaveAll(List<PokemonInfo> pokemons)
        {
            var module = await GetModule();

            var json = JsonSerializer.Serialize(pokemons);

            await module.InvokeVoidAsync(
                "setItem",
                STORAGE_KEY,
                json
            );
        }

        public async ValueTask DisposeAsync()
        {
            if (localStorageModule is not null)
            {
                await localStorageModule.DisposeAsync();
            }
        }
    }
}
