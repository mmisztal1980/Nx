#!/usr/bin/env ruby

# File Name: Rakefile
# Author : mmisztal1980@gmail.com
# Copyright 2014, Maciej Misztal
# All rights reserved - Do Not Redistribute
#
# Description : The rake file for Nx's build system, 
# featuring automated versioning, testing and nuget packaging

$LOAD_PATH << './.build'
include Rake::DSL
require 'build.system.rb'

solution_file = "Nx.sln"

unit_tests = {
  :nx_core_tests => "#{CONFIG[:SOLUTIONDIR]}/Nx.Core.Tests/.rake"
}

integration_tests = {
  :nx_mongo_integration_tests => "#{CONFIG[:SOLUTIONDIR]}/Nx.Mongo.IntegrationTests/.rake"
}

packages = {
  :nx_core_package => "#{CONFIG[:SOLUTIONDIR]}/Nx.Core.Package/.rake"
}

task :default, [:build_config, :build_platform, :build_version] => [:build, :unit_tests]

task :incremental_build, [:build_config, :build_version] => [:clean, :build, :unit_tests]

task :integration_tests, [:build_config, :build_platform, :build_version] => :unit_tests do |t, args|
  integration_tests.each { |namespace, rakefile| 
    load rakefile
    Rake::Task["#{namespace}:run"].invoke(CONFIG[:CONFIGURATION], CONFIG[:NUNIT_PATH])
  }
end

task :unit_tests, [:build_config, :build_platform, :build_version] => :build do |t, args|
  unit_tests.each { |namespace, rakefile| 
    load rakefile
    Rake::Task["#{namespace}:run"].invoke(CONFIG[:CONFIGURATION], CONFIG[:NUNIT_PATH])
  } 
end

task :build, [:build_config, :build_platform, :build_version] => :assemblyinfo do |b, args|
  ensure_submodules()
  ensure_nuget_packages()
  build_task = is_nix() ? "build_xbuild" : "build_msbuild"
  Rake::Task[build_task].invoke
end

# Executes the build using the MSBuild runner (Windows)
msbuild :build_msbuild do |b|
  config = CONFIG.key?(:CONFIGURATION) ? CONFIG[:CONFIGURATION] : CONFIG[:DEFAULT_CONFIGURATION]
  platform = CONFIG.key?(:PLATFORM) ? CONFIG[:PLATFORM] : CONFIG[:DEFAULT_PLATFORM]

  b.properties :configuration => config, :platform => platform #, "OutputPath" => OUTPUT_DIR
  b.targets :Build
  b.solution = solution_file
end

# Executes the build using the XBuild runner (Mono)
xbuild :build_xbuild do |b|
  config = CONFIG.key?(:CONFIGURATION) ? CONFIG[:CONFIGURATION] : CONFIG[:DEFAULT_CONFIGURATION]
  platform = CONFIG.key?(:PLATFORM) ? CONFIG[:PLATFORM] : CONFIG[:DEFAULT_PLATFORM]

  b.properties :configuration => config, :platform => platform #, "OutputPath" => OUTPUT_DIR
  b.targets :Build
  b.solution = solution_file
end


# Patches all AssemblyInfo.cs files' version
assemblyinfo :assemblyinfo , [:build_config, :build_platform, :build_version] => :configure do |t, args|   
  assemblies = FileList.new("./**/AssemblyInfo.cs") do |f|
    f.exclude(/packages/)
  end

  assemblies.each { |assemblyInfoFile| 
    puts "#{File.expand_path(assemblyInfoFile)}"

    t.version = CONFIG.key?(:VERSION) ? CONFIG[:VERSION] : CONFIG[:DEFAULT_VERSION]
    t.file_version = CONFIG[:DEFAULT_FILE_VERSION]
    t.copyright = CONFIG[:COPYRIGHT]
    t.output_file = t.input_file = assemblyInfoFile
    t.execute
  }
end

# Configures the build environment
task :configure , [:build_config, :build_platform, :build_version] do |t, args|
  CONFIG[:CONFIGURATION] = args.build_config != nil ? args.build_config : CONFIG[:DEFAULT_CONFIGURATION]
  CONFIG[:PLATFORM] = args.build_platofrm != nil ? args.build_platform : CONFIG[:DEFAULT_PLATFORM]
  CONFIG[:VERSION] = args.build_version != nil ? args.build_version : CONFIG[:DEFAULT_VERSION]

  puts "Build configuration : #{CONFIG[:CONFIGURATION]}, #{CONFIG[:PLATFORM]}, Version : #{CONFIG[:VERSION]}"
end

# Cleans all bin and obj folders, excluding the ones found in NuGet package folders
task :clean , [:build_config, :build_platform, :build_version] => :configure do |b, args|
  files = FileList.new("#{CONFIG[:SOLUTIONDIR]}/**/bin", "#{CONFIG[:SOLUTIONDIR]}/**/obj") do |f|
    f.exclude(/packages/)
  end

  files.each { |dir|
    puts dir
    FileUtils.rm_rf(dir) 
  }
end