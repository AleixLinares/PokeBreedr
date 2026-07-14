using PokeBreedr.Dto;
using PokeBreedr.Enums;
using PokeBreedr.Interfaces;
using PokeBreedr.Utils;

namespace PokeBreedr.Services
{
    public class ImportExportPokemonService : IImportExportPokemonService
    {
        public List<PokemonInfoDto> ImportPokemons(List<PokemonInfoDto> pokemons, Dictionary<string,PokemonCSVData> pokemonsInfo, string csv, out string results)
        {
            results = string.Empty;

            Dictionary<Guid, PokemonInfoDto> currentPokemonDictionary = pokemons.ToDictionary(x => x.Guid);

            string[] lines = csv.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; ++i)
            {
                string line = lines[i];

                PokemonInfoDto? pokemon = this.TryParseCsvLine(line, pokemonsInfo.Keys.ToList(), out string error);

                if (pokemon == null)
                {
                    string resultLine = "row: " + i + "," + line + "," + "Error: " + error + "\n";
                    results += resultLine;
                    continue;
                }

                var pokemonInfo = pokemonsInfo[pokemon.Pokemon];

                var eggroup = pokemonInfo.EggGroup1;

                if (pokemon.Gender == PokemonGenderEnum.Ditto && eggroup != "Ditto")
                {
                    string resultLine = "row: " + i + "," + line + "," + "Error: " + "Only a Ditto can be Gender Ditto" + "\n";
                    results += resultLine;
                    continue;
                }
                else if (pokemon.Gender == PokemonGenderEnum.Genderless && eggroup != "Genderless")
                {
                    string resultLine = "row: " + i + "," + line + "," + "Error: " + "This pokemon cannot be genderless" + "\n";
                    results += resultLine;
                    continue;
                }

                pokemon.EggGroup1 = pokemonInfo.EggGroup1;
                pokemon.EggGroup2 = pokemonInfo.EggGroup2;

                if (pokemon.Pokemon == "Ditto")
                {
                    pokemon.Gender = PokemonGenderEnum.Ditto;
                }
                else if (eggroup == "Genderless")
                {
                    pokemon.Gender = PokemonGenderEnum.Genderless;
                }

                currentPokemonDictionary[pokemon.Guid] = pokemon;
            }
            
            return currentPokemonDictionary.Values.ToList();
        }

        public string ExportPokemons(List<PokemonInfoDto> pokemons)
        {
            string result = string.Empty;

            foreach (var pokemon in pokemons)
            {
                var nature = pokemon.Nature.HasValue ? pokemon.Nature.Value.ToString() : string.Empty;

                string particles = string.Join(";", pokemon.Particles);

                result += $"{pokemon.Guid},{pokemon.Pokemon},{pokemon.Name}," +
                    $"{pokemon.IsAlfa},{pokemon.HasHiddenAbility},{pokemon.Gender.ToString()},{pokemon.Nature?.ToString()}," +
                    $"{pokemon.HpIv},{pokemon.AttackIv},{pokemon.DefenseIv},{pokemon.SpAttackIv},{pokemon.SpDefenseIv},{pokemon.SpeedIv},{particles}\n";
            }

            return result;
        }

        private PokemonInfoDto? TryParseCsvLine(string line, List<string> validPokemons, out string errors)
        {
            var parsedLine = line.Split(',', StringSplitOptions.TrimEntries);

            errors = string.Empty;

            if (parsedLine.Length != 14)
            {
                if (parsedLine.Length == 13)
                {
                    var temp = (string[])parsedLine.Clone();

                    parsedLine = new string[14];

                    for (int i = 0; i < 13; ++i)
                    {
                        parsedLine[i] = temp[i];
                    }

                    parsedLine[13] = string.Empty;
                }
                else
                {
                    errors = "The line doesn't match the expected structure separated by comas" +
                        ": Id; Pokemon; Nickname; IsAlpha; HasHiddenAbility; Gender; Nature; HpIv; AttackIv; DefenseIv; SpAttackIv; SpDefenseIv; SpeedIv; Particles";
                    return null;
                }
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
                    errors = "Id: invalid format. Expected the following format: " + Guid.Empty;
                    return null;
                }

                pokemon.Guid = guid;
            }

            if (parsedLine[1] == string.Empty || parsedLine[1].IsWhiteSpace() || !validPokemons.Contains(parsedLine[1], StringComparer.OrdinalIgnoreCase))
            {
                errors = "Pokemon: is required and must be valid";
                return null;
            }

            pokemon.Pokemon = char.ToUpperInvariant(parsedLine[1][0]) + parsedLine[1][1..];

            if (parsedLine[2] != string.Empty || parsedLine[2].IsWhiteSpace())
            {
                pokemon.Name = parsedLine[2];
            }

            var validValues = new[] { string.Empty, "0", "1", "true", "false" };

            if (!parsedLine[3].IsWhiteSpace() && !validValues.Contains(parsedLine[3], StringComparer.OrdinalIgnoreCase))
            {
                errors = "IsAlpha: invalid value. Allowed values are false, 0, empty, true, or 1.";
                return null;
            }

            pokemon.IsAlfa =
                parsedLine[3].Equals("1") ||
                parsedLine[3].Equals("true", StringComparison.OrdinalIgnoreCase);

            if (!parsedLine[4].IsWhiteSpace() && !validValues.Contains(parsedLine[4], StringComparer.OrdinalIgnoreCase))
            {
                errors = "HasHiddenAbility: invalid value. Allowed values are false, 0, empty, true, or 1.";
                return null;
            }

            pokemon.HasHiddenAbility =
                parsedLine[4].Equals("1") ||
                parsedLine[4].Equals("true", StringComparison.OrdinalIgnoreCase);

            if (!parsedLine[5].TryParseEnum<PokemonGenderEnum>(out var genderEnum))
            {
                errors = $"Gender: is required. Allowed values are male (0), female (1), genderless (2), or ditto (0).";
                return null;
            }

            pokemon.Gender = genderEnum;

            if (parsedLine[6] != string.Empty && !parsedLine[6].IsWhiteSpace())
            {
                if (!parsedLine[6].TryParseEnum<PokemonNatureEnum>(out var pokemonNature))
                {
                    errors = "Nature: invalid value.";
                    return null;
                }

                pokemon.Nature = pokemonNature;
            }
            byte[] iVs = new byte[6];
            int j, k;
            for (j = 7, k = 0; k < 6; ++j, ++k)
            {
                if (!byte.TryParse(parsedLine[j], out var iv))
                {
                    errors = "IVs: must be a number between 0 and 31.";
                    return null;
                }
                if (iv < 0 || iv > 31)
                {
                    errors = "IVs: must be a number between 0 and 31.";
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
                var particles = parsedLine[13].Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                foreach (var particle in particles)
                {
                    if (!particle.TryParseEnum<ParticleEnum>(out var particleType))
                    {
                        errors = $"Particle: '{particle}' does not exist.";
                        return null;
                    }
                    pokemon.Particles.Add(particleType);
                }

            }

            return pokemon;
        }
    }
}
