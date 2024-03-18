using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.API.Data;

public class IdentityDbContext : IdentityDbContext<IdentityUser>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }
}