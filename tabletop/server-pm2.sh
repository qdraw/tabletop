#!/bin/bash
pm2 stop tabletop
echo "Starting up build pipeline"
export ASPNETCORE_URLS="http://localhost:5145/"
#export ASPNETCORE_URLS="http://*:5123"
export ASPNETCORE_ENVIRONMENT="Production"

cd tabletop/bin/Debug/netcoreapp2.0/
pm2 start --name tabletop ./linux-arm/publish/tabletop
echo "tabletop started"
