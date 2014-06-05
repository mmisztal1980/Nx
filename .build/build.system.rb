# File Name: build.system.rb
# Author : mmisztal1980@gmail.com
# Copyright (C) 2013 - 2014, Maciej Misztal
# All rights reserved
#
# Description : The build system file containing the :
# - CONFIG hash
# - PACKAGES hash
# - platform detection & platform-dependent runtime invocation functions

require 'albacore'
require 'uri'
require 'net/http'
require 'net/https'
require 'rake'

require 'git.rb'
require 'nuget.rb'
require 'nunit.rb'

unless defined? _BUILD_SYSTEM_
  _BUILD_SYSTEM_ = true
  
  module BuildSystem
    solutionDir = File.expand_path("..", File.dirname(__FILE__))
    packagesDir = "#{solutionDir}/packages"

    CONFIG = {
        :SOLUTIONDIR            => solutionDir,
        :PACKAGESDIR            => packagesDir,
        :DEFAULT_CONFIGURATION  => "Debug",
        :DEFAULT_PLATFORM       => "Any CPU",
        :DEFAULT_VERSION        => "0.0.0.1",
        :DEFAULT_FILE_VERSION   => "1.0.0.0",
        :COPYRIGHT              => "Copyright (C) 2014 Maciej Misztal",
        :NUGET_PATH             => "#{solutionDir}/.nuget/NuGet.exe",
        :NUNIT_PATH             => "#{packagesDir}/NUnit.Runners.2.6.3/tools/nunit-console.exe",
        :NUNIT_PLATFORM         => "net-4.5",
        :NUNIT_TRACELEVEL       => "Info",
     } unless defined? CONFIG

    PACKAGES = {} unless defined? PACKAGES
    
    if RUBY_VERSION =~ /^1\.8/
      class Dir
        class << self
          def exists? (path)
            File.directory?(path)
          end
          alias_method :exist?, :exists?
        end
      end
    end

    # Returns true if running on a UNIX based platform
    def self.is_running_on_nix()
      !RUBY_PLATFORM.match("linux|darwin").nil?
    end # is_running_on_nix

    # Passes all arguments to the system method, optionally including the mono runtime
    def self.invoke_runtime(*cmd)
      puts "Executing #{cmd}"
      if BuildSystem::is_running_on_nix()
        self.class.send(:system, "mono --runtime=v4.0", *cmd)
      else
        self.class.send(:system, *cmd)
      end     
    end # invoke_runtime
  end
end