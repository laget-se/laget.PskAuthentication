# laget.PskAuthentication
Default implementation of Secure Pre-Shared Key (PSK) Authentication for laget.se using AES.

![Nuget](https://img.shields.io/nuget/v/laget.PskAuthentication.Client?label=laget.PskAuthentication.Client)
![Nuget](https://img.shields.io/nuget/dt/laget.PskAuthentication.Client?label=laget.PskAuthentication.Client)

![Nuget](https://img.shields.io/nuget/v/laget.PskAuthentication.Core?label=laget.PskAuthentication.Core)
![Nuget](https://img.shields.io/nuget/dt/laget.PskAuthentication.Core?label=laget.PskAuthentication.Core)

![Nuget](https://img.shields.io/nuget/v/laget.PskAuthentication.Mvc?label=laget.PskAuthentication.Mvc)
![Nuget](https://img.shields.io/nuget/dt/laget.PskAuthentication.Mvc?label=laget.PskAuthentication.Mvc)

## Configuration
> This example is shown using Autofac since this is the go-to IoC for us.
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
            Key = _configuration.GetValue<string>("Security:Key"),
            IV = _configuration.GetValue<string>("Security:IV"),
            Salt = _configuration.GetValue<string>("Security:Salt"),
            Secret = _configuration.GetValue<string>("Security:Secret")
        })).AsSelf();
    }
}
```

### appsettings.json
```c#
"Security": {
  "Key": "...",
  "IV": "...",
  "Salt": "...",
  "Secret": "..."
}
```

## Usage
### Controller
```c#
[PskAuthentication]
public class SomeController : ControllerBase
{
}
```

## Aes
You can generate the necessary Aes properties via the code below or visit https://rextester.com/RMKPPK46300

```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
    
namespace Rextester
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.GenerateIV();
            aes.GenerateKey();
            
            Console.WriteLine(Convert.ToBase64String(aes.IV));
            Console.WriteLine(Convert.ToBase64String(aes.Key));
        }
    }
}
```
