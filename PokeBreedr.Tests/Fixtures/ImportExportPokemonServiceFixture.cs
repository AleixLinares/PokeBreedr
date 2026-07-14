using PokeBreedr.Services;

namespace PokeBreedr.Tests.Fixtures
{
    public class ImportExportPokemonServiceFixture
    {
        public ImportExportPokemonService Service { get; }

        public ImportExportPokemonServiceFixture()
        {
            Service = new ImportExportPokemonService();
        }
    }
}