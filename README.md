# laget.PskAuthentication
Default implementation of Secure Pre-Shared Key (PSK) Authentication for laget.se using Rijndael.

## Usage
### appsettings.json
```c#
"Security": {
  "Psk": "...",
  "Salt": "...",
  "RijndaelIV": "...",
  "RijndaelKey": "..."
}
```

### Controller
```c#
[PskAuthentication]
public class Receipt : ControllerBase
{
}
```
