using SimpleApi.Domain.Services;
using SpotifyAPI.Web;

namespace SimpleApi.Services.Clients;

public class SpotifyLocalClient : ISpotifyLocalClient
{
    private const int PageSize = 20;
    public SpotifyApiSettings _settings;

    public SpotifyLocalClient(SimpleApiSettings settings)
    {
        _settings = settings.SpotifyApiSettings;
    }

    public async Task<PublicUser> GetCurrentProfile(string userId)
    {
        //var config = SpotifyClientConfig.CreateDefault();

        //var request = new ClientCredentialsRequest(_settings.ClientId, _settings.Secret);
        //var response = await new OAuthClient(config).RequestToken(request);

        //var client = new SpotifyClient(config.WithToken(response.AccessToken));
        ////return client;

        var client = await GetClient();
        var profile = await client.UserProfile.Get(userId);
        return profile;
    }

    public async Task<IEnumerable<FullPlaylist>> GetUserPlaylists(string userId, int offset)
    {
        var client = await GetClient();
        var request = new PlaylistGetUsersRequest
        {
            Limit = PageSize,
            Offset = offset
        };
        var playlists = await client.Playlists.GetUsers(userId, request);
        return playlists.Items;
    }

    private async Task<SpotifyClient> GetClient()
    {
        var config = SpotifyClientConfig.CreateDefault();

        var request = new ClientCredentialsRequest(_settings.ClientId, _settings.Secret);
        var response = await new OAuthClient(config).RequestToken(request);

        var client = new SpotifyClient(config.WithToken(response.AccessToken));
        return client;


    }
    
}
