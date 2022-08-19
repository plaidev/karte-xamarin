#!/bin/bash -e

# Builds a fat library for a given xcode project (framework)

cd `dirname $0`

echo "Define parameters"

IOS_SDK_VER="15.5" # xcodebuild -showsdks
SRC_PROJ_NAME="KarteTracker"
SRC_PATH="../../karte-ios-tools/Example/$SRC_PROJ_NAME.xcworkspace"
KARTE_XM_PATH="../karte-component/src/ios-unified/source/KarteXamarin/KarteXamarin.xcworkspace"
BUILD_PATH="./build"
PRODUCTS_PATH="$BUILD_PATH/Build/Products"
OUTPUT_PATH="../karte-component/src/ios-unified/frameworks"
BUILD_SETTINGS="ENABLE_BITCODE=NO MACH_O_TYPE=mh_dylib"
BUILD_SETTINGS_KARTE_XM="ENABLE_BITCODE=NO"

echo "Build KarteXamarin framework for simulator and device"
rm -Rf "$BUILD_PATH"

xcodebuild -scheme KarteXamarin -sdk iphoneos$IOS_SDK_VER -workspace "$KARTE_XM_PATH" -configuration Release $BUILD_SETTINGS_KARTE_XM -derivedDataPath $BUILD_PATH
xcodebuild -scheme KarteXamarin -sdk iphonesimulator$IOS_SDK_VER -workspace "$KARTE_XM_PATH" -configuration Release $BUILD_SETTINGS_KARTE_XM -derivedDataPath $BUILD_PATH EXCLUDED_ARCHS="arm64"

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
cp -R "$PRODUCTS_PATH/Release-iphonesimulator/KarteCore/KarteCore.framework/Modules/KarteCore.swiftmodule/" "$PRODUCTS_PATH/Release-fat/KarteCore/KarteCore.framework/Modules/KarteCore.swiftmodule/"
cp -R "$PRODUCTS_PATH/Release-iphonesimulator/KarteUtilities/KarteUtilities.framework/Modules/KarteUtilities.swiftmodule/" "$PRODUCTS_PATH/Release-fat/KarteUtilities/KarteUtilities.framework/Modules/KarteUtilities.swiftmodule/"
cp -R "$PRODUCTS_PATH/Release-iphonesimulator/KarteInAppMessaging/KarteInAppMessaging.framework/Modules/KarteInAppMessaging.swiftmodule/" "$PRODUCTS_PATH/Release-fat/KarteInAppMessaging/KarteInAppMessaging.framework/Modules/KarteInAppMessaging.swiftmodule/"
cp -R "$PRODUCTS_PATH/Release-iphonesimulator/KarteVariables/KarteVariables.framework/Modules/KarteVariables.swiftmodule/" "$PRODUCTS_PATH/Release-fat/KarteVariables/KarteVariables.framework/Modules/KarteVariables.swiftmodule/"
cp -R "$PRODUCTS_PATH/Release-iphonesimulator/KarteRemoteNotification/KarteRemoteNotification.framework/Modules/KarteRemoteNotification.swiftmodule/" "$PRODUCTS_PATH/Release-fat/KarteRemoteNotification/KarteRemoteNotification.framework/Modules/KarteRemoteNotification.swiftmodule/"

echo "Combine iphoneos + iphonesimulator configuration as fat libraries"
lipo -create -output "$PRODUCTS_PATH/Release-fat/KarteXamarin.framework/KarteXamarin" "$PRODUCTS_PATH/Release-iphoneos/KarteXamarin.framework/KarteXamarin" "$PRODUCTS_PATH/Release-iphonesimulator/KarteXamarin.framework/KarteXamarin"
lipo -create -output "$PRODUCTS_PATH/Release-fat/KarteCore/KarteCore.framework/KarteCore" "$PRODUCTS_PATH/Release-iphoneos/KarteCore/KarteCore.framework/KarteCore" "$PRODUCTS_PATH/Release-iphonesimulator/KarteCore/KarteCore.framework/KarteCore"
lipo -create -output "$PRODUCTS_PATH/Release-fat/KarteUtilities/KarteUtilities.framework/KarteUtilities" "$PRODUCTS_PATH/Release-iphoneos/KarteUtilities/KarteUtilities.framework/KarteUtilities" "$PRODUCTS_PATH/Release-iphonesimulator/KarteUtilities/KarteUtilities.framework/KarteUtilities"
lipo -create -output "$PRODUCTS_PATH/Release-fat/KarteInAppMessaging/KarteInAppMessaging.framework/KarteInAppMessaging" "$PRODUCTS_PATH/Release-iphoneos/KarteInAppMessaging/KarteInAppMessaging.framework/KarteInAppMessaging" "$PRODUCTS_PATH/Release-iphonesimulator/KarteInAppMessaging/KarteInAppMessaging.framework/KarteInAppMessaging"
lipo -create -output "$PRODUCTS_PATH/Release-fat/KarteVariables/KarteVariables.framework/KarteVariables" "$PRODUCTS_PATH/Release-iphoneos/KarteVariables/KarteVariables.framework/KarteVariables" "$PRODUCTS_PATH/Release-iphonesimulator/KarteVariables/KarteVariables.framework/KarteVariables"
lipo -create -output "$PRODUCTS_PATH/Release-fat/KarteRemoteNotification/KarteRemoteNotification.framework/KarteRemoteNotification" "$PRODUCTS_PATH/Release-iphoneos/KarteRemoteNotification/KarteRemoteNotification.framework/KarteRemoteNotification" "$PRODUCTS_PATH/Release-iphonesimulator/KarteRemoteNotification/KarteRemoteNotification.framework/KarteRemoteNotification"

echo "Verify results"
lipo -info "$PRODUCTS_PATH/Release-fat/KarteXamarin.framework/KarteXamarin"
lipo -info "$PRODUCTS_PATH/Release-fat/KarteCore/KarteCore.framework/KarteCore"
lipo -info "$PRODUCTS_PATH/Release-fat/KarteUtilities/KarteUtilities.framework/KarteUtilities"
lipo -info "$PRODUCTS_PATH/Release-fat/KarteInAppMessaging/KarteInAppMessaging.framework/KarteInAppMessaging"
lipo -info "$PRODUCTS_PATH/Release-fat/KarteVariables/KarteVariables.framework/KarteVariables"
lipo -info "$PRODUCTS_PATH/Release-fat/KarteRemoteNotification/KarteRemoteNotification.framework/KarteRemoteNotification"

echo "Copy fat frameworks to the frameworks folder"
cp -Rf "$PRODUCTS_PATH/Release-fat/KarteXamarin.framework" "$OUTPUT_PATH"
cp -Rf "$PRODUCTS_PATH/Release-fat/KarteCore/KarteCore.framework" "$OUTPUT_PATH"
cp -Rf "$PRODUCTS_PATH/Release-fat/KarteUtilities/KarteUtilities.framework" "$OUTPUT_PATH"
cp -Rf "$PRODUCTS_PATH/Release-fat/KarteInAppMessaging/KarteInAppMessaging.framework" "$OUTPUT_PATH"
cp -Rf "$PRODUCTS_PATH/Release-fat/KarteVariables/KarteVariables.framework" "$OUTPUT_PATH"
cp -Rf "$PRODUCTS_PATH/Release-fat/KarteRemoteNotification/KarteRemoteNotification.framework" "$OUTPUT_PATH"
