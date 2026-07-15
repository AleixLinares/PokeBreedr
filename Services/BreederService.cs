using PokeBreedr.Dto;
using PokeBreedr.Enums;
using PokeBreedr.Models;
using PokeBreedr.Utils;

namespace PokeBreedr.Services
{
    public class BreederService
    {
        private readonly IPokemonInitialLoad PokemonInitialLoad;

        public BreederService(IPokemonInitialLoad pokemonInitialLoad)
        {
            this.PokemonInitialLoad = pokemonInitialLoad;
        }

        // Hauria de ser el dto?
        public async Task<List<CombinationInfo>> BreedPokemons(List<PokemonInfoDto> pokemons, ConfigCardInfoDto configuration)
        {
            pokemons = this.FilterInvalidPokemons(pokemons, configuration);
            List<CombinationInfo> finalCombinations = new List<CombinationInfo>();

            while(pokemons.Count > 1)
            {
                PokemonInfoDto pokemonCandidate = pokemons.First();
                List<PokemonInfoDto> validPokemonsForCandidate = this.FilterInvalidPokemonsForCandidate(pokemonCandidate, pokemons);

                if (validPokemonsForCandidate.Count > 0)
                {
                    var currentCombinations = await this.CombinationsByCandidateAndConfiguration(pokemonCandidate, validPokemonsForCandidate, configuration);
                    finalCombinations.AddRange(currentCombinations);
                }

                pokemons.RemoveAt(0);
            }
            Console.WriteLine($"Total combinaciones: {finalCombinations.Count}");

            return finalCombinations;
        }

        public List<PokemonInfoDto> FilterInvalidPokemons(List<PokemonInfoDto> pokemons, ConfigCardInfoDto configuration)
        {
            if (configuration.AlfaFilter == AlphaFilterEnum.AlphasOnly)
            {
                pokemons = pokemons.Where(i => i.IsAlfa).ToList();
            }
            else if (configuration.AlfaFilter == AlphaFilterEnum.NonAlphasOnly)
            {
                pokemons = pokemons.Where(i => !i.IsAlfa).ToList();
            }

            pokemons = this.FilterPokemnsForEggGroups(pokemons, configuration.OnlyEggGroup1, configuration.OnlyEggGroup2, false);

            return pokemons;

        }

        public List<PokemonInfoDto> FilterInvalidPokemonsForCandidate(PokemonInfoDto candidate, List<PokemonInfoDto> pokemons)
        {
            if (candidate.EggGroup1 == "Ditto")
            {
                return pokemons.Where(i => i.EggGroup1 != "Ditto").ToList();
            }
            if (candidate.EggGroup1 == "Genderless") 
            {
                return pokemons.Where(i => i.Pokemon == candidate.Pokemon || i.Pokemon == "Ditto").ToList();
            }
            else
            {
                pokemons = this.FilterPokemnsForEggGroups(pokemons, candidate.EggGroup1, candidate.EggGroup2);

                if (candidate.Gender == PokemonGenderEnum.Male)
                {
                    return pokemons.Where(i => i.Gender == PokemonGenderEnum.Female || i.EggGroup1 == "Ditto").ToList();
                }

                return pokemons.Where(i => i.Gender == PokemonGenderEnum.Male || i.EggGroup1 == "Ditto").ToList();
            }
        }

        private List<PokemonInfoDto> FilterPokemnsForEggGroups(List<PokemonInfoDto> pokemons, string? eggGroup1, string? eggGroup2, bool throwError = true)
        {
            if (eggGroup1 != null && eggGroup1 != string.Empty)
            {
                if (eggGroup2 != null && eggGroup2 != string.Empty)
                {
                    pokemons = pokemons.Where(i => i.EggGroup1 == eggGroup1 || i.EggGroup2 == eggGroup1 || i.EggGroup1 == eggGroup2 || i.EggGroup2 == eggGroup2 || i.EggGroup1 == "Ditto").ToList();
                }
                else
                {
                    pokemons = pokemons.Where(i => i.EggGroup1 == eggGroup1 || i.EggGroup2 == eggGroup1 || i.EggGroup1 == "Ditto").ToList();
                }

                return pokemons;
            }

            if (throwError)  throw new Exception("Something went wrong when filtering Egg Groups");

            return pokemons;
        }

        private async Task<List<CombinationInfo>> CombinationsByCandidateAndConfiguration(PokemonInfoDto candidate, List<PokemonInfoDto> pokemons, ConfigCardInfoDto configuration)
        {
            List<CombinationInfo> combinations = new List<CombinationInfo>();

            foreach (PokemonInfoDto pokemonCheck in pokemons)
            {
                int differencesCandidate = 0;
                int differencesCheck = 0;
                byte[] flags = new byte[7];
                bool isInCandidate, isInCheck;

                if (!configuration.IgnoreHpIv && candidate.HpIv.InRange(configuration.MinHpIv, configuration.MaxHpIv, out isInCandidate)  != pokemonCheck.HpIv.InRange(configuration.MinHpIv, configuration.MaxHpIv, out isInCheck))
                {
                    if (isInCandidate)
                    {
                        ++differencesCandidate;
                        flags[0] = 1;
                    }
                    else
                    {
                        ++differencesCheck;
                        flags[0] = 2;
                    }
                }

                if (!configuration.IgnoreAttackIv && candidate.AttackIv.InRange(configuration.MinAttackIv, configuration.MaxAttackIv, out isInCandidate) != pokemonCheck.AttackIv.InRange(configuration.MinAttackIv, configuration.MaxAttackIv, out isInCheck))
                {
                    if (isInCandidate)
                    {
                        ++differencesCandidate;
                        flags[1] = 1;
                    }
                    else
                    {
                        ++differencesCheck;
                        flags[1] = 2;
                    }
                }

                if (!configuration.IgnoreDefenseIv && candidate.DefenseIv.InRange(configuration.MinDefenseIv, configuration.MaxDefenseIv, out isInCandidate) != pokemonCheck.DefenseIv.InRange(configuration.MinDefenseIv, configuration.MaxDefenseIv, out isInCheck))
                {
                    if (isInCandidate)
                    {
                        ++differencesCandidate;
                        flags[2] = 1;
                    }
                    else
                    {
                        ++differencesCheck;
                        flags[2] = 2;
                    }
                }

                if (!configuration.IgnoreSpAttackIv && candidate.SpAttackIv.InRange(configuration.MinSpAttackIv, configuration.MaxSpAttackIv, out isInCandidate) != pokemonCheck.SpAttackIv.InRange(configuration.MinSpAttackIv, configuration.MaxSpAttackIv, out isInCheck))
                {
                    if (isInCandidate)
                    {
                        ++differencesCandidate;
                        flags[3] = 1;
                    }
                    else
                    {
                        ++differencesCheck;
                        flags[3] = 2;
                    }
                }

                if (!configuration.IgnoreSpDefenseIv && candidate.SpDefenseIv.InRange(configuration.MinSpDefenseIv, configuration.MaxSpDefenseIv, out isInCandidate) != pokemonCheck.SpDefenseIv.InRange(configuration.MinSpDefenseIv, configuration.MaxSpDefenseIv, out isInCheck))
                {
                    if (isInCandidate)
                    {
                        ++differencesCandidate;
                        flags[4] = 1;
                    }
                    else
                    {
                        ++differencesCheck;
                        flags[4] = 2;
                    }
                }

                if (!configuration.IgnoreSpeedIv && candidate.SpeedIv.InRange(configuration.MinSpeedIv, configuration.MaxSpeedIv, out isInCandidate) != pokemonCheck.SpeedIv.InRange(configuration.MinSpeedIv, configuration.MaxSpeedIv, out isInCheck))
                {
                    if (isInCandidate)
                    {
                        ++differencesCandidate;
                        flags[5] = 1;
                    }
                    else
                    {
                        ++differencesCheck;
                        flags[5] = 2;
                    }
                }

                if (configuration.SelectedNatures.Contains(candidate.Nature.ToString()) != configuration.SelectedNatures.Contains(pokemonCheck.Nature.ToString()))
                {
                    if (configuration.SelectedNatures.Contains(candidate.Nature.ToString()))
                    {
                        ++differencesCandidate;
                        flags[6] = 1;
                    }
                    else
                    {
                        ++differencesCheck;
                        flags[6] = 2;
                    }
                }

                if (differencesCandidate == 1 && differencesCheck == 1)
                {
                    CombinationInfo newCombination = new CombinationInfo(candidate, pokemonCheck, flags);
                    try
                    {
                        var pokemonInfo = await this.PokemonInitialLoad.GetPokemonInfo(newCombination.Pokemon);

                        newCombination.PokemonEgg = pokemonInfo.PokemonEgg;
                        combinations.Add(newCombination);
                    }
                    catch (KeyNotFoundException)
                    {
                        // El Pokémon no existe en el diccionario.
                        // Ignoramos esta combinación y continuamos.
                    }
                }
            }

            return combinations;
        }
    }
}
