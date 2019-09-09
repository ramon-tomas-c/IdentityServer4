# Introduction 
Template of a dockerized IdentityServer4

# Getting Started

## Lauch IS4 with Docker Compose

Add/Modify/Override IS4 settings in docker-compose.override file

```yaml
identity-server:
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ASPNETCORE_URLS=http://0.0.0.0:80
      - ConnectionString=Server=tcp:sql-data,1433;Database=IdentityDb;User Id=sa;Password=Pass@word;
      - IdentityUrl=http://localhost:5000
      - Clients__0__ClientId=webappclientid
      - Clients__0__ClientName=webappname
      - Clients__0__WebUrls__0=http://localhost:3000
      - Clients__0__RedirectUris__0=/
      - Clients__0__RedirectUris__1=/#/signin-callback
      - Clients__0__RedirectUris__2=/#/silent-renew
      - Clients__0__PostLogoutRedirectUris__0=/
      - Clients__0__AllowedScopes__0=openid
```

Execute docker compose to launch IS4 & SQL DB

```bash
docker-compose up
```

# Build

## Build IS4 image with Docker Compose
Execute docker compose to build an IS4 image

```bash
docker-compose build identity-server
```

# Settings

## IS4 client settings and default values

Default settings that can be overriden through AppSettings.json or Environment variables.

```csharp
 public string ClientId { get; set; }
 public string ClientName { get; set; }
 public ICollection<string> AllowedGrantTypes { get; set; } = GrantTypes.Code;
 public bool RequirePkce { get; set; } = true;
 public bool RequireClientSecret { get; set; } = false;
 public bool AllowAccessTokensViaBrowser { get; set; } = true;
 public bool AllowOfflineAccess { get; set; } = false;
 public ICollection<string> AllowedCorsOrigins { get; set; }
 public ICollection<string> WebUrls { get; set; }
 public ICollection<string> RedirectUris { get; set; }
 public ICollection<string> PostLogoutRedirectUris { get; set; }
 public ICollection<string> AllowedScopes { get; set; }
```
