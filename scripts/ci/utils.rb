require 'fileutils'
require 'open-uri'
require 'json'

# iOSはSDKとXamarin内のモジュール名で差異がある
IOS_MODULE_NAMES = %w(Core InAppMessaging Notification Variables)
IOS_SDK_MODULE_NAMES = %w(Core InAppMessaging RemoteNotification Variables)
ANDROID_MODULE_NAMES = %w(Core InAppMessaging Notifications Variables)

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
      latest_versions[module_name] = latest_version
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
    latest_versions[module_name] = latest_version
  end

  latest_versions
end