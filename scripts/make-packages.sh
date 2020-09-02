#!/bin/bash -e

cd `dirname $0`

echo "Define parameters"

NUGET_DIR_PATH="../karte-component/nuget"
OUTPUT_PATH="../karte-component/nuget/build"

echo "Make Packages"
rm -Rf "$OUTPUT_PATH"
mkdir "$OUTPUT_PATH"

nuget pack $NUGET_DIR_PATH/Karte.iOS.Core.nuspec -OutputDirectory $OUTPUT_PATH
nuget pack $NUGET_DIR_PATH/Karte.iOS.InAppMessaging.nuspec -OutputDirectory $OUTPUT_PATH
nuget pack $NUGET_DIR_PATH/Karte.iOS.Notification.nuspec -OutputDirectory $OUTPUT_PATH
nuget pack $NUGET_DIR_PATH/Karte.iOS.Variables.nuspec -OutputDirectory $OUTPUT_PATH

open $OUTPUT_PATH