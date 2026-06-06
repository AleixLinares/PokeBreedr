using PokeBreedr.Enums;
using PokeBreedr.Models;

namespace PokeBreedr.Services
{
    public class BreederService
    {

        // Hauria de ser el dto?
        public List<PokemonInfo> BreedPokemons(List<PokemonInfo> pokemons, ConfigCardInfo configuration)
        {
            pokemons = this.FilterInvalidPokemons(pokemons, configuration);

            return pokemons;
        }

        public List<PokemonInfo> FilterInvalidPokemons(List<PokemonInfo> pokemons, ConfigCardInfo configuration)
        {
            if (configuration.AlfaFilter == AlphaFilterEnum.AlphasOnly)
            {
                pokemons = pokemons.Where(i => i.IsAlfa).ToList();
            }
            else if (configuration.AlfaFilter == AlphaFilterEnum.NonAlphasOnly)
            {
                pokemons = pokemons.Where(i => !i.IsAlfa).ToList();
            }

            if (configuration.SelectedNatures != null && configuration.SelectedNatures.Count == 0)
            {
                pokemons.Where(i => configuration.SelectedNatures.Contains(i.Nature.ToString()));
            }

            var eggroup1 = configuration.OnlyEggGroup1;
            var eggroup2 = configuration.OnlyEggGroup2;

            if  (eggroup1 != null)
            {
                pokemons = pokemons.Where(i => i.EggGroup1 == eggroup1 || i.EggGroup2 == eggroup1).ToList();
            }

            if (eggroup2 != null)
            {
                pokemons = pokemons.Where(i => i.EggGroup1 == eggroup2 || i.EggGroup2 == eggroup2).ToList();
            }           

            return pokemons;

        }

    }
}
