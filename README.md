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
3. `npm install`
4. `ng build`
5. `cd ..`
6. `dotnet restore`
7. `dotnet build`
8. `dotnet run`
9. in browser `http://localhost:58893`

### Custom dev environment

If you need own development environment, you can add yours. See "dev2" configuration, which uses proxy.


## Prerequisites

1. Installed Node.js (version 10.x)
2. .NET CORE 2.1.6 SDK

VS Code extensions:
1. C#
2. TSLint
3. ESLint

## Docker instructions
To create docker image:

1. Go to root folder
2. Run `docker-compose build` (this can take a while)
3. When it's done run: `run_in_docker.bat` or `docker run -d -p 8080:80 --name synopackage -e "ASPNETCORE_ENVIRONMENT=docker" synopackage_dotnet`
4. visit website at `http://your_host_domain:8080` (i.e. `http://localhost:8080` if you run image on your local machine)

Limitations:
1. For now there is no support for system proxy (the website will work only in 'no proxy' host environment)
