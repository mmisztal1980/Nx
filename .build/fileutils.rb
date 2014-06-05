#!/usr/bin/env ruby

# File Name: git.rb
# Author : mmisztal1980@gmail.com
# Copyright 2014, Maciej Misztal
# All rights reserved 
# You are free to redistribute this file under Apache2 license
#
# Description : Contains the NuGet class, used to provide basic functionality
# for interacting with the NuGet package management system

unless defined? _BUILD_SYSTEM_GIT_
	_BUILD_SYSTEM_GIT_ = true

require 'build.system.rb'

	module BuildSystem
		class Utils
			def self.rename_file(oldname, newname)
  				# Ruby 1.8.7 on Mac sometimes reports File.exist? incorrectly
  				# so work around this [not sure why that happens]
  				begin
    				File.delete(newname)
  				rescue => msg
    			# File probably doesn't exist, if it does File.size? will work properly
    			if File.size?(newname) != nil then
      				fail "Failed to delete old file #{newname} with: #{msg}"
      				raise
    			end
  				end
  				File.rename(oldname, newname)
			end

			def self.ensure_dir_exists(dir_path)
  				unless File.directory? dir_path
    				puts "Creating folder : #{dir_path}"
    				Dir.mkdir(dir_path)
  				else
    				puts "#{dir_path} already exists"
  				end
			end
		end
	end
end