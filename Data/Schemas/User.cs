using GeneralPurposeLib;

namespace SerbleWebsite.Data.Schemas; 

public class User {
    
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    // 0=Disabled Account 1=Normal, 2=Admin
    public int PermLevel { get; set; }
    public string PermString { get; set; }
    
    // (appId, appSecret)
    public (string, string)[] AuthorizedApps {
        get {
            if (_obtainedAuthedApps != null) return _obtainedAuthedApps;
            ObtainAuthorizedApps();
            return _obtainedAuthedApps!;
        }
        set {
            if (_obtainedAuthedApps == null) ObtainAuthorizedApps();
            _obtainedAuthedApps = value;
        }
    }

    private (string, string)[]? _obtainedAuthedApps;
    private (string, string)[]? _originalAuthedApps;

    public IEnumerable<string> AuthorizedAppIds => AuthorizedApps.Select(x => x.Item1).ToArray();
    
    public User() {
        Id = "";
        Username = "";
        Email = "";
        PasswordHash = "";
        PermLevel = 0;
        PermString = "";
        _originalAuthedApps = Array.Empty<(string, string)>();
    }
    
    private void ObtainAuthorizedApps() {
        Program.StorageService!.GetAuthorizedApps(Id, out _originalAuthedApps);
        _obtainedAuthedApps = _originalAuthedApps;
        Logger.Debug($"Obtained Authorized Apps for {Username}");
    }

    public void RegisterChanges() {
        Logger.Debug($"Registering changes to user: '{Username}' with id: '{Id}'");
        
        Program.StorageService!.UpdateUser(this);

        if (_originalAuthedApps == null || _obtainedAuthedApps == null) {
            Logger.Debug("No changes to authorized apps were made");
            return;
        }
        
        // Find out which apps were added/removed
        (string, string)[] addedApps = _obtainedAuthedApps.Except(_originalAuthedApps).ToArray();
        (string, string)[] removedApps = _originalAuthedApps.Except(_obtainedAuthedApps).ToArray();
        
        // Add the new apps
        foreach ((string, string) app in addedApps) {
            Program.StorageService.AddAuthorizedApp(Id, app.Item1, app.Item2);
        }
        
        // Remove the removed apps
        foreach ((string, string) app in removedApps) {
            Program.StorageService.DeleteAuthorizedApp(Id, app.Item1);
        }
        
        Logger.Debug("Added/Removed authed apps: " + addedApps.Length + "/" + removedApps.Length);
    }
    
}