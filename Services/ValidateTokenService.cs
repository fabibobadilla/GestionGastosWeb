using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;

public class TokenValidationService : IAsyncDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _navigation;
    private PeriodicTimer? _timer;
    private CancellationTokenSource? _cts;

    public TokenValidationService(HttpClient httpClient, ILocalStorageService localStorage, NavigationManager navigation)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _navigation = navigation;
    }

    public void StartValidation()
    {
        _cts = new CancellationTokenSource();
        _timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

        _ = Task.Run(async () =>
        {
            while (await _timer.WaitForNextTickAsync(_cts.Token))
            {
                await ValidateTokenAsync();
            }
        });
    }

    private async Task ValidateTokenAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<bool>("api/auth/validate");

            if (!response)
            {
                await _localStorage.RemoveItemAsync("jwt-token");
                _navigation.NavigateTo("/login", forceLoad: true);
            }
        }
        catch
        {
            _navigation.NavigateTo("/login", forceLoad: true);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}
