require 'fileutils'
require 'open-uri'
require 'json'
require 'rexml/document'

script_dir_path = File.dirname($0)
tmp_dir_path = '../../tmp'
Dir.chdir(script_dir_path)

FileUtils.mkdir_p(tmp_dir_path)
Dir.chdir(tmp_dir_path)

artifact_ids = %w(Core InAppMessaging Notifications Variables)

artifact_ids.each do |artifact_id|
  library_detail = URI.open("https://search.maven.org/solrsearch/select?q=g:io.karte.android+AND+a:#{artifact_id.downcase}&wt=json").read
  latest_version = JSON.parse(library_detail)['response']['docs'][0]['latestVersion']
  file_name = "#{artifact_id.downcase}-#{latest_version}.aar"
  url = "https://search.maven.org/remotecontent?filepath=io/karte/android/#{artifact_id.downcase}/#{latest_version}/#{file_name}"
  URI.open(url) do |source|
    File.open(file_name, 'wb') do |dst|
      dst.write(source.read)
    end
  end

  jar_path = "../Karte.Android/Karte.Android.#{artifact_id}/Jars"
  FileUtils.mv(file_name, jar_path)

  csproj_path = "../Karte.Android/Karte.Android.#{artifact_id}/Karte.Android.#{artifact_id}.csproj"
  csproj = File.read(csproj_path)
  doc = REXML::Document.new(csproj)
  doc.context[:attribute_quote] = :quote
  REXML::XPath.first(doc, '//LibraryProjectZip').add_attribute('Include', "Jars\\#{file_name}")

  File.open(csproj_path, 'w') do |f|
    doc.write(f, transitive=false)
  end
end
