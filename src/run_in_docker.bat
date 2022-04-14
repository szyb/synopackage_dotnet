docker run -d -p 8080:80 --name synopackage -e "ASPNETCORE_ENVIRONMENT=docker" -v synopackage_cache:/app/wwwroot/cache szyb/synopackage_dotnet:latest
