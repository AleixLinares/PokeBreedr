using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PokeBreedr;
using PokeBreedr.Interfaces;
using PokeBreedr.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IPokemonPersistenceService, PokemonLocalStorageService>();
builder.Services.AddScoped<IBreedConfigPersistenceService, BreedConfigLocalStorageService>();

builder.Services.AddScoped<PokemonInitialLoad>();
builder.Services.AddScoped<BreederService>();

var host = builder.Build();

var store = host.Services.GetRequiredService<PokemonInitialLoad>();
await store.InitializeAsync();

await host.RunAsync();
