using Microsoft.AspNetCore.Components;
using PokeBreedr.Interfaces;
using PokeBreedr.Models;
using PokeBreedr.Dto;

namespace PokeBreedr.Pages
{
    public partial class Test
    {
        [Inject]
        private IPokemonPersistenceService PokemonPersistanceService { get; set; } = default!;

        private List<PokemonInfo> cards = new List<PokemonInfo>();

        public bool isLoading = true;

        protected override async Task OnInitializedAsync()
        {
            List<PokemonInfoDto> dtoList = await PokemonPersistanceService.GetAll();

            foreach (PokemonInfoDto pokemon in dtoList)
            {
                cards.Add(new PokemonInfo(pokemon));
            }
            isLoading = false;
        }
    }
}
