#Monkey patch Dir.exists? for Ruby 1.8.x



def is_nix
  !RUBY_PLATFORM.match("linux|darwin").nil?
end

def invoke_runtime(cmd)
  command = cmd
  if is_nix()
    command = "mono --runtime=v4.0 #{cmd}"
  end
  command
end

def transform_xml(input, output)
  input_file = File.new(input)
  xml = REXML::Document.new input_file
  
  yield xml
  
  input_file.close
  
  output_file = File.open(output, "w")
  formatter = REXML::Formatters::Default.new()
  formatter.write(xml, output_file)
  output_file.close
end
        
def ensure_submodules()
  system("git submodule init")
  system("git submodule update")
end

def fetch(uri, limit = 10, &block)
  # We should choose a better exception.
  raise ArgumentError, 'too many HTTP redirects' if limit == 0

  http = Net::HTTP.new(uri.host, uri.port)
  if uri.scheme == "https"
    http.verify_mode = OpenSSL::SSL::VERIFY_PEER
    http.use_ssl = true
  end
  
  resp = http.request(Net::HTTP::Get.new(uri.request_uri)) { |response|
    case response
    when Net::HTTPRedirection then
      location = response['location']
      if block_given? then
        fetch(URI(location), limit - 1, &block)
      else
        fetch(URI(location), limit - 1)
      end
      return
    else
      response.read_body do |segment|
        yield segment
      end
      return
    end
  }
end

def rename_file(oldname, newname)
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

def ensure_dir_exists(dir_path)
  unless File.directory? dir_path
    puts "Creating folder : #{dir_path}"
    Dir.mkdir(dir_path)
  else
    puts "#{dir_path} already exists"
  end
end