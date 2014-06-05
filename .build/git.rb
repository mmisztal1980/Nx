#!/usr/bin/env ruby

# File Name: git.rb
# Author : mmisztal1980@gmail.com
# Copyright 2014, Maciej Misztal
# All rights reserved 
# You are free to redistribute this file under Apache2 license
#
# Description : Contains the Git class, used to provide basic functionality
# for interacting with the git-scm

unless defined? _BUILD_SYSTEM_GIT_
	_BUILD_SYSTEM_GIT_ = true

	module BuildSystem
		class Git
			# Updates all submodules within the current working directory
			def self.ensure_submodules()
				system("git submodule init")
  				system("git submodule update")
			end
		end
	end
end