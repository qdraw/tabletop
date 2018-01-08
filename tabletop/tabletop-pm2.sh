#!/bin/bash
pm2 stop tabletop
echo "Starting up build pipeline"
export ASPNETCORE_URLS="http://localhost:5145/"
#export ASPNETCORE_URLS="http://*:5123"
export ASPNETCORE_ENVIRONMENT="Production"

cd tabletop
#hack to allow to do this multiple times
rm -rf obj
dotnet restore
dotnet build
pm2 start --name tabletop dotnet -- run

echo "User Service started"
