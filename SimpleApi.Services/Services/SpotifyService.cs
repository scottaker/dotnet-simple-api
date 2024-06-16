using SimpleApi.Domain.Services;
using SimpleApi.Services.Clients;
using SpotifyAPI.Web;

namespace SimpleApi.Services.Services;

public class SpotifyService : ISpotifyService
{
    private readonly ISpotifyLocalClient _client;

    public SpotifyService(ISpotifyLocalClient client)
    {
        _client = client;
    }

    public async Task<IEnumerable<FullPlaylist>> GetPlaylists(string userId)
    {
        var profile = await _client.GetCurrentProfile(userId);

        var offset = 0;
        var playlists = await _client.GetUserPlaylists(profile.Id, offset);
        return playlists;
    }

    public async Task<PublicUser> GetUserProfile(string userId)
    {
        var profile = await _client.GetCurrentProfile(userId);
        return profile;
    }
}


