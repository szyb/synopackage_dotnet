![.NET Core](https://github.com/szyb/synopackage_dotnet/workflows/.NET%20Core/badge.svg)
![Docker Image CI](https://github.com/szyb/synopackage_dotnet/workflows/Docker%20Image%20CI/badge.svg)
# Synopackage.com

This is a repository of https://synopackage.com a search engine for packages to Synology's DSM across all known servers.

## Prerequisites

1. Installed Node.js (version 12.x)
2. .NET 6.0.202 SDK

## Setting up dev environment
0. Best IDE for this project is Visual Studio Code. There are tasks defined and ready for debug (C# code). 
- Open folder with the code
- Go to folder `synopackage_dotnet.web/src`
- Run in command line `yarn`
- Run task `ng build (development)`
- Press F5 - to run with debug

OR

1. Clone or fork repository
2. `cd synopackage_dotnet.web/src/`
3. `yarn`
4. `ng build --configuration=development`
5. `cd ..`
6. `dotnet restore`
7. `dotnet build`
8. `dotnet run`
9. in browser `http://localhost:58893`

VS Code extensions:
1. C# (Omnisharp)
2. TSLint
3. ESLint

## Docker instructions
To create docker image:

1. Go to root folder
2. Run `docker build .` (this can take a while)
3. When it's done run: `docker run -d -p 8080:80 --name synopackage -v synopackage_cache:/app/wwwroot/cache szyb/synopackage_dotnet:latest`
  - For windows you can just run `run_in_docker.bat`
4. visit website at `http://your_host_domain:8080` (i.e. `http://localhost:8080` if you run image on your local machine)

### Proxy settings for docker
When proxy is required then use environment variables `http_proxy` and add it to command when running container, i.e.
`docker run -d -p 8080:80 --name synopackage -e "http_proxy=10.0.0.1:3128" -v synopackage_cache:/app/wwwroot/cache synopackage_dotnet`
