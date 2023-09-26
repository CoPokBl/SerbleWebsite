namespace SerbleWebsite.Data; 

public class ImageService {
    private readonly HttpClient _client;
    public ImageService(HttpClient client) {
        _client = client;
    }

    public async Task<Stream> GetImageStreamAsync(string url, string token) {
        _client.DefaultRequestHeaders.Add("SerbleAuth", "User " + token);
        HttpResponseMessage response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStreamAsync();
    }
}