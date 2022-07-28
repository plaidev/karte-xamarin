//
//  Copyright 2020 PLAID, Inc.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      https://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//

#import <Foundation/Foundation.h>

//! Project version number for KarteRemoteNotification.
FOUNDATION_EXPORT double KarteRemoteNotificationVersionNumber;

//! Project version string for KarteRemoteNotification.
FOUNDATION_EXPORT const unsigned char KarteRemoteNotificationVersionString[];

// In this header, you should import all the public headers of your framework using statements like #import <KarteRemoteNotification/PublicHeader.h>

FOUNDATION_EXPORT NSString * KRTRemoteNotificationCurrentLibraryVersion(void);

#import <KarteRemoteNotification/KRTApp+RemoteNotification.h>
#import <KarteRemoteNotification/KRTTracker+RemoteNotification.h>
