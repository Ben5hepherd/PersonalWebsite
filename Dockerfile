# Angular build stage
FROM node:18-alpine AS client-build
WORKDIR /app

COPY ./client/package*.json ./
COPY ./client/angular.json ./
COPY ./client/tsconfig*.json ./
COPY ./client/src ./src
COPY ./client/public ./public
RUN npm install && npm run build --prod && rm -fr node_modules

# .NET build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS api-build
WORKDIR /app
COPY ./PersonalWebsite.Api/*.csproj ./api/
RUN dotnet restore ./api/*.csproj
COPY ./PersonalWebsite.Api ./api
RUN dotnet publish ./api/*.csproj -c Release -o /app/publish

# Final production stage: Nginx + .NET (Using nginx:alpine and adding .NET runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS production

# Install tools for environment variable substitution, bash, and nginx
RUN apk add --no-cache bash gettext nginx

# Copy Angular build output to Nginx directory
COPY --from=client-build /app/dist/client/browser /usr/share/nginx/html

# Copy the custom Nginx configuration template
COPY ./nginx.conf.template /etc/nginx/nginx.conf.template

# Copy the .NET API published files to the final image
COPY --from=api-build /app/publish /app/publish

# Copy the start.sh script into the container
COPY start.sh /start.sh

# Make sure the start script is executable
RUN chmod +x /start.sh

# Set environment variables
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
ENV API_URL=${API_URL}

# Expose ports
EXPOSE 80 5000

# Run the start script
CMD /bin/sh -c "envsubst < /etc/nginx/nginx.conf.template > /etc/nginx/nginx.conf && /start.sh"
