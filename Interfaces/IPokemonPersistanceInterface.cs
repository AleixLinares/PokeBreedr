using PokeBreedr.Models;

namespace PokeBreedr.Interfaces
{
    public interface IPokemonPersistanceInterface
    {

        Task Save(PokemonInfo pokemonInfo);

        Task Delete(Guid pokemonInfoId);

        Task<PokemonInfo?> Obtain(Guid pokemonInfoID);

        Task<List<PokemonInfo>> GetAll();
    }
}
