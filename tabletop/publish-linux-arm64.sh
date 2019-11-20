#!/bin/bash
cd tabletop
rm -rf obj
rm -rf bin
dotnet restore
dotnet build
dotnet publish -c release -r linux-arm64 /p:MvcRazorCompileOnPublish=false
