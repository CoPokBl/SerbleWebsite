using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SerbleWebsite;

Console.WriteLine("Loading Serble Page...");

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});

try {
    await builder.Build().RunAsync();
}
catch (Exception e) {
    Console.Error.WriteLine("Sorry, an error has occured: " + e.Message);
    Console.Error.WriteLine(e);
}
