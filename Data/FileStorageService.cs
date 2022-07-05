using System.Security;
using System.Text.Json;
using GeneralPurposeLib;

namespace SerbleWebsite.Data; 

public class FileStorageService : IStorageService {
    
    private List<User> _users = new();

    public void Init() {
        _users = new List<User>();
        Logger.Info("Loading data from data.json...");
        if (File.Exists("data.json")) {
            _users = JsonSerializer.Deserialize<List<User>>(File.ReadAllText("data.json")) ?? new List<User>();
            Logger.Info("Loaded data from data.json");
        } else {
            Logger.Info("No data.json found, creating new data.json");
            File.WriteAllText("data.json", JsonSerializer.Serialize(_users));
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
                File.WriteAllText("data.json", JsonSerializer.Serialize(_users));
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
}