using PokeBreedr.Dto;
using PokeBreedr.Enums;
using PokeBreedr.Models;

namespace PokeBreedr.Services
{
    public class BreederService
    {

        // Hauria de ser el dto?
        public List<PokemonInfoDto> BreedPokemons(List<PokemonInfoDto> pokemons, ConfigCardInfoDto configuration)
        {
            pokemons = this.FilterInvalidPokemons(pokemons, configuration);
            List<PokemonInfoDto> finalCombinations = new List<PokemonInfoDto>();

            while(pokemons.Count > 0)
            {
                PokemonInfoDto pokemonCandidate = pokemons.First();
                List<PokemonInfoDto> validPokemonsForCandidate = this.FilterInvalidPokemonsForCandidate(pokemonCandidate, pokemons);

                if (validPokemonsForCandidate.Count > 0)
                {
                    finalCombinations.AddRange(this.CombinationsByCandidateAndConfiguration(pokemonCandidate, validPokemonsForCandidate, configuration));
                }

                pokemons.RemoveAt(0);
            }      

            return pokemons;
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

            pokemons = this.FilterPokemnsForEgggroups(pokemons, configuration.OnlyEggGroup1, configuration.OnlyEggGroup2);

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
                pokemons = this.FilterPokemnsForEgggroups(pokemons, candidate.EggGroup1, candidate.EggGroup2);

                if (candidate.Gender == PokemonGenderEnum.Male)
                {
                    return pokemons.Where(i => i.Gender == PokemonGenderEnum.Female).ToList();
                }

                return pokemons.Where(i => i.Gender == PokemonGenderEnum.Male).ToList();
            }
        }

        private List<PokemonInfoDto> FilterPokemnsForEgggroups(List<PokemonInfoDto> pokemons, string? eggGroup1, string? eggGroup2)
        {
            if (eggGroup1 != null)
            {
                pokemons = pokemons.Where(i => i.EggGroup1 == eggGroup1 || i.EggGroup2 == eggGroup1).ToList();
            }

            if (eggGroup2 != null)
            {
                pokemons = pokemons.Where(i => i.EggGroup1 == eggGroup2 || i.EggGroup2 == eggGroup2).ToList();
            }

            return pokemons;
        }

        private List<PokemonInfoDto> CombinationsByCandidateAndConfiguration(PokemonInfoDto candidate, List<PokemonInfoDto> pokemons, ConfigCardInfoDto configuration)
        {
            List<PokemonInfoDto> combinations = new List<PokemonInfoDto>();

            foreach (PokemonInfoDto pokemonCheck in pokemons)
            {
                int differences = 0;
                bool keephv, keepatt, keepdef, keepspatt, keepspdef, keepspeed, keepnature;

                if (candidate.HpIv.InRange(configuration.MinHpIv, configuration.MaxHpIv) != pokemonCheck.HpIv.InRange(configuration.MinHpIv, configuration.MaxHpIv))
                {
                    ++differences;
                    keephv = true;
                }

                if (candidate.AttackIv.InRange(configuration.MinAttackIv, configuration.MaxAttackIv) != pokemonCheck.AttackIv.InRange(configuration.MinAttackIv, configuration.MaxAttackIv))
                {
                    ++differences;
                    keepatt = true;
                }

                if (candidate.DefenseIv.InRange(configuration.MinDefenseIv, configuration.MaxDefenseIv) != pokemonCheck.DefenseIv.InRange(configuration.MinDefenseIv, configuration.MaxDefenseIv))
                {
                    ++differences;
                    keepdef = true;
                }

                if (candidate.SpAttackIv.InRange(configuration.MinSpAttackIv, configuration.MaxSpAttackIv) != pokemonCheck.SpAttackIv.InRange(configuration.MinSpAttackIv, configuration.MaxSpAttackIv))
                {
                    ++differences;
                    keepspatt = true;
                }

                if (candidate.SpDefenseIv.InRange(configuration.MinSpDefenseIv, configuration.MaxSpDefenseIv) != pokemonCheck.SpDefenseIv.InRange(configuration.MinSpDefenseIv, configuration.MaxSpDefenseIv))
                {
                    ++differences;
                    keepspdef = true;
                }

                if (candidate.SpeedIv.InRange(configuration.MinSpeedIv, configuration.MaxSpeedIv) != pokemonCheck.SpeedIv.InRange(configuration.MinSpeedIv, configuration.MaxSpeedIv))
                {
                    ++differences;
                    keepspeed = true;
                }

                if (configuration.SelectedNatures.Contains(candidate.Nature.ToString()) != configuration.SelectedNatures.Contains(pokemonCheck.Nature.ToString()))
                {
                    ++differences;
                    keepnature = true;
                }

                if (differences == 2)
                {
                    PokemonInfoDto newCombination = new PokemonInfoDto();

                    newCombination.Guid = Guid.NewGuid();

                    if (candidate.EggGroup1 == "Ditto")
                    {
                        newCombination.Pokemon = pokemonCheck.Pokemon;
                    }
                    else if (pokemonCheck.EggGroup1 == "Ditto")
                    {
                        newCombination.Pokemon = candidate.Pokemon;
                    }
                    else if (candidate.EggGroup1 == "Genderless")
                    {
                        newCombination.Pokemon = candidate.Pokemon;
                    }
                    else if (candidate.Gender == PokemonGenderEnum.Female)
                    {
                        newCombination.Pokemon = candidate.Pokemon;
                    }
                    else
                    {
                        newCombination.Pokemon = pokemonCheck.Pokemon;
                    }

                    newCombination.IsAlfa = candidate.IsAlfa && pokemonCheck.IsAlfa;
                    combinations.Add(newCombination);
                }
            }

            return combinations;
        }
    }
}
