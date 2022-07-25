namespace SerbleWebsite.Data.Schemas; 

public class Lockdown {
    
    public bool AllowAnyAccess { get; set; }
    public AccountAccessLevel[] AllowedPermLevels { get; set; }
    public bool AccessToAdminTools { get; set; }

}