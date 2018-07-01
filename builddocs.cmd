REM See https://github.com/dotnet/docfx/issues/1752#issuecomment-308909959
set MSBUILD_EXE_PATH=C:\Program Files\dotnet\sdk\2.0.0\MSBuild.dll
copy doc\MSBuild.dll.config "C:\Program Files\dotnet\sdk\2.0.0\MSBuild.dll.config" /Y
docfx doc\docfx.json --logLevel Warning
