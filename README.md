# laget.PskAuthentication
Default implementation of Secure Pre-Shared Key (PSK) Authentication for laget.se using Rijndael.

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

### Autofac (DI)
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
        builder.Register(p => _configuration.GetSection("Newbody").Get<NewbodyOptions>()).As<INewbodyOptions>().SingleInstance();

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
public class Receipt : ControllerBase
{
}
