#!/bin/bash -e

cd `dirname $0`

echo "Define parameters"

SRC_PATH="../karte-component/src/ios-unified/source"
OUTPUT_PATH="../karte-component/lib/ios-unified"

echo "Copy DLL"

cp -Rf "$SRC_PATH/Karte.iOS.Core/bin/Release/" "$OUTPUT_PATH"
cp -Rf "$SRC_PATH/Karte.iOS.InAppMessaging/bin/Release/" "$OUTPUT_PATH"
cp -Rf "$SRC_PATH/Karte.iOS.Notification/bin/Release/" "$OUTPUT_PATH"
cp -Rf "$SRC_PATH/Karte.iOS.Variables/bin/Release/" "$OUTPUT_PATH"