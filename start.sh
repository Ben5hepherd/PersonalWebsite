#!/bin/sh

# Start the .NET API in the background
echo "Starting .NET API..."
dotnet /app/publish/PersonalWebsite.Api.dll &

# Start Nginx in the foreground
echo "Starting Nginx..."
nginx -g 'daemon off;'