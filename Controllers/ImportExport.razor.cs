using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
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

        private IJSObjectReference? importExportCsvModule;

        private bool importFinished;
        private string resultCsv = string.Empty;
        private List<string> validPokemons = new List<string>();

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

                if (pokemon == null) 
                {
                    string resultLine = "row: " + i + "," + line + "," + "Error: " + error + "\n";
                    resultCsv += resultLine;
                    continue;
                }

                var pokemonInfo = await PokemonInitialLoad.GetPokemonInfo(pokemon.Pokemon);

                var eggroup = pokemonInfo.EggGroup1;

                if(pokemon.Gender == PokemonGenderEnum.Ditto && eggroup != "Ditto")
                {
                    string resultLine = "row: " + i + "," + line + "," + "Error: " + "Only a Ditto can be Gender Ditto" + "\n";
                    resultCsv += resultLine;
                    continue;
                }
                else if(pokemon.Gender == PokemonGenderEnum.Genderless && eggroup != "Genderless")
                {
                    string resultLine = "row: " + i + "," + line + "," + "Error: " + "This pokemon cannot be genderless" + "\n";
                    resultCsv += resultLine;
                    continue;
                }

                if(pokemon.Pokemon == "Ditto")
                {
                    pokemon.Gender = PokemonGenderEnum.Ditto;
                }
                else if(eggroup == "Genderless")
                {
                    pokemon.Gender = PokemonGenderEnum.Genderless;
                }

                currentPokemonDictionary.Add(pokemon.Guid, pokemon);
            }

            var newPokemonsList = currentPokemonDictionary.Values.ToList();

            await this.PokemonPersistenceService.SaveAll(newPokemonsList);
            importFinished = true;
        }

        private PokemonInfoDto? TryParseCsvLine(string line, out string errors)
        {
            var parsedLine = line.Split(',', StringSplitOptions.TrimEntries);

            errors = string.Empty;

            if (parsedLine.Length != 14)
            {
                errors = "The line doesn't match the expected structure separated by comas" +
                    ": Id; Pokemon; Nickname; IsAlpha; HasHiddenAbility; Gender; Nature; HpIv; AttackIv; DefenseIv; SpAttackIv; SpDefenseIv; SpeedIv; Particles";
                return null;
            }

            PokemonInfoDto pokemon = new PokemonInfoDto();

            if (parsedLine[0] == string.Empty || parsedLine[0].IsWhiteSpace()) 
            {
                pokemon.Guid = Guid.NewGuid();
            } 
            else
            {
                if (!Guid.TryParse(parsedLine[0], out var guid))
                {
                    errors = "The Id doesn't match the following format: " + Guid.Empty;
                    return null;
                }

                pokemon.Guid = guid;
            }

            if(parsedLine[1] == string.Empty || parsedLine[1].IsWhiteSpace() || !validPokemons.Contains(parsedLine[1], StringComparer.OrdinalIgnoreCase))
            {
                errors = "The Pokemon is required and has to be valid";
                return null;
            }

            pokemon.Pokemon = char.ToUpperInvariant(parsedLine[1][0]) + parsedLine[1][1..];

            if (parsedLine[2] != string.Empty || parsedLine[2].IsWhiteSpace())
            {
                pokemon.Name = parsedLine[2];
            }

            var validValues = new[] { string.Empty, "0", "1", "true", "false" };

            if (!parsedLine[3].IsWhiteSpace() && !validValues.Contains(parsedLine[3],StringComparer.OrdinalIgnoreCase))
            {
                errors = "IsAlpha can only be false or 0 or empty if it's not alpha or true or 1 if it is";
                return null;
            }

            pokemon.IsAlfa =
                parsedLine[3].Equals("1") ||
                parsedLine[3].Equals("true", StringComparison.OrdinalIgnoreCase);

            if (!parsedLine[4].IsWhiteSpace() && !validValues.Contains(parsedLine[3],StringComparer.OrdinalIgnoreCase))
            {
                errors = "HasHiddenAbility can only be false or 0 or empty if doesn't have hidden ability or true or 1 if it has";
                return null;
            }

            pokemon.HasHiddenAbility =
                parsedLine[4].Equals("1") ||
                parsedLine[4].Equals("true", StringComparison.OrdinalIgnoreCase);

            if (!parsedLine[5].TryParseEnum<PokemonGenderEnum>(out var genderEnum))
            {
                Console.WriteLine(parsedLine[5]);
                errors = $"The Gender is required and must be male (0); female (1); genderless (2) or ditto (0)";
                return null;
            }


            if (parsedLine[6] != string.Empty && !parsedLine[5].IsWhiteSpace())
            {
                if (!parsedLine[6].TryParseEnum<PokemonNatureEnum>(out var pokemonNature))
                {
                    errors = "The Nature is not valid";
                    return null;
                }

                pokemon.Nature = pokemonNature;
            }
            byte[] iVs = new byte[6];
            int j, k;
            for (j = 7, k= 0; j < 6; ++j, ++k)
            {
                if (!byte.TryParse(parsedLine[j], out var iv) || iv < 0 || iv > 31)
                {
                    errors = "The iVs have to be a number between 0 and 31";
                    return null;
                }

                iVs[k] = iv;
            }
            k = 0;
            pokemon.HpIv = iVs[k++];
            pokemon.AttackIv = iVs[k++];
            pokemon.DefenseIv = iVs[k++];
            pokemon.SpAttackIv = iVs[k++];
            pokemon.SpDefenseIv = iVs[k++];
            pokemon.SpeedIv = iVs[k];

            if (parsedLine[13] != string.Empty)
            {
                var particles = parsedLine[10].Split(';', StringSplitOptions.TrimEntries);

                foreach (var particle in particles)
                {
                    if(!particle.TryParseEnum<ParticleEnum>(out var particleType))
                    {
                        errors = "The particle" + particle + "doesn't exist";
                        return null;
                    }
                    pokemon.Particles.Add(particleType);
                }

            }

            return pokemon;
        }

        private async Task ExportCsv()
        {
            string csv = string.Empty;

            var module = await GetModule();

            List<PokemonInfoDto> currentPokemons = await PokemonPersistenceService.GetAll();

            foreach (var pokemon in currentPokemons)
            {
                var nature = pokemon.Nature.HasValue ? pokemon.Nature.Value.ToString() : string.Empty;

                string particles = string.Empty;

                foreach(var particle in pokemon.Particles)
                {
                    particles += $"{particle.ToString()};";
                }

                csv += $"{pokemon.Guid},{pokemon.Pokemon},{pokemon.Name}," +
                    $"{pokemon.IsAlfa},{pokemon.HasHiddenAbility},{pokemon.Gender.ToString()},{pokemon.Nature?.ToString()}," +
                    $"{pokemon.HpIv},{pokemon.AttackIv},{pokemon.DefenseIv},{pokemon.SpAttackIv},{pokemon.SpDefenseIv},{pokemon.SpeedIv},{particles}\n";
            }

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
