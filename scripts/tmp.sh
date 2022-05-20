#!/bin/bash -e

# Builds a fat library for a given xcode project (framework)

cd `dirname $0`

echo "Define parameters"

IOS_SDK_VER="15.5" # xcodebuild -showsdks
SRC_PROJ_NAME="Karte"
SRC_PATH="../../karte-ios-sdk/$SRC_PROJ_NAME.xcworkspace"
KARTE_XM_PATH="../karte-component/src/ios-unified/source/KarteXamarin/KarteXamarin.xcworkspace"
BUILD_PATH="./build"
PRODUCTS_PATH="$BUILD_PATH/Build/Products"
OUTPUT_PATH="../karte-component/src/ios-unified/frameworks"
BUILD_SETTINGS="ENABLE_BITCODE=NO MACH_O_TYPE=mh_dylib"
BUILD_SETTINGS_KARTE_XM="ENABLE_BITCODE=NO"

echo "Build KarteXamarin framework for simulator and device"
rm -Rf "$BUILD_PATH"

echo "Build Other iOS framework for simulator and device"

xcodebuild -scheme KarteCore -sdk iphoneos$IOS_SDK_VER -workspace "$SRC_PATH" -configuration Release $BUILD_SETTINGS -derivedDataPath $BUILD_PATH
#xcodebuild -scheme KarteCore -sdk iphonesimulator$IOS_SDK_VER -workspace "$SRC_PATH" -configuration Release $BUILD_SETTINGS -derivedDataPath $BUILD_PATH EXCLUDED_ARCHS="arm64"

