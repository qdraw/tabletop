#!/bin/bash
pm2 stop tabletop
echo "Starting up build pipeline"
export ASPNETCORE_URLS="http://localhost:5145/"
#export ASPNETCORE_URLS="http://*:5123"
export ASPNETCORE_ENVIRONMENT="Production"

cd tabletop
chmod +x /bin/Debug/netcoreapp2.0/linux-arm/tabletop
pm2 start --name tabletop ./bin/Debug/netcoreapp2.0/linux-arm/tabletop
echo "tabletop started"
