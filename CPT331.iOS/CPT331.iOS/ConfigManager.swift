//
//  ConfigManager.swift
//  CPT331.iOS
//
//  Created by Peter Weller on 3/11/2016.
//  Copyright © 2016 Peter Weller. All rights reserved.
//

import Foundation

/// Convenience wrapper for Config.plist
class ConfigManager {
    static let sharedInstance = ConfigManager()
    
    private let dictionary: NSDictionary!
    
    private init() {
        let plistPath = NSBundle.mainBundle().pathForResource("Config", ofType: "plist")!
        self.dictionary = NSDictionary(contentsOfFile: plistPath)
    }
    
    var eventGuardianAPI:String {
        get {
            return dictionary["API"]!.valueForKey("EventGuardian") as! String
        }
    }
    
    var darkSkyAPI:String {
        get {
            return dictionary["API"]!.valueForKey("DarkSky") as! String
        }
    }
}