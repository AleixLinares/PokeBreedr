using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PokeBreedr;
using PokeBreedr.Interfaces;
using PokeBreedr.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IPokemonInitialLoad, PokemonInitialLoad>();
builder.Services.AddScoped<IPokemonPersistenceService, PokemonLocalStorageService>();
builder.Services.AddScoped<IBreedConfigPersistenceService, BreedConfigLocalStorageService>();
builder.Services.AddScoped<IImportExportPokemonService, ImportExportPokemonService>();

builder.Services.AddScoped<BreederService>();

builder.Services.AddSingleton<ToastService>();

var host = builder.Build();

var store = host.Services.GetRequiredService<IPokemonInitialLoad>();
await store.InitializeAsync();

await host.RunAsync();
