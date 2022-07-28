require 'fileutils'

script_dir_path = File.dirname($0)
Dir.chdir(script_dir_path)

podspec_path = "../../karte-ios-sdk/KarteCore.podspec"
latest_core_version = ''
File.open(podspec_path, 'r') do |f|
  while line = f.readline
    # s.version = '2.19.0' のような行からバージョンを取り出す
    if line =~ /s\.version/
      latest_core_version = line.split('=')[1].strip[1..-2]
      break
    end
  end
end

if latest_core_version == ''
  puts 'Could not find KarteCore version in karte-ios-sdk/KarteCore.podspec'
  exit 1
end

karte_xamarin_podfile_path = "../../karte-component/src/ios-unified/source/KarteXamarin/Podfile"

karte_xamarin_core_version = ''
replace_line = ''

File.open(karte_xamarin_podfile_path, 'r') do |f|
  while line = f.readline
    # pod 'KarteCore', '2.13.0' のような行からバージョンを取り出す
    if line =~ /pod(\s)+'KarteCore'(.)*'[0-9.]+'/
      karte_xamarin_core_version = line.split(',')[1].strip[1..-2]
      break
    end
  end
end

if karte_xamarin_core_version == ''
  puts 'Could not find KarteCore version in karte-component/src/ios-unified/source/KarteXamarin/Podfile'
  exit 1
end

podfile = ""
if karte_xamarin_core_version != latest_core_version
  File.open(karte_xamarin_podfile_path, 'r') { |f| podfile = f.read }
  podfile.gsub!(/pod(\s)+'KarteCore'(.)*'[0-9.]+'/, "pod 'KarteCore', '#{latest_core_version}'")
  File.open(karte_xamarin_podfile_path, 'w') { |f| f.write(podfile) }
end

puts "Finished updating Podfile"