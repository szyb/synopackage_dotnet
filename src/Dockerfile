#######################
# Build runtime image #
#######################

FROM mcr.microsoft.com/dotnet/aspnet:6.0.4-bullseye-slim as base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT docker
EXPOSE 80/tcp

##################
# Build frontend #
##################

FROM node:16.15.1-bullseye-slim as build-front
WORKDIR /app

COPY Synopackage.Web .

RUN rm -rf wwwroot \
  && rm src/environments/environment.*.ts \
  && rm appsettings.*.json \
  && rm global.json

COPY Synopackage.Web/src/environments/environment.ts src/environments
RUN true
COPY Synopackage.Web/src/environments/environment.docker.ts src/environments
RUN true

RUN npm i --location=global @angular/cli@11.2.18

RUN yarn --network-timeout 100000 \
  && cd src \
  && npm rebuild node-sass \
  && ng build --configuration=docker

#################
# Build backend #
#################

FROM mcr.microsoft.com/dotnet/sdk:6.0.202-bullseye-slim as build-env
WORKDIR /app

COPY ["Synopackage.Web/Synopackage.Web.csproj",  "Synopackage.Web/"]
RUN true
COPY ["Synopackage.Generator/Synopackage.Generator.csproj",  "Synopackage.Generator/"]
RUN true
COPY ["Synopackage.Model/Synopackage.Model.csproj",  "Synopackage.Model/"]
RUN true
RUN dotnet restore "Synopackage.Web/Synopackage.Web.csproj"

COPY . .
RUN rm -rf Synopackage.Web/wwwroot \
  && rm Synopackage.Web/src/environments/environment.*.ts \
  && rm Synopackage.Web/appsettings.*.json \
  && rm Synopackage.Web/global.json

COPY Synopackage.Web/appsettings.json Synopackage.Web/appsettings.json
RUN true
COPY Synopackage.Web/appsettings.docker.json Synopackage.Web/appsettings.docker.json
RUN true

RUN dotnet publish Synopackage.Docker.sln -c Release -o out --no-restore

#######################
# Build runtime image #
#######################

FROM base as final
WORKDIR /app
COPY --from=build-env /app/out .
COPY --from=build-front /app/wwwroot wwwroot
VOLUME /app/wwwroot/cache

ENTRYPOINT ["dotnet", "Synopackage.Web.dll"]
