#!/usr/bin/env ruby

# File Name: nunit.rb
# Author : mmisztal1980@gmail.com
# Copyright 2014, Maciej Misztal
# All rights reserved 
# You are free to redistribute this file under Apache2 license
#
# Description : Contains the NUnit class, used to provide basic functionality
# for running tests with the NUnit framework

require 'build.system.rb'

unless defined? _NUNIT_
	_NUNIT_ = true
	
	module BuildSystem
		class NUnit
			# Runs all testAssemblies using the NUnit executable at the provided path
			def self.run(nunit_path, testAssemblies)
				testAssemblies.each { |testAssembly|			
					BuildSystem::invoke_runtime("#{nunit_path}", "-labels", "#{testAssembly}","/framework:#{CONFIG[:NUNIT_PLATFORM]}", "/trace=#{CONFIG[:NUNIT_TRACELEVEL]}")
				}
			end # run
		end
	end
end