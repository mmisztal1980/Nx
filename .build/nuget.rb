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
require 'utils.rb'

	module BuildSystem
		class NuGet

			# Updates all submodules within the current working 
		public
      # Reads all NuGet packages
			def self.read_nuget_packages()
  				FileList["#{CONFIG[:SOLUTIONDIR]}/**/packages.config"].each { |config_file|
    				read_package_config(config_file) { |pkg|
      					name = pkg.attributes["id"]
      					version = pkg.attributes["version"]
      					
                PACKAGES[name]={}
      					PACKAGES[name][:name]=name
      					PACKAGES[name][:version]=version
      					PACKAGES[name][:folder]="#{CONFIG[:PACKAGESDIR]}/#{name}.#{version}"
      					PACKAGES[name][:filename]="#{CONFIG[:PACKAGESDIR]}/#{name}.#{version}.nupkg"
      					PACKAGES[name][:url]="http://packages.nuget.org/api/v1/package/#{name}/#{version}"
    				}
  				}

          PACKAGES.keys.each {|id|
            puts "Read package #{PACKAGES[id][:name]} with version #{PACKAGES[id][:version]}"
          }
			end # read_nuget_packages

			def self.ensure_nuget_packages()
  				Dir.mkdir CONFIG[:PACKAGESDIR] unless Dir.exists? CONFIG[:PACKAGESDIR]
  				read_nuget_packages
  				if all_packages_present?() then
    				puts "All packages up to date"
    				print_nuget_package_manifest
    				return
  				end
    
  				if (BuildSystem::is_running_on_nix()) then
    				puts "updating packages with internal nuget replacement"
    				ensure_all_nuget_packages_nix
    				print_nuget_package_manifest
  				else
    				puts "updating packages with nuget"
    				FileList["#{CONFIG[:SOLUTIONDIR]}**/packages.config"].each { |config_file|
      					puts "updating packages for #{config_file}"
      					sh invoke_runtime("#{NUGET_PATH} install #{config_file} -o #{CONFIG[:PACKAGESDIR]}")
    				}
    
    				print_nuget_package_manifest
  				end
			end # ensure_nuget_packages

      # Checks if all NuGet packages are present
      def self.all_packages_present?()
        result = true
        PACKAGES.values.each { |pkg|        
          if !Dir.exists? pkg[:folder] then
            puts "Package missing: #{pkg[:name]}"
            result = false
          end
        }

        puts "All packages present" unless !result
        return result
      end # all_packages_present?

		private 
			def self.read_package_config(filename)
  				input_file = File.new(filename)
  				xml = REXML::Document.new input_file
  
  				xml.elements.each("packages/package") { |element|
    				yield element
  				}
  
  				input_file.close
			end # read_package_config

      # Prints the package manifest
      def self.print_nuget_package_manifest()
        puts "Building with NuGet packages:"
        PACKAGES.values.each { |pkg|
          puts "#{pkg[:name]} => #{pkg[:version]}"
        }
      end # print_nuget_package_manifest

      def self.ensure_all_nuget_packages_nix()
        PACKAGES.values.each { |pkg|
          ensure_nuget_package_nix(pkg[:name])
        }
      end # ensure_all_nuget_packages_nix

      # NuGet doesn't work on Mono. So we're going to download our dependencies from NuGet.org.      
      def self.ensure_nuget_package_nix(name)
        zip_file = PACKAGES[name][:filename]
        tmp_file = "#{zip_file}.tmp"
  
        if File.exist?(zip_file) and File.size?(zip_file) != nil then
          puts "#{zip_file} already exists, skipping download"
          BuildSystem::Utils::unzip_file(zip_file, PACKAGES[name][:folder])
          return
        end
  
        puts "fetching #{zip_file}"
        File.open(tmp_file, "w") { |f|
          uri = URI.parse(PACKAGES[name][:url])

          BuildSystem::Utils::fetch(uri) do |seg|
            f.write(seg)
          end
        }
                  
        if File.size?(tmp_file) == nil then
          fail "Download failed for #{zip_file}"
        end
  
        BuildSystem::Utils::rename_file(tmp_file, zip_file)
        BuildSystem::Utils::unzip_file(zip_file, PACKAGES[name][:folder])
      end # ensure_nuget_package_nix
		end
	end
end