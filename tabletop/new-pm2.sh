#!/bin/bash
export ASPNETCORE_URLS="http://localhost:5145/"
export ASPNETCORE_ENVIRONMENT="Production"

echo "Copy the database string and press [ENTER]:"
echo "for example: "
echo "Server=tcp:server.database.windows.net,1433;Database=databasename;Persist Security Info=False;User ID=username;Password=password-here;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
echo ">>>"
read -p "Enter: " SQLSERVERSTRING

export TABLETOP_SQL=$SQLSERVERSTRING

chmod +x ./tabletop
pm2 start --name tabletop ./tabletop
