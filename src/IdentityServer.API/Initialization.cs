using System.Security.Claims;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.API;

internal static class Initialization
{
    public static IApplicationBuilder InitializeData(this IApplicationBuilder applicationBuilder, IConfiguration configuration)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            context.Database.Migrate();
            var userMgr = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var admin = userMgr.FindByNameAsync("admin").Result;
            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin.user@email.com",
                    EmailConfirmed = true
                };
                var result = userMgr.CreateAsync(admin, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result =
                    userMgr.AddClaimsAsync(
                        admin,
                        [
                            new Claim(JwtClaimTypes.Name, "Admin User"),
                            new Claim(JwtClaimTypes.GivenName, "Admin"),
                            new Claim(JwtClaimTypes.FamilyName, "Freeman"),
                            new Claim(JwtClaimTypes.WebSite, "http://adminuser.com"),
                            new Claim("location", "somewhere")
                        ]
                    ).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
        }

        using (var serviceScope = applicationBuilder.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        {
            serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            context.Database.Migrate();

            foreach (var client in Config.GetClients(configuration))
            {
                var cliente = context.Clients.FirstOrDefault(x => x.ClientId.Equals(client.ClientId));
                if (cliente == null)
                {
                    context.Clients.Add(client.ToEntity());
                    context.SaveChanges();
                }
                else if (!cliente.Equals(client))
                {
                    cliente.BackChannelLogoutUri = client.BackChannelLogoutUri;
                    context.Clients.Update(cliente);
                    context.SaveChanges();
                }
            }

            foreach (var resource in Config.GetIdentityResources())
            {
                var identityResource = context.IdentityResources.FirstOrDefault(x => x.Name.Equals(resource.Name));
                if (identityResource == null)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                    context.SaveChanges();
                }
            }

            foreach (var api in Config.GetApis())
            {
                var apiResource = context.ApiScopes.FirstOrDefault(x => x.Name.Equals(api.Name));
                if (apiResource == null)
                {
                    context.ApiScopes.Add(api.ToEntity());
                    context.SaveChanges();
                }
            }
            foreach (var api in Config.GetApiResources())
            {
                var apiResource = context.ApiResources.FirstOrDefault(x => x.Name.Equals(api.Name));
                if (apiResource == null)
                {
                    context.ApiResources.Add(api.ToEntity());
                    context.SaveChanges();
                }
            }
        }
        return applicationBuilder;
    }
}