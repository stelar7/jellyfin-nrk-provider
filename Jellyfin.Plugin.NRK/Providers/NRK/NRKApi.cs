
namespace Jellyfin.Plugin.NRK.Providers.NRK
{



    public async Task<RootObject> WebRequestAPI(string link)
    {
        var httpClient = Plugin.Instance.GetHttpClient();
        using (HttpContent content = new FormUrlEncodedContent(Enumerable.Empty<KeyValuePair<string, string>>()))
        using (var response = await httpClient.PostAsync(link, content).ConfigureAwait(false))
        using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
        {
            return await JsonSerializer.DeserializeAsync<RootObject>(responseStream).ConfigureAwait(false);
        }
    }
}
