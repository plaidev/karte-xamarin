require 'fileutils'
require 'open-uri'
require 'json'
require 'rexml/document'

require './utils'

script_dir_path = File.dirname($0)
tmp_dir_path = '../../tmp'
Dir.chdir(script_dir_path)

FileUtils.mkdir_p(tmp_dir_path)
Dir.chdir(tmp_dir_path)

latest_versions = get_android_latest_versions

MODULE_NAMES.each do |module_name|
  latest_version = latest_versions[module_name]
  file_name = "#{module_name.downcase}-#{latest_version}.aar"
  url = "https://search.maven.org/remotecontent?filepath=io/karte/android/#{module_name.downcase}/#{latest_version}/#{file_name}"
  URI.open(url) do |source|
    File.open(file_name, 'wb') do |dst|
      dst.write(source.read)
    end
  end

  jar_path = "../Karte.Android/Karte.Android.#{module_name}/Jars"
  FileUtils.mv(file_name, jar_path)

  csproj_path = "../Karte.Android/Karte.Android.#{module_name}/Karte.Android.#{module_name}.csproj"
  csproj = File.read(csproj_path)
  doc = REXML::Document.new(csproj)
  doc.context[:attribute_quote] = :quote
  REXML::XPath.first(doc, '//LibraryProjectZip').add_attribute('Include', "Jars\\#{file_name}")

  File.open(csproj_path, 'w') do |f|
    doc.write(f)
  end
end
