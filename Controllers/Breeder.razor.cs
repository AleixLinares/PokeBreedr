using Microsoft.AspNetCore.Components;
using PokeBreedr.Interfaces;
using PokeBreedr.Models;
using PokeBreedr.Dto;

namespace PokeBreedr.Pages
{
    public partial class Breeder
    {
        [Inject]
        private IBreedConfigPersistenceService BreedConfigPersistenceService { get; set; } = default!;

        private List<ConfigCardInfo> cards = new List<ConfigCardInfo>();

        public bool isLoading = true;

        protected override async Task OnInitializedAsync()
        {
            List<ConfigCardInfoDto> dtoList = await BreedConfigPersistenceService.GetAll();

            foreach (ConfigCardInfoDto pokemon in dtoList)
            {
                cards.Add(new ConfigCardInfo(pokemon));
            }
            isLoading = false;
        }
    }
}
