require 'fileutils'
require 'open-uri'
require 'json'
require 'rexml/document'

require_relative './utils'

def increment_revision(version)
  versions = version.split('.')
  if versions.size == 3
    versions << '0'
  elsif versions.size == 4
    versions[3] = (versions[3].to_i + 1).to_s
  end
  versions.join('.')
end

script_dir_path = File.dirname($0)
tmp_dir_path = '../../tmp'
Dir.chdir(script_dir_path)

# Update versions of iOS modules
ios_latest_versions = get_ios_latest_versions
ios_core_version_changed = false
ios_any_module_version_changed = false
IOS_MODULE_NAMES.each do |module_name|
  nuspec_path = "../../karte-component/nuget/Karte.iOS.#{module_name}.nuspec"
  nuspec = File.read(nuspec_path)
  doc = REXML::Document.new(nuspec)

  doc_changed = false

  current_version = REXML::XPath.first(doc, '//version').text.strip
  if current_version != ios_latest_versions[module_name]
    REXML::XPath.first(doc, '//version').text = ios_latest_versions[module_name]
    doc_changed = true
    ios_core_version_changed = true if module_name == 'Core'
    ios_any_module_version_changed = true end

  REXML::XPath.each(doc, '//dependency') do |dep|
    IOS_MODULE_NAMES.each do |dep_module|
      if dep.attribute('id').to_s.strip == "Karte.iOS.#{dep_module}"
        REXML::XPath.first(doc, "//dependency[@id='#{"Karte.iOS.#{dep_module}"}']").add_attribute('version', ios_latest_versions[dep_module])
        doc_changed = true
      end
    end
  end

  File.open(nuspec_path, 'w') { |f| doc.write(f) } if doc_changed
end

# Update KarteXamarin version(iOS)
ios_new_version = ios_latest_versions['Core']
ios_new_version = increment_revision(ios_new_version) unless ios_core_version_changed

karte_xamarin_source_path = "../../karte-component/src/ios-unified/source/KarteXamarin/KarteXamarin/KarteXamarin.m"
karte_xamarin_source = File.read(karte_xamarin_source_path)
karte_xamarin_source.gsub!(/(return.*@")[0-9.]+"/, "\\1#{ios_new_version}\"")
File.open(karte_xamarin_source_path, 'w') { |f| f.write(karte_xamarin_source) }

# Update version of Android modules
android_latest_versions = get_android_latest_versions
android_core_version_changed = false
android_any_module_version_changed = false
ANDROID_MODULE_NAMES.each do |module_name|
  csproj_path = "../../Karte.Android/Karte.Android.#{module_name}/Karte.Android.#{module_name}.csproj"
  csproj = File.read(csproj_path)
  doc = REXML::Document.new(csproj)
  doc.context[:attribute_quote] = :quote

  current_version = REXML::XPath.first(doc, '//PackageVersion').text.strip
  if current_version != android_latest_versions[module_name]
    REXML::XPath.first(doc, '//PackageVersion').text = android_latest_versions[module_name]
    File.open(csproj_path, 'w') do |f|
      doc.write(f)
    end
    android_core_version_changed = true if module_name == 'Core'
    android_any_module_version_changed = true
  end
end

## Update KarteXamarin version(Android)
android_new_version = android_latest_versions['Core']
if !android_core_version_changed && android_any_module_version_changed
  android_new_version = increment_revision(android_new_version)
end

karte_xamarin_source_path = "../../Karte.Android/Karte.Android.Core/Additions/XamarinLibrary.cs"
karte_xamarin_source = File.read(karte_xamarin_source_path)
karte_xamarin_source.gsub!(/(Version.*=>.*")[0-9.]+"/, "\\1#{android_new_version}\"")
File.open(karte_xamarin_source_path, 'w') { |f| f.write(karte_xamarin_source) }
