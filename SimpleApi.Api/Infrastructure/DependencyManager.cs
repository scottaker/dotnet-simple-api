﻿using SimpleApi.API.Services;
using SimpleApi.Domain.Services;
using SimpleApi.Services.Clients;
using SimpleApi.Services.Services;
using ILogger = SimpleApi.Domain.Services.ILogger;

namespace SimpleApi.API.Infrastructure;


public class DependencyManager
{
    public void Services(IServiceCollection services, IConfiguration configuration)
    {
        IServiceCollection Add<TInterface, TClass>()
            where TInterface : class
            where TClass : class, TInterface
        {
            return services.AddScoped<TInterface, TClass>();
        }

        // application infrastructure
        services.AddHttpClient();
        Add<ILogger, DefaultLogger>();
        services.AddSingleton<SimpleApiSettings>(provider => GetConfig<SimpleApiSettings>(configuration, "SimpleApiSettings"));
        services.AddSingleton<ApiClientFactory>();

        // API CLIENTS
        services.AddScoped<ILocationClient>((p) =>
        {
            var clientFactory = p.GetService<ApiClientFactory>();
            var httpFactory = p.GetService<IHttpClientFactory>();
            var settings = p.GetService<SimpleApiSettings>();
            var logger = p.GetService<ILogger>();

            var client = clientFactory.Create(httpFactory, logger, settings.LocationApiSettings);
            return new LocationClient(client);
        });
        services.AddScoped<IWeatherClient>((p) =>
        {
            var clientFactory = p.GetService<ApiClientFactory>();
            var httpFactory = p.GetService<IHttpClientFactory>();
            var settings = p.GetService<SimpleApiSettings>();
            var logger = p.GetService<ILogger>();

            var client = clientFactory.Create(httpFactory, logger, settings.WeatherApiSettings);
            return new WeatherClient(client);
        });

        services.AddScoped<ISpotifyLocalClient, SpotifyLocalClient>();
        services.AddScoped<ISpotifyService, SpotifyService>();
        // SERVICES
        Add<ILocationService, LocationService>();
        Add<IWeatherService, WeatherService>();

        services.AddScoped<CityService>();

        //Add<IGPTCLient, GptClient>();
        //Add<IGptService, GptService>();
    }

    private static T GetConfig<T>(IConfiguration configuration, string name)
    {
        var config = configuration.GetSection(name).Get<T>();
        return config;
    }

    public class ApiClientFactory
    {
        public RapidApiClient Create(IHttpClientFactory factory, ILogger logger, ApiSettings settings)
        {
            return new RapidApiClient(factory, logger, settings);
        }
    }
}

