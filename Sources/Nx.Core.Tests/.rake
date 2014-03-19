projectDir = "#{File.dirname(__FILE__)}"
require 'build.system.rb'
require 'nunit.rb'

namespace :nx_core_tests do
	# Executes a set of tests
	task :run, [:build_config, :nunit_path] => :init do |t, args|
		test_assemblies = [
			"#{projectDir}/bin/#{args.build_config}/Nx.Core.Tests.dll"
		]

		nunit = BuildSystem::NUnit.new(args.nunit_path, test_assemblies)
		nunit.run
	end

	# Validates the build_config and nunit_path parameters sent to the .rake file
	task :init, [:build_config, :nunit_path] do |t, args|
		raise "Unknown build_config" unless !args.build_config.to_s.empty?
		raise "NUnit path missing" unless !args.build_config.to_s.empty?
	end
end