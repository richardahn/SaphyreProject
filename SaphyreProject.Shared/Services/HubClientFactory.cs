using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaphyreProject.Shared.Services
{
  public class HubClientFactory : IHubClientFactory
  {
    public HubConnection Create(string hubUrl)
    {
      return new HubConnectionBuilder()
        .WithUrl(hubUrl)
        .Build();
    }
  }
}
