using PokeBreedr.Dto;
using PokeBreedr.Models;

namespace PokeBreedr.Interfaces
{
    public interface IPokemonPersistenceService
    {

        Task Save(PokemonInfoDto pokemonInfo);

        Task Delete(Guid pokemonInfoId);

        Task<PokemonInfoDto?> Obtain(Guid pokemonInfoID);

        Task<List<PokemonInfoDto>> GetAll();
    }
}
