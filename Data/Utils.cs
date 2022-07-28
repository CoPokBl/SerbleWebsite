using System.Security.Cryptography;
using System.Text;
using Microsoft.JSInterop;

namespace SerbleWebsite.Data; 

public static class Utils {
    
    public static string Hash(string str) {
        StringBuilder builder = new StringBuilder();
        foreach (byte t in SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(str))) {
            builder.Append(t.ToString("x2"));
        }

        return builder.ToString();
    }

}

public class Cookie {
    readonly IJSRuntime _jsRuntime;
    string _expires = "";

    public Cookie(IJSRuntime jsRuntime) {
        _jsRuntime = jsRuntime;
        ExpireDays = 300;
    }

    public async Task SetValue(string key, string value, int? days = null) {
        string curExp = days != null ? days > 0 ? DateToUTC(days.Value) : "" : _expires;
        await SetCookie($"{key}={value}; expires={curExp}; path=/");
    }

    public async Task<string> GetValue(string key, string def = "") {
        string cValue = await GetCookie();
        if (string.IsNullOrEmpty(cValue)) return def;                

        string[] cookies = cValue.Split(';');
        foreach (string cookie in cookies) {
            string[] c = cookie.Split('=');
            if (c[0].Trim() == key) return c[1];
        }
        return def;
    }

    private async Task SetCookie(string value) {
        await _jsRuntime.InvokeVoidAsync("eval", $"document.cookie = \"{value}\"");
    }

    private async Task<string> GetCookie() {
        if (_jsRuntime == null) {
            return "";
        }
        return await _jsRuntime.InvokeAsync<string>("eval", $"document.cookie");
    }

    public int ExpireDays {
        set => _expires = DateToUTC(value);
    }

    private static string DateToUTC(int days) => DateTime.Now.AddDays(days).ToUniversalTime().ToString("R");
}