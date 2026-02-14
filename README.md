# ItemDash Server

## Setup

1. Clone the repository
2. Set up your configuration:
   - Copy `appsettings.json` to `appsettings.Development.json`
   - Update the connection string and JWT secret in `appsettings.Development.json`
   - Generate a new JWT secret (minimum 256 characters)

## Configuration

The project uses environment-specific configuration files:

- `appsettings.json` - Template with placeholder values (tracked in git)
- `appsettings.Development.json` - Local development settings (excluded from git)

### Required Settings

In `appsettings.Development.json`, configure:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=itemdash;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "Secret": "YOUR_256_CHARACTER_JWT_SECRET_HERE",
    "Issuer": "ItemDashServer",
    "Audience": "ItemDashServerUsers",
    "ExpiryInMinutes": 60
  }
}
```

## Running the Application

```bash
dotnet run --project ItemDashServer.Api
```

## Security Notes

- Never commit secrets to version control
- `appsettings.Development.json` contains sensitive data and is excluded from git
- Generate unique JWT secrets for each environment