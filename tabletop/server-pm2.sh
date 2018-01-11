#!/bin/bash
pm2 stop tabletop
echo "Starting up build pipeline"
export ASPNETCORE_URLS="http://localhost:5145/"
#export ASPNETCORE_URLS="http://*:5123"
export ASPNETCORE_ENVIRONMENT="Production"

cd tabletop/bin/release/netcoreapp2.0/linux-arm/publish
pm2 start --name tabletop ./tabletop
echo "tabletop started"

#start on mac
#dotnet run --launch-profile Production
