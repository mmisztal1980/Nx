# File Name: build.env.rb
# Author : mmisztal1980@gmail.com
# Copyright (C) 2013 - 2014, Maciej Misztal
# All rights reserved
#
# Description : The build system file containing the :
# - CONFIG hash
# - PACKAGES hash

require 'albacore'
require 'uri'
require 'net/http'
require 'net/https'
require 'rake'

require 'build.tools.rb'
require 'build.nuget.rb'

unless defined? _BUILD_SYSTEM_
	_BUILD_SYSTEM_ = true

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

	solutionDir = File.expand_path("..", File.dirname(__FILE__))
	packagesDir = "#{solutionDir}/packages"

	CONFIG = {
   			:SOLUTIONDIR 			      => solutionDir,
   			:PACKAGESDIR 			      => packagesDir,
  			:NUGET_PATH 			      => "#{solutionDir}/.nuget/NuGet.exe",
   			:NUNIT_PATH 			      => "#{packagesDir}/NUnit.Runners.2.6.3/tools/nunit-console.exe",
   			:DEFAULT_CONFIGURATION 	=> "Debug",
   			:DEFAULT_PLATFORM 		  => "Any CPU",
   			:DEFAULT_VERSION 		    => "0.0.0.1",
   			:DEFAULT_FILE_VERSION	  => "1.0.0.0",
   			:COPYRIGHT 				      => "Copyright (C) 2014 Maciej Misztal",
  	} unless defined? CONFIG

  PACKAGES = {} unless defined? PACKAGES
end