FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build-env
WORKDIR /app


# Setup node
ENV NODE_VERSION 12.13.1
ENV NODE_DOWNLOAD_SHA 074a6129da34b768b791f39e8b74c6e4ab3349d1296f1a303ef3547a7f9cf9be



COPY . .
RUN rm -rf wwwroot \
  && rm src/environments/environment.*.ts \
  && rm appsettings.*.json \
  && rm global.json

COPY src/environments/environment.ts src/environments
COPY src/environments/environment.docker.ts src/environments
COPY appsettings.json .
COPY appsettings.docker.json .

RUN curl -SL "https://nodejs.org/dist/v${NODE_VERSION}/node-v${NODE_VERSION}-linux-x64.tar.gz" --output nodejs.tar.gz \
  && echo "$NODE_DOWNLOAD_SHA nodejs.tar.gz" | sha256sum -c - \
  && tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
  && rm nodejs.tar.gz \
  && ln -s /usr/local/bin/node /usr/local/bin/nodejs\
  && npm i -g @angular/cli@7.3.9\
  && npm i -g yarn

# Copy csproj and restore as distinct layers
#COPY src/synopackage_dotnet/ ./synopackage_dotnet/
RUN dotnet restore \
  && yarn \
  && cd src \
  && npm rebuild node-sass \
  && ng build --configuration=docker


RUN dotnet publish -c Release -o out

# build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT docker
# Setup node
ENV NODE_VERSION 12.13.1
ENV NODE_DOWNLOAD_SHA 074a6129da34b768b791f39e8b74c6e4ab3349d1296f1a303ef3547a7f9cf9be


EXPOSE 80/tcp



RUN curl -SL "https://nodejs.org/dist/v${NODE_VERSION}/node-v${NODE_VERSION}-linux-x64.tar.gz" --output nodejs.tar.gz \
  && echo "$NODE_DOWNLOAD_SHA nodejs.tar.gz" | sha256sum -c - \
  && tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
  && rm nodejs.tar.gz \
  && ln -s /usr/local/bin/node /usr/local/bin/nodejs

COPY --from=build-env /app/out .

VOLUME /app/wwwroot/cache

ENTRYPOINT ["dotnet", "synopackage_dotnet.dll"]
