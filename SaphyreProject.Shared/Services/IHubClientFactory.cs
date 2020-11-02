using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaphyreProject.Shared.Services
{
  public interface IHubClientFactory
  {
    HubConnection Create(string hubUrl);
  }
}
