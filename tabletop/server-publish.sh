#!/bin/bash
cd tabletop
rm -rf obj
rm -rf bin
dotnet restore
dotnet build
dotnet publish -r linux-arm
