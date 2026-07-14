using PokeBreedr.Dto;
using PokeBreedr.Services;
using PokeBreedr.Tests.Fixtures;
using Xunit;

namespace PokeBreedr.Tests;

public class TestImportExportPokemonService : IClassFixture<ImportExportPokemonServiceFixture>, IClassFixture<PokemonCsvFixture>
{

    private readonly ImportExportPokemonService ImportExportPokemonService;

    private readonly Dictionary<string,PokemonCSVData> PokemonCsvData;

    // A fixture is the equivalent of @BeforeAll in Junit
    // For the JUnit @Before equivalent, initialize in the test class constructor.
    public TestImportExportPokemonService(
        ImportExportPokemonServiceFixture importExportFixture,
        PokemonCsvFixture pokemonCsvFixture)
    {
        ImportExportPokemonService = importExportFixture.Service;
        PokemonCsvData = pokemonCsvFixture.PokemonsInfo;
    }

    /// <summary>
    /// Import a complete correct example, check that is imported correctly, and export again and check its still correct.
    /// </summary>
    [Fact]
    public void ImportExportCompletePokemonCsv()
    {
        string csv = "802202c9-d394-4fb5-a303-99b386eadeab,Pikachu,Test,True,False,Male,Naive,31,5,16,22,9,26,Present;EerieHowl;PumpkidsTreat\n" +
            "a81b2982-56cc-4076-a4d3-9f9caf63919e,Ditto,,False,False,Ditto,Adamant,31,0,0,31,0,0,Present;LotusField\n" +
            "35ca0fe5-4972-4dcc-82a1-e75bc3c530ab,Charmander,,False,False,Female,,0,0,0,0,0,0,\n" +
            "37ac98d9-4130-4e4d-989f-1089bee12991,Pidgeotto,,True,True,Female,Naughty,26,23,31,31,31,31,Zodiac;Fireworks\n";

        var pokemons = ImportExportPokemonService.ImportPokemons(new List<PokemonInfoDto>(), PokemonCsvData, csv, out var result);

        Assert.Equal(string.Empty, result);
        Assert.Single(pokemons, p => p.Name == "Test" && p.Pokemon == "Pikachu");
        Assert.Single(pokemons, p => p.Pokemon == "Ditto");
        Assert.Single(pokemons, p => p.Pokemon == "Charmander");
        Assert.Single(pokemons, p => p.Pokemon == "Pidgeotto");

        string exportCsv = ImportExportPokemonService.ExportPokemons(pokemons);

        Assert.Equal(csv, exportCsv);
    }

    [Fact]
    public void ImportExportPokemonCsvWithErrors()
    {
        string csv =
            ",NotAPokemon,Test,True,False,Male,Naive,31,5,16,22,9,26,Present;EerieHowl;PumpkidsTreat\n" +     // Wrong Pokémon name
            ",Pikachu,Test,True,False,Male,Naive,5,16,22,9,26,\n" + // Missing column
            ",Pikachu,Test,True,False,Male,Naive,5,16,22,9,26\n" + // Missing column, without the final coma
            ",Pikachu,Test,True,False,Male,Naive,31,5,16,22,9,26,,\n" + // Too many column
            ",Charmander,,False,False,Female,NotANature,0,0,0,0,0,0\n" + // Invalid nature
            ",Pidgeotto,,Maybe,True,Female,Naughty,26,23,31,31,31,31,Zodiac;Fireworks\n" + // Invalid boolean
            ",Pikachu,Test,True,False,Male,Naive,99,5,16,22,9,26,Present;EerieHowl\n" +  // Invalid IV
            ",Ditto,,False,False,Ditto,Adamant,-1,0,0,31,0,0,Present;LotusField\n" +     // Negative IV
            ",,Test,True,False,Male,Naive,31,5,16,22,9,26,Present\n" + // Missing pokemon
            "333333333333-3333-3333-333333333333b,Ditto,Test,True,False,Male,Naive,31,5,16,22,9,26,\n" +    // Wrong GUID format
            ",Pikachu,Test,True,False,Genderless,,31,5,16,22,9,26,\n" + // Wrong Gender
            ",Pikachu,Test,True,False,Ditto,,31,5,16,22,9,26,\n" + // Wrong Gender
            ",Pikachu,Test,True,False,,,31,5,16,22,9,26,\n"; // No Gender

        string expectedResult =
            "row: 0,,NotAPokemon,Test,True,False,Male,Naive,31,5,16,22,9,26,Present;EerieHowl;PumpkidsTreat,Error: Pokemon: is required and must be valid\n" +
            "row: 1,,Pikachu,Test,True,False,Male,Naive,5,16,22,9,26,,Error: IVs: must be a number between 0 and 31.\n" +
            "row: 2,,Pikachu,Test,True,False,Male,Naive,5,16,22,9,26,Error: The line doesn't match the expected structure separated by comas: Id; Pokemon; Nickname; IsAlpha; HasHiddenAbility; Gender; Nature; HpIv; AttackIv; DefenseIv; SpAttackIv; SpDefenseIv; SpeedIv; Particles\n" +
            "row: 3,,Pikachu,Test,True,False,Male,Naive,31,5,16,22,9,26,,,Error: The line doesn't match the expected structure separated by comas: Id; Pokemon; Nickname; IsAlpha; HasHiddenAbility; Gender; Nature; HpIv; AttackIv; DefenseIv; SpAttackIv; SpDefenseIv; SpeedIv; Particles\n" +
            "row: 4,,Charmander,,False,False,Female,NotANature,0,0,0,0,0,0,Error: Nature: invalid value.\n" +
            "row: 5,,Pidgeotto,,Maybe,True,Female,Naughty,26,23,31,31,31,31,Zodiac;Fireworks,Error: IsAlpha: invalid value. Allowed values are false, 0, empty, true, or 1.\n" +
            "row: 6,,Pikachu,Test,True,False,Male,Naive,99,5,16,22,9,26,Present;EerieHowl,Error: IVs: must be a number between 0 and 31.\n" +
            "row: 7,,Ditto,,False,False,Ditto,Adamant,-1,0,0,31,0,0,Present;LotusField,Error: IVs: must be a number between 0 and 31.\n" +
            "row: 8,,,Test,True,False,Male,Naive,31,5,16,22,9,26,Present,Error: Pokemon: is required and must be valid\n" +
            "row: 9,333333333333-3333-3333-333333333333b,Ditto,Test,True,False,Male,Naive,31,5,16,22,9,26,,Error: Id: invalid format. Expected the following format: 00000000-0000-0000-0000-000000000000\n" +
            "row: 10,,Pikachu,Test,True,False,Genderless,,31,5,16,22,9,26,,Error: This pokemon cannot be genderless\n" +
            "row: 11,,Pikachu,Test,True,False,Ditto,,31,5,16,22,9,26,,Error: Only a Ditto can be Gender Ditto\n" +
            "row: 12,,Pikachu,Test,True,False,,,31,5,16,22,9,26,,Error: Gender: is required. Allowed values are male (0), female (1), genderless (2), or ditto (0).\n";

        ImportExportPokemonService.ImportPokemons(new List<PokemonInfoDto>(), PokemonCsvData, csv, out var result);

        Assert.Equal(expectedResult, result);

    }
}