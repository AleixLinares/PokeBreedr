using PokeBreedr.Dto;
using PokeBreedr.Enums;
using PokeBreedr.Models;
using PokeBreedr.Utils;
using System.Text.Json;

namespace PokeBreedr.Services
{
    public class BreederService
    {

        // Hauria de ser el dto?
        public List<CombinationInfo> BreedPokemons(List<PokemonInfoDto> pokemons, ConfigCardInfoDto configuration)
        {
            pokemons = this.FilterInvalidPokemons(pokemons, configuration);
            List<CombinationInfo> finalCombinations = new List<CombinationInfo>();

            while(pokemons.Count > 1)
            {
                PokemonInfoDto pokemonCandidate = pokemons.First();
                List<PokemonInfoDto> validPokemonsForCandidate = this.FilterInvalidPokemonsForCandidate(pokemonCandidate, pokemons);

                if (validPokemonsForCandidate.Count > 0)
                {
                    finalCombinations.AddRange(this.CombinationsByCandidateAndConfiguration(pokemonCandidate, validPokemonsForCandidate, configuration));
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

            pokemons = this.FilterPokemnsForEggGroups(pokemons, configuration.OnlyEggGroup1, configuration.OnlyEggGroup2);

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

        private List<PokemonInfoDto> FilterPokemnsForEggGroups(List<PokemonInfoDto> pokemons, string? eggGroup1, string? eggGroup2)
        {
            if (eggGroup1 != null)
            {
                pokemons = pokemons.Where(i => i.EggGroup1 == eggGroup1 || i.EggGroup2 == eggGroup1 || i.EggGroup1 == "Ditto").ToList();
            }

            if (eggGroup2 != null)
            {
                pokemons = pokemons.Where(i => i.EggGroup1 == eggGroup2 || i.EggGroup2 == eggGroup2 || i.EggGroup1 == "Ditto").ToList();
            }

            return pokemons;
        }

        private List<CombinationInfo> CombinationsByCandidateAndConfiguration(PokemonInfoDto candidate, List<PokemonInfoDto> pokemons, ConfigCardInfoDto configuration)
        {
            List<CombinationInfo> combinations = new List<CombinationInfo>();

            foreach (PokemonInfoDto pokemonCheck in pokemons)
            {
                int differences = 0;
                byte[] flags = new byte[7];
                bool isInCandidate, isInCheck;

                if (!configuration.IgnoreHpIv && candidate.HpIv.InRange(configuration.MinHpIv, configuration.MaxHpIv, out isInCandidate)  != pokemonCheck.HpIv.InRange(configuration.MinHpIv, configuration.MaxHpIv, out isInCheck))
                {
                    ++differences;
                    if (isInCandidate)
                    {
                        flags[0] = 1;
                    }
                    else
                    {
                        flags[0] = 2;
                    }
                }

                if (!configuration.IgnoreAttackIv && candidate.AttackIv.InRange(configuration.MinAttackIv, configuration.MaxAttackIv, out isInCandidate) != pokemonCheck.AttackIv.InRange(configuration.MinAttackIv, configuration.MaxAttackIv, out isInCheck))
                {
                    ++differences;
                    if (isInCandidate)
                    {
                        flags[1] = 1;
                    }
                    else
                    {
                        flags[1] = 2;
                    }
                }

                if (!configuration.IgnoreDefenseIv && candidate.DefenseIv.InRange(configuration.MinDefenseIv, configuration.MaxDefenseIv, out isInCandidate) != pokemonCheck.DefenseIv.InRange(configuration.MinDefenseIv, configuration.MaxDefenseIv, out isInCheck))
                {
                    ++differences;
                    if (isInCandidate)
                    {
                        flags[2] = 1;
                    }
                    else
                    {
                        flags[2] = 2;
                    }
                }

                if (!configuration.IgnoreSpAttackIv && candidate.SpAttackIv.InRange(configuration.MinSpAttackIv, configuration.MaxSpAttackIv, out isInCandidate) != pokemonCheck.SpAttackIv.InRange(configuration.MinSpAttackIv, configuration.MaxSpAttackIv, out isInCheck))
                {
                    ++differences;
                    if (isInCandidate)
                    {
                        flags[3] = 1;
                    }
                    else
                    {
                        flags[3] = 2;
                    }
                }

                if (!configuration.IgnoreSpDefenseIv && candidate.SpDefenseIv.InRange(configuration.MinSpDefenseIv, configuration.MaxSpDefenseIv, out isInCandidate) != pokemonCheck.SpDefenseIv.InRange(configuration.MinSpDefenseIv, configuration.MaxSpDefenseIv, out isInCheck))
                {
                    ++differences;
                    if (isInCandidate)
                    {
                        flags[4] = 1;
                    }
                    else
                    {
                        flags[4] = 2;
                    }
                }

                if (!configuration.IgnoreSpeedIv && candidate.SpeedIv.InRange(configuration.MinSpeedIv, configuration.MaxSpeedIv, out isInCandidate) != pokemonCheck.SpeedIv.InRange(configuration.MinSpeedIv, configuration.MaxSpeedIv, out isInCheck))
                {
                    ++differences;
                    if (isInCandidate)
                    {
                        flags[5] = 1;
                    }
                    else
                    {
                        flags[5] = 2;
                    }
                }

                if (configuration.SelectedNatures.Contains(candidate.Nature.ToString()) != configuration.SelectedNatures.Contains(pokemonCheck.Nature.ToString()))
                {
                    ++differences;
                    if (configuration.SelectedNatures.Contains(candidate.Nature.ToString()))
                    {
                        flags[6] = 1;
                    }
                    else
                    {
                        flags[6] = 2;
                    }
                }

                if (differences == 2)
                {
                    CombinationInfo newCombination = new CombinationInfo(candidate, pokemonCheck, flags);
                    combinations.Add(newCombination);
                }
            }

            return combinations;
        }
    }
}
