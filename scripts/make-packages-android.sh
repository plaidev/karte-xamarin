#!/bin/bash -e

cd `dirname $0`

echo "Define parameters"

PJT_DIR_PATH="../Karte.Android/"
OUTPUT_PATH="../Karte.Android/nupkg"

echo "Make Packages"
rm -Rf "$OUTPUT_PATH"
mkdir "$OUTPUT_PATH"

msbuild -t:"Clean;Build;pack" -p:"Configuration=Release;DesignTimeBuild=false" $PJT_DIR_PATH/Karte.Android.Core/Karte.Android.Core.csproj
msbuild -t:"Clean;Build;pack" -p:"Configuration=Release;DesignTimeBuild=false" $PJT_DIR_PATH/Karte.Android.InAppMessaging/Karte.Android.InAppMessaging.csproj
msbuild -t:"Clean;Build;pack" -p:"Configuration=Release;DesignTimeBuild=false" $PJT_DIR_PATH/Karte.Android.Notifications/Karte.Android.Notifications.csproj
msbuild -t:"Clean;Build;pack" -p:"Configuration=Release;DesignTimeBuild=false" $PJT_DIR_PATH/Karte.Android.Variables/Karte.Android.Variables.csproj

open $OUTPUT_PATH