using SpotifyAPI.Web;

namespace SimpleApi.Domain.Services;

public interface ISpotifyService
{
    Task<IEnumerable<FullPlaylist>> GetPlaylists(string userId);
    Task<PublicUser> GetUserProfile(string userId);
}

public interface ISpotifyLocalClient
{
    Task<PublicUser> GetCurrentProfile(string userId);
    Task<IEnumerable<FullPlaylist>> GetUserPlaylists(string userId, int offset);
}