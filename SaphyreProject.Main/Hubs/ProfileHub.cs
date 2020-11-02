using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SaphyreProject.Main.Models;
using SaphyreProject.Shared.ViewModels;
using SaphyreProject.Shared;
using SaphyreProject.Main.Data;
using SaphyreProject.Shared.Interfaces;

namespace SaphyreProject.Main.Hubs
{
  /// <summary>
  /// SignalR hub for clients to send requests to.
  /// This server in particular is used by the SignalR client on the server to be able to dispatch the updated profile
  /// </summary>
  public class ProfileHub : Hub, IProfileHub
  {
    private readonly ProfileContext _profileContext;
    public ProfileHub(ProfileContext profileContext)
    {
      _profileContext = profileContext;
    }

    // Fetch profile(send to caller)
    public async Task FetchProfile()
    {
      var profile = _profileContext.Profile.First();
      await Clients.Caller.SendAsync("FetchedProfile", new ProfileVM
      { 
        Id = profile.Id,
        FirstName = profile.FirstName,
        LastName = profile.LastName,
        PhoneNumber = profile.PhoneNumber
      });
    }

    // Dispatch profile to all clients
    public async Task DispatchProfile(ProfileVM profile, string message)
    {
      await Clients.All.SendAsync("DispatchedProfile", profile, new DispatchInfo(message));
    }
  }
}
