#!/bin/bash
cd tabletop
rm -rf obj
rm -rf bin
dotnet restore
dotnet build
dotnet publish -r linux-arm
#dotnet publish -r osx.10.11-x64
