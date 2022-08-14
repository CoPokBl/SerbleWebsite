using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GeneralPurposeLib;
using SerbleWebsite.Data.Schemas;

namespace SerbleWebsite.Data;
public static class SerbleApiHandler {

    public static async Task<SerbleApiResponse<User>> GetUser(string token) {
        // Send HTTP request to API
        HttpClient client = new();
        client.DefaultRequestHeaders.Add("SerbleAuth", "User " + token);
        HttpResponseMessage response;
        try {
            response = await client.GetAsync(Constants.SerbleApiUrl + "account");
        }
        catch (Exception e) {
            return new SerbleApiResponse<User>(false, "Failed: " + e);
        }
        if (!response.IsSuccessStatusCode) {
            Console.WriteLine("Response: " + await response.Content.ReadAsStringAsync());
            return new SerbleApiResponse<User>(false, $"Failed: {response.StatusCode}");
        }
        // Parse response
        string json = await response.Content.ReadAsStringAsync();
        Console.WriteLine(json);
        User user;
        try {
            user = JsonSerializer.Deserialize<User>(json).ThrowIfNull();
            Console.WriteLine("Username: " + user.Username);
            Console.WriteLine("PermLvl: " + user.PermLevel);
        }
        catch (Exception e) {
            return new SerbleApiResponse<User>(false, $"Failed to parse response: {e.Message}");
        }
        return new SerbleApiResponse<User>(user);
    }
    
    public static async Task<SerbleApiResponse<string>> LoginUser(string username, string password) {
        // Send HTTP request to API
        HttpClient client = new();
        client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}")));
        HttpResponseMessage response;
        try {
            response = await client.GetAsync(Constants.SerbleApiUrl + "auth");
        }
        catch (Exception e) {
            return new SerbleApiResponse<string>("Error: " + e);
        }
        if (!response.IsSuccessStatusCode) {
            Console.WriteLine("Response: " + await response.Content.ReadAsStringAsync());
            return new SerbleApiResponse<string>($"Non Success Code: {response.StatusCode}");
        }
        // Parse response
        string token;
        try {
            token = await response.Content.ReadAsStringAsync();
        }
        catch (Exception e) {
            Console.WriteLine("Response: " + await response.Content.ReadAsStringAsync());
            return new SerbleApiResponse<string>($"Failed to parse response: {e.Message}");
        }
        return new SerbleApiResponse<string>(token);
    }
    
    public static async Task<SerbleApiResponse<User>> RegisterUser(string username, string password) {
        // Send HTTP request to API
        HttpClient client = new();
        HttpResponseMessage response;
        try {
            response = await client.PostAsync(Constants.SerbleApiUrl + "account", new StringContent(new {
                username,
                password
            }.ToJson(), Encoding.UTF8, "application/json"));
        }
        catch (Exception e) {
            return new SerbleApiResponse<User>(false, "Failed: " + e);
        }
        if (!response.IsSuccessStatusCode) {
            return new SerbleApiResponse<User>(false, $"Failed: {response.StatusCode} ({await response.Content.ReadAsStringAsync()})");
        }
        // Parse response
        string json = await response.Content.ReadAsStringAsync();
        User user;
        try {
            user = JsonSerializer.Deserialize<User>(json).ThrowIfNull();
        }
        catch (Exception e) {
            return new SerbleApiResponse<User>(false, $"Failed to parse response: {e.Message}");
        }
        return new SerbleApiResponse<User>(user);
    }
    
    public static async Task<SerbleApiResponse<User>> EditUser(string token, AccountEditRequest[] edits) {
        // Send HTTP request to API
        HttpClient client = new();
        client.DefaultRequestHeaders.Add("SerbleAuth", "User " + token);
        HttpResponseMessage response;
        string jsonInp = edits.ToJson();
        Console.WriteLine(jsonInp);
        try {
            response = await client.PatchAsync(
                Constants.SerbleApiUrl + "account", 
                new StringContent(jsonInp, Encoding.UTF8, 
                    "application/json"));
        }
        catch (Exception e) {
            return new SerbleApiResponse<User>(false, "Failed: " + e);
        }
        if (!response.IsSuccessStatusCode) {
            string flag = "unknown";
            string responseContent = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.BadRequest) {
                flag = responseContent switch {
                    "Username is already taken" => "name-taken",
                    "Invalid email" => "email-invalid",
                    "Field doesn't exist" => "bad-field",
                    _ => flag
                };
            }
            Console.WriteLine(responseContent);
            return new SerbleApiResponse<User>(false, $"Failed: {response.StatusCode} ({await response.Content.ReadAsStringAsync()})", flag);
        }
        // Parse response
        string json = await response.Content.ReadAsStringAsync();
        User user;
        try {
            user = JsonSerializer.Deserialize<User>(json).ThrowIfNull();
        }
        catch (Exception e) {
            return new SerbleApiResponse<User>(false, $"Failed to parse response: {e.Message}");
        }
        return new SerbleApiResponse<User>(user);
    }
    
}

public class SerbleApiResponse<T> {
    
    public bool Success { get; set; }
    public T? ResponseObject { get; }
    public string ErrorMessage { get; }
    public string ErrorFlag { get; }
    
    public SerbleApiResponse(T responseObject) {
        ResponseObject = responseObject;
        Success = true;
        ErrorFlag = "";
        ErrorMessage = "";
    }
    
    public SerbleApiResponse(bool success, string errorMessage, string errorFlag = "") {
        Success = false;
        ResponseObject = default;
        ErrorMessage = errorMessage;
        ErrorFlag = errorFlag;
    }

}