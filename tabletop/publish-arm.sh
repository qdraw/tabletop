#!/bin/bash
cd tabletop
rm -rf obj
rm -rf bin
dotnet restore
dotnet build
dotnet publish -c release -r linux-arm /p:MvcRazorCompileOnPublish=false
#dotnet publish -r osx.10.11-x64
