using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using SerbleWebsite;
using SerbleWebsite.Data;

Console.WriteLine("Loading Serble Page...");

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});
builder.Services.AddLocalisations();

WebAssemblyHost host = builder.Build();

// Init globalisation
IJSRuntime js = host.Services.GetRequiredService < IJSRuntime > ();
string lang = await js.InvokeAsync < string > ("getCultureLang");
await host.Services.SetLanguage(lang);
Console.WriteLine("Language: " + lang);

Localiser localiser = new();
HtmlInteractor html = new(js);
await html.SetHtml("blazor-unhandled-error", localiser["unknown-error-occured"]);
await html.SetHtml("blazor-reload-link", localiser["reload"]);

try {
    await host.RunAsync();
}
catch (Exception e) {
    Console.Error.WriteLine("Sorry, an error has occured: " + e.Message);
    Console.Error.WriteLine(e);
}
