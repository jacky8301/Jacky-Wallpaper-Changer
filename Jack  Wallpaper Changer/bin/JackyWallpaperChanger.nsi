; NSIS Script to package the directory
!define VERSION "1.0.0"
OutFile "Jacky-Wallpaper-Changer-Setup_${VERSION}.exe"
InstallDir "$APPDATA\Jacky Wallpaper Changer"

Section "Install"
    ExecWait "taskkill /F /T /IM JackyWallpaperChanger.exe"
    Delete "$INSTDIR\*.dll"
    Delete "$INSTDIR\*.exe"
    Delete "$INSTDIR\logo.ico"
    Delete "$INSTDIR\app.manifest"
    DeleteRegValue HKLM32 "Software\Microsoft\Windows\CurrentVersion\Run" "JackyWallpaperChanger"
    SetOutPath "$INSTDIR"
    File /r "Release\*.dll"
    File /r "Release\*.exe"
    File "Release\logo.ico"
    WriteRegStr HKLM32 "Software\Microsoft\Windows\CurrentVersion\Run" "JackyWallpaperChanger" "$INSTDIR\JackyWallpaperChanger.exe"
    Exec "$INSTDIR\JackyWallpaperChanger.exe"
SectionEnd