using SerbleWebsite.Data.Schemas;

namespace SerbleWebsite.Data; 

public class MySqlStorageService : IStorageService {
    public void Init() {
        throw new NotImplementedException();
    }

    public void Deinit() {
        throw new NotImplementedException();
    }

    public void AddUser(User userDetails, out User newUser) {
        throw new NotImplementedException();
    }

    public void GetUser(string userId, out User? user) {
        throw new NotImplementedException();
    }

    public void UpdateUser(User userDetails) {
        throw new NotImplementedException();
    }

    public void DeleteUser(string userId) {
        throw new NotImplementedException();
    }

    public void GetUserFromName(string userName, out User? user) {
        throw new NotImplementedException();
    }

    public void AddOAuthApp(OAuthApp app) {
        throw new NotImplementedException();
    }

    public void GetOAuthApp(string appId, out OAuthApp? app) {
        throw new NotImplementedException();
    }

    public void UpdateOAuthApp(OAuthApp app) {
        throw new NotImplementedException();
    }

    public void DeleteOAuthApp(string appId) {
        throw new NotImplementedException();
    }

    public void GetOAuthAppsFromUser(string userId, out OAuthApp[] apps) {
        throw new NotImplementedException();
    }
}