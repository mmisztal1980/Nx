projectDir = "#{File.dirname(__FILE__)}"
require 'build.system.rb'
require 'nunit.rb'

namespace :nx_core_package do
	# Executes a set of tests
	task :run, [:build_config, :nuget_path] => :copy_binaries do |t, args|
		#test_assemblies = [
		#	"#{projectDir}/bin/#{args.build_config}/Nx.Core.UnitTests.dll"
		#]

		#BuildSystem::NUnit.run(args.nunit_path, test_assemblies)
	end

	tak :copy_binaries[:build_config] => :init do |t, args|
	end

	# Validates the build_config and nunit_path parameters sent to the .rake file
	task :init, [:build_config, :nuget_path] do |t, args|
		raise "Unknown build_config" unless !args.build_config.to_s.empty?
		raise "NuGet path missing" unless !args.nuget_path.to_s.empty?
	end
end