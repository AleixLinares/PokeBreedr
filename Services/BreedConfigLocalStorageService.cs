using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PokeBreedr.Dto;
using PokeBreedr.Interfaces;
using PokeBreedr.Models;
using System.Text.Json;

namespace PokeBreedr.Services
{
    public class BreedConfigLocalStorageService : IBreedConfigPersistenceService
    {
        private readonly IJSRuntime JSRuntime;
        private IJSObjectReference? localStorageModule;

        private const string STORAGE_KEY = "breedConfigs";

        public BreedConfigLocalStorageService(IJSRuntime jSRuntime)
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
        /// Save the cardInfo provided.
        /// </summary>
        /// <param name="cardInfo"></param>
        /// <returns></returns>
        public async Task Save(ConfigCardInfoDto cardInfo)
        {
            var configCards = await GetAll();

            var exists = configCards
                .FirstOrDefault(p => p.Guid == cardInfo.Guid);

            if (exists != null)
            {
                configCards.Remove(exists);
            }

            configCards.Add(cardInfo);

            await SaveAll(configCards);
        }

        /// <summary>
        /// Delete the config card with cardInfoId provided.
        /// </summary>
        /// <param name="cardInfoId"></param>
        /// <returns></returns>
        public async Task Delete(Guid cardInfoId)
        {
            var configCards = await GetAll();

            configCards.RemoveAll(p => p.Guid == cardInfoId);

            await SaveAll(configCards);
        }

        /// <summary>
        /// Obtain the config card with the cardInfoId provided.
        /// </summary>
        /// <param name="cardInfoID"></param>
        public async Task<ConfigCardInfoDto?> Obtain(Guid cardInfoID)
        {
            var configCards = await GetAll();

            return configCards
                .FirstOrDefault(p => p.Guid == cardInfoID);
        }

        /// <summary>
        /// Checks if the name provided is empty or already exists
        /// </summary>
        /// <param name="cardInfoID"></param>
        public async Task<bool> CheckIfNameUniqueAndNotEmpty(string? name, Guid? itemGuid)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            var configCards = await GetAll();

            name = name.Trim();
            return !configCards.Any(p => p.ConfigName == name && p.Guid != itemGuid);
        }

        /// <summary>
        /// Get all config cards. The list returned is never null.
        /// </summary>
        public async Task<List<ConfigCardInfoDto>> GetAll()
        {

            var module = await GetModule();

            var json = await module.InvokeAsync<string>(
                "getItem",
                STORAGE_KEY
            );
            try
            {
                var deserialized = string.IsNullOrEmpty(json)
                    ? new List<ConfigCardInfoDto>()
                    : JsonSerializer.Deserialize<List<ConfigCardInfoDto>>(json)!;

                return deserialized.OrderBy(i => i.ConfigName).ToList();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            return new List<ConfigCardInfoDto>();
        }

        private async Task SaveAll(List<ConfigCardInfoDto> configcards)
        {
            var module = await GetModule();

            var json = JsonSerializer.Serialize(configcards);

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
