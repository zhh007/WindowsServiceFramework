@echo off
echo Uninstalling service, please wait....
SelfInstallingWindowsService.exe /uninstall
echo Cleaning up files...
attrib *.log +r +s +h > NUL
attrib *Admin.exe +r +s +h > NUL
attrib *.config +r +s +h > NUL
attrib *.dll +r +s +h > NUL
attrib *.xml +r +s +h > NUL
attrib uninstallService.bat +r +s +h > NUL
del /s/q *.*
attrib *.log -r -s -h > NUL
attrib *Admin.exe -r -s -h > NUL
attrib *.config -r -s -h > NUL
attrib *.dll -r -s -h > NUL
attrib *.xml -r -s -h > NUL
attrib uninstallService.bat -r -s -h > NUL
del /s/q uninstallService.bat