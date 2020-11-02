using System;
using System.Collections.Generic;
using System.Text;

namespace SaphyreProject.Shared
{
  public class DispatchInfo
  {
    public string Message { get; set; }
    public DispatchInfo() { }
    public DispatchInfo(string msg) { Message = msg; }
  }
}
