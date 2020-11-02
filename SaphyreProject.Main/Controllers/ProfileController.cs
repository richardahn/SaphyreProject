using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SaphyreProject.Main.Data;
using SaphyreProject.Main.Settings;
using SaphyreProject.Shared.Services;
using SaphyreProject.Shared.ViewModels;

namespace SaphyreProject.Main.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ProfileController : ControllerBase
  {
    private readonly ProfileContext _profileContext;
    private readonly SignalRSettings _signalRSettings;
    private readonly ProfileHubProxy _profileHubProxy;

    public ProfileController(ProfileContext profileContext, ProfileHubProxy profileHubClient, IOptions<SignalRSettings> signalRSettings)
    {
      _profileContext = profileContext;
      _profileHubProxy = profileHubClient;
      _signalRSettings = signalRSettings.Value;

      _profileHubProxy.Connect(_signalRSettings.HubUrl);
    }

    [HttpGet]
    public ProfileVM Get()
    {
      var model = _profileContext.Profile.First();
      return new ProfileVM
      {
        Id = model.Id,
        FirstName = model.FirstName,
        LastName = model.LastName,
        PhoneNumber = model.PhoneNumber
      };
    }

    [HttpPost]
    public async Task Set(ProfileVM vm)
    {
      var modelToUpdate = _profileContext.Profile.First();
      modelToUpdate.FirstName = vm.FirstName;
      modelToUpdate.LastName = vm.LastName;
      modelToUpdate.PhoneNumber = vm.PhoneNumber;
      _profileContext.SaveChanges();
      await _profileHubProxy.DispatchProfile(new ProfileVM
      {
        Id = modelToUpdate.Id,
        FirstName = modelToUpdate.FirstName,
        LastName = modelToUpdate.LastName,
        PhoneNumber = modelToUpdate.PhoneNumber
      }, "Success!");
    }
  }
}
