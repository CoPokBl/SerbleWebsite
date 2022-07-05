namespace SerbleWebsite.Data; 

public class User {
    
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    // 0=Disabled Account 1=Normal, 2=Admin
    public int PermLevel { get; set; }
    public string PermString { get; set; }
    
}