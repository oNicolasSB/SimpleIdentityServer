using IdentityServer4.Models;

namespace IdentityServer.API;

public class Config
{

    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource
            {
                Name = "Role",
                UserClaims = new List<string> { "role" }
            }
        };
    }
    // public static IEnumerable<IdentityResource> IdentityResources =>
    // [
    //     new IdentityResources.OpenId(),
    //     new IdentityResources.Profile(),
    //     new IdentityResource
    //     {
    //         Name = "Role",
    //         UserClaims = new List<string> { "role" }
    //     }
    // ];

    public static IEnumerable<ApiScope> GetApis()
    {
        return new List<ApiScope>
        {
            new ApiScope("CoffeeeApi.read"),
            new ApiScope("CoffeeeApi.write")
        };
    }

    // public static IEnumerable<ApiScope> ApiScopes =>
    //     [new ApiScope("CoffeeeApi.read"), new ApiScope("CoffeeeApi.write")];

    public static IEnumerable<ApiResource> GetApiResources()
    {
        return new List<ApiResource>
        {
            new ApiResource("CoffeeeApi")
            {
                Scopes = { "CoffeeeApi.read", "CoffeeeApi.write" },
                ApiSecrets = { new Secret("ScopeSecret".Sha256()) },
                UserClaims = { "role" }
            }
        };
    }
    // public static IEnumerable<ApiResource> ApiResources =>
    // [
    //     new ApiResource("CoffeeeApi")
    //     {
    //         Scopes = { "CoffeeeApi.read", "CoffeeeApi.write" },
    //         ApiSecrets = { new Secret("ScopeSecret".Sha256()) },
    //         UserClaims = { "role" }
    //     }
    // ];
    public static IEnumerable<Client> GetClients(IConfiguration configuration)
    {
        return new List<Client>
        {
            new Client
        {
            ClientId = "m2m.client",
            ClientName = "Client credentials Client",
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets = { new Secret("ClientSecret1".Sha256())},
            AllowedScopes = { "CoffeeeApi.read" , "CoffeeeApi.write" }
        },
        new Client
        {
            ClientId = "interactive",
            ClientSecrets = { new Secret("ClientSecret1".Sha256()) },
            AllowedGrantTypes = GrantTypes.Code,
            RedirectUris = { "https://localhost:5444/signin-oidc" },
            FrontChannelLogoutUri = "https://localhost:5444/signout-oidc",
            PostLogoutRedirectUris = { "https://localhost:5444/signout-callback-oidc" },
            AllowOfflineAccess = true,
            AllowedScopes = new List<string> { "openid", "profile", "CoffeeeApi.read" },
            RequirePkce = true,
            RequireConsent = true,
            AllowPlainTextPkce = false
        },
        };
    }

    // public static IEnumerable<Client> Clients =>
    // [
    //     new Client
    //     {
    //         ClientId = "m2m.client",
    //         ClientName = "Client credentials Client",
    //         AllowedGrantTypes = GrantTypes.ClientCredentials,
    //         ClientSecrets = { new Secret("ClientSecret1".Sha256())},
    //         AllowedScopes = { "CoffeeeApi.read" , "CoffeeeApi.write" }
    //     },
    //     new Client
    //     {
    //         ClientId = "interactive",
    //         ClientSecrets = { new Secret("ClientSecret1".Sha256()) },
    //         AllowedGrantTypes = GrantTypes.Code,
    //         RedirectUris = { "https://localhost:5444/signin-oidc" },
    //         FrontChannelLogoutUri = "https://localhost:5444/signout-oidc",
    //         PostLogoutRedirectUris = { "https://localhost:5444/signout-callback-oidc" },
    //         AllowOfflineAccess = true,
    //         AllowedScopes = new List<string> { "openid", "profile", "CoffeeeApi.read" },
    //         RequirePkce = true,
    //         RequireConsent = true,
    //         AllowPlainTextPkce = false
    //     },
    // ];



}