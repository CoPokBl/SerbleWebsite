using System.Security;
using GeneralPurposeLib;
using Newtonsoft.Json;
using SerbleWebsite.Data.Schemas;
using JsonException = System.Text.Json.JsonException;

namespace SerbleWebsite.Data; 

/*
 * Dotnet's builtin JSON serializer does not seem to work with truples.
 * So I'm using Newtonsoft.Json.
 */

public class FileStorageService : IStorageService {
    
    private List<User> _users = new();
    private List<OAuthApp> _apps = new();

    public void Init() {
        _users = new List<User>();
        _apps = new List<OAuthApp>();
        
        // Add dummy data
        _users.Add(new User {
            Id = Guid.NewGuid().ToString(),
            Username = "admin",
            PasswordHash = "e",
            PermLevel = 0
        });
        OAuthApp app = new (_users.First().Id) {
            Name = "Test App",
            Description = "Test App"
        };
        _apps.Add(app);

        Logger.Info("Loading data from data.json...");
        if (File.Exists("data.json")) {
            string jsonData = File.ReadAllText("data.json");
            (List<User>, List<OAuthApp>) data = JsonConvert.DeserializeObject<(List<User>, List<OAuthApp>)>(jsonData);
            _users = data.Item1;
            _apps = data.Item2;
            Logger.Info("Loaded data from data.json");
        } else {
            Logger.Info("No data.json found, creating new data.json");
            File.WriteAllText("data.json", JsonConvert.SerializeObject((_users, _apps)));
            Logger.Info("Created new data.json");
        }
        Logger.Info("Data loaded");
    }

    public void Deinit() {
        bool retry = true;
        while (retry) {
            retry = false;
            bool error = false;
            string errorText = "Unspecified error";
            Logger.Info("Saving data to data.json...");
            try {
                File.WriteAllText("data.json", JsonConvert.SerializeObject((_users, _apps)));
                Logger.Info("Saved data to data.json");
            }
            catch (JsonException e) {
                Logger.Error($"Failed to save data to data.json: The data failed to serialize: {e.Message}");
                Logger.Error("----- Data will not be saved -----");
            } catch (IOException e) {
                errorText = $"Failed to save data to data.json (IOException): {e.Message}";
                error = true;
            } catch (UnauthorizedAccessException) {
                errorText =
                    $"Can't save data due to unauthorized access. Please make sure you have write access to: {Directory.GetCurrentDirectory()}";
                error = true;
            } catch (SecurityException) {
                errorText =
                    $"Can't save data due to unauthorized access. Please make sure you have access to: {Directory.GetCurrentDirectory()}";
                error = true;
            }

            if (!error) continue;
            Logger.Error(errorText);
            string? input = null;
            while (input == null) {
                Console.WriteLine("Would you like to retry? (y/n)");
                input = Console.ReadLine();
                if (input == null) continue;
                if (input.ToLower() == "y") {
                    retry = true;
                    Logger.Info("Reattempting to save data...");
                }
                else {
                    Logger.Info("----- Data will not be saved -----");
                }
            }
        }
    }

    public void AddUser(User userDetails, out User newUser) {
        newUser = userDetails;
        newUser.Id = Guid.NewGuid().ToString();
        _users.Add(newUser);
    }

    public void GetUser(string userId, out User? user) {
        user = _users.FirstOrDefault(u => u.Id == userId)!;
    }

    public void UpdateUser(User userDetails) {
        User? user = _users.FirstOrDefault(u => u.Id == userDetails.Id);
        if (user == null) return;
        int index = _users.IndexOf(user);
        _users[index] = userDetails;
    }

    public void DeleteUser(string userId) {
        _users.RemoveAll(u => u.Id == userId);
    }

    public void GetUserFromName(string userName, out User? user) {
        user = _users.FirstOrDefault(u => u.Username == userName);
    }

    public void AddOAuthApp(OAuthApp app) {
        _apps.Add(app);
    }

    public void GetOAuthApp(string appId, out OAuthApp? app) {
        app = _apps.FirstOrDefault(a => a.Id == appId);
    }

    public void UpdateOAuthApp(OAuthApp app) {
        OAuthApp? appToUpdate = _apps.FirstOrDefault(a => a.Id == app.Id);
        if (appToUpdate == null) return;
        int index = _apps.IndexOf(appToUpdate);
        _apps[index] = app;
    }

    public void DeleteOAuthApp(string appId) {
        _apps.RemoveAll(a => a.Id == appId);
    }

    public void GetOAuthAppsFromUser(string userId, out OAuthApp[] apps) {
        apps = _apps.Where(a => a.OwnerId == userId).ToArray();
    }
}