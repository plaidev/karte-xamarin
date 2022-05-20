#!/bin/bash -e

# Builds a fat library for a given xcode project (framework)

cd `dirname $0`

echo "Define parameters"

IOS_SDK_VER="14.4" # xcodebuild -showsdks
SRC_PROJ_NAME="Karte"
SRC_PATH="../../karte-ios-sdk/$SRC_PROJ_NAME.xcworkspace"
KARTE_XM_PATH="../../karte-component/src/ios-unified/source/KarteXamarin/KarteXamarin.xcworkspace"
BUILD_PATH="../build"
PRODUCTS_PATH="$BUILD_PATH/Build/Products"
OUTPUT_PATH="../../karte-component/src/ios-unified/frameworks"
BUILD_SETTINGS="ENABLE_BITCODE=NO MACH_O_TYPE=mh_dylib"
BUILD_SETTINGS_KARTE_XM="ENABLE_BITCODE=NO"

echo "Build KarteXamarin framework for simulator and device"
rm -Rf "$BUILD_PATH"

xcodebuild -scheme KarteXamarin -sdk iphoneos$IOS_SDK_VER -workspace "$KARTE_XM_PATH" -configuration Release $BUILD_SETTINGS_KARTE_XM -derivedDataPath $BUILD_PATH DEVELOPMENT_TEAM=""
xcodebuild -scheme KarteXamarin -sdk iphonesimulator$IOS_SDK_VER -workspace "$KARTE_XM_PATH" -configuration Release $BUILD_SETTINGS_KARTE_XM -derivedDataPath $BUILD_PATH EXCLUDED_ARCHS="arm64" DEVELOPMENT_TEAM=""

echo "Build Other iOS framework for simulator and device"

xcodebuild -scheme KarteCore -sdk iphoneos$IOS_SDK_VER -workspace "$SRC_PATH" -configuration Release $BUILD_SETTINGS -derivedDataPath $BUILD_PATH
xcodebuild -scheme KarteCore -sdk iphonesimulator$IOS_SDK_VER -workspace "$SRC_PATH" -configuration Release $BUILD_SETTINGS -derivedDataPath $BUILD_PATH EXCLUDED_ARCHS="arm64"

xcodebuild -scheme KarteUtilities -sdk iphoneos$IOS_SDK_VER -workspace "$SRC_PATH" -configuration Release $BUILD_SETTINGS -derivedDataPath $BUILD_PATH
xcodebuild -scheme KarteUtilities -sdk iphonesimulator$IOS_SDK_VER -workspace "$SRC_PATH" -configuration Release $BUILD_SETTINGS -derivedDataPath $BUILD_PATH EXCLUDED_ARCHS="arm64"

xcodebuild -scheme KarteInAppMessaging -sdk iphoneos$IOS_SDK_VER -workspace "$SRC_PATH" -configuration Release $BUILD_SETTINGS -derivedDataPath $BUILD_PATH
xcodebuild -scheme KarteInAppMessaging -sdk iphonesimulator$IOS_SDK_VER -workspace "$SRC_PATH" -configuration Release $BUILD_SETTINGS -derivedDataPath $BUILD_PATH EXCLUDED_ARCHS="arm64"

xcodebuild -scheme KarteVariables -sdk iphoneos$IOS_SDK_VER -workspace "$SRC_PATH" -configuration Release $BUILD_SETTINGS -derivedDataPath $BUILD_PATH
xcodebuild -scheme KarteVariables -sdk iphonesimulator$IOS_SDK_VER -workspace "$SRC_PATH" -configuration Release $BUILD_SETTINGS -derivedDataPath $BUILD_PATH EXCLUDED_ARCHS="arm64"

xcodebuild -scheme KarteRemoteNotification -sdk iphoneos$IOS_SDK_VER -workspace "$SRC_PATH" -configuration Release $BUILD_SETTINGS -derivedDataPath $BUILD_PATH
xcodebuild -scheme KarteRemoteNotification -sdk iphonesimulator$IOS_SDK_VER -workspace "$SRC_PATH" -configuration Release $BUILD_SETTINGS -derivedDataPath $BUILD_PATH EXCLUDED_ARCHS="arm64"

echo "Create fat binaries for Release-iphoneos and Release-iphonesimulator configuration"
echo "Copy one build as a fat framework"
cp -R "$PRODUCTS_PATH/Release-iphoneos" "$PRODUCTS_PATH/Release-fat"

echo "Combine modules from another build with the fat framework modules"
cp -R "$PRODUCTS_PATH/Release-iphonesimulator/KarteXamarin.framework/Modules/KarteXamarin.swiftmodule/" "$PRODUCTS_PATH/Release-fat/KarteXamarin.framework/Modules/KarteXamarin.swiftmodule/"
cp -R "$PRODUCTS_PATH/Release-iphonesimulator/KarteCore.framework/Modules/KarteCore.swiftmodule/" "$PRODUCTS_PATH/Release-fat/KarteCore.framework/Modules/KarteCore.swiftmodule/"
cp -R "$PRODUCTS_PATH/Release-iphonesimulator/KarteUtilities.framework/Modules/KarteUtilities.swiftmodule/" "$PRODUCTS_PATH/Release-fat/KarteUtilities.framework/Modules/KarteUtilities.swiftmodule/"
cp -R "$PRODUCTS_PATH/Release-iphonesimulator/KarteInAppMessaging.framework/Modules/KarteInAppMessaging.swiftmodule/" "$PRODUCTS_PATH/Release-fat/KarteInAppMessaging.framework/Modules/KarteInAppMessaging.swiftmodule/"
cp -R "$PRODUCTS_PATH/Release-iphonesimulator/KarteVariables.framework/Modules/KarteVariables.swiftmodule/" "$PRODUCTS_PATH/Release-fat/KarteVariables.framework/Modules/KarteVariables.swiftmodule/"
cp -R "$PRODUCTS_PATH/Release-iphonesimulator/KarteRemoteNotification.framework/Modules/KarteRemoteNotification.swiftmodule/" "$PRODUCTS_PATH/Release-fat/KarteRemoteNotification.framework/Modules/KarteRemoteNotification.swiftmodule/"

echo "Combine iphoneos + iphonesimulator configuration as fat libraries"
lipo -create -output "$PRODUCTS_PATH/Release-fat/KarteXamarin.framework/KarteXamarin" "$PRODUCTS_PATH/Release-iphoneos/KarteXamarin.framework/KarteXamarin" "$PRODUCTS_PATH/Release-iphonesimulator/KarteXamarin.framework/KarteXamarin"
lipo -create -output "$PRODUCTS_PATH/Release-fat/KarteCore.framework/KarteCore" "$PRODUCTS_PATH/Release-iphoneos/KarteCore.framework/KarteCore" "$PRODUCTS_PATH/Release-iphonesimulator/KarteCore.framework/KarteCore"
lipo -create -output "$PRODUCTS_PATH/Release-fat/KarteUtilities.framework/KarteUtilities" "$PRODUCTS_PATH/Release-iphoneos/KarteUtilities.framework/KarteUtilities" "$PRODUCTS_PATH/Release-iphonesimulator/KarteUtilities.framework/KarteUtilities"
lipo -create -output "$PRODUCTS_PATH/Release-fat/KarteInAppMessaging.framework/KarteInAppMessaging" "$PRODUCTS_PATH/Release-iphoneos/KarteInAppMessaging.framework/KarteInAppMessaging" "$PRODUCTS_PATH/Release-iphonesimulator/KarteInAppMessaging.framework/KarteInAppMessaging"
lipo -create -output "$PRODUCTS_PATH/Release-fat/KarteVariables.framework/KarteVariables" "$PRODUCTS_PATH/Release-iphoneos/KarteVariables.framework/KarteVariables" "$PRODUCTS_PATH/Release-iphonesimulator/KarteVariables.framework/KarteVariables"
lipo -create -output "$PRODUCTS_PATH/Release-fat/KarteRemoteNotification.framework/KarteRemoteNotification" "$PRODUCTS_PATH/Release-iphoneos/KarteRemoteNotification.framework/KarteRemoteNotification" "$PRODUCTS_PATH/Release-iphonesimulator/KarteRemoteNotification.framework/KarteRemoteNotification"

echo "Verify results"
lipo -info "$PRODUCTS_PATH/Release-fat/KarteXamarin.framework/KarteXamarin"
lipo -info "$PRODUCTS_PATH/Release-fat/KarteCore.framework/KarteCore"
lipo -info "$PRODUCTS_PATH/Release-fat/KarteUtilities.framework/KarteUtilities"
lipo -info "$PRODUCTS_PATH/Release-fat/KarteInAppMessaging.framework/KarteInAppMessaging"
lipo -info "$PRODUCTS_PATH/Release-fat/KarteVariables.framework/KarteVariables"
lipo -info "$PRODUCTS_PATH/Release-fat/KarteRemoteNotification.framework/KarteRemoteNotification"

echo "Copy fat frameworks to the frameworks folder"
cp -Rf "$PRODUCTS_PATH/Release-fat/KarteXamarin.framework" "$OUTPUT_PATH"
cp -Rf "$PRODUCTS_PATH/Release-fat/KarteCore.framework" "$OUTPUT_PATH"
cp -Rf "$PRODUCTS_PATH/Release-fat/KarteUtilities.framework" "$OUTPUT_PATH"
cp -Rf "$PRODUCTS_PATH/Release-fat/KarteInAppMessaging.framework" "$OUTPUT_PATH"
cp -Rf "$PRODUCTS_PATH/Release-fat/KarteVariables.framework" "$OUTPUT_PATH"
cp -Rf "$PRODUCTS_PATH/Release-fat/KarteRemoteNotification.framework" "$OUTPUT_PATH"
