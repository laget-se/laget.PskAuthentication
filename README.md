# laget.PskAuthentication

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
