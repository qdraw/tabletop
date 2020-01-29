#!/bin/bash
cd "$(dirname "$0")"

## DEPLOY ONLY
# for warnup check: ./pm2-warmup.sh --port 4823

PM2NAME="tabletop"
RUNTIME="linux-arm"

ARGUMENTS=("$@")

for ((i = 1; i <= $#; i++ )); do
  if [ $i -gt 1 ]; then
    PREV=$(($i-2))
    CURRENT=$(($i-1))

    if [[ ${ARGUMENTS[CURRENT]} == "--help" ]];
    then
        echo "--name pm2name"
        echo "--runtime linux-arm"
        echo "(or:) --runtime linux-arm64"
    fi

    if [[ ${ARGUMENTS[PREV]} == "--name" ]];
    then
        PM2NAME="${ARGUMENTS[CURRENT]}"
    fi

    if [[ ${ARGUMENTS[PREV]} == "--runtime" ]];
    then
        RUNTIME="${ARGUMENTS[CURRENT]}"
    fi
  fi
done

# settings
echo "pm2" $PM2NAME "runtime" $RUNTIME

if [ ! -f "tabletop-$RUNTIME.zip" ]; then
    echo "> tabletop-$RUNTIME.zip not found"
    exit
fi

pm2 stop $PM2NAME

if [ -f starsky.dll ]; then
    echo "delete dlls so, and everything except pm2 helpers, and"
    echo "configs, temp, thumbnailTempFolder, deploy zip, sqlite database"

    LSOUTPUT=$(ls)
    for ENTRY in $LSOUTPUT
    do
        if [[ $ENTRY != "appsettings"* && $ENTRY != "pm2-"*
        && $ENTRY != "tabletop-$RUNTIME.zip"
        && $ENTRY != *".db" ]];
        then
            rm -rf "$ENTRY"
        else
            echo "$ENTRY"
        fi
    done
fi


if [ -f tabletop-$RUNTIME.zip ]; then
   unzip -o tabletop-$RUNTIME.zip
else
   echo "> tabletop-$RUNTIME.zip File not found"
   exit
fi

if [ -f tabletop ]; then
    chmod +x ./tabletop
fi

pm2 start $PM2NAME

echo "!> done with deploying"
echo "!> to warmup, you need to run: ./pm2-warmup.sh --port 5145"
