using SaphyreProject.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SaphyreProject.Shared.Interfaces
{
  public interface IProfileHub
  {
    Task FetchProfile();
    Task DispatchProfile(ProfileVM profile, string message);
  }
}
