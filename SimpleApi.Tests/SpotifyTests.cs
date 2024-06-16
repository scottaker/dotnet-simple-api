using NUnit.Framework;
using SimpleApi.API.Services;
using SimpleApi.Domain.Services;
using SimpleApi.Tests.util;

namespace SimpleApi.Tests;

[TestFixture]
public class SpotifyTests
{

    [Test]
    public async Task Get_User_Profile()
    {
        var locator = GetLocator();
        var spotify = locator.Get<ISpotifyService>();

        var profile = await spotify.GetUserProfile("dakota148");

        Console.WriteLine($"{profile.Id,-20} {profile.DisplayName}");
    }



    [Test]
    public async Task Get_User_Playlists()
    {
        var locator = GetLocator();
        var spotify = locator.Get<ISpotifyService>();

        var playlists = await spotify.GetPlaylists("dakota148");

        foreach (var item in playlists)
        {
            Console.WriteLine($"{item.Id,-20} {item.Name}");
        }

    }

    private ServiceLocator GetLocator()
    {
        return new ServiceLocator(null);
    }
}