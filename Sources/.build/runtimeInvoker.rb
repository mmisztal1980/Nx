#!/usr/bin/env ruby

module BuildSystem
	class RuntimeInvoker
		def initialize()
		end

		protected
			# Returns true if running on a UNIX based platform
			def is_nix()
				!RUBY_PLATFORM.match("linux|darwin").nil?
			end
			# Invokes the command, taking the current platform into account
			def invoke_runtime(cmd)
  				command = cmd
  				if is_nix()
    				command = "mono --runtime=v4.0 #{cmd}"
  				end
  				command
			end
	end
end