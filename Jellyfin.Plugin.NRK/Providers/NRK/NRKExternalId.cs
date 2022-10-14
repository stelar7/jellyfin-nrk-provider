namespace Jellyfin.Plugin.NRK.Providers.NRK
{
    public class NRKExternalId : IExternalId
    {
        public bool Supports(IHasProviderIds item) => item is Series || item is Movie;

        public string ProviderName => "NRK";

        public string Key => ProviderNames.NRK;

        public ExternalIdMediaType? Type => ExternalIdMediaType.Series;

        public string UrlFormatString => "https://tv.nrk.no/serie/{0}";
    }
}
