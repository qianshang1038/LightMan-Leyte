set WORKSPACE=..\..
set LUBAN_DLL=%WORKSPACE%\Config\LubanConfig\Luban\Luban.dll
set CONF_ROOT=.

dotnet %LUBAN_DLL% ^
    -t all ^
    -d json ^
    -c cs-simple-json ^
    --conf %CONF_ROOT%\luban.conf ^
    -x outputDataDir=%WORKSPACE%\Assets\Data\Config ^
    -x outputCodeDir=%WORKSPACE%\Assets\Scripts\Config

pause