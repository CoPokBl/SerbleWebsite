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
            if (_obtainedAuthedApps == null) {
                Program.StorageService!.GetAuthorizedApps(Id, out _obtainedAuthedApps);
            }
            return _obtainedAuthedApps;
        }
        set {
            if (!_hasAuthedAppsBeenInitialized) {
                _originalAuthedApps = value;
                _hasAuthedAppsBeenInitialized = true;
            }
            _obtainedAuthedApps = value;
        }
    }
    
    private (string, string)[]? _obtainedAuthedApps;
    private (string, string)[] _originalAuthedApps;
    private bool _hasAuthedAppsBeenInitialized;

    public IEnumerable<string> AuthorizedAppIds => AuthorizedApps.Select(x => x.Item1).ToArray();
    
    public User() {
        Id = "";
        Username = "";
        Email = "";
        PasswordHash = "";
        PermLevel = 0;
        PermString = "";
        AuthorizedApps = Array.Empty<(string, string)>();
        _originalAuthedApps = Array.Empty<(string, string)>();
    }

    public void RegisterChanges() {
        Program.StorageService!.UpdateUser(this);

        if (!_hasAuthedAppsBeenInitialized || _obtainedAuthedApps == null) {
            return;
        }
        
        // Find out which apps were added/removed
        IEnumerable<(string, string)> addedApps = _obtainedAuthedApps.Except(_originalAuthedApps);
        IEnumerable<(string, string)> removedApps = _originalAuthedApps.Except(_obtainedAuthedApps);
        
        // Add the new apps
        foreach ((string, string) app in addedApps) {
            Program.StorageService.AddAuthorizedApp(Id, app.Item1, app.Item2);
        }
        
        // Remove the removed apps
        foreach ((string, string) app in removedApps) {
            Program.StorageService.DeleteAuthorizedApp(Id, app.Item1);
        }
    }
    
}