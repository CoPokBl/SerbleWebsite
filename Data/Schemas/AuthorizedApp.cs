namespace SerbleWebsite.Data.Schemas; 

public class AuthorizedApp {
    public string AppId { get; set; }
    public string Scopes { get; set; }
    
    public AuthorizedApp(string appId, string scopes) {
        AppId = appId;
        Scopes = scopes;
    }

    public AuthorizedApp() {
        AppId = "";
        Scopes = "";
    }
}