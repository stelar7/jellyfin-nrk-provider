namespace Jellyfin.Plugin.NRK.Providers.NRK
{
    private readonly NRKApi _nrkApi;

    public class NRKProvider : IRemoteMetadataProvider<Series, SeriesInfo>, IHasOrder
    {
        public NRKProvider()
        {
            _nrkApi = new NRKApi();
        }

        public async Task<MetadataResult<Series>> GetMetadata(SeriesInfo info, CancellationToken token)
        {
            var result = new MetadataResult<Series>();
            Media media = null;

            var key = info.ProviderIds.GetOrDefault(ProviderNames.NRK);
            if (!string.IsNullOrEmpty(key))
            {
                media = await _nrkApi.getSeries(key);
            }
            else
            {
                MediaSearchResult msr = await _nrkApi.Search(info.Name, cancellationToken);
                if (msr != null)
                {
                    media = await _nrkApi.GetSeries(msr.id.ToString());
                }
            }

            if (media != null)
            {
                result.HasMetadata = true;
                result.Item = media.ToSeries();
                result.People = media.GetPeopleInfo();
                result.Provider = ProviderNames.NRK;
                StoreImageUrl(media.id.ToString(), media.GetImageUrl(), "image");
            }

            return result;
        }

        public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(SeriesInfo searchInfo, CancellationToken cancellationToken)
        {
            var results = new List<RemoteSearchResult>();

            var key = searchInfo.ProviderIds.GetOrDefault(ProviderNames.NRK);
            if (!string.IsNullOrEmpty(key))
            {
                Media result = await _nrkApi.GetSeries(key).ConfigureAwait(false);
                if (result != null)
                {
                    results.Add(result.ToSearchResult());
                }
            }

            if (!string.IsNullOrEmpty(searchInfo.Name))
            {
                List<MediaSearchResult> name_results = await _nrkApi.Search(searchInfo.Name, cancellationToken).ConfigureAwait(false);
                foreach (var media in name_results)
                {
                    results.Add(media.ToSearchResult());
                }
            }

            return results;
        }
    }
}
