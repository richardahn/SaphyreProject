using Microsoft.AspNetCore.SignalR.Client;
using SaphyreProject.Shared.Interfaces;
using SaphyreProject.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SaphyreProject.Shared.Services
{
  /// <summary>
  /// This is a SignalR client that acts as a wrapper to ProfileHub,
  /// they share the same interface.
  /// </summary>
  public class ProfileHubProxy : IProfileHub
  {
    private readonly IHubClientFactory _hubClientFactory;
    private HubConnection _hubConnection;
    public ProfileHubProxy(IHubClientFactory hubClientFactory)
    {
      _hubClientFactory = hubClientFactory;
    }

    public void Connect(string url)
    {
      _hubConnection = _hubClientFactory.Create(url);
    }

    public async Task FetchProfile()
    {
      await _hubConnection.StartAsync();
      await _hubConnection.SendAsync("FetchProfile");
      await _hubConnection.StopAsync();
    }

    public async Task DispatchProfile(ProfileVM profile, string message)
    {
      await _hubConnection.StartAsync();
      await _hubConnection.SendAsync("DispatchProfile", profile, message);
      await _hubConnection.StopAsync();
    }
  }
}
