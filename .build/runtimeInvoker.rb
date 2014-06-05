#!/usr/bin/env ruby

module BuildSystem
	class RuntimeInvoker
		def initialize()
		end

		protected
			# Invokes the command, taking the current platform into account
			def invoke_runtime(cmd)
  				command = cmd
  				if BuildSystem::is_running_on_nix()
    				command = "mono --runtime=v4.0 #{cmd}"
  				end
  				command
			end
	end
end