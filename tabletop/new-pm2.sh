#!/bin/bash
export ASPNETCORE_URLS="http://localhost:5145/"
export ASPNETCORE_ENVIRONMENT="Production"

echo "Copy the database string and press [ENTER]:"
echo "for example: "
echo "Server=tcp:server.database.windows.net,1433;Database=databasename;Persist Security Info=False;User ID=username;Password=password-here;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
echo ">>>"
read -p "Enter: " SQLSERVERSTRING

export TABLETOP_SQL=$SQLSERVERSTRING

FOLDER=false
if [[ $OSTYPE == "darwin"* ]]; then
	FOLDER="osx.10.11-x64"
elif [[ $OSTYPE == "linux-gnu" ]]; then
	if [[ arch == "i386" ]]; then
		FOLDER="linux-x64"
	fi
	if [[ arch == "armv7l" ]]; then
		FOLDER="linux-arm"
	fi
fi
# check types here: https://docs.microsoft.com/en-us/dotnet/core/rid-catalog

echo "select arch type: "$FOLDER

if [ $FOLDER != false ]; then

	if [ ! -d "tabletop/bin/release/netcoreapp2.0/$FOLDER/publish" ]; then
		echo "no folder"
		exit
	fi
	cd tabletop/bin/release/netcoreapp2.0/$FOLDER/publish
	pm2 start --name tabletop ./tabletop
	echo "tabletop started"

	pm2 status
fi


#start on mac
#dotnet run --launch-profile Production
