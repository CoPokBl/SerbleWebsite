using GeneralPurposeLib;
using Microsoft.JSInterop;

namespace SerbleWebsite.Data; 

public class GoogleReCaptchaService {
    private IJSRuntime _jsRuntime;
    private IHttpContextAccessor _httpContextAccessor;
    
    public GoogleReCaptchaService(IJSRuntime jsRuntime, IHttpContextAccessor httpContextAccessor) {
        _jsRuntime = jsRuntime;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Checks user using Google ReCaptcha
    /// </summary>
    /// <returns>True if user is good, false if user if bad</returns>
    public async Task<bool> CheckUser() {
        string captcha = await _jsRuntime.InvokeAsync<string>("getCaptcha");
        string ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()!;
        
        // Send http request
        HttpClient client = new();
        HttpContent content = new StringContent($"");
        HttpResponseMessage response = await client.PostAsync(
            $"https://www.google.com/recaptcha/api/siteverify?" +
            $"secret={Program.Config!["google_recaptcha_secret_key"]}&" +
            $"response={captcha}&remoteip={ip}", 
            content);
        if (!response.IsSuccessStatusCode) {
            Logger.Error("Google reCaptcha service failed");
            Logger.Error(response.Content.ReadAsStringAsync());
            return false;
        }
        string responseString = await response.Content.ReadAsStringAsync();
        return true;
        // TODO: Finish this
    }
    
}