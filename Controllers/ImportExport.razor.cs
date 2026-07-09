using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using PokeBreedr.Dto;
using PokeBreedr.Enums;
using PokeBreedr.Interfaces;

namespace PokeBreedr.Pages
{
    public partial class ImportExport
    {
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        [Inject]
        private IPokemonPersistenceService PokemonPersistenceService { get; set; } = default!;

        private IJSObjectReference? importExportCsvModule;

        private bool importFinished;
        private string resultCsv = string.Empty;

        private async Task<IJSObjectReference> GetModule()
        {
            importExportCsvModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>(
                "import",
                "./js/importExportCsv.js");

            return importExportCsvModule;
        }

        private async Task ImportCsv(InputFileChangeEventArgs e)
        {
            var file = e.File;

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);

            var csv = await reader.ReadToEndAsync();

            List<PokemonInfoDto> currentPokemons = await PokemonPersistenceService.GetAll();

            Dictionary<Guid,PokemonInfoDto> currentPokemonDictionary = currentPokemons.ToDictionary(x => x.Guid);

            string[] lines = csv.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; ++i)
            {
                string line = lines[i];

                PokemonInfoDto? pokemon = this.TryParseCsvLine(line, out string error);

                if (error != string.Empty) 
                {
                    string resultLine = "row: " + i + "," + line + "," + "Error: " + error + "\n";
                    resultCsv += resultLine;
                    continue;
                }
            }

            resultCsv = "";

            importFinished = true;
        }

        private PokemonInfoDto? TryParseCsvLine(string line, out string errors)
        {
            var parsedLine = line.Split(',', StringSplitOptions.TrimEntries);
            errors = string.Empty;

            if (parsedLine.Length != 12)
            {
                errors = "The line doesn't match the expected structure separated by comas" +
                    ": Id; Pokemon; Gender; Nickname; Nature; HpIv; AttackIv; DefenseIv; SpAttackIv; SpDefenseIv; SpeedIv; Particles";
                return null;
            }

            PokemonInfoDto pokemon = new PokemonInfoDto();

            if (parsedLine[0].IsWhiteSpace() || parsedLine[0] == string.Empty) 
            {
                pokemon.Guid = Guid.NewGuid();
            } 
            else
            {
                var parsedGuid = Guid.TryParse(parsedLine[0], out var guid);

                if (!parsedGuid)
                {
                    errors = "The Id doesn't match the following format: " + Guid.Empty;
                    return null;
                }

                pokemon.Guid = guid;
            }

            if (parsedLine[1].IsWhiteSpace() || parsedLine[1] == string.Empty)
            {
                errors = "The Gender is required";
                return null;
            }

            if (parsedLine[1] != "0" || parsedLine[1] != "1")
            {
                errors = "The Gender can only be 0 (male); 1 (female); 2 (genderless) or 3 (ditto)";
                return null;
            }

            int.TryParse(parsedLine[1], out var gender);

            pokemon.Gender = (PokemonGenderEnum)gender;

            pokemon.Name = parsedLine[2];


            if (parsedLine[3] != string.Empty)
            {
                var parsedNature = int.TryParse(parsedLine[3], out var nature);

                if (!parsedNature || nature < 0 || nature > 24)
                {
                    errors = "The Nature has to be a number between 0 and 24";
                    return null;
                }

                pokemon.Nature = (PokemonNatureEnum)nature;
            }
            int[] iVs = new int[6];
            for(int j = 0; j < 6; ++j)
            {

            }

            return pokemon;
        }

        private async Task ExportCsv()
        {
            string csv = "column1,column2";

            var module = await GetModule();

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
