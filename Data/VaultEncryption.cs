using Microsoft.JSInterop;

namespace SerbleWebsite.Data; 

public static class VaultEncryption {
    
    public static async Task<string> Encrypt(IJSRuntime js, string original, string password) {
        string encrypted = await js.InvokeAsync<string>("window.cryptoApi.encrypt", original, password);
        return encrypted;
    }

    public static async Task<string> Decrypt(IJSRuntime js, string encrypted, string password) {
        string decrypted = await js.InvokeAsync<string>("window.cryptoApi.decrypt", encrypted, password);
        return decrypted;
    }
    
}