## Prerequisites

1. Installed Node.js (version 16.x)
2. .NET 6.0.x SDK
3. Installed yarn (might by a part od Node.js 16.x - need to check it)
3. Installed Angular CLI version 11.2.18 (`yarn global add @angular/cli@11.2.18`)

## Setting up dev environment
### Visual Studio 2022 Community
- Open Synopackage.sln solution
- Open console and go to web project `cd Synopackage.Web\src`
- Run in console `yarn`
- Compile frontend `ng build`
- Press F5 to run with debug

### Visual Studio Code
- Open `src` folder
- Open console and go to web project `cd Synopackage.Web\src`
- Run in console `yarn`
- IN VS Code run task `ng build (development)`
- Press F5 - to run with debug

VS Code extensions:
1. C# (Omnisharp)
2. TSLint
3. ESLint

### Compile and run from command line
- Fork the repository
- Clone the repository on local computer
- `cd src/synopackage_dotnet.web/src/`
- `yarn`
- `ng build --configuration=development`
- `cd ..`
- `dotnet restore`
- `dotnet build`
- `dotnet run`
- in browser `http://localhost:58893`

## Docker instructions
To create docker image:

1. Go to `src` folder
2. Run `docker build .` (this can take a while)
3. Check the image hash (`docker images`)
4. Run it `docker run -d -p 8080:80 --name synopackage -v synopackage_cache:/app/wwwroot/cache [image_hash]`
Alternative you can download latest available image from docker registry:
- `docker run -d -p 8080:80 --name synopackage -v synopackage_cache:/app/wwwroot/cache szyb/synopackage_dotnet:latest`
5. visit website at `http://your_host_domain:8080` (i.e. `http://localhost:8080` if you run image on your local machine)

### Proxy settings for docker
When proxy is required then use environment variables `http_proxy` and add it to command when running container, i.e.
`docker run -d -p 8080:80 --name synopackage -e "http_proxy=10.0.0.1:3128" -v synopackage_cache:/app/wwwroot/cache synopackage_dotnet:latest`

# Issues?

Found issue? Feel free to contribute: file an issue or create pull request

Enjoy!
