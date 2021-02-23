# laget.PskAuthentication
Default implementation of Secure Pre-Shared Key (PSK) Authentication for laget.se using Rijndael.

![Nuget](https://img.shields.io/nuget/v/laget.PskAuthentication.Client?label=laget.PskAuthentication.Client)
![Nuget](https://img.shields.io/nuget/dt/laget.PskAuthentication.Client?label=laget.PskAuthentication.Client)

![Nuget](https://img.shields.io/nuget/v/laget.PskAuthentication.Core?label=laget.PskAuthentication.Core)
![Nuget](https://img.shields.io/nuget/dt/laget.PskAuthentication.Core?label=laget.PskAuthentication.Core)

![Nuget](https://img.shields.io/nuget/v/laget.PskAuthentication.Mvc?label=laget.PskAuthentication.Mvc)
![Nuget](https://img.shields.io/nuget/dt/laget.PskAuthentication.Mvc?label=laget.PskAuthentication.Mvc)

## Usage
### appsettings.json
```c#
"Security": {
  "RijndaelIV": "...",
  "RijndaelKey": "...",
  "Salt": "...",
  "Secret": "..."
}
```

### Autofac
```c#
public class OptionModule : Module
{
    readonly IConfiguration _configuration;

    public OptionModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(c => new PskAuthenticationAttribute(new PskAuthenticationOptions
        {
            RijndaelKey = _configuration.GetValue<string>("Security:RijndaelKey"),
            RijndaelIV = _configuration.GetValue<string>("Security:RijndaelIV"),
            Salt = _configuration.GetValue<string>("Security:Salt"),
            Secret = _configuration.GetValue<string>("Security:Secret")
        })).AsSelf();
    }
}
```

### Controller
```c#
[PskAuthentication]
public class SomeController : ControllerBase
{
}
