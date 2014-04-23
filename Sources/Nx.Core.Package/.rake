PROJECTDIR = "#{File.dirname(__FILE__)}"

require 'build.system.rb'
require 'nuget.rb'

files = {	
	"#{PROJECTDIR}/../Nx.Core/bin/Release/Nx.Core.dll" => "#{PROJECTDIR}/lib",
	"#{PROJECTDIR}/../Nx.Core/bin/Release/Nx.Core.pdb" => "#{PROJECTDIR}/lib",
}

namespace :nx_core_package do
	task :init => :clean do
		puts "init called"

		#ensure_dir_exists("#{PROJECTDIR}/lib")
		#ensure_dir_exists("#{PROJECTDIR}/tools")
		#ensure_dir_exists("#{PROJECTDIR}/src")
	end

	# Cleans all target directories before copying files
	task :clean do
		files.values.uniq.each { |path| 
			puts "Cleaning #{path}"
			FileUtils.rm_rf Dir.glob("#{path}/*")
		}
	end

	task :pack => [:init, :clean]do
		create_nuget_package("#{PROJECTDIR}/Package.nuspec", "#{PROJECTDIR}")
	end

	task :deploy => do
	end
end