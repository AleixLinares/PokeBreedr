using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Microsoft.VisualBasic;
using PokeBreedr.Dto;
using PokeBreedr.Enums;
using PokeBreedr.Interfaces;
using PokeBreedr.Services;
using PokeBreedr.Utils;

namespace PokeBreedr.Pages
{
    public partial class ImportExport
    {
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        [Inject]
        private IPokemonPersistenceService PokemonPersistenceService { get; set; } = default!;

        [Inject]
        private PokemonInitialLoad PokemonInitialLoad { get; set; } = default!;

        [Inject]
        private IImportExportPokemonService ImportExportPokemonService { get; set; } = default!;

        private IJSObjectReference? importExportCsvModule;

        private bool importFinished;
        private string resultCsv = string.Empty;
        private string StatusMessage = string.Empty;
        private bool IsError;
        private List<string> validPokemons = new List<string>();
        private List<PokemonInfoDto> newPokemonsList = new List<PokemonInfoDto>();

        protected override async Task OnInitializedAsync()
        {
            validPokemons = await PokemonInitialLoad.GetAllPokemonsNames();
        }

        private async Task<IJSObjectReference> GetModule()
        {
            importExportCsvModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>(
                "import",
                "./js/importExportCsv.js");

            return importExportCsvModule;
        }

        private async Task SubmitCsv()
        {
            await this.PokemonPersistenceService.SaveAll(newPokemonsList);
            newPokemonsList = new List<PokemonInfoDto>();
            importFinished = false;
            StatusMessage = "All Pokemons imported correclty";
        }

        private async Task ImportCsv(InputFileChangeEventArgs e)
        {
            resultCsv = string.Empty;
            IsError = false;
            StatusMessage = string.Empty;
            newPokemonsList = new List<PokemonInfoDto>();

            var file = e.File;

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);

            var csv = await reader.ReadToEndAsync();

            List<PokemonInfoDto> currentPokemons = await PokemonPersistenceService.GetAll();
            Dictionary<string,PokemonCSVData> pokemonsInfo = await PokemonInitialLoad.GetAllPokemonsInfo();

            newPokemonsList = ImportExportPokemonService.ImportPokemons(currentPokemons, pokemonsInfo, csv, out resultCsv);

            importFinished = true;

            if (string.IsNullOrEmpty(resultCsv))
            {
                IsError = false;
                StatusMessage = "All Pokemons processed correclty. You can now submit your pokemons.";
            }
            else
            {
                IsError = true;
                StatusMessage = "One or more Pokemons could not be processed correclty, download results to see which are wrong";
            }
        }

        private async Task ExportCsv()
        {
            var module = await GetModule();

            List<PokemonInfoDto> currentPokemons = await PokemonPersistenceService.GetAll();

            string csv = ImportExportPokemonService.ExportPokemons(currentPokemons);

            await module.InvokeVoidAsync(
                "downloadCsv",
                "export.csv",
                csv);
        }

        private async Task ExportResultCsv()
        {
            var module = await GetModule();
            
            await module.InvokeVoidAsync(
                "downloadCsv",
                "importResult.csv",
                resultCsv);
        }

        public async ValueTask DisposeAsync()
        {
            if (importExportCsvModule is not null)
            {
                await importExportCsvModule.DisposeAsync();
            }
        }
    }
}
