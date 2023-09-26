namespace SerbleWebsite.Data; 

public class ImageService {
    private readonly HttpClient _client;
    private bool _headerAdded;
    public ImageService(HttpClient client) {
        _client = client;
    }

    public async Task<Stream> GetImageStreamAsync(string url, string token) {
        if (!_headerAdded) {
            _client.DefaultRequestHeaders.Add("SerbleAuth", "User " + token);
            _headerAdded = true;
        }
        HttpResponseMessage response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStreamAsync();
    }
}