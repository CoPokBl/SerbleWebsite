namespace SerbleWebsite.Data; 

public interface IStorageService {
    public void Init();
    public void Deinit();
    
    public void AddUser(User userDetails, out User newUser);
    public void GetUser(string userId, out User? user);
    public void UpdateUser(User userDetails);
    public void DeleteUser(string userId);
}