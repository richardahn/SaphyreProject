using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SaphyreProject.Main.Models;

namespace SaphyreProject.Main.Data
{
  public class ProfileContext : DbContext
  {
    public ProfileContext(DbContextOptions<ProfileContext> options) : base(options) { }
    public DbSet<Profile> Profile { get; set; }
  }
}
