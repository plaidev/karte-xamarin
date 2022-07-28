require 'fileutils'
require 'open-uri'
require 'json'

VERSION_FILE_PATH = '../../.karte_sdk_versions'
VERSION_DIFF_FILE_PATH = '../../.karte_sdk_versions_diff'

# iOSはSDKとXamarin内のモジュール名で差異がある
IOS_MODULE_NAMES = %w(Core InAppMessaging Notification Variables)
IOS_SDK_MODULE_NAMES = %w(Core InAppMessaging RemoteNotification Variables)
ANDROID_MODULE_NAMES = %w(Core InAppMessaging Notifications Variables)

def check_sdk_update
  FileUtils.touch(VERSION_FILE_PATH) unless File.exist?(VERSION_FILE_PATH)

  current_versions = get_current_version

  diff = { ios: {}, android: {} }

  all_latest_versions = { ios: get_ios_latest_versions, android: get_android_latest_versions }

  all_latest_versions.each do |platform, latest_versions|
    current_versions[platform].each do |mod, current_version|
      if latest_versions[mod.to_sym] != current_version
        diff[platform][mod.to_sym] = latest_versions[mod.to_sym]
      end
    end
  end

  File.open(VERSION_DIFF_FILE_PATH, 'w') do |f|
    JSON.dump(diff, f)
  end
end

def update_sdk_version_file
  version_diff = get_version_diff
  new_versions = get_current_version

  new_versions[:ios] = new_versions[:ios].merge(version_diff[:ios])
  new_versions[:android] = new_versions[:android].merge(version_diff[:android])

  File.open(VERSION_FILE_PATH, 'w') do |f|
    JSON.dump(new_versions, f)
  end
end

def get_ios_latest_versions
  latest_versions = {}
  IOS_SDK_MODULE_NAMES.zip(IOS_MODULE_NAMES).each do |sdk_module_name, module_name|
    podspec_path = "../../karte-ios-sdk/Karte#{sdk_module_name}.podspec"
    latest_version = ''
    File.open(podspec_path, 'r') do |f|
      while line = f.readline
        # s.version = '2.19.0' のような行からバージョンを取り出す
        if line =~ /s\.version/
          latest_version = line.split('=')[1].strip[1..-2]
          break
        end
      end
    end
    if latest_version != ''
      latest_versions[module_name.to_sym] = latest_version
    else
      raise "Version info not found in Karte#{sdk_module_name}.podspec"
    end
  end

  latest_versions
end

def get_android_latest_versions
  latest_versions = {}

  ANDROID_MODULE_NAMES.each do |module_name|
    library_detail = URI.open("https://search.maven.org/solrsearch/select?q=g:io.karte.android+AND+a:#{module_name.downcase}&wt=json").read
    latest_version = JSON.parse(library_detail)['response']['docs'][0]['latestVersion']
    latest_versions[module_name.to_sym] = latest_version
  end

  latest_versions
end

def get_version_diff
  version_diff = {}
  File.open(VERSION_DIFF_FILE_PATH, 'r') do |f|
    version_diff = JSON.load(f.read, nil, symbolize_names: true, create_additions: false)
  end
  version_diff
end

def get_current_version
  current_versions = nil
  File.open(VERSION_FILE_PATH, 'r') do |f|
    current_versions = JSON.load(f.read, nil, symbolize_names: true, create_additions: false)
  end
  current_versions || {}
end

def cleanup
  FileUtils.rm(VERSION_DIFF_FILE_PATH)
end