@echo off

:: Set repository info
set key=%NuGetApiKey%
set url=%NuGetGalleryUrl%

:: Make sure the nuget executable is writable
echo $(ProjectDir)
attrib -R %~dp0/NuGet.exe

:: Make sure the nupkg files are writeable and create backup
IF EXIST *.nupkg (
	echo.
	echo Creating backup...
	forfiles /m *.nupkg /c "cmd /c attrib -R @File"
	forfiles /m *.nupkg /c "cmd /c move /Y @File @File.bak"
)

echo.
echo Updating NuGet...
cmd /c nuget.exe update -Self

echo.
echo Creating package...
nuget.exe pack Package.nuspec -Verbose -Version %1

:: Check if package should be published
IF /I "%2"=="Publish" goto :publish
goto :eof

:publish
IF EXIST *.nupkg (
	echo.
	echo Publishing package...
	echo API Key: %key%
	echo NuGet Url: %url%
	forfiles /m *.nupkg /c "cmd /c %~dp0/nuget.exe push @File %key%
	goto :eof
)

:eof