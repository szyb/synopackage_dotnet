FROM mcr.microsoft.com/dotnet/aspnet:6.0.4-bullseye-slim as base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT docker
EXPOSE 80/tcp


FROM mcr.microsoft.com/dotnet/sdk:6.0.202-bullseye-slim as build-env
WORKDIR /app


# Setup node
ENV NODE_VERSION 16.15.1
ENV NODE_DOWNLOAD_SHA f78a49c0c9c2f546c3a44eb434c49a852125441422a1bcfc433dedc58d6a241c

COPY ["Synopackage.Web/Synopackage.Web.csproj",  "Synopackage.Web/"]
COPY ["Synopackage.Generator/Synopackage.Generator.csproj",  "Synopackage.Generator/"]
COPY ["Synopackage.Model/Synopackage.Model.csproj",  "Synopackage.Model/"]
RUN dotnet restore "Synopackage.Web/Synopackage.Web.csproj"

COPY . .
RUN rm -rf Synopackage.Web/wwwroot \
  && rm Synopackage.Web/src/environments/environment.*.ts \
  && rm Synopackage.Web/appsettings.*.json \
  && rm Synopackage.Web/global.json

COPY Synopackage.Web/src/environments/environment.ts Synopackage.Web/src/environments
RUN true
COPY Synopackage.Web/src/environments/environment.docker.ts Synopackage.Web/src/environments
RUN true
COPY Synopackage.Web/appsettings.json Synopackage.Web/appsettings.json
RUN true
COPY Synopackage.Web/appsettings.docker.json Synopackage.Web/appsettings.docker.json

RUN curl -SL "https://nodejs.org/dist/v${NODE_VERSION}/node-v${NODE_VERSION}-linux-x64.tar.gz" --output nodejs.tar.gz \
  && echo "$NODE_DOWNLOAD_SHA nodejs.tar.gz" | sha256sum -c - \
  && tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
  && rm nodejs.tar.gz \
  && ln -s /usr/local/bin/node /usr/local/bin/nodejs\
  && npm i --location=global @angular/cli@8.3.22\
  && npm i --location=global yarn

# Build frontend

RUN cd Synopackage.Web \
  && yarn --network-timeout 100000 \
  && cd src \
  && npm rebuild node-sass \
  && ng build --configuration=docker


RUN dotnet publish Synopackage.Docker.sln -c Release -o out

# build runtime image
FROM base as final
WORKDIR /app
COPY --from=build-env /app/out .
VOLUME /app/wwwroot/cache

ENTRYPOINT ["dotnet", "Synopackage.Web.dll"]
