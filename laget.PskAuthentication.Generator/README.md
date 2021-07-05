# laget.PskAuthentication.Generator
We're using this Generator to generate a `pre-shared key` (`PSK`) that should be used for testing purposes only, since the valid period for this `pre-shared key` is __`10 years`__!

## Configuration
To be able to generate a `pre-shared key` you need to specify the needed properties in `appsettings.Development.json`. 

The part you need to provide values for is:
```c#
"Security": {
    "RijndaelIV": "",
    "RijndaelKey": "",
    "Salt": "",
    "Secret": ""
  }
```

> You can generate the necessary Rijndael properties via the code below or visit https://rextester.com/EFWCK32767

### appsettings.json
```c#
{
  "Security": {
    "RijndaelIV": "",
    "RijndaelKey": "",
    "Salt": "",
    "Secret": ""
  },
  "Serilog": {
    "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File" ],
    "MinimumLevel": "Verbose",
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "restrictedToMinimumLevel": "Verbose"
        }
      },
      {
        "Name": "File",
        "Args": {
          "path": "C:\\ProgramData\\laget.PskAuthentication.Generator\\debug.development-.log",
          "restrictedToMinimumLevel": "Verbose",
          "retainedFileCountLimit": 7,
          "rollingInterval": "Day",
          "shared": true
        }
      }
    ]
  }
}
```