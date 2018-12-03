FROM microsoft/dotnet:2.1-sdk as build-env
WORKDIR /app

# Setup node
ENV NODE_VERSION 10.13.0
ENV NODE_DOWNLOAD_SHA b4b5d8f73148dcf277df413bb16827be476f4fa117cbbec2aaabc8cc0a8588e1

RUN curl -SL "https://nodejs.org/dist/v${NODE_VERSION}/node-v${NODE_VERSION}-linux-x64.tar.gz" --output nodejs.tar.gz \
  && echo "$NODE_DOWNLOAD_SHA nodejs.tar.gz" | sha256sum -c - \
  && tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
  && rm nodejs.tar.gz \
  && ln -s /usr/local/bin/node /usr/local/bin/nodejs\
  && npm i -g @angular/cli@6.0.8 

# Copy csproj and restore as distinct layers
COPY src/synopackage_dotnet/ ./synopackage_dotnet/
RUN cd synopackage_dotnet \
  && dotnet restore \
  && npm install

RUN cd synopackage_dotnet \
  && dotnet publish -c Release -o out

# build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app

# Setup node
ENV NODE_VERSION 10.13.0
ENV NODE_DOWNLOAD_SHA b4b5d8f73148dcf277df413bb16827be476f4fa117cbbec2aaabc8cc0a8588e1

RUN curl -SL "https://nodejs.org/dist/v${NODE_VERSION}/node-v${NODE_VERSION}-linux-x64.tar.gz" --output nodejs.tar.gz \
  && echo "$NODE_DOWNLOAD_SHA nodejs.tar.gz" | sha256sum -c - \
  && tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
  && rm nodejs.tar.gz \
  && ln -s /usr/local/bin/node /usr/local/bin/nodejs\
  && npm i -g @angular/cli@6.0.8

COPY --from=build-env /app/synopackage_dotnet/out .
ENTRYPOINT ["dotnet", "synopackage_dotnet.dll"]
