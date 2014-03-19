require 'runtimeInvoker.rb'

module BuildSystem
	class NUnit < RuntimeInvoker
		def initialize(nunit_path, test_assemblies)
			@nunitPath = nunit_path
			@testAssemblies = test_assemblies
		end

		def run
			@testAssemblies.each { |testAssembly|
				puts "attempting to run : #{testAssembly}"
				nunit = invoke_runtime(@nunitPath)
				sh "#{nunit} -labels #{testAssembly} /framework:net-4.5 /trace=Info"
			}
		end
	end
end