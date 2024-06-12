using Microsoft.JSInterop;

namespace SerbleWebsite.Data;

/// <summary>
/// Manages storage of token in cookie vs local storage.
/// </summary>
public class TokenManager {
    private IJSRuntime _js;

    public TokenManager(IJSRuntime js) {
        _js = js;
    }

    public async Task<string> GetToken() {
        Cookie cookies = new(_js);
        HtmlInteractor js = new(_js);

        string cToken = await cookies.GetValue("token");
        string lToken = await js.GetLocalStorage("token");

        return string.IsNullOrWhiteSpace(cToken) ? lToken : cToken;
    }

    public async Task SetToken(string token, bool rememberMe) {
        Cookie cookies = new(_js);
        HtmlInteractor js = new(_js);

        if (rememberMe) {
            await js.SetLocalStorage("token", token);
            return;
        }

        await cookies.SetValue("token", token, 24);  // Save for 1 day
    }

    public async Task ClearToken() {
        Cookie cookies = new(_js);
        HtmlInteractor js = new(_js);

        await cookies.SetValue("token", "");
        await js.SetLocalStorage("token", "");
    }
}