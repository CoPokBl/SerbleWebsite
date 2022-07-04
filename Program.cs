
using GeneralPurposeLib;
using SerbleWebsite.Data;
using LogLevel = GeneralPurposeLib.LogLevel;

namespace SerbleWebsite;

public static class Program {
    
    private static ConfigManager? _configManager;
    private static readonly Dictionary<string, string> ConfigDefaults = new() {
        { "bind_url", "http://*:5000" },
        { "storage_service", "http" },
        { "http_authorization_token", "my very secure auth token" },
        { "http_url", "https://myverysecurestoragebackend.io/" },
        { "my_host" , "https://theplacewherethisappisaccessable.com/" },
        { "token_issuer", "CoPokBl" },
        { "token_audience", "Privileged Users" },
        { "token_secret" , Guid.NewGuid().ToString() }
    };
    public static Dictionary<string, string>? Config;
    public static IStorageService? StorageService;
    
    private static int Main(string[] args) {
        // Logger
        Logger.Init(LogLevel.Debug);

        // Config
        Logger.Info("Loading config...");
        _configManager = new ConfigManager("config.json", ConfigDefaults);
        Config = _configManager.LoadConfig();
        Logger.Info("Config loaded.");
        
        // Storage service
        try {
            StorageService = Config["storage_service"] switch {
                "http" => null,
                _ => throw new Exception("Unknown storage service")
            };
        }
        catch (Exception e) {
            if (e.Message != "Unknown storage service") throw;
            Logger.Error("Invalid storage service specified in config.");
            Logger.WaitFlush();
            return 1;
        }

        // Init storage
        Logger.Info("Initializing storage...");
        try {
            StorageService?.Init();
        }
        catch (Exception e) {
            Logger.Error("Failed to initialize storage");
            Logger.Error(e);
            Logger.WaitFlush();
            return 1;
        }

        if (args.Length != 0) {

            switch (args[0]) {
                
                default:
                    Console.WriteLine("Unknown command");
                    return 1;

            }
        }

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddControllers();
        builder.WebHost.UseUrls(Config["bind_url"]);

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment()) {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        
        app.UseStaticFiles();
        app.UseRouting();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        bool didError = false;
        try {
            app.Run();
            Logger.Info("Server stopped with no errors.");
        }
        catch (Exception e) {
            Logger.Error(e);
            Logger.Error("Server stopped with error.");
            didError = true;
        }
        
        // Shutdown storage
        Logger.Info("Shutting down storage...");
        try {
            StorageService?.Deinit();
        }
        catch (Exception e) {
            Logger.Error("Failed to shutdown storage");
            Logger.Error(e);
        }

        Logger.WaitFlush();
        return didError ? 1 : 0;
    }

}