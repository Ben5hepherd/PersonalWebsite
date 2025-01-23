FROM node:18-alpine AS client-build
WORKDIR /app
COPY ./client/package*.json ./
COPY ./client/src ./src
COPY ./client/public ./public
COPY ./client/angular.json ./
COPY ./client/tsconfig*.json ./
RUN npm install && npm install -g serve && npm run build --prod && rm -fr node_modules

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS api-build
WORKDIR /app
COPY ./PersonalWebsite.Api/*.csproj ./api/
RUN dotnet restore ./api/*.csproj
COPY ./PersonalWebsite.Api ./api/
RUN dotnet publish ./api/*.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final
COPY --from=client-build /app/dist/client/browser /usr/share/nginx/html
COPY --from=api-build /app/publish /app/publish

RUN apk add --no-cache nginx
COPY nginx.conf /etc/nginx/nginx.conf

ENV ASPNETCORE_URLS=http://0.0.0.0:5000

EXPOSE 80
EXPOSE 5000

CMD ["/bin/sh", "-c", "dotnet /app/publish/PersonalWebsite.Api.dll & exec nginx -g 'daemon off;'"]