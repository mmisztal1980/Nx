#!/usr/bin/env ruby

# File Name: Rakefile
# Author : mmisztal1980@gmail.com
# Copyright 2014, Maciej Misztal
# All rights reserved 
# You are free to redistribute this file under Apache2 license
#
# Description : The rake file for Nx's build system, 
# featuring automated versioning, testing and nuget packaging

$LOAD_PATH << './.build'
require 'build.system.rb'

include Rake::DSL
include BuildSystem

solution_file = "Nx.sln"

unit_tests = {
  :nx_core_unit_tests => "#{CONFIG[:SOLUTIONDIR]}/Tests/Nx.Core.UnitTests/.rake"
}

integration_tests = {
  :nx_core_integration_tests => "#{CONFIG[:SOLUTIONDIR]}/Tests/Nx.Core.IntegrationTests/.rake"
  #:nx_mongo_integration_tests => "#{CONFIG[:SOLUTIONDIR]}/Nx.Mongo.IntegrationTests/.rake"
}

packages = {
  :nx_core_package => "#{CONFIG[:SOLUTIONDIR]}/Nx.Core.Package/.rake"
}

## Default task 
task :default, [:build_config, :build_platform, :build_version] => [:build, :unit_tests]

## CI Tasks
task :incremental_build, [:build_config, :build_version, :build_platform] => [:clean, :build, :unit_tests]
task :daily_build, [:build_config, :build_version, :build_platform] => [:clean, :build, :integration_tests]

## Tasks
task :create_nuget_packages do
  packages.each { |namespace, rakefile| 
    load rakefile
    Rake::Task["#{namespace}:pack"].invoke(CONFIG[:CONFIGURATION], CONFIG[:NUNIT_PATH])
  }
end # create_nuget_packages

# Loads pre-defined integration-test rake files and executes the :run task 
task :integration_tests, [:build_config, :build_platform, :build_version] => :unit_tests do |t, args|
  integration_tests.each { |namespace, rakefile| 
    load rakefile
    Rake::Task["#{namespace}:run"].invoke(CONFIG[:CONFIGURATION], CONFIG[:NUNIT_PATH])
  }
end # integration_tests

# Loads pre-defined unit-test rake files and executes the :run task
task :unit_tests, [:build_config, :build_platform, :build_version] => :build do |t, args|
  unit_tests.each { |namespace, rakefile| 
    load rakefile
    Rake::Task["#{namespace}:run"].invoke(CONFIG[:CONFIGURATION], CONFIG[:NUNIT_PATH])
  } 
end # unit_tests

# Ensures all git submodules and nuget packages are present,
# launches an apropriate build runner (platform-dependent)
task :build, [:build_config, :build_platform, :build_version] => [:ensure_nuget_packages, :assemblyinfo] do |b, args|
  BuildSystem::Git.ensure_submodules()
  BuildSystem::NuGet.ensure_nuget_packages()
  Rake::Task[BuildSystem::is_running_on_nix() ? "build_xbuild" : "build_msbuild"].invoke
end # build

# Executes the build using the MSBuild runner (Windows)
msbuild :build_msbuild do |b|
  config = CONFIG.key?(:CONFIGURATION) ? CONFIG[:CONFIGURATION] : CONFIG[:DEFAULT_CONFIGURATION]
  platform = CONFIG.key?(:PLATFORM) ? CONFIG[:PLATFORM] : CONFIG[:DEFAULT_PLATFORM]

  b.properties :configuration => config, :platform => platform #, "OutputPath" => OUTPUT_DIR
  b.targets :Build
  b.solution = solution_file
end # build_msbuild

# Executes the build using the XBuild runner (Mono)
xbuild :build_xbuild do |b|
  config = CONFIG.key?(:CONFIGURATION) ? CONFIG[:CONFIGURATION] : CONFIG[:DEFAULT_CONFIGURATION]
  platform = CONFIG.key?(:PLATFORM) ? CONFIG[:PLATFORM] : CONFIG[:DEFAULT_PLATFORM]

  b.properties :configuration => config, :platform => platform #, "OutputPath" => OUTPUT_DIR
  b.targets :Build
  b.solution = solution_file
end # build_xbuild

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
end # assemblyinfo

# Configures the build environment
task :configure , [:build_config, :build_platform, :build_version] do |t, args|
  CONFIG[:CONFIGURATION] = args.build_config != nil ? args.build_config : CONFIG[:DEFAULT_CONFIGURATION]
  CONFIG[:PLATFORM] = args.build_platform != nil ? args.build_platform : CONFIG[:DEFAULT_PLATFORM]
  CONFIG[:VERSION] = args.build_version != nil ? args.build_version : CONFIG[:DEFAULT_VERSION]

  puts "Build configuration : #{CONFIG[:CONFIGURATION]}, #{CONFIG[:PLATFORM]}, Version : #{CONFIG[:VERSION]}"
end # configure

# Cleans all bin and obj folders, excluding the ones found in NuGet package folders
task :clean , [:build_config, :build_platform, :build_version] => :configure do |b, args|
  files = FileList.new("#{CONFIG[:SOLUTIONDIR]}/**/bin", "#{CONFIG[:SOLUTIONDIR]}/**/obj") do |f|
    f.exclude(/packages/)
  end

  puts "Cleaning solution :"
  files.each { |dir|
    puts dir
    FileUtils.rm_rf(dir) 
  }
end # clean

# Ensures all packages are present, regardless of the platform
task :ensure_nuget_packages do
  puts "Ensuring all NuGet packages are present"
  BuildSystem::NuGet.ensure_nuget_packages()
end # ensure_nuget_packages