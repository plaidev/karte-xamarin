require 'fileutils'
require 'open-uri'
require 'json'
require 'rexml/document'

require_relative './utils'

def increment_revision(version)
  versions = version.split('.')
  if versions.size == 3
    versions << '1'
  elsif versions.size == 4
    versions[3] = (versions[3].to_i + 1).to_s
  end
  versions.join('.')
end

def increment_minor_version(version)
  versions = version.split('.')
  versions[1] = (versions[1].to_i + 1).to_s
  versions.join('.')
end

def load_ios_nuspec(module_name)
  nuspec = File.read("../../karte-component/nuget/Karte.iOS.#{module_name}.nuspec")
  doc = REXML::Document.new(nuspec)
end

def write_ios_nuspec(doc, module_name)
  File.open("../../karte-component/nuget/Karte.iOS.#{module_name}.nuspec", 'w') { |f| doc.write(f) }
end

def update_version_in_nuspec(nuspec_doc)
  current_version = REXML::XPath.first(nuspec_doc, '//version').text.strip
  new_version = yield(current_version)
  REXML::XPath.first(nuspec_doc, '//version').text = new_version
end

script_dir_path = File.dirname($0)
Dir.chdir(script_dir_path)

version_diff = get_version_diff

ios_core_new_version = ''

# 依存しているiOS SDKがアップデートされているモジュールのバージョンを更新する
version_diff[:ios].each do |module_name, _|
  doc = load_ios_nuspec(module_name)

  new_version = ''
  update_version_in_nuspec(doc) do |current_version|
    new_version = increment_minor_version(current_version)
  end

  ios_core_new_version = new_version if module_name == :Core

  write_ios_nuspec(doc, module_name)
end

# Coreは依存SDKがアップデートされていなくてもrevisionをあげる
if version_diff[:ios][:Core].nil?
  doc = load_ios_nuspec('Core')

  new_version = ''
  update_version_in_nuspec(doc) do |current_version|
    new_version = increment_revision(current_version)
  end

  ios_core_new_version = new_version
  write_ios_nuspec(doc, 'Core')
end

# Update KarteXamarin version(iOS)
karte_xamarin_source_path = "../../karte-component/src/ios-unified/source/KarteXamarin/KarteXamarin/KarteXamarin.m"
karte_xamarin_source = File.read(karte_xamarin_source_path)
karte_xamarin_source.gsub!(/(return.*@")[0-9.]+"/, "\\1#{ios_core_new_version}\"")
File.open(karte_xamarin_source_path, 'w') { |f| f.write(karte_xamarin_source) }

# Update version of Android modules
android_core_new_version = ''
version_diff[:android].each do |module_name, _|
  csproj_path = "../../Karte.Android/Karte.Android.#{module_name}/Karte.Android.#{module_name}.csproj"
  csproj = File.read(csproj_path)
  doc = REXML::Document.new(csproj)
  doc.context[:attribute_quote] = :quote

  current_version = REXML::XPath.first(doc, '//PackageVersion').text.strip
  new_version = increment_minor_version(current_version)
  REXML::XPath.first(doc, '//PackageVersion').text = new_version 
  File.open(csproj_path, 'w') do |f|
    doc.write(f)
  end

  android_core_new_version = new_version if module_name == :Core
end

# Coreは依存SDKがアップデートされていなくてもrevisionをあげる
if version_diff[:android][:Core].nil?
  csproj_path = "../../Karte.Android/Karte.Android.Core/Karte.Android.Core.csproj"
  csproj = File.read(csproj_path)
  doc = REXML::Document.new(csproj)
  doc.context[:attribute_quote] = :quote

  current_version = REXML::XPath.first(doc, '//PackageVersion').text.strip
  new_version = increment_revision(current_version)
  android_core_new_version = new_version
  REXML::XPath.first(doc, '//PackageVersion').text = new_version 
  File.open(csproj_path, 'w') do |f|
    doc.write(f)
  end
end

# Update KarteXamarin version(Android)
karte_xamarin_source_path = "../../Karte.Android/Karte.Android.Core/Additions/XamarinLibrary.cs"
karte_xamarin_source = File.read(karte_xamarin_source_path)
karte_xamarin_source.gsub!(/(Version.*=>.*")[0-9.]+"/, "\\1#{android_core_new_version}\"")
File.open(karte_xamarin_source_path, 'w') { |f| f.write(karte_xamarin_source) }

# 現在のSDKバージョンを保存する
update_sdk_version_file