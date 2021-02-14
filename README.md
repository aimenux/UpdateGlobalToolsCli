![.NET](https://github.com/aimenux/UpdateGlobalToolsCli/workflows/.NET/badge.svg)

# UpdateGlobalToolsCli
```
Providing net global tool in order to update all net global tools
```

> In this repo, i m building a global tool that allows to update all net global tools.
>
> To run code in debug or release mode, type the following commands in your favorite terminal : 
> - `.\App.exe`
> - `.\App.exe -h`
> - `.\App.exe -f [NugetConfigFile]`
>
> To install, run, update, uninstall global tool from a local source path, type commands :
> - `dotnet tool install -g --add-source .\Nugets\ --configfile .\Nugets\nuget.config UpdateGlobalTools`
> - `UpdateGlobalTools`
> - `UpdateGlobalTools -h`
> - `UpdateGlobalTools -f [NugetConfigFile]`
> - `dotnet tool update -g UpdateGlobalTools --ignore-failed-sources`
> - `dotnet tool uninstall -g UpdateGlobalTools`
>
> To install global tool from [nuget source](https://www.nuget.org/packages/UpdateGlobalTools), type these command :
> - `dotnet tool install --global UpdateGlobalTools --ignore-failed-sources`

**`Tools`** : vs19, net 5.0
