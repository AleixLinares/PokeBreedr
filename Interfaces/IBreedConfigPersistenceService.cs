using PokeBreedr.Dto;
using PokeBreedr.Models;

namespace PokeBreedr.Interfaces
{
    public interface IBreedConfigPersistenceService
    {

        Task Save(ConfigCardInfoDto cardInfo);

        Task Delete(Guid cardInfoId);

        Task<ConfigCardInfoDto?> Obtain(Guid cardInfoID);

        Task<List<ConfigCardInfoDto>> GetAll();

        Task<bool> CheckIfNameUniqueAndNotEmpty(string? name, Guid? itemGuid);
    }
}
