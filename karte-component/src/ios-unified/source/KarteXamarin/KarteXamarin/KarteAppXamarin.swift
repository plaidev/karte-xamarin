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

import Foundation
import WebKit
import KarteCore


@objc(KRTConfigurationXamarin)
public class KarteConfigurationXamarin: NSObject {
    
    @objc
    public static var defaultConfiguration: KarteConfigurationXamarin {
        KarteConfigurationXamarin(configuration: Configuration.defaultConfiguration)
    }
    
    @objc
    public var isDryRun: Bool {
        self.configuration.isDryRun
    }
    
    @objc
    public var isOptOut: Bool {
        self.configuration.isOptOut
    }
    
    @objc
    public weak var idfaDelegate: KarteIDFADelegateXamarin? {
        get {
            (self.configuration.idfaDelegate as! KarteIDFADelegateXamarin)
        } set {
            self.configuration.idfaDelegate = newValue
        }
    }
    
    @objc
    public override init() {
        self.configuration = Configuration.defaultConfiguration
    }
    
    var configuration: Configuration
    
    init(configuration: Configuration) {
        self.configuration = configuration
    }
    
    deinit {
    }
}

@objc(KRTIDFADelegateXamarin)
public protocol KarteIDFADelegateXamarin: IDFADelegate {
    
    @objc var advertisingIdentifierString: String? { get }
    
    @objc var isAdvertisingTrackingEnabled: Bool { get }
}

@objc(KRTXamarin)
public class KarteAppXamarin: NSObject, Library {
    
    @objc
    public class func _krt_load() {
        KarteApp.register(library: self)
    }
    
    public static var name: String {
        "xamarin"
    }
    
    public static var version: String {
        KRTXamarinCurrentLibraryVersion();
    }
    
    public static var isPublic: Bool {
        true
    }
    
    public static func configure(app: KarteApp) {
    }
    
    public static func unconfigure(app: KarteApp) {
    }
    
    @objc
    public class var visitorId: String {
        KarteApp.visitorId
    }
    
    @objc
    public class var configuration: KarteConfigurationXamarin {
        KarteConfigurationXamarin(configuration: KarteApp.configuration)
    }
    
    @objc
    public class var isOptOut: Bool {
        KarteApp.isOptOut
    }
    
    @objc
    public class func setup(appKey: String) {
        KarteApp.setup(appKey: appKey, configuration: Configuration.defaultConfiguration)
    }
    
    @objc
    public class func setup(appKey: String, configuration: KarteConfigurationXamarin) {
        KarteApp.setup(appKey: appKey, configuration: configuration.configuration)
    }
    
    @objc
    public class func setLogLevel(logLevel: LogLevel) {
        KarteApp.setLogLevel(logLevel)
    }
    
    @objc
    public class func setLogEnabled(_ isEnabled: Bool) {
        KarteApp.setLogEnabled(isEnabled)
    }
    
    @objc
    public class func optIn() {
        KarteApp.optIn()
    }
    
    @objc
    public class func optOut() {
        KarteApp.optOut()
    }
    
    @objc
    public class func renewVisitorId() {
        KarteApp.renewVisitorId()
    }
    
    @objc(application:openURL:)
    @discardableResult
    public class func application(_ app: UIApplication, open url: URL) -> Bool {
        KarteApp.application(app, open: url)
    }
    
    deinit {
    }
}

@objc(KRTTrackerXamarin)
public class TrackerXamarin: NSObject {
    
    @objc
    public init(view: UIView?) {
        _ = Tracker.init(view: view)
    }
    
    deinit {
    }
    
    @objc
    @discardableResult
    public static func track(_ name: String) -> TrackingTaskXamarin {
        let task = TrackerObjectiveC.track(name, values: [:])
        return TrackingTaskXamarin(trackingTask: task)
    }
    
    @objc
    @discardableResult
    public static func track(_ name: String, values: NSDictionary = [:]) -> TrackingTaskXamarin {
        let task = TrackerObjectiveC.track(name, values: values as! [String : Any])
        return TrackingTaskXamarin(trackingTask: task)
    }
    
    @objc
    @discardableResult
    public static func identify(_ values: NSDictionary) -> TrackingTaskXamarin {
        let task = Tracker.identify(JSONConvertibleConverter.convert(values as! [String : Any]))
        return TrackingTaskXamarin(trackingTask: task)
    }
    
    @objc
    @discardableResult
    public static func view(_ viewName: String) -> TrackingTaskXamarin {
        let task = Tracker.view(viewName, title: nil)
        return TrackingTaskXamarin(trackingTask: task)
    }
    
    @objc
    @discardableResult
    public static func view(_ viewName: String, title: String?) -> TrackingTaskXamarin {
        let task = Tracker.view(viewName, title: title, values: [:])
        return TrackingTaskXamarin(trackingTask: task)
    }
    
    @objc
    @discardableResult
    public static func view(_ viewName: String, title: String?, values: NSDictionary) -> TrackingTaskXamarin {
        let task = Tracker.view(
            viewName,
            title: title,
            values: JSONConvertibleConverter.convert(values as! [String : Any])
        )
        return TrackingTaskXamarin(trackingTask: task)
    }
    
    // Instance Methods
    
    @objc(trackForScene:)
    @discardableResult
    public func track(_ name: String) -> TrackingTaskXamarin {
        let task = TrackerObjectiveC.track(name, values: [:])
        return TrackingTaskXamarin(trackingTask: task)
    }
    
    @objc(trackForScene:values:)
    @discardableResult
    public func track(_ name: String, values: NSDictionary = [:]) -> TrackingTaskXamarin {
        let task = TrackerObjectiveC.track(name, values: values as! [String : Any])
        return TrackingTaskXamarin(trackingTask: task)
    }
    
    @objc(identifyForScene:)
    @discardableResult
    public func identify(_ values: NSDictionary) -> TrackingTaskXamarin {
        let task = Tracker.identify(JSONConvertibleConverter.convert(values as! [String : Any]))
        return TrackingTaskXamarin(trackingTask: task)
    }
    
    @objc(viewForScene:)
    @discardableResult
    public func view(_ viewName: String) -> TrackingTaskXamarin {
        let task = Tracker.view(viewName, title: nil)
        return TrackingTaskXamarin(trackingTask: task)
    }
    
    @objc(viewForScene:title:)
    @discardableResult
    public func view(_ viewName: String, title: String?) -> TrackingTaskXamarin {
        let task = Tracker.view(viewName, title: title, values: [:])
        return TrackingTaskXamarin(trackingTask: task)
    }
    
    @objc(viewForScene:title:values:)
    @discardableResult
    public func view(_ viewName: String, title: String?, values: NSDictionary) -> TrackingTaskXamarin {
        let task = Tracker.view(
            viewName,
            title: title,
            values: JSONConvertibleConverter.convert(values as! [String : Any])
        )
        return TrackingTaskXamarin(trackingTask: task)
    }
}

@objc(KRTTrackingTaskXamarin)
public class TrackingTaskXamarin: NSObject {
    @objc public var completion: TrackCompletion? {
        get {
            trackingTask.completion
        }
        set {
            trackingTask.completion = newValue
        }
    }
    
    var trackingTask: TrackingTask
    
    init(trackingTask: TrackingTask) {
        self.trackingTask = trackingTask
    }
    
    func resolve() {
        DispatchQueue.main.async {
            self.trackingTask.completion?(true)
        }
    }
    
    func reject() {
        DispatchQueue.main.async {
            self.trackingTask.completion?(false)
        }
    }
    
    deinit {
    }
}

@objc(KRTUserSyncXamarin)
public class UserSyncXamarin: NSObject {
    
    @objc(appendingQueryParameterWithURLString:)
    public static func appendingQueryParameterWithURLString(urlString: String) -> String {
        UserSync.appendingQueryParameter(urlString)
    }
    
    @objc(appendingQueryParameterWithURL:)
    public static func appendingQueryParameter(_ url: URL) -> URL {
        UserSync.appendingQueryParameter(url)
    }
    
    @objc(setUserSyncScriptWithWebView:)
    public static func setUserSyncScript(_ webView: WKWebView) {
        UserSync.setUserSyncScript(webView)
    }
    
    deinit {
    }
}
