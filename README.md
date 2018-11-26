# Synopackage.com

This is a repository of https://search.synopackage.com a search engine for packages to Synology's DSM across all known servers.

## Setting up dev environment
0. Best IDE for this project is Visual Studio Code. There are tasks defined and ready for debug (C# code). 
- Open folder `src/synopackage_dotnet`
- Run task `ng build (development)`
- Press F5 - to run with debug

OR

1. Clone or fork repository
2. `cd src/synopackage_dotnet/src/`
3. `ng build`
4. `cd ..`
5. `dotnet restore`
6. `dotnet build`
7. `dotnet run`
8. in browser `http://localhost:58893`

### Custom dev environment

If you need own development environment, you can add yours. See "dev2" configuration, which uses proxy.


## Prerequisites

1. Node.js (version 10.x)
2. .NET CORE 2.1.6

VS Code extensions:
1. C#
2. TSLint
3. ESLint
